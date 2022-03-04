using System;
using System.ComponentModel.DataAnnotations;

namespace GestaoDeEquipamentos.ConsoleApp
{
    
    internal class Program
    {
        static string[][] registroDeEquipamentos = new string[1000][];
        static int idEquipamentos = 0;
        static string[][] controleDeChamados = new string[1000][];
        static int idChamados = 0;
        static int[] equipamentosEmChamados = new int [1000];
        static int contadorEquipamentosEmChamado = 0;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Manutenção de Equipamentos 1.0 ");
                VisualizarMenu();
                Escrever("\n Digite sua opção: ");
                char opcao = Convert.ToChar(Console.ReadLine().ToUpper());
                if (opcao == 'S')
                {
                    break;
                }

                Console.Clear();
                VerificaOpcao(opcao);
                Console.Clear();

            }
        }

        #region Controle de Chamados
        static void ExcluirChamado()
        {
            string[][] controleDeChamadosAuxiliar = new string[1000][];
            int idParaExcluirChamado;
            if(idChamados == 0)
            {
                Aviso("Não há nenhum chamado de manutenção no momento! ", ConsoleColor.Yellow);
            }
            else
            {
                VisualizarChamados();
                Escrever("Digite o ID do chamado que deseja excluir: ");
                idParaExcluirChamado = Convert.ToInt32(Console.ReadLine());
                if(idParaExcluirChamado > idChamados)
                {
                    Aviso("O ID digitedo é inexistente!", ConsoleColor.Red);
                }
                else
                {
                    int j = 0;
                    for (int i = 0; i < idChamados; i++)
                    {
                        if(idParaExcluirChamado != i)
                        {
                            controleDeChamados[j][0] = controleDeChamados[i][0];
                            controleDeChamados[j][1] = controleDeChamados[i][1];
                            controleDeChamados[j][2] = controleDeChamados[i][2];
                            controleDeChamados[j][3] = controleDeChamados[i][3];
                            controleDeChamados[j][4] = controleDeChamados[i][4];
                            j++;

                        }
                    }
                    idChamados--;
                    Aviso("Chamado excluido com sucesso!", ConsoleColor.Green);
                }
            }


        }

        static void EditarChamados()
        {
            if(idChamados == 0)
            {
                Aviso("Não há nenhum chamado cadastrado!", ConsoleColor.Yellow);
            }
            else
            {
                VisualizarChamados();
                Escrever("Digite o ID do chamado que deseja editar: ");
                int idEditarChamado = Convert.ToInt32(Console.ReadLine());
                for (int i = 0; i < idChamados; i++)
                {
                    if(i == idEditarChamado)
                    {
                        Escrever(" Qual informação deseja editar? \n 1 - Título do chamado \n 2 - Descrição \n 3 - Equipamento \n 4 - Data de Abertura \n > ");
                        char opcao = Convert.ToChar(Console.ReadLine());

                        switch (opcao)
                        {
                            case '1':
                                while(true){
                                    Escrever("Digite o novo título: ");
                                    string titulo = Console.ReadLine();
                                    if (titulo != "")
                                    {
                                        Aviso("O titulo não pode estar vazio! Tente Novamente.", ConsoleColor.Red);
                                        continue;
                                    }
                                    else
                                    {
                                        controleDeChamados[idEditarChamado][1] = titulo;
                                        break;
                                    }
                                }
                                break;
                            case '2':
                                Escrever("Digite a nova descrição: ");
                                string descricao = Console.ReadLine();
                                if(descricao == "")
                                {
                                    Aviso("A Descrição não pode estar vazia! Tente novamente.", ConsoleColor.Red);
                                }
                                controleDeChamados[idEditarChamado][2] = Console.ReadLine();
                                Aviso("O Chamado foi editado com sucesso!", ConsoleColor.Green);
                                break;
                            case '3':
                                Escrever("Digite o Id do Equipamento: ");
                                int idEquipamentoDoChamado = Convert.ToInt32(Console.ReadLine());
                                if (VerificaID(idEquipamentoDoChamado) == false)
                                {
                                    Aviso("O ID digitado é inválido! Tente novamente", ConsoleColor.Red);
                                    Console.Clear();
                                    continue;
                                }
                                controleDeChamados[idEditarChamado][3] = registroDeEquipamentos[idEditarChamado][1];
                                Aviso("O Chamado foi editado com sucesso!", ConsoleColor.Green);

                                break;
                            case '4':
                                string dataAbertura;
                                try
                                {
                                    Escrever("Digite a data de abertura: ");
                                    dataAbertura = DateTime.Parse(Console.ReadLine()).ToString("dd/MM/yyyy");
                                }
                                catch (Exception)
                                {
                                    Aviso("ERRO! A data de abertura deve ser válida! Tente novamente.", ConsoleColor.Red);
                                    continue;
                                }
                                if (DiferencaDeDiasAtualmente(dataAbertura) < 0)
                                {
                                    Console.Clear();
                                    Aviso("A data de fabricação não pode ser futura ao dia de hoje! Tente novamente. ", ConsoleColor.Red);
                                    continue;
                                }
                                controleDeChamados[idEditarChamado][4] = dataAbertura;
                                break;
                            default: Aviso("Opção inválida!", ConsoleColor.Red);
                                continue;
                              
                        }
                    }
                }
            }

        }

        static void VisualizarChamados()
        {
            if(idChamados == 0)
            {
                Aviso("Não há nenhum chamado em eberto! ", ConsoleColor.Yellow);
            }
            else
            {
                for (int i = 0; i < idChamados; i++)
                {
                    string titulo = controleDeChamados[i][1];
                    string descricao = controleDeChamados[i][2];
                    string equipamento = controleDeChamados[i][3];
                    string dataApertura = controleDeChamados[i][4];
                    int diasEmAberto = DiferencaDeDiasAtualmente(dataApertura);

                    Console.WriteLine("\n ID do chamado: {0}\n Titulo do Chamado:  {1}\n Descrição: {2} \n Equipamento: {3}  \n Data de Abertura: {4} \n Dias que o chamado está aberto: {5}",i, titulo, descricao, equipamento, dataApertura, diasEmAberto);
                }
                
                Console.WriteLine();
            }
            
        }

        static void AbrirChamado()
        {
            while (true)
            {
                string[][] chamadosAbertos = new string[1000][];

                Escrever("Titulo do chamado: ");
                string tituloChamado = Console.ReadLine().ToUpper();
                if(tituloChamado == "")
                {
                    Aviso("O título do chamado não pode estar vazio! Tente novamente.", ConsoleColor.Red);
                    continue;
                }

                Escrever("Descrição do chamado: ");
                string descricaoChamado = Console.ReadLine();
                if(descricaoChamado == "")
                {
                    Aviso("A descrição do chamado não pode estar vazia! Tente novamente.", ConsoleColor.Red);
                    continue;
                }

                Escrever("ID do equipamento: ");
                int idEquipamentoDoChamado = Convert.ToInt32(Console.ReadLine()); 

                if(VerificaID(idEquipamentoDoChamado) == false)
                {
                    Aviso("O ID digitado é inválido! Tente novamente", ConsoleColor.Red);
                    Console.Clear();
                    continue;
                }
                string equipamentoChamado = registroDeEquipamentos[idEquipamentoDoChamado][1];

                string dataAbertura;
                try
                {
                    Escrever("Digite a data de abertura: ");
                    dataAbertura = DateTime.Parse(Console.ReadLine()).ToString("dd/MM/yyyy");
                }
                catch (Exception)
                {
                    Aviso("ERRO! A data de abertura deve ser válida! Tente novamente.", ConsoleColor.Red);
                    continue;
                }
                if (DiferencaDeDiasAtualmente(dataAbertura) < 0)
                {
                    Console.Clear();
                    Aviso("A data de fabricação não pode ser futura ao dia de hoje! Tente novamente. ", ConsoleColor.Red);
                    continue;
                }

                string[] chamadoAberto = new string[] {idChamados.ToString(), tituloChamado, descricaoChamado, equipamentoChamado, dataAbertura};


                equipamentosEmChamados[contadorEquipamentosEmChamado] = idEquipamentoDoChamado;
                contadorEquipamentosEmChamado++;

                controleDeChamados[idChamados] = chamadoAberto;
                idChamados++;

                Aviso("Seu chamado foi realizado com sucesso! ", ConsoleColor.Green);
                Console.ReadKey();
                break;
            }

        }
        
        #endregion

        #region Cadastro de Equipamentos
        static void ExcluirRegistro()
        {

            string[][] registroDeEquipamentosAuxiliar = new string[1000][];
            int idParaExcluir;
            if (idEquipamentos == 0)
            {
                Aviso("Não há nenhum registro no momento! ", ConsoleColor.Yellow);
            }
            else
            {
                VisualizarRegistros();
                Escrever("Digite o ID do equipamento que deseja excluir: ");
                idParaExcluir = Convert.ToInt32(Console.ReadLine());
                if (VerificaID(idParaExcluir) == false)
                {
                    Aviso("O ID digitado é inexistente!", ConsoleColor.Red);
                }
                else
                {
                    for (int i = 0; i < contadorEquipamentosEmChamado; i++)
                    {
                        if(idParaExcluir == equipamentosEmChamados[i])
                        {
                            Aviso("Este equipamento está vinculado à um chamado! Você precisa excluir o chamado antes!", ConsoleColor.Red);
                        }
                        else
                        {
                            int j = 0;
                            for (int k = 0; k < idEquipamentos; k++)
                            {
                                if (idParaExcluir != k)
                                {
                                    registroDeEquipamentos[j][0] = registroDeEquipamentos[k][0];
                                    registroDeEquipamentos[j][1] = registroDeEquipamentos[k][1];
                                    registroDeEquipamentos[j][2] = registroDeEquipamentos[k][2];
                                    registroDeEquipamentos[j][3] = registroDeEquipamentos[k][3];
                                    registroDeEquipamentos[j][4] = registroDeEquipamentos[k][4];
                                    registroDeEquipamentos[j][5] = registroDeEquipamentos[i][5];   
                                    j++;
                                }
                                else
                                {
                                    idEquipamentos--;
                                    Aviso("Equipamento excluido com sucesso!", ConsoleColor.Green);
                                }

                            }
                        }
                    }
                    
                                    }
            }
            Console.ReadKey();

        }

        static bool VerificaID(int idParaVerificar)
        {
            if (idParaVerificar > idEquipamentos)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        static void EditarRegistros()
        {
            int idDoEquipamentoEditar;
            if (idEquipamentos == 0)
            {
                Aviso("Não há nenhum registro no momento!", ConsoleColor.Yellow);
                Console.ReadKey();
            }
            else
            {
                while (true)
                { 
                VisualizarRegistros();
                Escrever("\n Digite o ID do equipamento que deseja editar: ");
                idDoEquipamentoEditar = Convert.ToInt32(Console.ReadLine());

                    if (VerificaID(idDoEquipamentoEditar) == false)
                    {
                        Aviso("O ID digitado é inexistente!", ConsoleColor.Red);
                        continue;
                    }
                    else
                    {
                        for (int i = 0; i < idEquipamentos; i++)
                        {
                            if (idDoEquipamentoEditar == i)
                            {
                                Escrever(" Qual informação deseja editar? \n 1 - Nome do Equipamento \n 2 - Preço \n 3 - Nº de Série \n 4 - Data de Fabricação \n 5 - Fabricante \n > ");

                                char opcao = Convert.ToChar(Console.ReadLine());

                                switch (opcao)
                                {
                                    case '1':
                                        while (true)
                                        {
                                            Escrever("Nome do equipamento: ");
                                            string nomeEquipamento = Console.ReadLine().ToUpper();
                                            if (nomeEquipamento.Length < 6)
                                            {
                                                Console.Clear();
                                                Aviso("O nome do equipamento não pode ter menos de 6 caracteres! Tente novamente. ", ConsoleColor.Red);
                                                continue;
                                            }
                                            registroDeEquipamentos[idDoEquipamentoEditar][1] = nomeEquipamento;
                                            Aviso("Equipamento editado com sucesso!!! ", ConsoleColor.DarkGreen);
                                            break;
                                        }
                                        break;
                                    case '2':
                                        while (true)
                                        {
                                            decimal preco;
                                            try
                                            {
                                                Escrever("Preço de aquisição: ");
                                                preco = Convert.ToDecimal(Console.ReadLine());
                                            }
                                            catch (Exception)
                                            {
                                                Aviso("ERRO! O valor digitado deve conter apenas números! Tente Novamente.", ConsoleColor.Red);
                                                continue;
                                            }
                                            if (preco <= 0)
                                            {
                                                Aviso("O valor de aquisição não pode ser negativo ou igual a zero! Tente novamente.", ConsoleColor.Red);
                                                continue;
                                            }

                                            registroDeEquipamentos[idDoEquipamentoEditar][2] = preco.ToString();
                                            Aviso("Equipamento editado com sucesso!!! ", ConsoleColor.DarkGreen);
                                            break;
                                        }
                                        break;
                                    case '3':
                                        while (true)
                                        {
                                            Escrever("Nº de série: ");
                                            string numeroDeSerie = Console.ReadLine().ToUpper();
                                            if (numeroDeSerie == "")
                                            {
                                                Console.Clear();
                                                Aviso("O Nº de série não pode ser nulo! Tente novamante.", ConsoleColor.Red);
                                                continue;
                                            }
                                            for (int j = 0; j < idEquipamentos; j++)
                                            {
                                                if (numeroDeSerie == registroDeEquipamentos[j][3])
                                                {
                                                    Console.Clear();
                                                    Aviso("O Nº de série não pode ser igual ao de outro equipamento! Tente novamente.", ConsoleColor.Red);
                                                    continue;
                                                }

                                            }
                                            registroDeEquipamentos[idDoEquipamentoEditar][3] = numeroDeSerie;
                                            Aviso("Equipamento editado com sucesso!!! ", ConsoleColor.DarkGreen);
                                            break;
                                        }
                                        break;
                                    case '4':
                                        while(true){
                                            string dataFabricacao;
                                            try
                                            {
                                                Escrever("Digite a data de fabricação (ex: dd/MM/yyyy): ");
                                                dataFabricacao = DateTime.Parse(Console.ReadLine()).ToString("dd/MM/yyyy");
                                            }
                                            catch (Exception)
                                            {
                                                Console.Clear();
                                                Aviso("ERRO! A data de fabricação deve ser válida! Tente novamente.", ConsoleColor.Red);
                                                continue;
                                            }
                                            if (DiferencaDeDiasAtualmente(dataFabricacao) < 0)
                                            {
                                                Console.Clear();
                                                Aviso("A data de fabricação não pode ser futura ao dia de hoje! Tente novamente. ", ConsoleColor.Red);
                                                continue;
                                            }
                                            registroDeEquipamentos[idDoEquipamentoEditar][4] = dataFabricacao;
                                            Aviso("Equipamento editado com sucesso!!! ", ConsoleColor.DarkGreen);
                                            break;
                                        }
                                        break;
                                    case '5':
                                        while (true)
                                        {
                                            Escrever("Digite o nome do fabricante: ");
                                            string nomeFabricante = Console.ReadLine().ToUpper();
                                            if (nomeFabricante == "")
                                            {
                                                Console.Clear();
                                                Aviso("O nome do fabrincante não pode ser nulo! Tente novamente. ", ConsoleColor.Red);
                                                continue;
                                            }
                                            registroDeEquipamentos[idDoEquipamentoEditar][5] = nomeFabricante;
                                            Aviso("Equipamento editado com sucesso!!! ", ConsoleColor.DarkGreen);
                                            break;
                                        }
                                        break;
                                    default:
                                        Aviso("Opão inválida! Tente novamente.", ConsoleColor.Red);
                                        Console.ReadKey();
                                        continue;
                                }
                                                             
                                Console.ReadKey();
                                
                            }
                        }
                        break;
                    }
                }
            }
        } 

        static void VisualizarRegistros()
        {
            if (idEquipamentos == 0)
            {
                Aviso("Não há nenhum registro no momento!", ConsoleColor.Yellow);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0, -10} | {1, -30} | {2, -30} | {3, -30}", "Id", "Nome", "Nº de Série","Fabricante");
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
                Console.ResetColor();

                for (int i = 0; i < idEquipamentos; i++)
                {
                    Console.WriteLine("{0, -10} | {1, -30} | {2, -30} | {3, -30}", registroDeEquipamentos[i][0], registroDeEquipamentos[i][1], registroDeEquipamentos[i][3], registroDeEquipamentos[i][5]);

                }
            }
        } 

        static void RegistrarEquipamentos()
        {
            while (true)
            {
                Escrever("Nome do equipamento: ");
                string nomeEquipamento = Console.ReadLine().ToUpper();
                if (nomeEquipamento.Length < 6)
                {
                    Console.Clear();
                    Aviso("O nome do equipamento deve ter no mínimo 6 caracteres! Tente novamente. ", ConsoleColor.Red);
                    continue;
                }

                decimal preco ;
                try
                {
                    Escrever("Preço de aquisição: ");
                    preco = Convert.ToDecimal(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.Clear();
                    Aviso("ERRO! O valor digitado deve conter apenas números! Tente Novamente.", ConsoleColor.Red);
                    continue;
                }
                if (preco <= 0)
                {
                    Console.Clear();
                    Aviso("O valor de aquisição não pode ser negativo ou igual a zero! Tente novamente.", ConsoleColor.Red);
                    continue;
                }

                Escrever("Digite o Nº de série: ");
                string numeroDeSerie = Console.ReadLine().ToUpper();
                if (numeroDeSerie == "")
                {
                    Console.Clear();
                    Aviso("O Nº de série não pode ser nulo! Tente novamante.", ConsoleColor.Red);
                    continue;
                }
                for (int i = 0; i < idEquipamentos; i++)
                {
                    if (numeroDeSerie == registroDeEquipamentos[i][3])
                    {
                        Console.Clear();
                        Aviso("O Nº de série não pode ser igual ao de outro equipamento! Tente novamente.", ConsoleColor.Red);
                        continue;
                    }
                }

                string dataFabricacao;
                try
                {
                    Escrever("Digite a data de fabricação (ex: dd/MM/yyyy): ");
                    dataFabricacao = DateTime.Parse(Console.ReadLine()).ToString("dd/MM/yyyy");

                }
                catch (Exception)
                {
                    Console.Clear();
                    Aviso("ERRO! A data de fabricação deve ser válida! Tente novamente.", ConsoleColor.Red);
                    continue;
                }
                if (DiferencaDeDiasAtualmente(dataFabricacao) < 0)
                {
                    Console.Clear();
                    Aviso("A data de fabricação não pode ser futura ao dia de hoje! Tente novamente. ", ConsoleColor.Red);
                    continue;
                }

                Escrever("Digite o nome do fabricante: ");
                string nomeFabricante = Console.ReadLine().ToUpper();
                if (nomeFabricante == "")
                {
                    Console.Clear();
                    Aviso("O nome do fabrincante não pode ser nulo! Tente novamente. ", ConsoleColor.Red);
                    continue;
                }

                string[] equipamento = new string[] { idEquipamentos.ToString(), nomeEquipamento, preco.ToString(), numeroDeSerie, dataFabricacao, nomeFabricante };


                registroDeEquipamentos[idEquipamentos] = equipamento;
                Console.Clear();
                Aviso("Equipamento cadastrado com sucesso!!!", ConsoleColor.DarkGreen);
                idEquipamentos++;

                Console.WriteLine("Deseja continuar (S / N) ? ");
                char opcao = Convert.ToChar(Console.ReadLine().ToUpper());
                if (opcao == 'S')
                {
                    Console.Clear();
                    continue;
                }
                else
                {
                    Console.Clear();
                    break;
                }
            }
        }
        #endregion
        
        static int DiferencaDeDiasAtualmente(string data)
        {
            string[] dataSeparada = data.Split('/');

            int dia = Convert.ToInt32(dataSeparada[0]);
            int mes = Convert.ToInt32(dataSeparada[1]);
            int ano = Convert.ToInt32(dataSeparada[2]);

            DateTime dataCriacaoChamado = new DateTime(ano, mes, dia);
            DateTime dataAtual = DateTime.Now;
            TimeSpan periodoTempo = dataAtual - dataCriacaoChamado;

            int diasEmAberto = periodoTempo.Days;

            return diasEmAberto;
        }
        static void VisualizarMenu()
        {
            Console.WriteLine("                    MENU                    " +
                              "\n Controle de Equipamentos                 " +
                              "\n 1 - Para registrar um equipamento        " + 
                              "\n 2 - Para visualizar registros            " + 
                              "\n 3 - Para editar registros                " + 
                              "\n 4 - Excluir registro de equipamentos     " + 
                              "\n\n Controle de Chamados                   " +
                              "\n 5 - Abrir chamados                       " + 
                              "\n 6 - Visualizar chamados                  " + 
                              "\n 7 - Editar chamado                       " +
                              "\n 8 - Excluir chamado                      " +
                              "\n s - Para sair                            ");
        }
        static void VerificaOpcao(char opcao)
        {
            switch (opcao)
            {
                case '1':
                    RegistrarEquipamentos();
                    break;
                case '2':
                    VisualizarRegistros();
                    Console.ReadKey();
                    break;
                case '3':
                    EditarRegistros();
                    break;
                case '4':
                    ExcluirRegistro();
                    break;
                case '5':
                    AbrirChamado();
                    break;
                case '6':
                    VisualizarChamados();
                    Console.ReadKey();
                    break;
                case '7':
                    EditarChamados();
                    break;
                case '8':
                    ExcluirChamado();
                    break;

                default:
                    Aviso("Opção inválida! Tente Novamente. ", ConsoleColor.Red);
                    Console.ReadKey();
                    break;
            }

        }
        static void Aviso(string aviso, ConsoleColor cor)
        {
            Console.ForegroundColor = cor;
            Console.WriteLine(aviso);
            Console.ResetColor();
        }
        static void Escrever( string mensagem)
        {
            Console.Write(mensagem);
        }
    }
}
