namespace PDV.Relatorios
{
    partial class FrmFuncionarioRelatorio
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
            this.RViewerFuncionario = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // RViewerFuncionario
            // 
            this.RViewerFuncionario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RViewerFuncionario.LocalReport.ReportEmbeddedResource = "PDV.Relatorios.RelatorioFuncionario.rdlc";
            this.RViewerFuncionario.Location = new System.Drawing.Point(0, 0);
            this.RViewerFuncionario.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RViewerFuncionario.Name = "RViewerFuncionario";
            this.RViewerFuncionario.ServerReport.BearerToken = null;
            this.RViewerFuncionario.Size = new System.Drawing.Size(933, 450);
            this.RViewerFuncionario.TabIndex = 0;
            // 
            // FrmFuncionarioRelatorio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 450);
            this.Controls.Add(this.RViewerFuncionario);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FrmFuncionarioRelatorio";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relatorio Funcionário";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmFuncionarioRelatorio_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmFuncionarioRelatorio_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer RViewerFuncionario;
    }
}