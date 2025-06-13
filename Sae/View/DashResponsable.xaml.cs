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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sae.View
{
    /// <summary>
    /// Logique d'interaction pour DashResponsable.xaml
    /// </summary>
    public partial class DashResponsable : UserControl
    {
        public Materiel MaterielSelectionne { get; private set; }
        public ObservableCollection<Materiel> LesMaterieles { get; set; }
        public DashResponsable()
        {
            InitializeComponent();
            ChargeData();
            // NOTE: Initialisation du mécanisme de filtre pour le DataGrid.
            // On lie la vue de la collection à la méthode qui contient la logique de recherche.
            dgmateriel.Items.Filter = RechercheMotTextBox;
            RechercheTextBox.TextChanged += RechercheTextBox_TextChanged;
        }


        //Déclencheur qui force la vue à se rafraîchir à chaque fois que le texte de recherche change.
        private void RechercheTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgmateriel.ItemsSource).Refresh();
        }

        //Algorithme de recherche/filtrage appliqué sur la liste de matériels.
        private bool RechercheMotTextBox(object obj)
        {
            // 1. Si la barre de recherche est vide, on affiche tous les éléments.
            if (String.IsNullOrEmpty(RechercheTextBox.Text))
                return true;

            // 2. On convertit l'objet reçu en Materiel.
            Materiel unMateriel = obj as Materiel;

            // 3. On retourne 'true' seulement si le nom du matériel commence par le texte recherché (ignorant la casse).
            return (unMateriel.Nommateriel.StartsWith(RechercheTextBox.Text, StringComparison.OrdinalIgnoreCase));
        }


        //Algorithme pour charger les données depuis la BDD dans la vue.
        private void ChargeData()
        {
            try
            {
                // 1. On  recupere et on les place dans une ObservableCollection, qui notifie l'interface en cas de changement.
                LesMaterieles = new ObservableCollection<Materiel>(new Materiel().FindMaterielResp());
                // 2. On lie le contexte de données de la vue à cette classe pour le DataBinding.
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
                // 1. Récupérer le matériel associé au bouton cliqué (via la propriété Tag).
                Button btn = obj as Button;
                Materiel materielSelectionne = btn.Tag as Materiel;
                

                if (materielSelectionne != null)
                {
                    // 2. Naviguer vers la vue de traitement en passant l'objet sélectionné.
                    MaterielSelectionne = materielSelectionne;
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.MainContentContainer.Content = new TraiterResponsable(materielSelectionne);
                        ChargeData();
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors du traitement du matériel");
                MessageBox.Show("Une erreur est survenue lors du traitement du matériel.","Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            // Appeler la méthode de déconnexion de la fenêtre principale
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.SeDeconnecter();
            }
        }
    }
}