# API Banco Digital
Esta API fornece servi�os para um banco digital.   
Ela permite a cria��o e consulta de contas banc�rias, bem como opera��es de transa��es como dep�sitos e saques.  
A API tamb�m permite consultar o hist�rico de transa��es e gerar relat�rio com o total de dep�sitos e total de saques realizados.

## Configura��es iniciais:
Requisitos: Docker Desktop  
[Download](https://www.docker.com/products/docker-desktop)

## Executando a aplica��o:
1. Inicie o Docker Desktop(no modo Administrador).
2. Clone o reposit�rio do projeto:  
Url: https://github.com/leandromattos/BancoDigital
3. Accesse a pasta onde o projeto foi clonado.
4. Execute o arquivo __start-docker.bat__ na pasta raiz da solu��o.    
``./BancoDigital/start-docker.bat``  
*__Obs.:__ Com isso o script ir� configurar o cen�rio, ir� criar o container, banco de dados e executar a aplica��o.  
Caso API n�o esteja funcional, acesse o diret�rio do projeto atrav�s do powerShell e verifique o log atrav�s do comando: ``docker logs bancodigitalapi-banco-digital-api-1``.
Provavelmente pode ser alguma vari�vel de sistema n�o configurada.*
8. As credenciais de acesso, est�o no arquivo __.env__ na raiz do projeto.
7. [Clique aqui para acessar API](http://localhost:8080/swagger/index.html)


## API Banco Digital
	http://localhost:8080/swagger/index.html

## Endpoints
O Swagger cont�m exemplo de requisi��es para cada endpoint.
E exemplo de retorno de cada requisi��o.

### 1. Criar Conta Banc�ria
__M�todo__: POST  
__Rota:__ ``/api/contas``  
__Descri��o:__ Cria uma nova conta banc�ria.  
__Request Body (Exemplo):__

	{
		"nomeCliente": "Jo�o da Silva",
		"cpfCliente": "123.456.789-00",
		"tipoConta": "Corrente"
	}


### 2. Obter Conta por ID
__M�todo:__ GET  
__Rota:__ /api/contas/{id}  
__Descri��o:__ Consulta uma conta banc�ria atrav�s do ID da conta.  
__Par�metros de Rota:__ ``{id}``

### 3. Listar Contas Banc�rias
__M�todo:__ GET  
__Rota:__ ``/api/contas``  
__Descri��o:__ Lista todas as contas banc�rias.  


### 4. Realizar Dep�sito
__M�todo:__ POST
__Rota:__ ``/api/transacoes/depositar``  
__Descri��o:__ Realiza dep�sito em uma conta banc�ria.  
__Request Body (Exemplo):__  
``{
  "contaId": 1,
  "valor": 500.00
}``

### 5. Realizar Saque
__M�todo:__ POST  
__Rota:__ ``/api/transacoes/sacar``  
__Descri��o:__ Realizar saque em uma conta banc�ria.  
__Request Body (Exemplo):__
``{
  "contaId": 1,
  "valor": 100.00
}``


### 6. Listar Hist�rico de Transa��es
__M�todo:__ GET  
__Rota:__ ``/api/transacoes/historico/{contaId}``  
__Descri��o:__ Consulta o hist�rico de transa��es (saques e dep�sitos) para uma conta banc�ria espec�fica.  
__Par�metros de Rota (Exemplo):__  
``contaId: ID da conta banc�ria.``  
``page: N�mero da p�gina (opcional, padr�o � 1).``  
``pageSize: N�mero de itens por p�gina (opcional, padr�o � 10).``


### 7. Gerar Relat�rio de Transa��es
__M�todo:__ GET  
__Rota:__ ``/api/transacoes/relatorio``  
__Descri��o:__ Gera um relat�rio com o total de saques e dep�sitos realizados em uma conta banc�ria, dentro de um intervalo de datas.  
__Par�metros de Query (Exemplo):__  
``dataInicio: Data de in�cio do per�odo (formato: yyyy-MM-dd).``  
``dataFim: Data de fim do per�odo (formato: yyyy-MM-dd).``

### Retornos
#### C�digos de Status
__200 OK:__ Solicita��o bem-sucedida.  
__400 Bad Request:__ Erro de valida��o ou outros erros de neg�cio.    
__404 Not Found:__ Recurso n�o encontrado.



