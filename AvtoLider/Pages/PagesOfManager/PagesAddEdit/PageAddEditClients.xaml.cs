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
using AvtoLider.Clases;
using AvtoLider.Models;

namespace AvtoLider.Pages.PagesOfManager.PagesAddEdit
{
    /// <summary>
    /// Логика взаимодействия для PageAddEditClients.xaml
    /// </summary>
    public partial class PageAddEditClients : Page
    {
        private Clients _currentClients = new Clients();
        public PageAddEditClients(Clients selectedClients)
        {
            InitializeComponent();
            if (selectedClients != null)
                _currentClients = selectedClients;

            DataContext = _currentClients;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            DateTime birth = DateTime.Parse(tbBirth.Text);

            _currentClients.Birthday = DateTime.Parse(birth.ToString("yyyy/MM/dd"));


            if (string.IsNullOrWhiteSpace(_currentClients.FirstName))
                errors.AppendLine("Укажите имя клиента");
            if (string.IsNullOrWhiteSpace(_currentClients.LastName))
                errors.AppendLine("Укажите фамилию клиента");
            if (string.IsNullOrWhiteSpace(_currentClients.Email))
                errors.AppendLine("Укажите электронную почту клиента");
            if (tbBirth.Text.Length == 0)
                errors.AppendLine("Укажите дату рождения клиента");
            if (!tbPhone.IsMaskCompleted)
                errors.AppendLine("Укажите номер телефона");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentClients.Id == 0)
                bdAvtoLiderEntities.GetContext().Clients.Add(_currentClients);

            try
            {
                bdAvtoLiderEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                ClassManager.BaseFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы точно хотите отменить изменения?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                ClassManager.BaseFrame.GoBack();
            }
            else
            {
                return;
            }
        }

        private void calBirth_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            tbBirth.Text = calBirth.SelectedDate.Value.ToString("dd/MM/yyyy");
            grCal.Visibility = Visibility.Collapsed;
        }

        private void btnCalVisible_Click(object sender, RoutedEventArgs e)
        {
            btnCalVisible.Click += btnCalHidden_Click;
            grCal.Visibility = Visibility.Visible;
            btnCalVisible.Click -= btnCalVisible_Click;

        }

        private void btnCalHidden_Click(object sender, RoutedEventArgs e)
        {
            grCal.Visibility = Visibility.Collapsed;
            btnCalVisible.Click -= btnCalHidden_Click;
            btnCalVisible.Click += btnCalVisible_Click;
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ClassManager.countConf == "0")
            {
                tBlockCountConf.Visibility = Visibility.Visible;
                tbCountConf.Visibility = Visibility.Visible;
                tbCountConf.Text = "0";
            }
            else if (ClassManager.countConf == "1")
            {
                tBlockCountConf.Visibility = Visibility.Collapsed;
                tbCountConf.Visibility = Visibility.Collapsed;
            }
        }
    }
}
