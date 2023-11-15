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

namespace PDV.Cadastros
{
    #region Formulario
    public partial class FrmUsuario : Form
    {
        private SqlConnection conexao = new SqlConnection("Data Source=ALMEIDA;Initial Catalog=PDV;Integrated Security=True");

        #region Funcao
        private void ConfiguraLista()
        {
            try
            {
                DgUsuario.Columns["IDUsuario"].Visible = false;
                DgUsuario.Columns["NomeUsuario"].DisplayIndex = 1;
                DgUsuario.Columns["Senha"].DisplayIndex = 2;
                DgUsuario.Columns["DataCadastro"].DisplayIndex = 4;
                DgUsuario.Columns["Cargo"].DisplayIndex = 3;

                DgUsuario.Columns["NomeUsuario"].HeaderText = "Usuário";
                DgUsuario.Columns["Senha"].HeaderText = "Senha";
                DgUsuario.Columns["DataCadastro"].HeaderText = "Cadastro";
                DgUsuario.Columns["Cargo"].HeaderText = "Cargo";

                DgUsuario.Columns["NomeUsuario"].Width = 90;
                DgUsuario.Columns["Senha"].Width = 90;
                DgUsuario.Columns["DataCadastro"].Width = 50;
                DgUsuario.Columns["Cargo"].Width = 250;

                DgUsuario.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                DgUsuario.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                DgUsuario.BackgroundColor = Color.FromArgb(0, 64, 64);
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario", conexao);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                DgUsuario.DataSource = dt;
                ConfiguraLista();
                CarregaComboCargo();
                ContagemRegistros();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void IncluiUsuario()
        {
            try
            {
                if (TxtUsuario.Text == "")
                {
                    MessageBox.Show("Digite um nome de Usuário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtUsuario.Focus();
                    return;
                }
                else if (TxtSenha.Text == "")
                {
                    MessageBox.Show("Digite uma senha para o Usuário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtSenha.Focus();
                    return;
                }
                else if (CmbCargo.Text == "")
                {
                    MessageBox.Show("Selecione o Cargo do Usuário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CmbCargo.Focus();
                    return;
                }
                else
                {
                    using (SqlConnection connection = new SqlConnection("Data Source = ALMEIDA; Initial Catalog = PDV; Integrated Security = True"))
                    {
                        using (SqlCommand cmd = new SqlCommand("InserirUsuario", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@NomeUsuario", TxtUsuario.Text.Trim());
                            cmd.Parameters.AddWithValue("@Senha", TxtSenha.Text.Trim());
                            string dataTexto = DtData.Text;
                            string dataFormatada;
                            if (DateTime.TryParse(dataTexto, out _))
                            {
                                dataFormatada = DateTime.Parse(dataTexto).ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                MessageBox.Show("A data não está em um formato válido.", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            cmd.Parameters.AddWithValue("@DataCadastro", dataFormatada);
                            cmd.Parameters.AddWithValue("@Cargo", CmbCargo.Text.Trim());

                            connection.Open();
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Cadastro realizado com sucesso!", "SUCESSO!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AtualizaLista();
                            LimpaCampos();
                            TxtUsuario.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AlteraUsuario()
        {
            try
            {
                if (TxtUsuario.Text == "")
                {
                    MessageBox.Show("Digite um nome de Usuário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtUsuario.Focus();
                    return;
                }
                else if (TxtSenha.Text == "")
                {
                    MessageBox.Show("Digite uma senha para o Usuário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtSenha.Focus();
                    return;
                }
                else if (CmbCargo.Text == "")
                {
                    MessageBox.Show("Selecione o Cargo do Usuário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CmbCargo.Focus();
                    return;
                }
                else
                {
                    using (SqlConnection connection = new SqlConnection("Data Source = ALMEIDA; Initial Catalog = PDV; Integrated Security = True"))
                    {
                        connection.Open();

                        using (SqlCommand cmd = new SqlCommand("AlterarUsuario", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@IDUsuario", TxtID.Text.Trim());
                            cmd.Parameters.AddWithValue("@NomeUsuario", TxtUsuario.Text.Trim());
                            cmd.Parameters.AddWithValue("@Senha", TxtSenha.Text.Trim());
                            string dataTexto = DtData.Text;
                            if (DateTime.TryParse(dataTexto, out DateTime dataFormatada))
                            {
                                cmd.Parameters.AddWithValue("@DataCadastro", dataFormatada.ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                MessageBox.Show("A data não está em um formato válido.", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            cmd.Parameters.AddWithValue("@Cargo", CmbCargo.Text.Trim());

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Registro alterado com sucesso!", "SUCESSO!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AtualizaLista();
                            LimpaCampos();
                            TxtUsuario.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DeletaUsuario()
        {
            try
            {
                if (DgUsuario.SelectedRows.Count > 0)
                {
                    int usuarioID = Convert.ToInt32(DgUsuario.SelectedRows[0].Cells["IDUsuario"].Value);
                    string usuario = DgUsuario.SelectedRows[0].Cells["NomeUsuario"].Value.ToString();

                    if (MessageBox.Show("Deseja realmente deletar o Usuário: " + usuario + " ?", "EXCLUSÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        using (SqlConnection connection = new SqlConnection("Data Source = ALMEIDA; Initial Catalog = PDV; Integrated Security = True"))
                        {
                            connection.Open();

                            using (SqlCommand command = new SqlCommand("DeletarUsuario", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@IDUsuario", usuarioID);
                                command.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Usuário deletado com sucesso!", "SUCESSO!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AtualizaLista();
                        LimpaCampos();
                        TxtUsuario.Focus();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Selecione um Usuário na lista para excluir!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CarregaComboCargo()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=ALMEIDA;Initial Catalog=PDV;Integrated Security=True"))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM FuncionarioCargo", connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        CmbCargo.DataSource = table;
                        CmbCargo.DisplayMember = "Cargo";
                        CmbCargo.ValueMember = "IDCargo";
                    }
                }
                CmbCargo.SelectedIndex = 0;
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
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE NomeUsuario LIKE @NomeUsuario ORDER BY NomeUsuario ASC", connection))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@NomeUsuario", TxtLocalizar.Text + "%");

                        connection.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            DgUsuario.DataSource = dt;
                        }
                    }
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
                int quantidadeRegistros = DgUsuario.RowCount;
                LblRegistros.Text = $"{quantidadeRegistros}";
            }
            catch (Exception ex)
            {

            }
        }

        private void PreencheCamposComDadosUsuarioAtual()
        {
            try
            {
                TxtID.Text = DgUsuario.CurrentRow.Cells[0].Value?.ToString();
                TxtUsuario.Text = DgUsuario.CurrentRow.Cells[1].Value?.ToString();
                TxtSenha.Text = DgUsuario.CurrentRow.Cells[2].Value?.ToString();
                CmbCargo.Text = DgUsuario.CurrentRow.Cells[4].Value?.ToString();
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
                TxtUsuario.Text = string.Empty;
                TxtSenha.Text = string.Empty;
                CmbCargo.Text = string.Empty;
                TxtLocalizar.Text = string.Empty;

                BtnIncluir.Enabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        public FrmUsuario()
        {
            InitializeComponent();
        }

        private void FrmUsuario_Load(object sender, EventArgs e)
        {
            try
            {
                AtualizaLista();
                DtData.Value = DateTime.Now;
                TxtLocalizar.TextChanged += TxtLocalizar_TextChanged;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void FrmUsuario_KeyDown(object sender, KeyEventArgs e)
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

                if (DgUsuario.SelectedRows.Count > 0)
                {
                    if (DgUsuario.CurrentRow != null)
                    {
                        // seta para cima
                        if (e.KeyCode == Keys.Up)
                        {
                            int currentIndex = DgUsuario.CurrentRow.Index;

                            if (currentIndex > 0)
                            {
                                int previousRowIndex = currentIndex - 1;
                                DgUsuario.CurrentCell = DgUsuario.Rows[previousRowIndex].Cells[DgUsuario.CurrentCell.ColumnIndex];
                                PreencheCamposComDadosUsuarioAtual();

                                e.Handled = true;
                            }
                        }
                        // seta para baixo
                        else if (e.KeyCode == Keys.Down)
                        {
                            int currentIndex = DgUsuario.CurrentRow.Index;

                            if (currentIndex < DgUsuario.Rows.Count - 1)
                            {
                                int nextRowIndex = currentIndex + 1;
                                DgUsuario.CurrentCell = DgUsuario.Rows[nextRowIndex].Cells[DgUsuario.CurrentCell.ColumnIndex];
                                PreencheCamposComDadosUsuarioAtual();

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

        private void DgUsuario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                PreencheCamposComDadosUsuarioAtual();
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
                LimpaCampos();
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
                IncluiUsuario();
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
                AlteraUsuario();
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
                DeletaUsuario();
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
                TxtUsuario.Focus();
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
