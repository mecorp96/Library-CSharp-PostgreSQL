using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alumnos.Negocio.NegAlumnos;
using Alumnos.Datos.ClaseDatos;
using Alumnos.Regsitros.Alumno;




namespace Alumnos.Vistas
{
    public partial class FrmAlumnos : Form
    {
        private Timer timerbuscar;

        private NegAlumnos _negalumnos;
        private Alumno alumno;
        private Alumno currentAlumno= new Alumno();

        public FrmAlumnos(Boolean seleccionar, ClaseDatos clDatos)
        {
            InitializeComponent();
            InicializarEventos();
            _negalumnos = new NegAlumnos(clDatos);
            btnSeleccionar.Enabled = seleccionar;
            CargarTabla();
            PonerCabecera();

         

        }

        //Método para inicializar botones y dataGridView
        private void InicializarEventos() {

            this.btnCerrar.Click += new System.EventHandler(this.BtnCerrar_Click);
                //La siguiente sentencia inicializa el dataGridView por fila
            dataGridView1.RowEnter += DataGridView1_RowEnter;
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
            btnAltas.Click += BtnAltas_Click;
            //txtDni.TextChanged += TxtDni_TextChanged;
            btnModoDeconectado.Click += BtnModoDeconectado_Click;
            btnSincronizar.Click += BtnSincronizar_Click;

        }

        private void DataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

            int fila = (e.RowIndex == 0 ? 0 : e.RowIndex);
            currentAlumno = _negalumnos.DevolverAlumno(fila);

            MostrarDatos();

            //Botón de bajas y desconectado está activado siempre que haya alguna fila
            btnBajas.Enabled = (fila != -1);
            btnModoDeconectado.Enabled = (fila != -1);
        }


        private void CargarTabla() {
            _negalumnos.Actualizar();

            dataGridView1.DataSource = _negalumnos.DevolverTabla();
            //dataGridView1.DataSource = _negalumnos.DevolverTabla("alumnos");

           // PonerCabecera();        
        
        }


        /*Con el siguiente método, al tener la cabecera en true, no podemos modificar directamente
         * clicando en el GridView, pero al usar el método desconectado cambiamos la variable cabecera,
         * por lo que entonces estando en el estado Desconectado, podemos modificar directamente desde el GridView
         * luego solo tenemos que darle al botón de sincronizar que se encarga de actualizar todo.
         */
        private void PonerCabecera(Boolean cabecera = true)
        {
                
                dataGridView1.Columns[0].HeaderCell.Value = "Registro";
                dataGridView1.Columns[1].HeaderCell.Value = "Dni";
                dataGridView1.Columns[2].HeaderCell.Value = "Nombre";
                dataGridView1.Columns[3].HeaderCell.Value = "Apellido1";
                dataGridView1.Columns[4].HeaderCell.Value = "Apellido2";
                dataGridView1.AllowUserToAddRows = !cabecera;
                dataGridView1.AllowUserToDeleteRows = !cabecera;
                dataGridView1.ReadOnly = cabecera;

        
        
        }

        private void CargarTablaDesconectado()
        {
            //El método ActualizarDesconectado() tiene la acción de consultar hasta el registro nº10
            //En este método cogemos el modalumnos mediante un return
            //Y se lo ponemos al dataGridView para que se muestren solo estos
            dataGridView1.DataSource = _negalumnos.ActualizarDesconectado();

            PonerCabecera(false);        

        }

        private void MostrarDatos()
        {
         
            txtRegistro.Text = (currentAlumno != null ?  Convert.ToString(currentAlumno.Registro) : "");
            txtNombre.Text = (currentAlumno != null ? currentAlumno.Nombre : "");
            txtDni.Text = (currentAlumno != null ? currentAlumno.Dni : "");
            txtApellido1.Text = (currentAlumno != null ? currentAlumno.Apellido1 : "");
            txtApellido2.Text = (currentAlumno != null ? currentAlumno.Apellido2 : ""); ;


        }




        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
            //this.SetVisibleCore(false);

        }
        
        private void CambiosBotones(object sender, EventArgs e)
        {

           
            //Siempre que haya texto en uno de los siguientes campos, el botón de altas está activado
            btnAltas.Enabled = !string.IsNullOrEmpty(txtDni.Text) && (
                        !string.IsNullOrEmpty(txtNombre.Text) ||
                        !string.IsNullOrEmpty(txtApellido1.Text) ||
                        !string.IsNullOrEmpty(txtApellido2.Text));
            
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
                timerbuscar.Tick +=new EventHandler(timerbuscar_Tick);
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

            _negalumnos.Buscar = txtBuscar.Text;
            dataGridView1.Refresh();
        }

        private void BtnAltas_Click(object sender, EventArgs e)
        {
          
                alumno=new Alumno();
                alumno.Registro = _negalumnos.ultimoIde();
                alumno.Dni=txtDni.Text;
                alumno.Nombre=txtNombre.Text;
                alumno.Apellido1 = txtApellido1.Text;
                alumno.Apellido2 = txtApellido2.Text;

            _negalumnos.Altas(alumno);
               //String id = _negalumnos.Altas(alumno);
               //txtRegistro.Text=(Convert.ToString(id));
               CargarTabla();
               


          
        }
        /*
        private void TxtDni_TextChanged(object sender, EventArgs e)
        {
            CambiosBotones();
        }

        private void CambiosBotones()
        {
        
        }
        */

        private void btnBajas_Click(object sender, EventArgs e)
        {
            
           var result=MessageBox.Show( "Está seguro que desea borrar el registro seleccionado ?", "Atención", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            if(result==DialogResult.Yes){
                if (_negalumnos.Borrar(currentAlumno) > 0) {
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
            _negalumnos.Sincronizar();
        }
    }
}

