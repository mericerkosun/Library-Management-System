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

namespace LibraryManagementSystem
{
    public partial class FormLogin : Form
    {
        FormBooks formBooks;

        public FormLogin()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source = .\SQLEXPRESS; Initial Catalog = DbYTALibrary; Integrated Security = True");

        private void buttonLogin_Click(object sender, EventArgs e)
        {

            string password = "";

            try
            {
                baglanti.Open();
                SqlCommand sqlKomut = new SqlCommand("SELECT Password FROM TableAdminLogin WHERE UserName = @p1", baglanti);
                sqlKomut.Parameters.AddWithValue("@p1", textBoxUserName.Text);
                SqlDataReader sqlDataReader = sqlKomut.ExecuteReader();

                while(sqlDataReader.Read())
                {
                    password = sqlDataReader[0].ToString();
                }

                if(password==textBoxPassword.Text)
                {
                    formBooks = new FormBooks();
                    this.Hide();
                    formBooks.Show();
                }
                else
                {
                    MessageBox.Show("Wrong user name or password!");
                    textBoxUserName.Text = "";
                    textBoxPassword.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Error! " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }
    }
}
