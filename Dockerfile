# Étape 1 : Utiliser l'image officielle .NET SDK pour construire l'application
FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:35792ea4ad1db051981f62b313f1be3b46b1f45cadbaa3c288cd0d3056eefb83 AS build-env
WORKDIR /App

# Copier les fichiers .csproj et restaurer les dépendances
COPY ./Application/Application.csproj ./Application/
COPY ./Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY ./Domain/Domain.csproj ./Domain/
COPY ./Presentation/Presentation.csproj ./Presentation/

RUN dotnet restore ./Presentation/Presentation.csproj

# Copier le reste des fichiers et construire l'application
COPY ./Application ./Application/
COPY ./Infrastructure ./Infrastructure/
COPY ./Domain ./Domain/
COPY ./Presentation ./Presentation/

RUN dotnet publish ./Presentation/Presentation.csproj -c Release -o out

# Étape 2 : Utiliser l'image officielle .NET Runtime pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:6c4df091e4e531bb93bdbfe7e7f0998e7ced344f54426b7e874116a3dc3233ff
WORKDIR /App
COPY --from=build-env /App/out .

# Définir le point d'entrée de l'application
ENTRYPOINT ["dotnet", "Presentation.dll"]
