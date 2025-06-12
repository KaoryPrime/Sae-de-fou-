using Sae.Model;
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
    /// Logique d'interaction pour TraiterEmploye.xaml
    /// </summary>
    public partial class TraiterEmploye : UserControl
    {
        private Materiel materielSelectionne;
        public TraiterEmploye()
        {
            InitializeComponent();
            InitialiserInterface();
        }

        public TraiterEmploye(Materiel materiel)
        {
            InitializeComponent();
            materielSelectionne = materiel;
            InitialiserInterface();
        }
        private void ButtonRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentContainer.Content = new RetourMateriel();
        }
        private void InitialiserInterface()
        {
            if (materielSelectionne != null)
            {
                System.Diagnostics.Debug.WriteLine($"Commentaires récupérés: '{materielSelectionne.Commentaires}'");
                System.Diagnostics.Debug.WriteLine($"Commentaires est null: {materielSelectionne.Commentaires == null}");
                System.Diagnostics.Debug.WriteLine($"Commentaires est vide: {string.IsNullOrEmpty(materielSelectionne.Commentaires)}");

                // Remplir les informations du matériel
                TxtNomMateriel.Text = materielSelectionne.Nommateriel;
                TxtCategorie.Text = $"Catégorie: {materielSelectionne.Categorie?.Libellecategorie ?? "Non définie"}";
                TxtReference.Text = $"Référence: {materielSelectionne.Reference ?? "Non définie"}";
                TxtEtatActuel.Text = materielSelectionne.Etat?.Libelleetat ?? "État inconnu";
                DefinieCouleurEtat();

            }
            else
            {
                // Cas où aucun matériel n'est sélectionné
                TxtNomMateriel.Text = "Aucun matériel sélectionné";
                TxtCategorie.Text = "";
                TxtReference.Text = "";
                TxtEtatActuel.Text = "N/A";
            }
        }

        private void DefinieCouleurEtat()
        {
            if (materielSelectionne?.Etat?.Libelleetat != null)
            {
                switch (materielSelectionne.Etat.Libelleetat.ToLower())
                {
                    case "à réviser":
                        TxtEtatActuel.Background = new SolidColorBrush(Colors.Orange);
                        TxtEtatActuel.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    case "à réparer":
                        TxtEtatActuel.Background = new SolidColorBrush(Colors.Red);
                        TxtEtatActuel.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    case "disponible":
                        TxtEtatActuel.Background = new SolidColorBrush(Colors.Green);
                        TxtEtatActuel.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    default:
                        TxtEtatActuel.Background = new SolidColorBrush(Colors.Gray);
                        TxtEtatActuel.Foreground = new SolidColorBrush(Colors.White);
                        break;
                }
            }
        }
    }
}
