using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Proyecto_API.BaseDatos;
using Proyecto_API.Models;
using System.Data;

namespace Proyecto_API.Controllers
{
    [ApiController]
    [Route("representative")]
    public class RepresentativeController : ControllerBase
    {
        #region Crear
        [HttpPost]
        [Route("register-representative")]
        public void registrarRepresentante(Representante representante)
        {
            string query = "INSERT INTO REPRESENTANTES (CED_REPRESENTANTE, NOMBRE_REPRESENTANTE, APELLIDO_REPRESENTANTE, CONTRASEÑA, ESTADO)";
            query = query + " VALUES (@CED_REPRESENTANTE, @NOMBRE_REPRESENTANTE, @APELLIDO_REPRESENTANTE, @CONTRASEÑA, @ESTADO)";

            MySqlConnection conn = BD.getConnection();
            MySqlCommand comando = new MySqlCommand(query, conn);

            comando.Parameters.AddWithValue("@CED_REPRESENTANTE", representante._cedRepresentative);
            comando.Parameters.AddWithValue("@NOMBRE_REPRESENTANTE", representante._nameRepresentative);
            comando.Parameters.AddWithValue("@APELLIDO_REPRESENTANTE", representante._lastNameRepresentative);
            comando.Parameters.AddWithValue("@CONTRASEÑA", representante._password);
            comando.Parameters.AddWithValue("@ESTADO", representante._state);

            comando.ExecuteNonQuery();

            BD.CloseConnection(conn);
        }
        #endregion

        #region Listar
        [HttpGet]
        [Route("list-representatives")]
        public List<Representante> listarRepresentantes()
        {
            var ListaRepresentantes = new List<Representante>();

            var conn = BD.getConnection();
            string sQuery = "SELECT * FROM REPRESENTANTES";
            MySqlCommand command = new MySqlCommand(sQuery, conn);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Representante representative = new Representante();
                representative._cedRepresentative = reader.GetString(0);
                representative._nameRepresentative = reader.GetString(1);
                representative._lastNameRepresentative = reader.GetString(2);
                representative._password = reader.GetString(3);
                representative._state = Convert.ToBoolean(reader.GetInt32(4));

                ListaRepresentantes.Add(representative);
            }

            reader.Close();

            BD.CloseConnection(conn);

            return ListaRepresentantes;
        }
        #endregion

        #region Buscar por id
        [HttpGet]
        [Route("search-representative")]
        public Representante buscarRepresentantePorId(string ced_representante)
        {
            var representante = new Representante();

            var conn = BD.getConnection();
            string sQuery = "SELECT * FROM REPRESENTANTES WHERE CED_REPRESENTANTE = '" + ced_representante + "'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sQuery, conn);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            if (dataSet.Tables[0].Rows[0]["CED_REPRESENTANTE"].ToString() == ced_representante)
            {
                representante._cedRepresentative = dataSet.Tables[0].Rows[0]["CED_REPRESENTANTE"].ToString();
                representante._nameRepresentative = dataSet.Tables[0].Rows[0]["NOMBRE_REPRESENTANTE"].ToString();
                representante._lastNameRepresentative = dataSet.Tables[0].Rows[0]["APELLIDO_REPRESENTANTE"].ToString();
                representante._password = dataSet.Tables[0].Rows[0]["CONTRASEÑA"].ToString();
                representante._state = Convert.ToBoolean(dataSet.Tables[0].Rows[0]["ESTADO"]);
            }

            BD.CloseConnection(conn);

            return representante;
        }
        #endregion

        #region Actualizar
        [HttpPut]
        [Route("update-representative")]
        public void actualizarRepresentante(Representante representante)
        {
            var connection = BD.getConnection();
            string sql = "SELECT * FROM REPRESENTANTES WHERE CED_REPRESENTANTE = '" + representante._cedRepresentative + "'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            if (dataSet.Tables[0].Rows[0]["CED_REPRESENTANTE"].ToString().Equals(representante._cedRepresentative))
            {
                string sSentenciaSql = "UPDATE REPRESENTANTES ";
                sSentenciaSql = sSentenciaSql + "SET NOMBRE_REPRESENTANTE = @NOMBRE_REPRESENTANTE, ";
                sSentenciaSql = sSentenciaSql + "APELLIDO_REPRESENTANTE = @APELLIDO_REPRESENTANTE, ";
                sSentenciaSql = sSentenciaSql + "CONTRASEÑA = @CONTRASEÑA, ";
                sSentenciaSql = sSentenciaSql + "ESTADO = @ESTADO ";
                sSentenciaSql = sSentenciaSql + "WHERE CED_REPRESENTANTE = @CED_REPRESENTANTE";
                MySqlConnection conexion = BD.getConnection();
                MySqlCommand comando = new MySqlCommand(sSentenciaSql, conexion);

                comando.Parameters.AddWithValue("@NOMBRE_REPRESENTANTE", representante._nameRepresentative);
                comando.Parameters.AddWithValue("@APELLIDO_REPRESENTANTE", representante._lastNameRepresentative);
                comando.Parameters.AddWithValue("@CONTRASEÑA", representante._password);
                comando.Parameters.AddWithValue("@ESTADO", representante._state);
                comando.Parameters.AddWithValue("@CED_REPRESENTANTE", representante._cedRepresentative);

                comando.ExecuteNonQuery();
                BD.CloseConnection(conexion);
            }
            BD.CloseConnection(connection);
        }
        #endregion

        #region Eliminar
        [HttpDelete]
        [Route("delete-representative")]
        public void eliminarRepresentante (string ced_representante, bool state)
        {
            var connection = BD.getConnection();
            string sql = "SELECT * FROM REPRESENTANTES WHERE CED_REPRESENTANTE = '" + ced_representante + "'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            if (dataSet.Tables[0].Rows[0]["CED_REPRESENTANTE"].ToString().Equals(ced_representante))
            {
                string sSentenciaSql = "UPDATE REPRESENTANTES ";
                sSentenciaSql = sSentenciaSql + "SET ESTADO = @ESTADO ";
                sSentenciaSql = sSentenciaSql + "WHERE CED_REPRESENTANTE = @CED_REPRESENTANTE";
                MySqlConnection conexion = BD.getConnection();
                MySqlCommand comando = new MySqlCommand(sSentenciaSql, conexion);

                comando.Parameters.AddWithValue("@ESTADO", state);
                comando.Parameters.AddWithValue("@CED_REPRESENTANTE", ced_representante);

                comando.ExecuteNonQuery();
                BD.CloseConnection(conexion);
            }
            BD.CloseConnection(connection);
        }
        #endregion
    }
}
