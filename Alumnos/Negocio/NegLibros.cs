using System;
using Alumnos.Datos.ClaseDatos;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alumnos.Registros.Libro;
using System.Data;
using Npgsql;

namespace Alumnos.Negocio.NegLibros
{
    class NegLibros
    {
        private ClaseDatos _clDatos;
        private Libro libro;
        private DataTable _dtabla;
        private String _buscar = "";
        private DataTable _DatosMod;



        public NegLibros(ClaseDatos clDatos)
        {
            _clDatos = clDatos;
            libro = null;
            _dtabla = null;
            _DatosMod = null;
            _buscar = "";
        }

        public void Actualizar()
        {
            String sql = "";
            sql = "select * from libros ";
            if (!String.IsNullOrEmpty(_buscar))
            {
                sql += " where "
                    + "Titulo like '%" + _buscar + "%'"
                    + "or Autor like '%" + _buscar + "%'"
                    + "or Editorial like '%" + _buscar + "%'"
                    + "or Asignatura like '%" + _buscar + "%'"
                    + "or estado like '%" + _buscar + "%'";
            }
            _clDatos.Query(sql, "libros");

        }
        public DataTable DevolverTabla(String nombreConsulta = "libros")
        {

            _dtabla = _clDatos.getDatos(nombreConsulta);
            return _dtabla;
        }


        // *************FUNCIONES DESCONECTADO *************
        public DataTable ActualizarDesconectado()
        {
            _clDatos.QueryDesconectado("select * from libros where codigo<10", "modlibros");
            _DatosMod = _clDatos.getDatos("modlibros");
            return _DatosMod;
        }
        public void Sincronizar()
        {
            _clDatos.Sincronizar("modlibros");
        }





        public Libro DevolverLibro(int fila)
        {

            libro = new Libro();

            libro.Codigo = _dtabla.Rows[fila].Field<int>(0);
            libro.Titulo = _dtabla.Rows[fila].Field<String>(1);
            libro.Autor = _dtabla.Rows[fila].Field<String>(2);
            libro.Editorial = _dtabla.Rows[fila].Field<String>(3);
            libro.Asignatura = _dtabla.Rows[fila].Field<String>(4);
            libro.Estado = _dtabla.Rows[fila].Field<String>(5);

            return libro;

        }
        public int Modificaciones(Libro libro)
        {

            return 0;

        }

        public int Borrar(Libro libro)
        {
            int fila = _clDatos.EjecutaComando("DELETE FROM libros "
                    + "WHERE codigo = " + libro.Codigo);
            if (fila > 0)
            {
                Actualizar();
            }
            return fila;
        }

        public int ultimoIde()
        {
            String sql = "SELECT codigo FROM libros WHERE codigo = (SELECT MAX(codigo) FROM libros);";
            int lastRegistro = 0;

            _clDatos.OpenConection();

            NpgsqlCommand comando = new NpgsqlCommand(sql, _clDatos.Conn);
            lastRegistro = Convert.ToInt32(comando.ExecuteScalar()) + 1;

            return lastRegistro;

        }


        public String Altas(Libro libro)
        {


            string sql = "insert into libros (codigo, Titulo, Autor, Editorial, Asignatura, estado) values (@codigo, @titulo, @autor, @editorial, @asignatura, @estado);";
                //+ " SELECT LAST_INSERT_ID();";

            _clDatos.OpenConection();

            NpgsqlCommand comando = new NpgsqlCommand(sql, _clDatos.Conn);

            comando.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Integer, 14);
            comando.Parameters["@codigo"].Value = libro.Codigo;

            comando.Parameters.Add("@titulo", NpgsqlTypes.NpgsqlDbType.Varchar, 67);
            comando.Parameters["@titulo"].Value = libro.Titulo;

            comando.Parameters.Add("@autor", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            comando.Parameters["@autor"].Value = libro.Autor;

            comando.Parameters.Add("@editorial", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            comando.Parameters["@editorial"].Value = libro.Editorial;

            comando.Parameters.Add("@asignatura", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            comando.Parameters["@asignatura"].Value = libro.Asignatura;

            comando.Parameters.Add("@estado", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            comando.Parameters["@estado"].Value = libro.Estado;




            Object obt = comando.ExecuteScalar();


            _clDatos.CloseConection();
            return Convert.ToString(obt);

        }
        public String Buscar
        {
            get { return _buscar; }
            set
            {
                _buscar = value;
                Actualizar();
            }
        }



    }
}
