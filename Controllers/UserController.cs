using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Proyecto_API.BaseDatos;
using Proyecto_API.Models;
using System.Data;

namespace Proyecto_API.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        #region Crear
        [HttpPost]
        [Route("register-user")]
        public void registrarUsuario(Usuario usuario)
        {
            string query = "INSERT INTO USUARIOS (CED_USUARIO, NOMBRE, APELLIDO, CONTRASEÑA, ROL, ESTADO)";
            query = query + " VALUES (@CED_USUARIO, @NOMBRE, @APELLIDO, @CONTRASEÑA, @ROL, @ESTADO)";

            MySqlConnection conn = BD.getConnection();
            MySqlCommand comando = new MySqlCommand(query, conn);

            comando.Parameters.AddWithValue("@CED_USUARIO", usuario._cedUsuario);
            comando.Parameters.AddWithValue("@NOMBRE", usuario._name);
            comando.Parameters.AddWithValue("@APELLIDO", usuario._lastName);
            comando.Parameters.AddWithValue("@CONTRASEÑA", usuario._password);
            comando.Parameters.AddWithValue("@ROL", usuario._rol);
            comando.Parameters.AddWithValue("@ESTADO", usuario._state);

            comando.ExecuteNonQuery();

            BD.CloseConnection(conn);
        }
        #endregion

        #region Listar
        [HttpGet]
        [Route("list-users")]
        public List<Usuario> listarUsuarios()
        {
            var ListaUsuarios = new List<Usuario>();

            var conn = BD.getConnection();
            string sQuery = "SELECT * FROM USUARIOS";
            MySqlCommand command = new MySqlCommand(sQuery, conn);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Usuario user = new Usuario();
                user._cedUsuario = reader.GetString(0);
                user._name = reader.GetString(1);
                user._lastName = reader.GetString(2);
                user._password = reader.GetString(3);
                user._rol = reader.GetString(4);
                user._state = Convert.ToBoolean(reader.GetInt32(5));

                ListaUsuarios.Add(user);
            }

            reader.Close();

            BD.CloseConnection(conn);

            return ListaUsuarios;
        }
        #endregion

        #region Buscar por id
        [HttpGet]
        [Route("search-user")]
        public Usuario buscarEstudiantePorId(string ced_usuario)
        {
            var usuario = new Usuario();

            var conn = BD.getConnection();
            string sQuery = "SELECT * FROM USUARIOS WHERE CED_USUARIO = '" + ced_usuario + "'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sQuery, conn);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            if (dataSet.Tables[0].Rows[0]["CED_USUARIO"].ToString().Equals(ced_usuario))
            {
                usuario._cedUsuario = dataSet.Tables[0].Rows[0]["CED_USUARIO"].ToString();
                usuario._name = dataSet.Tables[0].Rows[0]["NOMBRE"].ToString();
                usuario._lastName = dataSet.Tables[0].Rows[0]["APELLIDO"].ToString();
                usuario._password = dataSet.Tables[0].Rows[0]["CONTRASEÑA"].ToString();
                usuario._rol = dataSet.Tables[0].Rows[0]["ROL"].ToString();
                usuario._state = Convert.ToBoolean(dataSet.Tables[0].Rows[0]["ESTADO"]);
            }

            BD.CloseConnection(conn);

            return usuario;
        }
        #endregion

        #region Actualizar
        [HttpPut]
        [Route("update-user")]
        public void actualizarUsuario(Usuario usuario)
        {
            var connection = BD.getConnection();
            string sql = "SELECT * FROM USUARIOS WHERE CED_USUARIO = '" + usuario._cedUsuario + "'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            if (dataSet.Tables[0].Rows[0]["CED_USUARIO"].ToString().Equals(usuario._cedUsuario))
            {
                string sSentenciaSql = "UPDATE USUARIOS ";
                sSentenciaSql = sSentenciaSql + "SET NOMBRE = @NOMBRE, ";
                sSentenciaSql = sSentenciaSql + "APELLIDO = @APELLIDO, ";
                sSentenciaSql = sSentenciaSql + "CONTRASEÑA = @CONTRASEÑA, ";
                sSentenciaSql = sSentenciaSql + "ROL = @ROL, ";
                sSentenciaSql = sSentenciaSql + "ESTADO = @ESTADO ";
                sSentenciaSql = sSentenciaSql + "WHERE CED_USUARIO = @CED_USUARIO";
                MySqlConnection conexion = BD.getConnection();
                MySqlCommand comando = new MySqlCommand(sSentenciaSql, conexion);

                comando.Parameters.AddWithValue("@NOMBRE", usuario._name);
                comando.Parameters.AddWithValue("@APELLIDO", usuario._lastName);
                comando.Parameters.AddWithValue("@CONTRASEÑA", usuario._password);
                comando.Parameters.AddWithValue("@ROL", usuario._rol);
                comando.Parameters.AddWithValue("@ESTADO", usuario._state);
                comando.Parameters.AddWithValue("@CED_USUARIO", usuario._cedUsuario);

                comando.ExecuteNonQuery();
                BD.CloseConnection(conexion);
            }
            BD.CloseConnection(connection);
        }
        #endregion

        #region Eliminar
        [HttpDelete]
        [Route("delete-user")]
        public void eliminarUsuario(string ced_usuario, bool state)
        {
            var connection = BD.getConnection();
            string sql = "SELECT * FROM USUARIOS WHERE CED_USUARIO = '" + ced_usuario + "'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            if (dataSet.Tables[0].Rows[0]["CED_USUARIO"].ToString().Equals(ced_usuario))
            {
                string sSentenciaSql = "UPDATE USUARIOS ";
                sSentenciaSql = sSentenciaSql + "SET ESTADO = @ESTADO ";
                sSentenciaSql = sSentenciaSql + "WHERE CED_USUARIO = @CED_USUARIO";
                MySqlConnection conexion = BD.getConnection();
                MySqlCommand comando = new MySqlCommand(sSentenciaSql, conexion);

                comando.Parameters.AddWithValue("@ESTADO", state);
                comando.Parameters.AddWithValue("@CED_USUARIO", ced_usuario);

                comando.ExecuteNonQuery();
                BD.CloseConnection(conexion);
            }
            BD.CloseConnection(connection);
        }
        #endregion
    }
}
