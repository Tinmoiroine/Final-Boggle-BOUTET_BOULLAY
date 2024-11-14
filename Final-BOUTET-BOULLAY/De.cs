using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_BOUTET_BOULLAY
{
    /// <summary>
    /// Classe contenant les informations sur les dés du plateau ainsi que toutes ses fonctionnalités
    /// </summary>
    public class De
    {
        //Initialisation des variables de la classe Dé
        private List<string> faces;
        private string faceVisible;

        // Propriété de sécurité pour les différentes variables des dés
        public string FaceVisible { get { return this.faceVisible; } set { this.faceVisible = value; } }
        public List<string> Faces { get { return this.faces; } set { this.faces = value; } }

        /// <summary>
        /// Constructeur de la classe Dé utilisant seulement le paramètre du chemin du fichier "path"
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException"></exception>
        public De(string path)
        {
            faces = new List<string>();
            ChargerLettres(path);
            if (faces.Count != 6)
            {
                throw new ArgumentException("Le fichier Lettres.txt doit contenir exactement 6 lettres.");
            }
        }

        /// <summary>
        /// Méthode de chargement des lettres depuis le fichier et remplir les faces du dé
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="Exception"></exception>
        private void ChargerLettres(string path)
        {
            try
            {
                string[] lignes = File.ReadAllLines(path);
                foreach (var ligne in lignes)
                {
                    string[] parts = ligne.Split(';');
                    if (parts.Length >= 1)
                    {
                        string lettre = parts[0].Trim().ToUpper();
                        if (!faces.Contains(lettre) && lettre.Length == 1)
                        {
                            faces.Add(lettre);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur Ouberture Fichier : " + ex.Message);
            }
        }

        /// <summary>
        /// Méthode permettant de tirer au hasard une lettre parmi les 6 du dé
        /// </summary>
        /// <param name="r"></param>
        public void Lance(Random r)
        {
            int index = r.Next(0, faces.Count);
            faceVisible = faces[index];
        }

        /// <summary>
        /// Override de toString retournant la description de ce dé lancé
        /// </summary>
        /// <returns>Retourne un string</returns>
        public override string ToString()
        {
            return $"Dé : Faces = {string.Join(", ", faces)}, Face visible : {faceVisible}";
        }
    }
}