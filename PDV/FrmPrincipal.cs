using PDV.Cadastros;
using PDV.Relatorios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDV
{
    #region Formulario
    public partial class FrmPrincipal : Form
    {
        public string UsuarioLogado { get; set; }
        public string CargoUsuario { get; set; }

        #region Funcao
        private void CarregaProgressBar()
        {
            PgBarPrincipal.Invoke(new Action(() => PgBarPrincipal.Maximum = 5));

            for (int i = 0; i <= 5; i++)
            {
                PgBarPrincipal.Invoke(new Action(() => PgBarPrincipal.Value = i));
                System.Threading.Thread.Sleep(50);
            }
            PgBarPrincipal.Invoke(new Action(() => PgBarPrincipal.Visible = false));
        }
        #endregion

        public FrmPrincipal()
        {
            InitializeComponent();
            Load += FrmPrincipal_Load;
            //PicBoxFuncionarios.Click += PicBoxFuncionarios_Click;
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            try
            {
                LblUsuario.Text = UsuarioLogado;
                LblCargo.Text = CargoUsuario;

                LblData.Text = DateTime.Now.ToString("dd/MM/yyyy");
                LblHora.Text = DateTime.Now.ToString("HH:mm:ss");

                Thread thread = new Thread(new ThreadStart(CarregaProgressBar));
                thread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void MenuTrocarDeUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();

                FrmLogin myFrmLogin = new FrmLogin();
                if (myFrmLogin.ShowDialog() == DialogResult.OK)
                {
                    FrmPrincipal myFrmPrincipal = new FrmPrincipal();
                    myFrmPrincipal.Show();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void MenuSair_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Sair do sistema?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    this.Close();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void PicBoxCalculadora_Click(object sender, EventArgs e)
        {
            try
            {
                PgBarPrincipal.Visible = true;

                Thread thread = new Thread(new ThreadStart(CarregaProgressBar));
                thread.Start();

                using (FrmCalculadora frm = new FrmCalculadora())
                {
                    frm.ShowDialog();
                }

                PgBarPrincipal.Invoke(new Action(() => PgBarPrincipal.Visible = false));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void PicBoxRelatorios_Click(object sender, EventArgs e)
        {
            try
            {
                if (GBoxRelatorios.Visible)
                {
                    GBoxRelatorios.Visible = false;
                }
                else
                {
                    GBoxRelatorios.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LblRelatoriosClientes_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    FrmCliente frmCliente = new FrmCliente();

            //    var dt = frmCliente.ImprimeRelatorio();

            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        using (Relatorios.FrmClienteRelatorio frmClienteRelatorio = new Relatorios.FrmClienteRelatorio(dt))
            //        {
            //            frmClienteRelatorio.ShowDialog();
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Não há dados para exibir no Relatório!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        private void LblRelatoriosFuncionarios_Click(object sender, EventArgs e)
        {
            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        private void PicBoxRelatoriosExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (GBoxExcel.Visible)
                {
                    GBoxExcel.Visible = false;
                }
                else
                {
                    GBoxExcel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LblExcelClientes_Click(object sender, EventArgs e)
        {
            try
            {
                using (Cadastros.FrmCliente frm = new Cadastros.FrmCliente())
                {
                    frm.Show();
                    frm.Visible = false;
                    DataGridView dgFuncionario = frm.GetDataGridView();
                    frm.GeraDadosClienteEXCEL();
                    frm.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LblExcelFuncionarios_Click(object sender, EventArgs e)
        {
            try
            {
                using (Cadastros.FrmFuncionario frm = new Cadastros.FrmFuncionario())
                {
                    frm.Show();
                    frm.Visible = false;
                    DataGridView dgFuncionario = frm.GetDataGridView();
                    frm.GeraDadosFuncionarioEXCEL();
                    frm.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void PicBoxUsuarios_Click(object sender, EventArgs e)
        {
            try
            {
                PgBarPrincipal.Visible = true;

                Thread thread = new Thread(new ThreadStart(CarregaProgressBar));
                thread.Start();

                using (Cadastros.FrmUsuario frm = new Cadastros.FrmUsuario())
                {
                    frm.ShowDialog();
                }

                PgBarPrincipal.Invoke(new Action(() => PgBarPrincipal.Visible = false));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void PicBoxClientes_Click(object sender, EventArgs e)
        {
            try
            {
                PgBarPrincipal.Visible = true;

                Thread thread = new Thread(new ThreadStart(CarregaProgressBar));
                thread.Start();

                using (Cadastros.FrmCliente frm = new Cadastros.FrmCliente())
                {
                    frm.ShowDialog();
                }

                PgBarPrincipal.Invoke(new Action(() => PgBarPrincipal.Visible = false));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void PicBoxFuncionarios_Click(object sender, EventArgs e)
        {
            try
            {
                PgBarPrincipal.Visible = true;

                Thread thread = new Thread(new ThreadStart(CarregaProgressBar));
                thread.Start();

                using (Cadastros.FrmFuncionario frm = new Cadastros.FrmFuncionario())
                {
                    frm.ShowDialog();
                }

                PgBarPrincipal.Invoke(new Action(() => PgBarPrincipal.Visible = false));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void PicBoxCargoFuncionario_Click(object sender, EventArgs e)
        {
            try
            {
                PgBarPrincipal.Visible = true;

                Thread thread = new Thread(new ThreadStart(CarregaProgressBar));
                thread.Start();

                using (Cadastros.FrmCargo frm = new Cadastros.FrmCargo())
                {
                    frm.ShowDialog();
                }

                PgBarPrincipal.Invoke(new Action(() => PgBarPrincipal.Visible = false));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void PicBoxSair_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Sair do sistema?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    this.Close();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void TimerDataHora_Tick(object sender, EventArgs e)
        {
            try
            {
                LblData.Text = DateTime.Now.ToString("dd/MM/yyyy");
                LblHora.Text = DateTime.Now.ToString("HH:mm:ss");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    #endregion
}
