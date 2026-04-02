using System;

namespace LibrarieModele
{

    public enum CuloareMasina
    {
        Inexistenta = 0,
        Rosu = 1,
        Alb = 2,
        Negru = 3,
        Albastru = 4,
        Gri = 5
    }

    [Flags]
    public enum OptiuniMasina
    {
        Niciuna = 0,
        AerConditionat = 1,
        Navigatie = 2,
        CutieAutomata = 4,
        ScauneIncalzite = 8,
        SenzoriParcare = 16
    }
    public class Masina
    {
        private const char SEPARATOR_FISIER = ';';
        public int IdMasina { get; set; }
        public string NumarInmatriculare {  get; set; }
        public string Marca {  get; set; }
        public string Model { get; set; }
        public int An {  get; set; }
        public double Rulaj { get; set; }
        public CuloareMasina Culoare { get; set; }
        public OptiuniMasina Optiuni { get; set; }
        public Masina()
        {
            NumarInmatriculare = string.Empty;
            Marca = string.Empty;
            Model = string.Empty;
            An = 0;
            Rulaj = 0;
            Culoare = CuloareMasina.Inexistenta;
            Optiuni = OptiuniMasina.Niciuna;
        }
        public Masina(int id, string nr, string marca, string model, int an, double rulaj=0)
        {
            IdMasina = id;
            NumarInmatriculare = nr;
            Marca = marca;
            Model = model;
            An = an;
            Rulaj = rulaj;
            Culoare = CuloareMasina.Inexistenta;
            Optiuni = OptiuniMasina.Niciuna;
        }

        public string Info()
        {
            return $"Id: {IdMasina}, Numar inmatriculare: {NumarInmatriculare}, Marca: {Marca}, Model: {Model}, An: {An}, Rulaj: {Rulaj}, Culoare: {Culoare}, Optiuni: {Optiuni}";
        }


        public Masina(string linieFisier)
        {
            var date = linieFisier.Split(SEPARATOR_FISIER);
            IdMasina = Convert.ToInt32(date[0]);
            NumarInmatriculare = date[1];
            Marca = date[2];
            Model = date[3];
            An = Convert.ToInt32(date[4]);
            Rulaj = Convert.ToDouble(date[5]);
            Culoare = (CuloareMasina)Convert.ToInt32(date[6]);
            Optiuni = (OptiuniMasina)Convert.ToInt32(date[7]);
        }

        public string ConversieLaSirPentruFisier()
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}",
                SEPARATOR_FISIER, IdMasina, NumarInmatriculare, Marca, Model, An, Rulaj, (int)Culoare, (int)Optiuni);
        }
    }
}
