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
    public partial class FormProveedores : Form
    {
        SqlConnection conexion = new SqlConnection(@"Data Source=DESKTOP-V3L425G\SQLEXPRESS;Initial Catalog=A3SCRUD;Integrated Security=True");
        public FormProveedores()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (conexion.State == ConnectionState.Closed) conexion.Open();

                
                string query = "INSERT INTO PROVEEDORES (nif, nombre, direccion) VALUES ( @nif, @nom, @dir)";

                SqlCommand cmd = new SqlCommand(query, conexion);

               
               
                cmd.Parameters.AddWithValue("@nif", txtNif.Text); 
                cmd.Parameters.AddWithValue("@nom", txtNombre.Text);
                cmd.Parameters.AddWithValue("@dir", txtDireccion.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Proveedor guardado con éxito.");
                LlenarTablaProveedores(); 
                LimpiarCampos();          
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
        public void LlenarTablaProveedores() 
        {

            try
            {
                // 1. Limpiar el origen de datos actual
                dataGridView1.DataSource = null;

                // 2. Definir la consulta con los nombres de columna de tu SQL Server
                string consulta = "SELECT id_proveedor, nif, nombre, direccion FROM PROVEEDORES";

                // 3. Ejecutar la consulta
                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
                DataTable dt = new DataTable();
                adaptador.Fill(dt);

                // 4. Asignar los resultados al DataGridView
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // 1. Verificamos que haya un DNI escrito
            if (string.IsNullOrEmpty(txtIdProveedor.Text))
            {
                MessageBox.Show("Por favor, selecciona un PROVEEDOR de la tabla para eliminar.");
                return;
            }

            // 2. Preguntar al usuario si está seguro
            DialogResult resultado = MessageBox.Show("¿Estás seguro de eliminar al proveedor con DNI: " + txtIdProveedor.Text + "?",
                "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    if (conexion.State == ConnectionState.Closed) conexion.Open();

                    // 3. Consulta SQL para eliminar
                    string query = "DELETE FROM PROVEEDORES WHERE dni = @dni";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@dni", txtIdProveedor.Text);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Proveedor eliminado correctamente.");
                        LlenarTablaProveedores(); // Refrescamos la tabla
                        LimpiarCampos();       // Limpiamos los cuadros de texto
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message);
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                    {
                        conexion.Close(); // Cambia Open() por Close()
                    }
                }


            }
        }
        private void LimpiarCampos()
        {
            txtIdProveedor.Clear();
            txtNif.Clear();     
            txtNombre.Clear();
            txtDireccion.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];

                txtIdProveedor.Text = fila.Cells[0].Value.ToString(); // id_proveedor
                txtNif.Text = fila.Cells[1].Value.ToString(); // nif
                txtNombre.Text = fila.Cells[2].Value.ToString(); // nombre
                txtDireccion.Text = fila.Cells[3].Value.ToString(); // direccion
            }
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            LlenarTablaProveedores();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (conexion.State == ConnectionState.Closed) conexion.Open();

                // Usamos UPDATE para cambiar los datos basándonos en el ID
                string query = "UPDATE PROVEEDORES SET nif=@nif, nombre=@nom, direccion=@dir WHERE id_proveedor=@id";

                SqlCommand cmd = new SqlCommand(query, conexion);

                // Pasamos los datos de los TextBox a los parámetros
                cmd.Parameters.AddWithValue("@id", txtIdProveedor.Text);
                cmd.Parameters.AddWithValue("@nif", txtNif.Text);
                cmd.Parameters.AddWithValue("@nom", txtNombre.Text);
                cmd.Parameters.AddWithValue("@dir", txtDireccion.Text);

                int filasAfectadas = cmd.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Datos del proveedor actualizados correctamente.");
                    LlenarTablaProveedores(); // Refresca la tabla para ver los cambios
                    LimpiarCampos();          // Limpia los cuadros de texto
                }
                else
                {
                    MessageBox.Show("No se encontró el proveedor para modificar.");
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
    }
}

