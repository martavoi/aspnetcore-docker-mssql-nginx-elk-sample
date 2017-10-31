Dockerized ASP.NET Core WebAPI app

All-in Web API sample to get started with ASP.NET Core on Docker. Incorporates nginx reverse proxy with load balancer configuration along with advanced logging approach (serilog + elasticsearch + kibana).

## Getting Started

```{r, engine='bash', count_lines}
git clone git@github.com:martavoi/aspnetcore-docker-mssql-nginx-elk-sample.git
cd aspnetcore-docker-mssql-nginx-elk-sample
docker-compose -f docker-compose.prod.yml up --build
dotnet restore
dotnet ef database update
```

Navigate to [http://localhost](http://localhost) for API Documentation.

### Prerequisites

You have to install [Docker](https://docs.docker.com/engine/installation/) first to get things done. For development all that you need are [ASP.NET Core SKD](https://www.microsoft.com/net/download/core).

### Installing

There are two docker-compose configurations: minimal and production. Also, you can launch asp.net core app directly from your IDE (.vscode tasks.json provided) - ensure you have MSSQL up and running (with respect to appsettings.Development.json)

Since web api rely on EF Core Code-First Migrations, you need to update DB shcema once.

```{r, engine='bash', count_lines}
dotnet restore
dotnet ef database update
```

## License

This project is licensed under the Apache License - see the [LICENSE](LICENSE) file for details