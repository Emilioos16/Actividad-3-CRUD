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
    public partial class FormClientes : Form
    {
        SqlConnection conexion = new SqlConnection(@"Data Source=DESKTOP-V3L425G\SQLEXPRESS;Initial Catalog=A3SCRUD;Integrated Security=True");
        public FormClientes()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (conexion.State == ConnectionState.Closed) conexion.Open();

                string query = "INSERT INTO CLIENTES (dni, nombre, apellidos, fechaNac, tlfno, email) " +
               "VALUES (@dni, @nom, @ape, @fec, @tel, @mail)";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@dni", txtDni.Text);
                cmd.Parameters.AddWithValue("@nom", txtNombre.Text);
                cmd.Parameters.AddWithValue("@ape", txtApellidos.Text);
                cmd.Parameters.AddWithValue("@fec", dtpFechaNac.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@tel", txtTelefono.Text);
                cmd.Parameters.AddWithValue("@mail", txtEmail.Text);

                cmd.ExecuteNonQuery();

                // Mensaje de éxito solicitado en las instrucciones
                MessageBox.Show("El usuario fue agregado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information); //
                txtDni.Clear();
                txtNombre.Clear();
                txtApellidos.Clear();
                txtTelefono.Clear();
                txtEmail.Clear();
                dtpFechaNac.Value = DateTime.Now;

                LlenarTablaClientes(); // Método para refrescar el DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }
        public void LlenarTablaClientes()
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = null;
            string consulta = "SELECT id_cliente, dni, nombre, apellidos, fechaNac, tlfno, email FROM CLIENTES";
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
            DataTable dt = new DataTable();
            adaptador.Fill(dt);
            dataGridView1.DataSource = dt; //
        }

        

        private void FormClientes_Load(object sender, EventArgs e)
        {
            LlenarTablaClientes(); // Carga los datos al abrir la ventana

        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (conexion.State == ConnectionState.Closed) conexion.Open();

                // Consulta SQL para actualizar los datos basados en el DNI
                string query = "UPDATE CLIENTES SET nombre=@nom, apellidos=@ape, fechaNac=@fec, tlfno=@tel, email=@mail WHERE dni=@dni";

                SqlCommand cmd = new SqlCommand(query, conexion);

                // Parámetros con los nuevos datos de los TextBox
                cmd.Parameters.AddWithValue("@dni", txtDni.Text);
                cmd.Parameters.AddWithValue("@nom", txtNombre.Text);
                cmd.Parameters.AddWithValue("@ape", txtApellidos.Text);
                cmd.Parameters.AddWithValue("@fec", dtpFechaNac.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@tel", txtTelefono.Text);
                cmd.Parameters.AddWithValue("@mail", txtEmail.Text);

                int cantidad = cmd.ExecuteNonQuery();

                if (cantidad > 0)
                {
                    MessageBox.Show("El usuario fue modificado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LlenarTablaClientes(); // Refrescamos la tabla para ver los cambios
                }
                else
                {
                    MessageBox.Show("No se encontró un cliente con ese DNI para modificar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar: " + ex.Message);
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

                // Esto sube los datos de la tabla a los cuadros de texto
                txtDni.Text = fila.Cells[1].Value.ToString();
                txtNombre.Text = fila.Cells[2].Value.ToString();
                txtApellidos.Text = fila.Cells[3].Value.ToString();
                dtpFechaNac.Value = Convert.ToDateTime(fila.Cells[4].Value);
                txtTelefono.Text = fila.Cells[5].Value.ToString();
                txtEmail.Text = fila.Cells[6].Value.ToString();
            }
        }
         
        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close(); // Cierra el formulario actual
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // 1. Verificamos que haya un DNI escrito
            if (string.IsNullOrEmpty(txtDni.Text))
            {
                MessageBox.Show("Por favor, selecciona un cliente de la tabla para eliminar.");
                return;
            }

            // 2. Preguntar al usuario si está seguro
            DialogResult resultado = MessageBox.Show("¿Estás seguro de eliminar al cliente con DNI: " + txtDni.Text + "?",
                "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    if (conexion.State == ConnectionState.Closed) conexion.Open();

                    // 3. Consulta SQL para eliminar
                    string query = "DELETE FROM CLIENTES WHERE dni = @dni";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@dni", txtDni.Text);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Cliente eliminado correctamente.");
                        LlenarTablaClientes(); // Refrescamos la tabla
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
            txtDni.Clear();
            txtNombre.Clear();
            txtApellidos.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            dtpFechaNac.Value = DateTime.Now; // Reinicia la fecha a hoy
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}


