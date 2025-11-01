using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoMedicamento.Models
{
    public class Medicamento
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Laboratorio { get; set; } = "";
        private readonly Queue<Lote> lotes = new Queue<Lote>();

        public Medicamento() { }

        public Medicamento(int id, string nome, string laboratorio)
        {
            Id = id;
            Nome = nome ?? "";
            Laboratorio = laboratorio ?? "";
        }

        // Expor leitura dos lotes para listagem analítica (sem permitir set externo)
        public IEnumerable<Lote> Lotes => lotes;

        // Retornar disponibilidade do medicamento em todos os lotes
        public int QtdeDisponivel()
        {
            int total = 0;
            foreach (var l in lotes) total += l.Qtde;
            return total;
        }

        // Colocar o lote comprado na fila de lotes (FIFO)
        public void Comprar(Lote lote)
        {
            if (lote == null) return;
            if (lote.Qtde <= 0) return;
            lotes.Enqueue(lote);
        }

        // Vender: abate do lote mais antigo e retira lotes zerados; retorna true/false
        public bool Vender(int qtde)
        {
            if (qtde <= 0) return false;

            int disponivel = QtdeDisponivel();
            if (disponivel < qtde) return false;

            int restante = qtde;

            // Enquanto eu precisar vender e houver lotes
            while (restante > 0 && lotes.Count > 0)
            {
                var frente = lotes.Peek();

                if (frente.Qtde <= restante)
                {
                    // Consome todo o lote e remove da fila
                    restante -= frente.Qtde;
                    lotes.Dequeue();
                }
                else
                {
                    // Consome uma parte do lote
                    frente.Qtde -= restante;
                    restante = 0;
                }
            }

            return true;
        }

        public override string ToString()
        {
            // retornar id + "-" + nome + "-" + laboratorio + "-" + qtdeDisponivel()
            return $"{Id}-{Nome}-{Laboratorio}-{QtdeDisponivel()}";
        }

        // Permitir comparação pelo id do medicamento
        public override bool Equals(object? obj)
        {
            if (obj is Medicamento other)
                return this.Id == other.Id;
            return false;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}
