using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для PageAddEditConfigurations.xaml
    /// </summary>
    public partial class PageAddEditConfigurations : Page
    {
        private Configuration _currentConf = new Configuration();
        public PageAddEditConfigurations(Configuration selectedConf)
        {
            InitializeComponent();
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US"); 
            if (selectedConf != null)
                _currentConf = selectedConf;

            DataContext = _currentConf;
            cbSteeringWheel.ItemsSource = bdAvtoLiderEntities.GetContext().TypesOfSteeringWheelsUpholstery.ToList();
            cbSeat.ItemsSource = bdAvtoLiderEntities.GetContext().TypesOfSeatUpholstery.ToList();
            cbCars.ItemsSource = bdAvtoLiderEntities.GetContext().Cars.ToList();
            cbRims.ItemsSource = bdAvtoLiderEntities.GetContext().TypesOfRims.ToList();
            cbCover.ItemsSource = bdAvtoLiderEntities.GetContext().TypesOfCover.ToList();
            cbTires.ItemsSource = bdAvtoLiderEntities.GetContext().TypesOfTires.ToList();
            cbClients.ItemsSource = bdAvtoLiderEntities.GetContext().Clients.ToList();
            cbUsers.ItemsSource = bdAvtoLiderEntities.GetContext().Users.Where(p => p.RoleId == 2).ToList();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var sqlAmountCars = $"UPDATE  Cars Set Amount = Amount-1 Where Id = {tbCarId.Text};";
            var sqlCountConfClients = $"UPDATE  Clients Set CountConfigurations = CountConfigurations+1 Where Id = {tbClientId.Text};"; 

            StringBuilder errors = new StringBuilder();

            if (cbUsers.Text.Length == 0)
                errors.AppendLine("Укажите менеджера");
            if (cbClients.Text.Length == 0)
                errors.AppendLine("Укажите клиента");
            if (cbCars.Text.Length == 0)
                errors.AppendLine("Укажите автомобиль");                
            if (cbRims.Text.Length == 0)
                errors.AppendLine("Укажите тип дисков автомобиля");
            if (cbCover.Text.Length == 0)
                errors.AppendLine("Укажите тип покрытия автомобиля");
            if (cbTires.Text.Length == 0)
                errors.AppendLine("Укажите тип шин автомобиля");
            if (cbSeat.Text.Length == 0)
                errors.AppendLine("Укажите тип обивки сидений");
            if (cbSteeringWheel.Text.Length == 0)
                errors.AppendLine("Укажите тип обивки руля");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentConf.Id == 0)
            {
                try
                {
                    if (tbCarAmount.Text=="0")
                    {
                        MessageBox.Show("Выбранного автомобиля нет в наличии", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else 
                    {
                        bdAvtoLiderEntities.GetContext().Configuration.Add(_currentConf);
                        var updateCars = bdAvtoLiderEntities.GetContext().Database.ExecuteSqlCommand(sqlAmountCars);
                        var updateClients = bdAvtoLiderEntities.GetContext().Database.ExecuteSqlCommand(sqlCountConfClients);
                        bdAvtoLiderEntities.GetContext().SaveChanges();
                        MessageBox.Show("Информация сохранена!");
                        ClassManager.BaseFrame.GoBack();
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }    
            else if (_currentConf.Id != 0)
            {
                try
                {
                    if (tbCarAmount.Text == "0")
                    {
                        MessageBox.Show("Выбранного автомобиля нет в наличии", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        bdAvtoLiderEntities.GetContext().SaveChanges();
                        MessageBox.Show("Информация сохранена!");
                        ClassManager.BaseFrame.GoBack();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
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

        private void allPrice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClassManager.configuration=="1")
            {
                btnCalculate.Visibility = Visibility.Collapsed;
                var n = Convert.ToDouble(tbCarPrice.Text) + Convert.ToDouble(tbCoverPrice.Text) + Convert.ToDouble(tbRimsPrice.Text) 
                    + Convert.ToDouble(tbTiresPrice.Text) + Convert.ToDouble(tbSeatPrice.Text) + Convert.ToDouble(tbStWheelsPrice.Text);
                tbAllPrice.Text = n.ToString() + " ₽";
            }
            else if (ClassManager.configuration == "0")
            {
                btnCalculate.Visibility = Visibility.Visible;
            }
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            var n = Convert.ToDouble(tbCarPrice.Text) + Convert.ToDouble(tbCoverPrice.Text) + Convert.ToDouble(tbRimsPrice.Text) 
                + Convert.ToDouble(tbTiresPrice.Text) + Convert.ToDouble(tbSeatPrice.Text) + Convert.ToDouble(tbStWheelsPrice.Text);
            tbAllPrice.Text = n.ToString() + " ₽";
        }
    }
}
