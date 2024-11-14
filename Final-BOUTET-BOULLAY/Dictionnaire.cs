using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_BOUTET_BOULLAY
{
    /// <summary>
    /// Classe contenant tous les dictionnaires de la partie (Anglais et Français) et leurs fonctions de contrôles 
    /// </summary>
    public class Dictionnaire
    {
        // Initialisation des variables de la classe Dictionnaire
        private Dictionary<char, List<string>> mots; // La liste est volontairement mise au format dictionnary pour optimiser le temps de recherche
        private string langue;
        // Sécurité des variables 
        public Dictionary<char, List<string>> Mots { get { return this.mots; } set { this.mots = value; } } 
        public string Langue { get { return this.langue; } set { this.langue = value; } }

        /// <summary>
        /// Constructeur qui charge les mots à partir d'un fichier et initialise la langue
        /// </summary>
        /// <param name="cheminFichier"></param>
        /// <param name="langue"></param>
        public Dictionnaire(string cheminFichier, string langue)
        {
            this.langue = langue;
            this.mots = new Dictionary<char, List<string>>();
            List<string> lines = new List<string>();
            using (StreamReader sr = new StreamReader(cheminFichier))
            {
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                    i++;
                }
            }
            foreach (string line in lines)
            {
                string[] mots_s = line.Split(' ');
                foreach (string mot in mots_s)
                {
                    if (!mots.ContainsKey(mot[0]))
                    {
                        mots.Add(mot[0], new List<string>());
                    }
                    this.mots[mot[0]].Add(mot);
                }
            }
        }

        /// <summary>
        /// Méthode d'initialisation de la méthode récursive.
        /// </summary>
        /// <returns></returns>
        public bool RechDichoRecursif(string mot)
        {
            if (mot != null && mot.Length > 0 && this.mots.Keys.Contains(mot[0]))
            {
                return RechercheDichotomique(mot.ToUpper(), 0, this.mots[mot.ToUpper()[0]].Count - 1);
            }
            else
            { return false; }
        }

        /// <summary>
        /// Méthode de la recherche dichotomique récursive d'un mot dans le dictionnaire adéquate (FR ou EN)
        /// Il s'agit d'un algorithme de Recherche dichotomique classique du cours.
        /// </summary>
        /// <param name="mot"></param>
        /// <param name="mini"></param>
        /// <param name="maxi"></param>
        /// <returns>False si mot inexistant, sinon True (mot trouvé dans le dictionnaire)</returns>
        private bool RechercheDichotomique(string mot, int mini, int maxi)
        {
            if (mini == (maxi + mini) / 2)
            {
                if (this.mots[mot[0]][mini] == mot || this.mots[mot[0]][maxi] == mot)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (this.mots[mot[0]][(maxi + mini) / 2].CompareTo(mot) < 0)
                {
                    return RechercheDichotomique(mot, (maxi + mini) / 2, maxi);
                }
                else if (this.mots[mot[0]][(maxi + mini) / 2].CompareTo(mot) > 0)
                {
                    return RechercheDichotomique(mot, mini, (maxi + mini) / 2);
                }
                else
                {
                    return true;
                }
            }
        }


        /// <summary>
        /// Override de ToString retournant  le nombre de mots par premières lettres dans l'ordre alphabétiques
        /// </summary>
        /// <returns>Retourne un string contenant le nombre de mots par premières lettres dans l'ordre alphabétiques</returns>
        public override string ToString()
        {
            string res = $"Dictionnaire {this.langue} :\n";
            foreach (char key in this.mots.Keys) { res += key + ": " + this.mots[key].Count() + '\n'; }
            return res;
        }



        /// <summary>
        /// Initialisation du quicksort sur chaque première lettre du dictionnaire
        /// </summary>
        public void Tri_QuickSort()
        {
            foreach (char key in this.mots.Keys)
            {
                this.mots[key] = QuickSortlist(this.mots[key], 0, this.mots[key].Count - 1);
            }
        }

        /// <summary>
        /// Algorithme QuickSort Classique
        /// </summary>
        /// <param name="list"></param>
        /// <param name="leftIndex"></param>
        /// <param name="rightIndex"></param>
        /// <returns></returns>
        private List<string> QuickSortlist(List<string> list, int leftIndex, int rightIndex)
        {
            int i = leftIndex;
            int j = rightIndex;
            string pivot = list[leftIndex];
            while (i <= j)
            {
                while (list[i].CompareTo(pivot) < 0)
                {
                    i++;
                }
                while (list[j].CompareTo(pivot) > 0)
                {
                    j--;
                }
                if (i <= j)
                {
                    (list[i], list[j]) = (list[j], list[i]);
                    i++;
                    j--;
                }
            }
            if (leftIndex < j)
                QuickSortlist(list, leftIndex, j);
            if (i < rightIndex)
                QuickSortlist(list, i, rightIndex);
            return list;
        }
    }
}
