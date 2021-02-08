![PvCustomizer Logo](.readme/logo.png "PvCustomizer Logo")

# PvCustomizer - What and Why

## What

This utility lets you customize the Project Window in two ways:

1. Draw custom icons for folders based on path/name/extension/regex matching.
2. Draw custom icons for Scriptable Object assets, adding flavor and ease-of-use to your project.

## Why

The creator of the repository holds the view that customization of the project browser/window in Unity is such a
fundamental QoL improvement that it shouldn't be locked behind assets that cost 15-20$ each. He does not claim that the
effort put behind such assets by their creators isn't reward-worthy. If you can afford these assets and think the
professional support to be a plus, I highly recommend purchasing them to support their ongoing development.

## Please Note

This utility is still in very early stages of development. The core is there, and it works -- on the surface. Please
report any bugs you find to harsh@aka.al or create a new issue.

# Table of Contents

- [Installation](#installation)
- [Quick Overview](#overview)
- [Folder Icons](#folder-icons)
- [Asset Icons](#asset-icons)
    - [Introduction](#asset-icons__introduction)
    - [Usage](#asset-icons__usage)
- [Settings](#settings)
- [Contact](#contact)

# Installation <a name="installation"/>

Use the Unity Package Manager to download from this git url. This lets you fetch updates more easily in the future.

# Overview <a name="overview"/>

There are two main parts of this utility: folder icons and asset icons.

### Asset Icons
The most usage out of **Asset Icons** you'll probably need is to put the `PvIconAttribute` on a serialized field of a Scriptable Object.
If the type of the field matches one of the drawers defined in the project, a custom icon will be drawn inside the Project View.

Example:
```
public class InventoryItem : ScriptableObject 
{
    [PvIcon]
    public Sprite itemSprite;
}
```

![Asset Icon Example](.readme/example1.png "Asset Icons Panel")

### Folder Icons

Select a folder in the Unity Project Window and press `CTRL + ALT + C` to open the folder rule panel.

In this panel, you can configure the rule and the icon drawn for folders that match that rule. You can view all currently registered rules in the Preferences/PvCustomizer settings menu.

![Folder Rule Panel](.readme/example2.png "Folder Rule Panel")

You can disable rules, change type of rule, duplicate rules, modify priority (in case multiple rules match the folder). Don't forget to apply the changes!

# Folder Icons <a name="folder-icons"/>

![Folder Rule Panel](.readme/example2.png "Folder Rule Panel")

## Options

- The text field at the top is the **rule string**. The enum next to is the **rule type**. These are used to match a folder asset to a rule. 
  
| RuleType | Description |
|:--------:|-------------|
| Name | This rule type matches the folder name directly. So, all folders in your assets with this name will match the rule.|
| Path | This is the strictest rule type. It matches only complete path of the folder from root of project. |
| Extension | This rule matches assets by their extension. E.g, ".pdf", ".txt", etc.
| Regex | This rule tries to do a C# regex match with the folder path. E.g. `.*Editor.*` matches the Editor folders and all of their descendant folders. Paths start from the project root, e.g. `Assets/Path/To/Asset`.

- **Enabled**: This toggle can enable or disable the usage of a rule, so you can temporarily disable rules.
- **Priority** : When multiple rules match a folder, the rule with the highest priority is used to draw the icon.
- **Erase Default**: Whether the icon will erase the default Unity icon. This is useful in case you want to add a small graphic to the folder instead of drawing a completely new one.
- **Large Icon**: When zoom is low in the second pane of the Project Window, this sprite will be used to draw the icon.
- **Small Icon**: When zoom is high in the second pane or we're drawing the folder tree in the first pane, this sprite will be used for the icon.
- **Background**: This sprite will be drawn *behind* the folder label.
- **Text Color**: This color will be used to draw the folder label text.

## Buttons

- **TrashCan/Delete**: Deletes the rule from the project. No take-backs.
- **Settings**: Shortcut for opening the asset settings.
- **Left Arrow**: Activates when multiple rules are being viewed. Goes to previous rule.
- **Right Arrow**: Activates when multiple rules are being viewed. Goes to next rule.
- **Plus/Duplicate**: Duplicates the currently selected rule. It won't be saved unless *Apply* is pressed.


- **Apply**: Applies any changes and saves any new rules. Only works on the currently selected rule, in case of multiple rules being viewed.
- **Cancel**: Closes the panel without saving any changes.
- **Reset Changes**: Restores the currently selected rule to the last saved state, abandoning all changes made since opening this panel.

# Asset Icons<a name="asset-icons"/>

## Introduction<a name="asset-icons__introduction"/>

todo

## Usage<a name="asset-icons__usage"/>

todo

#Settings <a name="settings"/>


You may access the PvCustomizer settings under `Preferences/PvCustomizer`.

![Settings Menu](.readme/settings.png "Settings Menu")

- **Tint Amount**: When an asset is selected, how much to tint the drawn graphic.
- **Draw Folder Icons**: Project-wide toggle for drawing folder icons at all.
- **Draw Asset Icons**: Project-wide toggle for drawing asset icons at all.
- **Folder Rules**: A list of all of the folder rules registered in this asset.

# Contact<a name="contact"/>

Add a new issue, or drop me an e-mail at [harsh@aka.al](mailto:harsh@aka.al "Mail to harsh@aka.al"). If the topic is this repository, put `PvCustomizer` somewhere in the subject.

I also have a no-effort blog at [https://hdeep.tech](https://hdeep.tech).

Finally, you can find me under `karmicto` in the [Game Dev League Discord](https://discord.gg/eJbG9VD8R9 "GDL Discord Invite Link").