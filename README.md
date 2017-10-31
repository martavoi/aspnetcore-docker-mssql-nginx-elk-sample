Dockerized ASP.NET Core WebAPI app

All-in-one Web API sample to get started with ASP.NET Core on Docker. Incorporates nginx reverse proxy with load balancer configuration along with advanced logging approach (serilog + elasticsearch + kibana).

## Getting Started

```{r, engine='bash', count_lines}
git clone git@github.com:martavoi/aspnetcore-docker-mssql-nginx-elk-sample.git
cd aspnetcore-docker-mssql-nginx-elk-sample
docker-compose -f docker-compose.prod.yml up --build
```
To initialize database run:

```{r, engine='bash', count_lines}
cd ./Oxagile.Demos.Data
dotnet restore
dotnet ef database update -s ../Oxagile.Demos.Api/Oxagile.Demos.Api.csproj
```

Navigate to [http://localhost](http://localhost) for API Documentation.

### Prerequisites

You have to install [Docker](https://docs.docker.com/engine/installation/) first to get things done. For development all that you need is [ASP.NET Core SDK](https://www.microsoft.com/net/download/core).

### Installing

There are two docker-compose configurations: minimal and production. Also, you can launch asp.net core app directly from your IDE (.vscode tasks.json provided) - ensure you have MSSQL up and running (with respect to appsettings.Development.json)

Since web api rely on EF Core Code-First Migrations, you need to update DB shcema once.

```{r, engine='bash', count_lines}
cd ./Oxagile.Demos.Data
dotnet restore
dotnet ef database update -s ../Oxagile.Demos.Api/Oxagile.Demos.Api.csproj
```

### Contributing

Any improvements are appreciated. Post a PR and feel free to discuss anything must be added/changed.

To update db schema using EF Core Migrations Tools, from within .\Oxagile.Demos.Data directory run:
```{r, engine='bash', count_lines}
dotnet ef migrations add <migration_name> -s ../Oxagile.Demos.Api/Oxagile.Demos.Api.csproj
```

## License

This project is licensed under the Apache License - see the [LICENSE](LICENSE) file for details