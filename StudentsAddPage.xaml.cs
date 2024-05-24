using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace RoboticsStudents
{
    /// <summary>
    /// Логика взаимодействия для StudentsAddPage.xaml
    /// </summary>
    public partial class StudentsAddPage : Page
    {
        public StudentsAddPage()
        {
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open();

                string query = "" +
                    "SELECT Name FROM EducationInstitution";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EducationInstitution.Items.Add(reader["Name"].ToString());
                        }
                    }
                }
                query = "SELECT DISTINCT locality FROM Street";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        IEnumerable<string> uniqueBanks = dataTable.AsEnumerable()
                                .Select(row => row.Field<string>("locality"))
                                .Distinct();

                        Locality.ItemsSource = uniqueBanks.ToList();
                    }
                }
                query = "SELECT Name FROM CreativeAssociation";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CreativeAssociation.Items.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
        }

        private void Locality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Group.Items.Clear();
            try
            {
                string selecteLocality = Locality.SelectedItem.ToString();
                using (SqlConnection connection = new SqlConnection(Manager.connectionString))
                {
                    connection.Open();
                    string query = "SELECT Name FROM Street WHERE locality = @locality";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@locality", selecteLocality);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) 
                            {
                                Street.Items.Add(reader["Name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void CreativeAssociation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Group.Items.Clear();
            try
            {
                int selecteAssociation = CreativeAssociation.SelectedIndex;
                using (SqlConnection connection = new SqlConnection(Manager.connectionString))
                {
                    connection.Open();
                    string query = "SELECT Name FROM [Group] WHERE IdCreativeAssociation = @IdCreativeAssociation";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdCreativeAssociation", selecteAssociation+1);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Group.Items.Add(reader["Name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open ();
                int idEducation = 0;
                int idStreet = 0;
                /// Ищем образовательное учреждение 
                string query = "SELECT id FROM EducationInstitution WHERE Name = @Name";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", EducationInstitution.Text.ToUpper());
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idEducation = reader.GetInt32(0);
                        }
                        else
                        {
                            idEducation = 0;
                        }
                    }
                }

                /// Ищем улицу
                query = "SELECT id FROM Street WHERE locality = @locality AND Name = @Name";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@locality", Locality.Text.ToUpper());
                    command.Parameters.AddWithValue("@Name", Street.Text.ToUpper());
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idStreet = reader.GetInt32(0);
                        }
                        else
                        {
                            idStreet = 0;
                        }
                    }
                }





            }
        }
    }
}
