using System;
using System.Collections.Generic;
using System.IO;
using static ConverterHtml.Entity;
using static ConverterHtml.MethodPersistence;
using static ConverterHtml.GenericMethod;

namespace ConverterHtml
{
    public class ProcessFileMemory
    {
        //Gera uma lista nova com os dados necessarios para o banco de dados
        public static void NewList(List<string> InputOldList, EstruturaCabecalho InputCabecalho, List<EstruturaProduto> InputProduto, EstruturaDataBase InputConnectData)
        {
            int i;
            decimal preUni;
            string opc, line;
            EstruturaProduto p;
            EstruturaProduto p_temp = new EstruturaProduto();
            try
            {
                if (InputOldList != null)
                {
                    for (i = 0; i < InputOldList.Count; i++)
                    {
                        opc = InputOldList[i];
                        switch (opc)
                        {
                            case "NÚMERO DA ORDEM DE COMPRA / PURCHASE ORDER:":
                                i += 2;
                                InputCabecalho.PurchaseOrder = ReplaceAllSpaces(InputOldList[i]);
                                i += 1;
                                InputCabecalho.Versao = InputOldList[i];
                                break;

                            case "ENVIAR PARA / SHIP TO:":

                                while (true)
                                {
                                    line = InputOldList[i];
                                    if (line.StartsWith("Attn:"))
                                    {
                                        InputCabecalho.Nome = ReplaceAllSpaces(InputOldList[i]);
                                        i += 1;
                                        InputCabecalho.CNPJ = ReplaceAllSpaces(InputOldList[i]);
                                        break;
                                    }
                                    i++;
                                }
                                break;

                            case "DATA DA ORDEM / ORDER DATE:":
                                i += 4;
                                InputCabecalho.DataOrdem = InputOldList[i];
                                i += 1;
                                InputCabecalho.NomeEmail = ReplaceAllSpaces(InputOldList[i]);
                                break;

                            case "Linha /Ln":
                                i += 7;
                                line = InputOldList[i];
                                while (true)
                                {
                                    if (String.Compare(line, "REQUISITOS DE DOCUMENTAÇÃO DA FATURA / INVOICE DOCUMENTATION REQUIREMENTS") == 0)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        p = new EstruturaProduto();
                                        
                                        p.Linha = InputOldList[i];
                                        i += 1;
                                        p.IdAmazon = ReplaceAllSpaces(InputOldList[i]);
                                        i += 1;
                                        line = InputOldList[i];
                                        //Analise se tem codigo da planner
                                        if (String.Compare(line, "Brasil Tax Item Code:") == 0)
                                        {
                                            //Espaco em branco por causa da falta de codigo da Planner
                                            p.IdPlanner = " ";
                                            p.Observacao = Constants.ConstantsObservacao.NaoCadastrado;
                                            i += 2;
                                        }
                                        else
                                        {
                                            p.IdPlanner = ReplaceAllSpaces(line);
                                            i += 3;
                                            p_temp = ProdutoDB.FindById(p,InputConnectData);
                                            
                                            if (p_temp != null)
                                            {
                                                p.PreUniPlanner = p_temp.PreUniPlanner;
                                                p.Tamanho = p_temp.Tamanho;
                                                p.Observacao = p_temp.Observacao;
                                            }
                                            else {
                                                p.Observacao = Constants.ConstantsObservacao.NaoCadastrado;
                                            }
                                        }
                                        //Analise se tem alguma observacao
                                        line = InputOldList[i];
                                        if (line.StartsWith("Note:"))
                                        {
                                            while (true)
                                            {
                                                //Para se encontrar a data
                                                line = InputOldList[i];
                                                if (ValidadeDate(line))
                                                {
                                                    break;
                                                }
                                                i += 1;
                                            }
                                        }
                                        i += 1;
                                        p.Quantidade = Convert.ToDecimal(GenericMethod.FormatValue(InputOldList[i]));
                                        i += 2;
                                        p.PreUniAmazon = Convert.ToDecimal(GenericMethod.FormatValue(InputOldList[i]));
                                        if (
                                            ((String.Compare(p.Observacao,Constants.ConstantsObservacao.NaoCadastrado)) != 0) 
                                            &&  (!((p_temp.Observacao).Contains(Constants.ConstantsObservacao.CodigoErrado)))
                                            ){
                                            preUni = p.PreUniAmazon - p.PreUniPlanner;
                                            if (preUni == 0)
                                            {
                                                p.Observacao = Constants.ConstantsObservacao.OK;
                                            }
                                            else
                                            {
                                                p.Observacao = Constants.ConstantsObservacao.ErroPreco + $" '{preUni}'";
                                            }
                                        }
                                        i += 2;
                                        line = InputOldList[i];
                                        InputProduto.Add(p);
                                    }
                                }
                                break;

                            default:
                                break;
                        }//Fim de Switch
                    }//Fim de For
                }
            }//Fim do Try
            finally
            {

            }
        }//End NewList

       
    }
}


/*
 
where IdPlanner='VNL008C131' or
IdPlanner='VNL008C124' or
IdPlanner='VNL008C118' or
IdPlanner='VNL008B991' or
IdPlanner='VNL008C089' or
IdPlanner='VNL008B802' or
IdPlanner='VNL008B699' 

 
 
 
 */