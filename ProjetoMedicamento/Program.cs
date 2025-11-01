using System;
using System.Globalization;
using ProjetoMedicamento.Models;

namespace ProjetoMedicamento
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var banco = new Medicamentos();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== PROJETO MEDICAMENTO ===");
                Console.WriteLine("0. Finalizar processo");
                Console.WriteLine("1. Cadastrar medicamento");
                Console.WriteLine("2. Consultar medicamento (sintético)");
                Console.WriteLine("3. Consultar medicamento (analítico)");
                Console.WriteLine("4. Comprar medicamento (cadastrar lote)");
                Console.WriteLine("5. Vender medicamento (abater do lote mais antigo)");
                Console.WriteLine("6. Listar medicamentos (dados sintéticos)");
                Console.WriteLine("---------------------------");

                int opcao = LerInt("Escolha uma opção: ");

                switch (opcao)
                {
                    case 0:
                        Console.WriteLine("Finalizando...");
                        return;

                    case 1:
                        CadastrarMedicamento(banco);
                        break;

                    case 2:
                        ConsultarSintetico(banco);
                        break;

                    case 3:
                        ConsultarAnalitico(banco);
                        break;

                    case 4:
                        ComprarLote(banco);
                        break;

                    case 5:
                        VenderMedicamento(banco);
                        break;

                    case 6:
                        ListarSintetico(banco);
                        break;

                    default:
                        Console.WriteLine("Opção inválida.");
                        Pausar();
                        break;
                }
            }
        }

        // --------- Ações do Menu ---------

        static void CadastrarMedicamento(Medicamentos banco)
        {
            Console.WriteLine("\n-- Cadastrar Medicamento --");
            int id = LerInt("ID: ");
            Console.Write("Nome: ");
            string nome = Console.ReadLine() ?? "";
            Console.Write("Laboratório: ");
            string lab = Console.ReadLine() ?? "";

            var med = new Medicamento(id, nome, lab);
            banco.Adicionar(med);

            Console.WriteLine("Medicamento cadastrado (se o ID não existia).");
            Pausar();
        }

        static void ConsultarSintetico(Medicamentos banco)
        {
            Console.WriteLine("\n-- Consulta Sintética --");
            int id = LerInt("ID do medicamento: ");

            var buscado = banco.Pesquisar(new Medicamento { Id = id });
            if (buscado.Id == 0 && string.IsNullOrEmpty(buscado.Nome) && string.IsNullOrEmpty(buscado.Laboratorio))
            {
                Console.WriteLine("Medicamento NÃO encontrado.");
            }
            else
            {
                // Dados do medicamento: ID + NOME + LABORATÓRIO + QTDE DISPONÍVEL
                Console.WriteLine($"ID: {buscado.Id}");
                Console.WriteLine($"Nome: {buscado.Nome}");
                Console.WriteLine($"Laboratório: {buscado.Laboratorio}");
                Console.WriteLine($"Qtde disponível: {buscado.QtdeDisponivel()}");
            }
            Pausar();
        }

        static void ConsultarAnalitico(Medicamentos banco)
        {
            Console.WriteLine("\n-- Consulta Analítica --");
            int id = LerInt("ID do medicamento: ");

            var buscado = banco.Pesquisar(new Medicamento { Id = id });
            if (buscado.Id == 0 && string.IsNullOrEmpty(buscado.Nome) && string.IsNullOrEmpty(buscado.Laboratorio))
            {
                Console.WriteLine("Medicamento NÃO encontrado.");
            }
            else
            {
                // Dados do medicamento (sintético)
                Console.WriteLine($"ID: {buscado.Id}");
                Console.WriteLine($"Nome: {buscado.Nome}");
                Console.WriteLine($"Laboratório: {buscado.Laboratorio}");
                Console.WriteLine($"Qtde disponível: {buscado.QtdeDisponivel()}");

                // + lotes (ID + QTDE + DATA DE VENCIMENTO)
                Console.WriteLine("\nLotes (FIFO):");
                int i = 1;
                foreach (var lote in buscado.Lotes)
                {
                    Console.WriteLine($" {i++}) {lote.Id} - {lote.Qtde} - {lote.Venc:dd/MM/yyyy}");
                }
                if (i == 1) Console.WriteLine(" (sem lotes no momento)");
            }
            Pausar();
        }

        static void ComprarLote(Medicamentos banco)
        {
            Console.WriteLine("\n-- Comprar (Cadastrar Lote) --");
            int idMed = LerInt("ID do medicamento: ");

            var med = banco.Pesquisar(new Medicamento { Id = idMed });
            if (med.Id == 0 && string.IsNullOrEmpty(med.Nome) && string.IsNullOrEmpty(med.Laboratorio))
            {
                Console.WriteLine("Medicamento NÃO encontrado. Cadastre primeiro (opção 1).");
                Pausar();
                return;
            }

            int idLote = LerInt("ID do lote: ");
            int qtde = LerInt("Quantidade do lote: ");
            DateTime venc = LerData("Data de vencimento (dd/MM/aaaa): ");

            med.Comprar(new Lote(idLote, qtde, venc));
            Console.WriteLine("Lote inserido na fila.");
            Pausar();
        }

        static void VenderMedicamento(Medicamentos banco)
        {
            Console.WriteLine("\n-- Vender --");
            int idMed = LerInt("ID do medicamento: ");

            var med = banco.Pesquisar(new Medicamento { Id = idMed });
            if (med.Id == 0 && string.IsNullOrEmpty(med.Nome) && string.IsNullOrEmpty(med.Laboratorio))
            {
                Console.WriteLine("Medicamento NÃO encontrado.");
                Pausar();
                return;
            }

            int qtde = LerInt("Quantidade a vender: ");
            bool ok = med.Vender(qtde);
            Console.WriteLine(ok ? "Venda realizada." : "Venda NÃO realizada (estoque insuficiente).");
            Pausar();
        }

        static void ListarSintetico(Medicamentos banco)
        {
            Console.WriteLine("\n-- Listagem (Sintético) --");
            // Mostrar dados sintéticos usando ToString(): id-nome-laboratorio-qtde
            int count = 0;
            foreach (var m in banco.ListarTodos())
            {
                Console.WriteLine(m.ToString());
                count++;
            }
            if (count == 0) Console.WriteLine("Nenhum medicamento cadastrado.");
            Pausar();
        }

        // --------- Utilidades ---------

        static int LerInt(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                var s = Console.ReadLine();
                if (int.TryParse(s, out int v))
                    return v;
                Console.WriteLine("Valor inválido, tente novamente.");
            }
        }

        static DateTime LerData(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                var s = Console.ReadLine();
                if (DateTime.TryParseExact(s, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                                           DateTimeStyles.None, out DateTime d))
                    return d;
                Console.WriteLine("Data inválida. Use dd/MM/aaaa.");
            }
        }

        static void Pausar()
        {
            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();
        }
    }
}
