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

    public partial class FormCompras : Form
    {
        SqlConnection conexion = new SqlConnection(@"Data Source=DESKTOP-V3L425G\SQLEXPRESS;Initial Catalog=A3SCRUD;Integrated Security=True");
        public FormCompras()
        {
            InitializeComponent();
        }

        private void FormCompras_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (conexion.State == ConnectionState.Closed) conexion.Open();

                
                string query = "INSERT INTO COMPRAS (id_cliente, id_producto, fechaCompra, cantidad, precio) " +
               "VALUES (@idCli, @idProd, @fecha, @cant, @pre)";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@idCli", txtIdCliente.Text);
                cmd.Parameters.AddWithValue("@idProd", txtIdProducto.Text);
                cmd.Parameters.AddWithValue("@fecha", dtpFechaCompra.Value); // Usa .Value para DateTimePicker
                cmd.Parameters.AddWithValue("@cant", txtCantidad.Text);
                cmd.Parameters.AddWithValue("@pre", txtPrecio.Text); 

                cmd.ExecuteNonQuery();
                MessageBox.Show("Compra registrada con éxito.");
                LlenarTablaCompras();
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
        public void LlenarTablaCompras()
        {
            try
            {
               
                dataGridView1.DataSource = null;

               
                string consulta = "SELECT id_compra, id_cliente, id_producto, fechaCompra, cantidad, precio FROM COMPRAS";

                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);

                
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar datos: " + ex.Message);
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];

                txtIdCompra.Text = fila.Cells[0].Value.ToString();
                txtIdCliente.Text = fila.Cells[1].Value.ToString();
                txtIdProducto.Text = fila.Cells[2].Value.ToString();
                dtpFechaCompra.Text = fila.Cells[3].Value.ToString();
                txtCantidad.Text = fila.Cells[4].Value.ToString();
                txtPrecio.Text = fila.Cells[5].Value.ToString();
            }
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            LlenarTablaCompras();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdCompra.Text))
            {
                MessageBox.Show("Por favor, selecciona una compra de la tabla para eliminar.");
                return;
            }

            DialogResult resultado = MessageBox.Show("¿Estás seguro de eliminar esta compra?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    conexion.Open();
                    string query = "DELETE FROM COMPRAS WHERE id_compra = @id";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@id", txtIdCompra.Text);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Compra eliminada con éxito.");
                        LimpiarCampos();
                        LlenarTablaCompras();
                    }
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message);
                    if (conexion.State == ConnectionState.Open) conexion.Close();
                }
            }
        }
        private void LimpiarCampos()
        {
            txtIdCompra.Clear();
            txtIdCliente.Clear();
            txtIdProducto.Clear();
            txtCantidad.Clear();
            txtPrecio.Clear();
            dtpFechaCompra.Value = DateTime.Now;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdCompra.Text))
            {
                MessageBox.Show("Por favor, selecciona una compra de la tabla para modificar.");
                return;
            }

            try
            {
                if (conexion.State == ConnectionState.Closed) conexion.Open();

                string query = "UPDATE COMPRAS SET id_cliente=@idCli, id_producto=@idProd, " +
                               "fechaCompra=@fecha, cantidad=@cant, precio=@pre WHERE id_compra=@id";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@id", txtIdCompra.Text);
                cmd.Parameters.AddWithValue("@idCli", txtIdCliente.Text);
                cmd.Parameters.AddWithValue("@idProd", txtIdProducto.Text);
                cmd.Parameters.AddWithValue("@fecha", dtpFechaCompra.Value);
                cmd.Parameters.AddWithValue("@cant", txtCantidad.Text);
                cmd.Parameters.AddWithValue("@pre", txtPrecio.Text);

                int filas = cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    MessageBox.Show("Datos actualizados correctamente.");
                    LlenarTablaCompras(); // Refresca el DataGridView
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
