using System;
using System.Collections.Generic;
using System.Data.SqlClient; //acesso ao sqlserver..
using static ConverterHtml.Entity;
using System.Linq;


namespace ConverterHtml
{
    public class MethodPersistence
    {

        //declarar atributos..
        private static SqlConnection  Con; //conexão com o banco de dados
        private static SqlCommand     Cmd; //executar comandos SQL
        private static SqlDataReader  Dr;  //Ler dados de consultas
        private static SqlTransaction Tr;  //Transações em banco de dados (commit/rollback)




        /// Classe de persistencia para a entidade Produto
        public class ProdutoDB
        {

            //Conectar no banco de dados
            protected static void OpenConnection(EstruturaDataBase InputConnectData)
            {
                Console.WriteLine("Getting Connection ...");
                //your connection string 
                string connString = @"Data Source=" + InputConnectData.Datasource + ";Initial Catalog="
                            + InputConnectData.Database + ";Persist Security Info=True;User ID=" + InputConnectData.Username + ";Password=" + InputConnectData.Password;

                //create instanace of database connection
                Con = new SqlConnection(connString);

                try
                {
                    Console.WriteLine("Openning Connection ...");

                    //open connection
                    Con.Open();

                    Console.WriteLine("Connection successful!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }

            }//Fim Connect
             //Fechar conexao no banco de dados 
            public static void CloseConnection()
            {
                Console.WriteLine("Close connection successful!");
                if (Con != null) { 
                    Con.Close(); 
                }
            }//Fim de CloseConnection

            public static void Insert(EstruturaProduto p, EstruturaDataBase InputConnectData)
            {
                try
                {

                    OpenConnection(InputConnectData);
                    Cmd = new SqlCommand("insert into ProdutosAmazon (IdPlanner,IdAmazon,PrecoUnitario,Tamanho) values(@v1, @v2, @v3, @v4)", Con);
                    Cmd.Parameters.AddWithValue("@v1", p.IdPlanner);
                    Cmd.Parameters.AddWithValue("@v2", p.IdAmazon);
                    Cmd.Parameters.AddWithValue("@v3", p.PreUniPlanner);
                    Cmd.Parameters.AddWithValue("@v4", p.Tamanho);
                    Cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    //lançar uma exceção para o projeto principal..
                    Console.WriteLine("Erro ao inserir Produto: " + e.Message);
                    
                }
                finally
                {
                    CloseConnection(); //fechar conexão..
                }
            }//Fim de Insert

            public static void Update(EstruturaProduto p, EstruturaDataBase InputConnectData)
            {
                try
                {
                    OpenConnection(InputConnectData); //abrir conexao
                    Cmd = new SqlCommand("update ProdutosAmazon set IdAmazon = @v2, PrecoUnitario = @v3, Tamanho = @v4 where IdPlanner = @v1", Con);
                    Cmd.Parameters.AddWithValue("@v1", p.IdPlanner);
                    Cmd.Parameters.AddWithValue("@v2", p.IdAmazon);
                    Cmd.Parameters.AddWithValue("@v3", p.PreUniPlanner);
                    Cmd.Parameters.AddWithValue("@v4", p.Tamanho);
                    Cmd.ExecuteNonQuery(); //executar
                }
                catch (Exception e)
                {
                    //lançar exceção..
                    Console.WriteLine("Erro ao atuaalizar Produto: " + e.Message);
                }
                finally
                {
                    CloseConnection(); //fechar conexao
                }
            }//Fim de Update

            public static void Delete(string IdPlanner, EstruturaDataBase InputConnectData)
            {
                try
                {
                    OpenConnection(InputConnectData); //abrir conexão..
                    Cmd = new SqlCommand("delete from ProdutosAmazon where IdPlanner = @v1", Con);
                    Cmd.Parameters.AddWithValue("@v1", IdPlanner);
                    Cmd.ExecuteNonQuery(); //executar..
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erro ao excluir Produto: " + e.Message);
                }
                finally
                {
                    CloseConnection(); //fechar conexão..
                }
            }//Fim de Delete

            public static EstruturaProduto FindById(EstruturaProduto InputProduto, EstruturaDataBase InputConnectData)
            {
                try
                {

                    OpenConnection(InputConnectData); //abrir conexão..
                    EstruturaProduto p = new EstruturaProduto(); //classe de entidade...

                    Cmd = new SqlCommand("select * from ProdutosAmazon where IdPlanner like @v1 and IdAmazon like @v2", Con);
                    Cmd.Parameters.AddWithValue("@v1", InputProduto.IdPlanner);
                    if (InputProduto.IdAmazon.Contains("-")) {
                        Cmd.Parameters.AddWithValue("@v2", (InputProduto.IdAmazon).Substring(0, (InputProduto.IdAmazon).IndexOf("-")));
                    }
                    else {
                        Cmd.Parameters.AddWithValue("@v2", InputProduto.IdAmazon);
                    }
                    

                    Dr = Cmd.ExecuteReader();

                    //verificar se o DataReader obteve algum registro..
                    if (Dr.Read()) //verificando se o DataReader obteve algum registro..
                    {
                        p.PreUniPlanner = Convert.ToDecimal(Dr["PrecoUnitario"]);
                        p.Tamanho = Convert.ToString(Dr["Tamanho"]);
                        p.Observacao = "";
                        return p; //retornar o Produto..
                    }
                    else {
                        Dr.Close();
                        Cmd = new SqlCommand("select * from ProdutosAmazon where IdPlanner like @v1", Con);
                        Cmd.Parameters.AddWithValue("@v1", InputProduto.IdPlanner);
                        Dr = Cmd.ExecuteReader();
                        if (Dr.Read()) //verificando se o DataReader obteve algum registro..
                        { 
                            p.Observacao = Constants.ConstantsObservacao.CodigoErrado + Convert.ToString(Dr["IdAmazon"]);
                            p.PreUniPlanner = Convert.ToDecimal(Dr["PrecoUnitario"]);
                            p.Tamanho = Convert.ToString(Dr["Tamanho"]);
                            return p;
                        }
                        else
                        {
                            return null; //retornar vazio..
                        } 
                    }

                }
                catch (Exception e)
                {
                    //lançar exceção..
                    Console.WriteLine("Erro ao obter Produto: " + e.Message);
                    return null;
                }
                finally
                {
                    CloseConnection(); //fechar conexao..
                }
            }//Fim FindById

            public static List<EstruturaProduto> FindAll(EstruturaDataBase InputConnectData)
            {
                try
                {
                    OpenConnection(InputConnectData); //abrir conexão..
                    Cmd = new SqlCommand("select * from ProdutosAmazon", Con);
                    Dr = Cmd.ExecuteReader(); //executa a consulta e le os registros..

                    List<EstruturaProduto> lista = new List<EstruturaProduto>(); //lista vazia..

                    //enquanto houver registros na consulta..
                    while (Dr.Read())
                    {
                        EstruturaProduto p = new EstruturaProduto();

                        lista.Add(p); //adicionar o Produto dentro da lista..
                    }

                    return lista; //retornar a lista..
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erro ao listar Produtos: " + e.Message);
                    return null;
                }
                finally
                {
                    CloseConnection(); //fechar conexão..
                }
            }//Fim FindAll
            public static List<EstruturaProdutoGRU> FindAllGRU(EstruturaDataBase InputConnectData)
            {
                EstruturaProduto p_temp = new EstruturaProduto();
                List<EstruturaProdutoGRU> lista = new List<EstruturaProdutoGRU>(); //lista vazia..
                try
                {
                    OpenConnection(InputConnectData); //abrir conexão..
                    //Cmd = new SqlCommand("select * from GRU8", Con);
                    Cmd = new SqlCommand("select * from CatalogoPlannerLucas", Con);
                    Dr = Cmd.ExecuteReader(); //executa a consulta e le os registros..
                    //enquanto houver registros na consulta..
                    while (Dr.Read())
                    {
                        EstruturaProdutoGRU p = new EstruturaProdutoGRU();
                        p.IdAmazon = Convert.ToString(Dr["IdAmazon"]);
                        p.IdPlanner = Convert.ToString(Dr["IdPlanner"]);
                        p.Descricao = Convert.ToString(Dr["Descricao"]);
                        p.Quantidade = Convert.ToString(Dr["Quantidade"]);
                        lista.Add(p); //adicionar o Produto dentro da lista..
                    }

                    foreach (EstruturaProdutoGRU pGRU in lista)
                    {
                        if (String.Compare(pGRU.IdPlanner, "(blank)") != 0)
                        {
                            p_temp.IdAmazon = pGRU.IdAmazon;
                            p_temp.IdPlanner = pGRU.IdPlanner;
                            p_temp = FindById(p_temp, InputConnectData);
                            if (p_temp != null)
                            {
                                pGRU.PreUniPlanner = Convert.ToString(p_temp.PreUniPlanner);
                                pGRU.Tamanho = p_temp.Tamanho;
                                pGRU.Observacao = "OK";
                            }
                            else
                            {
                                pGRU.Observacao = "Nao cadastrado";
                            }

                        }
                        else
                        {
                            pGRU.Observacao = "Nao cadastrado";
                        }
                        p_temp = new EstruturaProduto();
                    }
                    return lista; //retornar a lista..
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erro ao listar Produtos: " + e.Message);
                    return null;
                }
                finally
                {
                    CloseConnection(); //fechar conexão..
                }
            }//Fim FindAllGRU

            public static EstruturaProduto FindByIdCompararDB(EstruturaProduto InputProduto, EstruturaDataBase InputConnectData)
            {
                try
                {
                    OpenConnection(InputConnectData); //abrir conexão..

                    Cmd = new SqlCommand("select * from ProdutosAmazon where IdPlanner like @v1", Con);
                    Cmd.Parameters.AddWithValue("@v1", InputProduto.IdPlanner);                    
                    Dr = Cmd.ExecuteReader();

                    //verificar se o DataReader obteve algum registro..
                    if (Dr.Read()) //verificando se o DataReader obteve algum registro..
                    {
                        EstruturaProduto p = new EstruturaProduto(); //classe de entidade...

                        p.PreUniPlanner = Convert.ToDecimal(Dr["PrecoUnitario"]);                      
                        return p; //retornar o Produto..
                    }
                    else
                    {
                        return null; //retornar vazio..
                    }
                }
                catch (Exception e)
                {
                    //lançar exceção..
                    Console.WriteLine("Erro ao obter Produto: " + e.Message);
                    return null;
                }
                finally
                {
                    CloseConnection(); //fechar conexao..
                }
            }//Fim FindById

            public static List<EstruturaProduto> FindAll2(EstruturaDataBase InputConnectData)
            {
                try
                {
                    OpenConnection(InputConnectData); //abrir conexão..
                    //Cmd = new SqlCommand("select * from CatalogoPlannerLucas", Con);
                    Cmd = new SqlCommand("select * from GRU8_ITEMS_PLANNER_20210115", Con);
                    Dr = Cmd.ExecuteReader(); //executa a consulta e le os registros..

                    List<EstruturaProduto> lista = new List<EstruturaProduto>(); //lista vazia..

                    //enquanto houver registros na consulta..
                    while (Dr.Read())
                    {
                        EstruturaProduto p = new EstruturaProduto();
                        p.IdAmazon = Convert.ToString(Dr["IdAmazon"]);
                        p.IdPlanner = Convert.ToString(Dr["IdPlanner"]);
                        p.PreUniAmazon = Convert.ToDecimal(Dr["Preco"]);
                        p.Quantidade = Convert.ToDecimal(Dr["Quantidade"]);

                        lista.Add(p); //adicionar o Produto dentro da lista..
                    }

                    return lista; //retornar a lista..
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erro ao listar Produtos: " + e.Message);
                    return null;
                }
                finally
                {
                    CloseConnection(); //fechar conexão..
                }
            }//Fim FindAll



        }//Fim ProdutoDal
    }
}