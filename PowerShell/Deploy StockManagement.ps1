# Variables
$projectPath = "C:\DRJ_Development\GitHub\DRJTechnology\StockManagement"
$timestamp = Get-Date -Format "yyyy-MM-dd-HHmm"
$imageTag = "stockmanagement:$timestamp"
$tarFile = "stockmanagement-$timestamp.tar"
$logFile = "deploy-log-$timestamp.txt"
$logFolder = "$projectPath\deploy-logs"
$remoteUser = "daveb"
$remoteHost = "192.168.1.91"
$remotePath = "/home/daveb"
$remoteDeployPath = "/opt/stockmanagement"

# Ensure log folder exists
if (!(Test-Path $logFolder)) {
    New-Item -ItemType Directory -Path $logFolder | Out-Null
}

# Start logging
Start-Transcript -Path "$logFolder\$logFile"

try {
    Write-Host "`n=== Building Docker Image: $imageTag ==="
    cd $projectPath
    docker build -t $imageTag -f StockManagement/StockManagement/Dockerfile .

    Write-Host "`n=== Saving Docker Image to TAR ==="
    docker save -o $tarFile $imageTag

    Write-Host "`n=== Copying TAR File to Server ==="
    scp $tarFile "$remoteUser@$remoteHost:$remotePath/"

    Write-Host "`n=== SSHing into Server to Load and Deploy ==="
    ssh "$remoteUser@$remoteHost" @"
      echo 'Loading Docker image...'
      sudo docker load -i $remotePath/$tarFile

      echo 'Switching to deployment directory...'
      cd $remoteDeployPath

      echo 'Running docker-compose...'
      sudo docker compose up -d

      echo 'Pruning unused Docker images...'
      sudo docker image prune -a -f

      echo 'Cleaning up old tar files...'
      find $remotePath -name 'stockmanagement-*.tar' -type f -mtime +7 -delete
"@

    Write-Host "`n=== Cleaning up local TAR file ==="
    Remove-Item $tarFile -Force

    Write-Host "`n=== Cleaning up old local TAR files (keep latest 3) ==="
    Get-ChildItem -Path $projectPath -Filter "stockmanagement-*.tar" | 
        Sort-Object LastWriteTime -Descending | 
        Select-Object -Skip 3 | 
        Remove-Item -Force

    Write-Host "`n=== Cleaning up old logs (keep latest 5) ==="
    Get-ChildItem -Path $logFolder -Filter "deploy-log-*.txt" | 
        Sort-Object LastWriteTime -Descending | 
        Select-Object -Skip 5 | 
        Remove-Item -Force

    Write-Host "`n✅ Deployment succeeded. Log saved to $logFolder\$logFile"
}
catch {
    Write-Error "`n❌ Deployment failed: $_"
}
finally {
    Stop-Transcript
}
