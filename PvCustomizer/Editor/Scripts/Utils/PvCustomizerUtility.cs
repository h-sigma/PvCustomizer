using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Akaal.Editor.Utils
{
    public static class PvCustomizerUtility
    {
        public const int DefaultPriority = 100;

        /// <summary>
        /// Maps the rect of the whole project item to the area meant for the graphic.
        /// </summary>
        public static Rect ItemRectToIconRect(Rect itemRect, bool square = false)
        {
            Rect iconRect = new Rect(itemRect);
            if (itemRect.width > itemRect.height)
            {
                if (square)
                {
                    iconRect.width = itemRect.height;
                }
                else
                {
                    iconRect.width = itemRect.height * 4.5f / 4f;
                }
            }
            else iconRect.height = itemRect.width;

            return iconRect;
        }

        public static Rect ItemRectToTextRect(Rect itemRect)
        {
            Rect iconRect = new Rect(itemRect);
            if (itemRect.width > itemRect.height)
            {
                iconRect.width -= itemRect.height;
                iconRect.x     += itemRect.height;
            }
            else
            {
                iconRect.y      += itemRect.width;
                iconRect.height =  itemRect.height - itemRect.width;
            }

            return iconRect;
        }

        /// <summary>
        /// Whether icon being drawn is "small" or "large" (<see cref="IconSizeType"/>).
        /// </summary>
        public static IconSizeType GetSizeType(Rect itemRect)
        {
            if (itemRect.width > itemRect.height) return IconSizeType.Small;
            else return IconSizeType.Large;
        }

        /// <summary>
        /// Tries to parse length string into a float.
        /// If string ends with a '%', it is considered relative to some parent length. Else, it is considered fixed length.
        /// </summary>
        /// <param name="lengthString">String containing length description.</param>
        /// <param name="parentLength">Length of parent, in case string defines relative length.</param>
        /// <param name="length">Fixed length parsed from input.</param>
        /// <returns>True if string is valid length. False otherwise.</returns>
        public static bool TryParseLengthString(string lengthString, float parentLength, out float length)
        {
            if (lengthString.EndsWith("%"))
            {
                string lengthStringTrimmed = lengthString.Substring(0, lengthString.Length - 1);
                if (!float.TryParse(lengthStringTrimmed, out length)) return false;
                length = (parentLength * length / 100f);
                return true;
            }

            return float.TryParse(lengthString, out length);
        }

        /// <summary>
        /// Gets the normalized position for each anchor w.r.t. top-left being (0, 0) and bottom-right being (1,1).
        /// </summary>
        public static Vector2 GetNormalizedOffsetFromAnchor(PvAnchor attrIconAnchor)
        {
            switch (attrIconAnchor)
            {
                case PvAnchor.UpperLeft: return new Vector2(0, 0);
                case PvAnchor.UpperCenter: return new Vector2(0.5f, 0);
                case PvAnchor.UpperRight: return new Vector2(1, 0);
                case PvAnchor.MiddleLeft: return new Vector2(0, 0.5f);
                case PvAnchor.MiddleCenter: return new Vector2(0.5f, 0.5f);
                case PvAnchor.MiddleRight: return new Vector2(1, 0.5f);
                case PvAnchor.LowerLeft: return new Vector2(0, 1);
                case PvAnchor.LowerCenter: return new Vector2(0.5f, 1);
                case PvAnchor.LowerRight: return new Vector2(1f, 1);
                default: throw new ArgumentOutOfRangeException(nameof(attrIconAnchor), attrIconAnchor, null);
            }
        }

        /// <summary>
        /// Grid is defined as format 'rows:columns/position', e.g. '3/3:4' is row 2 col 1 (Middle-Left) in a 3 by 3 grid. 
        /// </summary>
        public static bool TryParseGridRect(string grid, out int rows, out int cols, out int pos)
        {
            rows = 0;
            cols = 0;
            pos  = 0;
            try
            {
                if (_gridParse.TryGetValue(grid, out var pair))
                {
                    if (!pair.Item1) return false;
                    rows = pair.rows;
                    cols = pair.cols;
                    pos  = pair.pos;
                    return true;
                }

                if (string.IsNullOrEmpty(grid))
                {
                    return InvalidResult();
                }

                const string pattern = "(?<row>\\d+)/(?<col>\\d+):(?<pos>\\d+)";

                var matches = Regex.Matches(grid, pattern);
                if (matches.Count == 0)
                {
                    return InvalidResult();
                }

                var  groups  = matches[0].Groups;
                bool success = int.TryParse(groups["row"].Captures[0].Value, out rows);
                success &= int.TryParse(groups["col"].Captures[0].Value, out cols);
                success &= int.TryParse(groups["pos"].Captures[0].Value, out pos);

                if (!success) return InvalidResult();

                _gridParse[grid] = (true, rows, cols, pos);
                return true;
            }
            catch
            {
                return InvalidResult();
            }

            bool InvalidResult()
            {
                _gridParse[grid] = (false, 0, 0, 0);
                return false;
            }
        }

        private static readonly Dictionary<string, (bool valid, int rows, int cols, int pos)> _gridParse =
            new Dictionary<string, (bool valid, int rows, int cols, int pos)>();

        public static Mesh GetPrimitiveMesh(string meshName)
        {
            return GetPrimitiveMesh(meshName == "Plane" ? PrimitiveType.Plane : PrimitiveType.Sphere);
        }

        private static Mesh GetPrimitiveMesh(PrimitiveType meshType)
        {
            if (_meshes.TryGetValue(meshType, out var mesh)) return mesh;
            var go = GameObject.CreatePrimitive(meshType);
            mesh = go.GetComponent<MeshFilter>().sharedMesh;
            Object.DestroyImmediate(go);
            _meshes[meshType] = mesh;
            return mesh;
        }

        private static Dictionary<PrimitiveType, Mesh> _meshes = new Dictionary<PrimitiveType, Mesh>();
    }
}