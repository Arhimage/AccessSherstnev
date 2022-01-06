using System;
using System.Windows.Forms;

using static AccessSherstnev.AccessData;
using static AccessSherstnev.Globals;

namespace AccessSherstnev
{
    public partial class FormStaff : Form
    {
        public FormStaff()
        {
            InitializeComponent();
        }

        public string user_id;

        void getData()
        {
            string query = "SELECT Контракты.[Стоимость контракта], Контракты.[Начало контракта], Контракты.[Окончание контракта], Контракты.[Количество спектаклей], Контракты.[Премия по окончанию] FROM Актеры INNER JOIN Контракты ON Актеры.[Код актера] = Контракты.[Код актера] WHERE(((Актеры.[Код актера]) = " + user_id + "));";
            string[] dataName = new string[5]
            {
                "Стоимость контракта",
                "Начало контракта",
                "Окончание контракта",
                "Количество спектаклей",
                "Премия по окончанию",
            };
            DataAccessLight dataContracts = new DataAccessLight(query, dataName, connectionAdress, true, false);

            dataContracts.getDataGrid(ref dataGridView1);

            query = "SELECT Регалии.[Название регалии] FROM Регалии INNER JOIN(Актеры INNER JOIN[Регалии актеров] ON Актеры.[Код актера] = [Регалии актеров].[Код актера]) ON Регалии.Код = [Регалии актеров].[Код регалии] WHERE((Актеры.[Код актера]) = " + user_id + ")";
            dataName = new string[1]
            {
                "Название регалии",
            };
            DataAccessLight dataRegalies = new DataAccessLight(query, dataName, connectionAdress, true, false);

            dataRegalies.getListBox(ref listBox1);

            query = "SELECT Спектакли.[Дата спектакля] FROM Спектакли INNER JOIN(((Актеры INNER JOIN Контракты ON Актеры.[Код актера] = Контракты.[Код актера]) INNER JOIN[Роли в спектаклях] ON Контракты.[Код контракта] = [Роли в спектаклях].[Код контракта]) INNER JOIN[Список ролей] ON[Роли в спектаклях].Код = [Список ролей].[Код роли]) ON Спектакли.[Код спектакля] = [Список ролей].[Код спектакля] WHERE((Актеры.[Код актера]) = " + user_id + ")";
            dataName = new string[1]
            {
                "Дата спектакля",
            };
            DataAccessLight dataDates = new DataAccessLight(query, dataName, connectionAdress, true, false);

            dataDates.getListBox(ref listBox2);
        }

        private void FormStaff_Load(object sender, EventArgs e)
        {
            getData();

            var screen = Screen.FromControl(this);
            this.Top = screen.Bounds.Height / 2 - this.Height / 2;
            this.Left = screen.Bounds.Width / 2 - this.Width / 2;
        }
    }
}
