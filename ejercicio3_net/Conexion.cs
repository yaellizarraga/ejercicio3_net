using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ejercicio3_net
{
    class Conexion
    {
        private string host = "localhost"; //Nombre o ip del servidor de MySQL
        private string database = "distribuidores"; //Nombre de la base de datos
        private string user = "root"; //Usuario de acceso a MySQL
        private string pass = "Francerrito"; //Contraseña de usuario de acceso a MySQL
        public MySqlConnection conn;

        public void conectar()
        {
   
            string connectionString = "SERVER=" + host + ";" + "DATABASE=" +database + ";" + "UID=" + user + ";" + "PASSWORD=" + pass + ";";
            conn = new MySqlConnection(connectionString);
            conn.Open();
        }

        public void desconectar()
        {
            conn.Close();
        }



    }
}
