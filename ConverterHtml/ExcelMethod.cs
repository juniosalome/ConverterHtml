using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using static ConverterHtml.Entity;
using static ConverterHtml.Constants;


namespace ConverterHtml
{
    class ExcelMethod
    {
        static Application ExcelApp;
  
        public static void ExportExcel(EstruturaCabecalho InputCabecalho, List<EstruturaProduto> InputListaProduto, EstruturaArquivoCaminho InputFilePath)
        {
            ExcelApp = new Application();
            Workbook workbook = null;
            Sheets worksheets = null;
            int r;//r stands for ExcelRow and c for ExcelColumn 
            int i;
            int numSheet;//Numero de planilhas no Excel
            var newsheet = new Worksheet();
            try
            {
                if (ExcelApp == null)
                {
                    //MessageBox.Show("Excel is not properly installed!!");
                    Console.WriteLine("Excel is not properly installed!!");
                    return;
                }
                ExcelApp.Visible = false;
                ExcelApp.DisplayAlerts = false;

                if (!(File.Exists(InputFilePath.Excel01)))
                {
                    workbook = ExcelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                    workbook.SaveAs(InputFilePath.Excel01);
                    workbook.Close();
                }
                
                workbook = ExcelApp.Workbooks.Open(InputFilePath.Excel01, 0, false, 5, "", "", false, XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                worksheets = workbook.Worksheets;
                numSheet = ExcelApp.ActiveWorkbook.Worksheets.Count;
                newsheet = (Worksheet)worksheets.Add(worksheets[numSheet], Type.Missing, Type.Missing, Type.Missing);
                newsheet.Name = InputCabecalho.PurchaseOrder;
                newsheet.Cells[1, 1] = "Purchase Order";
                newsheet.Cells[1, 2] = "Versao";
                newsheet.Cells[1, 3] = "Order date";
                newsheet.Cells[2, 1] = InputCabecalho.PurchaseOrder;
                newsheet.Cells[2, 2] = InputCabecalho.Versao;
                newsheet.Cells[2, 3] = InputCabecalho.DataOrdem;
                newsheet.Cells[3, 1] = "Nome|Email";
                newsheet.Cells[3, 2] = "CNPJ";
                newsheet.Cells[4, 1] = InputCabecalho.NomeEmail;
                newsheet.Cells[4, 2] = InputCabecalho.CNPJ;
                //Produtos
                newsheet.Cells[5, 1] = "Linha";
                newsheet.Cells[5, 2] = "IdAmazon";
                newsheet.Cells[5, 3] = "IdPlanner";
                newsheet.Cells[5, 4] = "PreUniAmazon";
                newsheet.Cells[5, 5] = "PreUniPlanner";
                newsheet.Cells[5, 6] = "Quantidade";
                newsheet.Cells[5, 7] = "Tamanho";
                newsheet.Cells[5, 8] = "Observacao";

                r = 6;
                foreach (EstruturaProduto p in InputListaProduto)
                {
                    newsheet.Cells[r, 1] = p.Linha;
                    newsheet.Cells[r, 2] = p.IdAmazon;
                    newsheet.Cells[r, 3] = p.IdPlanner;
                    newsheet.Cells[r, 4] = p.PreUniAmazon;
                    newsheet.Cells[r, 5] = p.PreUniPlanner;
                    newsheet.Cells[r, 6] = p.Quantidade;
                    newsheet.Cells[r, 7] = p.Tamanho;
                    newsheet.Cells[r, 8] = p.Observacao;

                    if ((String.Compare(p.Observacao, ConstantsObservacao.NaoCadastrado) == 0) ||
                        ((p.Observacao.StartsWith(ConstantsObservacao.CodigoErrado)))) {
                        for (i = 1; i < 9; i++)
                        {
                            newsheet.Cells[r, i].Interior.Color = System.Drawing.Color.FromArgb(255,255,0);//Amarelo
                            newsheet.Cells[r, i].Borders.LineStyle = XlLineStyle.xlContinuous;
                        }
                    } 
                    else { 
                        if (p.Observacao.StartsWith(ConstantsObservacao.ErroPreco)) {
                            for (i = 1; i < 9; i++)
                            {
                                newsheet.Cells[r, i].Interior.Color = System.Drawing.Color.FromArgb(255,37,37);//Vermelho 
                                newsheet.Cells[r, i].Borders.LineStyle = XlLineStyle.xlContinuous;
                            }
                        } 
                        else {
                            for (i = 1; i < 9; i++)
                            {
                                newsheet.Cells[r, i].Interior.Color = System.Drawing.Color.FromArgb(112,173,71);//Verde
                                newsheet.Cells[r, i].Borders.LineStyle = XlLineStyle.xlContinuous;
                            }
                        } 
                    }
                    r++;
                }
                newsheet = (Worksheet)workbook.Worksheets.get_Item(InputCabecalho.Indice + 1);
                newsheet.Select();
                workbook.Save();
                Console.WriteLine($"Arquivo salvo com sucesso, no caminho {InputFilePath.Excel01}");
            }
            catch (Exception exHandle)
            {
                //Nao exibir mensagem quando usuario cancelar, aperta para nao substituir ou fechar a caixa de acao
                if (String.Compare("Exceção de HRESULT: 0x800A03EC", exHandle.Message) != 0)
                {
                    Console.WriteLine("Exception: " + exHandle.Message);
                }
                else {
                    Console.WriteLine("Usuario nao salvou o arquivo");
                }
            }
            finally
            {
                workbook.Close();
                ExcelApp.Quit();
                Marshal.ReleaseComObject(newsheet);
                Marshal.ReleaseComObject(worksheets);
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(ExcelApp);
                foreach (Process process in Process.GetProcessesByName("Excel"))
                {
                    process.Kill();
                }
            }
        }//Fim ExportExcel
        //Remover a Planilha 1  se o acabou de criar o excel
        public static void RemoveSheet(EstruturaArquivoCaminho InputFilePath)
        {
            ExcelApp = new Application();
            Workbook workbook = null;
            Sheets worksheets = null;
            try {

                if (ExcelApp == null)
                {
                    //MessageBox.Show("Excel is not properly installed!!");
                    Console.WriteLine("Excel is not properly installed!!");
                    return;
                }
                ExcelApp.Visible = false;
                ExcelApp.DisplayAlerts = false;

                if (!(File.Exists(InputFilePath.Excel01)))
                {
                    Console.WriteLine("Arquivo não exite!!");
                    return;

                }
                workbook = ExcelApp.Workbooks.Open(InputFilePath.Excel01, 0, false, 5, "", "", false, XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                worksheets = workbook.Worksheets;

                //removing a worksheet using its sheet name
                for (int i = ExcelApp.ActiveWorkbook.Worksheets.Count; i > 0; i--)
                  {
                      Worksheet wkSheet = (Worksheet)ExcelApp.ActiveWorkbook.Worksheets[i];
                      if (String.Compare(wkSheet.Name, "Planilha1") == 0)
                      {
                          wkSheet.Delete();
                      }
                  }
                workbook.Save();
               
                Console.WriteLine($"Arquivo salvo com sucesso, no caminho {InputFilePath.Excel01}");
            }
            catch (Exception exHandle)
            {
                //Nao exibir mensagem quando usuario cancelar, aperta para nao substituir ou fechar a caixa de acao
                if (String.Compare("Exceção de HRESULT: 0x800A03EC", exHandle.Message) != 0)
                {
                    Console.WriteLine("Exception: " + exHandle.Message);
                }
                else
                {
                    Console.WriteLine("Usuario nao salvou o arquivo");
                }
            }
            finally
            {
                workbook.Close();
                ExcelApp.Quit();
                Marshal.ReleaseComObject(worksheets);
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(ExcelApp);
                foreach (Process process in Process.GetProcessesByName("Excel"))
                {
                    process.Kill();
                }
            }
        }//Fim RemoveSheet
        //Verificar se existe alguma planilha com aquele nome
        public static void VerifyWorkBook(EstruturaCabecalho InputCabecalho, EstruturaArquivoCaminho InputFilePath) {
            
            InputCabecalho.PlanilhaNova = 0;

            if (!(File.Exists(InputFilePath.Excel01)))
            {
                return;
            }

            ExcelApp = new Application();
            Workbook workbook = null;
            Sheets worksheets = null;

            try
            {
                if (ExcelApp == null)
                {
                    //MessageBox.Show("Excel is not properly installed!!");
                    Console.WriteLine("Excel is not properly installed!!");
                    InputCabecalho.PlanilhaNova = 1;
                }
                ExcelApp.Visible = false;
                ExcelApp.DisplayAlerts = false;
                workbook = ExcelApp.Workbooks.Open(InputFilePath.Excel01, 0, false, 5, "", "", false, XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                worksheets = workbook.Worksheets;

                //removing a worksheet using its sheet name
                for (int i = ExcelApp.ActiveWorkbook.Worksheets.Count; i > 0; i--)
                {
                    Worksheet wkSheet = (Worksheet)ExcelApp.ActiveWorkbook.Worksheets[i];
                    if (String.Compare(wkSheet.Name, InputCabecalho.PurchaseOrder) == 0)
                    {
                        InputCabecalho.PlanilhaNova = 1;

                    }
                }
            }
            catch (Exception exHandle)
            {
                //Nao exibir mensagem quando usuario cancelar, aperta para nao substituir ou fechar a caixa de acao
                if (String.Compare("Exceção de HRESULT: 0x800A03EC", exHandle.Message) != 0)
                {
                    Console.WriteLine("Exception: " + exHandle.Message);
                    InputCabecalho.PlanilhaNova = 1;
                }
            }
            finally
            {
                workbook.Close();
                ExcelApp.Quit();
                Marshal.ReleaseComObject(worksheets);
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(ExcelApp);
                foreach (Process process in Process.GetProcessesByName("Excel"))
                {
                    process.Kill();
                }
            }
        }//Fim VerifyWorkBook


        public static void ExportExcel01(List<EstruturaProduto> InputListaProduto, EstruturaArquivoCaminho InputFilePath)
        {
            ExcelApp = new Application();
            Workbook workbook = null;
            Sheets worksheets = null;
            int r;//r stands for ExcelRow and c for ExcelColumn 
            int i;
            int numSheet;//Numero de planilhas no Excel
            var newsheet = new Worksheet();
            int max = 7; 
            try
            {
                if (ExcelApp == null)
                {
                    //MessageBox.Show("Excel is not properly installed!!");
                    Console.WriteLine("Excel is not properly installed!!");
                    return;
                }
                ExcelApp.Visible = false;
                ExcelApp.DisplayAlerts = false;

                if (!(File.Exists(InputFilePath.Excel01)))
                {
                    workbook = ExcelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                    workbook.SaveAs(InputFilePath.Excel01);
                    workbook.Close();
                }

                workbook = ExcelApp.Workbooks.Open(InputFilePath.Excel01, 0, false, 5, "", "", false, XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                worksheets = workbook.Worksheets;
                numSheet = ExcelApp.ActiveWorkbook.Worksheets.Count;
                newsheet = (Worksheet)worksheets.Add(worksheets[numSheet], Type.Missing, Type.Missing, Type.Missing);
                
                //Produtos
                newsheet.Cells[1, 1] = "IdAmazon";
                newsheet.Cells[1, 2] = "IdPlanner";
                newsheet.Cells[1, 3] = "PreUniAmazon";
                newsheet.Cells[1, 4] = "PreUniPlanner";
                newsheet.Cells[1, 5] = "Quantidade";
                newsheet.Cells[1, 6] = "Observacao";
                r = 2;
                foreach (EstruturaProduto p in InputListaProduto)
                {
                    newsheet.Cells[r, 1] = p.IdAmazon;
                    newsheet.Cells[r, 2] = p.IdPlanner;
                    newsheet.Cells[r, 3] = p.PreUniAmazon;
                    newsheet.Cells[r, 4] = p.PreUniPlanner;
                    newsheet.Cells[r, 5] = p.Quantidade;
                    newsheet.Cells[r, 6] = p.Observacao;
                    
                    if (String.Compare(p.Observacao, ConstantsObservacao.NaoCadastrado) == 0)
                    {
                        for (i = 1; i < max; i++)
                        {
                            newsheet.Cells[r, i].Interior.Color = System.Drawing.Color.FromArgb(255, 255, 0);//Amarelo
                            newsheet.Cells[r, i].Borders.LineStyle = XlLineStyle.xlContinuous;
                        }
                    }
                    else
                    {
                        if (p.Observacao.StartsWith(ConstantsObservacao.ErroPreco))
                        {
                            for (i = 1; i < max; i++)
                            {
                                newsheet.Cells[r, i].Interior.Color = System.Drawing.Color.FromArgb(255, 37, 37);//Vermelho 
                                newsheet.Cells[r, i].Borders.LineStyle = XlLineStyle.xlContinuous;
                            }
                        }
                        else
                        {
                            for (i = 1; i < max; i++)
                            {
                                newsheet.Cells[r, i].Interior.Color = System.Drawing.Color.FromArgb(112, 173, 71);//Verde
                                newsheet.Cells[r, i].Borders.LineStyle = XlLineStyle.xlContinuous;
                            }
                        }
                    }
                    r++;
                }
                newsheet = (Worksheet)workbook.Worksheets.get_Item(1);
                newsheet.Select();
                workbook.Save();
                Console.WriteLine($"Arquivo salvo com sucesso, no caminho {InputFilePath.Excel01}");
            }
            catch (Exception exHandle)
            {
                //Nao exibir mensagem quando usuario cancelar, aperta para nao substituir ou fechar a caixa de acao
                if (String.Compare("Exceção de HRESULT: 0x800A03EC", exHandle.Message) != 0)
                {
                    Console.WriteLine("Exception: " + exHandle.Message);
                }
                else
                {
                    Console.WriteLine("Usuario nao salvou o arquivo");
                }
            }
            finally
            {
                workbook.Close();
                ExcelApp.Quit();
                Marshal.ReleaseComObject(newsheet);
                Marshal.ReleaseComObject(worksheets);
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(ExcelApp);
                foreach (Process process in Process.GetProcessesByName("Excel"))
                {
                    process.Kill();
                }
            }
        }//Fim ExportExcel

    }
}
