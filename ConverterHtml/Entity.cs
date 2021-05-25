using System.IO;
using System.Collections.Generic;


namespace ConverterHtml
{
    public class Entity
    {
        public class EstruturaProduto
        {
            public string  IdPlanner     { get; set; }
            public string  IdAmazon      { get; set; }
            public string  Linha         { get; set; }
            public string  Observacao    { get; set; }
            public decimal PreUniAmazon  { get; set; }
            public decimal PreUniPlanner { get; set; }
            public decimal Quantidade    { get; set; }
            public string  Tamanho       { get; set; }

        }
        public class EstruturaProdutoGRU
        {
            public string IdPlanner     { get; set; }
            public string IdAmazon      { get; set; }
            public string Observacao    { get; set; }
            public string Descricao     { get; set; }
            public string PreUniPlanner { get; set; }
            public string Quantidade    { get; set; }
            public string Tamanho       { get; set; }

        }
        public class EstruturaCabecalho
        {
            public string CNPJ          { get; set; }
            public string DataOrdem     { get; set; }
            public string Nome          { get; set; }
            //Nome e email presente na nota
            public string NomeEmail     { get; set; }
            //Codigo da ordem de compra da Amazon
            public string PurchaseOrder { get; set; }
            //Versao do documento
            public string Versao        { get; set; }
            //Indice da lista
            public int Indice { get; set; }  
            //Verifica se planilha é nova ou nao, 0 planilha nova, 1 planilha ja existe
            public int PlanilhaNova { get; set; }  
        }
        public class EstruturaArquivoCaminho
        {
            public List <string> Read01  { get; set; }
           // public string Read01  { get; set; }
            public string Read02  { get; set; }
            public string Write01 { get; set; }
            public string Write02 { get; set; }
            public string Excel01 { get; set; }
            public string Excel02 { get; set; }
        }
        public class EstruturaArquivoStream
        {
            public StreamReader Read01  { get; set; }
            public StreamReader Read02  { get; set; }
            public StreamWriter Write01 { get; set; }
            public StreamWriter Write02 { get; set; }
        }
        //Estrutura para conexao do banco de dados.
        public class EstruturaDataBase
        {
            //your server
            public string Datasource { get; set; }
            //your database name
            public string Database { get; set; }
            //username of server to connect
            public string Username { get; set; }
            //password
            public string Password { get; set; }
        }
        public class EstruturaPlanilhaExcel
        {
            public List<string> Read01 { get; set; }
            // public string Read01  { get; set; }
            public string Read02 { get; set; }
            public string Write01 { get; set; }
            public string Write02 { get; set; }
            public string Excel01 { get; set; }
            public string Excel02 { get; set; }
        }


    }
}
