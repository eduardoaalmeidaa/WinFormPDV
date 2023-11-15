using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDV.Relatorios
{
    #region Formulario
    public partial class FrmClienteRelatorio : Form
    {
        DataTable dt = new DataTable();

        public FrmClienteRelatorio(DataTable dt)
        {
            InitializeComponent();
            this.dt = dt;
        }

        private void FrmClienteRelatorio_Load(object sender, EventArgs e)
        {
            this.RViewerCliente.LocalReport.DataSources.Clear();
            this.RViewerCliente.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSetCliente", dt));
            this.RViewerCliente.RefreshReport();
        }

        private void FrmClienteRelatorio_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    #endregion
}
