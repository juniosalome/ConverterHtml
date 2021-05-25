using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; //acesso ao sqlserver..
using static ConverterHtml.Entity; //namespace das classes de entidade..

namespace ConverterHtml
{
    public class GenericMethodPersistence
    {
        /// Classe de persistencia para a entidade Produto
        public class ProdutoDal : GenericMethodConnection
        {
            public void Insert(Produto p, ConnectData InputConnectData)
            {
                try
                {
                    OpenConnection(InputConnectData);
                    /*Cmd = new SqlCommand("insert into ProdutosAmazon IdPlanner,IdAmazon,PrecoUnitario,Tamanho) values(@v1, @v2, @v3, @v4)", Con);
                    Cmd.Parameters.AddWithValue("@v1", p.IdPlanner);
                    Cmd.Parameters.AddWithValue("@v2", p.IdAmazon);
                    Cmd.Parameters.AddWithValue("@v3", p.PrecoUnitario);
                    Cmd.Parameters.AddWithValue("@v4", p.Tamanho);
                    Cmd.ExecuteNonQuery();*/
                    System.Console.WriteLine("Teste CCCCCC");
                }
                catch (Exception e)
                {
                    //lançar uma exceção para o projeto principal..
                    throw new Exception("Erro ao inserir Produto: " + e.Message);
                }
                finally
                {
                    CloseConnection(); //fechar conexão..
                }
            }//Fim de Insert

            public void Update(Produto p, ConnectData InputConnectData)
            {
                try
                {
                    OpenConnection(); //abrir conexao
                    Cmd = new SqlCommand("update Produto set Nome = @v1, Email = @v2, Sexo = @v3 where IdProduto = @v4", Con);
                    Cmd.Parameters.AddWithValue("@v1", p.IdPlanner);
                    Cmd.Parameters.AddWithValue("@v2", p.IdAmazon);
                    Cmd.Parameters.AddWithValue("@v3", p.PrecoUnitario);
                    Cmd.Parameters.AddWithValue("@v4", p.Tamanho);
                    Cmd.ExecuteNonQuery(); //executar
                }
                catch (Exception e)
                {
                    //lançar exceção..
                    throw new Exception("Erro ao atuaalizar Produto: " + e.Message);
                }
                finally
                {
                    CloseConnection(); //fechar conexao
                }
            }//Fim de Update

            public void Delete(int IdProduto, ConnectData InputConnectData)
            {
                try
                {
                    OpenConnection(InputConnectData); //abrir conexão..
                    Cmd = new SqlCommand("delete from Produto where IdProduto = @v1", Con);
                    Cmd.Parameters.AddWithValue("@v1", IdProduto);
                    Cmd.ExecuteNonQuery(); //executar..
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao excluir Produto: " + e.Message);
                }
                finally
                {
                    CloseConnection(); //fechar conexão..
                }
            }//Fim de Delete


            public Produto FindById(int IdPlanner, ConnectData InputConnectData)
            {
                try
                {
                    OpenConnection(InputConnectData); //abrir conexão..

                    Cmd = new SqlCommand("select * from Produto where IdPlanner = @v1", Con);
                    Cmd.Parameters.AddWithValue("@v1", IdPlanner);
                    Dr = Cmd.ExecuteReader();

                    //verificar se o DataReader obteve algum registro..
                    if (Dr.Read()) //verificando se o DataReader obteve algum registro..
                    {
                        Produto p = new Produto(); //classe de entidade...
                        p.IdPlanner = (string)Dr["IdProduto"];
                        p.IdAmazon = (string)Dr["IdAmazon"];
                        p.PrecoUnitario = (float)Dr["PrecoUnitario"];
                        p.Tamanho = (string)Dr["Tamanho"];


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
                    throw new Exception("Erro ao obter Produto: " + e.Message);
                }
                finally
                {
                    CloseConnection(); //fechar conexao..
                }
            }//Fim FindById

            public List<Produto> FindAll(ConnectData InputConnectData)
            {
                try
                {
                    OpenConnection(InputConnectData); //abrir conexão..
                    Cmd = new SqlCommand("select * from ProdutosAmazon", Con);
                    Dr = Cmd.ExecuteReader(); //executa a consulta e le os registros..

                    List<Produto> lista = new List<Produto>(); //lista vazia..

                    //enquanto houver registros na consulta..
                    while (Dr.Read())
                    {
                        Produto p = new Produto();

                        p.IdPlanner = (string)Dr["IdProduto"];
                        p.IdAmazon = (string)Dr["IdAmazon"];
                        p.PrecoUnitario = (float)Dr["PrecoUnitario"];
                        p.Tamanho = (string)Dr["Tamanho"];

                        lista.Add(p); //adicionar o Produto dentro da lista..
                    }

                    return lista; //retornar a lista..
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao listar Produtos: " + e.Message);
                }
                finally
                {
                    CloseConnection(); //fechar conexão..
                }
            }//Fim FindAll

        }//Fim ProdutoDal


    }
}

