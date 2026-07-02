# Hackathon Manager

## Config

Connection string [appsettings.json](HackathonManager.ws/appsettings.json) file:
```json
"ConnectionString": {
	"DefaultConnection": "...",
}
```

Sql Server Docker container:
- bash
```bash
docker run -d \
  --name sqlserver \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" \
  -p 1433:1433 \
  -v sqlserver_data:/var/opt/mssql \
  mcr.microsoft.com/mssql/server:2022-latest
```
- powershell
```ps
docker run -d --name sqlserver -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 -v sqlserver_data:/var/opt/mssql mcr.microsoft.com/mssql/server:2022-latest
```
