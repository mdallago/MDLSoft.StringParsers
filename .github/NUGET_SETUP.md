# NuGet Publishing Setup Guide

This guide will help you set up automated NuGet package publishing for your MDLSoft.StringParsers library.

## 🔑 Step 1: Create NuGet API Key

### Option A: Using NuGet.org Website
1. **Sign in** to [nuget.org](https://www.nuget.org)
2. **Go to API Keys**: Click your username → "API Keys"
3. **Create New API Key**:
   - **Key Name**: `MDLSoft.StringParsers GitHub Actions`
   - **Owner**: Select your account or organization
   - **Scopes**: Select "Push new packages and package versions"
   - **Select Packages**: Choose "Unlist packages" or specific packages
   - **Glob Pattern**: `MDLSoft.StringParsers*` (or leave empty for all)
4. **Copy the API Key** (you won't see it again!)

### Option B: Using .NET CLI
```bash
# Login to NuGet
dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org

# This will prompt you to login via browser
dotnet nuget trusted-signers add author nuget.org
```

## 🔐 Step 2: Add API Key to GitHub Secrets

1. **Go to your GitHub repository**
2. **Navigate to**: Settings → Secrets and variables → Actions
3. **Click "New repository secret"**
4. **Add the secret**:
   - **Name**: `NUGET_API_KEY`
   - **Value**: Paste your NuGet API key
5. **Click "Add secret"**

## 🏗️ Step 3: Set Up GitHub Environment (Optional but Recommended)

For additional security, set up a production environment:

1. **Go to**: Settings → Environments
2. **Click "New environment"**
3. **Environment name**: `production`
4. **Configure protection rules**:
   - ✅ **Required reviewers**: Add yourself or team members
   - ✅ **Wait timer**: 0 minutes (or set a delay)
   - ✅ **Deployment branches**: Selected branches → `main`

## 🚀 Step 4: Test the Setup

### Automatic Publishing (Push to main)
```bash
# Make a change and push to main
git add .
git commit -m "feat: trigger nuget publishing test"
git push origin main
```

### Manual Publishing (Create a Release)
1. **Go to**: GitHub → Releases → "Create a new release"
2. **Tag version**: `v1.0.2` (increment from current version)
3. **Release title**: `Release 1.0.2`
4. **Description**: Describe what changed
5. **Click "Publish release"**

### Manual Dispatch
1. **Go to**: Actions → CI/CD Pipeline
2. **Click "Run workflow"**
3. **Select branch**: main
4. **Click "Run workflow"**

## 📦 Step 5: Verify Publication

After successful publishing:

1. **Check NuGet.org**: Visit [nuget.org/packages/MDLSoft.StringParsers](https://www.nuget.org/packages/MDLSoft.StringParsers)
2. **Check GitHub Actions**: Verify the workflow completed successfully
3. **Test Installation**:
   ```bash
   dotnet add package MDLSoft.StringParsers --version 1.0.1
   ```

## 🔧 Troubleshooting

### Common Issues

#### 🚫 "Package already exists" Error
- **Solution**: Version number hasn't been incremented
- **Fix**: Update version in `MDLSoft.StringParsers.csproj`

#### 🔑 "Invalid API Key" Error
- **Solution**: API key is wrong or expired
- **Fix**: Generate new API key and update GitHub secret

#### 🛡️ "Environment protection rule" Error
- **Solution**: Waiting for approval or protection rules
- **Fix**: Approve deployment in GitHub Actions or adjust environment rules

#### 📦 "Package validation failed" Error
- **Solution**: Package doesn't meet NuGet requirements
- **Fix**: Check package metadata in `.csproj` file

### Debug Commands

```bash
# Test package creation locally
dotnet pack --configuration Release --output ./test-packages

# Validate packages locally
./scripts/validate-packages.ps1 -PackageDirectory "./test-packages"

# Test publish to test feed (if you have one)
dotnet nuget push ./test-packages/*.nupkg --source "your-test-feed" --api-key "test-key"
```

## ⚙️ Advanced Configuration

### Versioning Strategy

The workflows support multiple versioning approaches:

1. **Manual Version in .csproj**:
   ```xml
   <Version>1.0.2</Version>
   ```

2. **Git Tag Version** (in release workflow):
   - Git tags like `v1.0.2` automatically update the version

3. **Semantic Versioning**:
   - Consider using GitVersion or similar tools

### Package Metadata

Ensure your `.csproj` has complete metadata:

```xml
<PropertyGroup>
  <TargetFrameworks>netstandard2.0;net40</TargetFrameworks>
  <GeneratePackageOnBuild>True</GeneratePackageOnBuild>
  <Title>MDLSoft.StringParsers</Title>
  <Description>A powerful .NET library for parsing and writing structured text data</Description>
  <Authors>Your Name</Authors>
  <Company>MDLSoft</Company>
  <PackageLicenseFile>LICENSE</PackageLicenseFile>
  <PackageReadmeFile>README.md</PackageReadmeFile>
  <RepositoryUrl>https://github.com/mdallago/MDLSoft.StringParsers</RepositoryUrl>
  <RepositoryType>git</RepositoryType>
  <PackageTags>parser;csv;fixed-width;text-processing</PackageTags>
  <Version>1.0.1</Version>
</PropertyGroup>
```

## 🎉 You're All Set!

Once configured, your library will automatically publish to NuGet on:
- ✅ **Push to main branch** (via CI/CD pipeline)
- ✅ **GitHub Releases** (via release workflow)
- ✅ **Manual dispatch** (on-demand)

Your users can then install it with:
```bash
dotnet add package MDLSoft.StringParsers
```