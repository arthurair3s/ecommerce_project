# language: pt

Funcionalidade: Produtos do E-commerce
  Como um usuário do e-commerce
  Quero executar todos as ações possiveis
  Para gerenciar meus produtos eficientemente
  Ações: POST, GET, PATCH, PUT e DELETE

Cenário: a criação de um produto deve ser bem-sucedida
	Dado que eu tenho um produto valido:
		| Model  | ReleaseDate | Specifications | Price  | StockQuantity |    Type    |
		| IPhone | 2024-01-15  | 128GB, 6GB RAM | 999.99 |            50 | Smartphone |
	Quando eu envio uma requisição POST para "/products"
	Então a resposta deve ter o código de status 201
	E o corpo da resposta deve conter os detalhes do produto criado
