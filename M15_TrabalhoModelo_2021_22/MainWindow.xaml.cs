using M15_TrabalhoModelo_2021_22.Consultas;
using M15_TrabalhoModelo_2021_22.Emprestimos;
using M15_TrabalhoModelo_2021_22.Leitores;
using M15_TrabalhoModelo_2021_22.Livros;
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

namespace M15_TrabalhoModelo_2021_22
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BaseDados bd = new BaseDados();
        public MainWindow()
        {
            InitializeComponent();
            _NavigationFrame.Content = new Consulta(bd);
        }
        //Leitores
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _NavigationFrame.Content = new Leitor(bd);
        }
        //Exit
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            _NavigationFrame.Content = new Livro(bd);

        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            _NavigationFrame.Content = new Emprestimo(bd);

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            _NavigationFrame.Content = new Consulta(bd);
            
        }
    }
}
