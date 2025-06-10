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

namespace Sae.View
{
    /// <summary>
    /// Logique d'interaction pour DashResponsable.xaml
    /// </summary>
    public partial class DashResponsable : UserControl
    {
        public DashResponsable()
        {
            InitializeComponent();
        }

        private void RechercheTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string recherche = ((TextBox)sender).Text.ToLower();

            // Parcourir tous les Grid dans le StackPanel et les masquer/afficher selon la recherche
            foreach (Grid ligne in ((StackPanel)FindName("LignesStackPanel")).Children.OfType<Grid>())
            {
                TextBlock materiel = ligne.Children.OfType<TextBlock>().FirstOrDefault();
                if (materiel != null)
                {
                    ligne.Visibility = string.IsNullOrEmpty(recherche) ||
                                      materiel.Text.ToLower().Contains(recherche)
                                      ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
    }
}
