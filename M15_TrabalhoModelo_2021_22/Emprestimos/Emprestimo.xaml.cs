using M15_TrabalhoModelo_2021_22.Leitores;
using M15_TrabalhoModelo_2021_22.Livros;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace M15_TrabalhoModelo_2021_22.Emprestimos
{
    /// <summary>
    /// Interaction logic for Emprestimo.xaml
    /// </summary>
    public partial class Emprestimo : Page
    {
        BaseDados bd;
        public Emprestimo(BaseDados bd)
        {
            InitializeComponent();
            this.bd = bd;
            AtualizaCBLeitores();
            AtualizaCBLivros();
            btDevolver.Visibility = Visibility.Hidden;
            AtualizaGrid();
        }

        private void AtualizaGrid()
        {
            DGEmprestimos.ItemsSource = C_Emprestimo.ListaEmprestimosPorConcluir(bd);
        }

        private void AtualizaCBLivros()
        {
            cbLivros.ItemsSource = C_Livro.ListarTodos(bd, true);
        }

        private void AtualizaCBLeitores()
        {
            cbLeitores.ItemsSource = C_Leitor.ListarTodos(bd);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            C_Leitor c_Leitor = (C_Leitor)cbLeitores.SelectedItem;
            C_Livro c_Livro = (C_Livro)cbLivros.SelectedItem;
            if(c_Livro==null || c_Leitor == null)
            {
                return;
            }
            C_Emprestimo emprestimo = new C_Emprestimo(c_Leitor.nleitor,
                c_Livro.nlivro);
            emprestimo.Adicionar(bd);
            AtualizaCBLivros();
            AtualizaCBLeitores();
            AtualizaGrid();
        }

        private void DGEmprestimos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
