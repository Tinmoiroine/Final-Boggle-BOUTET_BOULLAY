using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_BOUTET_BOULLAY
{
    public class Plateau
    {
        private De[,] des;
        private Dictionnaire dictionnaire;

        // Constructeur qui initialise le plateau avec un dictionnaire et une grille de dés
        public Plateau(Dictionnaire dictionnaire, string pathToDeFile)
        {
            this.dictionnaire = dictionnaire;
            des = new De[4, 4];
            InitialiserDes(pathToDeFile);
        }

        // Initialise les dés sur le plateau (grille de 4x4)
        private void InitialiserDes(string pathToDeFile)
        {
            var random = new Random();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    des[i, j] = new De(pathToDeFile);  // Crée un nouveau dé à partir du fichier des lettres
                    des[i, j].Lance(random); // Lance le dé pour obtenir une face visible
                }
            }
        }

        // Méthode pour mélanger les dés du plateau
        public void MelangerDes(Random random)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    // Remplissez chaque case du tableau des dés
                    des[i, j].Lance(random); // Lance le dé pour obtenir une nouvelle face visible
                }
            }
        }


        // Retourne la représentation en chaîne du plateau
        public override string ToString()
        {
            var plateauAffichage = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    plateauAffichage.Append(des[i, j].FaceVisible + " ");
                }
                plateauAffichage.AppendLine();
            }
            return plateauAffichage.ToString();
        }

        // Teste si un mot est valide sur le plateau et dans le dictionnaire
        public bool Test_Plateau(string mot)
        {
            mot = mot.ToUpper();  // Met le mot en majuscule pour uniformité

            // Vérifie si le mot est dans le dictionnaire
            if (!dictionnaire.RechDichoRecursif(mot))
                return false;

            // Teste si le mot est valide en fonction des lettres adjacentes du plateau
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (des[i, j].FaceVisible == mot[0].ToString() &&
                        TestMotRecursif(mot, 0, i, j, new HashSet<(int, int)>()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Méthode récursive qui teste la validité du mot en suivant les lettres adjacentes
        private bool TestMotRecursif(string mot, int index, int x, int y, HashSet<(int, int)> visite)
        {
            // Si l'index du mot atteint sa longueur, le mot a été formé
            if (index == mot.Length)
                return true;

            // Vérifie les limites du plateau et si la case a déjà été visitée
            if (x < 0 || x >= 4 || y < 0 || y >= 4 || visite.Contains((x, y)) || des[x, y].FaceVisible != mot[index].ToString())
                return false;

            // Marque la case comme visitée
            visite.Add((x, y));

            // Parcours des 8 directions possibles autour de la case actuelle
            var directions = new (int, int)[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
            foreach (var (dx, dy) in directions)
            {
                if (TestMotRecursif(mot, index + 1, x + dx, y + dy, new HashSet<(int, int)>(visite)))
                {
                    return true;
                }
            }

            // Si aucune direction n'a permis de former le mot, on marque la case comme non visitée
            visite.Remove((x, y));
            return false;
        }
    }
}
