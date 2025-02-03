#!/bin/bash

echo "Iniciando o entrypoint.sh"

echo "Banco de dados => Checando status..."

# Esperar o banco de dados ficar dispon�vel
until pg_isready -h banco-digital-db -p 5432 -U postgres; do
  echo "Aguardando o banco de dados..."
  sleep 2
done

echo "Banco de dados => Pronto!"

# Mudar para o diret�rio onde o projeto est� localizado (ajuste se necess�rio)
cd /app

echo "Executando migracoes => Iniciando..."

# Aplica as migra��es automaticamente
dotnet ef database update --no-build --project /app/BancoDigitalAPI --context BancoContext

echo "Executando migracoes => Pronto!"

echo "Iniciando projeto..."
# Rodar o app
exec dotnet BancoDigitalAPI.dll