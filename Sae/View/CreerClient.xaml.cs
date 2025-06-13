using Npgsql;
using Sae.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Data;

namespace Sae.View
{
    public partial class CreerClient : UserControl
    {
        public CreerClient()
        {
            InitializeComponent();
        }

        private void ButtonCreerClient_Click(object sender, RoutedEventArgs e)
        {
            // --- 1. Récupération des données ---
            string nom = TextBoxNom.Text;
            string prenom = TextBoxPrenom.Text;
            string email = TextBoxEmail.Text;
            string telephone = TextBoxTel.Text;

            // --- 2. Validation des entrées de l'utilisateur ---
            // NOTE: On vérifie que les champs ne sont pas vides.
            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(telephone))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Champs requis", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // NOTE: On vérifie le format de l'email avec une expression régulière (Regex).
            // Ceci est une vérification de base, des Regex plus complexes existent.
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Le format de l'adresse email est invalide.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // NOTE: On vérifie le format du numéro de téléphone (accepte les formats français courants).
            if (!Regex.IsMatch(telephone, @"^(\+33|0)[1-9](\d{2}){4}$"))
            {
                MessageBox.Show("Le format du numéro de téléphone est invalide. (Ex: 0612345678 ou +33612345678)", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // --- 3. Création de l'objet Client ---
            // NOTE: On crée une instance du modèle Client avec les données validées.
            Client nouveauClient = new Client
            {
                Nomclient = nom,
                Prenomclient = prenom,
                Mailclient = email,
                Telclient = telephone
            };

            // --- 4. Appel de la logique métier ---
            // La vue ne fait qu'appeler la méthode Create(). C'est le modèle qui se charge de parler à la BDD.
            try
            {
                if (nouveauClient.Create())
                {
                    MessageBox.Show("Client ajouté avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Optionnel : vider les champs après succès
                    TextBoxNom.Clear();
                    TextBoxPrenom.Clear();
                    TextBoxEmail.Clear();
                    TextBoxTel.Clear();
                }
                else
                {
                    MessageBox.Show("La création du client a échoué pour une raison inconnue.", "Échec", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // NOTE: On utilise notre logger et on affiche un message simple à l'utilisateur.
                LogError.Log(ex, "Erreur lors de la tentative de création de client.");
                MessageBox.Show("Une erreur technique est survenue. L'opération a été annulée.", "Erreur Critique", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentContainer.Content = new DashEmploye();
        }
    }   }
