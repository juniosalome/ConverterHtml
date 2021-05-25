using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Security;
using static ConverterHtml.Entity;
using static ConverterHtml.MethodPersistence;
using static ConverterHtml.ExcelMethod;
using static ConverterHtml.GenericMethod;
using static ConverterHtml.Constants;

namespace ConverterHtml
{
    public partial class FormAnalisarArquivoAmazon : Form
    {
        List<string> duplicada = new List<string> { };
        EstruturaArquivoCaminho FilePath = new EstruturaArquivoCaminho
        {
            Read01 = new List<string> { },
            Read02 = "",
            Write01 = "",
            Write02 = "",
            Excel01 = "",
            Excel02 = "",
        };

        public FormAnalisarArquivoAmazon()
        {
            InitializeComponent();
        }
        private void BtnSelecionarArquivo_Click(object sender, EventArgs e)
        {
            //define as propriedades do controle 
            //OpenFileDialog
            this.ofdSelecionarArquivo.Multiselect = true;
            this.ofdSelecionarArquivo.Title = "Selecionar Arquivos";
            ofdSelecionarArquivo.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //filtra para exibir somente arquivos html
            ofdSelecionarArquivo.Filter = "All Files *.html | *.html";
            ofdSelecionarArquivo.CheckFileExists = true;
            ofdSelecionarArquivo.CheckPathExists = true;
            ofdSelecionarArquivo.FilterIndex = 2;
            ofdSelecionarArquivo.RestoreDirectory = true;
            ofdSelecionarArquivo.ReadOnlyChecked = true;
            ofdSelecionarArquivo.ShowReadOnly = true;
            DialogResult dr = this.ofdSelecionarArquivo.ShowDialog();
            if (dr == DialogResult.OK)
            {
                // Le os arquivos selecionados 
                foreach (String arquivo in ofdSelecionarArquivo.FileNames)
                {
                    // cria um PictureBox
                    try
                    {
                        tbSelecionarArquivo.Text += arquivo+"; ";
                        duplicada.Add(Convert.ToString(arquivo));
                    }
                    catch (SecurityException ex)
                    {
                        // O usuário  não possui permissão para ler arquivos
                        MessageBox.Show("Erro de segurança Contate o administrador de segurança da rede.\n\n" +
                                                    "Mensagem : " + ex.Message + "\n\n" +
                                                    "Detalhes (enviar ao suporte):\n\n" + ex.StackTrace);
                    }
                }
            }
        }//fim e evento botão
        private void BtnNomeArquivoExcel_Click(object sender, EventArgs e)
        {
            //define as propriedades do controle 
            //OpenFileDialog
            this.sfdNomeArquivoExcel.Title = "Caminho do Arquivo";
            sfdNomeArquivoExcel.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //filtra para exibir somente arquivos xlsx
            sfdNomeArquivoExcel.Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            sfdNomeArquivoExcel.CheckFileExists = false;
            sfdNomeArquivoExcel.CheckPathExists = true;
            sfdNomeArquivoExcel.FilterIndex = 2;
            sfdNomeArquivoExcel.RestoreDirectory = true;
            sfdNomeArquivoExcel.DefaultExt = "xlsx";
            DialogResult dr = this.sfdNomeArquivoExcel.ShowDialog();
            if (dr == DialogResult.OK) {
                tbNomeArquivoExcel.Text = sfdNomeArquivoExcel.FileName;
            }
        }
            private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnOK_Click(object sender, EventArgs e)
         {
            FilePath.Read01 = duplicada.Distinct().ToList();
            FilePath.Excel01 = Convert.ToString(tbNomeArquivoExcel.Text);
            if (FilePath.Read01 != null && FilePath.Excel01 != null 
                && FilePath.Read01.Count != 0 && String.Compare(FilePath.Excel01,"")!=0 )
            {
                try
                {
                    //ProcessFile.FileOption(FilePath, ConstantsFile.ReadFileList);
                    ProcessFile.FileOption(FilePath, ConstantsFile.ReadFileList);
                    MessageBox.Show("Acabou o processo!!!");
                }//Fim Try
                finally
                {
                    ProdutoDB.CloseConnection();
                    tbSelecionarArquivo.Text = "";
                    tbNomeArquivoExcel.Text = "";
                    duplicada.Clear();
                }
            }
            else
            {
                MessageBox.Show("Precisa de pelo menos um Html e o caminho para a planilha");
            }
        }
        private void TbSelecionarArquivo_TextChanged(object sender, EventArgs e)
        {

        }
        private void TbNomeArquivoExecel_TextChanged(object sender, EventArgs e)
        {

        }
        private void OfdSelecionarArquivo_FileOk(object sender, CancelEventArgs e)
        {

        }
        private void SfdNomeArquivoExcel_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
