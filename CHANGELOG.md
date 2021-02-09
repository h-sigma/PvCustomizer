# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Upcoming]
- Fix for GUI drawing over project window toolbars.
- Optional Samples.
- Collab/VCS overlay support.
- PvIcon Attribute for regular serializable classes.
- String expression conditions for displaying of asset icons.

## [0.0.3] - 2021-02-09
### Fixes
- Fixed issue with large icon being categorized as tree view icons.
- Fixed null ref errors when trying to draw a sprite with null texture.
 
### Added
- Added support for displaying asset icons selectively for small/large sizes only.

## [0.0.2] - 2021-02-08
### Changes
- Changed Samples from optional directory to in-package.
- Fixed jerry-rigged solution for tree view drawing
- "TreeView" value to Enum IconSizeType
- Project-wide toggles for asset and folder drawing
- Selection tint not works based on guid of selected assets, meaning selecting folder from tree view now tints

## Added
- Icons for readme.md

## [0.0.1] - 2021-02-08
### Added
- All source files.
- License
- Readme
- Changelog