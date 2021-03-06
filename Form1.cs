using System;
using System.Windows.Forms;

using static AccessSherstnev.AccessData;
using static AccessSherstnev.Globals;
using System.Collections.Generic;

namespace AccessSherstnev
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

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
            string query = "SELECT Актеры.[Код актера], Актеры.Управляющий FROM Актеры WHERE(((Актеры.Фамилия) = '" + Фамилия.Text + "') AND((Актеры.Имя) = '" + Имя.Text + "') AND((Актеры.Отчество) = '" + Отчество.Text + "'))";
            string[] dataName = new string[2]
            {
                "Код актера",
                "Управляющий",
            };
            DataAccessLight dataAccessLight = new DataAccessLight(query, dataName, connectionAdress, true, false);
            List<List<string>> data = dataAccessLight.getData();
            user_id = int.Parse(data[0][0]);
            string status = data[1][0];
            if (status == "False")
            {
                FormStaff fs = new FormStaff();
                fs.user_id = user_id.ToString();
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