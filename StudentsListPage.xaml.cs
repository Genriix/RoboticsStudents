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
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace RoboticsStudents
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class StudentsListPage : System.Windows.Controls.Page
    {
        public StudentsListPage()
        {
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open();
                string query = "SELECT " +
                    "Student.id, " +
                    "F_Name, " +
                    "L_Name, " +
                    "M_Name, " +
                    "[Student/Group].StudyYear, " +
                    "[Group].Name AS GroupName, " +
                    "CreativeAssociation.Name AS CreativeName " +
                    "FROM Student " +
                    "JOIN [Student/Group] ON Student.id = [Student/Group].IdStudent " +
                    "JOIN [Group] ON [Student/Group].IdGroup = [Group].id " +
                    "JOIN CreativeAssociation ON [Group].IdCreativeAssociation = CreativeAssociation.id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (System.Data.DataTable dataTable = new System.Data.DataTable())
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            try
                            {
                                adapter.Fill(dataTable);

                                Students.ItemsSource = dataTable.DefaultView;
                            }
                            catch (Exception ex)
                            {
                                System.Windows.MessageBox.Show("Error: " + ex.Message);
                            }
                        }
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
