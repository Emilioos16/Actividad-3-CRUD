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

namespace Actividad_3_CRUD
{
    public partial class FormProductos : Form
    {
        SqlConnection conexion = new SqlConnection(@"Data Source=DESKTOP-V3L425G\SQLEXPRESS;Initial Catalog=A3SCRUD;Integrated Security=True");
        public FormProductos()
        {
            InitializeComponent();
        }

        private void FormProductos_Load(object sender, EventArgs e)
        {

        }


        public void LlenarTablaProductos()
        {
            try
            {
                // Limpiamos el DataGridView antes de cargar
                dataGridView1.DataSource = null;

                // La consulta debe tener los campos exactos de tu tabla PRODUCTOS
                string consulta = "SELECT id_producto, nombre, precio, id_proveedor, tallas, categoria FROM PRODUCTOS";

                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);

                // Asignamos el resultado al diseño gris de tu formulario
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar productos: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (conexion.State == ConnectionState.Closed) conexion.Open();

                string query = "INSERT INTO PRODUCTOS (nombre, precio, id_proveedor, tallas, categoria) " +
                               "VALUES (@nom, @pre, @idProv, @tal, @cat)";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@nom", txtNombre.Text);
                cmd.Parameters.AddWithValue("@pre", txtPrecio.Text);
                cmd.Parameters.AddWithValue("@idProv", txtIdProveedor.Text);
                cmd.Parameters.AddWithValue("@tal", txtTallas.Text);
                cmd.Parameters.AddWithValue("@cat", txtCategoria.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Producto guardado con éxito.");
                LlenarTablaProductos();
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (conexion.State == ConnectionState.Closed) conexion.Open();

                string query = "UPDATE PRODUCTOS SET nombre=@nom, precio=@pre, id_proveedor=@idProv, tallas=@tal, categoria=@cat WHERE id_producto=@id";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@id", txtIdProducto.Text);
                cmd.Parameters.AddWithValue("@nom", txtNombre.Text);
                cmd.Parameters.AddWithValue("@pre", txtPrecio.Text);
                cmd.Parameters.AddWithValue("@idProv", txtIdProveedor.Text);
                cmd.Parameters.AddWithValue("@tal", txtTallas.Text);
                cmd.Parameters.AddWithValue("@cat", txtCategoria.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Producto modificado correctamente.");
                LlenarTablaProductos(); // Refresca el DataGridView
                LimpiarCampos();
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


        private void LimpiarCampos()
        {
            txtIdProveedor.Clear();
            txtNombre.Clear();
            txtPrecio.Clear();
            txtIdProveedor.Clear();
            txtTallas.Clear();
            txtCategoria.Clear();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIdProducto.Text == "")
                {
                    MessageBox.Show("Seleccione un producto de la tabla primero.");
                    return;
                }

                if (conexion.State == ConnectionState.Closed) conexion.Open();

                string query = "DELETE FROM PRODUCTOS WHERE id_producto=@id";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@id", txtIdProducto.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Producto eliminado.");
                LlenarTablaProductos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];

             
                txtIdProducto.Text = fila.Cells[0].Value.ToString();
                txtNombre.Text = fila.Cells[1].Value.ToString();
                txtPrecio.Text = fila.Cells[2].Value.ToString();
                txtIdProveedor.Text = fila.Cells[3].Value.ToString();
                txtTallas.Text = fila.Cells[4].Value.ToString();
                txtCategoria.Text = fila.Cells[5].Value.ToString();
            }
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            LlenarTablaProductos();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    
}
