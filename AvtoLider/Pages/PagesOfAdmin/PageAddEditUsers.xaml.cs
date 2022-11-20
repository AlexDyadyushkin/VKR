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
using AvtoLider.Models;
using AvtoLider.Clases;

namespace AvtoLider.Pages.PagesOfAdmin
{
    /// <summary>
    /// Логика взаимодействия для PageAddEditUsers.xaml
    /// </summary>
    public partial class PageAddEditUsers : Page
    {
        private Users _currentUser = new Users();
        public PageAddEditUsers(Users selectedUsers)
        {
            InitializeComponent();

            if (selectedUsers != null)
                _currentUser = selectedUsers;

            DataContext = _currentUser;
            cbRoles.ItemsSource = bdAvtoLiderEntities.GetContext().UserRoles.ToList();
        }

        public string a { get; set; }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
                StringBuilder errors = new StringBuilder();
                if (string.IsNullOrWhiteSpace(_currentUser.Login))
                {
                    errors.AppendLine("Укажите логин");
                }
                if (string.IsNullOrWhiteSpace(_currentUser.Password))
                {
                    errors.AppendLine("Укажите пароль");
                }
                if (string.IsNullOrWhiteSpace(_currentUser.Name))
                {
                    errors.AppendLine("Укажите имя и фамилию");
                }
                if (_currentUser.UserRoles == null)
                {
                    errors.AppendLine("Выберите роль");
                }
                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (_currentUser.Id == 0)
                {
                    try
                    {
                        string loginChek = "SELECT * FROM Users WHERE Login ='" + _currentUser.Login + "'";
                        var sql = bdAvtoLiderEntities.GetContext().Users.SqlQuery(loginChek).ToArray();                                 
                        if (sql.Length != 0)
                        {
                            MessageBox.Show("Пользователь с данным логином уже существует", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (sql.Length == 0)
                        {

                            bdAvtoLiderEntities.GetContext().Users.Add(_currentUser);
                            bdAvtoLiderEntities.GetContext().SaveChanges();
                            MessageBox.Show("Пользователь успешно добавлен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClassManager.BaseFrame.Navigate(new PageUsers());
                        }
                    }
                    catch (Exception en)
                    {
                        MessageBox.Show(en.Message.ToString());
                    }
                }
                else if (_currentUser.Id != 0)
                {
                    try
                    {
                        string loginCheck = "SELECT * FROM Users WHERE Login ='" + tbLogin.Text + "'";
                        var sql = bdAvtoLiderEntities.GetContext().Users.SqlQuery(loginCheck).ToArray();
                        if (tbLogin.Text == a)
                        {
                            try
                            {
                                bdAvtoLiderEntities.GetContext().SaveChanges();
                                MessageBox.Show("Пользователь успешно изменен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                                ClassManager.BaseFrame.Navigate(new PageUsers());
                            }
                            catch (Exception en)
                            {
                                MessageBox.Show(en.Message.ToString());
                            }
                        }
                        else if (tbLogin.Text != a)
                        {
                            sql = bdAvtoLiderEntities.GetContext().Users.SqlQuery(loginCheck).ToArray();
                            if (sql.Length != 0)
                            {
                                MessageBox.Show("Пользователь с данным логином уже существует", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else if (sql.Length == 0)
                            {
                                bdAvtoLiderEntities.GetContext().SaveChanges();
                                MessageBox.Show("Пользователь успешно изменен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                                ClassManager.BaseFrame.Navigate(new PageUsers());
                            }
                        }                      
                    }
                    catch (Exception en)
                    {
                        MessageBox.Show(en.Message.ToString());
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
             a = tbLogin.Text;
        }
    }
}
