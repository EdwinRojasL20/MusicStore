using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Ubicacion
    {

        private CD_Ubicacion objCapaDato = new CD_Ubicacion();

        public List<DEPARTAMENTO> ObtenerDepartamento()
        {
            return objCapaDato.ObtenerDepartamento();
        }

        public List<LOCALIDAD> ObtenerLocalidad(string iddepartamento)
        {
            return objCapaDato.ObtenerLocalidad(iddepartamento);
        }

        public List<DISTRITO> ObtenerDistrito(string iddepartamento, string idlocalidad)
        {
            return objCapaDato.ObtenerDistrito(iddepartamento, idlocalidad);
        }

    }
}
