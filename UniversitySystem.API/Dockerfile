FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY EnrollmentApp.sln .
COPY UniversitySystem.API/UniversitySystem.API.csproj UniversitySystem.API/
COPY UniversitySystem.Application/UniversitySystem.Application.csproj UniversitySystem.Application/
COPY UniversitySystem.Domain/UniversitySystem.Domain.csproj UniversitySystem.Domain/
COPY UniversitySystem.Infrastructure/UniversitySystem.Infrastructure.csproj UniversitySystem.Infrastructure/
COPY UniversitySystem.Persistence/UniversitySystem.Persistence.csproj UniversitySystem.Persistence/

RUN dotnet restore

COPY . .
WORKDIR /src/UniversitySystem.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UniversitySystem.API.dll"]
