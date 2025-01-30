# Fase de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Instala dependências necessárias
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev

# Define a variável de configuração de build (Release ou Debug)
ARG BUILD_CONFIGURATION=Release

# Define o diretório de trabalho dentro do container
WORKDIR /src

# Copia apenas os arquivos .csproj necessários para o build da API
COPY ["src/Api/Api.csproj", "Api/"]
COPY ["src/Application/Application.csproj", "Application/"]
COPY ["src/Integrations/Integrations.csproj", "Integrations/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restaura as dependências para o projeto Api
WORKDIR "/src/Api"
RUN dotnet restore "Api.csproj"

# Copia os arquivos de código fonte para dentro do container (incluindo os projetos necessários)
COPY ["src/Application", "Application/"]
COPY ["src/Integrations", "Integrations/"]
COPY ["src/Api", "Api/"]
COPY ["src/Domain", "Domain/"]
COPY ["src/Infrastructure", "Infrastructure/"]

# Realiza o build do projeto Api
RUN dotnet build "Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Fase de publicação
FROM build AS publish
RUN dotnet publish "Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

# Fase final - imagem para execução
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS final

# Define o diretório de trabalho para a aplicação
WORKDIR /app

# Exponha a porta que o aplicativo vai usar
EXPOSE 8080

# Copia os arquivos da fase de publicação para o container final
COPY --from=publish /app/publish .

# Define o comando que será executado para iniciar a aplicação
ENTRYPOINT ["dotnet", "Api.dll"]
