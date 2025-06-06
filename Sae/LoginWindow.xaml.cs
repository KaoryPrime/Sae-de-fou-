using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Npgsql;
using Sae.Model;
using System.Windows.Controls;

namespace Sae
{
    public partial class LoginWindow : Window
    {
        public List<Employe> LesEmployes { get; set; }

        public LoginWindow()
        {
            InitializeComponent();
            ChargeData();

            // Configuration des événements clavier
            this.KeyDown += LoginWindow_KeyDown;
            UsernameTextBox.KeyDown += LoginWindow_KeyDown;
            PasswordBox.KeyDown += LoginWindow_KeyDown;

            // Focus initial sur le champ username
            this.Loaded += (s, e) => UsernameTextBox.Focus();
        }

        private void ChargeData()
        {
            try
            {
                LesEmployes = new List<Employe>(new Employe().FindAll());
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL lors du chargement des employés");
                MessageBox.Show("Problème lors de récupération des données, veuillez consulter votre admin",
                              "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        private void LoginWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(sender, e);
            }
            else if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            // Validation des champs (similaire à la validation dans MainWindow)
            if (string.IsNullOrEmpty(username))
            {
                ShowError("Veuillez saisir votre identifiant.");
                UsernameTextBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowError("Veuillez saisir votre mot de passe.");
                PasswordBox.Focus();
                return;
            }

            // Désactiver les contrôles pendant la vérification
            SetControlsEnabled(false);

            try
            {
                Employe employeConnecte = AuthenticateUser(username, password);
                if (employeConnecte != null)
                {
                    // Connexion réussie - Structure similaire à l'ouverture de WindowChien
                    ShowSuccess("Connexion réussie !");

                    // Ouvrir la fenêtre principale avec l'employé connecté
                    MainWindow mainWindow = new MainWindow(employeConnecte);
                    mainWindow.Show();

                    // Fermer la fenêtre de connexion
                    this.Close();
                }
                else
                {
                    ShowError("Identifiant ou mot de passe incorrect.");
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors de l'authentification");
                ShowError("Erreur de connexion. Veuillez réessayer.");
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        private Employe AuthenticateUser(string username, string password)
        {
            try
            {
                // Recherche dans la liste des employés chargés (comme pour les chiens)
                foreach (Employe employe in LesEmployes)
                {
                    if (employe.Login.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                        employe.Mdp == password)
                    {
                        return employe;
                    }
                }

                // Alternative avec requête directe si nécessaire
                return AuthenticateUserFromDatabase(username, password);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'authentification : {ex.Message}");
            }
        }

        private Employe AuthenticateUserFromDatabase(string username, string password)
        {
            try
            {
                string query = @"
                    SELECT NUMEMPLOYE, NUMROLE, NOM, PRENOM, LOGIN, MDP 
                    FROM EMPLOYE 
                    WHERE LOGIN = @LOGIN AND MDP = @MDP";

                using (NpgsqlCommand command = new NpgsqlCommand(query))
                {
                    command.Parameters.AddWithValue("@LOGIN", username);
                    command.Parameters.AddWithValue("@MDP", password);

                    // Utilisation d'une méthode similaire à celle utilisée pour les chiens
                    var result = DataAccess.Instance.ExecuteSelect(command);

                    if (result != null && result.Rows.Count > 0)
                    {
                        var row = result.Rows[0];
                        return new Employe(
                            Convert.ToInt32(row["NUMEMPLOYE"]),
                            Convert.ToInt32(row["NUMROLE"]),
                            row["NOM"].ToString(),
                            row["PRENOM"].ToString(),
                            row["LOGIN"].ToString(),
                            row["MDP"].ToString()
                        );
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur de base de données : {ex.Message}");
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            LoginButton.IsEnabled = enabled;
            UsernameTextBox.IsEnabled = enabled;
            PasswordBox.IsEnabled = enabled;

            LoginButton.Content = enabled ? "Se connecter" : "Connexion...";
        }

        private void ResetForm()
        {
            PasswordBox.Clear();
            UsernameTextBox.Focus();
        }

        private void ShowError(string message)
        {
            MessageBox.Show(this, message, "Attention",
                          MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowSuccess(string message)
        {
            MessageBox.Show(this, message, "Succès",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Méthode pour fermer l'application si nécessaire
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}