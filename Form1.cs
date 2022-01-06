using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace AccessSherstnev
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string user_name = "unknown";
        int user_id = 0;

        void show(object sender, EventArgs e)
        {
            this.Show();
            var screen = Screen.FromControl(this);
            this.Top = screen.Bounds.Height / 2 - this.Height / 2;
            this.Left = screen.Bounds.Width / 2 - this.Width / 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + AppDomain.CurrentDomain.BaseDirectory + "../" + "../" + "../" + "../" + "Sherstnev_1.accdb'";//строка оеденения
            OleDbConnection dbConnection = new OleDbConnection(connectionString);
            dbConnection.Open();
            string query = "SELECT Актеры.[Код актера], Актеры.Управляющий FROM Актеры WHERE(((Актеры.Фамилия) = '" + Фамилия.Text +  "') AND((Актеры.Имя) = '" + Имя.Text + "') AND((Актеры.Отчество) = '" + Отчество.Text + "'))";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            OleDbDataReader dbReader = dbCommand.ExecuteReader();
            if (dbReader.HasRows == false)
                MessageBox.Show("Проверьте правильность введенных данных!", "Ошибка!");
            else
            {
                string status = "";
                while (dbReader.Read())
                {
                    user_name = Фамилия.Text + " " + Имя.Text + " " + Отчество.Text;
                    user_id = (int)dbReader["Код актера"];
                    status = dbReader["Управляющий"].ToString();
                }
                if (status == "False")
                {
                    FormStaff fs = new FormStaff();
                    fs.Text = user_id.ToString();
                    fs.FormClosed += new FormClosedEventHandler(show);
                    fs.Show();
                    this.Hide();
                }
                else
                {
                    FormAdmin fa = new FormAdmin();
                    fa.FormClosed += new FormClosedEventHandler(show);
                    fa.Show();
                    this.Hide();
                }


            }
            dbReader.Close();
            dbConnection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            auth(false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormData FD = new FormData();
            FD.Show(this);
            FD.FormClosed += new FormClosedEventHandler(show);
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            auth(true);
        }


        void auth(bool t)
        {
            if(t == true)
            {
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;

                Фамилия.Visible = true;
                Имя.Visible = true;
                Отчество.Visible = true;

                button1.Visible = true;
                button2.Visible = true;

                label4.Visible = false;

                button3.Visible = false;
                button4.Visible = false;
            }
            else
            {
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;

                Фамилия.Visible = false;
                Имя.Visible = false;
                Отчество.Visible = false;

                button1.Visible = false;
                button2.Visible = false;

                label4.Visible = true;

                button3.Visible = true;
                button4.Visible = true;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var screen = Screen.FromControl(this);
            this.Top = screen.Bounds.Height / 2 - this.Height / 2;
            this.Left = screen.Bounds.Width / 2 - this.Width / 2;
        }
    }
}
