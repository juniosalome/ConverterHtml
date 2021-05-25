using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class ConsoleApp1
    {
        static void Main(string[] args)
        {
            //Variaveis
            int counter = 0;
            string line;
            //string filePathWrite = @"C:\Windows\Temp\Temp_Html";
            string filePathWrite = @"C:\Users\junio\Documents\Temp_Html.txt";
            // string filePathRead = @"C:\Users\junio\Documents\Temp_Html - Copia (3).txt";
            //string filePathRead = @"C:\Users\junio\Documents\Temp_Html.txt";
            //string filePathRead = @"C:\Users\junio\Documents\HTML_Amazon\4R-04212157.html";
            //string filePathRead = @"C:\Users\junio\Documents\HTML_Amazon\4R-03910542.html";
            //string filePathRead = @"C:\Users\junio\Documents\HTML_Amazon\4R-04212157.html";
            string filePathRead = @"C:\Users\junio\Documents\HTML_Amazon\4R-04213399.html";
            //string filePathRead = @"C:\Users\junio\Documents\HTML_Amazon\4R-04224812.html";
            //string filePathRead = @"C:\Users\junio\Documents\HTML_Amazon\4R-04228112.html";
            //string filePathRead = @"C:\Users\junio\Documents\HTML_Amazon\4R-04228112.html";

            // ProcessFile.Read


            /* if (String.IsNullOrEmpty(filePathRead)) {
                 System.Console.WriteLine("Arquivo de entrada em branco ou invalido");
                 System.Console.ReadLine();
             }
             if (String.IsNullOrEmpty(filePathWrite)) {
                 System.Console.WriteLine("Erro ao tentar criar o arquivo.");
                 System.Console.ReadLine();
             }*/

            System.IO.StreamReader fileRead;
            System.IO.StreamWriter fileWrite;
            fileRead = new System.IO.StreamReader(filePathRead);
            fileWrite = new System.IO.StreamWriter(filePathWrite);
            while ((line = fileRead.ReadLine()) != null)
            {
                line = RemoveTag(line);
                if (!string.IsNullOrWhiteSpace(line))
                {
                    fileWrite.WriteLine(line);
                    counter++;
                }
            }
            System.Console.WriteLine("Numero de linhas: {0}.", counter);
            fileWrite.WriteLine(counter);
            fileWrite.Close();
            fileRead.Close();
            NewFile(filePathWrite);
            // Suspend the screen.  
            System.Console.ReadLine();
        }//End Main

        //Remover as tags
        public static string RemoveTag(string input)
        {
            string srt;
            srt = Regex.Replace(input, "<.*?>", String.Empty);
            srt = RemoveTab(srt);
            return srt;
        }

        //Remover as tabulacoes
        public static string RemoveTab(string input)
        {
            string str;
            const string reduceMultiSpace = @"[ ]{2,}";
            str = Regex.Replace(input.Replace("\t", ""), reduceMultiSpace, "");
            return str;
        }

        //Trocar ponto por virgula e retirar a virgula
        public static string FormatValue(string input)
        {
            string str;
            str = input.Replace(",", "");
            str = str.Replace(".", ",");
            return str;
        }
        public static bool ValidadeDate(string input)
        {
            Regex teste = new Regex(@"(\d{4}\-\d{2}\-\d{2})");
            return teste.Match(input).Success;
        }
        //Tratar novo arquivo
        public static void NewFile(string Caminhofile)
        {
            int i;
            bool resp;
            string opc, line, temp;
            string filePathWrite = @"C:\Users\junio\Documents\New.txt";
            System.IO.StreamReader fileRead;
            System.IO.StreamWriter fileWrite;

            fileRead = new System.IO.StreamReader(Caminhofile);
            fileWrite = new System.IO.StreamWriter(filePathWrite);

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
                        for (i = 0; i < 3; i++)
                        {
                            line = fileRead.ReadLine();
                        }
                        line = fileRead.ReadLine() + "\t" + fileRead.ReadLine();
                        fileWrite.WriteLine(line);
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
                                        if (resp = ValidadeDate(temp))
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
                                    temp = FormatValue(fileRead.ReadLine());
                                    line = line + "\t" + temp;
                                }
                                temp = "";
                                fileWrite.WriteLine(line);
                            }
                        }
                        break;

                    case "Total da Ordem de Compra / Purchase Order Total":
                        temp = fileRead.ReadLine();
                        line = FormatValue(fileRead.ReadLine());
                        opc = opc + "\t" + line;
                        fileWrite.WriteLine(opc);
                        opc = null;
                        break;
                    default:
                        break;
                }
            }
            fileWrite.Close();
            fileRead.Close();
        }//End NewFile
    }//End Program
}//End ConsoleApp3
