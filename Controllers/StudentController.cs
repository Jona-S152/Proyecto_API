using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Proyecto_API.BaseDatos;
using Proyecto_API.Models;
using System.Data;

namespace Proyecto_API.Controllers
{
    [ApiController]
    [Route("student")]
    public class StudentController : ControllerBase
    {
        #region Crear
        [HttpPost]
        [Route("register-student")]
        public void registrarEstudiante(Estudiante estudiante)
        {
            string query = "INSERT INTO ESTUDIANTES (COD_ESTUDIANTE, CED_REPRESENTANTE, NOMBRE_ESTUDIANTE, APELLIDO_ESTUDIANTE, SALDO, ESTADO)";
            query = query + " VALUES (@COD_ESTUDIANTE, @CED_REPRESENTANTE, @NOMBRE_ESTUDIANTE, @APELLIDO_ESTUDIANTE, @SALDO, @ESTADO)";

            MySqlConnection conn = BD.getConnection();
            MySqlCommand comando = new MySqlCommand(query, conn);

            comando.Parameters.AddWithValue("@COD_ESTUDIANTE", estudiante._codStudent);
            comando.Parameters.AddWithValue("@CED_REPRESENTANTE", estudiante._cedRepresentative);
            comando.Parameters.AddWithValue("@NOMBRE_ESTUDIANTE", estudiante._nameStudent);
            comando.Parameters.AddWithValue("@APELLIDO_ESTUDIANTE", estudiante._lastNameStudent);
            comando.Parameters.AddWithValue("@SALDO", estudiante._balance);
            comando.Parameters.AddWithValue("@ESTADO", estudiante._state);

            comando.ExecuteNonQuery();

            BD.CloseConnection(conn);
        }
        #endregion

        #region Listar
        [HttpGet]
        [Route("list-students")]
        public List<Estudiante> listarEstudiantes()
        {
            var ListaEstudiantes = new List<Estudiante>();

            var conn = BD.getConnection();
            string sQuery = "SELECT * FROM ESTUDIANTES";
            MySqlCommand command = new MySqlCommand(sQuery, conn);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Estudiante student = new Estudiante();
                student._codStudent = reader.GetString(0);
                student._cedRepresentative = reader.GetString(1);
                student._nameStudent = reader.GetString(2);
                student._lastNameStudent = reader.GetString(3);
                student._balance = reader.GetDecimal(4);
                student._state = Convert.ToBoolean(reader.GetInt32(5));

                ListaEstudiantes.Add(student);
            }

            reader.Close();

            BD.CloseConnection(conn);

            return ListaEstudiantes;
        }
        #endregion

        #region Buscar por id
        [HttpGet]
        [Route("search-student")]
        public Estudiante buscarEstudiantePorId(string cod_estudiante)
        {
            var estudiante = new Estudiante();

            var conn = BD.getConnection();
            string sQuery = "SELECT * FROM ESTUDIANTES WHERE COD_ESTUDIANTE = '" + cod_estudiante + "'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sQuery, conn);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            if (dataSet.Tables[0].Rows[0]["COD_ESTUDIANTE"].ToString() == cod_estudiante)
            {
                estudiante._codStudent = dataSet.Tables[0].Rows[0]["COD_ESTUDIANTE"].ToString();
                estudiante._cedRepresentative = dataSet.Tables[0].Rows[0]["CED_REPRESENTANTE"].ToString();
                estudiante._nameStudent = dataSet.Tables[0].Rows[0]["NOMBRE_ESTUDIANTE"].ToString();
                estudiante._lastNameStudent = dataSet.Tables[0].Rows[0]["APELLIDO_ESTUDIANTE"].ToString();
                estudiante._balance = Convert.ToDecimal(dataSet.Tables[0].Rows[0]["SALDO"]);
                estudiante._state = Convert.ToBoolean(dataSet.Tables[0].Rows[0]["ESTADO"]);
            }

            BD.CloseConnection(conn);

            return estudiante;
        }
        #endregion

        #region Actualizar
        [HttpPut]
        [Route("update-student")]
        public void actualizarEstudiante(Estudiante estudiante)
        {
            var connection = BD.getConnection();
            string sql = "SELECT * FROM ESTUDIANTES WHERE COD_ESTUDIANTE = '" + estudiante._codStudent + "'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            if (dataSet.Tables[0].Rows[0]["COD_ESTUDIANTE"].ToString() == estudiante._codStudent)
            {
                string sSentenciaSql = "UPDATE ESTUDIANTES ";
                sSentenciaSql = sSentenciaSql + "SET CED_REPRESENTANTE = @CED_REPRESENTANTE, ";
                sSentenciaSql = sSentenciaSql + "NOMBRE_ESTUDIANTE = @NOMBRE_ESTUDIANTE, ";
                sSentenciaSql = sSentenciaSql + "APELLIDO_ESTUDIANTE = @APELLIDO_ESTUDIANTE, ";
                sSentenciaSql = sSentenciaSql + "SALDO = @SALDO, ";
                sSentenciaSql = sSentenciaSql + "ESTADO = @ESTADO ";
                sSentenciaSql = sSentenciaSql + "WHERE COD_ESTUDIANTE = @COD_ESTUDIANTE";
                MySqlConnection conexion = BD.getConnection();
                MySqlCommand comando = new MySqlCommand(sSentenciaSql, conexion);

                comando.Parameters.AddWithValue("@CED_REPRESENTANTE", estudiante._cedRepresentative);
                comando.Parameters.AddWithValue("@NOMBRE_ESTUDIANTE", estudiante._nameStudent);
                comando.Parameters.AddWithValue("@APELLIDO_ESTUDIANTE", estudiante._lastNameStudent);
                comando.Parameters.AddWithValue("@SALDO", estudiante._balance);
                comando.Parameters.AddWithValue("@ESTADO", estudiante._state);
                comando.Parameters.AddWithValue("@COD_ESTUDIANTE", estudiante._codStudent);

                comando.ExecuteNonQuery();
                BD.CloseConnection(conexion);
            }
            BD.CloseConnection(connection);
        }
        #endregion

        #region Actualizar saldo
        [HttpPut]
        [Route("update-balance")]
        public void actualizarSaldo(string cod_estudiante, decimal saldo)
        {
            var connection = BD.getConnection();
            string sql = "SELECT * FROM ESTUDIANTES WHERE COD_ESTUDIANTE = '" + cod_estudiante + "'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            if (dataSet.Tables[0].Rows[0]["COD_ESTUDIANTE"].ToString().Equals(cod_estudiante))
            {
                string sSentenciaSql = "UPDATE ESTUDIANTES ";
                sSentenciaSql = sSentenciaSql + "SET SALDO = @SALDO ";
                sSentenciaSql = sSentenciaSql + "WHERE COD_ESTUDIANTE = @COD_ESTUDIANTE";
                MySqlConnection conexion = BD.getConnection();
                MySqlCommand comando = new MySqlCommand(sSentenciaSql, conexion);

                comando.Parameters.AddWithValue("@SALDO", saldo);
                comando.Parameters.AddWithValue("@COD_ESTUDIANTE", cod_estudiante);

                comando.ExecuteNonQuery();
                BD.CloseConnection(conexion);
            }

            BD.CloseConnection(connection);
        }
        #endregion

        //#region Actualizar saldo (Compra)
        //[HttpPut]
        //[Route("update-balance-purchase")]
        //public void actualizarSaldoCompra(string cod_estudiante, decimal saldo)
        //{
        //    var connection = BD.getConnection();
        //    string sql = "SELECT * FROM ESTUDIANTES WHERE COD_ESTUDIANTE = '" + cod_estudiante + "'";
        //    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);

        //    DataSet dataSet = new DataSet();
        //    adapter.Fill(dataSet);

        //    if (dataSet.Tables[0].Rows[0]["COD_ESTUDIANTE"].ToString().Equals(cod_estudiante))
        //    {
        //        string sSentenciaSql = "UPDATE ESTUDIANTES ";
        //        sSentenciaSql = sSentenciaSql + "SET SALDO = @SALDO ";
        //        sSentenciaSql = sSentenciaSql + "WHERE COD_ESTUDIANTE = @COD_ESTUDIANTE";
        //        MySqlConnection conexion = BD.getConnection();
        //        MySqlCommand comando = new MySqlCommand(sSentenciaSql, conexion);

        //        comando.Parameters.AddWithValue("@SALDO", saldo);
        //        comando.Parameters.AddWithValue("@COD_ESTUDIANTE", cod_estudiante);

        //        comando.ExecuteNonQuery();
        //        BD.CloseConnection(conexion);
        //    }

        //    BD.CloseConnection(connection);
        //}
        //#endregion

        #region Eliminar
        [HttpDelete]
        [Route("delete-student")]
        public void eliminarProducto(string cod_estudiante, bool state)
        {
            var connection = BD.getConnection();
            string sql = "SELECT * FROM ESTUDIANTES WHERE COD_ESTUDIANTE = '" + cod_estudiante + "'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            if (dataSet.Tables[0].Rows[0]["COD_ESTUDIANTE"].ToString().Equals(cod_estudiante))
            {
                string sSentenciaSql = "UPDATE ESTUDIANTES ";
                sSentenciaSql = sSentenciaSql + "SET ESTADO = @ESTADO ";
                sSentenciaSql = sSentenciaSql + "WHERE COD_ESTUDIANTE = @COD_ESTUDIANTE";
                MySqlConnection conexion = BD.getConnection();
                MySqlCommand comando = new MySqlCommand(sSentenciaSql, conexion);

                comando.Parameters.AddWithValue("@ESTADO", state);
                comando.Parameters.AddWithValue("@COD_ESTUDIANTE", cod_estudiante);

                comando.ExecuteNonQuery();
                BD.CloseConnection(conexion);
            }
            BD.CloseConnection(connection);
        }
        #endregion
    }
}
