# MinhaLoja - Back end
MinhaLoja tem por intuito demonstrar uma opção para criação de um back-end para o seu sistema. Construída utilizando diversos padrões de mercado, e principalmente valendo-se dos conceitos do DDD para uma melhor organização das implementações de regras de negócio. Código que tem por base a plataforma .NET e linguagem C#.

### Organização Código
- Domain: Core do sistema; regras de negócio;
- Infra: Auxilia o domínio a buscar/gravar registros no banco de dados;
- Workers: Aplicações que são executados em backgrounds: receber mensagens de um Message Broker, temporizadores, etc.
- Apis: Serviços REST;
- Tests: testes unitários, integração, etc.

Uma api/worker consomem o domínio através do envio de Requests, que estão presentes em *ApplicationServices*. AppService orquestra a chamada a Repositories, Entidades, Queries, Events e retorna sucesso ou falha após realiza uma determinada operação. 

### Frameworks/Libs
- .NET 5
- ASP.NET
- Entity Framework
- Redis
- Application Insights*
- Azure Service Bus*
- Azure Storage
- Azure Functions
- Worker Service
- MimeKit
- MediatR
- SQL Server
- MongoDB
- xUnit
- Docker

*necessário ter uma conta junto ao Azure

### Design/Padrões Arquiteturais
- DDD
- Unit of Work
- Repository
- Inversão de Controle/Injeção de dependência
- JWT
- CQRS
- Event Sourcing

### Implementação: Configurações
- docker-compose
via terminal: docker-compose up -d

- appsettings.json
utilizar appsettings.EXAMPLE.json para a criação das configurações em Apis e Works.

### Migrations
- executar migrations em: 
src\MinhaLoja.Infra.Data (Update-Database -Context MinhaLojaContext)
src\MinhaLoja.Infra.Api.Identity (Update-Database -Context IdentityMinhaLojaContext) 

### Requisições Api
'MinhaLoja - AdminLoja.postman_collection.json' contém as configurações para realização de requisições via Postman.

### Regras de Negócio
- Possibilitar que vendedores possam se cadastrar e anunciar seus produtos.
- Também haverá a possibilidade de se cadastrar Marcas e Tipo de Produtos (Eletrodoméstico, Refrigerador, TV, etc.).
- O vendedor só terá acesso ao sistema após seu cadastro ser aprovado por um Usuário Master.
- Erros que ocorrerem durante requisições deverão ser logados.
