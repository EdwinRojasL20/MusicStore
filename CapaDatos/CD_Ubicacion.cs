using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;

using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Ubicacion
    {

        public List<DEPARTAMENTO> ObtenerDepartamento()
        {

            List<DEPARTAMENTO> lista = new List<DEPARTAMENTO>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from DEPARTAMENTO";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            lista.Add(
                                new DEPARTAMENTO()
                                {
                                    IdDepartamento = dr["IdDepartamento"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                }

                                );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<DEPARTAMENTO>();
            }
            return lista;
        }


        public List<LOCALIDAD> ObtenerLocalidad(string iddepartamento)
        {

            List<LOCALIDAD> lista = new List<LOCALIDAD>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from LOCALIDAD where IdDepartamento = @iddepartamento";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@iddepartamento", iddepartamento);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            lista.Add(
                                new LOCALIDAD()
                                {
                                    IdLocalidad = dr["IdLocalidad"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                }

                                );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<LOCALIDAD>();
            }
            return lista;
        }


        public List<DISTRITO> ObtenerDistrito(string iddepartamento, string idlocalidad)
        {

            List<DISTRITO> lista = new List<DISTRITO>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from DISTRITO where IdLocalidad = @idlocalidad and IdDepartamento = @iddepartamento";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idlocalidad", idlocalidad);
                    cmd.Parameters.AddWithValue("@iddepartamento", iddepartamento);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            lista.Add(
                                new DISTRITO()
                                {
                                    IdDristrito = dr["IdDristrito"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                }

                                );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<DISTRITO>();
            }
            return lista;
        }


    }
}
