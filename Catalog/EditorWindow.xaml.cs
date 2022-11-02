using System.Windows;

namespace Catalog
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window
    {


        public EditorWindow()
        {
            InitializeComponent();
        }


        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Price.Text) || string.IsNullOrWhiteSpace(Code.Text) ||
                string.IsNullOrWhiteSpace(Name.Text) || string.IsNullOrWhiteSpace(BarCode.Text) ||
                string.IsNullOrWhiteSpace(Quantity.Text) || string.IsNullOrWhiteSpace(Model.Text) ||
                string.IsNullOrWhiteSpace(Sort.Text) || string.IsNullOrWhiteSpace(Color.Text) ||
                string.IsNullOrWhiteSpace(Size.Text) || string.IsNullOrWhiteSpace(Wight.Text))
            {
                MessageBox.Show("Поля не могут быть пустыми!", "Сообщение");
            }
            if (decimal.TryParse(Price.Text.Replace('.', ','), out _) && int.TryParse(Code.Text, out _)
                && decimal.TryParse(Quantity.Text.Replace('.', ','), out _))
            {

                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Неверное значение", "Сообщение");
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
