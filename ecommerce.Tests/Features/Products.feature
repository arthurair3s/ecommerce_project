# language: pt

Funcionalidade: Produtos do E-commerce
  Como um usuário do e-commerce
  Quero executar todos as ações possiveis
  Para gerenciar meus produtos eficientemente
  Ações: POST, GET, PATCH, PUT e DELETE

@POST
Cenário: a criação de um produto deve ser bem-sucedida
	Dado que eu recebo um produto valido:
		| Model  | ReleaseDate | Specifications | Price  | StockQuantity |    Type    |
		| IPhone | 2024-01-15  | 128GB, 6GB RAM | 999.99 |            50 | Smartphone |
	Quando eu envio uma requisição POST para "/api/products"
	Então a resposta deve ter o código de status 201
	E o corpo da resposta deve conter os detalhes do produto criado

@GET
Cenário: a listagem de produtos deve ser bem-sucedida
	Dado que eu tenho produtos validos:
		|  Model  | ReleaseDate | Specifications | Price  | StockQuantity |    Type    |
		| Pixel 6 | 2021-10-28  | 128GB, 8GB RAM | 599.99 |            40 | Smartphone |
	Quando eu envio uma requisição GET para "/api/products"
	Então a resposta deve ter o código de status 200
	E o corpo da resposta deve conter a lista de produtos cadastrados

@GET
Cenário: a listagem de um produto específico deve ser bem-sucedida
	Dado que eu tenho um produto valido:
		| Model      | ReleaseDate | Specifications   | Price   | StockQuantity |    Type    |
		| Galaxy S21 | 2021-01-29  | 128GB, 8GB RAM   | 799.99  |            30 | Smartphone |
	Quando eu envio uma requisição GET para "/api/products/<id>"
	Então a resposta deve ter o código de status 200
	E o corpo da resposta deve conter os detalhes do produto cadastrado

@UPDATE
Cenário: a atualização completa de um produto deve ser bem-sucedida
	Dado que eu tenho um produto valido:
		|  Model | ReleaseDate | Specifications | Price  | StockQuantity |    Type    |
		| Moto G | 2020-02-20  | 64GB, 4GB RAM  | 299.99 |            20 | Smartphone |
	E eu recebo os novos dados do produto:
		| Model | ReleaseDate | Specifications | Price  | StockQuantity |    Type    |
		| Edit  | 2020-02-20  | 64GB, 4GB RAM  | 99.99  |            20 | Smartphone |
	Quando eu envio uma requisição PUT para "/api/products/<id>"
	Então a resposta deve ter o código de status 200
	E o corpo da resposta deve conter os detalhes atualizados do produto