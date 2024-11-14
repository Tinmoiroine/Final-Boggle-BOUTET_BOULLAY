using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_BOUTET_BOULLAY
{
    public class Jeu
    {
        private List<Joueur> joueurs;  // Liste des joueurs
        private Dictionnaire dictionnaire;  // Dictionnaire pour vérifier la validité des mots
        private int tempsTotalJeu; // Temps total de la partie en secondes
        private int tempsTourJoueur; // Temps de chaque tour d'un joueur en secondes

        public Jeu(string cheminDictionnaire, string langueDictionnaire, int tempsTotalJeu = 360, int tempsTourJoueur = 60)
        {
            dictionnaire = new Dictionnaire(cheminDictionnaire, langueDictionnaire);
            this.tempsTotalJeu = tempsTotalJeu;
            this.tempsTourJoueur = tempsTourJoueur;
            joueurs = new List<Joueur>
            {
                new Joueur("Joueur 1"),
                new Joueur("Joueur 2")
            };
        }

        public void DemarrerPartie()
        {
            DateTime debutJeu = DateTime.Now;
            while ((DateTime.Now - debutJeu).TotalSeconds < tempsTotalJeu)
            {
                foreach (var joueur in joueurs)
                {
                    Console.WriteLine($"\n{joueur.Nom}, c'est votre tour !");
                    JouerTour(joueur);

                    if ((DateTime.Now - debutJeu).TotalSeconds >= tempsTotalJeu)
                    {
                        Console.WriteLine("Temps de jeu total écoulé !");
                        break;
                    }
                }
            }
            AfficherResultats();
        }

        private void JouerTour(Joueur joueur)
        {
            // Créez un objet Dictionnaire
            Dictionnaire dictionnaire = new Dictionnaire("MotsPossiblesFR.txt", "FR"); // ou Mots_PossiblesEN.txt, selon votre langue

            // Créez un objet Plateau en passant l'objet dictionnaire et le chemin du fichier des dés
            Plateau plateau = new Plateau(dictionnaire, "Lettres.txt");

            plateau.MelangerDes(new Random());

            DateTime debutTour = DateTime.Now;
            while ((DateTime.Now - debutTour).TotalSeconds < tempsTourJoueur)
            {
                Console.Write("Entrez un mot (ou appuyez sur Entrée pour passer) : ");
                string mot = Console.ReadLine()?.ToUpper();

                if (string.IsNullOrWhiteSpace(mot)) break;

                if (joueur.Contain(mot))
                {
                    Console.WriteLine("Mot déjà trouvé. Essayez un autre mot.");
                    continue;
                }

                if (plateau.Test_Plateau(mot))
                {
                    joueur.Add_Mot(mot);
                    joueur.Add_Score(CalculerScoreMot(mot));
                    Console.WriteLine($"Mot accepté! Score actuel: {joueur.Score}");
                }
                else
                {
                    Console.WriteLine("Mot non valide sur le plateau ou absent du dictionnaire.");
                }
            }
        }

        private int CalculerScoreMot(string mot)
        {
            // Exemple de calcul de score basé sur la longueur du mot
            return mot.Length;
        }

        private void AfficherResultats()
        {
            Console.WriteLine("\n=== Résultats Finaux ===");
            foreach (var joueur in joueurs)
            {
                Console.WriteLine($"{joueur.Nom} - Score: {joueur.Score}");
            }

            Joueur gagnant = DeterminerGagnant();
            if (gagnant != null)
            {
                Console.WriteLine($"\nLe gagnant est {gagnant.Nom} avec un score de {gagnant.Score} !");
            }
            else
            {
                Console.WriteLine("\nLa partie est terminée avec un score égal.");
            }
        }

        private Joueur DeterminerGagnant()
        {
            if (joueurs[0].Score > joueurs[1].Score)
                return joueurs[0];
            if (joueurs[1].Score > joueurs[0].Score)
                return joueurs[1];
            return null;
        }
    }
}
