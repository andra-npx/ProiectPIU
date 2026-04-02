namespace LibrarieModele
{
    public class Masina
    {
        public int IdMasina { get; set; }
        public string NumarInmatriculare {  get; set; }
        public string Marca {  get; set; }
        public string Model { get; set; }
        public int An {  get; set; }
        public double Rulaj { get; set; }
        public Masina()
        {
            NumarInmatriculare = string.Empty;
            Marca = string.Empty;
            Model = string.Empty;
            An = 0;
            Rulaj = 0;
        }
        public Masina(int id, string nr, string marca, string model, int an, double rulaj=0)
        {
            IdMasina = id;
            NumarInmatriculare = nr;
            Marca = marca;
            Model = model;
            An = an;
            Rulaj = rulaj;
        }

        public string Info()
        {
            return $"Id: {IdMasina}, numar inmatriculare: {NumarInmatriculare}, marca: {Marca}, model: {Model}, an: {An}, rulaj: {Rulaj}";
        }
    }
}
