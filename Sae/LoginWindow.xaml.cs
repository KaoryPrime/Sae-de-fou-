using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Npgsql;
using Sae.Model;

namespace Sae
{
    public partial class LoginWindow : Window
    {
        public Employe EmployeConnecte { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
            this.KeyDown += LoginWindow_KeyDown;
            this.Loaded += (s, e) => UsernameTextBox.Focus();
        }

        private void LoginWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                LoginButton_Click(sender, e);
            else if (e.Key == Key.Escape)
                this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

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

            try
            {
                // Tente d'authentifier l'utilisateur via PostgreSQL
                EmployeConnecte = AuthenticateUser(username, password);

                if (EmployeConnecte != null)
                {
                    this.DialogResult = true;
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
                LogError.Log(ex, "Erreur lors de l'authentification : " + ex.Message);
                ShowError("Erreur de connexion à la base de données. Veuillez vérifier vos identifiants et réessayer. (Détails: " + ex.Message + ")");
            }
        }

        private Employe AuthenticateUser(string username, string password)
        {
            try
            {
                // 1. Tenter de définir la chaîne de connexion avec les identifiants de l'utilisateur
                // Cela va tenter d'établir une connexion test
                DataAccess.Instance.SetConnectionDetails(username, password);

                // 2. Si SetConnectionDetails n'a pas levé d'exception, la connexion est valide.
                // Maintenant, récupérer l'objet Employe basé sur le login.
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM employe WHERE login = @login;"))
                {
                    cmd.Parameters.AddWithValue("login", username);
                    System.Data.DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);

                    if (dt.Rows.Count > 0)
                    {
                        System.Data.DataRow dr = dt.Rows[0];
                        // Créer l'objet Employe avec les données récupérées
                        return new Employe(
                            (int)dr["numemploye"],
                            (int)dr["numrole"],
                            (string)dr["nom"],
                            (string)dr["prenom"],
                            (string)dr["login"],
                            (string)dr["mdp"] // Le mot de passe ici est le mot de passe BDD, pas le hash si vous en utilisez un
                        );
                    }
                    else
                    {
                        // L'utilisateur existe dans la BDD (connexion réussie), mais n'est pas trouvé dans la table 'employe'
                        // Cela pourrait indiquer un problème de configuration ou de données.
                        LogError.Log(new Exception("User found in DB but not in employe table"), $"Authenticated user {username} not found in employe table.");
                        return null;
                    }
                }
            }
            catch (NpgsqlException dbEx)
            {
                // Gérer les erreurs spécifiques à Npgsql (ex: mot de passe invalide, utilisateur inexistant)
                LogError.Log(dbEx, $"Erreur d'authentification PostgreSQL pour {username}");
                // Ne retournez pas dbEx.Message directement à l'utilisateur final pour des raisons de sécurité,
                // mais loggez-le.
                return null; // Échec de l'authentification
            }
            catch (Exception ex)
            {
                // Gérer d'autres erreurs potentielles
                LogError.Log(ex, $"Erreur inattendue lors de l'authentification pour {username}");
                return null; // Échec de l'authentification
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(this, message, "Attention", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}