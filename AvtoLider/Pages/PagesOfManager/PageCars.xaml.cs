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
    /// Логика взаимодействия для PageManager.xaml
    /// </summary>
    public partial class PageCars : Page
    {
        private Word.Document document = null;
        public PageCars()
        {
            InitializeComponent();
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            ClassManager.BaseFrame.Navigate(new PageAddEditCars((sender as Button).DataContext as Cars));
            ClassManager.image = "1";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ClassManager.BaseFrame.Navigate(new PageAddEditCars(null));
            ClassManager.image = "0";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var carsForRemoving = listCars.SelectedItems.Cast<Cars>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {carsForRemoving.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    bdAvtoLiderEntities.GetContext().Cars.RemoveRange(carsForRemoving);
                    bdAvtoLiderEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    listCars.ItemsSource = bdAvtoLiderEntities.GetContext().Cars.ToList();
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
                listCars.ItemsSource = bdAvtoLiderEntities.GetContext().Cars.ToList();
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
            listCars.ItemsSource = bdAvtoLiderEntities.GetContext().Cars.ToList().Where(p => p.Brand.ToLower().Contains(tbSearch.Text.ToLower()) || 
            p.Model.ToLower().Contains(tbSearch.Text.ToLower()) || 
            p.Brand.ToLower().Contains(tbSearch.Text.ToLower()) && p.Model.ToLower().Contains(tbSearch.Text.ToLower()));
        }

        private void FormDocument()
        {
            try
            {
                var rows = listCars.ItemsSource.Cast<Cars>().ToList();
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
                cellRange.Text = "Марка";
                cellRange = table.Cell(1, 2).Range;
                cellRange.Text = "Модель";
                cellRange = table.Cell(1, 3).Range;
                cellRange.Text = "Цвет";
                cellRange = table.Cell(1, 4).Range;
                cellRange.Text = "Цена";
                cellRange = table.Cell(1, 5).Range;
                cellRange.Text = "Количество";

                table.Rows[1].Range.Bold = 1;
                int currentRow = 1;
                foreach (var row in rows)
                {
                    currentRow++;

                    cellRange = table.Cell(currentRow, 1).Range;
                    cellRange.Text = row.Brand;

                    cellRange = table.Cell(currentRow, 2).Range;
                    cellRange.Text = row.Model;

                    cellRange = table.Cell(currentRow, 3).Range;
                    cellRange.Text = row.Color;

                    cellRange = table.Cell(currentRow, 4).Range;
                    cellRange.Text = row.Price.ToString();

                    cellRange = table.Cell(currentRow, 5).Range;
                    cellRange.Text = row.Amount.ToString();
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

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta / 3);
            e.Handled = true;
        }        

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSort.SelectedIndex == 0)
            {
                listCars.ItemsSource = bdAvtoLiderEntities.GetContext().Cars.ToList();
            }
            else if (cbSort.SelectedIndex==1)
            {
                listCars.ItemsSource = bdAvtoLiderEntities.GetContext().Cars.Where(p => p.Price <= 1000000).ToList();
            }
            else if (cbSort.SelectedIndex == 2)
            {
                listCars.ItemsSource = bdAvtoLiderEntities.GetContext().Cars.Where(p => p.Price <= 2000000).ToList();
            }
            else if (cbSort.SelectedIndex == 3)
            {
                listCars.ItemsSource = bdAvtoLiderEntities.GetContext().Cars.Where(p => p.Price <= 3000000).ToList();
            }
            else if (cbSort.SelectedIndex == 4)
            {
                listCars.ItemsSource = bdAvtoLiderEntities.GetContext().Cars.Where(p => p.Price <= 4000000).ToList();
            }
            else if (cbSort.SelectedIndex == 5)
            {
                listCars.ItemsSource = bdAvtoLiderEntities.GetContext().Cars.Where(p => p.Price <= 5000000).ToList();
            }
        }
    }
}
