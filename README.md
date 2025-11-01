# 💊 Projeto Medicamento  
**Participantes:** Gustavo Murai e Igor Murai  

## 📘 Descrição
Aplicação **console em C# (.NET)** que simula o controle de medicamentos e seus lotes usando estrutura de dados **fila (Queue)**.

O sistema permite cadastrar medicamentos, registrar lotes, vender com controle FIFO e listar informações de forma sintética ou analítica.

## 🧩 Estrutura das Classes
- **Lote:** armazena ID, quantidade e data de vencimento.  
- **Medicamento:** possui ID, nome, laboratório e fila de lotes.  
  - Métodos: `comprar()`, `vender()`, `qtdeDisponivel()`.  
- **Medicamentos:** gerencia uma lista de medicamentos e permite adicionar, pesquisar e remover (quando estoque = 0).


