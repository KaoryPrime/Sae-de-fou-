using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model
{
    public class LogError
    {
        public static void Log(Exception ex, string msg)
        {
            // 1. Définir le nom du fichier qui stockera les erreurs.
            string logFile = "error.log";

            // 2. Construire le message d'erreur complet.
            string content = $"{DateTime.Now}:{msg}\n {ex.Message}\n{ex.StackTrace}\n";
            try
            {
                // 3. Ajouter le message à la fin du fichier (le crée s'il n'existe pas).
                File.AppendAllText(logFile, content);
            }
            catch
            {
                Debug.WriteLine("--- ERREUR CRITIQUE DANS LE LOGGER ---");
                Debug.WriteLine("Impossible d'écrire dans le fichier de log : " + logFile);
            }
        }
    }
}
