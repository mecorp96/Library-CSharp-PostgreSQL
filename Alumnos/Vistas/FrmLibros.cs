using Alumnos.Datos.ClaseDatos;
using Alumnos.Negocio.NegLibros;
using Alumnos.Registros.Libro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alumnos.Vistas
{
    public partial class FrmLibros : Form
    {
        private Timer timerbuscar;

        private NegLibros _neglibros;
        private Libro libro;
        private Libro currentLibro = new Libro();

        public FrmLibros(Boolean seleccionar, ClaseDatos clDatos)
        {
            InitializeComponent();
            InicializarEventos();
            _neglibros = new NegLibros(clDatos);
            btnSeleccionar.Enabled = seleccionar;
            CargarTabla();
            PonerCabecera();



        }


        private void InicializarEventos()
        {
            this.btnCerrar.Click += new System.EventHandler(this.BtnCerrar_Click);
            dataGridView1.RowEnter += DataGridView1_RowEnter;
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
            btnAltas.Click += BtnAltas_Click;
            //txtTitulo.TextChanged += TxtDni_TextChanged;
            btnModoDeconectado.Click += BtnModoDeconectado_Click;
            btnSincronizar.Click += BtnSincronizar_Click;

        }

        private void DataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int fila = (e.RowIndex == 0 ? 0 : e.RowIndex);
            currentLibro = _neglibros.DevolverLibro(fila);

            MostrarDatos();

            btnBajas.Enabled = (fila != -1);
            btnModoDeconectado.Enabled = (fila != -1);
        }


        private void CargarTabla()
        {
            _neglibros.Actualizar();
            dataGridView1.DataSource = _neglibros.DevolverTabla();
            //dataGridView1.DataSource = _negalumnos.DevolverTabla("alumnos");

            // PonerCabecera();        

        }

        private void PonerCabecera(Boolean cabecera = true)
        {

            dataGridView1.Columns[0].HeaderCell.Value = "Código";
            dataGridView1.Columns[1].HeaderCell.Value = "Título";
            dataGridView1.Columns[2].HeaderCell.Value = "Autor";
            dataGridView1.Columns[3].HeaderCell.Value = "Editorial";
            dataGridView1.Columns[4].HeaderCell.Value = "Asignatura";
            dataGridView1.Columns[5].HeaderCell.Value = "estado";
            dataGridView1.AllowUserToAddRows = !cabecera;
            dataGridView1.AllowUserToDeleteRows = !cabecera;
            dataGridView1.ReadOnly = cabecera;



        }

        private void CargarTablaDesconectado()
        {

            dataGridView1.DataSource = _neglibros.ActualizarDesconectado();

            PonerCabecera(false);

        }

        private void MostrarDatos()
        {

            txtCodigo.Text = (currentLibro != null ? Convert.ToString(currentLibro.Codigo) : "");
            txtAutor.Text = (currentLibro != null ? currentLibro.Autor : "");
            txtTitulo.Text = (currentLibro != null ? currentLibro.Titulo : "");
            txtEditorial.Text = (currentLibro != null ? currentLibro.Editorial : "");
            txtAsignatura.Text = (currentLibro != null ? currentLibro.Asignatura : "");
            txtEstado.Text = (currentLibro != null ? currentLibro.Estado : ""); ;


        }




        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
            //this.SetVisibleCore(false);

        }
        private void CambiosBotones(object sender, EventArgs e)
        {



            btnAltas.Enabled = !string.IsNullOrEmpty(txtTitulo.Text) && (
                        !string.IsNullOrEmpty(txtAutor.Text) ||
                        !string.IsNullOrEmpty(txtEditorial.Text) ||
                        !string.IsNullOrEmpty(txtAsignatura.Text) ||
                        !string.IsNullOrEmpty(txtEstado.Text));

        }





        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            ActivoTimer();
        }


        private void ActivoTimer()
        {
            if ((timerbuscar != null) && timerbuscar.Enabled)
            {
                timerbuscar.Start();
            }
            else
            {

                timerbuscar = new Timer();
                timerbuscar.Tick += new EventHandler(timerbuscar_Tick);
                timerbuscar.Interval = 300;
                timerbuscar.Start();
            }
        }

        private void timerbuscar_Tick(object sender, EventArgs e)
        {
            timerbuscar.Stop();
            timerbuscar.Dispose();
            BuscarAhora();
        }

        private void BuscarAhora()
        {

            _neglibros.Buscar = txtBuscar.Text;
            dataGridView1.Refresh();
        }

        private void BtnAltas_Click(object sender, EventArgs e)
        {

            libro = new Libro();
            libro.Codigo = _neglibros.ultimoIde();
            libro.Titulo = txtTitulo.Text;
            libro.Autor = txtAutor.Text;
            libro.Editorial = txtEditorial.Text;
            libro.Asignatura = txtAsignatura.Text;
            libro.Estado = txtEstado.Text;

            _neglibros.Altas(libro);

            //String id = _neglibros.Altas(libro);
            //txtCodigo.Text = (Convert.ToString(id));
            CargarTabla();
            
                


        }

        /*private void TxtDni_TextChanged(object sender, EventArgs e)
        {
            CambiosBotones();
        }

        private void CambiosBotones()
        {

        }*/

        private void btnBajas_Click(object sender, EventArgs e)
        {

            var result = MessageBox.Show("Está seguro que desea borrar el registro seleccionado ?", "Atención", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            if (result == DialogResult.Yes)
            {
                if (_neglibros.Borrar(currentLibro) > 0)
                {
                    CargarTabla();
                }
            }

        }


        private void BtnModoDeconectado_Click(object sender, EventArgs e)
        {
            CargarTablaDesconectado();
        }

        private void BtnSincronizar_Click(object sender, EventArgs e)
        {
            _neglibros.Sincronizar();
        }
    }
}
