Change Execution Policy for Current Session Only

	Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

Run the script (From the directory it's in)
	.\DeployStockManagement.ps1



sudo docker load -i stockmanagement-latest.tar
cd /opt/stockmanagement/
sudo docker compose up -d
sudo docker image prune -a -f
