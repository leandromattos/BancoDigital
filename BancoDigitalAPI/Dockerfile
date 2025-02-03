# Estágio 1: Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o projeto e restaurar dependências
COPY BancoDigitalAPI.csproj ./BancoDigitalAPI.csproj

# Restaurar as dependências
RUN dotnet restore BancoDigitalAPI.csproj

# Copiar o restante do código e compilar
COPY . ./
RUN dotnet publish -c Release -o /app --no-restore

# Instalar a ferramenta dotnet-ef
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Estágio 2: Execução da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime

# Instalar PostgreSQL (para usar o pg_isready)
RUN apt-get update && apt-get install -y postgresql-client

# Copiar os arquivos publicados
WORKDIR /app
COPY --from=build /app .

# Copiar o script entrypoint.sh para o container
#COPY entrypoint.sh /app/entrypoint.sh

# Expor as portas 80 e 443
EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Development

#RUN chmod +x /app/entrypoint.sh

# Definir o comando de execução
#ENTRYPOINT ["/bin/bash", "/app/entrypoint.sh"]
ENTRYPOINT ["dotnet", "BancoDigitalAPI.dll"]