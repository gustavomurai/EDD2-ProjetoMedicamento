using System;

namespace ProjetoMedicamento.Models
{
    public class Lote
    {
        public int Id { get; set; }
        public int Qtde { get; set; }
        public DateTime Venc { get; set; }

        public Lote() { }

        public Lote(int id, int qtde, DateTime venc)
        {
            Id = id;
            Qtde = qtde;
            Venc = venc;
        }

        public override string ToString()
        {
            // retornar id + "-" + qtde + "-" + venc
            return $"{Id}-{Qtde}-{Venc:dd/MM/yyyy}";
        }
    }
}
