namespace Final_BOUTET_BOULLAY
{
    /// <summary>
    /// Classe contenant les informations d'un joueur de la partie et ses fonctions de contrôles
    /// </summary>
    public class Joueur
    {
        // Variables de la classe Joueu
        private string nom;
        private int score;
        private List<string> motsTrouves;

        // Propriétés publiques
        public string Nom { get; private set; }
        public int Score { get; private set; }
        public List<string> MotsTrouves { get { return this.motsTrouves; } }

        /// <summary>
        /// Constructeur de la classe joueur prenant le nom en entrée.
        /// </summary>
        /// <param name="nom"></param>
        /// <exception cref="ArgumentException"></exception>
        public Joueur(string nom)
        {
            if (string.IsNullOrWhiteSpace(nom))
            {
                throw new ArgumentException("Le nom du joueur ne peut pas être vide."); // Prise en compte du cas ou l'utilisateur renvoie un nom de joueur vide
            }                
            this.nom = nom;
            this.score = 0;
            this.motsTrouves = new List<string>();
        }

        // Vérifie si le mot existe déjà dans les mots trouvés
        public bool test_contient(string mot)
        {
            return this.motsTrouves.Contains(mot);
        }

        /// <summary>
        /// Ajoute le mot en entrée à la liste des mots trouvés (si celui ci n'existe pas
        /// </summary>
        /// <param name="mot"></param>
        public void Add_Mot(string mot)
        {
            if (!test_contient(mot))
            {
                this.motsTrouves.Add(mot);
            }
        }

        /// <summary>
        /// Méthode ajoutant le score attribué au joueur.
        /// </summary>
        /// <param name="points"></param>
        public void Add_Score(int points)
        {
            this.score += points;
        }

        /// <summary>
        /// Override de ToString qui retourne les informations d'un joueur
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Joueur: {this.nom}, Score: {this.score}, Mots trouvés: {string.Join(", ", this.motsTrouves)}";
        }
    }
}
