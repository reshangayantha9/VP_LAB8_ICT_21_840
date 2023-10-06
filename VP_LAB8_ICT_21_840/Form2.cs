using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace VP_LAB8_ICT_21_840
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-R85B60T\\SQLEXPRESS;Initial Catalog=Lab8;Integrated Security=True");
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;

        private void loadData()
        {
            try
            {
                cmd = new SqlCommand("SELECT * FROM [STUDENT]", conn);
                sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                dt = new DataTable();
                dt.Clear();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception e)
            {
                MessageBox.Show("Data Fetching Error !!!");
                Console.WriteLine(e.Message);
            }
        }
        private void clear()
        {
            txtId.Text = "";
            txtName.Text = "";
            txtGrade.Text = "";
            txtSearch.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            loadData();
        }

        private void parameters()
        {
            cmd.Parameters.AddWithValue("ID", txtId.Text);
            cmd.Parameters.AddWithValue("Name", txtName.Text);
            cmd.Parameters.AddWithValue("dob", dateTimePicker1.Value.Date.ToString());
            cmd.Parameters.AddWithValue("Grade", txtGrade.Text);
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow selectedRow = dataGridView1.Rows[index];
            txtId.Text = selectedRow.Cells[0].Value.ToString();
            txtName.Text = selectedRow.Cells[1].Value.ToString();
            if (DateTime.TryParse(selectedRow.Cells[2].Value.ToString(), out DateTime selectedDate))
            {
                dateTimePicker1.Value = selectedDate;
            }
            txtGrade.Text = selectedRow.Cells[3].Value.ToString();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtId.Text) || String.IsNullOrEmpty(txtName.Text) || String.IsNullOrEmpty(txtGrade.Text))
            {
                MessageBox.Show("Complete the all field");
            }
            else
            {
                try
                {
                    cmd = new SqlCommand("INSERT INTO [STUDENT] values (@ID,@Name,@dob,@Grade)", conn);
                    parameters();
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("New Student added");
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Student insert failed !!!");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                cmd = new SqlCommand("SELECT * FROM [STUDENT] WHERE ID LIKE @search +'%' OR Name Like @search + '%'", conn);
                cmd.Parameters.AddWithValue("search", txtSearch.Text);
                sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                dt = new DataTable();
                dt.Clear();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Data not Retrieving");
                Console.WriteLine(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtId.Text) || String.IsNullOrEmpty(txtName.Text) || String.IsNullOrEmpty(txtGrade.Text))
            {
                MessageBox.Show("Complete the all field");
            }
            else
            {
                try
                {
                    cmd = new SqlCommand("UPDATE [STUDENT] SET Name=@Name,dob=@dob,Grade=@Grade WHERE ID=@ID", conn);
                    parameters();
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Updated Student");
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Student update failed !!!");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                cmd = new SqlCommand("DELETE FROM [STUDENT] WHERE ID=@ID", conn);
                parameters();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Deleted Student");
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Student delete failed !!!");
                Console.WriteLine(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }
    }
}
