using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alumnos.Regsitros.Alumno
{
    class Alumno
    {
        private int _registro;
        private String _dni;
        private String _apellido1;
        private String _apellido2;
        private String _nombre;


        public Alumno() { 
           
        }
        public Alumno(int registro, String dni, String nombre, String apellido1, String apellido2){
            _registro = registro;
            _nombre = nombre;
            _dni = dni;
            _apellido1 = apellido1;
            _apellido2 = apellido2;

        }

        public int Registro
        {
            get { return _registro; }
            set { _registro = value; }
        }


        public String Dni
        {
            get { return _dni; }
            set { _dni = value; }
        }
       

        public String Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
     
        public String Apellido1
        {
            get { return _apellido1; }
            set { _apellido1 = value; }
        }
       

        public String Apellido2
        {
            get { return _apellido2; }
            set { _apellido2 = value; }
        }



    }
}
