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
    public partial class FormDataExt : Form
    {
        public FormDataExt()
        {
            InitializeComponent();
        }

        string filterData;

        void getData(string filterData, string typeData)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + AppDomain.CurrentDomain.BaseDirectory + "../" + "../" + "../" + "../" + "Sherstnev_1.accdb'";//строка оеденения
            OleDbConnection dbConnection = new OleDbConnection(connectionString);
            dbConnection.Open();
            string query;
            switch (typeData)
            {
                case "actor":
                    query = "SELECT Актеры.Фамилия, Актеры.Имя, Актеры.Отчетсво, [Роли в спектаклях].[Название роли] FROM Актеры INNER JOIN(Контракты INNER JOIN ([Репертуар театра] INNER JOIN [Роли в спектаклях] ON[Репертуар театра].Код = [Роли в спектаклях].[Код постановки]) ON Контракты.[Код контракта] = [Роли в спектаклях].[Код контракта]) ON Актеры.[Код актера] = Контракты.[Код актера] WHERE((([Репертуар театра].Код) = " + filterData + "));";
                    break;
                case "info":
                    query = "SELECT [Репертуар театра].[Описание постановки], [Репертуар театра].[Название постановки] FROM[Репертуар театра] WHERE([Репертуар театра].Код = " + filterData + ");";
                    break;
                default:
                    query = "SELECT Спектакли.[Дата спектакля] FROM[Репертуар театра] INNER JOIN Спектакли ON[Репертуар театра].Код = Спектакли.[Код постановки] WHERE((([Репертуар театра].Код) = " + filterData + "))";
                    break;
            }
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            OleDbDataReader dbReader = dbCommand.ExecuteReader();
            if (dbReader.HasRows == false)
            {
                /*                MessageBox.Show("Данные не найдены!", "Ошибка!");*/

            }
            else
            {
                switch (typeData)
                {
                    case "actor":
                        while (dbReader.Read())
                        {
                            Роли.Items.Add(dbReader["Фамилия"] + " " + dbReader["Имя"] + " " + dbReader["Отчетсво"] + " в роли '" + dbReader["Название роли"] + "'");
                        }
                        break;
                    case "info":
                        while (dbReader.Read())
                        {
                            Название.Text = dbReader["Название постановки"].ToString();
                            Описание.Text += dbReader["Описание постановки"];
                        }
                        break;
                    default:
                        while (dbReader.Read())
                        {
                            Сеансы.Items.Add(dbReader["Дата спектакля"]);
                        }
                        break;
                }
            }
            dbReader.Close();
            dbConnection.Close();
        }

        private void FormDataExt_Load(object sender, EventArgs e)
        {
            filterData = this.Text;
            getData(filterData, "actor");
            getData(filterData, "info");
            getData(filterData, "dates");
            this.Text = "Мой театр";
            var screen = Screen.FromControl(this);
            this.Top = screen.Bounds.Height / 2 - this.Height / 2;
            this.Left = screen.Bounds.Width / 2 - this.Width / 2;
        }
    }
}
