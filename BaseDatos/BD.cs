using MySql.Data.MySqlClient;

namespace Proyecto_API.BaseDatos
{
    public static class BD
    {
        public static string sDbName = "bpmvl8zkxynxr7igf60r";
        public static string sServerName = "bpmvl8zkxynxr7igf60r-mysql.services.clever-cloud.com";
        public static string sUser = "umt00bw765w5fti3";
        public static string sPassword = "CzLt50Y6Ajtsuuo4FfbG";

        public static string sStringConnection = "server=" + sServerName + ";uid=" + sUser + ";pwd=" + sPassword + ";database=" + sDbName + "";


        public static MySqlConnection getConnection()
        {
            MySqlConnection conn = new MySqlConnection(sStringConnection);
            conn.Open();
            return conn;
        }

        public static void CloseConnection(MySqlConnection connection)
        {
            connection.Close();
        }
    }
}
