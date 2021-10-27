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

        }

        private void LimparForm()
        {
            ImgFoto.Source = null;
            ImgFoto.Tag = "";
            tbNome.Text = "";

        }
    }
}
