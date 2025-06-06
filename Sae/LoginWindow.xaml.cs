using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Security.Cryptography;
using System.Text;
using Npgsql;
using Sae.Model;
using System.Windows.Controls;

namespace Sae
{
    public partial class LoginWindow : Window
    {
        private DataAccess dataAccess;

        public LoginWindow()
        {
            InitializeComponent();

            // Initialiser DataAccess
            dataAccess = DataAccess.Instance;

            // Permettre la connexion avec la touche Entrée
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
                // Requête pour vérifier l'utilisateur
                string query = @"
                    SELECT COUNT(*) 
                    FROM users 
                    WHERE username = @username 
                    AND password_hash = @passwordHash 
                    AND is_active = true";

                using (NpgsqlCommand command = new NpgsqlCommand(query))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@passwordHash", HashPassword(password));

                    object result = dataAccess.ExecuteSelectUneValeur(command);
                    int userCount = Convert.ToInt32(result);

                    return userCount > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur de base de données : {ex.Message}");
            }
        }

        private string HashPassword(string password)
        {
            // Utilisation de SHA256 pour hasher le mot de passe
            // En production, utilisez plutôt bcrypt ou Argon2
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
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