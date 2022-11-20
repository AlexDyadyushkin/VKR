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
    /// Логика взаимодействия для PageAddEditCover.xaml
    /// </summary>
    public partial class PageAddEditCover : Page
    {
        private TypesOfCover _currentCover = new TypesOfCover();
        public PageAddEditCover(TypesOfCover selectedCover)
        {
            InitializeComponent();
            if (selectedCover != null)
                _currentCover = selectedCover;

            DataContext = _currentCover;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var sqlDateImage = $"UPDATE  TypesOfCover Set Photo = (Select * From OpenRowSet(Bulk N'{tbPhoto.Text}', Single_Blob) As image) Where Id = {tbId.Text};";

            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentCover.Title))
                errors.AppendLine("Укажите название покрытия");
            if (tbPhoto.Text.Length == 0)
            {
                errors.AppendLine("Выберите фото");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentCover.Id == 0)
            {
                try
                {
                    bdAvtoLiderEntities.GetContext().TypesOfCover.Add(_currentCover);
                    bdAvtoLiderEntities.GetContext().SaveChanges();
                    var sqlAddImage = $"UPDATE  TypesOfCover Set Photo = (Select * From OpenRowSet(Bulk N'{tbPhoto.Text}', Single_Blob) As image) Where Id = {_currentCover.Id};";
                    var update = bdAvtoLiderEntities.GetContext().Database.ExecuteSqlCommand(sqlAddImage);
                    MessageBox.Show("Информация сохранена!");
                    ClassManager.BaseFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else if (_currentCover.Id != 0)
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
