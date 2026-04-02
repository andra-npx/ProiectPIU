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
    }
}
