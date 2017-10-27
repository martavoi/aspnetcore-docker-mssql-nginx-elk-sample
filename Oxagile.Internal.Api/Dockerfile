FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

COPY . ./
WORKDIR /app
RUN dotnet publish -c Release -o build

FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/build .

EXPOSE 5000

ENTRYPOINT ["dotnet", "Oxagile.Internal.Api.dll"]