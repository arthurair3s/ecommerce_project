# language: pt

Funcionalidade: Produtos do E-commerce
  Como um usuário do e-commerce
  Quero executar todos as ações possiveis
  Para gerenciar meus produtos eficientemente
  Ações: POST, GET, PATCH, PUT e DELETE

@POST
Cenário: a criação de um produto deve ser bem-sucedida
	Dado que eu tenho um produto valido:
		| Model  | ReleaseDate | Specifications | Price  | StockQuantity |    Type    |
		| IPhone | 2024-01-15  | 128GB, 6GB RAM | 999.99 |            50 | Smartphone |
	Quando eu envio uma requisição POST para "/api/products"
	Então a resposta deve ter o código de status 201
	E o corpo da resposta deve conter os detalhes do produto criado

@GET
Cenário: a listagem de produtos deve ser bem-sucedida
	Dado que existem produtos cadastrados no sistema
	Quando eu envio uma requisição GET para "/api/products"
	Então a resposta deve ter o código de status 200
	E o corpo da resposta deve conter a lista de produtos cadastrados

@GET
Cenário: a obtenção de um produto por ID deve ser bem-sucedida
	Dado que existe um produto com ID <id> no sistema
	Quando eu envio uma requisição GET para "/api/products/1"
	Então a resposta deve ter o código de status 200
	E o corpo da resposta deve conter os detalhes do produto com ID <id>

	Exemplos: 
		| id |
		| 3  |
		| 4  |
		| 7  |

@UPDATE
Cenário: a atualização completa de um produto deve ser bem-sucedida