namespace PDV.Relatorios
{
    partial class FrmClienteRelatorio
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
            this.RViewerCliente = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // RViewerCliente
            // 
            this.RViewerCliente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RViewerCliente.LocalReport.ReportEmbeddedResource = "PDV.Relatorios.RelatorioCliente.rdlc";
            this.RViewerCliente.Location = new System.Drawing.Point(0, 0);
            this.RViewerCliente.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.RViewerCliente.Name = "RViewerCliente";
            this.RViewerCliente.ServerReport.BearerToken = null;
            this.RViewerCliente.Size = new System.Drawing.Size(933, 450);
            this.RViewerCliente.TabIndex = 1;
            // 
            // FrmClienteRelatorio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 450);
            this.Controls.Add(this.RViewerCliente);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FrmClienteRelatorio";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relatorio Cliente";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmClienteRelatorio_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmClienteRelatorio_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer RViewerCliente;
    }
}