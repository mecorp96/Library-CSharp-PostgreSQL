using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
 /*dll que previamente hemos descargado de la página de mysql y posteriormente hemos agregado
   como referencia a nuestro proyecto*/
using MySql.Data.MySqlClient;
using Npgsql;
using System.Configuration;




namespace Alumnos.Datos.ClaseDatos
{
    public class ClaseDatos

        
    {
        /* objeto que nos conecta con el servidor*/
        //private MySqlConnection _conn;
        private NpgsqlConnection _conn;

        /* lo utilizamos para manejar los datos en el cliente*/
        private DataSet _datos=new DataSet();

           /*objeto intermedio para traer los datos del servidor al cliente
          através de sql ejecutadas en el servidor*/
        //private MySqlDataAdapter adaptador;
        private NpgsqlDataAdapter adaptador;
        private String _servidor;
        private String _basedatos;
        private String _usuario;
        private String _clave;
        private NpgsqlCommandBuilder buildercomando;

      
        public ClaseDatos()
        {
            _servidor = "localhost";
            _basedatos = "libros";
            _usuario = "postgres";
            _clave = "postgres";
            _conn = null;


        }

        public ClaseDatos(String servidor, String basedatos, String usuario, String clave) {
            _servidor = servidor;
            _basedatos = basedatos;
            _usuario = usuario;
            _clave = clave;
            _conn = null;
                           
        }
        private void IniciliazarConection(){
            /* datos referentes a nuestro servidor y base de datos*/
          if (_conn == null) {
            //_conn = new MySqlConnection();
            _conn = new NpgsqlConnection();
                //_conn.ConnectionString = "Server=" + _servidor + ";Port=3306;Database=" + _basedatos + ";Uid=" + _usuario + ";Pwd=" + _clave + ";";
                _conn.ConnectionString = "Server=" + _servidor + ";Port=5432;User Id=" + _usuario + ";Password=" + _clave + ";Database=" + _basedatos + ";";
            }
         }

        public Boolean OpenConection() {

            IniciliazarConection();
            //si la connexión está cerrada; se pueden hacer más cosas
            if (_conn.State == System.Data.ConnectionState.Closed)
            {
                _conn.Open();
                return true;

            }
            return false;
            
        }

        public void CloseConection() {

            if (_conn.State == System.Data.ConnectionState.Open)
            {
                _conn.Close();
            }



        }
        
        
        /*
        public MySqlConnection Conn
        {
            get { return _conn; }
            

        }
        */

        public NpgsqlConnection Conn
        {
            get { return _conn; }


        }

        /*Al añadir cuaquier otra gestión de tablas o consultas¿? nos encontramos con un error, y es que si hacemos una nueva consulta
        'sql sobre el mismo dataset llamado libros, por ejemplo, los datos se añaden al final, es decir, tenemos el 
        'doble de registros. Por ello limpiamos el datatset. Lo hacemos con un try para ahorrarnos trabajo
        'ya que si éste no existe se produce un error sólo la primera vez.*/

        public void Query(String sql, String nombreConsulta) {
            try
            {
                _datos.Tables[nombreConsulta].Clear();
            }
            catch (Exception)
            {
                
                
            }
            //No sé si ponerlo en otro lado hay que pensar
            IniciliazarConection();
           //no es necesario abrir o cerrar la conexión ya que esta operación la realiza de manera automática
                adaptador = new NpgsqlDataAdapter(sql, _conn);
                adaptador.Fill(_datos, nombreConsulta);
           
            
           
        }

        public DataTable getDatos(String nombreConsulta) {

             return _datos.Tables[nombreConsulta];

         }

        public int EjecutaComando(string sql) {

            NpgsqlCommand comando = new NpgsqlCommand();
            int i = -1;
            try
            {
                if (OpenConection())
                {
                    comando.Connection = _conn;
                    comando.CommandType = CommandType.Text;
                    comando.CommandText = sql;
                    i = comando.ExecuteNonQuery();
                }
                CloseConection();
                



            }
            catch (Exception e)
            {

                throw new ApplicationException("Error ejecutano comando");
            }
            return i;
        }


        /*
   DESCONECTADO*/
        public void QueryDesconectado(String sql, String nombreConsulta)
        {
            try
            {
                _datos.Tables[nombreConsulta].Clear();
            }
            catch (Exception)
            {


            }
            //No sé si ponerlo en otro lado hay que pensar
            IniciliazarConection();
            //no es necesario abrir o cerrar la conexión ya que esta operación la realiza de manera automática
            //con los datos ya en el cliente (adaptador) y utilizando el método fill rellenamos
            //con los mismos el dataset (datos) para poder utilizar los mismos en el cliente
            adaptador = new NpgsqlDataAdapter(sql, _conn);
            buildercomando = new NpgsqlCommandBuilder(adaptador);
            adaptador.Fill(_datos, nombreConsulta);

        }

        public void Sincronizar(String nombreConsulta)
        { 
             /* a la hora de sincronizar lo primero que miramos es si ha habido cambios en datatset
                en tal caso la orden adaptador.update abre la conexión con el servidor y actualiza los datos en el mismo
                una vez realizada la actualización le indicamos al datatset que los datos que contiene 
                ya están actualizados en el servidor con la orden datos.AcceptChanges()
              */
         if (_datos.HasChanges(System.Data.DataRowState.Modified)){
           
            adaptador.Update(_datos.Tables[nombreConsulta]);
            _datos.AcceptChanges();
           }
        
        }
        public void Reject() { 
        
        // deshacemos los cambios realizados en el datatset
            _datos.RejectChanges();
        
        }
 
    }
  
    /* Descoctado
  
     *   Sub Anadir_Registro(NomConsulta As String, ByVal ParamArray Valores_Campos() As String)
        ' alta desconectada
        'creamos fila de manera genérica
        Dim fila As DataRow

        'aquí fila toma la estructura que tiene del datatset, es decir, toma la estructura 
        'de la consulta sql que hemos hecho en el adaptador

        fila = datos.Tables(NomConsulta).NewRow

        For i As Integer = 0 To Valores_Campos.Count - 1
            fila(i) = Valores_Campos(i)
        Next

        'añadimos al dataset la nueva fila 
        datos.Tables(NomConsulta).Rows.Add(fila)
    End Sub
     * 
         
     */
}
