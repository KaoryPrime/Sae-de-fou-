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
    /// Logique d'interaction pour RetourMateriel.xaml
    /// </summary>
    public partial class RetourMateriel : UserControl
    {
        public Materiel MaterielSelectionne { get; private set; }
        public ObservableCollection<Materiel> LesMaterieles { get; set; }
        public RetourMateriel()
        {
            InitializeComponent();
            ChargeData();

            //Initialisation du mécanisme de filtre pour le DataGrid.
            dgmateriel.Items.Filter = RechercheMotTextBox;
            RechercherTextBox.TextChanged += RechercheTextBox_TextChanged;
        }

        //Déclencheur qui force la vue à se rafraîchir à chaque fois que le texte de recherche change.
        private void RechercheTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgmateriel.ItemsSource).Refresh();
        }

        //Algorithme de recherche/filtrage appliqué sur la liste de matériels.
        private bool RechercheMotTextBox(object obj)
        {
            if (String.IsNullOrEmpty(RechercherTextBox.Text))
                return true;

            Materiel unMateriel = obj as Materiel;

            if (unMateriel != null)
            {
                if (unMateriel.Nommateriel.StartsWith(RechercherTextBox.Text, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        //Algorithme pour charger et filtrer les matériels "En location".
        private void ChargeData()
        {
            try
            {
                // 1. Récupérer tous les matériels.
                List<Materiel> tousLesMateriels = new Materiel().FindMaterielResp();

                ObservableCollection<Materiel> filteredMateriels = new ObservableCollection<Materiel>();
                foreach (Materiel materiel in tousLesMateriels)
                {
                    if (materiel.Etat.Libelleetat == "En location") 
                    {
                        filteredMateriels.Add(materiel);
                    }
                }

                LesMaterieles = filteredMateriels; 
                this.DataContext = this; 
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL lors du chargement des materiels");
                MessageBox.Show("Problème lors de récupération des données", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        //Logique de navigation lors du clic sur le bouton "Traiter".
        private void TraiterMateriel_Click(object obj, RoutedEventArgs e)
        {
            try
            {
                Button btn = obj as Button;
                Materiel materielSelectionne = btn.Tag as Materiel;

                if (materielSelectionne != null)
                {
                    MaterielSelectionne = materielSelectionne;
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.MainContentContainer.Content = new TraiterEmploye(materielSelectionne);   
                    ChargeData();
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors du traitement du matériel");
                MessageBox.Show("Une erreur est survenue lors du traitement du matériel.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentContainer.Content = new DashEmploye();
        }

        private void ButtonAnnuler_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentContainer.Content = new DashEmploye();
        }

        private void ButtonReporter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonValider_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
