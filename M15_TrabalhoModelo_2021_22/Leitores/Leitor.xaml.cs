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

namespace M15_TrabalhoModelo_2021_22.Leitores
{
    /// <summary>
    /// Interaction logic for Leitor.xaml
    /// </summary>
    public partial class Leitor : Page
    {
        BaseDados bd;
        int NrRegistosPorPagina = 5;
        public Leitor(BaseDados bd)
        {
            InitializeComponent();
            this.bd = bd;
            //esconder os botões
            btAtualizar.Visibility = Visibility.Hidden;
            btRemover.Visibility = Visibility.Hidden;
            AtualizaGrid();
            ContarPaginas();
        }
        void ContarPaginas()
        {
            decimal npaginas = Math.Ceiling((decimal)C_Leitor.NrLeitores(bd) / NrRegistosPorPagina);
            cbPaginacao.Items.Clear();
            for (int i = 1; i <= npaginas; i++)
                cbPaginacao.Items.Add(i);

        }
        private void AtualizaGrid()
        {
            if(cbPaginacao.SelectedItem==null)
                DGLeitores.ItemsSource = C_Leitor.ListarTodos(bd);
            else
            {
                //paginar
                int pagina=int.Parse(cbPaginacao.SelectedItem.ToString());
                int primeiro = (pagina - 1) * NrRegistosPorPagina;
                DGLeitores.ItemsSource = C_Leitor.ListarTodos(bd, 
                    primeiro + 1, primeiro + NrRegistosPorPagina);
            }
            
        }

        //fotografia
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "C:\\";
            ofd.Multiselect = false;
            ofd.Filter = "Imagens |*.jpg;*.png | Todos os ficheiros|*.*";
            if (ofd.ShowDialog() == true)
            {
                if(ofd.FileName!=String.Empty &&
                    File.Exists(ofd.FileName))
                {
                    ImgFoto.Source = new BitmapImage(new Uri(ofd.FileName));
                    ImgFoto.Tag = ofd.FileName;
                }
            }
        }
        //adicionar
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //validar os dados
            if(ImgFoto.Tag==null || ImgFoto.Tag.ToString()=="")
            {
                MessageBox.Show("Tem de indicar uma foto");
                return;
            }
            string nome = tbNome.Text;
            DateTime data = DPData.SelectedDate.Value;
            var foto = Utils.ImagemParaVetor(ImgFoto.Tag.ToString());

            //criar um objeto do tipo c_leitor
            C_Leitor novo = new C_Leitor(0, nome, data, foto, true);

            //executar a função adicionar
            novo.Adicionar(bd);
            //limpar form
            LimparForm();
            //atualizar a grid
            AtualizaGrid();
            ContarPaginas();
        }

        private void LimparForm()
        {
            ImgFoto.Source = null;
            ImgFoto.Tag = "";
            tbNome.Text = "";

            btRemover.Visibility = Visibility.Hidden;
            btAtualizar.Visibility = Visibility.Hidden;
            btAdicionar.Visibility = Visibility.Visible;
            DGLeitores.SelectedItem = null;
        }

        private void DGLeitores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //mostrar os dados do leitor selecionado
            C_Leitor lt = (C_Leitor)DGLeitores.SelectedItem;
            if (lt == null) return;
            tbNome.Text = lt.nome;
            DPData.SelectedDate = lt.data_nascimento;
            string ficheiro = Utils.pastaDoPrograma() + @"\temp.jpg";
            Utils.VetorParaImagem(lt.fotografia, ficheiro);
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            img.UriSource = new Uri(ficheiro);
            img.EndInit();
            ImgFoto.Source = img;

            File.Delete(ficheiro);
            //mostrar botões
            btRemover.Visibility = Visibility.Visible;
            btAtualizar.Visibility = Visibility.Visible;
            btAdicionar.Visibility = Visibility.Hidden;
        }

        private void btRemover_Click(object sender, RoutedEventArgs e)
        {
            C_Leitor lt = (C_Leitor)DGLeitores.SelectedItem;
            if (lt == null) return;
            C_Leitor.Remover(bd, lt.nleitor);
            LimparForm();
            AtualizaGrid();
            ContarPaginas();
        }

        private void btAtualizar_Click(object sender, RoutedEventArgs e)
        {
            C_Leitor lt = (C_Leitor)DGLeitores.SelectedItem;
            if (lt == null) return;
            lt.nome = tbNome.Text;
            lt.data_nascimento = DPData.SelectedDate.Value;
            if (ImgFoto.Tag != null && ImgFoto.Tag.ToString()!="")
                lt.fotografia = Utils.ImagemParaVetor(ImgFoto.Tag.ToString());
            lt.Atualizar(bd);
            LimparForm();
            AtualizaGrid();
        }

        private void btCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimparForm();
        }

        private void tbPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            DGLeitores.ItemsSource = C_Leitor.PesquisarPorNome(bd, tbPesquisa.Text);
        }

        private void btImprimir_Click(object sender, RoutedEventArgs e)
        {
            Utils.printDG<C_Leitor>(DGLeitores, "Leitores");
        }

        private void cbPaginacao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AtualizaGrid();
        }

        private void DGLeitores_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGridTextColumn col = e.Column as DataGridTextColumn;
            if(col!=null && e.PropertyType == typeof(DateTime))
            {
                if (col.Header.ToString() == "data_nascimento")
                    col.Binding = new Binding(e.PropertyName) { StringFormat = "dd-MM-yyyy" };
            }
        }
    }
}
