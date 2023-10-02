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
    public partial class FormBooks : Form
    {

        SqlConnection baglanti = new SqlConnection(@"Data Source = .\SQLEXPRESS; Initial Catalog = DbYTALibrary; Integrated Security = True");

        public FormBooks()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxBookName_TextChanged(object sender, EventArgs e)
        {

        }

        private void showData()
        {
            try
            {
                string q = "SELECT * FROM TableBooks";
                SqlDataAdapter da = new SqlDataAdapter(q, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dataGridViewBooks.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void FormBooks_Load(object sender, EventArgs e)
        {
            showData();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {

            try
            {
                baglanti.Open();

                SqlCommand sqlCommand = new SqlCommand("INSERT INTO TableBooks (BookName, AuthorName, AuthorSurname, ISBN, Status, BookGenreCode) VALUES (@P1, @P2, @P3, @P4, @P5, @P6)", baglanti);
                sqlCommand.Parameters.AddWithValue("@P1", textBoxBookName.Text);
                sqlCommand.Parameters.AddWithValue("@P2", textBoxAuName.Text);
                sqlCommand.Parameters.AddWithValue("@P3", textBoxAuSurname.Text);
                sqlCommand.Parameters.AddWithValue("@P4", textBoxISBN.Text);
                sqlCommand.Parameters.AddWithValue("@P5", "True");
                sqlCommand.Parameters.AddWithValue("@P6", textBoxGenreCode.Text);

                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding a book! " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }

            showData();
            
        }

        private void dataGridViewBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            labelDelayFee.Text = "0";

            int selectedRow = dataGridViewBooks.SelectedCells[0].RowIndex;

            labelID.Text = dataGridViewBooks.Rows[selectedRow].Cells[0].Value.ToString();
            textBoxBookName.Text = dataGridViewBooks.Rows[selectedRow].Cells[1].Value.ToString();
            textBoxAuName.Text = dataGridViewBooks.Rows[selectedRow].Cells[2].Value.ToString();
            textBoxAuSurname.Text = dataGridViewBooks.Rows[selectedRow].Cells[3].Value.ToString();
            textBoxISBN.Text = dataGridViewBooks.Rows[selectedRow].Cells[4].Value.ToString();
            textBoxGenreCode.Text = dataGridViewBooks.Rows[selectedRow].Cells[8].Value.ToString();

            textBoxBorrower.Text = dataGridViewBooks.Rows[selectedRow].Cells[6].Value.ToString();

            if (dataGridViewBooks.Rows[selectedRow].Cells[7].Value != DBNull.Value)
            {
                dateTimePickerBorrowDate.Value = (DateTime)dataGridViewBooks.Rows[selectedRow].Cells[7].Value;
            }

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {

            try
            {
                baglanti.Open();

                SqlCommand sqlCommand = new SqlCommand("UPDATE TableBooks SET BookName = @P1, AuthorName = @P2, AuthorSurname = @P3, ISBN = @P4, BookGenreCode = @P5 WHERE ID = @P6", baglanti);

                sqlCommand.Parameters.AddWithValue("@P1", textBoxBookName.Text);
                sqlCommand.Parameters.AddWithValue("@P2", textBoxAuName.Text);
                sqlCommand.Parameters.AddWithValue("@P3", textBoxAuSurname.Text);
                sqlCommand.Parameters.AddWithValue("@P4", textBoxISBN.Text);
                sqlCommand.Parameters.AddWithValue("@P5", textBoxGenreCode.Text);
                sqlCommand.Parameters.AddWithValue("@P6", labelID.Text);

                sqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating a book! " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }

            showData();

        }

        private void buttonBorrowBook_Click(object sender, EventArgs e)
        {
            if (labelID.Text != "-")
            {
                try
                {
                    baglanti.Open();
                    SqlCommand sqlCommand = new SqlCommand("UPDATE TableBooks SET Borrower = @P1, BorrowingDate = @P2, Status = @P3 WHERE ID = @P4", baglanti);
                    sqlCommand.Parameters.AddWithValue("@P1", textBoxBorrower.Text);
                    sqlCommand.Parameters.Add("@P2", SqlDbType.Date).Value = dateTimePickerBorrowDate.Value.Date;
                    sqlCommand.Parameters.AddWithValue("@P3", "False");
                    sqlCommand.Parameters.AddWithValue("@P4", labelID.Text);
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while returning a book! " + ex.Message);
                }
                finally
                {
                    baglanti.Close();
                }

                showData();
            }

            else
            {
                MessageBox.Show("Please select a book from the list.");
            }
        }

        private void buttonCalculatePrice_Click(object sender, EventArgs e)
        {
            if(labelID.Text != "-")
            {
                DateTime dateToday = DateTime.Now;
                int dayDifference = (int)(dateToday - dateTimePickerBorrowDate.Value.Date).TotalDays;

                if(dayDifference > 10) 
                {
                    double delayFee = (dayDifference - 10) * 0.1;
                    labelDelayFee.Text = delayFee.ToString();


                }
            }
        }

        private void buttonReturnBook_Click(object sender, EventArgs e)
        {
            if (labelID.Text != "-")
            {
                try
                {
                    baglanti.Open();
                    SqlCommand sqlCommand = new SqlCommand("UPDATE TableBooks SET Borrower = @P1, BorrowingDate = @P2, Status = @P3 WHERE ID = @P4", baglanti);
                    
                    
                    sqlCommand.Parameters.AddWithValue("@P1", "");
                    sqlCommand.Parameters.Add("@P2", SqlDbType.Date).Value = DBNull.Value;
                    sqlCommand.Parameters.AddWithValue("@P3", "True");
                    sqlCommand.Parameters.AddWithValue("@P4", labelID.Text);
                    sqlCommand.ExecuteNonQuery();

                    textBoxBorrower.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while returning a book! " + ex.Message);
                }
                finally
                {
                    baglanti.Close();
                }

                showData();
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            showSearchResults();
        }

        private void showSearchResults()
        {
            try
            {
                string q = "SELECT* FROM TableBooks WHERE BookName LIKE '" + textBoxBookName.Text
                                                                             + "%' AND AuthorName LIKE '" + textBoxAuName.Text + "%' "
                                                                             + " AND AuthorSurname LIKE '" + textBoxAuSurname.Text + "%' "
                                                                             + " AND ISBN LIKE '" + textBoxISBN.Text + "%' "
                                                                             + " AND BookGenreCode LIKE '" + textBoxGenreCode.Text + "%' "
                                                                             + " AND Borrower LIKE '" + textBoxBorrower.Text + "%' ";

                SqlDataAdapter da = new SqlDataAdapter(q, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dataGridViewBooks.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            clearTextBox();
        }

        private void clearTextBox()
        {
            labelID.Text = "-";
            textBoxBookName.Text = "";
            textBoxAuName.Text = "";
            textBoxAuSurname.Text = "";
            textBoxISBN.Text = "";
            textBoxGenreCode.Text = "";
            textBoxBorrower.Text = "";
        }

        private void buttonShowAll_Click(object sender, EventArgs e)
        {
            showData();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {

            if (labelID.Text == "-" || labelID.Text == "")
            {
                MessageBox.Show("Please select the book that you want to delete from the list!");
            }
            else
            {
                try
                {
                    baglanti.Open();

                    SqlCommand sqlCommand = new SqlCommand("DELETE FROM TableBooks WHERE ID = @P1", baglanti);
                    sqlCommand.Parameters.AddWithValue("@P1", labelID.Text);


                    sqlCommand.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while deleting a book! " + ex.Message);
                }
                finally
                {
                    baglanti.Close();
                }

                showData();
                clearTextBox();
            }
        }

        private void FormBooks_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
