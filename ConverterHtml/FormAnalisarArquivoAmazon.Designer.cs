
using System.Windows.Forms;

namespace ConverterHtml
{
    partial class FormAnalisarArquivoAmazon
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelecionarArquivo = new System.Windows.Forms.Button();
            this.btnNomeArquivoExcel = new System.Windows.Forms.Button();
            this.tbSelecionarArquivo = new System.Windows.Forms.TextBox();
            this.tbNomeArquivoExcel = new System.Windows.Forms.TextBox();
            this.ofdSelecionarArquivo = new System.Windows.Forms.OpenFileDialog();
            this.sfdNomeArquivoExcel = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(108, 279);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(308, 279);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnSelecionarArquivo
            // 
            this.btnSelecionarArquivo.Location = new System.Drawing.Point(414, 78);
            this.btnSelecionarArquivo.Name = "btnSelecionarArquivo";
            this.btnSelecionarArquivo.Size = new System.Drawing.Size(75, 51);
            this.btnSelecionarArquivo.TabIndex = 0;
            this.btnSelecionarArquivo.Text = "Selecionar Arquivo";
            this.btnSelecionarArquivo.UseVisualStyleBackColor = true;
            this.btnSelecionarArquivo.Click += new System.EventHandler(this.BtnSelecionarArquivo_Click);
            // 
            // btnNomeArquivoExcel
            // 
            this.btnNomeArquivoExcel.Location = new System.Drawing.Point(414, 192);
            this.btnNomeArquivoExcel.Name = "btnNomeArquivoExcel";
            this.btnNomeArquivoExcel.Size = new System.Drawing.Size(75, 33);
            this.btnNomeArquivoExcel.TabIndex = 0;
            this.btnNomeArquivoExcel.Text = "Salvar em";
            this.btnNomeArquivoExcel.UseVisualStyleBackColor = true;
            this.btnNomeArquivoExcel.Click += new System.EventHandler(this.BtnNomeArquivoExcel_Click);
            // 
            // tbSelecionarArquivo
            // 
            this.tbSelecionarArquivo.Location = new System.Drawing.Point(60, 30);
            this.tbSelecionarArquivo.Multiline = true;
            this.tbSelecionarArquivo.Name = "tbSelecionarArquivo";
            this.tbSelecionarArquivo.Size = new System.Drawing.Size(348, 156);
            this.tbSelecionarArquivo.TabIndex = 1;
            this.tbSelecionarArquivo.TextChanged += new System.EventHandler(this.TbSelecionarArquivo_TextChanged);
            // 
            // tbNomeArquivoExcel
            // 
            this.tbNomeArquivoExcel.Location = new System.Drawing.Point(60, 192);
            this.tbNomeArquivoExcel.Multiline = true;
            this.tbNomeArquivoExcel.Name = "tbNomeArquivoExcel";
            this.tbNomeArquivoExcel.Size = new System.Drawing.Size(348, 33);
            this.tbNomeArquivoExcel.TabIndex = 1;
            this.tbNomeArquivoExcel.TextChanged += new System.EventHandler(this.TbNomeArquivoExecel_TextChanged);
            // 
            // ofdSelecionarArquivo
            // 
            this.ofdSelecionarArquivo.FileOk += new System.ComponentModel.CancelEventHandler(this.OfdSelecionarArquivo_FileOk);
            // 
            // sfdNomeArquivoExcel
            // 
            this.sfdNomeArquivoExcel.FileOk += new System.ComponentModel.CancelEventHandler(this.SfdNomeArquivoExcel_FileOk);
            // 
            // FormAnalisarArquivoAmazon
            // 
            this.ClientSize = new System.Drawing.Size(598, 419);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelecionarArquivo);
            this.Controls.Add(this.btnNomeArquivoExcel);
            this.Controls.Add(this.tbSelecionarArquivo);
            this.Controls.Add(this.tbNomeArquivoExcel);
            this.Name = "FormAnalisarArquivoAmazon";
            //this.Load += new System.EventHandler(this.Form_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        //Buttons
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnSelecionarArquivo;
        private System.Windows.Forms.Button btnNomeArquivoExcel;
        //Label
        
        //GroupBox
        
        //TextBox
        private System.Windows.Forms.TextBox tbSelecionarArquivo;
        private System.Windows.Forms.TextBox tbNomeArquivoExcel;
        //OpenFileDialog
        private System.Windows.Forms.OpenFileDialog ofdSelecionarArquivo;
        //SaveFileDialog
        private System.Windows.Forms.SaveFileDialog sfdNomeArquivoExcel;
        //FlowLayoutPanel

    }
}