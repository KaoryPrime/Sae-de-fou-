using Sae.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour RechercheReservation.xaml
    /// </summary>
    public partial class RechercheReservation : UserControl
    {
        public ObservableCollection<Reservation> LesReservations { get; set; }

        public RechercheReservation()
        {
            InitializeComponent();
            ChargeData();

            //Initialisation du mécanisme de filtre pour le DataGrid.
            dgReservations.Items.Filter = RechercheMotTextBox;
            RechercheTextBox.TextChanged += RechercheTextBox_TextChanged;
        }

        //Algorithme pour charger les données des réservations depuis la BDD.
        private void ChargeData()
        {
            try
            {
                // 1. On récupère les réservations avec les détails (noms du client/matériel).
                LesReservations = new ObservableCollection<Reservation>(new Reservation().FindAllWithDetails());
                // 2. On lie le contexte de la vue pour le DataBinding.
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL lors du chargement des réservations");
                MessageBox.Show("Problème lors de récupération des données", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        //Déclencheur qui force la vue à se rafraîchir à chaque fois que le texte de recherche change.
        private void RechercheTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgReservations.ItemsSource).Refresh();
        }


        // Algorithme de recherche/filtrage appliqué sur la liste de réservations.
        private bool RechercheMotTextBox(object obj)
        {
            // 1. Si la barre de recherche est vide, on affiche tous les éléments.
            if (String.IsNullOrEmpty(RechercheTextBox.Text))
                return true;

            Reservation uneReservation = obj as Reservation;

            if (uneReservation != null)
            {
                // Recherche sur le nom du client ou du matériel
                if (uneReservation.ClientNom != null && uneReservation.ClientNom.StartsWith(RechercheTextBox.Text, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (uneReservation.MaterielNom != null && uneReservation.MaterielNom.StartsWith(RechercheTextBox.Text, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                // 2. On retourne 'true' si le nom du client OU le nom du matériel commence par le texte recherché.

            }

            return false;
        }

        private void ButtonRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentContainer.Content = new DashEmploye();
        }

    }
}