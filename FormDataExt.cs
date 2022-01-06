using System;
using System.Windows.Forms;

using static AccessSherstnev.Globals;
using static AccessSherstnev.AccessData;

namespace AccessSherstnev
{
    public partial class FormDataExt : Form
    {

        DataAccessLight dataActers;
        DataAccessLight dataInfo;
        DataAccessLight dataDate;
        public FormDataExt()
        {
            InitializeComponent();
        }

        public string filterData;

        void getData()
        {
            string query = "SELECT Актеры.Фамилия, Актеры.Имя, Актеры.Отчество, [Роли в спектаклях].[Название роли] FROM Актеры INNER JOIN(Контракты INNER JOIN ([Репертуар театра] INNER JOIN [Роли в спектаклях] ON[Репертуар театра].Код = [Роли в спектаклях].[Код постановки]) ON Контракты.[Код контракта] = [Роли в спектаклях].[Код контракта]) ON Актеры.[Код актера] = Контракты.[Код актера] WHERE((([Репертуар театра].Код) = " + filterData + ")); ";
            string[] dataName = new string[4]
            {
                "Фамилия",
                "Имя",
                "Отчество",
                "Название роли",
            };
            dataActers = new DataAccessLight(query, dataName, connectionAdress, true, false);

            dataActers.getListBox(ref Роли);

            query = "SELECT [Репертуар театра].[Описание постановки], [Репертуар театра].[Название постановки] FROM[Репертуар театра] WHERE([Репертуар театра].Код = " + filterData + ");";
            dataName = new string[2]
            {
                "Описание постановки",
                "Название постановки",
            };
            dataInfo = new DataAccessLight(query, dataName, connectionAdress, true, false);

            Название.Text = dataInfo.getData()[1][0];
            Описание.Text += dataInfo.getData()[0][0];

            query = "SELECT Спектакли.[Дата спектакля] FROM[Репертуар театра] INNER JOIN Спектакли ON[Репертуар театра].Код = Спектакли.[Код постановки] WHERE((([Репертуар театра].Код) = " + filterData + "))";
            dataName = new string[1]
            {
                "Дата спектакля",
            };
            dataDate = new DataAccessLight(query, dataName, connectionAdress, true, false);

            dataDate.getListBox(ref Сеансы);
        }

        private void FormDataExt_Load(object sender, EventArgs e)
        {
            getData();

            var screen = Screen.FromControl(this);
            this.Top = screen.Bounds.Height / 2 - this.Height / 2;
            this.Left = screen.Bounds.Width / 2 - this.Width / 2;
        }
    }
}
