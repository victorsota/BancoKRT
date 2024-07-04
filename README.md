# Projeto de Gestão de Limites de Transações PIX

Este projeto em ASP.NET Core MVC é desenvolvido para gerenciar limites de transações PIX usando DynamoDB da AWS como banco de dados NoSQL.

## Tecnologias Utilizadas

- ASP.NET Core 3.1
- DynamoDB (AWS SDK for .NET)
- C#
- Razor Pages
- HTML/CSS/Bootstrap

## Funcionalidades Principais

1. **Cadastro de Limites**: Permite adicionar novos limites de transações PIX associados a CPF, conta e agência.
   
2. **Consulta e Edição**: Busca e atualiza limites existentes baseados em CPF ou conta.

3. **Exclusão Segura**: Remove limites de transações PIX com confirmação baseada no ID do registro.

4. **Validação de Dados**: Garante que os dados inseridos sigam regras de validação especificadas.

## Estrutura do Projeto

- `/Controllers`: Controladores da aplicação MVC.
- `/Models`: Modelos de dados, incluindo o `LimitePixModel` para representar limites de transações PIX.
- `/Views`: Interfaces de usuário em Razor Pages, incluindo formulários para adicionar, buscar, editar e excluir limites.

## Configuração

Certifique-se de ter configurado corretamente as credenciais da AWS no seu ambiente de desenvolvimento para acesso ao DynamoDB.


