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
            if (ckTodos.IsChecked == false)
                DGEmprestimos.ItemsSource = C_Emprestimo.ListaEmprestimosPorConcluir(bd);
            else
                DGEmprestimos.ItemsSource = C_Emprestimo.ListaEmprestimosTodos(bd);
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
            if(ckTodos.IsChecked==false)
                btDevolver.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            C_Emprestimo emprestimo = (C_Emprestimo)DGEmprestimos.SelectedItem;
            if (emprestimo == null) return;
            emprestimo.Receber(bd);
            AtualizaCBLivros();
            AtualizaGrid();
            btDevolver.Visibility = Visibility.Hidden;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            AtualizaGrid();
        }

        private void DGEmprestimos_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGridTextColumn col = e.Column as DataGridTextColumn;
            if (col != null && e.PropertyType == typeof(DateTime))
            {
               // if (col.Header.ToString() == "data_nascimento")
                    col.Binding = new Binding(e.PropertyName) { StringFormat = "dd-MM-yyyy" };
            }
        }
    }
}
