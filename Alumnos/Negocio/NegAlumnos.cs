using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using Alumnos.Datos.ClaseDatos;
using Alumnos.Regsitros.Alumno;
using Npgsql;

namespace Alumnos.Negocio.NegAlumnos
{
    class NegAlumnos
    {
        private ClaseDatos _clDatos;
        private Alumno alumno;
        private DataTable _dtabla;
        private String _buscar = "";
        private DataTable _DatosMod;



        public NegAlumnos(ClaseDatos clDatos)
        {
            _clDatos = clDatos;
            alumno = null;
            _dtabla = null;
            _DatosMod = null;
            _buscar = "";
        }

        public void Actualizar() {
            String sql = "";
            sql = "select * from alumnos ";
            if(!String.IsNullOrEmpty(_buscar)){
                sql += " where " 
                    +"dni like '%" + _buscar + "%'"
                    + "or nombre like '%" + _buscar + "%'"
                    + "or apellido1 like '%" + _buscar + "%'"
                    + "or apellido2 like '%" + _buscar + "%'";
            }    
                _clDatos.Query(sql, "alumnos");
            
        }
        public DataTable DevolverTabla(String nombreConsulta = "alumnos")
        {

            _dtabla = _clDatos.getDatos(nombreConsulta);
            return _dtabla;
        }


        // *************FUNCIONES DESCONECTADO *************
        public DataTable ActualizarDesconectado(){
            _clDatos.QueryDesconectado("select * from alumnos where registro<10", "modalumnos");
            _DatosMod = _clDatos.getDatos("modalumnos");
            return _DatosMod;
           }
        public void Sincronizar() {
            _clDatos.Sincronizar("modalumnos");
        }
     



   
        public Alumno DevolverAlumno(int fila){
         
                alumno = new Alumno();

                alumno.Registro = _dtabla.Rows[fila].Field<int>(0);
                alumno.Dni = _dtabla.Rows[fila].Field<String>(1);
                alumno.Nombre = _dtabla.Rows[fila].Field<String>(2);
                alumno.Apellido1 = _dtabla.Rows[fila].Field<String>(3);
                alumno.Apellido2 = _dtabla.Rows[fila].Field<String>(4);

                return alumno;
            
        }
        public int Modificaciones(Alumno alumno) {

            return 0;

        }
    
        public int Borrar(Alumno alumno) {
        int fila = _clDatos.EjecutaComando("DELETE FROM alumnos "
                + "WHERE registro = " + alumno.Registro);
        if (fila > 0) {
            Actualizar();
        }
        return fila;
    }

        public int ultimoIde()
        {
            String sql = "SELECT registro FROM alumnos WHERE registro = (SELECT MAX(registro) FROM alumnos);";
            int lastRegistro = 0;

            _clDatos.OpenConection();

            NpgsqlCommand comando = new NpgsqlCommand(sql, _clDatos.Conn);
            lastRegistro = Convert.ToInt32(comando.ExecuteScalar()) + 1;

            return lastRegistro;

        }

        public String Altas(Alumno alumno){


            string sql = "insert into alumnos (registro, dni, nombre, apellido1, apellido2) values (@registro, @dni, @nombre, @apellido1, @apellido2);";
                //+ " SELECT LAST_INSERT_ID();";

            _clDatos.OpenConection();

            NpgsqlCommand comando = new NpgsqlCommand(sql, _clDatos.Conn);
            
            comando.Parameters.Add("@registro", NpgsqlTypes.NpgsqlDbType.Integer, 14);
            comando.Parameters["@registro"].Value = alumno.Registro;
            
            comando.Parameters.Add("@dni", NpgsqlTypes.NpgsqlDbType.Varchar, 14);
            comando.Parameters["@dni"].Value = alumno.Dni;

            comando.Parameters.Add("@nombre", NpgsqlTypes.NpgsqlDbType.Varchar, 26);
            comando.Parameters["@nombre"].Value = alumno.Nombre;

            comando.Parameters.Add("@apellido1", NpgsqlTypes.NpgsqlDbType.Varchar, 21);
            comando.Parameters["@apellido1"].Value = alumno.Apellido1;

            comando.Parameters.Add("@apellido2", NpgsqlTypes.NpgsqlDbType.Varchar, 21);
            comando.Parameters["@apellido2"].Value = alumno.Apellido2;
           

           

            Object obt = comando.ExecuteScalar();


            _clDatos.CloseConection();
            return Convert.ToString(obt);
        
        }
        public String Buscar
        {
            get { return _buscar; }
            set { _buscar = value;
            Actualizar();
            }
        }
    

      
    }
}
