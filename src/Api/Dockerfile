# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copiar arquivos do projeto e restaurar dependências
COPY ["src/Api/Api.csproj", "API/"]
COPY ["src/Application/Application.csproj", "Application/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["src/Integrations/Integrations.csproj", "Integrations/"]
RUN dotnet restore "Api/Api.csproj"

# Copiar todo o código e publicar a aplicação
COPY . .
WORKDIR /src/API
RUN dotnet publish -c Release -o /out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Expõe a porta para o Railway (ex.: 5000)
EXPOSE 5000

# Comando para rodar a aplicação
ENTRYPOINT ["dotnet", "Api.dll"]