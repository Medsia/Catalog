using System;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Windows;

namespace Catalog
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLiteConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                sqlConnection = new SQLiteConnection(ConfigurationManager.ConnectionStrings["CatalogDb"].ConnectionString);

                sqlConnection.Open();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message");
            }
            DataGridUpdate();
        }

        void DataGridUpdate()
        {
            try
            {

                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM Products " +
                    "INNER JOIN Prices ON Products.PriceId = Prices.PriceId", sqlConnection))
                {
                    FillDataGrid(dataAdapter);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message");
            }
        }
        private void Button_Add1000_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SQLiteCommand commandForProducts = new SQLiteCommand("INSERT INTO [Products] (Id, PriceId, Code, Name, BarCode, Quantity, Model," +
                " Sort, Color, Size, Wight, DataChanges)" +
                " VALUES (@Id, @PriceId, @Code, @Name, @BarCode, @Quantity, @Model, @Sort, @Color, @Size, @Wight, @DataChanges)",
                sqlConnection);
                SQLiteCommand commandForPrices = new SQLiteCommand("INSERT INTO [Prices] (PriceId, Price) VALUES (@PriceId, @Price)", sqlConnection);

                Random rnd = new Random();

                for (int i = 0; i != 1000; i++)
                {

                    string rPrice = rnd.Next(20).ToString();
                    string rId = Guid.NewGuid().ToString();
                    string rPriceId = GetPriceGuid(rPrice);

                    commandForProducts.Parameters.AddWithValue("Id", rId);
                    commandForProducts.Parameters.AddWithValue("PriceId", rPriceId);
                    commandForProducts.Parameters.AddWithValue("Name", RandomString(5, rnd));
                    commandForProducts.Parameters.AddWithValue("Code", rnd.Next(10000));
                    commandForProducts.Parameters.AddWithValue("BarCode", rnd.Next(10000));
                    commandForProducts.Parameters.AddWithValue("Quantity", rnd.Next(100));
                    commandForProducts.Parameters.AddWithValue("Model", RandomString(5, rnd));
                    commandForProducts.Parameters.AddWithValue("Sort", RandomString(5, rnd));
                    commandForProducts.Parameters.AddWithValue("Color", RandomString(5, rnd));
                    commandForProducts.Parameters.AddWithValue("Size", rnd.Next(100));
                    commandForProducts.Parameters.AddWithValue("Wight", rnd.Next(100));
                    commandForProducts.Parameters.AddWithValue("DataChanges", DateTime.Now);
                    commandForPrices.Parameters.AddWithValue("PriceId", rPriceId);
                    commandForPrices.Parameters.AddWithValue("Price", rPrice);
                    commandForProducts.ExecuteNonQuery();
                    commandForPrices.ExecuteNonQuery();
                }
                DataGridUpdate();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message");
            }

            string RandomString(int length, Random rnd)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                return new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[rnd.Next(s.Length)]).ToArray());
            }

        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            EditorWindow editorWindow = new EditorWindow();
            try
            {
                SQLiteCommand commandForProducts = new SQLiteCommand("INSERT INTO [Products] (Id, PriceId, Code, Name, BarCode, Quantity, Model," +
                " Sort, Color, Size, Wight, DataChanges)" +
                " VALUES (@Id, @PriceId, @Code, @Name, @BarCode, @Quantity, @Model, @Sort, @Color, @Size, @Wight, @DataChanges)",
                sqlConnection);
                SQLiteCommand commandForPrices = new SQLiteCommand("INSERT INTO [Prices] (PriceId, Price) VALUES (@PriceId, @Price)",
                    sqlConnection);

                editorWindow.ShowDialog();
                if (editorWindow.DialogResult == true)
                {

                    string price = editorWindow.Price.Text.Replace(',', '.');
                    string priceId = GetPriceGuid(price);
                    string id = Guid.NewGuid().ToString();

                    commandForPrices.Parameters.AddWithValue("Price", price);
                    commandForProducts.Parameters.AddWithValue("Code", editorWindow.Code.Text);
                    commandForProducts.Parameters.AddWithValue("Quantity", editorWindow.Quantity.Text.Replace(',', '.'));
                    commandForProducts.Parameters.AddWithValue("Id", id);
                    commandForProducts.Parameters.AddWithValue("PriceId", priceId);
                    commandForProducts.Parameters.AddWithValue("Name", editorWindow.Name.Text);
                    commandForProducts.Parameters.AddWithValue("BarCode", editorWindow.BarCode.Text);
                    commandForProducts.Parameters.AddWithValue("Model", editorWindow.Model.Text);
                    commandForProducts.Parameters.AddWithValue("Sort", editorWindow.Sort.Text);
                    commandForProducts.Parameters.AddWithValue("Color", editorWindow.Color.Text);
                    commandForProducts.Parameters.AddWithValue("Size", editorWindow.Size.Text);
                    commandForProducts.Parameters.AddWithValue("Wight", editorWindow.Wight.Text);
                    commandForProducts.Parameters.AddWithValue("DataChanges", DateTime.Now);
                    commandForPrices.Parameters.AddWithValue("PriceId", priceId);

                    commandForProducts.ExecuteNonQuery();
                    commandForPrices.ExecuteNonQuery();
                    MessageBox.Show("Строка добавлена", "Сообщение");
                    DataGridUpdate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверное значение\n" + $"({ ex.Message})", "Ошибка");
            }
        }

        private void Button_Edit_Click(object sender, RoutedEventArgs e)
        {
            EditorWindow editorWindow = new EditorWindow();
            try
            {
                editorWindow.Code.Text = ((DataRowView)DGridProducts.SelectedItems[0]).Row["Code"].ToString();
                editorWindow.Price.Text = ((DataRowView)DGridProducts.SelectedItems[0]).Row["Price"].ToString();
                editorWindow.Name.Text = ((DataRowView)DGridProducts.SelectedItems[0]).Row["Name"].ToString(); ;
                editorWindow.BarCode.Text = ((DataRowView)DGridProducts.SelectedItems[0]).Row["BarCode"].ToString();
                editorWindow.Quantity.Text = ((DataRowView)DGridProducts.SelectedItems[0]).Row["Quantity"].ToString();
                editorWindow.Model.Text = ((DataRowView)DGridProducts.SelectedItems[0]).Row["Model"].ToString();
                editorWindow.Sort.Text = ((DataRowView)DGridProducts.SelectedItems[0]).Row["Sort"].ToString();
                editorWindow.Color.Text = ((DataRowView)DGridProducts.SelectedItems[0]).Row["Color"].ToString();
                editorWindow.Size.Text = ((DataRowView)DGridProducts.SelectedItems[0]).Row["Size"].ToString();
                editorWindow.Wight.Text = ((DataRowView)DGridProducts.SelectedItems[0]).Row["Wight"].ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка операции", "Ошибка");
                return;
            }

            editorWindow.ShowDialog();

            if (editorWindow.DialogResult == true)
            {
                try
                {
                    SQLiteCommand commandForProducts = new SQLiteCommand("Update Products SET PriceId = (@PriceId), " +
                        "Code = (@Code), Name =(@Name), BarCode =(@BarCode), Quantity =(@Quantity), Model =(@Model), " +
                        "Sort =(@Sort), Color =(@Color), Size =(@Size), Wight =(@Wight), DataChanges =(@DataChanges) " +
                    "WHERE Id=@Id", sqlConnection);
                    SQLiteCommand commandForPrices = new SQLiteCommand("INSERT INTO [Prices] (PriceId, Price) " +
                        "VALUES (@PriceId, @Price)", sqlConnection);

                    string price = editorWindow.Price.Text.Replace(',', '.');
                    string priceId = GetPriceGuid(price);

                    commandForProducts.Parameters.AddWithValue("Id", ((DataRowView)DGridProducts.SelectedItems[0]).Row["ProductId"].ToString());
                    commandForProducts.Parameters.AddWithValue("PriceId", priceId);
                    commandForProducts.Parameters.AddWithValue("Name", editorWindow.Name.Text);
                    commandForProducts.Parameters.AddWithValue("BarCode", editorWindow.BarCode.Text);
                    commandForProducts.Parameters.AddWithValue("Model", editorWindow.Model.Text);
                    commandForProducts.Parameters.AddWithValue("Sort", editorWindow.Sort.Text);
                    commandForProducts.Parameters.AddWithValue("Color", editorWindow.Color.Text);
                    commandForProducts.Parameters.AddWithValue("Size", editorWindow.Size.Text);
                    commandForProducts.Parameters.AddWithValue("Wight", editorWindow.Wight.Text);
                    commandForProducts.Parameters.AddWithValue("DataChanges", DateTime.Now);
                    commandForPrices.Parameters.AddWithValue("Price", price);
                    commandForPrices.Parameters.AddWithValue("PriceId", priceId);
                    commandForProducts.Parameters.AddWithValue("Code", editorWindow.Code.Text);
                    commandForProducts.Parameters.AddWithValue("Quantity", editorWindow.Quantity.Text.Replace(',', '.'));

                    commandForProducts.ExecuteNonQuery();
                    commandForPrices.ExecuteNonQuery();
                    MessageBox.Show("Данные изменены", "Сообщение");
                    DataGridUpdate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Неверное значение\n" + $"({ ex.Message})", "Ошибка");
                }
            }
        }
        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            string Message = "Вы уверены, что хотите удалить данные?";
            if (MessageBox.Show(Message, "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            try
            {

                SQLiteCommand command = new SQLiteCommand("DELETE FROM Products WHERE Id=@Id", sqlConnection);
                command.Parameters.AddWithValue("Id", ((DataRowView)DGridProducts.SelectedItems[0]).Row["ProductId"].ToString());
                command.ExecuteNonQuery();
                DataGridUpdate();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка \n" + $"({ ex.Message})", "Ошибка");
            }
        }
        private void Button_DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            string Message = "Вы уверены, что хотите удалить все данные?";
            if (MessageBox.Show(Message, "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            try
            {

                SQLiteCommand command = new SQLiteCommand("DELETE FROM Products", sqlConnection);
                command.ExecuteNonQuery();
                DataGridUpdate();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка \n" + $"({ ex.Message})", "Ошибка");
            }
        }
        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string item = null;

                if (SearchByCode.IsChecked == true)
                    item = "Code";
                else if (SearchByName.IsChecked == true)
                    item = "Name";
                else if (SearchByBarCode.IsChecked == true)
                    item = "BarCode";
                else if (SearchByPrice.IsChecked == true)
                    item = "Price";

                SQLiteCommand command = new SQLiteCommand("SELECT * FROM Products " +
                    "INNER JOIN Prices ON Products.PriceId = Prices.PriceId " +
                    $"WHERE {item} LIKE  '{SearchPart.Text.Replace(',', '.')}%'", sqlConnection);

                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command.CommandText, sqlConnection))
                {
                    FillDataGrid(dataAdapter);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Operation fail", "Message");
                return;
            }
        }
        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            sqlConnection.Close();
            Close();
        }
        void FillDataGrid(SQLiteDataAdapter dataAdapter)
        {

            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            DecodeIds(dataTable);
            DGridProducts.ItemsSource = dataTable.DefaultView;

        }

        //Add a DataColumns with decoded Ids
        private static void DecodeIds(DataTable dataTable)
        {
            DataColumn columnProductId = dataTable.Columns.Add("ProductId", typeof(string));
            DataColumn columnPriceId = dataTable.Columns.Add("IdPrice", typeof(string));

            foreach (DataRow row in dataTable.Rows)
            {
                row[columnProductId] = Encoding.UTF8.GetString((byte[])row["Id"]);
            }
            foreach (DataRow row in dataTable.Rows)
            {
                row[columnPriceId] = Encoding.UTF8.GetString((byte[])row["PriceId"]);
            }
        }

        // Generating/Getting a key for PriceId 
        string GetPriceGuid(string price)
        {
            using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM PRICES", sqlConnection))
            {

                DataTable dataTable = new DataTable();

                dataAdapter.Fill(dataTable);
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row["Price"].ToString() == price)
                        return Encoding.UTF8.GetString((byte[])row["PriceId"]);
                }
                return Guid.NewGuid().ToString();
            }
        }

    }
}
