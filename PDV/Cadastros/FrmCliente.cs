using PDV.Relatorios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace PDV.Cadastros
{
    #region Formulario
    public partial class FrmCliente : Form
    {
        private SqlConnection conexao = new SqlConnection("Data Source=ALMEIDA;Initial Catalog=PDV;Integrated Security=True");

        string cpfAntigo;
        string foto;

        #region Funcao
        public DataGridView GetDataGridView()
        {
            return DgCliente;
        }

        private void ConfiguraLista()
        {
            try
            {
                DgCliente.Columns["IDCliente"].Visible = false;
                DgCliente.Columns["Codigo"].Visible = false;
                DgCliente.Columns["Nome"].DisplayIndex = 2;
                DgCliente.Columns["Cpf"].DisplayIndex = 3;
                DgCliente.Columns["ValorAberto"].DisplayIndex = 4;
                DgCliente.Columns["Telefone"].DisplayIndex = 5;
                DgCliente.Columns["Email"].DisplayIndex = 6;
                DgCliente.Columns["StatusCliente"].DisplayIndex = 7;
                DgCliente.Columns["Inadimplente"].DisplayIndex = 8;
                DgCliente.Columns["Endereco"].DisplayIndex = 9;
                DgCliente.Columns["Foto"].Visible = false;
                DgCliente.Columns["DataCadastro"].DisplayIndex = 11;

                //DgCliente.Columns["Codigo"].HeaderText = "Código";
                DgCliente.Columns["Nome"].HeaderText = "Nome";
                DgCliente.Columns["Cpf"].HeaderText = "CPF";
                DgCliente.Columns["ValorAberto"].HeaderText = "Valor Aberto";
                DgCliente.Columns["Telefone"].HeaderText = "Telefone";
                DgCliente.Columns["Email"].HeaderText = "Email";
                DgCliente.Columns["StatusCliente"].HeaderText = "Status Cliente";
                DgCliente.Columns["Inadimplente"].HeaderText = "Inadimplente";
                DgCliente.Columns["Endereco"].HeaderText = "Endereço";
                DgCliente.Columns["DataCadastro"].HeaderText = "Cadastro";

                //DgCliente.Columns["Codigo"].Width = 28;
                DgCliente.Columns["Nome"].Width = 80;
                DgCliente.Columns["Cpf"].Width = 50;
                DgCliente.Columns["ValorAberto"].Width = 45;
                DgCliente.Columns["Telefone"].Width = 50;
                DgCliente.Columns["Email"].Width = 100;
                DgCliente.Columns["StatusCliente"].Width = 50;
                DgCliente.Columns["Inadimplente"].Width = 30;
                DgCliente.Columns["Endereco"].Width = 100;
                DgCliente.Columns["DataCadastro"].Width = 100;

                DgCliente.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                DgCliente.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                DgCliente.BackgroundColor = Color.FromArgb(0, 64, 64);
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Cliente", conexao);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                DgCliente.DataSource = dt;
                ConfiguraLista();
                ContagemRegistros();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void IncluiCliente()
        {
            try
            {
                string cpf = TxtCpf.Text;
                string email = TxtEmail.Text;

                if (TxtNome.Text == "")
                {
                    MessageBox.Show("Digite o Nome do Cliente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtNome.Focus();
                    return;
                }
                //else if (TxtCod.Text == "")
                //{
                //    MessageBox.Show("Digite o Código do Cliente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    TxtCod.Focus();
                //    return;
                //}
                else if (TxtCpf.Text == "")
                {
                    MessageBox.Show("Digite o CPF do Cliente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtCpf.Focus();
                    return;
                }
                else if (cpf.Length < 14)
                {
                    MessageBox.Show("CPF deve conter 11 dígitos!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtCpf.Focus();
                    return;
                }
                else if (TxtEmail.Text == "")
                {
                    MessageBox.Show("Digite o Email do Cliente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtEmail.Focus();
                    return;
                }
                else if (!email.Contains('@'))
                {
                    MessageBox.Show("Digite um Domínio para o Email!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtEmail.Focus();
                    return;
                }
                else if (TxtEndereco.Text == "")
                {
                    MessageBox.Show("Digite o Endereço do Cliente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtEndereco.Focus();
                    return;
                }
                else if (TxtValorAberto.Text == "")
                {
                    MessageBox.Show("Digite um valor aberto!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtValorAberto.Focus();
                    return;
                }
                else if (CmbInadimplente.Text == "")
                {
                    MessageBox.Show("Selecione se é Inadimplente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CmbInadimplente.Focus();
                    return;
                }
                else if (ChBoxDesbloqueado.Checked == false && ChBoxBloqueado.Checked == false)
                {
                    MessageBox.Show("Selecione o Status do Cliente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    GBoxStatusDoCliente.Focus();
                    return;
                }
                else if (PicBoxImagemCli.Image == null)
                {
                    MessageBox.Show("Selecione uma imagem para o Cliente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    PicBoxImagemCli.Focus();
                    return;
                }
                else
                {
                    string novoCpf = TxtCpf.Text.Trim();

                    using (SqlConnection connection = new SqlConnection("Data Source = ALMEIDA; Initial Catalog = PDV; Integrated Security = True"))
                    {
                        using (SqlCommand cmd = new SqlCommand("InserirCliente", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Codigo", TxtCod.Text.Trim());
                            cmd.Parameters.AddWithValue("@Nome", TxtNome.Text.Trim());
                            cmd.Parameters.AddWithValue("@Cpf", TxtCpf.Text.Trim());
                            cmd.Parameters.AddWithValue("@ValorAberto", TxtValorAberto.Text.Trim());
                            cmd.Parameters.AddWithValue("@Telefone", TxtTelefone.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", TxtEmail.Text.Trim());
                            cmd.Parameters.AddWithValue("@StatusCliente", ChBoxDesbloqueado.Checked ? "Desbloqueado" : "Bloqueado");
                            cmd.Parameters.AddWithValue("@Inadimplente", CmbInadimplente.Text.Trim());
                            cmd.Parameters.AddWithValue("@Endereco", TxtEndereco.Text.Trim());
                            cmd.Parameters.AddWithValue("@Foto", CarregaImagem());
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

                            //Verifica se ha CPF duplicado.
                            if (novoCpf == cpfAntigo)
                            {
                                using (SqlCommand cmdVerifica = new SqlCommand("SELECT * FROM Cliente WHERE Cpf = @Cpf", connection))
                                {
                                    cmdVerifica.Parameters.AddWithValue("@Cpf", novoCpf);

                                    DataTable dt = new DataTable();
                                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmdVerifica))
                                    {
                                        adapter.Fill(dt);
                                    }

                                    if (dt.Rows.Count > 0)
                                    {
                                        MessageBox.Show("CPF já cadastrado!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        TxtCpf.Text = "";
                                        TxtCpf.Focus();
                                        return;
                                    }
                                }
                            }

                            connection.Open();
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Cadastro realizado com sucesso!", "SUCESSO!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AtualizaLista();
                            LimpaCampos();
                            TxtNome.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AlteraCliente()
        {
            try
            {
                string cpf = TxtCpf.Text;
                string email = TxtEmail.Text;

                if (TxtNome.Text == "")
                {
                    MessageBox.Show("Digite o Nome do Cliente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtNome.Focus();
                    return;
                }
                //else if (TxtCod.Text == "")
                //{
                //    MessageBox.Show("Digite o Código do Cliente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    TxtCpf.Focus();
                //    return;
                //}
                else if (TxtCpf.Text == "")
                {
                    MessageBox.Show("Digite o CPF do Cliente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtCpf.Focus();
                    return;
                }
                else if (cpf.Length < 14)
                {
                    MessageBox.Show("CPF deve conter 11 dígitos!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtCpf.Focus();
                    return;
                }
                else if (TxtEmail.Text == "")
                {
                    MessageBox.Show("Digite o Email do Cliente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtEmail.Focus();
                    return;
                }
                else if (!email.Contains('@'))
                {
                    MessageBox.Show("Digite um Domínio para o Email!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtEmail.Focus();
                    return;
                }
                else if (TxtEndereco.Text == "")
                {
                    MessageBox.Show("Digite o Endereço do Cliente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtEndereco.Focus();
                    return;
                }
                else if (TxtValorAberto.Text == "")
                {
                    MessageBox.Show("Digite um valor aberto!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtValorAberto.Focus();
                    return;
                }
                else if (CmbInadimplente.Text == "")
                {
                    MessageBox.Show("Selecione se é Inadimplente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CmbInadimplente.Focus();
                    return;
                }
                else if (ChBoxDesbloqueado.Checked == false && ChBoxBloqueado.Checked == false)
                {
                    MessageBox.Show("Selecione o Status do Cliente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    GBoxStatusDoCliente.Focus();
                    return;
                }
                else if (PicBoxImagemCli.Image == null)
                {
                    MessageBox.Show("Selecione uma imagem para o Cliente!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    PicBoxImagemCli.Focus();
                    return;
                }
                else
                {
                    string novoCpf = TxtCpf.Text.Trim();
                    Image novaImg = PicBoxImagemCli.Image;

                    using (SqlConnection connection = new SqlConnection("Data Source = ALMEIDA; Initial Catalog = PDV; Integrated Security = True"))
                    {
                        connection.Open();

                        using (SqlCommand cmd = new SqlCommand("AlterarCliente", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@IDCliente", TxtID.Text.Trim());
                            cmd.Parameters.AddWithValue("@Codigo", TxtCod.Text.Trim());
                            cmd.Parameters.AddWithValue("@Nome", TxtNome.Text.Trim());
                            cmd.Parameters.AddWithValue("@Cpf", TxtCpf.Text.Trim());
                            decimal valorAberto;
                            if (decimal.TryParse(TxtValorAberto.Text.Trim(), out valorAberto))
                            {
                                cmd.Parameters.AddWithValue("@ValorAberto", valorAberto);
                            }
                            else
                            {
                                MessageBox.Show("O valor aberto não é um número válido.", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            cmd.Parameters.AddWithValue("@Telefone", TxtTelefone.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", TxtEmail.Text.Trim());
                            cmd.Parameters.AddWithValue("@StatusCliente", ChBoxDesbloqueado.Checked ? "Desbloqueado" : "Bloqueado");
                            cmd.Parameters.AddWithValue("@Inadimplente", CmbInadimplente.Text.Trim());
                            cmd.Parameters.AddWithValue("@Endereco", TxtEndereco.Text.Trim());
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

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Registro alterado com sucesso!", "SUCESSO!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AtualizaLista();
                            LimpaCampos();
                            TxtNome.Focus();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DeletaCliente()
        {
            try
            {
                if (DgCliente.SelectedRows.Count > 0)
                {
                    int funcID = Convert.ToInt32(DgCliente.SelectedRows[0].Cells["IDCliente"].Value);
                    string funcNome = DgCliente.SelectedRows[0].Cells["Nome"].Value.ToString();

                    if (MessageBox.Show("Deseja realmente deletar o Cliente: " + funcNome + " ?", "EXCLUSÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        using (SqlConnection connection = new SqlConnection("Data Source = ALMEIDA; Initial Catalog = PDV; Integrated Security = True"))
                        {
                            connection.Open();

                            using (SqlCommand command = new SqlCommand("DeletarCliente", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@IDCliente", funcID);
                                command.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Cliente deletado com sucesso!", "SUCESSO!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AtualizaLista();
                        LimpaCampos();
                        TxtNome.Focus();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Selecione um Cliente na lista para excluir!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void FormataCpf(object sender, EventArgs e)
        {
            try
            {
                if (TxtCpf.Text.Length == 3)
                {
                    TxtCpf.Text += ".";
                    TxtCpf.SelectionStart = TxtCpf.Text.Length;
                }

                else if (TxtCpf.Text.Length == 7)
                {
                    TxtCpf.Text += ".";
                    TxtCpf.SelectionStart = TxtCpf.Text.Length;
                }

                else if (TxtCpf.Text.Length == 11)
                {
                    TxtCpf.Text += "-";
                    TxtCpf.SelectionStart = TxtCpf.Text.Length;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void FormataTelefone(object sender, EventArgs e)
        {
            try
            {
                string digitsOnly = new string(TxtTelefone.Text.Where(char.IsDigit).ToArray());

                if (digitsOnly.Length >= 2)
                {
                    string ddd = digitsOnly.Substring(0, 2);
                    string phoneNumber = digitsOnly.Substring(2);
                    string formattedNumber = $"({ddd}) {phoneNumber}";

                    TxtTelefone.Text = formattedNumber;

                    TxtTelefone.SelectionStart = TxtTelefone.Text.Length;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private byte[] CarregaImagem()
        {
            byte[] imagem_byte = null;
            if (foto == "")
            {
                return null;
            }

            FileStream fs = new FileStream(foto, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            imagem_byte = br.ReadBytes((int)fs.Length);

            return imagem_byte;
        }

        private void CarregaImagemDoBanco(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=ALMEIDA;Initial Catalog=PDV;Integrated Security=True"))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Foto FROM Cliente WHERE IDCliente = @IDCliente", connection))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@IDCliente", id);

                        connection.Open();

                        byte[] imagemBytes = (byte[])cmd.ExecuteScalar();

                        if (imagemBytes != null)
                        {
                            using (MemoryStream ms = new MemoryStream(imagemBytes))
                            {
                                PicBoxImagemCli.Image = Image.FromStream(ms);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Imagem não encontrada.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar imagem: " + ex.Message);
            }
        }

        private void LocalizaCampos()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=ALMEIDA;Initial Catalog=PDV;Integrated Security=True"))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Cliente WHERE Nome LIKE @Nome ORDER BY Nome ASC", connection))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Nome", TxtLocalizar.Text + "%");

                        connection.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            DgCliente.DataSource = dt;
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
                int quantidadeRegistros = DgCliente.RowCount;
                LblRegistros.Text = $"{quantidadeRegistros}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void PreencheCamposComDadosClienteAtual()
        {
            try
            {
                TxtID.Text = DgCliente.CurrentRow.Cells[0].Value?.ToString();
                TxtCod.Text = DgCliente.CurrentRow.Cells[1].Value?.ToString();
                TxtNome.Text = DgCliente.CurrentRow.Cells[2].Value?.ToString();
                TxtCpf.Text = DgCliente.CurrentRow.Cells[3].Value?.ToString();
                cpfAntigo = TxtCpf.Text;
                TxtValorAberto.Text = DgCliente.CurrentRow.Cells[4].Value?.ToString();
                TxtTelefone.Text = DgCliente.CurrentRow.Cells[5].Value?.ToString();
                TxtEmail.Text = DgCliente.CurrentRow.Cells[6].Value?.ToString();

                string statusCliente = DgCliente.CurrentRow.Cells[7].Value?.ToString();
                ChBoxDesbloqueado.Checked = (statusCliente == "Desbloqueado");
                ChBoxBloqueado.Checked = (statusCliente == "Bloqueado");

                CmbInadimplente.Text = DgCliente.CurrentRow.Cells[8].Value?.ToString();
                TxtEndereco.Text = DgCliente.CurrentRow.Cells[9].Value?.ToString();

                if (DgCliente.CurrentRow.Index >= 0)
                {
                    int id = Convert.ToInt32(DgCliente.Rows[DgCliente.CurrentRow.Index].Cells["IDCliente"].Value);
                    CarregaImagemDoBanco(id);
                    PicBoxImagemCli.SizeMode = PictureBoxSizeMode.CenterImage;
                    PicBoxImagemCli.SizeMode = PictureBoxSizeMode.Zoom;
                }

                DtData.Text = DgCliente.CurrentRow.Cells[11].Value?.ToString();

                LblCarregaImagem.Visible = false;
                BtnIncluir.Enabled = false;
                PicBoxImagemCli.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Um erro ocorreu: {ex.Message}", "ERRO!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public DataTable ImprimeRelatorio()
        {
            var dt = new DataTable();
            dt.Columns.Add("Codigo");
            dt.Columns.Add("Nome");
            dt.Columns.Add("Cpf");
            dt.Columns.Add("ValorAberto");
            dt.Columns.Add("Telefone");
            dt.Columns.Add("Email");
            dt.Columns.Add("StatusCliente");
            dt.Columns.Add("Inadimplente");
            dt.Columns.Add("Endereco");
            dt.Columns.Add("DataCadastro");

            try
            {
                foreach (DataGridViewRow item in DgCliente.Rows)
                {
                    string dataCadastro = Convert.ToDateTime(item.Cells["DataCadastro"].Value).ToString("dd/MM/yyyy");

                    dt.Rows.Add(
                        item.Cells["Codigo"].Value.ToString(),
                        item.Cells["Nome"].Value.ToString(),
                        item.Cells["Cpf"].Value.ToString(),
                 "R$" + item.Cells["ValorAberto"].Value.ToString(),
                        item.Cells["Telefone"].Value.ToString(),
                        item.Cells["Email"].Value.ToString(),
                        item.Cells["StatusCliente"].Value.ToString(),
                        item.Cells["Inadimplente"].Value.ToString(),
                        item.Cells["Endereco"].Value.ToString(),
                        dataCadastro
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao preencher DataTable: {ex.Message}");
            }
            return dt;
        }

        public void GeraDadosClienteEXCEL()
        {
            try
            {
                if (MessageBox.Show("Deseja importar dados Cliente ?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Excel.Application excelApp = new Excel.Application();
                    excelApp.Visible = true;
                    Excel.Workbook workbook = excelApp.Workbooks.Add();
                    Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                    int columnIndex = 1;
                    for (int i = 0; i < DgCliente.Columns.Count; i++)
                    {
                        if (DgCliente.Columns[i].HeaderText != "IDCliente" && DgCliente.Columns[i].HeaderText != "Foto")
                        {
                            worksheet.Cells[1, columnIndex] = DgCliente.Columns[i].HeaderText;
                            columnIndex++;
                        }
                    }

                    for (int i = 0; i < DgCliente.Rows.Count; i++)
                    {
                        columnIndex = 1;
                        for (int j = 0; j < DgCliente.Columns.Count; j++)
                        {
                            if (DgCliente.Columns[j].HeaderText != "IDCliente" && DgCliente.Columns[j].HeaderText != "Foto")
                            {
                                if (DgCliente.Columns[j].HeaderText == "Cadastro" && DgCliente.Rows[i].Cells[j].Value is DateTime dataCadastro)
                                {
                                    worksheet.Cells[i + 2, columnIndex] = dataCadastro.ToShortDateString();
                                }
                                else
                                {
                                    worksheet.Cells[i + 2, columnIndex] = DgCliente.Rows[i].Cells[j].Value.ToString();
                                }

                                columnIndex++;
                            }
                        }
                    }

                    worksheet.Columns.AutoFit();
                    worksheet.Columns["IDCliente"].EntireColumn.Hidden = true;
                    worksheet.Columns["Foto"].EntireColumn.Hidden = true;

                    string downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
                    workbook.SaveAs(downloadPath + @"\Clientes.xlsx");
                    excelApp.Quit();
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

        private void LimpaCampos()
        {
            try
            {
                TxtNome.Text = string.Empty;
                TxtCod.Text = string.Empty;
                TxtCpf.Text = string.Empty;
                TxtTelefone.Text = string.Empty;
                TxtEmail.Text = string.Empty;
                TxtEndereco.Text = string.Empty;
                TxtValorAberto.Text = string.Empty;
                CmbInadimplente.Text = string.Empty;
                TxtLocalizar.Text = string.Empty;

                ChBoxDesbloqueado.Checked = false;
                ChBoxBloqueado.Checked = false;
                PicBoxImagemCli.Enabled = true;
                LblCarregaImagem.Visible = true;
                BtnIncluir.Enabled = true;

                PicBoxImagemCli.Image = Properties.Resources.imagem;
                PicBoxImagemCli.SizeMode = PictureBoxSizeMode.CenterImage;
                PicBoxImagemCli.Image = null;
                foto = "img/imagem.png";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        public FrmCliente()
        {
            InitializeComponent();
        }

        private void FrmCliente_Load(object sender, EventArgs e)
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

        private void FrmCliente_KeyDown(object sender, KeyEventArgs e)
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
                
                if (DgCliente.SelectedRows.Count > 0)
                {
                    if (DgCliente.CurrentRow != null)
                    {
                        // seta para cima
                        if (e.KeyCode == Keys.Up)
                        {
                            int currentIndex = DgCliente.CurrentRow.Index;

                            if (currentIndex > 0)
                            {
                                int previousRowIndex = currentIndex - 1;

                                DgCliente.CurrentCell = DgCliente.Rows[previousRowIndex].Cells[DgCliente.CurrentCell.ColumnIndex];

                                PreencheCamposComDadosClienteAtual();

                                e.Handled = true;
                            }
                        }
                        // seta para baixo
                        else if (e.KeyCode == Keys.Down)
                        {
                            int currentIndex = DgCliente.CurrentRow.Index;

                            if (currentIndex < DgCliente.Rows.Count - 1)
                            {
                                int nextRowIndex = currentIndex + 1;

                                DgCliente.CurrentCell = DgCliente.Rows[nextRowIndex].Cells[DgCliente.CurrentCell.ColumnIndex];

                                PreencheCamposComDadosClienteAtual();

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

        private void TxtNome_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //somente letras e space
                if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void TxtCod_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //apenas numeros
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void TxtCpf_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TxtCpf.TextChanged += new EventHandler(FormataCpf);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void TxtCpf_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //apenas numeros
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void TxtTelefone_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TxtTelefone.TextChanged += new EventHandler(FormataTelefone);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void TxtTelefone_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //apenas numeros
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void TxtValorAberto_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //apenas numeros
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ChBoxDesbloqueado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChBoxDesbloqueado.Checked == true)
                {
                    ChBoxBloqueado.Enabled = false;
                }
                else
                {
                    ChBoxBloqueado.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ChBoxBloqueado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChBoxBloqueado.Checked == true)
                {
                    ChBoxDesbloqueado.Enabled = false;
                }
                else
                {
                    ChBoxDesbloqueado.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void PicBoxImagemCli_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();

                dialog.Filter = "Imagens(*.jpg; *.png) | *.jpg; *.png";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foto = dialog.FileName.ToString();
                    PicBoxImagemCli.ImageLocation = foto;
                    PicBoxImagemCli.SizeMode = PictureBoxSizeMode.CenterImage;
                    PicBoxImagemCli.SizeMode = PictureBoxSizeMode.Zoom;
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

        private void DgCliente_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                PreencheCamposComDadosClienteAtual();
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
                IncluiCliente();
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
                AlteraCliente();
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
                DeletaCliente();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                var dt = ImprimeRelatorio();

                using (Relatorios.FrmClienteRelatorio frm = new Relatorios.FrmClienteRelatorio(dt))
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                GeraDadosClienteEXCEL();
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
                TxtNome.Focus();
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
