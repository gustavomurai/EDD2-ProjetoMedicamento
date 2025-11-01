using System.Collections.Generic;
using System.Linq;

namespace ProjetoMedicamento.Models
{
    public class Medicamentos
    {
        private readonly List<Medicamento> listaMedicamentos = new List<Medicamento>();

        public Medicamentos() { }

        // Adicionar o medicamento na lista
        public void Adicionar(Medicamento medicamento)
        {
            if (medicamento == null) return;
            // Evitar duplicidade por Id
            if (listaMedicamentos.Any(m => m.Id == medicamento.Id)) return;

            listaMedicamentos.Add(medicamento);
        }

        // Remover somente se quantidade disponível for 0
        public bool Deletar(Medicamento medicamento)
        {
            var existente = listaMedicamentos.FirstOrDefault(m => m.Id == medicamento.Id);
            if (existente == null) return false;

            if (existente.QtdeDisponivel() == 0)
            {
                listaMedicamentos.Remove(existente);
                return true;
            }

            return false;
        }

        // Pesquisar pelo id; se achar retorna completo; se não, retorna objeto vazio
        public Medicamento Pesquisar(Medicamento medicamento)
        {
            var encontrado = listaMedicamentos.FirstOrDefault(m => m.Id == medicamento.Id);
            return encontrado ?? new Medicamento(); // “objeto vazio” por construtor padrão
        }

        // Suporte para listagem no menu
        public IEnumerable<Medicamento> ListarTodos() => listaMedicamentos;
    }
}
