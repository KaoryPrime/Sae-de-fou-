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
    /// Logique d'interaction pour DashResponsable.xaml
    /// </summary>
    public partial class DashResponsable : UserControl
    {
        public ObservableCollection<Materiel> LesMaterieles { get; set; }
        public DashResponsable()
        {
            InitializeComponent();
            ChargeData();
            dgmateriel.Items.Filter = RechercheMotTextBox;
            RechercheTextBox.TextChanged += RechercheTextBox_TextChanged;
        }

        private void RechercheTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgmateriel.ItemsSource).Refresh();
        }
        private bool RechercheMotTextBox(object obj)
        {
            if (String.IsNullOrEmpty(RechercheTextBox.Text))
                return true;
            Materiel unChien = obj as Materiel;
            return (unChien.Nommateriel.StartsWith(RechercheTextBox.Text, StringComparison.OrdinalIgnoreCase));
        }
        private void ChargeData()
        {
            try
            {
                LesMaterieles = new ObservableCollection<Materiel>(new Materiel().FindMaterielResp());
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL lors du chargement des materiels");
                MessageBox.Show("Problème lors de récupération des données", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }
        private void TraiterMateriel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                Materiel materielSelectionne = btn.Tag as Materiel;

                if (materielSelectionne != null)
                {
                    string message = $"Voulez-vous vraiment traiter le matériel :\n\n" +
                                   $"Nom: {materielSelectionne.Nommateriel}\n" +
                                   $"Référence: {materielSelectionne.Reference}\n" +
                                   $"État actuel: {materielSelectionne.Etat?.Libelleetat}\n" +
                                   $"Catégorie: {materielSelectionne.Categorie?.Libellecategorie}";

                    MessageBoxResult result = MessageBox.Show(message, "Confirmation",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Ici vous pouvez ajouter la logique pour traiter le matériel
                        // Par exemple, changer son état, ouvrir une fenêtre de traitement, etc.

                        // Exemple : Ouvrir une fenêtre de traitement
                        // FenetreTraitement fenetreTraitement = new FenetreTraitement(materielSelectionne);
                        // fenetreTraitement.ShowDialog();

                        // Exemple : Changer l'état du matériel
                        // materielSelectionne.Numetat = 1; // État "Disponible" par exemple
                        // materielSelectionne.UpdateEtat(); // Méthode à créer dans la classe Materiel

                        MessageBox.Show($"Le matériel '{materielSelectionne.Nommateriel}' a été traité avec succès!",
                            "Traitement effectué", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Recharger les données pour refléter les changements
                        ChargeData();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors du traitement du matériel");
                MessageBox.Show("Une erreur est survenue lors du traitement du matériel.",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
