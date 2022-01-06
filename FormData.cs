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

using static AccessSherstnev.Globals;

namespace AccessSherstnev
{
    public partial class FormData : Form
    {
        public FormData()
        {
            InitializeComponent();
        }

        void show(object sender, EventArgs e)
        {
            this.Show();
            var screen = Screen.FromControl(this);
            this.Top = screen.Bounds.Height / 2 - this.Height / 2;
            this.Left = screen.Bounds.Width / 2 - this.Width / 2;
        }

        private void buttonClick(object sender, EventArgs e)
        {
            FormDataExt formDataExt = new FormDataExt();
            string temp = ((sender as Button).Name).ToString();
            formDataExt.Text = temp.Substring(1);
            formDataExt.Show(this);
            formDataExt.FormClosed += new FormClosedEventHandler(show);
            this.Hide();
        }

        void getData(Func<String, String, String, Boolean> delegat, bool filter)
        {
            OleDbConnection dbConnection = new OleDbConnection(connectionAdress);
            dbConnection.Open();
            string query = "SELECT [Репертуар театра].[Название постановки], [Репертуар театра].Код FROM[Репертуар театра]";
            if (filter)
            {
                query = "SELECT DISTINCT [Репертуар театра].[Название постановки], [Репертуар театра].Код FROM[Репертуар театра] INNER JOIN Спектакли ON[Репертуар театра].Код = Спектакли.[Код постановки] WHERE(((Спектакли.[Дата спектакля]) Between #" + DateTime.Parse(ДатаС.Text).ToString("dd-MM-yyyy") + "# And #" + DateTime.Parse(ДатаПо.Text).ToString("dd-MM-yyyy") + "#)); ";
            }
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            OleDbDataReader dbReader = dbCommand.ExecuteReader();
            if (dbReader.HasRows == false)
                MessageBox.Show("Данные не найдены!", "Ошибка!");
            else
            {
                while (dbReader.Read())
                {
                    delegat.Invoke(dbReader["Код"].ToString(), dbReader["Название постановки"].ToString(), "C:\\Users\\shers\\Desktop\\2659534639.jpg");
                }
            }
            dbReader.Close();
            dbConnection.Close();
        }

        bool releaseTable(string number, string name, string imgWay = "C:\\Users\\shers\\Desktop\\2659534639.jpg")
        {
            Label label = new Label();
            label.Text = name;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Anchor = AnchorStyles.None;
            label.Dock = DockStyle.Fill;
            Font fn = new Font("Microsoft Sans Serif", 10);
            label.Font = fn;

            Button button = new Button();
            button.Name = "b" + number;
            button.Text = "Подробнее";
            button.Width = 120;
            button.Height = 30;
            button.Click += buttonClick;
            button.Anchor = AnchorStyles.None;

            PictureBox pictureBox = new PictureBox();
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Dock = DockStyle.Fill;
            try
            {
                pictureBox.Image = Image.FromFile(imgWay);
            }
            catch (Exception ex)
            {
                
            }

            TableLayoutPanel table = new TableLayoutPanel();
            table.Dock = DockStyle.Fill;
            table.RowStyles.Add(new RowStyle());
            table.RowStyles.Add(new RowStyle());
            table.RowStyles.Add(new RowStyle());
            table.ColumnCount = 1;
            table.RowStyles[0].SizeType = SizeType.AutoSize;
            table.RowStyles[1].SizeType = SizeType.Percent;
            table.RowStyles[1].Height = 100;
            table.RowStyles[2].SizeType = SizeType.AutoSize;

            table.Controls.Add(label);
            table.Controls.Add(pictureBox);
            table.Controls.Add(button);
            if (tableLayoutPanel4.Controls.Count % 3 == 0)
            {
                tableLayoutPanel4.RowStyles[tableLayoutPanel4.RowStyles.Count - 1].SizeType = SizeType.Absolute;
                tableLayoutPanel4.RowStyles[tableLayoutPanel4.RowStyles.Count - 1].Height = 270;
            }
            if (tableLayoutPanel4.Controls.Count % 3 != 0)
            {
                tableLayoutPanel4.RowStyles[tableLayoutPanel4.RowStyles.Count - 1].SizeType = SizeType.Absolute;
                tableLayoutPanel4.RowStyles[tableLayoutPanel4.RowStyles.Count - 1].Height = 270;
            }
            else
            {
                tableLayoutPanel4.RowStyles.Add(new RowStyle());
                tableLayoutPanel4.RowStyles[tableLayoutPanel4.RowStyles.Count - 1].SizeType = SizeType.Absolute;
                tableLayoutPanel4.RowStyles[tableLayoutPanel4.RowStyles.Count - 1].Height = 270;
            }
            tableLayoutPanel4.Controls.Add(table);
/*            tableLayoutPanel4.Controls.Count;*/


            
            return true;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void FormData_Load(object sender, EventArgs e)
        {
            var screen = Screen.FromControl(this);
            this.Top = screen.Bounds.Height / 2 - this.Height / 2;
            this.Left = screen.Bounds.Width / 2 - this.Width / 2;
            getData(releaseTable, false);
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 f1 = (Form1)this.Owner;
            f1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tableLayoutPanel4.Controls.Clear();
            getData(releaseTable, true);
        }
    }
}
