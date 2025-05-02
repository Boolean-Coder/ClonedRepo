using System;
using System.Diagnostics;

namespace H2CloneRepos
{
    internal class Program
    {
        /** 
         * Hauptmethode: Fragt den Benutzer nach der gewünschten Funktion und führt die entsprechende Aktion aus.
         */
        static void Main(string[] args)
        {
            bool exit = false; // Steuerung der Schleife

            while (!exit) // Schleife läuft, bis der Benutzer sich entscheidet zu beenden
            {
                Console.WriteLine(@"
                                    =====Auswahl======:
                                    1️) Neues lokales Repository erstellen und auf GitHub hochladen
                                    2️) Bestehendes lokales Repository auf GitHub hochladen
                                    0️) Beenden
                                    ");
                Console.Write("Eingabe:");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateLocalRepo();
                        break;
                    case "2":
                        UploadExistingRepo();
                        break;
                    case "0":
                        Console.WriteLine("Programm wird beendet...");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Ungültige Eingabe. Bitte wähle erneut.");
                        break;
                }
            }
        }

        /** 
         * Erstellt ein neues lokales Repository und pusht es zu GitHub.
         */
        static void CreateLocalRepo()
        {
            Console.Write("📂 Gib den Namen des neuen Repositorys ein: ");
            string repoName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(repoName))
            {
                Console.WriteLine("❌ Fehler: Der Repository-Name darf nicht leer sein.");
                return;
            }

            ExecuteCommand($"mkdir {repoName} && cd {repoName} && git init");
            Console.WriteLine("\n✅ Lokales Repository wurde erstellt.\n");

            Console.Write("🔗 Gib die GitHub-URL für das neue Repository ein: ");
            string repoUrl = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(repoUrl))
            {
                Console.WriteLine("❌ Fehler: Die GitHub-URL darf nicht leer sein.");
                return;
            }

            ExecuteCommand($"git remote add origin {repoUrl} && git add . && git commit -m \"Initial commit\" && git push -u origin main");
            Console.WriteLine("🚀 Repository wurde erfolgreich auf GitHub hochgeladen!");
        }

        /** 
         * Lädt ein bestehendes lokales Repository zu GitHub hoch.
         */
        static void UploadExistingRepo()
        {
            Console.Write("🔗 Gib die GitHub-URL für das bestehende Repository ein: ");
            string repoUrl = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(repoUrl))
            {
                Console.WriteLine("❌ Fehler: Die GitHub-URL darf nicht leer sein.");
                return;
            }

            ExecuteCommand($"git remote add origin {repoUrl} && git add . && git commit -m \"Update\" && git push -u origin main");
            Console.WriteLine("🚀 Bestehendes Repository wurde erfolgreich auf GitHub hochgeladen!");
        }

        /**
         * Führt einen PowerShell-Befehl aus und gibt ggf. eine Fehlermeldung aus.
         */
        static void ExecuteCommand(string command)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("powershell.exe", $"-Command {command}")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = psi })
                {
                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        Console.WriteLine($"❌ Fehler: Das Kommando konnte nicht ausgeführt werden. Grund:\n{error}");
                    }
                    else
                    {
                        Console.WriteLine("✅ Befehl erfolgreich ausgeführt!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Unerwarteter Fehler: {ex.Message}");
            }
        }
    }
}
