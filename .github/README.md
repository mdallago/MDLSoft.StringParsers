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

1. **For NuGet Publishing**: Add `NUGET_API_KEY` secret to repository settings
2. **For Codecov**: Repository will be automatically detected
3. **For Security Scanning**: CodeQL is enabled by default

## Usage

- **Push to main**: Runs full CI pipeline
- **Create PR**: Runs build and test validation  
- **Create Release**: Publishes to NuGet (if API key is configured)
- **Manual Dispatch**: Can trigger workflows manually from Actions tab