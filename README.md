# DeliveryFlow.API

# **Descrição do projeto**
O DeliveryFlow.API foi desenvolvido para atender às necessidades da empresa fictícia TechsysLog, especializada em logística e gerenciamento de entregas.
A aplicação tem como objetivo realizar o controle completo de pedidos e entregas, permitindo o cadastro de usuários, pedidos e registros de entrega através de uma API desenvolvida em ASP.NET Core.

# **Solução**
API REST desenvolvida com .NET 8, utilizando Entity Framework Core e os padrões Repository e Unit of Work, com suporte a:
- Autenticação JWT
- Validações de entrada
- Tratamento global de exceções
- Logging estruturado
- Documentação via Swagger
- Testes unitários com xUnit

## Documentação
- [Desafio Técnico](docs/TestePrático-Desenvolvedor-PedroIghorHolandaPucci.pdf)

## Estrutura do Projeto

A aplicação foi organizada em camadas com o objetivo de promover separação de responsabilidades, manutenibilidade, testabilidade e escalabilidade.

### Camadas da aplicação

- **API**: Responsável pelos controllers, autenticação JWT, configuração da aplicação, middlewares e documentação Swagger.
- **Application**: Contém regras de negócio, services, DTOs, validações e contratos/interfaces da aplicação.
- **Domain**: Camada central do domínio, contendo entidades, objetos compartilhados e regras de negócio principais.
- **Infrastructure**: Responsável pela persistência de dados, Entity Framework Core, Identity, migrations, repositories e integrações externas.
- **Shared**: Contém utilitários, helpers, logs, responses e classes compartilhadas entre as camadas.
- **Tests**: Projeto destinado aos testes unitários da aplicação utilizando xUnit.

## Observabilidade
A aplicação possui:
- Serilog
- Middleware global de exceções

## **Como Executar o Projeto**
### **1. Configuração Inicial do Banco de Dados**
1. Faça o clone do projeto.
2. Verifique se a pasta `Migrations` no projeto está vazia. Caso contrário, delete todos os arquivos dessa pasta.   
3. Execute os seguintes comandos no **Package Manager Console**:
   - Certifique-se de selecionar o projeto relacionado ao banco de dados no menu "Default project".
   - Execute:
     ```bash
     add-migration PrimeiraMigracao
     update-database
     ```
   Esses comandos irão criar e configurar o banco de dados no SQL Server..
---
### **2. Executando a Aplicação**
1. Abra o projeto no Visual Studio 2022.
2. Configure o projeto principal para execução:
   - Clique com o botão direito no projeto **DeliveryFlow.API** e selecione `Set as Startup Project`.
3. Clique no botão **HTTPS** no menu superior para iniciar a aplicação.

### **Tratamento de Exceções**
Foi implementado um middleware global chamado ExceptionMiddleware para centralizar o tratamento de erros da aplicação.

**Mensagens Tratadas**  
  Ajustadas as classes `Program` e `RepositoryUoW` para integrar o middleware.
- **Mensagens de Erro:**  
  - Banco de dados indisponível:  
    ```text
    The database is currently unavailable. Please try again later.
    ```
  - Erros inesperados:  
    ```text
    An unexpected error occurred. Please contact support if the problem persists.
    ```
---
### **Configuração do Log**
- O sistema gera logs diários com informações sobre os processos executados no projeto.
- O log será salvo no diretório:  
  `C://Users//User//Downloads//ProcessoSeletivo-Logs`.  
  **Nota**: É necessário criar a pasta manualmente nesse caminho ou alterar o diretório no código, caso deseje personalizá-lo.

  **Formato**:
  - Logs estruturados
  - Arquivos gerados diariamente
---
### **Finalização**
- Após seguir todas as etapas anteriores, a aplicação estará disponível juntamente com a interface **Swagger** para testes e exploração dos endpoints da API..
---
