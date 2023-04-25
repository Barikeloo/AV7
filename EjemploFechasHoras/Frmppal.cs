using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EjemploFechasHoras
{
    public partial class Frmppal : Form
    {
        public Frmppal()
        {
            InitializeComponent();
        }

        private void Frmppal_Load(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("HH:mm:ss");

            // Cargo en el datagrid todos los fichajes existentes en la BD
            if (ConBD.Conexion != null)
            {
                ConBD.AbrirConexion();
                dgvFichajes.DataSource = Fichaje.VerFichajesTotales();
                ConBD.CerrarConexion();

            }
        }

        private void tmrReloj_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("HH:mm:ss");
            lblFecha.Text = DateTime.Now.ToString("dd-mm-yyyy");
        }

        private void btnEntrada_Click(object sender, EventArgs e)
        {
            // Creamos el fichaje
            Fichaje f = new Fichaje(txtNif.Text);
            // Asignamos la fecha y hora de entrada al fichaje
            f.Fecha = DateTime.Now.Date;
            f.HEntrada = DateTime.Now;
            
            if (ConBD.Conexion != null)
            {
                ConBD.AbrirConexion();
                // Se llama al método DarEntrada.
                if (f.DarEntrada()==1)
                {
                    MessageBox.Show("Fichaje de entrada realizado con éxito");
                    dgvFichajes.DataSource = Fichaje.VerFichajesTotales();
                }
                ConBD.CerrarConexion();
            }
        }

        private void btnSalida_Click(object sender, EventArgs e)
        {

            // Creamos el fichaje
            Fichaje f = new Fichaje(txtNif.Text);
            f.HSalida= DateTime.Now;
            f.Finalizado = true;

            if (ConBD.Conexion != null)
            {
                ConBD.AbrirConexion();
                if (f.DarSalida() == 1)
                {
                    MessageBox.Show("Fichaje de salida realizado con éxito");
                    dgvFichajes.DataSource = Fichaje.VerFichajesTotales();
                } else
                {
                    MessageBox.Show("No se ha podido realizar la salida con éxito");
                }
                ConBD.CerrarConexion();
            }


        }
    }
}
