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
namespace VP_LAB8_ICT_21_840
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }

        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-R85B60T\\SQLEXPRESS;Initial Catalog=Lab8;Integrated Security=True");
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;

        string gender;
        private void showDateTime() {
            timer1.Start();
            lblDate.Text=DateTime.Now.ToLongDateString();
            lblTime.Text = DateTime.Now.ToLongTimeString();
        }
        private void loadData()
        {
            try
            {
                cmd = new SqlCommand("SELECT * FROM [EMPLOYEE]", conn);
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
            txtAge.Text = "";
            txtContact.Text = "";
            txtSearch.Text = "";
            txtCity.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            gender = "";
            dateTimePicker1.Value = DateTime.Now;
            loadData(); 
        }
        
        private void parameters()
        {
            cmd.Parameters.AddWithValue("empID", txtId.Text);
            cmd.Parameters.AddWithValue("Name", txtName.Text);
            cmd.Parameters.AddWithValue("City", txtCity.Text);
            cmd.Parameters.AddWithValue("Age", txtAge.Text);
            if (radioButton1.Checked == true)
            {
                gender = "male";
            }
            else if(radioButton2.Checked==true)
            {
                gender = "female";
            }
            cmd.Parameters.AddWithValue("Gender", gender);
            cmd.Parameters.AddWithValue("JoinDate", dateTimePicker1.Value.Date.ToString());
            cmd.Parameters.AddWithValue("Contact", txtContact.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            lblTime.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            showDateTime();
            loadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow selectedRow= dataGridView1.Rows[index];
            txtId.Text = selectedRow.Cells[0].Value.ToString();
            txtName.Text= selectedRow.Cells[1].Value.ToString();
            txtCity.Text= selectedRow.Cells[2].Value.ToString();
            txtAge.Text= selectedRow.Cells[3].Value.ToString();
            gender = selectedRow.Cells[4].Value.ToString();
            if (gender.Trim().Equals("male", StringComparison.OrdinalIgnoreCase))
            {
                radioButton1.Checked = true;
            }
            else if(gender.Trim().Equals("female", StringComparison.OrdinalIgnoreCase))
            {
                radioButton2.Checked = true;
            }
            if (DateTime.TryParse(selectedRow.Cells[5].Value.ToString(), out DateTime selectedDate))
            {
                dateTimePicker1.Value = selectedDate;
            }
            txtContact.Text= selectedRow.Cells[6].Value.ToString();

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtId.Text) || String.IsNullOrEmpty(txtName.Text)||String.IsNullOrEmpty(txtCity.Text)
                ||String.IsNullOrEmpty(txtAge.Text)||String.IsNullOrEmpty(txtContact.Text))
            {
                MessageBox.Show("Complete the all field");
            }
            else
            {
                try
                {
                    cmd = new SqlCommand("INSERT INTO [EMPLOYEE] values (@empID,@Name,@City,@Age,@Gender,@JoinDate,@Contact)",conn);
                    parameters();
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("New Employee added");
                    clear();
                }catch(Exception ex)
                {
                    MessageBox.Show("Employee insert failed !!!");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                cmd = new SqlCommand("SELECT * FROM [EMPLOYEE] WHERE empID LIKE @search +'%' OR Name Like @search + '%'",conn);
                cmd.Parameters.AddWithValue("search", txtSearch.Text);
                sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                dt = new DataTable();
                dt.Clear();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;

            }catch(Exception ex)
            {
                MessageBox.Show("Data not Retrieving");
                Console.WriteLine(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(txtId.Text) || String.IsNullOrEmpty(txtName.Text) || String.IsNullOrEmpty(txtCity.Text)
                || String.IsNullOrEmpty(txtAge.Text) || String.IsNullOrEmpty(txtContact.Text) )
            {
                MessageBox.Show("Complete the all field");
            }
            else
            {
                try
                {
                    cmd = new SqlCommand("UPDATE [EMPLOYEE] SET Name=@Name,City=@City,Age=@Age,Gender=@Gender,JoinDate=@JoinDate,Contact=@Contact WHERE empID=@empID", conn);
                    parameters();
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Updated Employee");
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Employee update failed !!!");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                cmd = new SqlCommand("DELETE FROM [EMPLOYEE] WHERE empID=@empID", conn);
                parameters();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Deleted Employee");
                clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Employee delete failed !!!");
                Console.WriteLine(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }
    }
}
