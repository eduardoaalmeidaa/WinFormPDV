using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace PDV
{
    #region Formulario
    public partial class FrmLogin : Form
    {
        #region Funcao
        private void Login()
        {
            bool AcessoPermitido = false;
            try
            {
                if (TxtUsuario.Text == "")
                {
                    MessageBox.Show("Digite o Usuário", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtUsuario.Focus();
                }
                else if (TxtSenha.Text == "")
                {
                    MessageBox.Show("Digite uma Senha!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtSenha.Focus();
                }
                else
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection("Data Source=ALMEIDA;Initial Catalog=PDV;Integrated Security=True"))
                        {
                            connection.Open();
                            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE NomeUsuario = @NomeUsuario AND Senha = @Senha", connection))
                            {
                                cmd.Parameters.AddWithValue("@NomeUsuario", TxtUsuario.Text);
                                cmd.Parameters.AddWithValue("@Senha", TxtSenha.Text);
                                cmd.Parameters.AddWithValue("@Cargo", LblCargo.Text);

                                using (SqlDataReader dr = cmd.ExecuteReader())
                                {
                                    if (dr.HasRows)
                                    {
                                        if (dr.Read())
                                        {
                                            LblCargo.Text = dr["Cargo"].ToString();
                                            AcessoPermitido = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Falha ao conectar!" + Environment.NewLine + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (AcessoPermitido)
                    {
                        FrmPrincipal frm = new FrmPrincipal();
                        frm.UsuarioLogado = TxtUsuario.Text;
                        frm.CargoUsuario = LblCargo.Text;
                        frm.Show();
                        this.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Usuário ou Senha inválidos!", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        TxtUsuario.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LblOlhoFechado_Click(object sender, EventArgs e)
        {
            try
            {
                TxtSenha.PasswordChar = '\0';

                LblOlhoAberto.Visible = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LblOlhoAberto_Click(object sender, EventArgs e)
        {
            try
            {
                TxtSenha.PasswordChar = '*';

                LblOlhoAberto.Visible = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Login();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
