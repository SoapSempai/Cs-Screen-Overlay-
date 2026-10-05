param(
    [Parameter(Mandatory=$true)]
    [string]$ProjectPath
)

$ErrorActionPreference = "Stop"

$packageRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectPath = (Resolve-Path $ProjectPath).Path

$stamp = Get-Date -Format "yyyyMMdd_HHmmss"
$backupPath = Join-Path $projectPath "TEST1_backup_$stamp"
New-Item -ItemType Directory -Path $backupPath | Out-Null

$filesToReplace = @(
    "TEST 1.csproj",
    "Program.cs",
    "OverlayForm.cs",
    "SafeInputController.cs",
    "Data\DesktopCapture.cs",
    "Data\AnalysisReport.cs",
    "Data\CharacterDetection.cs",
    "Data\ReferenceImageValidator.cs",
    "Data\CharacterReferenceStore.cs",
    "Data\CharacterAnalyzer.cs"
)

foreach ($relative in $filesToReplace) {
    $source = Join-Path $packageRoot $relative
    $destination = Join-Path $projectPath $relative

    if (Test-Path $destination) {
        $backupDestination = Join-Path $backupPath $relative
        $backupDir = Split-Path $backupDestination -Parent
        New-Item -ItemType Directory -Force -Path $backupDir | Out-Null
        Copy-Item $destination $backupDestination -Force
    }

    $destinationDir = Split-Path $destination -Parent
    New-Item -ItemType Directory -Force -Path $destinationDir | Out-Null
    Copy-Item $source $destination -Force
}

$oldDetector = Join-Path $projectPath "Data\VisualRegionAnalyzer.cs"
if (Test-Path $oldDetector) {
    $oldDetectorBackup = Join-Path $backupPath "Data\VisualRegionAnalyzer.cs"
    New-Item -ItemType Directory -Force -Path (Split-Path $oldDetectorBackup -Parent) | Out-Null
    Copy-Item $oldDetector $oldDetectorBackup -Force
    Remove-Item $oldDetector -Force
}

$oldProgramBackup = Join-Path $backupPath "README.txt"
"TEST 1 backup created before Character Analysis replacement." | Set-Content $oldProgramBackup

Write-Host ""
Write-Host "TEST 1 Character Analysis installed."
Write-Host "Backup: $backupPath"
Write-Host ""
Write-Host "Next:"
Write-Host "  cd `"$projectPath`""
Write-Host "  dotnet build"
Write-Host "  dotnet run"
