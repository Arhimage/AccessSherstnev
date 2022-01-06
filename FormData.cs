using System;
using System.Drawing;
using System.Windows.Forms;
using static AccessSherstnev.Globals;
using static AccessSherstnev.AccessData;
using System.Collections.Generic;

namespace AccessSherstnev
{
    public partial class FormData : Form
    {
        DataAccessLight dataAccessLight;

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
            formDataExt.filterData = temp.Substring(1);
            formDataExt.Show(this);
            formDataExt.FormClosed += new FormClosedEventHandler(show);
            this.Hide();
        }

        void getData(string query)
        {
            tableLayoutPanel4.Controls.Clear();
            tableLayoutPanel4.RowCount = 0;
            string[] dataName = new string[3]
            {
                "Код",
                "Название постановки",
                "Ссылка на картинку",
            };
            dataAccessLight = new DataAccessLight(query, dataName, connectionAdress, true, true);
            List<List<string>> data = dataAccessLight.getData();
            for (int i = 0; i < data[dataName.Length - 1].Count; i++)
            {
                if (data[2][i] == "" || data[2][i] == "Неизвестный объект")
                {
                    releaseTable(data[0][i], data[1][i]);
                }
                else
                {
                    releaseTable(data[0][i], data[1][i], data[2][i]);
                }
            }
        }

        bool releaseTable(string number, string name, string imgWay = "https://aspromed.ru/media/thumbs/product_images/fallback_image.jpg.180x180_q85.png")
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
                pictureBox.Load(imgWay);
            }
            catch (Exception)
            {
                pictureBox.Image = Image.FromFile(imgWay);
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
            return true;
        }

        private void FormData_Load(object sender, EventArgs e)
        {
            var screen = Screen.FromControl(this);
            this.Top = screen.Bounds.Height / 2 - this.Height / 2;
            this.Left = screen.Bounds.Width / 2 - this.Width / 2;

            string query = "SELECT [Репертуар театра].[Название постановки], [Репертуар театра].Код, [Репертуар театра].[Ссылка на картинку] FROM[Репертуар театра]";
            getData(query);
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 f1 = (Form1)this.Owner;
            f1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tableLayoutPanel4.Controls.Clear();

            string query = "SELECT DISTINCT [Репертуар театра].[Название постановки], [Репертуар театра].Код, [Репертуар театра].[Ссылка на картинку] FROM[Репертуар театра] INNER JOIN Спектакли ON[Репертуар театра].Код = Спектакли.[Код постановки] WHERE(((Спектакли.[Дата спектакля]) Between #" + DateTime.Parse(ДатаС.Text).ToString("dd-MM-yyyy") + "# And #" + DateTime.Parse(ДатаПо.Text).ToString("dd-MM-yyyy") + "#)); ";
            getData(query);
        }
    }
}
