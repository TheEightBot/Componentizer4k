# Componentizer4k Development Instructions

Componentizer4k is a .NET MAUI navigation component library that simplifies in-page or component-based navigation. It consists of a core library, MAUI-specific implementation, and a sample application.

**CRITICAL: Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.**

## Platform Requirements and Limitations

### Full Development (macOS/Windows Required)
- **macOS or Windows** is required for complete MAUI development including mobile targets
- Install .NET 8.0 SDK
- Install MAUI workloads: `dotnet workload install maui --ignore-failed-sources`
- Can build all projects: Componentizer.Core, Componentizer.MAUI, SampleApp
- Can run and test the sample application

### Limited Development (Linux/CI Compatible)
- **Linux environments** can only build the Core library
- Building MAUI projects will fail with workload errors on Linux
- Use `dotnet build Componentizer.Core/Componentizer.Core.csproj` for Core-only development
- Cannot run sample application validation

## Quick Start and Build Commands

### Core Library Development (Works on All Platforms)
**NEVER CANCEL: Allow 60+ seconds for initial builds. Set timeout to 120+ seconds.**

```bash
# Build core library - takes ~8 seconds
dotnet build Componentizer.Core/Componentizer.Core.csproj

# Create NuGet package - takes ~3 seconds  
dotnet pack Componentizer.Core/Componentizer.Core.csproj

# Restore dependencies - takes ~5 seconds
dotnet restore Componentizer.Core/Componentizer.Core.csproj
```

### Full Solution Build (macOS/Windows Only)
**NEVER CANCEL: MAUI builds can take 2-5 minutes. Set timeout to 10+ minutes.**

```bash
# Install MAUI workloads first (one-time setup) - takes 30-60 seconds
dotnet workload install maui --ignore-failed-sources

# Build entire solution - takes 2-5 minutes
dotnet build Componentizer.sln

# Create all NuGet packages - takes 1-2 minutes
dotnet pack Componentizer.sln --configuration Release
```

### Sample Application (macOS/Windows Only)
**NEVER CANCEL: MAUI app builds take 3-10 minutes depending on platform. Set timeout to 15+ minutes.**

```bash
# Build sample app - takes 3-10 minutes
dotnet build SampleApp/SampleApp.csproj

# Run sample app (platform-specific targets)
# For Windows: dotnet run --project SampleApp/SampleApp.csproj --framework net8.0-windows10.0.19041.0
# For macOS: dotnet run --project SampleApp/SampleApp.csproj --framework net8.0-maccatalyst
# For Android/iOS: Requires emulator or device setup
```

## Code Quality and Validation

### Formatting and Linting
**NEVER CANCEL: Formatting can take 5-15 seconds. Set timeout to 60+ seconds.**

```bash
# Check code formatting - takes ~9 seconds
dotnet format --verify-no-changes Componentizer.Core/Componentizer.Core.csproj

# Apply formatting fixes - takes ~7 seconds
dotnet format Componentizer.Core/Componentizer.Core.csproj

# For full solution (macOS/Windows only) - takes 15-30 seconds
# NOTE: Will fail on Linux due to MAUI workload requirements
dotnet format --verify-no-changes Componentizer.sln
dotnet format Componentizer.sln
```

### Build Validation Steps
Always run these commands before committing changes:

1. **Format code**: `dotnet format Componentizer.Core/Componentizer.Core.csproj` (Core only works on all platforms)
2. **Build solution**: `dotnet build Componentizer.sln` (macOS/Windows) or `dotnet build Componentizer.Core/Componentizer.Core.csproj` (Linux)
3. **Verify packages**: `dotnet pack --configuration Release Componentizer.Core/Componentizer.Core.csproj`

**No automated tests exist** - this library has no test projects.

## Project Structure and Key Files

### Projects
- **Componentizer.Core/**: Core navigation interfaces and implementation (.NET 8.0)
  - `IComponentNavigation.cs` - Main navigation service interface
  - `ComponentNavigation.cs` - Core navigation implementation
  - `IComponentNavigator.cs` - Navigator component interface
  - `IPreventBackNavigation.cs` - Interface for preventing navigation
  
- **Componentizer.MAUI/**: MAUI-specific implementation (multi-target)
  - `MauiComponentNavigator.cs` - MAUI Grid-based navigator control
  - `MauiBuilderExtensions.cs` - DI registration extensions
  - `AssemblyInfo.cs` - XAML namespace definitions

- **SampleApp/**: Demo MAUI application
  - `MainPage.xaml` - Main page with MauiComponentNavigator
  - `MainViewModel.cs` - Navigation demonstration logic
  - `ComponentA.xaml`/`ComponentB.xaml` - Sample navigation targets

### Configuration Files
- **Componentizer.sln** - Main solution file
- **global.json** - .NET SDK version configuration
- **Directory.build.props** - Shared project properties and analyzer references
- **stylecop.json** - Code style rules configuration
- **.editorconfig** - Comprehensive code formatting rules
- **.github/workflows/nuget.yml** - CI/CD pipeline for NuGet publishing

### Important File Contents

#### Solution Projects (from `Componentizer.sln`)
```
Componentizer.Core - Core library (.NET 8.0)
Componentizer.MAUI - MAUI implementation (multi-target)
SampleApp - Demo application (multi-target MAUI)
```

#### Target Frameworks (from project files)
```
Componentizer.Core: net8.0
Componentizer.MAUI: net8.0-android;net8.0-ios;net8.0-maccatalyst;net8.0-windows10.0.19041.0
SampleApp: net8.0-android;net8.0-ios;net8.0-maccatalyst;net8.0-windows10.0.19041.0
```

## Manual Validation (macOS/Windows Only)

When making changes to the MAUI implementation or sample app, always validate with the sample application:

1. **Build and run sample app**: Follow the Sample Application commands above
2. **Test navigation**: Use the "Go Forward >" and "< Go Backward" buttons
3. **Verify component switching**: Ensure ComponentA and ComponentB load correctly
4. **Check parameter passing**: ComponentB should receive timestamp parameters

## Common Development Tasks

### Adding New Navigation Features
1. Modify interfaces in `Componentizer.Core/`
2. Update implementation in `ComponentNavigation.cs`
3. Update MAUI implementation in `MauiComponentNavigator.cs`
4. Test with sample app (macOS/Windows only)
5. Apply code formatting and build validation

### Updating Dependencies
1. Edit `.csproj` files to update PackageReference versions
2. Run `dotnet restore` on affected projects
3. Test build with updated dependencies
4. Verify sample app still functions (macOS/Windows only)

### CI/CD Compatibility
The GitHub Actions workflow (`.github/workflows/nuget.yml`) requires:
- **macOS runner** (uses `macos-latest`)
- **.NET 8.x SDK**
- **MAUI workloads** installed via `dotnet workload install maui --ignore-failed-sources`
- **Build time expectations**: 2-5 minutes for full solution build

## Troubleshooting

### "MAUI workloads not installed" errors on Linux
- **Expected behavior** - MAUI is not supported on Linux
- **Solution**: Use Core project development only or switch to macOS/Windows

### Build failures after dependency updates
- Run `dotnet restore` on all projects
- Clear `bin` and `obj` directories if needed
- Verify .NET 8.0 SDK is installed

### Formatting errors during CI
- Always run `dotnet format` before committing
- Check `.editorconfig` rules for specific formatting requirements
- Pay attention to final newline requirements (files must end with newline)

## Time Expectations Summary
- **Core library build**: 8-15 seconds
- **Full solution build**: 2-5 minutes
- **MAUI workload install**: 30-60 seconds  
- **Code formatting**: 7-15 seconds
- **Package creation**: 2-5 seconds
- **Sample app build**: 3-10 minutes
