using System;
using System.Collections.Generic;
using System.IO;
using static ConverterHtml.Entity;
using static ConverterHtml.GenericMethod;
using static ConverterHtml.ExcelMethod;

namespace ConverterHtml
{
    public class ProcessFile
    {

        static EstruturaCabecalho Cabecalho = new EstruturaCabecalho { };
        static List<EstruturaProduto> ListaProduto = new List<EstruturaProduto> { };
        static List<string> ListaProdutoArquivo = new List<string> { };
        //estrutura com os dados necessarios para estabelecer conexao com o banco de dados
        static EstruturaDataBase connData = new EstruturaDataBase
        {
            //your server
            Datasource = "localhost",
            //your database name
            Database = "PlannerAmazon",
            //username of server to connect
            Username = "sa",
            //password
            Password = "econguiloo"
        };


        //Seleciona o procedimento para os arquivos    
        public static void FileOption(EstruturaArquivoCaminho InputFilePath, int opc)
        {
            EstruturaArquivoStream StreamFile = new EstruturaArquivoStream();
            int i;

            switch (opc)
            {
                case 1:
                    try
                    {
                        for (i = 0; i < InputFilePath.Read01.Count; i++)
                        {
                            StreamFile.Read01 = OpenFileRead(InputFilePath.Read01[i]);
                            ReadFileList(StreamFile, ListaProdutoArquivo);
                            ////Precisa ler a lista gerada pelo arquivo apos retirar as tags
                            ProcessFileMemory.NewList(ListaProdutoArquivo, Cabecalho, ListaProduto, connData);

                            InputFilePath.Write01 = @"C:\Users\junio\Documents\" + Cabecalho.PurchaseOrder + "New.txt";
                            WriteFile02(InputFilePath.Write01, ListaProdutoArquivo);

                            Cabecalho.Indice = i;
                            Console.WriteLine("Arquivo processado");
                            VerifyWorkBook(Cabecalho, InputFilePath);
                            //Se nao encontrar planilha com nome igual 
                            if (Cabecalho.PlanilhaNova == 0)
                            {
                                ExportExcel(Cabecalho, ListaProduto, InputFilePath);
                            }
                            else
                            {
                                Console.WriteLine($"Essa planilha ja existe no arquivo '{Cabecalho.PurchaseOrder}'");
                            }
                            ListaProdutoArquivo.Clear();
                            ListaProduto.Clear();
                        }
                        RemoveSheet(InputFilePath);
                    }
                    catch (Exception exHandle)
                    {
                        Console.WriteLine("Exception: " + exHandle.Message);
                    }
                    finally
                    {
                        CloseFile(StreamFile);
                        InputFilePath.Read01.Clear();
                    }
                    break;
                case 2:
                    //WriteFile
                    try
                    {
                        //for (i = 0; i < InputFilePath.Read01.Count; i++)
                        //{
                        StreamFile.Read01 = OpenFileRead(InputFilePath.Read01[0]);
                        ReadFileList(StreamFile, ListaProdutoArquivo);
                        ////Precisa ler a lista gerada pelo arquivo apos retirar as tags
                        ProcessFileMemory.NewList(ListaProdutoArquivo, Cabecalho, ListaProduto, connData);

                        InputFilePath.Write01 = @"C:\Users\junio\Documents\" + Cabecalho.PurchaseOrder + ".txt";
                        WriteFile02(InputFilePath.Write01, ListaProdutoArquivo);
                        NewFile(InputFilePath.Write01, Cabecalho.PurchaseOrder);
                        InputFilePath.Read01[0] = InputFilePath.Write01;
                        ListaProdutoArquivo.Clear();
                        ListaProduto.Clear();
                        StreamFile.Read01 = OpenFileRead(InputFilePath.Read01[0]);
                        ReadFileList(StreamFile, ListaProdutoArquivo);
                        ProcessFileMemory.NewList(ListaProdutoArquivo, Cabecalho, ListaProduto, connData);
                        InputFilePath.Write01 = @"C:\Users\junio\Documents\" + Cabecalho.PurchaseOrder + "_LP.txt";
                        WriteFile03(InputFilePath.Write01, ListaProduto);

                        Console.WriteLine("Arquivo processado");
                        //}
                    }
                    catch (Exception exHandle)
                    {
                        Console.WriteLine("Exception: " + exHandle.Message);
                    }
                    finally
                    {
                        CloseFile(StreamFile);
                        InputFilePath.Read01.Clear();
                    }
                    break;
                case 3:
                    //
                    break;
                case 4:
                    //ReadWriteFile(StreamReader inputFileRead, StreamWriter inputFileWrite);
                    break;
                case 5:
                    // NewFile(string InputFilePathRead);
                    break;
                case 6:
                    //
                    //WriteFileSimple(InputFilePath.Excel01);
                    break;


                default:
                    break;
            }
        }
        //Trata o arquivo de entrada removendo as tags, espacos em branco e coloca em uma lista
        public static void ReadFileList(EstruturaArquivoStream InputFile, List<string> InputList)
        {
            //Variaveis
            string line;
            try
            {
                while ((line = InputFile.Read01.ReadLine()) != null)
                {
                    line = RemoveTag(line);
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        InputList.Add(line);
                    }
                }
            }
            finally
            {
                InputFile.Read01.Close();
            }
        }//End ReadFileList
        //Escreve no arquivo
        public static void WriteFile(string InputFilePathWrite, List<EstruturaProduto> InputProduto, EstruturaCabecalho InputCabecalho)
        {
            string line;
            StreamWriter fileWrite;
            fileWrite = OpenFileWriter(InputFilePathWrite);
            try
            {
                line = InputCabecalho.CNPJ + "\t" + InputCabecalho.DataOrdem + "\t" + InputCabecalho.Nome + "\t" + InputCabecalho.NomeEmail + "\t" + InputCabecalho.PurchaseOrder + "\t" + InputCabecalho.Versao;
                fileWrite.WriteLine(line);
                foreach (EstruturaProduto p in InputProduto)
                {
                    line = p.Linha + "\t" + p.IdAmazon + "\t" + p.IdPlanner + "\t" + p.PreUniAmazon + "\t" + p.PreUniPlanner + "\t" + p.Quantidade + "\t" + p.Tamanho + "\t" + p.Observacao;
                    fileWrite.WriteLine(line);
                }
            }
            finally
            {
                fileWrite.Close();
            }
        }//Fim WriteFile
        public static void WriteFileGRU(string InputFilePathWrite, List<EstruturaProdutoGRU> InputProduto)
        {
            string line;
            StreamWriter fileWrite;
            fileWrite = GenericMethod.OpenFileWriter(InputFilePathWrite);

            try
            {
                line = "IdAmazon" + "\t" +
        "IdPlanner" + "\t" +
        "Descricao" + "\t" +
        "PreUniPlanner" + "\t" +
        "Quantidade" + "\t" +
        "Tamanho" + "\t" +
        "Observacao";

                fileWrite.WriteLine(line);

                foreach (EstruturaProdutoGRU p in InputProduto)
                {
                    line = p.IdAmazon + "\t" +
                        p.IdPlanner + "\t" +
                        p.Descricao + "\t" +
                        p.PreUniPlanner + "\t" +
                        p.Quantidade + "\t" +
                        p.Tamanho + "\t" +
                        p.Observacao;
                    fileWrite.WriteLine(line);
                }
            }
            finally
            {
                fileWrite.Close();
            }
        }//Fim WriteFileGRU
        //Trata o arquivo de entrada removendo as tags, espacos em branco
        public static void ReadWriteFile(StreamReader inputFileRead, StreamWriter inputFileWrite)
        {
            //Variaveis
            string line;
            while ((line = inputFileRead.ReadLine()) != null)
            {
                line = GenericMethod.RemoveTag(line);
                if (!string.IsNullOrWhiteSpace(line))
                {
                    inputFileWrite.WriteLine(line);
                }
            }
        }//End ReadWriteFile
        //Gera um arquivo com os dados necessarios para o banco de dados
        public static void NewFile(string InputFilePathRead, string x)
        {
            int i;
            bool resp;
            string opc, line, temp;
            string filePathWrite = @"C:\Users\junio\Documents\" + x + "_n.txt";
            System.IO.StreamReader fileRead;
            System.IO.StreamWriter fileWrite;

            fileRead = GenericMethod.OpenFileRead(InputFilePathRead);
            fileWrite = GenericMethod.OpenFileWriter(filePathWrite);
            try
            {
                if ((fileRead != null) && (fileWrite != null))
                {
                    while ((opc = fileRead.ReadLine()) != null)
                    {
                        switch (opc)
                        {
                            case "NÚMERO DA ORDEM DE COMPRA / PURCHASE ORDER:":
                                line = fileRead.ReadLine();
                                opc = opc + "\t" + line;
                                fileWrite.WriteLine(opc);
                                line = fileRead.ReadLine();
                                line = line + "\t" + fileRead.ReadLine();
                                fileWrite.WriteLine(line);
                                break;

                            case "ENVIAR PARA / SHIP TO:":

                                while (true)
                                {
                                    temp = fileRead.ReadLine();
                                    if (temp.StartsWith("Attn:"))
                                    {
                                        line = temp + "\t" + fileRead.ReadLine();
                                        fileWrite.WriteLine(line);
                                        break;
                                    }
                                }
                                break;

                            case "DATA DA ORDEM / ORDER DATE:":
                                opc = opc + "\t" + fileRead.ReadLine();
                                fileWrite.WriteLine(opc);
                                for (i = 0; i < 2; i++)
                                {
                                    line = fileRead.ReadLine();
                                }
                                line = fileRead.ReadLine() + "\t" + fileRead.ReadLine();
                                fileWrite.WriteLine(line);
                                break;

                            case "Linha /Ln":
                                /*Pega cabecalho do arquiv do arquivo html
                                 * opc = opc + "\t" + fileRead.ReadLine() + "\t" + fileRead.ReadLine()
                                          + "\t" + fileRead.ReadLine() + "\t" + fileRead.ReadLine()
                                          + "\t" + fileRead.ReadLine() + "\t" + fileRead.ReadLine();*/
                                line = opc + "\t" + "Número e Descrição do Item / Item Number & Description" + "\t" + "Codigo Planner" + "\t" + "Observacao" + "\t" + "Qtd. Pedida / Qty Ordered" + "\t" + "Preço Unitário / Unit Price" + "\t" + "Total / Total";
                                opc = opc + "\t" + fileRead.ReadLine() + "\t" + fileRead.ReadLine()
                                          + "\t" + fileRead.ReadLine() + "\t" + fileRead.ReadLine()
                                          + "\t" + fileRead.ReadLine() + "\t" + fileRead.ReadLine();
                                fileWrite.WriteLine(line);
                                while (true)
                                {
                                    line = fileRead.ReadLine();
                                    if (String.Compare(line, "REQUISITOS DE DOCUMENTAÇÃO DA FATURA / INVOICE DOCUMENTATION REQUIREMENTS") == 0)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        line = line + "\t" + fileRead.ReadLine();
                                        temp = fileRead.ReadLine();
                                        //Analise se tem codigo da planner
                                        if (String.Compare(temp, "Brasil Tax Item Code:") == 0)
                                        {
                                            //Espaco em branco por causa da falta de codigo da Planner
                                            line = line + "\t" + " ";
                                            temp = fileRead.ReadLine();
                                        }
                                        else
                                        {
                                            line = line + "\t" + temp;
                                            temp = fileRead.ReadLine() + fileRead.ReadLine();
                                        }
                                        //Analise se tem alguma observacao
                                        temp = fileRead.ReadLine();
                                        if (temp.StartsWith("Note:"))
                                        {
                                            line = line + "\t";
                                            while (true)
                                            {
                                                line = line + temp;
                                                //Para se encontrar a data
                                                temp = fileRead.ReadLine();
                                                if (resp = GenericMethod.ValidadeDate(temp))
                                                {
                                                    break;
                                                }
                                                line = line + "|";
                                            }
                                        }
                                        else
                                        {
                                            line = line + "\t" + " ";
                                        }

                                        line = line + "\t" + fileRead.ReadLine();
                                        temp = fileRead.ReadLine();
                                        for (i = 0; i < 2; i++)
                                        {
                                            temp = GenericMethod.FormatValue(fileRead.ReadLine());
                                            line = line + "\t" + temp;
                                        }
                                        temp = "";
                                        fileWrite.WriteLine(line);
                                    }
                                }
                                break;

                            case "Total da Ordem de Compra / Purchase Order Total":
                                temp = fileRead.ReadLine();
                                line = GenericMethod.FormatValue(fileRead.ReadLine());
                                opc = opc + "\t" + line;
                                fileWrite.WriteLine(opc);
                                opc = null;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }//Fim do Try
            finally
            {
                fileWrite.Close();
                fileRead.Close();
            }
        }//End NewFile
         //Escreve no arquivo
        public static void WriteFile02(string InputFilePathWrite, List<String> InputProduto)
        {
            ;
            StreamWriter fileWrite;
            fileWrite = OpenFileWriter(InputFilePathWrite);
            try
            {
                foreach (string line in InputProduto)
                {
                    fileWrite.WriteLine(line);
                }
            }
            finally
            {
                fileWrite.Close();
            }
        }//Fim WriteFile02
         //Escreve no arquivo
        public static void WriteFile03(string InputFilePathWrite, List<EstruturaProduto> InputProduto)
        {
            StreamWriter fileWrite;
            fileWrite = OpenFileWriter(InputFilePathWrite);
            try
            {
                foreach (EstruturaProduto line in InputProduto)
                {
                    fileWrite.WriteLine(line.IdPlanner    );
                    fileWrite.WriteLine(line.IdAmazon     );
                    fileWrite.WriteLine(line.Linha        );
                    fileWrite.WriteLine(line.Observacao   );
                    fileWrite.WriteLine(line.PreUniAmazon );
                    fileWrite.WriteLine(line.PreUniPlanner);
                    fileWrite.WriteLine(line.Quantidade);
                    fileWrite.WriteLine(line.Tamanho);
                }
                
            }
            finally
            {
                fileWrite.Close();
            }
        }//Fim WriteFile03
    }
}