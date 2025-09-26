# Release Process

## 🚀 How to Create a New Release

### 1. **Trigger the Create Release Workflow**
1. Go to **Actions** tab in GitHub
2. Select **"Create Release"** workflow  
3. Click **"Run workflow"**
4. Fill in the form:
   - **Version**: `1.1.2` (without 'v' prefix)
   - **Release notes**: Optional custom notes (auto-generated if empty)

### 2. **What Happens Automatically**
The workflow will:
- ✅ Update version in `MDLSoft.StringParsers.csproj`
- ✅ Commit the version change to main branch
- ✅ Create a git tag (e.g., `v1.1.2`)
- ✅ Create a GitHub release with auto-generated notes
- ✅ Trigger the Release workflow which publishes to NuGet.org

### 3. **Workflow Chain**
```
Create Release Workflow → GitHub Release Created → Release Workflow → NuGet Published
```

## 📋 **Current Workflows**

| Workflow | Trigger | Purpose |
|----------|---------|---------|
| **CI/CD Pipeline** | Push to main/develop, PRs | Build, test, validate packages |
| **Create Release** | Manual trigger | Create releases and tags |
| **Release** | Release published | Publish to NuGet.org |

## ✨ **Benefits of This Process**

- 🎯 **Controlled Publishing**: Only publish to NuGet when you create a release
- 🏷️ **Proper Tagging**: Automatic git tags for version tracking
- 📝 **Release Notes**: Auto-generated from commit history
- 🔄 **Clean Workflow**: Clear separation between CI/CD and releasing
- 🚫 **No Accidents**: Can't accidentally publish on regular commits

## 🛠️ **For Development**

Regular development commits to `main` will:
- ✅ Build and test the code
- ✅ Create NuGet packages for validation
- ❌ **NOT** publish to NuGet.org

Only when you run the "Create Release" workflow will packages be published to NuGet.org.