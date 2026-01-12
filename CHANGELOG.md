# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.0-beta4] - 2026-01-12

## [1.0.0-beta3] - 2026-01-12
### Fixed
- Fixed PackageReadmeFile reference to use correct case (README.md instead of readme.md)


## [1.0.0-beta2] - 2026-01-12
### Added
- CI/CD pipeline for pull requests and main branch pushes with matrix build strategy
- Code coverage reports with Codecov integration
- SourceLink support for better debugging experience
- EditorConfig for consistent code formatting
- Enhanced NuGet package metadata with tags and improved description
- CHANGELOG.md to track version history
- Automated CHANGELOG.md update script that runs on release tag creation
- Automated release notes extraction from CHANGELOG.md for GitHub releases
- Scripts for release automation (.github/scripts/update-changelog.sh, extract-release-notes.sh)

### Changed
- GitHub Actions release workflow now supports all target frameworks (.NET 8.0, 9.0, 10.0)
- Improved NuGet package description with more details
- Added AnalysisLevel property for latest analyzer features
- Disabled automatic package generation on build (GeneratePackageOnBuild=false) to avoid SDK packaging issues
- Package is now created explicitly via CI/CD workflow using `dotnet pack`

### Fixed
- Removed duplicate TargetFrameworks property in project file
- Fixed typo in release workflow comment ("tay" → "tag")
- Fixed README.md path in package metadata (correct case and path separator)
- Added Claude Code temporary files to .gitignore (.claude, tmpclaude-*)


## [0.0.1] - 2026-01-12

### Added
- Initial release of Porticle.Reflection.Extensions
- Extension methods for converting .NET reflection types to human-readable strings
- Support for PropertyInfo, ParameterInfo, FieldInfo, and EventInfo
- Type extension method with optional NullabilityInfo parameter
- Comprehensive handling of:
  - Nullable reference types (string?, List<string>?)
  - Nullable value types (int?, DateTime?)
  - Generic types (List<T>, Dictionary<K,V>)
  - Arrays (single-dimensional, jagged, and multi-dimensional)
  - C# type aliases (int, string, bool, etc.)
  - Event handlers (EventHandler, Action, Func, custom delegates)
- ThreadLocal NullabilityInfoContext for performance optimization
- Dictionary-based type alias lookup for improved performance
- 125+ comprehensive unit tests with xUnit
- Support for .NET 8.0, 9.0, and 10.0
- XML documentation for all public APIs
- MIT License
- Comprehensive README with usage examples
- GitHub Actions workflow for automated releases

[Unreleased]: https://github.com/Machibuse/Porticle.Reflection.Extensions/compare/v1.0.0-beta4...HEAD
[1.0.0-beta4]: https://github.com/Machibuse/Porticle.Reflection.Extensions/releases/tag/v1.0.0-beta4
[1.0.0-beta3]: https://github.com/Machibuse/Porticle.Reflection.Extensions/releases/tag/v1.0.0-beta3
[1.0.0-beta2]: https://github.com/Machibuse/Porticle.Reflection.Extensions/releases/tag/v1.0.0-beta2
[0.0.1]: https://github.com/Machibuse/Porticle.Reflection.Extensions/releases/tag/v0.0.1
