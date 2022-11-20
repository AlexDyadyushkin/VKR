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
using Microsoft.Win32;

namespace AvtoLider.Pages.PagesOfManager.PagesAddEdit
{
    /// <summary>
    /// Логика взаимодействия для PageAddEditSteeringWheels.xaml
    /// </summary>
    public partial class PageAddEditSteeringWheels : Page
    {
        private TypesOfSteeringWheelsUpholstery _currentSteeringWheels = new TypesOfSteeringWheelsUpholstery();
        public PageAddEditSteeringWheels(TypesOfSteeringWheelsUpholstery selectedSteeringWheels)
        {
            InitializeComponent();
            if (selectedSteeringWheels != null)
                _currentSteeringWheels = selectedSteeringWheels;

            DataContext = _currentSteeringWheels;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var sqlDateImage = $"UPDATE  TypesOfSteeringWheelsUpholstery Set Photo = (Select * From OpenRowSet(Bulk N'{tbPhoto.Text}', Single_Blob) As image) Where Id = {tbId.Text};";

            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentSteeringWheels.Title))
                errors.AppendLine("Укажите тип перетяжки руля");
            if (tbPhoto.Text.Length == 0)
            {
                errors.AppendLine("Выберите фото");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentSteeringWheels.Id == 0)
            {
                try
                {
                    bdAvtoLiderEntities.GetContext().TypesOfSteeringWheelsUpholstery.Add(_currentSteeringWheels);
                    bdAvtoLiderEntities.GetContext().SaveChanges();
                    var sqlAddImage = $"UPDATE  TypesOfSteeringWheelsUpholstery Set Photo = (Select * From OpenRowSet(Bulk N'{tbPhoto.Text}', Single_Blob) As image) Where Id = {_currentSteeringWheels.Id};";
                    var update = bdAvtoLiderEntities.GetContext().Database.ExecuteSqlCommand(sqlAddImage);
                    MessageBox.Show("Информация сохранена!");
                    ClassManager.BaseFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }                              
            }
            else if (_currentSteeringWheels.Id != 0)
            {
                if (tbPhoto.Text != "")
                {
                    if (tbPhoto.Text == "Картинка уже загружена")
                    {
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
                    else if (tbPhoto.Text != "Картинка уже загружена")
                    {
                        try
                        {
                            var update = bdAvtoLiderEntities.GetContext().Database.ExecuteSqlCommand(sqlDateImage);
                            bdAvtoLiderEntities.GetContext().SaveChanges();
                            MessageBox.Show("Информация сохранена!");
                            ClassManager.BaseFrame.GoBack();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }
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

        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Картинки Png  (*.png) |*.png; |Картинки Jpg (*.jpg) |*.jpg;";

            if (openFileDialog.ShowDialog() == true)
            {
                tbPhoto.Text = openFileDialog.FileName;
            }
        }    
    }
}
