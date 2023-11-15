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
    public partial class FrmFuncionarioRelatorio : Form
    {
        DataTable dt = new DataTable();

        public FrmFuncionarioRelatorio(DataTable dt)
        {
            InitializeComponent();
            this.dt = dt;
        }

        private void FrmFuncionarioRelatorio_Load(object sender, EventArgs e)
        {
            this.RViewerFuncionario.LocalReport.DataSources.Clear();
            this.RViewerFuncionario.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSetFuncionario", dt));
            this.RViewerFuncionario.RefreshReport();
        }

        private void FrmFuncionarioRelatorio_KeyDown(object sender, KeyEventArgs e)
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
