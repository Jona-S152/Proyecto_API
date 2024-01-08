using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Proyecto_API.BaseDatos;
using Proyecto_API.Models;
using System.Data;

namespace Proyecto_API.Controllers
{
    [ApiController]
    [Route("product")]
    public class ProductController : ControllerBase
    {
        #region Crear
        [HttpPost]
        [Route("save-product")]
        public void registrarProducto(Producto producto)
        {
            string query = "INSERT INTO PRODUCTOS (NOMBRE_PRODUCTO, CATEGORY, STOCK, PRECIO, ESTADO, URL_IMAGE)";
            query = query + " VALUES (@NOMBRE_PRODUCTO, @CATEGORY, @STOCK, @PRECIO, @ESTADO, @URL_IMAGE)";

            MySqlConnection conn = BD.getConnection();
            MySqlCommand comando = new MySqlCommand(query, conn);

            comando.Parameters.AddWithValue("@NOMBRE_PRODUCTO", producto._productName);
            comando.Parameters.AddWithValue("@CATEGORY", producto._category);
            comando.Parameters.AddWithValue("@STOCK", producto._stock);
            comando.Parameters.AddWithValue("@PRECIO", producto._price);
            comando.Parameters.AddWithValue("@ESTADO", producto._state);
            comando.Parameters.AddWithValue("@URL_IMAGE", producto._urlImage);

            comando.ExecuteNonQuery();

            BD.CloseConnection(conn);
        }
        #endregion

        #region Listar
        [HttpGet]
        [Route("list-products")]
        public List<Producto> listarProductos()
        {
            var ListaProductos = new List<Producto>();

            var conn = BD.getConnection();
            string sQuery = "SELECT * FROM PRODUCTOS";
            MySqlCommand command = new MySqlCommand(sQuery, conn);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Producto product = new Producto();
                product._idProduct = reader.GetInt32(0);
                product._productName = reader.GetString(1);
                product._category = reader.GetString(2);
                product._stock = reader.GetInt32(3);
                product._price = reader.GetDecimal(4);
                product._state = Convert.ToBoolean(reader.GetInt32(5));
                product._urlImage = reader.GetString(6);

                ListaProductos.Add(product);
            }

            reader.Close();

            BD.CloseConnection(conn);

            return ListaProductos;
        }
        #endregion

        #region Buscar por id
        [HttpGet]
        [Route("search-product")]
        public Producto buscarProductoPorId(int id_producto)
        {
            var producto = new Producto();

            var conn = BD.getConnection();
            string sQuery = "SELECT * FROM PRODUCTOS WHERE ID_PRODUCTO = '"+id_producto+"'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sQuery, conn);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            if (Convert.ToInt32(dataSet.Tables[0].Rows[0]["ID_PRODUCTO"]) == id_producto)
            {
                producto._idProduct = Convert.ToInt32(dataSet.Tables[0].Rows[0]["ID_PRODUCTO"]);
                producto._productName = dataSet.Tables[0].Rows[0]["NOMBRE_PRODUCTO"].ToString();
                producto._category = dataSet.Tables[0].Rows[0]["CATEGORY"].ToString();
                producto._stock = Convert.ToInt32(dataSet.Tables[0].Rows[0]["STOCK"]);
                producto._price = Convert.ToDecimal(dataSet.Tables[0].Rows[0]["PRECIO"]);
                producto._state = Convert.ToBoolean(dataSet.Tables[0].Rows[0]["ESTADO"]);
                producto._urlImage = dataSet.Tables[0].Rows[0]["URL_IMAGE"].ToString();

            }

            BD.CloseConnection(conn);

            return producto;
        }
        #endregion

        #region Actualizar
        [HttpPut]
        [Route("update-product")]
        public void actualizarProducto(Producto producto)
        {
            var connection = BD.getConnection();
            string sql = "SELECT * FROM PRODUCTOS WHERE ID_PRODUCTO = '" + producto._idProduct + "'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            if (Convert.ToInt32(dataSet.Tables[0].Rows[0]["ID_PRODUCTO"]) == producto._idProduct)
            {
                string sSentenciaSql = "UPDATE PRODUCTOS ";
                sSentenciaSql = sSentenciaSql + "SET NOMBRE_PRODUCTO = @NOMBRE_PRODUCTO, ";
                sSentenciaSql = sSentenciaSql + "CATEGORY = @CATEGORY, ";
                sSentenciaSql = sSentenciaSql + "STOCK = @STOCK, ";
                sSentenciaSql = sSentenciaSql + "PRECIO = @PRECIO, ";
                sSentenciaSql = sSentenciaSql + "ESTADO = @ESTADO, ";
                sSentenciaSql = sSentenciaSql + "URL_IMAGE = @URL_IMAGE ";
                sSentenciaSql = sSentenciaSql + "WHERE ID_PRODUCTO = @ID_PRODUCTO";
                MySqlConnection conexion = BD.getConnection();
                MySqlCommand comando = new MySqlCommand(sSentenciaSql, conexion);

                comando.Parameters.AddWithValue("@NOMBRE_PRODUCTO", producto._productName);
                comando.Parameters.AddWithValue("@CATEGORY", producto._category);
                comando.Parameters.AddWithValue("@STOCK", producto._stock);
                comando.Parameters.AddWithValue("@PRECIO", producto._price);
                comando.Parameters.AddWithValue("@ESTADO", producto._state);
                comando.Parameters.AddWithValue("@URL_IMAGE", producto._urlImage);
                comando.Parameters.AddWithValue("@ID_PRODUCTO", producto._idProduct);

                comando.ExecuteNonQuery();
                BD.CloseConnection(conexion);
            }
            BD.CloseConnection(connection);
        }
        #endregion

        #region Eliminar
        [HttpDelete]
        [Route("delete-product")]
        public void eliminarProducto(int id_producto, bool state)
        {
            var connection = BD.getConnection();
            string sql = "SELECT * FROM PRODUCTOS WHERE ID_PRODUCTO = '" + id_producto + "'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            if (Convert.ToInt32(dataSet.Tables[0].Rows[0]["ID_PRODUCTO"]) == id_producto)
            {
                string sSentenciaSql = "UPDATE PRODUCTOS ";
                sSentenciaSql = sSentenciaSql + "SET ESTADO = @ESTADO ";
                sSentenciaSql = sSentenciaSql + "WHERE ID_PRODUCTO = @ID_PRODUCTO";
                MySqlConnection conexion = BD.getConnection();
                MySqlCommand comando = new MySqlCommand(sSentenciaSql, conexion);

                comando.Parameters.AddWithValue("@ESTADO", state);
                comando.Parameters.AddWithValue("@ID_PRODUCTO", id_producto);

                comando.ExecuteNonQuery();
                BD.CloseConnection(conexion);
            }
            BD.CloseConnection(connection);
        }
        #endregion
    }
}
