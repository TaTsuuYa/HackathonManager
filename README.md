# Hackathon Manager

## Rapport de projet Hackathon Manager
rapport: [here](./Assets/repport.pdf)
## Endpoint documentation
documentation: [here](./Hackathonmanager.ws/README.md)

## Config

Connection string [appsettings.json](HackathonManager.ws/appsettings.json) file:
```json
"ConnectionString": {
	"DefaultConnection": "...",
}
```

## Database
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

## Run Project
Download packages
```bash
dotnet restore
```
Run Project
```bash
dotnet run --project HackathonManager.ws
```

## Endpoints

url: [swagger interface](http://localhost:5042/swagger/)

## Diagrams

### Class Diagram
![img](./Assets/class_diagram.png)

### Architecture Diagram
![img](./Assets/architecture_diagram.png)
