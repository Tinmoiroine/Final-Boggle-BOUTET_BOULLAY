using NUnit.Framework;
using System;
using System.Collections.Generic;
using Final_BOUTET_BOULLAY; 

namespace GlobalTest
{
    public class Tests
    {
        private const string fichierTest = "LettresTest.txt";
        private const string fichierTestFR = "Mots_PossiblesFR.txt";
        private Dictionnaire dictionnaireFR;
        private Plateau plateau;
        private Dictionnaire dictionnaire;
        private Random random;


        [SetUp]
        public void Setup() {
            File.WriteAllLines(fichierTest, new[]
            {
                "A;1;9",
                "B;2;8",
                "C;3;7",
                "D;1;6",
                "E;2;5",
                "F;3;4"
            });

            File.WriteAllLines(fichierTestFR, new[]
            {
                "CHAT", "CHIEN", "SOURIS", "CHAT", "TIGRE", "LION"
            });

            dictionnaireFR = new Dictionnaire(fichierTestFR, "FR");
            dictionnaire = new Dictionnaire("MotsPossiblesFR.txt", "FR");
            plateau = new Plateau("Lettres.txt");
            random = new Random();

        }
        //-------------------------------------------------------------------- Test Classe Joueur ---------------------------------------------------------------------
        [Test]
        public void TestConstructeurJoueur()
        {
            var joueur = new Joueur("Alice");
            Assert.AreEqual("Alice", joueur.Nom);
            Assert.AreEqual(0, joueur.Score);
            Assert.IsFalse(joueur.Contain("TEST"));
        }

        // Test de la m�thode Contain
        [Test]
        public void TestContainJoueur()
        {
            var joueur = new Joueur("Alice");
            joueur.Add_Mot("TEST");
            Assert.IsTrue(joueur.Contain("TEST"));
            Assert.IsFalse(joueur.Contain("NON"));
        }

        // Test de la m�thode Add_Mot
        [Test]
        public void TestAdd_MotJoueur()
        {
            var joueur = new Joueur("Alice");
            joueur.Add_Mot("TEST");
            Assert.IsTrue(joueur.Contain("TEST"));

            // Essaye d'ajouter le m�me mot
            joueur.Add_Mot("TEST");

            // V�rifie qu'il n'y a pas de doublon
            Assert.AreEqual(1, joueur.MotsTrouves.Count);
        }

        // Test de la m�thode AjouterScore
        [Test]
        public void TestAjouterScoreJoueur()
        {
            var joueur = new Joueur("Alice");
            joueur.Add_Score(10);
            Assert.AreEqual(10, joueur.Score);
        }

        // Test de la m�thode ToString
        [Test]
        public void TestToStringJoueur()
        {
            var joueur = new Joueur("Alice");
            joueur.Add_Mot("TEST");
            joueur.Add_Score(10);
            string attendu = "Joueur: Alice, Score: 10, Mots trouv�s: TEST";
            Assert.AreEqual(attendu, joueur.ToString());
        }
        //-------------------------------------------------------------------------------------------- Fin Test Classe Joueur ----------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------- Test Classe D� ------------------------------------------------------------------------
        [TearDown]
        public void Cleanup()
        {
            // Supprime le fichier de test apr�s les tests
            if (File.Exists(fichierTest))
            {
                File.Delete(fichierTest);
            }
        }

        [Test]
        public void TestConstructeur_ChargeLettres()
        {
            var de = new De(fichierTest);

            // V�rifie que le d� a exactement 6 faces
            Assert.AreEqual(6, de.ToString().Split(',').Length);
        }

        [Test]
        public void TestLance_DefinitFaceVisible()
        {
            var de = new De(fichierTest);
            Random random = new Random();

            // Lance le d� et v�rifie que faceVisible est d�finie
            de.Lance(random);
            Assert.IsNotNull(de.FaceVisible);
            Assert.Contains(de.FaceVisible, new List<string> { "A", "B", "C", "D", "E", "F" });
        }

        [Test]
        public void TestToString_DescriptionCorrecte()
        {
            var de = new De(fichierTest);
            Random random = new Random();
            de.Lance(random);

            // V�rifie que ToString contient bien les lettres et la face visible
            string description = de.ToString();
            Assert.IsTrue(description.Contains("D� : Faces = A, B, C, D, E, F"));
            Assert.IsTrue(description.Contains("Face visible : " + de.FaceVisible));
        }

        [Test]
        public void TestConstructeur_ErreurSiMoinsDeSixFaces()
        {
            // Cr�e un fichier avec moins de 6 lettres pour tester l'exception
            File.WriteAllLines(fichierTest, new[] { "A;1;9", "B;2;8", "C;3;7" });
            Assert.Throws<ArgumentException>(() => new De(fichierTest));
        }
        //-------------------------------------------------------------------------------------------- Fin Test Classe D� --------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------- Test Classe Dictionnaire --------------------------------------------------------------

        [TearDown]
        public void CleanupDictionnaire()
        {
            // Suppression du fichier de test apr�s les tests
            if (File.Exists(fichierTestFR))
            {
                File.Delete(fichierTestFR);
            }
        }

        [Test]
        public void TestChargementMots()
        {
            // V�rifie que le dictionnaire a charg� les mots sans doublon
            Assert.AreEqual(5, dictionnaireFR.CompterMotsParLongueur().Values.Sum());
        }

        [Test]
        public void TestCompterMotsParLongueur()
        {
            var motsParLongueur = dictionnaireFR.CompterMotsParLongueur();

            // V�rifie que le dictionnaire compte correctement les mots par longueur
            Assert.AreEqual(2, motsParLongueur[4]); // 2 mots de 4 lettres : CHAT, LION
            Assert.AreEqual(3, motsParLongueur[5]); // 3 mots de 5 lettres : CHIEN, SOURIS, TIGRE
        }

        [Test]
        public void TestCompterMotsParLettre()
        {
            var motsParLettre = dictionnaireFR.CompterMotsParLettre();

            // V�rifie que le dictionnaire compte correctement les mots par premi�re lettre
            Assert.AreEqual(2, motsParLettre['C']); // 2 mots qui commencent par C : CHAT, CHIEN
            Assert.AreEqual(1, motsParLettre['S']); // 1 mot qui commence par S : SOURIS
            Assert.AreEqual(1, motsParLettre['T']); // 1 mot qui commence par T : TIGRE
            Assert.AreEqual(1, motsParLettre['L']); // 1 mot qui commence par L : LION
        }

        [Test]
        public void TestRechDichoRecursif()
        {
            // V�rifie que les mots existants sont trouv�s
            Assert.IsTrue(dictionnaireFR.RechDichoRecursif("CHAT"));
            Assert.IsTrue(dictionnaireFR.RechDichoRecursif("SOURIS"));

            // V�rifie que les mots non existants ne sont pas trouv�s
            Assert.IsFalse(dictionnaireFR.RechDichoRecursif("ELEPHANT"));
            Assert.IsFalse(dictionnaireFR.RechDichoRecursif("ZEBRE"));
        }

        [Test]
        public void TestToString()
        {
            string description = dictionnaireFR.ToString();

            // V�rifie que ToString contient des informations correctes sur les mots par longueur et par lettre
            Assert.IsTrue(description.Contains("Nombre de mots par longueur: 4 lettres: 2 mots, 5 lettres: 3 mots"));
            Assert.IsTrue(description.Contains("Nombre de mots par lettre: C: 2 mots, S: 1 mots, T: 1 mots, L: 1 mots"));
            Assert.IsTrue(description.Contains("Dictionnaire (FR)"));
        }
        //-------------------------------------------------------------------------------------------- Fin Test Classe Dictionnaire ----------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------- Test Classe Plateau -------------------------------------------------------------------


    }
}
