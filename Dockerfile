FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /SimbirREST_API
COPY . ./

RUN dotnet restore "./SimbirREST_API/SimbirREST_API.csproj"
RUN dotnet publish -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /SimbirREST_API
COPY --from=build-env /SimbirREST_API/out .
ENTRYPOINT ["dotnet", "SimbirREST_API.dll", "--ef-migrate"]
