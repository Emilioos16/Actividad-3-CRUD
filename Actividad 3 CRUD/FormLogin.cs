using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Actividad_3_CRUD
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == "1234")
            {
                MessageBox.Show("Acceso concedido.");
                this.Hide();

                FormProductos ventana = new FormProductos();
                ventana.ShowDialog();

                this.Close();
            }
            else
            {
                MessageBox.Show("Contraseña incorrecta. Intente de nuevo.");
                txtPassword.Clear();
            }
        }
    }
}
