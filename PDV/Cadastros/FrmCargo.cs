using Newtonsoft.Json.Linq;
using PDV.Relatorios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PDV.Cadastros
{
    #region Formulario
    public partial class FrmCargo : Form
    {
        private SqlConnection conexao = new SqlConnection("Data Source=ALMEIDA;Initial Catalog=PDV;Integrated Security=True");

        #region Funcao
        private void ConfiguraLista()
        {
            try
            {
                DgCargo.Columns["IDCargo"].Visible = false;
                DgCargo.Columns["Cargo"].DisplayIndex = 1;

                DgCargo.Columns["Cargo"].HeaderText = "Cargo";

                DgCargo.Columns["Cargo"].Width = 160;

                DgCargo.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                DgCargo.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                DgCargo.BackgroundColor = Color.FromArgb(0, 64, 64);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AtualizaLista()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM FuncionarioCargo", conexao);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                DgCargo.DataSource = dt;
                ConfiguraLista();
                ContagemRegistros();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void IncluiCargo()
        {
            try
            {
                if (TxtCargo.Text == "")
                {
                    MessageBox.Show("Digite um nome para o Cargo!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtCargo.Focus();
                    return;
                }
                else
                {
                    using (SqlConnection connection = new SqlConnection("Data Source = ALMEIDA; Initial Catalog = PDV; Integrated Security = True"))
                    {
                        using (SqlCommand cmd = new SqlCommand("InserirFuncionarioCargo", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Cargo", TxtCargo.Text.Trim());

                            connection.Open();
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Cadastro realizado com sucesso!", "SUCESSO!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AtualizaLista();
                            LimpaCampos();
                            TxtCargo.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AlteraCargo()
        {
            try
            {
                if (TxtCargo.Text == "")
                {
                    MessageBox.Show("Digite um nome para o Cargo!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtCargo.Focus();
                    return;
                }
                else
                {
                    using (SqlConnection connection = new SqlConnection("Data Source = ALMEIDA; Initial Catalog = PDV; Integrated Security = True"))
                    {
                        connection.Open();

                        using (SqlCommand cmd = new SqlCommand("AlterarFuncionarioCargo", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@IDCargo", TxtID.Text.Trim());
                            cmd.Parameters.AddWithValue("@Cargo", TxtCargo.Text.Trim());

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Registro alterado com sucesso!", "SUCESSO!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AtualizaLista();
                            LimpaCampos();
                            TxtCargo.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DeletaCargo()
        {
            try
            {
                if (DgCargo.SelectedRows.Count > 0)
                {
                    int cargoID = Convert.ToInt32(DgCargo.SelectedRows[0].Cells["IDCargo"].Value);
                    string cargo = DgCargo.SelectedRows[0].Cells["Cargo"].Value.ToString();

                    if (MessageBox.Show("Deseja realmente deletar o Cargo: " + cargo + " ?", "EXCLUSÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        using (SqlConnection connection = new SqlConnection("Data Source = ALMEIDA; Initial Catalog = PDV; Integrated Security = True"))
                        {
                            connection.Open();

                            using (SqlCommand command = new SqlCommand("DeletarFuncionarioCargo", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@IDCargo", cargoID);
                                command.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Cargo deletado com sucesso!", "SUCESSO!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AtualizaLista();
                        LimpaCampos();
                        TxtCargo.Focus();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Selecione um Cargo na lista para excluir!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ContagemRegistros()
        {
            try
            {
                int quantidadeRegistros = DgCargo.RowCount;
                LblRegistros.Text = $"{quantidadeRegistros}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LocalizaCampos()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=ALMEIDA;Initial Catalog=PDV;Integrated Security=True"))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM FuncionarioCargo WHERE Cargo LIKE @Cargo ORDER BY Cargo ASC", connection))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Cargo", TxtLocalizar.Text + "%");

                        connection.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            DgCargo.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void PreencheCamposComDadosCargoAtual()
        {
            try
            {
                TxtID.Text = DgCargo.CurrentRow.Cells[0].Value?.ToString();
                TxtCargo.Text = DgCargo.CurrentRow.Cells[1].Value?.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LimpaCampos()
        {
            try
            {
                TxtCargo.Text = string.Empty;
                TxtLocalizar.Text = string.Empty;

                BtnIncluir.Enabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        public FrmCargo()
        {
            InitializeComponent();
        }

        private void FrmCargo_Load(object sender, EventArgs e)
        {
            try
            {
                AtualizaLista();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void FrmCargo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    BtnDeletar.PerformClick();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    this.Dispose();
                }

                if (DgCargo.SelectedRows.Count > 0)
                {
                    if (DgCargo.CurrentRow != null)
                    {
                        // seta para cima
                        if (e.KeyCode == Keys.Up)
                        {
                            int currentIndex = DgCargo.CurrentRow.Index;

                            if (currentIndex > 0)
                            {
                                int previousRowIndex = currentIndex - 1;
                                DgCargo.CurrentCell = DgCargo.Rows[previousRowIndex].Cells[DgCargo.CurrentCell.ColumnIndex];
                                PreencheCamposComDadosCargoAtual();

                                e.Handled = true;
                            }
                        }
                        // seta para baixo
                        else if (e.KeyCode == Keys.Down)
                        {
                            int currentIndex = DgCargo.CurrentRow.Index;

                            if (currentIndex < DgCargo.Rows.Count - 1)
                            {
                                int nextRowIndex = currentIndex + 1;
                                DgCargo.CurrentCell = DgCargo.Rows[nextRowIndex].Cells[DgCargo.CurrentCell.ColumnIndex];
                                PreencheCamposComDadosCargoAtual();

                                e.Handled = true;
                            }
                        }
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void TxtLocalizar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LocalizaCampos();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DgCargo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                PreencheCamposComDadosCargoAtual();
                BtnIncluir.Enabled = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                AtualizaLista();
                TxtCargo.Text = "";
                BtnIncluir.Enabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                IncluiCargo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                AlteraCargo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnDeletar_Click(object sender, EventArgs e)
        {
            try
            {
                DeletaCargo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnLimpar_Click(object sender, EventArgs e)
        {
            try
            {
                LimpaCampos();
                TxtCargo.Focus();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    #endregion
}
