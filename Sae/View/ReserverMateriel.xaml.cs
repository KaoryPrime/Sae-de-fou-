using Sae.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Sae.View
{
    public partial class ReserverMateriel : UserControl
    {
        public ObservableCollection<Materiel> LesMaterieles { get; set; }

        public ReserverMateriel()
        {
            InitializeComponent();
            ChargeData();
            // La liaison du filtre se fait de préférence après que le contrôle soit chargé
            this.Loaded += ReserverMateriel_Loaded;
        }

        private void ReserverMateriel_Loaded(object sender, RoutedEventArgs e)
        {
            if (MaterielListBox.ItemsSource != null)
            {
                CollectionViewSource.GetDefaultView(MaterielListBox.ItemsSource).Filter = FiltreMateriel;
            }
        }

        //Algorithme pour charger les données initiales de la page.
        private void ChargeData()
        {
            try
            {
                // 1. Charger la liste principale des matériels.
                LesMaterieles = new ObservableCollection<Materiel>(new Materiel().LoadMaterielData());
                this.DataContext = this;

                // 2. Charger et peupler les ComboBox pour les filtres.
                List<Categorie> lesCategories = new Categorie().FindAll();
                CategorieComboBox.Items.Add("Toutes les catégories");
                foreach (Categorie cat in lesCategories)
                {
                    CategorieComboBox.Items.Add(cat.Libellecategorie);
                }
                CategorieComboBox.SelectedIndex = 0;

                // Charger et peupler la ComboBox des types
                List<Sae.Model.Type> lesTypes = new Model.Type().FindAll();
                TypeComboBox.Items.Add("Tous les types");
                foreach (Sae.Model.Type type in lesTypes)
                {
                    TypeComboBox.Items.Add(type.Libelletype);
                }
                TypeComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL lors du chargement des données pour la réservation");
                MessageBox.Show("Problème lors de récupération des données", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //Algorithme de filtrage multi-critères pour la liste de matériels.
        private bool FiltreMateriel(object item)
        {
            Materiel unMateriel = item as Materiel;
            if (unMateriel == null) return false;

            // Filtre par mot-clé
            if (!string.IsNullOrEmpty(MotCleTextBox.Text) && unMateriel.Nommateriel.IndexOf(MotCleTextBox.Text, StringComparison.OrdinalIgnoreCase) < 0)
                return false;

            // Filtre par catégorie
            if (CategorieComboBox.SelectedIndex > 0 && unMateriel.Categorie?.Libellecategorie != CategorieComboBox.SelectedItem.ToString())
                return false;

            // Filtre par type
            if (TypeComboBox.SelectedIndex > 0 && unMateriel.Type?.Libelletype != TypeComboBox.SelectedItem.ToString())
                return false;

            return true; // Le matériel passe tous les filtres actifs
        }

        //Méthode centrale qui force la vue à ré-appliquer le filtre.
        private void RafraichirFiltre()
        {
            if (MaterielListBox?.ItemsSource != null)
            {
                CollectionViewSource.GetDefaultView(MaterielListBox.ItemsSource).Refresh();
            }
        }

        // --- GESTION DES ÉVÉNEMENTS ---
        private void MotCleTextBox_TextChanged(object sender, TextChangedEventArgs e) { RafraichirFiltre(); }
        private void CategorieComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { RafraichirFiltre(); }
        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { RafraichirFiltre(); }

        private void ButtonRetour_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainContentContainer.Content = new DashEmploye();
            }
        }

        //Logique de navigation pour passer à l'étape de confirmation.
        private void BtnReserver_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Materiel materielSelectionne = btn.Tag as Materiel;

            if (materielSelectionne != null && Application.Current.MainWindow is MainWindow mainWindow)
            {
                // Naviguer vers la nouvelle vue de confirmation
                mainWindow.MainContentContainer.Content = new ConfirmerReservation(materielSelectionne);
            }
        }
    }
}