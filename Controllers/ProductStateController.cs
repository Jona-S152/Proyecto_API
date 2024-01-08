using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Proyecto_API.BaseDatos;
using Proyecto_API.Models;
using System.Data;

namespace Proyecto_API.Controllers
{
    [ApiController]
    [Route("product-state")]
    public class ProductStateController : ControllerBase
    {
        #region Crear
        [HttpPost]
        [Route("register-product-state")]
        public void registrarEstadoProducto(EstadoProducto estadoProducto)
        {
            string query = "INSERT INTO ESTADO_PRODUCTOS (COD_ESTUDIANTE, ID_PRODUCTO, CANTIDAD, FECHA_CREACIÓN, ESTADO_PAGADO)";
            query = query + " VALUES (@COD_ESTUDIANTE, @ID_PRODUCTO, @CANTIDAD, @FECHA_CREACIÓN, @ESTADO_PAGADO)";

            MySqlConnection conn = BD.getConnection();
            MySqlCommand comando = new MySqlCommand(query, conn);

            comando.Parameters.AddWithValue("@COD_ESTUDIANTE", estadoProducto._codStudent);
            comando.Parameters.AddWithValue("@ID_PRODUCTO", estadoProducto._idProduct);
            comando.Parameters.AddWithValue("@CANTIDAD", estadoProducto._quantity);
            comando.Parameters.AddWithValue("@FECHA_CREACIÓN", estadoProducto._creationDate);
            comando.Parameters.AddWithValue("@ESTADO_PAGADO", estadoProducto._statePaid);

            comando.ExecuteNonQuery();

            BD.CloseConnection(conn);
        }
        #endregion

        #region Listar
        [HttpGet]
        [Route("list-product-state")]
        public List<EstadoProducto> listarEstadoProducto()
        {
            var ListaEstadoProducto = new List<EstadoProducto>();

            var conn = BD.getConnection();
            string sQuery = "SELECT * FROM ESTADO_PRODUCTOS";
            MySqlCommand command = new MySqlCommand(sQuery, conn);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                EstadoProducto state = new EstadoProducto();
                state._codStudent = reader.GetString(0);
                state._idProduct = reader.GetInt32(1);
                state._quantity = reader.GetInt32(2);
                state._creationDate = reader.GetDateTime(3);
                state._statePaid = Convert.ToBoolean(reader.GetInt32(4));

                ListaEstadoProducto.Add(state);
            }

            reader.Close();

            BD.CloseConnection(conn);

            return ListaEstadoProducto;
        }
        #endregion

        #region Buscar por estudiante
        [HttpGet]
        [Route("list-product-state-by-student")]
        public List<EstadoProducto> listarEstadoProductosPorId(string cod_estudiante)
        {
            var ListaEstadoProducto = new List<EstadoProducto>();

            var conn = BD.getConnection();
            string sQuery = "SELECT * FROM ESTADO_PRODUCTOS WHERE COD_ESTUDIANTE = '" + cod_estudiante + "'";
            MySqlCommand command = new MySqlCommand(sQuery, conn);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0).Equals(cod_estudiante))
                {
                    EstadoProducto state = new EstadoProducto();
                    state._codStudent = reader.GetString(0);
                    state._idProduct = reader.GetInt32(1);
                    state._quantity = reader.GetInt32(2);
                    state._creationDate = reader.GetDateTime(3);
                    state._statePaid = Convert.ToBoolean(reader.GetInt32(4));

                    ListaEstadoProducto.Add(state);
                }
                
            }

            reader.Close();

            BD.CloseConnection(conn);

            return ListaEstadoProducto;
        }
        #endregion

        #region Update Paid
        [HttpPut]
        [Route("set-paid-state")]
        public void actualizarEstadoProducto(string cod_estudiante, bool state)
        {
            var connection = BD.getConnection();
            string sql = "SELECT * FROM ESTADO_PRODUCTOS WHERE COD_ESTUDIANTE = '" + cod_estudiante + "'";
            MySqlCommand command = new MySqlCommand(sql, connection);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0).Equals(cod_estudiante) && reader.GetBoolean(4) == false)
                {
                    string sSentenciaSql = "UPDATE ESTADO_PRODUCTOS ";
                    sSentenciaSql = sSentenciaSql + "SET ESTADO_PAGADO = @ESTADO_PAGADO ";
                    sSentenciaSql = sSentenciaSql + "WHERE COD_ESTUDIANTE = @COD_ESTUDIANTE";
                    MySqlConnection conexion = BD.getConnection();
                    MySqlCommand comando = new MySqlCommand(sSentenciaSql, conexion);

                    comando.Parameters.AddWithValue("@ESTADO_PAGADO", state);
                    comando.Parameters.AddWithValue("@COD_ESTUDIANTE", cod_estudiante);

                    comando.ExecuteNonQuery();

                    BD.CloseConnection(conexion);
                }

            }

            reader.Close();

            BD.CloseConnection(connection);
        }
        #endregion
    }
}
