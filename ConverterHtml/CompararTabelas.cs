using System;
using System.Collections.Generic;
using System.IO;
using static ConverterHtml.Entity;
using static ConverterHtml.GenericMethod;
using static ConverterHtml.ExcelMethod;
using static ConverterHtml.ProcessFile;
using static ConverterHtml.ProcessFileMemory;
using static ConverterHtml.MethodPersistence;

namespace ConverterHtml
{
   public class CompararTabelas
    {
        static EstruturaCabecalho Cabecalho = new EstruturaCabecalho { };
        static List<EstruturaProduto> ListaProduto = new List<EstruturaProduto> { };
        static List<string> ListaProdutoArquivo = new List<string> { };
        static EstruturaArquivoCaminho InputFilePath = new EstruturaArquivoCaminho {
            Excel01 = @"C:\Users\junio\Documents\GRU8_ITEMS_PLANNER.xlsx"

        };
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


        public static void comparar() {
            List <EstruturaProduto> p_list = new List<EstruturaProduto>();
            EstruturaProduto p_temp = new EstruturaProduto();
            decimal preUni;

            p_list = ProdutoDB.FindAll2(connData);
            foreach (EstruturaProduto p in p_list) {


                if (p.IdPlanner == null)
                {
                    p.Observacao = Constants.ConstantsObservacao.NaoCadastrado;
                }
                else
                {

                    p_temp = ProdutoDB.FindByIdCompararDB(p, connData);
                    if (p_temp == null)
                    {
                        p.Observacao = Constants.ConstantsObservacao.NaoCadastrado;
                    }
                    else
                    {

                        preUni = p.PreUniAmazon - p_temp.PreUniPlanner;
                        if (preUni == 0)
                        {
                            p.Observacao = Constants.ConstantsObservacao.OK;
                        }
                        else
                        {
                            p.Observacao = Constants.ConstantsObservacao.ErroPreco + $" '{preUni}'";
                        }
                        p.PreUniPlanner = p_temp.PreUniPlanner;
                    }
                }
            }


            ExportExcel01(p_list, InputFilePath);
            Console.WriteLine("Fim COmparar");
            Console.Read();
            // 



        }
    }
}
