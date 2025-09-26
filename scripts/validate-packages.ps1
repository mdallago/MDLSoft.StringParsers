#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Validates NuGet packages without requiring signatures
.DESCRIPTION
    This script validates NuGet packages by checking their structure and metadata
    without requiring cryptographic signatures, avoiding NU3004 errors.
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$PackageDirectory
)

Write-Host "🔍 Validating NuGet packages in: $PackageDirectory" -ForegroundColor Cyan

$packageFiles = Get-ChildItem $PackageDirectory -Filter "*.nupkg" -Recurse

if ($packageFiles.Count -eq 0) {
    Write-Error "❌ No .nupkg files found in $PackageDirectory"
    exit 1
}

Write-Host "📦 Found $($packageFiles.Count) package(s) to validate" -ForegroundColor Green

foreach ($file in $packageFiles) {
    Write-Host "`n🔍 Validating: $($file.Name)" -ForegroundColor Yellow
    
    # Check file size
    if ($file.Length -lt 1KB) {
        Write-Error "❌ Package $($file.Name) is too small (less than 1KB)"
        exit 1
    }
    
    Write-Host "  ✅ Size check passed ($($file.Length) bytes)"
    
    # Validate package structure
    try {
        # Create a proper temporary directory with GUID to ensure uniqueness
        $tempDir = Join-Path ([System.IO.Path]::GetTempPath()) ([System.Guid]::NewGuid().ToString())
        New-Item -ItemType Directory -Path $tempDir -Force | Out-Null
        
        Write-Host "  📁 Using temp directory: $tempDir"
        
        # Verify temp directory was created
        if (-not (Test-Path $tempDir)) {
            throw "Failed to create temporary directory: $tempDir"
        }
        
        # Extract package to temporary directory
        Add-Type -AssemblyName System.IO.Compression.FileSystem
        [System.IO.Compression.ZipFile]::ExtractToDirectory($file.FullName, $tempDir)
        
        # Check for .nuspec file
        $nuspecFiles = Get-ChildItem $tempDir -Filter "*.nuspec" -Recurse
        if ($nuspecFiles.Count -eq 0) {
            throw "Missing .nuspec file"
        }
        
        Write-Host "  ✅ Package structure is valid"
        
        # Check for lib directory (should contain assemblies)
        $libDirs = Get-ChildItem $tempDir -Name "lib" -Directory -ErrorAction SilentlyContinue
        if ($libDirs) {
            $assemblies = Get-ChildItem (Join-Path $tempDir "lib") -Filter "*.dll" -Recurse -ErrorAction SilentlyContinue
            Write-Host "  ✅ Found $($assemblies.Count) assembly/assemblies"
        }
        
        # Parse and validate .nuspec content
        $nuspecContent = Get-Content $nuspecFiles[0].FullName -Raw
        if ($nuspecContent -match '<id>([^<]+)</id>') {
            $packageId = $matches[1]
            Write-Host "  ✅ Package ID: $packageId"
        }
        
        if ($nuspecContent -match '<version>([^<]+)</version>') {
            $packageVersion = $matches[1]
            Write-Host "  ✅ Package Version: $packageVersion"
        }
        
        # Clean up
        Remove-Item $tempDir -Recurse -Force -ErrorAction SilentlyContinue
        
        Write-Host "  ✅ Package $($file.Name) validation completed successfully" -ForegroundColor Green
        
    }
    catch {
        Write-Error "❌ Failed to validate package $($file.Name): $($_.Exception.Message)"
        
        # Clean up on error
        if (Test-Path $tempDir) {
            Remove-Item $tempDir -Recurse -Force -ErrorAction SilentlyContinue
        }
        
        exit 1
    }
}

Write-Host "`n🎉 All $($packageFiles.Count) package(s) validated successfully!" -ForegroundColor Green
Write-Host "✅ No signature validation required - packages are structurally valid" -ForegroundColor Cyan