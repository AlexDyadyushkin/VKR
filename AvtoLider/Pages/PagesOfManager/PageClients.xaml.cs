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
using AvtoLider.Pages.PagesOfManager.PagesAddEdit;
using AvtoLider.Clases;
using AvtoLider.Models;
using Word = Microsoft.Office.Interop.Word;

namespace AvtoLider.Pages.PagesOfManager
{
    /// <summary>
    /// Логика взаимодействия для PageClients.xaml
    /// </summary>
    public partial class PageClients : Page
    {
        private Word.Document document = null;
        public PageClients()
        {
            InitializeComponent();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            ClassManager.BaseFrame.Navigate(new PageAddEditClients((sender as Button).DataContext as Clients));
            ClassManager.countConf = "1";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ClassManager.BaseFrame.Navigate(new PageAddEditClients(null));
            ClassManager.countConf = "0";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var clientsForRemoving = dGridClients.SelectedItems.Cast<Clients>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {clientsForRemoving.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    bdAvtoLiderEntities.GetContext().Clients.RemoveRange(clientsForRemoving);
                    bdAvtoLiderEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    dGridClients.ItemsSource = bdAvtoLiderEntities.GetContext().Clients.ToList();
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
                dGridClients.ItemsSource = bdAvtoLiderEntities.GetContext().Clients.ToList();
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            BaseWindow startWindow = (BaseWindow)Application.Current.MainWindow;

            startWindow.winBaseWindow.WindowState = WindowState.Minimized;

            FormDocument();
            document.Application.Dialogs[Microsoft.Office.Interop.Word.WdWordDialog.wdDialogFilePrint].Show();
            document.Application.Quit();
            document = null;
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            dGridClients.ItemsSource = bdAvtoLiderEntities.GetContext().Clients.ToList().Where(p => p.FirstName.ToLower().Contains(tbSearch.Text.ToLower()) || p.LastName.ToLower().Contains(tbSearch.Text.ToLower()));
        }

        private void FormDocument()
        {
            try
            {
                var rows = dGridClients.ItemsSource.Cast<Clients>().ToList();
                if (rows.Count == 0)
                {
                    MessageBox.Show("Нет данных для печати", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var app = new Word.Application();
                document = app.Documents.Add();
                Word.Paragraph tableParagraph = document.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;
                Word.Table table = document.Tables.Add(tableRange, rows.Count + 1, 5);
                table.Borders.InsideLineStyle = table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                table.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                Word.Range cellRange;
                cellRange = table.Cell(1, 1).Range;
                cellRange.Text = "Имя";
                cellRange = table.Cell(1, 2).Range;
                cellRange.Text = "Фамилия";
                cellRange = table.Cell(1, 3).Range;
                cellRange.Text = "Электронная почта";
                cellRange = table.Cell(1, 4).Range;
                cellRange.Text = "Номер телефона";
                cellRange = table.Cell(1, 5).Range;
                cellRange.Text = "Дата рождения";

                table.Rows[1].Range.Bold = 1;
                int currentRow = 1;
                foreach (var row in rows)
                {
                    currentRow++;

                    cellRange = table.Cell(currentRow, 1).Range;
                    cellRange.Text = row.FirstName;

                    cellRange = table.Cell(currentRow, 2).Range;
                    cellRange.Text = row.LastName;

                    cellRange = table.Cell(currentRow, 3).Range;
                    cellRange.Text = row.Email;

                    cellRange = table.Cell(currentRow, 4).Range;
                    cellRange.Text = row.PhoneNumber.ToString();

                    cellRange = table.Cell(currentRow, 5).Range;
                    cellRange.Text = row.Birthday.ToShortDateString();
                }

                document.Paragraphs.Add();
                Word.Paragraph sum = document.Paragraphs.Add();
                sum.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                Word.Range sumRange = sum.Range;
                sumRange.Bold = 1;
            }
            catch
            {
                MessageBox.Show("Ошибка в формировании отчета", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
