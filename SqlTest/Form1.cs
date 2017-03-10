using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using MySql.Data.MySqlClient;

namespace SqlTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var customer = this.GetAllCustomers();
            //var c1 = this.GetCustomerByI3D(10000);
            int i3D = this.CreateNewVertriebsGebietWithI3D("Apfel", "Birne");

        }

        private IDbConnection GetConnection()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "SERVER";
            builder.InitialCatalog = "DATENBANK";
            builder.UserID = "USERNAME";
            builder.Password = "PASSWORT";

            return new SqlConnection(builder.ConnectionString);

            //var builder = new MySqlConnectionStringBuilder();
            //builder.Database = "asdblf";
            //builder.UserID = "asdf";
            //builder.Password = "asdf";
            //builder.Server = "asdf";

            //return new MySqlConnection(builder.ConnectionString);
        }

        private bool CreateNewVertriebsGebiet(string shortName, string longName)
        {
            using (var connection = this.GetConnection())
            {
                connection.Open();

                int rowsAffected = connection.Execute("INSERT INTO Vertriebsgebiete (Kurztext, Langtext) VALUES (@Short, @Long)", new {Short = shortName, Long = longName});

                return rowsAffected > 0;
            }
        }

        private int CreateNewVertriebsGebietWithI3D(string shortName, string longName)
        {
            using (var connection = this.GetConnection())
            {
                connection.Open();
               
                int i3D = connection.ExecuteScalar<int>("INSERT INTO Vertriebsgebiete (Kurztext, Langtext) VALUES (@Short, @Long); SELECT SCOPE_IDENTITY()", new { Short = shortName, Long = longName });
                return i3D;

            }
        }

        private Customer GetCustomerByI3D(int i3D)
        {
            using (IDbConnection connection = this.GetConnection())
            {
                connection.Open();

                return connection.QueryFirst<Customer>("SELECT I3D, Name FROM dbo.Kunden WHERE I3D = @I3D", new { I3D = i3D });
            }
        }

        private List<Customer> GetAllCustomers()
        {
            using (IDbConnection connection = this.GetConnection())
            {
                connection.Open();
                
                return connection.Query<Customer>("SELECT I3D, Name FROM dbo.Kunden").ToList();


                //using (var command = connection.CreateCommand())
                //{
                //    command.CommandText = "SELECT I3D, Name FROM dbo.Kunden";

                //    var result = new List<Customer>();

                //    using (var reader = command.ExecuteReader())
                //    {
                //        while (reader.Read())
                //        {
                //            int i3D = reader.GetInt32(0);
                //            string name = reader.GetString(1);

                //            result.Add(new Customer
                //            {
                //                I3D = i3D,
                //                Name = name
                //            });
                //        }
                //    }

                //    return result;
                //}
            }
        }
    }

    public class Customer
    {
        public int I3D { get; set; }
        public string Name { get; set; }
    }
}
