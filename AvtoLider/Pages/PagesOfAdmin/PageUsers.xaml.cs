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
using Word = Microsoft.Office.Interop.Word;

namespace AvtoLider.Pages.PagesOfAdmin
{
    /// <summary>
    /// Логика взаимодействия для PageAdmin.xaml
    /// </summary>
    public partial class PageUsers : Page
    {
        public PageUsers()
        {
            InitializeComponent();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            ClassManager.BaseFrame.Navigate(new PageAddEditUsers((sender as Button).DataContext as Users));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ClassManager.BaseFrame.Navigate(new PageAddEditUsers(null));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var usersForRemoving = dGridUsers.SelectedItems.Cast<Users>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {usersForRemoving.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    bdAvtoLiderEntities.GetContext().Users.RemoveRange(usersForRemoving);
                    bdAvtoLiderEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    dGridUsers.ItemsSource = bdAvtoLiderEntities.GetContext().Users.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                bdAvtoLiderEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dGridUsers.ItemsSource = bdAvtoLiderEntities.GetContext().Users.ToList();
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            dGridUsers.ItemsSource = bdAvtoLiderEntities.GetContext().Users.ToList().Where(p => p.Name.ToLower().Contains(tbSearch.Text.ToLower()) || p.Login.ToLower().Contains(tbSearch.Text.ToLower()) || p.Login.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
        }
    }
}
