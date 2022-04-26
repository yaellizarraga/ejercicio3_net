using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ejercicio3_net
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if(String.IsNullOrEmpty(txtNombres.Text))
                {
                    MessageBox.Show("Ingrese el nombre");
                    return;
                }

                if(String.IsNullOrEmpty(txtApePaterno.Text))
                {
                    MessageBox.Show("Ingrese un apellido paterno");
                    return;
                }
                var con = new Conexion();

                con.conectar();
                // Datos del formulario
                string name = txtNombres.Text;
                string a_paterno = txtApePaterno.Text;
                string a_materno = txtApeMaterno.Text;
                string calle = txtCalle.Text;
                int numero = int.Parse(txtNum.Text);
                string colonia = txtColonia.Text;
                DateTime fecha_registro = DateTime.Now;

                string insertPerson = "INSERT INTO persons (names, f_lastname, s_lastname) VALUES ('" + name + "', '" + a_paterno + "', '" + a_materno + "')";
                MySqlCommand cmd_persons = new MySqlCommand(insertPerson, con.conn);
                cmd_persons.ExecuteNonQuery();
                long idPerson = cmd_persons.LastInsertedId;

                var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var Charsarr = new char[8];
                var random = new Random();

                for (int i = 0; i < Charsarr.Length; i++)
                {
                    Charsarr[i] = characters[random.Next(characters.Length)];
                }

                var code = new String(Charsarr);

                string insertDistributor = "INSERT INTO distributors (id_persons, code, register_date) VALUES (" + idPerson + ", '" + code + "', '" + fecha_registro + "')";
                MySqlCommand cmd_distributors = new MySqlCommand(insertDistributor, con.conn);
                cmd_distributors.ExecuteNonQuery();

                string insertAddress = "INSERT INTO addresses (id_persons, street, h_number, neighborhood) VALUES (" + idPerson + ", '" + calle + "', " + numero + ", '" + colonia + "');";
                MySqlCommand cmd_address = new MySqlCommand(insertAddress, con.conn);
                cmd_address.ExecuteNonQuery();

                con.desconectar();

                txtNombres.Clear();
                txtApePaterno.Clear();
                txtApeMaterno.Clear();
                txtCalle.Clear();
                txtNum.Clear();
                txtColonia.Clear();

                MessageBox.Show("Datos insertados con exito");

            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(txtClave.Text))
            {
                MessageBox.Show("Ingrese la clave a consultar");
            }
            var con = new Conexion();
            con.conectar();
            string clave = txtClave.Text;
            string query = @"SELECT distributors.`code`,
            CONCAT(
                persons.`names`,
                ' ',
                persons.`f_lastname`
              ) AS nombre_completo,
              addresses.`street`,
              addresses.`h_number`,
              addresses.`neighborhood` 
            FROM
              persons
              INNER JOIN addresses
                ON addresses.`id_persons` = persons.`id` 
                INNER JOIN distributors ON distributors.`id_persons` = persons.`id`
            WHERE p_status = 1  AND code = '"+clave+"';";
            MySqlCommand cmd_select = new MySqlCommand(query, con.conn);
            MySqlDataReader selector = cmd_select.ExecuteReader();
            while (selector.Read())
            {
                txtNombres.Text = selector["nombre_completo"].ToString();
                txtCalle.Text = selector["street"].ToString();
                txtNum.Text = selector["h_number"].ToString();
                txtColonia.Text = selector["neighborhood"].ToString();
            }
        }
    }
}
