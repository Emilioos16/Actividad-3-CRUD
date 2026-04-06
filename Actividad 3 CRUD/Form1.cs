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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Actividad_3_CRUD
{
    public partial class Form1 : Form
    {
        string categoriaActual = "";
        SqlConnection conexion = new SqlConnection(@"Data Source=DESKTOP-V3L425G\SQLEXPRESS;Initial Catalog=A3SCRUD;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
        }

        private void misiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelBienvenida.Visible = true; 
            panelProductos.Visible = false;
            txtMensajes.Text = "MISION:\r\nNuestra mision es darnos a conocer a nivel nacional y lograr el objetivo de que todos los mexicanos utilicen productos mexicanos, que abramos los ojos y veamos que no por que el producto sea de otro pais, signifique que es mejor.";
        }

        private void bienvenidoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mensajeDeBienvenidaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelBienvenida.Visible = true; 
            panelProductos.Visible = false;
            txtMensajes.Text = "Bienvenido a la tienda en línea  UMI, donde podrá encontrar todo lo que usted necesita y si no lo encuentra se lo conseguimos. Somos su mejor opcion hoy y siempre.";
        }

        private void quiénesSomosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelBienvenida.Visible = true; 
            panelProductos.Visible = false;
            txtMensajes.Text = "¿Quienes somos?\r\nSomos una empresa mexicana, que trata de apoyar al comercio mexicano, todos nuestros productos son 100% mexicanos, por que creemos en nosotros y por que sabemos que podemos.";
        }

        private void visiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelBienvenida.Visible = true; 
            panelProductos.Visible = false;
            txtMensajes.Text = "VISION:\r\nNuestra vision es que una vez que nos encontremos a nivel nacional, buscaremos el mercado internacional y llevaremos el nombre de nuestro producto, de nuestro pais, a todo el mundo para que sepan de lo que somos capaces.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            string tallasElegidas = "";
            foreach (Control c in groupTallas.Controls)
            {
                if (c is System.Windows.Forms.CheckBox && ((System.Windows.Forms.CheckBox)c).Checked)
                {
                    tallasElegidas += c.Text + ", ";
                }
            }

            try
            {
                conexion.Open();

                
                string query = "INSERT INTO PRODUCTOS (nombre, precio, id_proveedor, tallas, categoria) " +
                               "VALUES (@nom, @pre, @idp, @tal, @cat)";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@nom", txtNombreProd.Text);
                cmd.Parameters.AddWithValue("@pre", txtPrecioProd.Text);
                cmd.Parameters.AddWithValue("@idp", cmbProveedor.SelectedValue); 
                cmd.Parameters.AddWithValue("@tal", tallasElegidas);
                cmd.Parameters.AddWithValue("@cat", categoriaActual); 

                cmd.ExecuteNonQuery();
                MessageBox.Show("¡Producto guardado con éxito!");

                
                txtNombreProd.Clear();
                txtPrecioProd.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open) conexion.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void damasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            categoriaActual = "Damas"; 
            panelProductos.Visible = true; 
            panelBienvenida.Visible = false; 
            panelProductos.BringToFront();
            LlenarProveedores(); 
        }
        public void LlenarProveedores()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed) conexion.Open();
                string query = "SELECT id_proveedor, nombre FROM PROVEEDORES";
                SqlCommand cmd = new SqlCommand(query, conexion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbProveedor.DataSource = dt;
                cmbProveedor.DisplayMember = "nombre";
                cmbProveedor.ValueMember = "id_proveedor";
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar proveedores: " + ex.Message);
                if (conexion.State == ConnectionState.Open) conexion.Close();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Seguro que deseas eliminar este producto?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    if (conexion.State == ConnectionState.Closed) conexion.Open();

                    // Borramos asegurándonos de estar en la categoría correcta
                    string query = "DELETE FROM PRODUCTOS WHERE nombre = @nom AND categoria = @cat";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@nom", txtNombreProd.Text);
                    cmd.Parameters.AddWithValue("@cat", categoriaActual);

                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        MessageBox.Show("Producto eliminado.");
                        txtNombreProd.Clear();
                        txtPrecioProd.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conexion.Close();
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            string tallasElegidas = "";
            foreach (Control c in groupTallas.Controls)
            {
                if (c is System.Windows.Forms.CheckBox && ((System.Windows.Forms.CheckBox)c).Checked)
                {
                    tallasElegidas += c.Text + ", ";
                }
            }

            try
            {
                conexion.Open();

                
                string query = "UPDATE PRODUCTOS SET precio = @pre, id_proveedor = @idp, tallas = @tal, categoria = @cat " +
                               "WHERE nombre = @nom";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@nom", txtNombreProd.Text); 
                cmd.Parameters.AddWithValue("@pre", txtPrecioProd.Text); 
                cmd.Parameters.AddWithValue("@idp", cmbProveedor.SelectedValue);
                cmd.Parameters.AddWithValue("@tal", tallasElegidas);
                cmd.Parameters.AddWithValue("@cat", categoriaActual);

                int filasAfectadas = cmd.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("¡Producto actualizado correctamente!");
                }
                else
                {
                    MessageBox.Show("No se encontró ningún producto con ese nombre para modificar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open) conexion.Close();
            }
        }

        private void caballerosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            categoriaActual = "Caballeros"; 
            panelProductos.Visible = true;   
            panelBienvenida.Visible = false; 

            panelProductos.BringToFront();   
            LlenarProveedores();
        }

        private void clienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormClientes ventana = new FormClientes(); 
            ventana.ShowDialog();
        }

        private void proveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProveedores ventana = new FormProveedores(); 
            ventana.ShowDialog();
        }

        private void productoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            login.ShowDialog();
        }

        private void comprasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCompras ventana = new FormCompras();
            ventana.ShowDialog();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
