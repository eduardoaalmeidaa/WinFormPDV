using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDV
{
    public partial class FrmCalculadora : Form
    {
        decimal valor1 = 0, valor2 = 0;
        string operacao = "";

        #region Formulario

        #region Funcao
        private void CalculaResultado()
        {
            try
            {
                valor2 = decimal.Parse(TxtResultado.Text, CultureInfo.InvariantCulture);

                switch (operacao)
                {
                    case "SOMA":
                        TxtResultado.Text = Convert.ToString(valor1 + valor2);
                        break;
                    case "SUB":
                        TxtResultado.Text = Convert.ToString(valor1 - valor2);
                        break;
                    case "MULT":
                        TxtResultado.Text = Convert.ToString(valor1 * valor2);
                        break;
                    case "DIV":
                        TxtResultado.Text = Convert.ToString(valor1 / valor2);
                        break;
                    case "PORC":
                        TxtResultado.Text = Convert.ToString(valor1 % valor2);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        public FrmCalculadora()
        {
            InitializeComponent();
        }

        private void FrmCalculadora_Load(object sender, EventArgs e)
        {
            try
            {
                BtnIgual.TabIndex = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void FrmCalculadora_KeyDown(object sender, KeyEventArgs e)
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

        private void LblFechar_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnPorcentagem_Click(object sender, EventArgs e)
        {
            try
            {
                valor1 = decimal.Parse(TxtResultado.Text, CultureInfo.InvariantCulture);
                TxtResultado.Text = "";
                operacao = "PORC";
                LblOperacao.Text = "%";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnCE_Click(object sender, EventArgs e)
        {
            try
            {
                TxtResultado.Text = "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnC_Click(object sender, EventArgs e)
        {
            try
            {
                TxtResultado.Text = "";
                valor1 = 0;
                valor2 = 0;
                LblPonteiro.Text = "";
                LblOperacao.Text = "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Btn7_Click(object sender, EventArgs e)
        {
            try
            {
                TxtResultado.Text += "7";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Btn8_Click(object sender, EventArgs e)
        {
            try
            {
                TxtResultado.Text += "8";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Btn9_Click(object sender, EventArgs e)
        {
            try
            {
                TxtResultado.Text += "9";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnMultiplicacao_Click(object sender, EventArgs e)
        {
            try
            {
                valor1 = decimal.Parse(TxtResultado.Text, CultureInfo.InvariantCulture);
                TxtResultado.Text = "";
                operacao = "MULT";
                LblOperacao.Text = "x";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Btn4_Click(object sender, EventArgs e)
        {
            try
            {
                TxtResultado.Text += "4";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Btn5_Click(object sender, EventArgs e)
        {
            try
            {
                TxtResultado.Text += "5";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Btn6_Click(object sender, EventArgs e)
        {
            try
            {
                TxtResultado.Text += "6";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnSubtracao_Click(object sender, EventArgs e)
        {
            try
            {
                valor1 = decimal.Parse(TxtResultado.Text, CultureInfo.InvariantCulture);
                TxtResultado.Text = "";
                operacao = "SUB";
                LblOperacao.Text = "-";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            try
            {
                TxtResultado.Text += "1";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Btn2_Click(object sender, EventArgs e)
        {
            try
            {
                TxtResultado.Text += "2";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Btn3_Click(object sender, EventArgs e)
        {
            try
            {
                TxtResultado.Text += "3";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnAdicao_Click(object sender, EventArgs e)
        {
            try
            {
                {
                    valor1 = decimal.Parse(TxtResultado.Text, CultureInfo.InvariantCulture);
                    TxtResultado.Text = "";
                    operacao = "SOMA";
                    LblOperacao.Text = "+";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnDivisao_Click(object sender, EventArgs e)
        {
            try
            {
                valor1 = decimal.Parse(TxtResultado.Text, CultureInfo.InvariantCulture);
                TxtResultado.Text = "";
                operacao = "DIV";
                LblOperacao.Text = "/";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Btn0_Click(object sender, EventArgs e)
        {
            try
            {
                TxtResultado.Text += "0";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnVirgula_Click(object sender, EventArgs e)
        {
            try
            {
                TxtResultado.Text += ",";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnIgual_Click(object sender, EventArgs e)
        {
            try
            {
                CalculaResultado();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    #endregion
}
