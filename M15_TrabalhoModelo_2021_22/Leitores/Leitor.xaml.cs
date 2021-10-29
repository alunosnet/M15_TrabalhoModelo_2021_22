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
        public Leitor(BaseDados bd)
        {
            InitializeComponent();
            this.bd = bd;
            //esconder os botões
            btAtualizar.Visibility = Visibility.Hidden;
            btRemover.Visibility = Visibility.Hidden;
            AtualizaGrid();
        }

        private void AtualizaGrid()
        {
            DGLeitores.ItemsSource = C_Leitor.ListarTodos(bd);
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
        }

        private void btAtualizar_Click(object sender, RoutedEventArgs e)
        {
            C_Leitor lt = (C_Leitor)DGLeitores.SelectedItem;
            if (lt == null) return;
            lt.nome = tbNome.Text;
            lt.data_nascimento = DPData.SelectedDate.Value;
            if (ImgFoto.Tag != null)
                lt.fotografia = Utils.ImagemParaVetor(ImgFoto.Tag.ToString());
            lt.Atualizar(bd);
            LimparForm();
            AtualizaGrid();
        }

        private void btCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimparForm();
        }
    }
}
