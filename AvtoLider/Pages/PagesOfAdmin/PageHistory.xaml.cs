using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AvtoLider.Pages;
using AvtoLider.Clases;
using AvtoLider.Models;

namespace AvtoLider.Pages.PagesOfAdmin
{
    /// <summary>
    /// Логика взаимодействия для PageHistory.xaml
    /// </summary>
    public partial class PageHistory : Page
    {
        public PageHistory()
        {
            InitializeComponent();
            dGridHistory.ItemsSource = bdAvtoLiderEntities.GetContext().LoginHistory.ToList();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {     
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите очистить историю?", "Информация", MessageBoxButton.YesNo, MessageBoxImage.Information);
                try
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        bdAvtoLiderEntities deleteHistory = new bdAvtoLiderEntities();
                        var sql = deleteHistory.Database.ExecuteSqlCommand("DELETE  FROM LoginHistory Where Id != 0");
                        bdAvtoLiderEntities.GetContext().SaveChanges();
                        dGridHistory.ItemsSource = bdAvtoLiderEntities.GetContext().LoginHistory.ToArray();
                        MessageBox.Show("История очищена!");
                    }                    
                }
                catch
                {
                    MessageBox.Show("Отсутствует связь с базой", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                }     
        }
    }
}
