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
    /// Logique d'interaction pour DashEmploye.xaml
    /// </summary>
    public partial class DashEmploye : UserControl
    {
        public DashEmploye()
        {
            InitializeComponent();
        }
        private void ButDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            // Appeler la méthode de déconnexion de la fenêtre principale
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.SeDeconnecter();
            }
        }

        private void ReserverButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentContainer.Content = new ReserverMateriel();
        }

        private void RechercherButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentContainer.Content = new RechercheReservation();
        }

        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentContainer.Content = new RetourMateriel();
        }

        private void CreerClientButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentContainer.Content = new CreerClient();
        }
    }
}
