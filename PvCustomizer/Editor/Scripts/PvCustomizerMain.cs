using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Akaal.Editor.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Akaal.Editor
{
    public class PvCustomizerMain
    {
        private class DrawerRegistry : IDrawerRegistry
        {
            private readonly List<IDrawer> _drawers = new List<IDrawer>();

            #region Implementation of IIconDrawerRegistry

            public IDrawer GetIconDrawer(Type type)
            {
                int maxPriority = Int32.MinValue;
                int index       = -1;
                for (var i = 0; i < _drawers.Count; i++)
                {
                    var drawer = _drawers[i];
                    if (drawer.ValidForType(type) && drawer.Priority > maxPriority)
                    {
                        index       = i;
                        maxPriority = drawer.Priority;
                    }
                }

                if (index > -1)
                {
                    return _drawers[index];
                }
                else
                {
                    return null;
                }
            }

            public void RegisterIconDrawer(IDrawer drawer)
            {
                _drawers.Add(drawer);
            }

            public bool UnregisterIconDrawer(IDrawer drawer)
            {
                return _drawers.Remove(drawer);
            }

            #endregion
        }

        public readonly IDrawerRegistry Registry = new DrawerRegistry();

        /// <summary>
        /// Here's how the drawer checks what avenue to take:
        /// 1. If valid folder and folder has assigned icon, draw replacement icon and exit.
        /// 2. Else if asset successfully loads, try to draw it by passing any PvIconAttributes through registry.
        /// 3. Else check if asset path matches regex, and draw replacement if it does.
        /// 4. Else check if asset extension matches one of the registered extns, and draw replacement if it does
        /// 5. Failed to find a good substitute icon. Leave original icon intact.
        /// </summary>
        public void DrawProjectIcon(string guid, Rect selectionrect)
        {
            /*
                        ValidateProjectBrowser();
                        bool didClip = false;
                        if (_getProjectBrowserRect != null)
                        {
                            Rect projectBrowserRect = _getProjectBrowserRect();
                            Rect clippingRect = new Rect(Vector2.down * (18f), projectBrowserRect.size - new Vector2(0f, 36f));
                            /EditorGUI.DrawRect(clippingRect, Color.green);
                            GUI.BeginClip(clippingRect);
                            selectionrect = new Rect(selectionrect.position - clippingRect.position, selectionrect.size);
                            didClip       = true;
                        }
                        */

            //determine whether given asset is currently selected
            bool selected = Array.IndexOf(Selection.assetGUIDs, guid) > -1;

            string path  = AssetDatabase.GUIDToAssetPath(guid);
            Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(Object));

            //PvCustomizerGUI.InvertedColors = selected;

            PvCustomizerSettings settings = PvCustomizerSettings.GetOrCreateSettings();

            if (AssetDatabase.IsValidFolder(path) && settings.DrawFolderIcons)
            {
                if (settings.TryMatchAgainstRules(path, out var pattern))
                    DrawFolderIcon(path, selectionrect, pattern, selected);
            }
            else if (asset != null && settings.DrawAssetIcons)
            {
                //if the class has an attribute defined, draw it
                if (PvIconAttributeCache.TryGetClassAttribute(asset.GetType(), out var attrs))
                {
                    foreach (PvIconAttribute attr in attrs)
                    {
                        TryDrawFromValue(asset, asset, selectionrect, attr, selected);
                    }
                }
                //else, descend into the members
                else
                {
                    bool didDraw = FindAndDrawAttributes(asset, selectionrect, selected);
                }
            }

            /*if (didClip)
            {
                GUI.EndClip();
            }*/
        }

        private static Func<Rect> _getProjectBrowserRect;

        private static void ValidateProjectBrowser()
        {
            if (_getProjectBrowserRect == null || (_getProjectBrowserRect.Target as EditorWindow) == null)
            {
                var projectBrowserType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ProjectBrowser");
                var positionProp = projectBrowserType.GetProperty(nameof(EditorWindow.position),
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                var projectBrowser = EditorWindow.GetWindow(projectBrowserType);
                if (positionProp == null || projectBrowser == null) return;
                _getProjectBrowserRect =
                    Delegate.CreateDelegate(typeof(Func<Rect>), projectBrowser, positionProp.GetMethod, false) as
                        Func<Rect>;
            }
        }

        #region Simple Icon-Replacement

        private void DrawFolderIcon(string path, Rect selectionRect, PvRuleItem rule, bool selected)
        {
            string name = Path.GetFileName(path);
            IconSizeType sizeType = PvCustomizerUtility.GetSizeType(selectionRect);
            Rect iconRect = PvCustomizerUtility.ItemRectToIconRect(selectionRect, sizeType == IconSizeType.TreeView);
            Color tint = selected ? (Color) PvCustomizerGUI.ICON_SELECTED_TINT : Color.white;

            if (rule.eraseDefaultFolder)
            {
                PvCustomizerGUI.DrawBackground(iconRect);
            }

            if (sizeType == IconSizeType.Small || sizeType == IconSizeType.TreeView && rule.smallIcon.sprite != null)
            {
                iconRect = PvCustomizerUtility.ItemRectToIconRect(selectionRect, true);
                PvCustomizerGUI.DrawSprite(iconRect, rule.smallIcon.sprite, tint: tint);
            }
            else if (sizeType == IconSizeType.Large && rule.largeIcon.sprite != null)
            {
                PvCustomizerGUI.DrawSprite(iconRect, rule.largeIcon.sprite, tint: tint);
            }

            if (rule.textBackground.sprite != null)
            {
                Rect textRect = PvCustomizerUtility.ItemRectToTextRect(selectionRect);
                textRect.height++;
                PvCustomizerGUI.DrawBackground(textRect);
                PvCustomizerGUI.DrawSprite(textRect, rule.textBackground.sprite, tint: tint);
                using (new TempFontSize(10))
                {
                    PvCustomizerGUI.DrawTextDirect(textRect, name,
                        textAnchor: sizeType == IconSizeType.Small || sizeType == IconSizeType.TreeView ? PvAnchor.MiddleLeft : PvAnchor.MiddleCenter,
                        color: rule.textColor);
                }
            }
        }

        #endregion

        #region Project Icon Drawing

        private bool FindAndDrawAttributes(Object asset, Rect fullRect, bool selected)
        {
            //get all relevant attributes and member info on which they're declared
            var triplets = PvIconAttributeCache.GetAttributeTriplets(asset.GetType());
            if (triplets.Exists(t => t.Attr.Display != "false" && !t.Attr.DontEraseDefault))
            {
                IconSizeType sizeType = PvCustomizerUtility.GetSizeType(fullRect);
                Rect iconRect = PvCustomizerUtility.ItemRectToIconRect(fullRect, sizeType == IconSizeType.TreeView);
                PvCustomizerGUI.DrawBackground(iconRect);
            }

            bool drewAtleastOnce = false;

            foreach (var (member, attr, valueType) in triplets)
            {
                object value = member.GetMemberValue(asset);
                drewAtleastOnce |= TryDrawFromValue(asset, value, fullRect, attr, selected);
            }

            return drewAtleastOnce;
        }

        private bool TryDrawFromValue(object asset, object value, Rect fullRect,
            PvIconAttribute attr, bool selected)
        {
            if (attr.Display == "false") return false;
            if (value        == null) return false;
            if (!TryGetDrawer(value.GetType(), out var drawer)) return false;

            //compile the style from attribute into something we can pass along to drawer
            IconStyle style = CompileStyle(asset, attr, fullRect);

            try
            {
                if (selected) style.Tint *= PvCustomizerGUI.ICON_SELECTED_TINT;
                drawer.Draw(value, fullRect, selected, style);
            }
            catch
            {
                //ignore
            }

            return true;

            bool TryGetDrawer(Type valueType, out IDrawer d)
            {
                //get proper drawer for this member's value type
                d = Registry.GetIconDrawer(valueType);
                return d != null;
            }
        }

        private IconStyle CompileStyle(object asset, PvIconAttribute attr, Rect fullRect)
        {
            IconSizeType sizeType = PvCustomizerUtility.GetSizeType(fullRect);
            Rect         iconRect = PvCustomizerUtility.ItemRectToIconRect(fullRect, sizeType == IconSizeType.TreeView);
            IconStyle style = new IconStyle
            {
                IsSet        = true,
                Tint         = attr.Tint,
                CustomValues = attr.CustomData,
                FontStyle    = attr.FontStyle,
                SizeType     = PvCustomizerUtility.GetSizeType(iconRect),
                MaxSize      = attr.MaxSize,
                ScaleMode    = attr.ScaleMode
            };

            //----------complex assignments

            #region Material

            if (string.IsNullOrEmpty(attr.Material))
            {
                style.Material = null;
            }
            else
            {
                ValueableMemberCache.Pair matMember = ValueableMemberCache.GetValueables(asset.GetType()).
                                                                           Find(v => v.Member.Name == attr.Material &&
                                                                               v.ValueType         == typeof(Material));

                style.Material = matMember.Member?.GetMemberValue(asset) as Material;
            }

            #endregion

            if (PvCustomizerUtility.TryParseGridRect(attr.Grid, out int rows, out int cols, out int pos))
            {
                float width  = iconRect.width  / cols;
                float height = iconRect.height / rows;

                float x = width  * (pos % cols);
                float y = height * (pos / rows);

                style.DrawRect = new Rect(iconRect.x + x, iconRect.y + y, width, height);
            }
            else
            {
                #region ScaleMode / DrawRect

                bool success = true;
                success &= PvCustomizerUtility.TryParseLengthString(attr.Width,  iconRect.width,  out float width);
                success &= PvCustomizerUtility.TryParseLengthString(attr.Height, iconRect.height, out float height);
                success &= PvCustomizerUtility.TryParseLengthString(attr.X,      iconRect.width,  out float x);
                success &= PvCustomizerUtility.TryParseLengthString(attr.Y,      iconRect.height, out float y);

                Rect drawRect;
                if (!success)
                {
                    drawRect = new Rect(iconRect);
                }
                else
                {
                    if (width > attr.MaxSize || height > attr.MaxSize)
                    {
                        float aspect = width / height;
                        if (width > height)
                        {
                            width  = attr.MaxSize;
                            height = aspect / width;
                        }
                        else
                        {
                            height = attr.MaxSize;
                            width  = aspect * height;
                        }
                    }

                    drawRect = new Rect(iconRect.x + x, iconRect.y + y, width, height);
                }

                Vector2 offset        = PvCustomizerUtility.GetNormalizedOffsetFromAnchor(attr.IconAnchor);
                Vector2 parentOffset  = iconRect.size * offset;
                Vector2 ownSizeOffset = drawRect.size * offset;
                drawRect.position += parentOffset;
                drawRect.position -= ownSizeOffset;

                style.DrawRect = drawRect;

                #endregion
            }

            return style;
        }

        #endregion
    }
}