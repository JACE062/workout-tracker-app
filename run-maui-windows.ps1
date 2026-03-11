param(
    [switch]$NoBuild
)

$ErrorActionPreference = 'Stop'

# MAUI Windows app can linger and lock the output exe, causing MSB3027/MSB3021 on next build.
Get-Process WorkoutTrackerApp -ErrorAction SilentlyContinue | Stop-Process -Force
Get-CimInstance Win32_Process -Filter "name = 'dotnet.exe'" |
    Where-Object { $_.CommandLine -match 'WorkoutTrackerApp|workout-tracker-app' } |
    ForEach-Object { Stop-Process -Id $_.ProcessId -Force }

$args = @(
    'run'
    '--project', 'WorkoutTrackerApp'
    '--framework', 'net9.0-windows10.0.19041.0'
)

if ($NoBuild) {
    $args += '--no-build'
}

dotnet @args
