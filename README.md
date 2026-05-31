# DeliveryFlow.API

# **Descrição do projeto**
- Este projeto foi desenvolvido para atender às necessidades da empresa fictícia TechsysLog, especializada em logística e gerenciamento de entregas. A aplicação tem como objetivo realizar o controle completo de pedidos e entregas, permitindo o cadastro de usuários, pedidos e registros de entrega através de uma API desenvolvida em ASP.NET Core.

# **Solução**
- API REST desenvolvida em .NET 8.0, utilizando Entity Framework Core e os padrões Unit of Work e Repository, com suporte a validações, tratamento de erros, logging e documentação via Swagger.

## Documentação
- [Desafio Técnico](docs/Teste Prático-Desenvolvedor-PedroIghorHolandaPucci.pdf)

## **Estrutura do Projeto**

- A aplicação foi organizada em camadas para promover separação de responsabilidades, manutenibilidade, testabilidade e escalabilidade.

### Camadas da aplicação
- **API**: Controllers, middlewares, autenticação, Swagger e configuração da aplicação.
- **Application**: Regras de negócio, services, DTOs, validações e contratos.
- **Domain**: Entidades, enums e regras centrais do domínio.
- **Infrastructure**: Persistência de dados, Entity Framework Core, Identity e integrações externas.
- **Shared**: Classes compartilhadas, helpers, responses e utilitários comuns.

## Observabilidade
A aplicação possui:
- Serilog
- Health Checks
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
   - Isso criará e configurará o banco de dados no Microsoft SQL Server.
---
### **2. Executando o Projeto**
1. Abra o projeto no Visual Studio 2022.
2. Configure o projeto principal para execução:
   - Clique com o botão direito no projeto **DeliveryFlow.API** e selecione `Set as Startup Project`.
3. Clique no botão **HTTPS** no menu superior para iniciar a aplicação.

### **Banco de Dados**
- **Centralização de Exceções:**  
  Implementada a classe `ExceptionMiddleware` para unificar o tratamento de erros no sistema.
- **Alterações Realizadas:**  
  Ajustadas as classes `Program` e `RepositoryUoW` para integrar o middleware.
- **Mensagens de Erro:**  
  - Se o banco de dados não existir, os endpoints retornam:  
    ```text
    The database is currently unavailable. Please try again later.
    ```
  - Para erros inesperados na criação do banco, é exibido:  
    ```text
    An unexpected error occurred. Please contact support if the problem persists.
    ```
---
### **Configuração do Log**
- O sistema gera logs diários com informações sobre os processos executados no projeto.
- O log será salvo no diretório:  
  `C://Users//User//Downloads//Gerenciador-Livraria`.  
  **Nota**: É necessário criar a pasta manualmente nesse caminho ou alterar o diretório no código, caso deseje personalizá-lo.

  **Formato do arquivo de log criado**:
- Arquivo diário com informações estruturadas.
---
### **Finalização**
- Após seguir as etapas anteriores, o sistema será iniciado, e uma página com a interface **Swagger** será aberta automaticamente no navegador configurado no Visual Studio. Essa página permitirá explorar e testar os endpoints da API.
---
