FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

COPY ["src/DemoWebApi/.", "DemoWebApi/"]

RUN dotnet build "DemoWebApi/DemoWebApi.csproj" -c Release -o /app/build

RUN dotnet publish "DemoWebApi/DemoWebApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

RUN addgroup --group serviceuser --gid 2000 \
    && adduser \    
    --uid 1000 \
    --gid 2000 \
    "serviceuser"
RUN chown serviceuser:serviceuser /app
USER serviceuser:serviceuser

COPY --from=build-env /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "DemoWebApi.dll"]