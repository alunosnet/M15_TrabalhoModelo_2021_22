using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace M15_TrabalhoModelo_2021_22.Livros
{
    /// <summary>
    /// Interaction logic for Livro.xaml
    /// </summary>
    public partial class Livro : Page
    {
        BaseDados bd;
        public Livro(BaseDados bd)
        {
            InitializeComponent();
            this.bd = bd;
            AtualizaGrid();
            LimparForm();
        }
        //imagem
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Imagens |*.jpg;*.png | Todos os ficheiros |*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileName != string.Empty && File.Exists(openFileDialog.FileName))
                {
                    ImgCapa.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    ImgCapa.Tag = openFileDialog.FileName;
                }
            }
        }
        
        private void LimparForm()
        {
            ImgCapa.Source = null;
            ImgCapa.Tag = null;
            tbNome.Text = "";
            tbAno.Text = "";
            tbPreco.Text = "";

            btRemover.Visibility = Visibility.Hidden;
            btAtualizar.Visibility = Visibility.Hidden;
            btAdicionar.Visibility = Visibility.Visible;
            DGLivros.SelectedItem = null;
        }

        private void btAdicionar_Click(object sender, RoutedEventArgs e)
        {
            //validar dados do form
            string nome = tbNome.Text;
            if (nome.Trim().Length == 0)
            {
                MessageBox.Show("O nome é obrigatório");
                return;
            }
            int ano = int.Parse(tbAno.Text);
            if (ano < 0 || ano > DateTime.Now.Year)
            {
                MessageBox.Show("O ano não está correto");
                return;
            }
            decimal preco = Decimal.Parse(tbPreco.Text);
            if (preco < 0)
            {
                MessageBox.Show("O preço não pode ser negativo");
                return;
            }
            Guid guid = Guid.NewGuid();
            string capa = Utils.pastaDoPrograma() + @"\" + guid.ToString();
            //criar objeto
            C_Livro lv = new C_Livro(0, nome, ano, DPData.SelectedDate.Value, preco, 
                capa, true);
            //guardar na bd
            lv.Adicionar(bd);
            //guardar imagem
            string ficheiro = ImgCapa.Tag.ToString();
            if (ficheiro != string.Empty)
            {
                if (File.Exists(ficheiro))
                    File.Copy(ficheiro, capa);
            }
            //limpar form
            LimparForm();
            AtualizaGrid();
        }
        private void AtualizaGrid()
        {
            DGLivros.ItemsSource = C_Livro.ListarTodos(bd);
        }
        private void DGLivros_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //mostrar os dados do leitor selecionado
            C_Livro lv = (C_Livro)DGLivros.SelectedItem;
            if (lv == null) return;
            tbNome.Text = lv.nome;
            tbPreco.Text = lv.preco.ToString();
            tbAno.Text = lv.ano.ToString();
            DPData.SelectedDate = lv.data_aquisicao;
            if (File.Exists(lv.capa))
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                img.UriSource = new Uri(lv.capa);
                img.EndInit();
                ImgCapa.Source = img;
            }
            //mostrar botões
            btRemover.Visibility = Visibility.Visible;
            btAtualizar.Visibility = Visibility.Visible;
            btAdicionar.Visibility = Visibility.Hidden;
        }

        private void btRemover_Click(object sender, RoutedEventArgs e)
        {
            C_Livro lv = (C_Livro)DGLivros.SelectedItem;
            if (lv == null) return;
            //apagar ficheiro da capa
            if (File.Exists(lv.capa))
            {
                File.Delete(lv.capa);
            }
            C_Livro.Remover(bd, lv.nlivro);
            LimparForm();
            AtualizaGrid();
        }

        private void btAtualizar_Click(object sender, RoutedEventArgs e)
        {
            C_Livro lv = (C_Livro)DGLivros.SelectedItem;
            if (lv == null) return;
            lv.nome = tbNome.Text;
            lv.ano = int.Parse(tbAno.Text);
            lv.data_aquisicao = DPData.SelectedDate.Value;
            lv.preco = decimal.Parse(tbPreco.Text);
            if (ImgCapa.Tag != null && ImgCapa.Tag.ToString() != "")
            {
                Guid guid = Guid.NewGuid();
                string capa = Utils.pastaDoPrograma() + @"\" + guid.ToString();
                //apagar ficheiro da capa
                if (File.Exists(lv.capa))
                {
                    File.Delete(lv.capa);
                }
                lv.capa = capa;
                if (File.Exists(ImgCapa.Tag.ToString()))
                    File.Copy(ImgCapa.Tag.ToString(), capa);
            }
            lv.Atualizar(bd);
            LimparForm();
            AtualizaGrid();
        }

        private void btCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimparForm();
        }

        private void DGLivros_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGridTextColumn col = e.Column as DataGridTextColumn;
            if (col != null && e.PropertyType == typeof(DateTime))
            {
                    col.Binding = new Binding(e.PropertyName) { StringFormat = "dd-MM-yyyy" };
            }
        }
    }
}
