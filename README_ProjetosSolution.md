## Domain

### Domain (MinhaLoja.Domain)
O domínio está organizado por *Bounded Context*. Os itens que compõem cada Bounded Context são:

- ApplicationServices: Expõe as funcionalidades a ser consumidas por camadas superiores; é a porta de entrada ao Domínio; Orquestra a chamada a Repositories, Entities, Queries, realiza *Commits*, dispara Events, dentre outros.

- Entities: As entidades estão divididas por AggregateRoot e Aggregate (https://martinfowler.com/bliki/DDD_Aggregate.html); Cada entidade é responsável por alterar seus dados. Sendo assim suas propriedades são privadas.

- Repositories: Acesso a fontes de dados; Cada Repository herda implementações genéricas para realizar *CRUD*.

- Queries: Regras para realizar determinadas tarefas. Exemplo: não poderá haver Produtos com mesmo nome, pertencentes a uma mesma Marca e um mesmo Vendedor.

- Events: Todas as vezes que o estado de uma entidade alterar, ou uma nova entidade for criada, um evento deverá ser disparado.

- EventsHandlers: Trata os eventos disparados.

Todos os itens descritos anteriormente estão divididos internamente por entidades do tipo (*somente*) AggregateRoot. 

### Core (MinhaLoja.Core)
Agrupa abstrações, validações genéricas e configurações que serão utilizadas pelas demais camadas da Solution; Shared Kernel.



## Apis

### Identity (MinhaLoja.Api.Identity)
Responsável por centralizar a autenticação, geração e validação de tokens, utilizados pelas demais Apis

### Api.AdminLoja (MinhaLoja.Api.AdminLoja)
Expõem endpoints utilizados por Usuário Vendedor e Usuário Master para a manutenção da Loja Virtual.



## Workers

### Worker.DomainEventsReceiver (MinhaLoja.Worker.DomainEventsReceiver)
Recebe os eventos disparados por Domain e os envia a Handlers. Os Eventos são oriundos de um Message Broker.



## Infra

### Infra.Api (MinhaLoja.Infra.Api)
Implementações base para projetos do tipo Api.

### Infra.Api.Identity (MinhaLoja.Infra.Api.Identity)
Implementações base para o projeto *MinhaLoja.Api.Identity*.

### Infra.Ioc (MinhaLoja.Infra.Ioc)
Referencia todas as demais *Class Libraries* e mapeia as dependências.

### Infra.Data (MinhaLoja.Infra.Data)
Acessa fontes de dados utilizadas pelo sistema. Implementa os Repositories definidos em Domain.

### Infra.ServiceBus (MinhaLoja.Infra.ServiceBus)
Implementações referentes ao envio/recebimento de mensagens de Message Brokers.

### Infra.Services (MinhaLoja.Infra.Services)
Demais serviços utilizados pelos projetos da Solution.
