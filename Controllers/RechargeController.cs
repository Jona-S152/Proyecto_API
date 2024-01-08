using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Proyecto_API.BaseDatos;
using Proyecto_API.Models;

namespace Proyecto_API.Controllers
{
    [ApiController]
    [Route("recharge")]
    public class RechargeController : ControllerBase
    {
        #region Crear
        [HttpPost]
        [Route("register-recharge")]
        public void registrarRecarga(Recarga recarga)
        {
            var connection = BD.getConnection();
            string query = "INSERT INTO RECARGAS (COD_ESTUDIANTE, CANTIDAD, FECHA_CREACIÓN)";
            query = query + " VALUES (@COD_ESTUDIANTE, @CANTIDAD, @FECHA_CREACIÓN)";

            MySqlCommand comand = new MySqlCommand(query, connection);

            comand.Parameters.AddWithValue("@COD_ESTUDIANTE", recarga._codStudent);
            comand.Parameters.AddWithValue("@CANTIDAD", recarga._quantity);
            comand.Parameters.AddWithValue("@FECHA_CREACIÓN", recarga._creationDate);

            comand.ExecuteNonQuery();

            BD.CloseConnection(connection);
        }
        #endregion

        #region Listar
        [HttpGet]
        [Route("list-recharge")]
        public List<Recarga> listarRecargas()
        {
            var ListaRecargas = new List<Recarga>();

            var conn = BD.getConnection();
            string sQuery = "SELECT * FROM RECARGAS";
            MySqlCommand command = new MySqlCommand(sQuery, conn);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Recarga recharge = new Recarga();
                recharge._idRecarga = reader.GetInt32(0);
                recharge._codStudent = reader.GetString(1);
                recharge._quantity = reader.GetDecimal(2);
                recharge._creationDate = reader.GetDateTime(3);

                ListaRecargas.Add(recharge);
            }

            reader.Close();

            BD.CloseConnection(conn);

            return ListaRecargas;
        }
        #endregion

        #region Buscar por estudiante
        [HttpGet]
        [Route("list-recharge-by-student")]
        public List<Recarga> listarRecargasPorId(string cod_estudiante)
        {
            var ListaRecargas = new List<Recarga>();

            var conn = BD.getConnection();
            string sQuery = "SELECT * FROM RECARGAS WHERE COD_ESTUDIANTE = '" + cod_estudiante + "'";
            MySqlCommand command = new MySqlCommand(sQuery, conn);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(1).Equals(cod_estudiante))
                {
                    Recarga recharge = new Recarga();
                    recharge._idRecarga = reader.GetInt32(0);
                    recharge._codStudent = reader.GetString(1);
                    recharge._quantity = reader.GetDecimal(2);
                    recharge._creationDate = reader.GetDateTime(3);

                    ListaRecargas.Add(recharge);
                }

            }

            reader.Close();

            BD.CloseConnection(conn);

            return ListaRecargas;
        }
        #endregion
    }
}
