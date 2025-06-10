using Npgsql;
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
    /// Logique d'interaction pour LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : UserControl
    {
        private DataAccess dataAccess;

        public LoginWindow()
        {
            InitializeComponent();
            dataAccess = DataAccess.Instance;

            this.KeyDown += LoginWindow_KeyDown;
            UsernameTextBox.KeyDown += LoginWindow_KeyDown;
            PasswordBox.KeyDown += LoginWindow_KeyDown;
        }

        private void LoginWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            // Validation des champs
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

            // Désactiver le bouton pendant la vérification
            LoginButton.IsEnabled = false;
            LoginButton.Content = "Connexion...";

            try
            {
                if (AuthenticateUser(username, password))
                {
                    // Connexion réussie
                    ShowSuccess("Connexion réussie !");

                    // Ouvrir la fenêtre principale et fermer la fenêtre de connexion
                    MainWindow mainWindow = new MainWindow(username);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    ShowError("Identifiant ou mot de passe incorrect.");
                    PasswordBox.Clear();
                    UsernameTextBox.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Erreur de connexion : {ex.Message}");
            }
            finally
            {
                // Réactiver le bouton
                LoginButton.IsEnabled = true;
                LoginButton.Content = "Se connecter";
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            try
            {
                // Requête pour vérifier l'utilisateur (colonnes correctes : LOGIN et MDP)
                string query = @"
                    SELECT COUNT(*) 
                    FROM EMPLOYE 
                    WHERE LOGIN = @LOGIN
                    AND MDP = @MDP";

                using (NpgsqlCommand command = new NpgsqlCommand(query))
                {
                    command.Parameters.AddWithValue("@LOGIN", username);
                    command.Parameters.AddWithValue("@MDP", password);

                    object result = dataAccess.ExecuteSelectUneValeur(command);

                    // Vérification si result n'est pas null
                    if (result != null && result != DBNull.Value)
                    {
                        int userCount = Convert.ToInt32(result);
                        return userCount > 0;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur de base de données : {ex.Message}");
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
