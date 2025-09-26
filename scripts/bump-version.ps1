#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Bumps the version number in the project file
.DESCRIPTION
    This script increments the version number in MDLSoft.StringParsers.csproj
    and optionally creates a git tag for the new version.
.PARAMETER VersionType
    The type of version bump: major, minor, or patch (default: patch)
.PARAMETER NewVersion
    Explicitly set a new version (overrides VersionType)
.PARAMETER CreateTag
    Create a git tag for the new version
.EXAMPLE
    ./scripts/bump-version.ps1 -VersionType patch
    ./scripts/bump-version.ps1 -NewVersion "1.2.0"
    ./scripts/bump-version.ps1 -VersionType minor -CreateTag
#>

param(
    [Parameter()]
    [ValidateSet("major", "minor", "patch")]
    [string]$VersionType = "patch",
    
    [Parameter()]
    [string]$NewVersion,
    
    [Parameter()]
    [switch]$CreateTag
)

$projectFile = "MDLSoft.StringParsers/MDLSoft.StringParsers.csproj"

if (-not (Test-Path $projectFile)) {
    Write-Error "❌ Project file not found: $projectFile"
    exit 1
}

# Read current version
$content = Get-Content $projectFile -Raw
if ($content -match '<Version>([^<]+)</Version>') {
    $currentVersion = $matches[1]
    Write-Host "📋 Current version: $currentVersion" -ForegroundColor Cyan
} else {
    Write-Error "❌ Could not find version in project file"
    exit 1
}

# Calculate new version
if ($NewVersion) {
    $newVer = $NewVersion
    Write-Host "🎯 Setting explicit version: $newVer" -ForegroundColor Green
} else {
    $versionParts = $currentVersion.Split('.')
    $major = [int]$versionParts[0]
    $minor = [int]$versionParts[1]  
    $patch = [int]$versionParts[2]
    
    switch ($VersionType) {
        "major" { 
            $major++; $minor = 0; $patch = 0
            Write-Host "🚀 Bumping major version" -ForegroundColor Green
        }
        "minor" { 
            $minor++; $patch = 0
            Write-Host "✨ Bumping minor version" -ForegroundColor Green
        }
        "patch" { 
            $patch++
            Write-Host "🔧 Bumping patch version" -ForegroundColor Green
        }
    }
    
    $newVer = "$major.$minor.$patch"
}

Write-Host "📈 New version: $newVer" -ForegroundColor Yellow

# Update project file
$newContent = $content -replace '<Version>[^<]+</Version>', "<Version>$newVer</Version>"
$newContent = $newContent -replace '<AssemblyVersion>[^<]+</AssemblyVersion>', "<AssemblyVersion>$newVer.0</AssemblyVersion>"
$newContent = $newContent -replace '<FileVersion>[^<]+</FileVersion>', "<FileVersion>$newVer.0</FileVersion>"

Set-Content $projectFile -Value $newContent -Encoding UTF8

Write-Host "✅ Updated project file with version $newVer" -ForegroundColor Green

# Create git tag if requested
if ($CreateTag) {
    $tagName = "v$newVer"
    
    Write-Host "🏷️ Creating git tag: $tagName" -ForegroundColor Cyan
    
    try {
        git tag -a $tagName -m "Release version $newVer"
        Write-Host "✅ Created tag: $tagName" -ForegroundColor Green
        Write-Host "💡 To push the tag: git push origin $tagName" -ForegroundColor Yellow
    }
    catch {
        Write-Warning "⚠️ Failed to create git tag: $($_.Exception.Message)"
    }
}

Write-Host "`n🎉 Version bump completed!" -ForegroundColor Green
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "  1. Review changes: git diff" -ForegroundColor White
Write-Host "  2. Commit changes: git add . && git commit -m 'chore: bump version to $newVer'" -ForegroundColor White
Write-Host "  3. Push to trigger publishing: git push origin main" -ForegroundColor White

if ($CreateTag) {
    Write-Host "  4. Push tag for release: git push origin v$newVer" -ForegroundColor White
}