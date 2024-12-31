# language: pt

@integration-test
Funcionalidade: Registrar Novo Cliente
  Como um administrador do sistema
  Eu quero ser capaz de registrar novos clientes
  Para poder gerenciar os clientes do meu e-commerce

Cenário: 001 - Registrar um novo cliente com sucesso
  Dado o payload para o registro de cliente:
      | first-name    | last-name      | birth-date | email               |  
      | Marcelo | Castelo Branco | 2000-12-31 | contato@marcelocastelo.io |
  Quando a request de registro de cliente eh feita
  Então o status code da response de registro de cliente deve ser 201
  E o header deve possuir a chave Location para o cliente registrado
  E a response da request de registro do cliente deve condizer com o payload
  E o cliente retornado na Location URI deve ser o mesmo que foi registrado
  E a response não deve conter mensagens de retorno
