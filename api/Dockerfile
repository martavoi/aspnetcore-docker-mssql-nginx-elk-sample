FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

COPY . ./
WORKDIR /app/Oxagile.Demos.Api
RUN dotnet publish -c Release -o build

FROM microsoft/aspnetcore:2.0

# RUN apt-get update \
#     && apt-get install -y --no-install-recommends \
#     procps

WORKDIR /app
RUN mkdir media
COPY --from=build-env /app/Oxagile.Demos.Api/build .

# RUN groupadd -r oxagile.api \
#     && useradd --no-log-init -r -g oxagile.api oxagile.api \
#     && chown -R oxagile.api:oxagile.api /app
# USER oxagile.api

EXPOSE 80

ENTRYPOINT ["dotnet", "Oxagile.Demos.Api.dll"]