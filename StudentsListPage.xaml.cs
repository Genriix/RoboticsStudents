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
using System.Data;
using System.IO;
using Microsoft.Win32;
using OfficeOpenXml;

namespace RoboticsStudents
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class StudentsListPage : Page
    {
        public StudentsListPage()
        {
            InitializeComponent();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog op = new OpenFileDialog();
            //op.Filter = "Документ эксель(*.xlsx) | *.xlsx | Все файлы(*.*) | *.*";
            //if (op.ShowDialog() == true)
            //{
            //    using (var stream = File.Open(op.FileName, FileMode.Open, FileAccess.Read))
            //    {
            //        using (var reader = ExcelReaderFactory.CreateReader(stream))
            //        {
            //            var result = reader.AsDataSet(new ExcelDataSetConfiguration{});
            //            var dataTable = result.Tables[0];
            //            Students.ItemsSource = dataTable.DefaultView;
            //        }
            //    }

            //    //foreach (DataColumn column in Students.Columns)
            //    //{
            //    //    var binding = new Binding(column.ColumnName);
            //    //    var dataGridColumn = new DataGridTextColumn { Header = column.ColumnName, Binding = binding };
            //    //    Students.Columns.Add(dataGridColumn);
            //    //}
            //}
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы Excel (*.xls;*.xlsx)|*.xls;*.xlsx|Все файлы (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];
                    DataTable dataTable = new DataTable();
                    foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns])
                    {
                        dataTable.Columns.Add(firstRowCell.Text);
                    }
                    for (int rowNumber = 2; rowNumber <= worksheet.Dimension.Rows; rowNumber++)
                    {
                        var row = worksheet.Cells[rowNumber, 1, rowNumber, worksheet.Dimension.Columns];
                        DataRow dataRow = dataTable.Rows.Add();
                        foreach (var cell in row)
                        {
                            dataRow[cell.Start.Column - 1] = cell.Text;
                        }
                    }
                    Students.ItemsSource = dataTable.DefaultView;
                }
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
