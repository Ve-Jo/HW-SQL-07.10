using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;


namespace WpfApp15
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Sells;Integrated Security=True;Connect Timeout=10;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public MainWindow()
        {
            InitializeComponent();
            LoadTableNames();
        }

        private void LoadTableNames()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                DataTable tablesSchema = connection.GetSchema("Tables");

                List<string> tableNames = new List<string>();
                foreach (DataRow row in tablesSchema.Rows)
                {
                    string tableName = (string)row["TABLE_NAME"];
                    tableNames.Add(tableName);
                }

                tableComboBox.ItemsSource = tableNames;
            }
        }

        private void LoadTableData(string tableName)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = $"SELECT * FROM {tableName}";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void LoadTable_Click(object sender, RoutedEventArgs e)
        {
            string selectedTable = (string)tableComboBox.SelectedItem;
            if (selectedTable != null)
            {
                LoadTableData(selectedTable);
            }
            else
            {
                MessageBox.Show("Выберите таблицу");
            }
        }
    }
}
