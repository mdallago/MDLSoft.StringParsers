# GitHub Actions Workflows

This directory contains GitHub Actions workflows for the MDLSoft.StringParsers project.

## Workflows

### 1. `ci.yml` - Complete CI/CD Pipeline
- **Triggers**: Push to main/develop, PRs to main, manual dispatch
- **Features**: 
  - Multi-OS testing (Ubuntu, Windows, macOS)
  - Multiple .NET versions
  - Code coverage with Codecov
  - NuGet package creation and validation
  - Security scanning with CodeQL
  - Deployment pipeline (disabled by default)

### 2. `build-test.yml` - Simple Build and Test
- **Triggers**: Push to main, PRs to main
- **Features**:
  - Multi-OS and multi .NET version testing
  - Code coverage reporting
  - Faster execution for basic validation

### 3. `release.yml` - Release and Publish
- **Triggers**: GitHub releases, manual dispatch
- **Features**:
  - Automatic version updating
  - NuGet package publishing
  - Artifact uploads

## Configuration Files

### `dependabot.yml`
- Automatically updates NuGet packages weekly
- Updates GitHub Actions weekly
- Creates PRs with proper labels and assignments

## Setup Instructions

### 🚀 **NuGet Publishing (ENABLED)**

**NuGet publishing is now ENABLED and ready to use!**

1. **Set up API Key**: 
   - Get your NuGet API key from [nuget.org](https://www.nuget.org/account/apikeys)
   - Add `NUGET_API_KEY` secret to repository settings
   - See detailed setup: [NUGET_SETUP.md](NUGET_SETUP.md)

2. **Publishing Triggers**:
   - ✅ **Automatic**: Push to main branch → publishes latest version
   - ✅ **Releases**: Create GitHub release → publishes tagged version  
   - ✅ **Manual**: Use workflow dispatch for on-demand publishing

3. **Environment Protection**:
   - Uses `production` environment for additional security
   - Requires manual approval before publishing (optional)

### 📊 **Other Services**

2. **For Codecov**: Repository will be automatically detected
3. **For Security Scanning**: CodeQL is enabled by default

## Package Signing (Optional)

The current validation does **not require package signing**. If you want to enable package signing:

1. **Generate a code signing certificate**
2. **Add certificate to repository secrets**:
   - `CERTIFICATE_BASE64`: Base64-encoded certificate
   - `CERTIFICATE_PASSWORD`: Certificate password
3. **Update the pack command in workflows**:
   ```yaml
   - name: Sign and Pack
     run: |
       dotnet pack --configuration Release --no-build --output ./artifacts
       # Add signing commands here if needed
   ```

## Troubleshooting

### NU3004 Error (Package not signed)
- **Issue**: `dotnet nuget verify` requires signed packages
- **Solution**: Packages are validated using structure checks instead of signature verification
- **Optional**: Enable package signing following the steps above

## Usage

- **Push to main**: Runs full CI pipeline
- **Create PR**: Runs build and test validation  
- **Create Release**: Publishes to NuGet (if API key is configured)
- **Manual Dispatch**: Can trigger workflows manually from Actions tab