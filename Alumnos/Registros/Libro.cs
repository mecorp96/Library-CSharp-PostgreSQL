using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alumnos.Registros.Libro
{
    class Libro
    {
        private int _codigo;
        private String _titulo;
        private String _autor;
        private String _editorial;
        private String _asignatura;
        private String _estado;


        public Libro()
        {

        }
        public Libro(int codigo, String titulo, String autor, String editorial, String asignatura, String estado)
        {
            _codigo = codigo;
            _titulo = titulo;
            _autor = autor;
            _editorial = editorial;
            _asignatura = asignatura;
            _estado = estado;

        }

        public int Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }


        public String Titulo
        {
            get { return _titulo; }
            set { _titulo = value; }
        }


        public String Asignatura
        {
            get { return _asignatura; }
            set { _asignatura = value; }
        }

        public String Autor
        {
            get { return _autor; }
            set { _autor = value; }
        }


        public String Editorial
        {
            get { return _editorial; }
            set { _editorial = value; }
        }

        public String Estado
        {
            get { return _estado; }
            set { _estado = value; }
        }



    }
}
