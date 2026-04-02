using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarieModele
{
    [Flags]
    public enum CategoriiPermis
    {
        Niciuna = 0,
        B = 1,
        C = 2,
        D = 4,
        E = 8,
        Toate = 16
    }
    public enum NivelExperienta
    {
        FaraExperienta = 0,
        Incepator = 1,
        Mediu = 2,
        Avansat = 3,
        Expert = 4
    }
    public class Sofer
    {
        private const char SEPARATOR_PRINCIPAL_FISIER = ';';
        private const char SEPARATOR_SECUNDAR_FISIER = ',';
        public int IdSofer { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public double KmParcursi { get; set; }
        public List<string> Trasee { get; set; }
        public CategoriiPermis Categorii { get; set; }
        public NivelExperienta Experienta { get; set; }
        public Sofer()
        {
            Nume = string.Empty;
            Prenume=string.Empty;
            KmParcursi = 0;
            Trasee = new List<string>();
            Categorii = CategoriiPermis.Niciuna; 
            Experienta = NivelExperienta.FaraExperienta;
        }
        public Sofer(int id, string nume, string prenume)
        {
            IdSofer = id;
            Nume = nume;
            Prenume = prenume;
            KmParcursi = 0;
            Trasee = new List<string>();
            Categorii = CategoriiPermis.B; 
            Experienta = NivelExperienta.FaraExperienta;
        }
        public void AdaugaTraseu(string rute, double km, Masina masinaUtilizata)
        {
            if(km>0)
            {
                Trasee.Add(rute);
                KmParcursi += km;
                masinaUtilizata.Rulaj += km;
            }
        }

        public string Info()
        {
            string ruteFormatate;

            if (Trasee != null && Trasee.Count > 0)
            {
                ruteFormatate = string.Join(", ", Trasee);
            }
            else
            {
                ruteFormatate = "Niciun traseu inregistrat";
            }

            return $"ID: {IdSofer}, Sofer: {Nume} {Prenume}, Km Parcursi: {KmParcursi}, Trasee: {ruteFormatate}, Categorie: {Categorii}, Nivel experienta: {Experienta}";
        }


        // Constructor pentru citirea din fisier
        public Sofer(string linieFisier)
        {
            var dateFisier = linieFisier.Split(SEPARATOR_PRINCIPAL_FISIER);
            IdSofer = Convert.ToInt32(dateFisier[0]);
            Nume = dateFisier[1];
            Prenume = dateFisier[2];
            KmParcursi = Convert.ToDouble(dateFisier[3]);
            Trasee = dateFisier[4].Split(new[] { SEPARATOR_SECUNDAR_FISIER }, StringSplitOptions.RemoveEmptyEntries).ToList();
            Categorii = (CategoriiPermis)Convert.ToInt32(dateFisier[5]);
            Experienta = (NivelExperienta)Convert.ToInt32(dateFisier[6]);
        }
        public string ConversieLaSirPentruFisier()
        {
            string sTrasee = (Trasee != null && Trasee.Count > 0)
                ? string.Join(SEPARATOR_SECUNDAR_FISIER.ToString(), Trasee)
                : " ";

            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}",
                SEPARATOR_PRINCIPAL_FISIER, IdSofer, Nume, Prenume, KmParcursi, sTrasee, (int)Categorii, (int)Experienta);
        }
    }
}
