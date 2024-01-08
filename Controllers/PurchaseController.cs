using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Proyecto_API.BaseDatos;
using Proyecto_API.Models;

namespace Proyecto_API.Controllers
{
    [ApiController]
    [Route("Purchase")]
    public class PurchaseController : ControllerBase
    {
        #region Crear
        [HttpPost]
        [Route("register-purchase")]
        public void registrarCompra(Compra compra)
        {
            string query = "INSERT INTO COMPRAS (COD_ESTUDIANTE, TOTAL_PRECIO, FECHA_CREACIÓN)";
            query = query + " VALUES (@COD_ESTUDIANTE, @TOTAL_PRECIO, @FECHA_CREACIÓN)";

            MySqlConnection conn = BD.getConnection();
            MySqlCommand comando = new MySqlCommand(query, conn);

            comando.Parameters.AddWithValue("@COD_ESTUDIANTE", compra._codStudent);
            comando.Parameters.AddWithValue("@TOTAL_PRECIO", compra._totalCompra);
            comando.Parameters.AddWithValue("@FECHA_CREACIÓN", compra._creationDate);

            comando.ExecuteNonQuery();

            BD.CloseConnection(conn);
        }
        #endregion

        #region Listar
        [HttpGet]
        [Route("list-purchase")]
        public List<Compra> listarCompras()
        {
            var ListaCompras = new List<Compra>();

            var conn = BD.getConnection();
            string sQuery = "SELECT * FROM COMPRAS";
            MySqlCommand command = new MySqlCommand(sQuery, conn);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Compra purchase = new Compra();
                purchase._idCompra = reader.GetInt32(0);
                purchase._codStudent = reader.GetString(1);
                purchase._totalCompra = reader.GetDecimal(2);
                purchase._creationDate = reader.GetDateTime(3);

                ListaCompras.Add(purchase);
            }

            reader.Close();

            BD.CloseConnection(conn);

            return ListaCompras;
        }
        #endregion

        #region Buscar por estudiante
        [HttpGet]
        [Route("list-purchase-by-student")]
        public List<Compra> listarComprasPorId(string cod_estudiante)
        {
            var ListaCompras = new List<Compra>();

            var conn = BD.getConnection();
            string sQuery = "SELECT * FROM COMPRAS WHERE COD_ESTUDIANTE = '" + cod_estudiante + "'";
            MySqlCommand command = new MySqlCommand(sQuery, conn);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(1).Equals(cod_estudiante))
                {
                    Compra purchase = new Compra();
                    purchase._idCompra = reader.GetInt32(0);
                    purchase._codStudent = reader.GetString(1);
                    purchase._totalCompra = reader.GetDecimal(2);
                    purchase._creationDate = reader.GetDateTime(3);

                    ListaCompras.Add(purchase);
                }

            }

            reader.Close();

            BD.CloseConnection(conn);

            return ListaCompras;
        }
        #endregion
    }
}
