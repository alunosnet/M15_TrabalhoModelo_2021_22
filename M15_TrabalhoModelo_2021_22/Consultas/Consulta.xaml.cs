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

namespace M15_TrabalhoModelo_2021_22.Consultas
{
    /// <summary>
    /// Interaction logic for Consulta.xaml
    /// </summary>
    public partial class Consulta : Page
    {
        BaseDados bd;
        public Consulta(BaseDados bd)
        {
            InitializeComponent();
            this.bd = bd;
        }

        private void cbConsultas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DGConsultas.DataContext = C_Consulta.ExecutaConsulta(bd,
                cbConsultas.SelectedIndex);
        }
    }
}
