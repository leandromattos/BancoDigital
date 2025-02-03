# API Banco Digital
Esta API fornece serviços para um banco digital.   
Ela permite a criação e consulta de contas bancárias, bem como operações de transações como depósitos e saques.  
A API também permite consultar o histórico de transações e gerar relatório com o total de depósitos e total de saques realizados.

## Configurações iniciais:
Requisitos: Docker Desktop  
[Download](https://www.docker.com/products/docker-desktop)

## Executando a aplicação:
1. Inicie o Docker Desktop(no modo Administrador).
2. Clone o repositório do projeto:  
Url: https://github.com/leandromattos/BancoDigital
3. Accesse a pasta onde o projeto foi clonado.
4. Execute o arquivo __start-docker.bat__ na pasta raiz da solução.    
``./BancoDigital/start-docker.bat``  
*__Obs.:__ Com isso o script irá configurar o cenário, irá criar o container, banco de dados e executar a aplicação.  
Caso API não esteja funcional, acesse o diretório do projeto através do powerShell e verifique o log através do comando: ``docker logs bancodigitalapi-banco-digital-api-1``.
Provavelmente pode ser alguma variável de sistema não configurada.*
8. As credenciais de acesso, estão no arquivo __.env__ na raiz do projeto.
7. [Clique aqui para acessar API](http://localhost:8080/swagger/index.html)


## API Banco Digital
	http://localhost:8080/swagger/index.html

## Endpoints
O Swagger contém exemplo de requisições para cada endpoint.
E exemplo de retorno de cada requisição.

### 1. Criar Conta Bancária
__Método__: POST  
__Rota:__ ``/api/contas``  
__Descrição:__ Cria uma nova conta bancária.  
__Request Body (Exemplo):__

	{
		"nomeCliente": "João da Silva",
		"cpfCliente": "123.456.789-00",
		"tipoConta": "Corrente"
	}


### 2. Obter Conta por ID
__Método:__ GET  
__Rota:__ /api/contas/{id}  
__Descrição:__ Consulta uma conta bancária através do ID da conta.  
__Parâmetros de Rota:__ ``{id}``

### 3. Listar Contas Bancárias
__Método:__ GET  
__Rota:__ ``/api/contas``  
__Descrição:__ Lista todas as contas bancárias.  


### 4. Realizar Depósito
__Método:__ POST
__Rota:__ ``/api/transacoes/depositar``  
__Descrição:__ Realiza depósito em uma conta bancária.  
__Request Body (Exemplo):__  
``{
  "contaId": 1,
  "valor": 500.00
}``

### 5. Realizar Saque
__Método:__ POST  
__Rota:__ ``/api/transacoes/sacar``  
__Descrição:__ Realizar saque em uma conta bancária.  
__Request Body (Exemplo):__
``{
  "contaId": 1,
  "valor": 100.00
}``


### 6. Listar Histórico de Transações
__Método:__ GET  
__Rota:__ ``/api/transacoes/historico/{contaId}``  
__Descrição:__ Consulta o histórico de transações (saques e depósitos) para uma conta bancária específica.  
__Parâmetros de Rota (Exemplo):__  
``contaId: ID da conta bancária.``  
``page: Número da página (opcional, padrão é 1).``  
``pageSize: Número de itens por página (opcional, padrão é 10).``


### 7. Gerar Relatório de Transações
__Método:__ GET  
__Rota:__ ``/api/transacoes/relatorio``  
__Descrição:__ Gera um relatório com o total de saques e depósitos realizados em uma conta bancária, dentro de um intervalo de datas.  
__Parâmetros de Query (Exemplo):__  
``dataInicio: Data de início do período (formato: yyyy-MM-dd).``  
``dataFim: Data de fim do período (formato: yyyy-MM-dd).``

### Retornos
#### Códigos de Status
__200 OK:__ Solicitação bem-sucedida.  
__400 Bad Request:__ Erro de validação ou outros erros de negócio.    
__404 Not Found:__ Recurso não encontrado.



