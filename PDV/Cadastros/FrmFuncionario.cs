using PDV.Relatorios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PDV.Cadastros.FrmFuncionario;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Excel = Microsoft.Office.Interop.Excel;

namespace PDV.Cadastros
{
    #region Formulario
    public partial class FrmFuncionario : Form
    {
        private SqlConnection conexao = new SqlConnection("Data Source=ALMEIDA;Initial Catalog=PDV;Integrated Security=True");

        string cpfAntigo;
        string foto;

        #region Funcao

        public DataGridView GetDataGridView()
        {
            return DgFuncionario;
        }

        private void ConfiguraLista()
        {
            try
            {
                DgFuncionario.Columns["ID"].Visible = false;
                DgFuncionario.Columns["Nome"].DisplayIndex = 1;
                DgFuncionario.Columns["Cpf"].DisplayIndex = 2;
                DgFuncionario.Columns["Telefone"].DisplayIndex = 3;
                DgFuncionario.Columns["Email"].DisplayIndex = 4;
                DgFuncionario.Columns["Endereco"].DisplayIndex = 5;
                DgFuncionario.Columns["Cargo"].DisplayIndex = 6;
                DgFuncionario.Columns["DataCadastro"].DisplayIndex = 7;
                DgFuncionario.Columns["Foto"].Visible = false;
                DgFuncionario.Columns["Observacao"].DisplayIndex = 9;

                DgFuncionario.Columns["Nome"].HeaderText = "Funcionário";
                DgFuncionario.Columns["Cpf"].HeaderText = "CPF";
                DgFuncionario.Columns["Telefone"].HeaderText = "Telefone";
                DgFuncionario.Columns["Email"].HeaderText = "Email";
                DgFuncionario.Columns["Endereco"].HeaderText = "Endereço";
                DgFuncionario.Columns["Cargo"].HeaderText = "Cargo";
                DgFuncionario.Columns["DataCadastro"].HeaderText = "Cadastro";
                DgFuncionario.Columns["Observacao"].HeaderText = "Observação";

                DgFuncionario.Columns["Nome"].Width = 140;
                DgFuncionario.Columns["Cpf"].Width = 65;
                DgFuncionario.Columns["Telefone"].Width = 65;
                DgFuncionario.Columns["Email"].Width = 150;
                DgFuncionario.Columns["Endereco"].Width = 250;
                DgFuncionario.Columns["Cargo"].Width = 160;
                DgFuncionario.Columns["DataCadastro"].Width = 50;
                DgFuncionario.Columns["Observacao"].Width = 210;

                DgFuncionario.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                DgFuncionario.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                DgFuncionario.BackgroundColor = Color.FromArgb(0, 64, 64);
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Funcionario", conexao);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                DgFuncionario.DataSource = dt;
                ConfiguraLista();
                ContagemRegistros();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void IncluiFuncionario()
        {
            try
            {
                string cpf = TxtCpf.Text;
                string email = TxtEmail.Text;

                if (TxtNome.Text == "")
                {
                    MessageBox.Show("Digite o Nome do Funcionário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtNome.Focus();
                    return;
                }
                else if (TxtCpf.Text == "")
                {
                    MessageBox.Show("Digite o CPF do Funcionário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtCpf.Focus();
                    return;
                }
                else if (cpf.Length < 14)
                {
                    MessageBox.Show("CPF deve conter 11 dígitos!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtCpf.Focus();
                    return;
                }
                else if (TxtTelefone.Text == "")
                {
                    MessageBox.Show("Digite o Telefone do Funcionário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtTelefone.Focus();
                    return;
                }
                else if (TxtEmail.Text == "")
                {
                    MessageBox.Show("Digite o Email do Funcionário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("Digite o Endereço do Funcionário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtEndereco.Focus();
                    return;
                }
                else if (CmbCargo.Text == "")
                {
                    MessageBox.Show("Selecione o Cargo do Funcionário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CmbCargo.Focus();
                    return;
                }
                else if (PicBoxImagemFunc.Image == null)
                {
                    MessageBox.Show("Selecione uma imagem para o Funcionário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    PicBoxImagemFunc.Focus();
                    return;
                }
                else
                {
                    string novoCpf = TxtCpf.Text.Trim();

                    using (SqlConnection connection = new SqlConnection("Data Source = ALMEIDA; Initial Catalog = PDV; Integrated Security = True"))
                    {
                        using (SqlCommand cmd = new SqlCommand("InserirFuncionario", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Nome", TxtNome.Text.Trim());
                            cmd.Parameters.AddWithValue("@Cpf", TxtCpf.Text.Trim());
                            cmd.Parameters.AddWithValue("@Telefone", TxtTelefone.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", TxtEmail.Text.Trim());
                            cmd.Parameters.AddWithValue("@Endereco", TxtEndereco.Text.Trim());
                            cmd.Parameters.AddWithValue("@Cargo", CmbCargo.Text.Trim());
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
                            cmd.Parameters.AddWithValue("@Observacao", TxtObservacao.Text.Trim());
                            cmd.Parameters.AddWithValue("@Foto", CarregaImagem());

                            //Verifica se ha CPF duplicado.
                            if (novoCpf == cpfAntigo)
                            {
                                using (SqlCommand cmdVerifica = new SqlCommand("SELECT * FROM Funcionario WHERE Cpf = @Cpf", connection))
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

        private void AlteraFuncionario()
        {
            try
            {
                string cpf = TxtCpf.Text;
                string telefone = TxtTelefone.Text;
                string email = TxtEmail.Text;

                if (TxtNome.Text == "")
                {
                    MessageBox.Show("Digite o Nome do Funcionário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtNome.Focus();
                    return;
                }
                else if (TxtCpf.Text == "")
                {
                    MessageBox.Show("Digite o CPF do Funcionário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtCpf.Focus();
                    return;
                }
                else if (cpf.Length < 14)
                {
                    MessageBox.Show("CPF deve conter 11 dígitos!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtCpf.Focus();
                    return;
                }
                else if (TxtTelefone.Text == "")
                {
                    MessageBox.Show("Digite o Telefone do Funcionário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtTelefone.Focus();
                    return;
                }
                else if (TxtEmail.Text == "")
                {
                    MessageBox.Show("Digite o Email do Funcionário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("Digite o Endereço do Funcionário!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtEndereco.Focus();
                    return;
                }
                else
                {
                    string novoCpf = TxtCpf.Text.Trim();
                    Image novaImg = PicBoxImagemFunc.Image;

                    using (SqlConnection connection = new SqlConnection("Data Source = ALMEIDA; Initial Catalog = PDV; Integrated Security = True"))
                    {
                        connection.Open();

                        using (SqlCommand cmd = new SqlCommand("AlterarFuncionario", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@ID", TxtID.Text.Trim());
                            cmd.Parameters.AddWithValue("@Nome", TxtNome.Text.Trim());
                            cmd.Parameters.AddWithValue("@Cpf", TxtCpf.Text.Trim());
                            cmd.Parameters.AddWithValue("@Telefone", TxtTelefone.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", TxtEmail.Text.Trim());
                            cmd.Parameters.AddWithValue("@Endereco", TxtEndereco.Text.Trim());
                            cmd.Parameters.AddWithValue("@Cargo", CmbCargo.Text.Trim());
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
                            cmd.Parameters.AddWithValue("@Observacao", TxtObservacao.Text.Trim());

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

        private void DeletaFuncionario()
        {
            try
            {
                if (DgFuncionario.SelectedRows.Count > 0)
                {
                    int funcID = Convert.ToInt32(DgFuncionario.SelectedRows[0].Cells["ID"].Value);
                    string funcNome = DgFuncionario.SelectedRows[0].Cells["Nome"].Value.ToString();

                    if (MessageBox.Show("Deseja realmente deletar o Funcionário: " + funcNome + " ?", "EXCLUSÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        using (SqlConnection connection = new SqlConnection("Data Source = ALMEIDA; Initial Catalog = PDV; Integrated Security = True"))
                        {
                            connection.Open();

                            using (SqlCommand command = new SqlCommand("DeletarFuncionario", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@ID", funcID);
                                command.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Funcionário deletado com sucesso!", "SUCESSO!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("Selecione um Funcionário na lista para excluir!", "ATENÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private DataTable ImprimeRelatorio()
        {
            var dt = new DataTable();
            dt.Columns.Add("Nome");
            dt.Columns.Add("Cpf");
            dt.Columns.Add("Telefone");
            dt.Columns.Add("Email");
            dt.Columns.Add("Endereco");
            dt.Columns.Add("Cargo");
            dt.Columns.Add("DataCadastro");
            dt.Columns.Add("Observacao");

            try
            {
                foreach (DataGridViewRow item in DgFuncionario.Rows)
                {
                    string dataCadastro = Convert.ToDateTime(item.Cells["DataCadastro"].Value).ToString("dd/MM/yyyy");

                    dt.Rows.Add(
                        item.Cells["Nome"].Value.ToString(),
                        item.Cells["Cpf"].Value.ToString(),
                        item.Cells["Telefone"].Value.ToString(),
                        item.Cells["Email"].Value.ToString(),
                        item.Cells["Endereco"].Value.ToString(),
                        item.Cells["Cargo"].Value.ToString(),
                        dataCadastro,
                        item.Cells["Observacao"].Value.ToString()
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao preencher DataTable: {ex.Message}");
            }
            return dt;
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
                    using (SqlCommand cmd = new SqlCommand("SELECT Foto FROM Funcionario WHERE ID = @ID", connection))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@ID", id);

                        connection.Open();

                        byte[] imagemBytes = (byte[])cmd.ExecuteScalar();

                        if (imagemBytes != null)
                        {
                            using (MemoryStream ms = new MemoryStream(imagemBytes))
                            {
                                PicBoxImagemFunc.Image = Image.FromStream(ms);
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
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Funcionario WHERE Nome LIKE @Nome ORDER BY Nome ASC", connection))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Nome", TxtLocalizar.Text + "%");

                        connection.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            DgFuncionario.DataSource = dt;
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
                int quantidadeRegistros = DgFuncionario.RowCount;
                LblRegistros.Text = $"{quantidadeRegistros}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void PreencheCamposComDadosFuncionarioAtual()
        {
            try
            {
                CarregaComboCargo();
                TxtID.Text = DgFuncionario.CurrentRow.Cells[0].Value?.ToString();
                TxtNome.Text = DgFuncionario.CurrentRow.Cells[1].Value?.ToString();
                TxtCpf.Text = DgFuncionario.CurrentRow.Cells[2].Value?.ToString();
                cpfAntigo = DgFuncionario.CurrentRow.Cells[2].Value?.ToString();
                TxtTelefone.Text = DgFuncionario.CurrentRow.Cells[3].Value?.ToString();
                TxtEmail.Text = DgFuncionario.CurrentRow.Cells[4].Value?.ToString();
                TxtEndereco.Text = DgFuncionario.CurrentRow.Cells[5].Value?.ToString();
                CmbCargo.Text = DgFuncionario.CurrentRow.Cells[6].Value?.ToString();
                DtData.Text = DgFuncionario.CurrentRow.Cells[7].Value?.ToString();

                if (DgFuncionario.CurrentRow.Index >= 0)
                {
                    int id = Convert.ToInt32(DgFuncionario.Rows[DgFuncionario.CurrentRow.Index].Cells["ID"].Value);
                    CarregaImagemDoBanco(id);
                    PicBoxImagemFunc.SizeMode = PictureBoxSizeMode.CenterImage;
                    PicBoxImagemFunc.SizeMode = PictureBoxSizeMode.Zoom;
                }

                TxtObservacao.Text = DgFuncionario.CurrentRow.Cells[9].Value?.ToString();

                LblCarregaImagem.Visible = false;
                BtnIncluir.Enabled = false;
                PicBoxImagemFunc.Enabled = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GeraDadosFuncionarioEXCEL()
        {
            try
            {
                if (MessageBox.Show("Deseja importar dados Funcionário ?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Excel.Application excelApp = new Excel.Application();
                    excelApp.Visible = true;
                    Excel.Workbook workbook = excelApp.Workbooks.Add();
                    Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                    int columnIndex = 1;
                    for (int i = 0; i < DgFuncionario.Columns.Count; i++)
                    {
                        if (DgFuncionario.Columns[i].HeaderText != "ID" && DgFuncionario.Columns[i].HeaderText != "Foto")
                        {
                            worksheet.Cells[1, columnIndex] = DgFuncionario.Columns[i].HeaderText;
                            columnIndex++;
                        }
                    }

                    for (int i = 0; i < DgFuncionario.Rows.Count; i++)
                    {
                        columnIndex = 1;
                        for (int j = 0; j < DgFuncionario.Columns.Count; j++)
                        {
                            if (DgFuncionario.Columns[j].HeaderText != "ID" && DgFuncionario.Columns[j].HeaderText != "Foto")
                            {
                                if (DgFuncionario.Columns[j].HeaderText == "Cadastro" && DgFuncionario.Rows[i].Cells[j].Value is DateTime dataCadastro)
                                {
                                    worksheet.Cells[i + 2, columnIndex] = dataCadastro.ToShortDateString();
                                }
                                else
                                {
                                    worksheet.Cells[i + 2, columnIndex] = DgFuncionario.Rows[i].Cells[j].Value.ToString();
                                }

                                columnIndex++;
                            }
                        }
                    }

                    worksheet.Columns.AutoFit();
                    worksheet.Columns["ID"].EntireColumn.Hidden = true;
                    worksheet.Columns["Foto"].EntireColumn.Hidden = true;

                    string downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
                    workbook.SaveAs(downloadPath + @"\Funcionarios.xlsx");
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
                TxtCpf.Text = string.Empty;
                TxtTelefone.Text = string.Empty;
                TxtEmail.Text = string.Empty;
                TxtEndereco.Text = string.Empty;
                CmbCargo.Text = string.Empty;
                TxtObservacao.Text = string.Empty;
                TxtLocalizar.Text = string.Empty;

                PicBoxImagemFunc.Enabled = true;
                LblCarregaImagem.Visible = true;
                BtnIncluir.Enabled = true;

                PicBoxImagemFunc.SizeMode = PictureBoxSizeMode.CenterImage;
                PicBoxImagemFunc.Image = null;
                foto = "img/imagem.png";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        public FrmFuncionario()
        {
            try
            {
                InitializeComponent();
                TxtNome.KeyPress += TxtNome_KeyPress;
                TxtCpf.KeyPress += TxtCpf_KeyPress;
                TxtTelefone.KeyPress += TxtTelefone_KeyPress;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void FrmFuncionario_Load(object sender, EventArgs e)
        {
            try
            {
                AtualizaLista();
                DtData.Value = DateTime.Now;
                TxtLocalizar.TextChanged += TxtLocalizar_TextChanged;
                CmbCargo.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void FrmFuncionario_KeyDown(object sender, KeyEventArgs e)
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

                if (DgFuncionario.SelectedRows.Count > 0)
                {
                    if (DgFuncionario.CurrentRow != null)
                    {
                        // seta para cima
                        if (e.KeyCode == Keys.Up)
                        {
                            int currentIndex = DgFuncionario.CurrentRow.Index;

                            if (currentIndex > 0)
                            {
                                int previousRowIndex = currentIndex - 1;

                                DgFuncionario.CurrentCell = DgFuncionario.Rows[previousRowIndex].Cells[DgFuncionario.CurrentCell.ColumnIndex];

                                PreencheCamposComDadosFuncionarioAtual();

                                e.Handled = true;
                            }
                        }
                        // seta para baixo
                        else if (e.KeyCode == Keys.Down)
                        {
                            int currentIndex = DgFuncionario.CurrentRow.Index;

                            if (currentIndex < DgFuncionario.Rows.Count - 1)
                            {
                                int nextRowIndex = currentIndex + 1;

                                DgFuncionario.CurrentCell = DgFuncionario.Rows[nextRowIndex].Cells[DgFuncionario.CurrentCell.ColumnIndex];

                                PreencheCamposComDadosFuncionarioAtual();

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

        private void CmbCargo_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                CarregaComboCargo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void PicBoxImagemFunc_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();

                dialog.Filter = "Imagens(*.jpg; *.png) | *.jpg; *.png";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foto = dialog.FileName.ToString();
                    PicBoxImagemFunc.ImageLocation = foto;
                    PicBoxImagemFunc.SizeMode = PictureBoxSizeMode.CenterImage;
                    PicBoxImagemFunc.SizeMode = PictureBoxSizeMode.Zoom;
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

        private void DgFuncionario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                PreencheCamposComDadosFuncionarioAtual();
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
                IncluiFuncionario();
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
                AlteraFuncionario();
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
                DeletaFuncionario();
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

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                var dt = ImprimeRelatorio();

                using (Relatorios.FrmFuncionarioRelatorio frm = new Relatorios.FrmFuncionarioRelatorio(dt))
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
                GeraDadosFuncionarioEXCEL();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnVoltar_Click_1(object sender, EventArgs e)
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
    }
    #endregion
}
