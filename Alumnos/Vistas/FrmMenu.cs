using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alumnos.Datos.ClaseDatos;



namespace Alumnos.Vistas
{
    public partial class FrmMenu : Form
    {

        private ClaseDatos clDatos;

        public FrmMenu()
        {
            clDatos = new ClaseDatos();
            InitializeComponent();
            InicializarEventos();
           
 
        }

        private void InicializarEventos() {
            btnAlumnos.Click += BtnAlumnos_Click;
            btnLibros.Click += BtnLibros_Click;
            this.FormClosing += FrmMenu_FormClosing;
            
        }

        private void FrmMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            clDatos = null;
        }

        private void BtnAlumnos_Click(object sender, EventArgs e)
        {
            FrmAlumnos frm = new FrmAlumnos(false, clDatos);
            frm.ShowDialog();
        }

        private void BtnLibros_Click(object sender, EventArgs e)
        {
            FrmLibros frm = new FrmLibros(false, clDatos);
            frm.ShowDialog();
        }

    }
}
