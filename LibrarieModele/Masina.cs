namespace LibrarieModele
{
    public class Masina
    {
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
        public Masina(string nr, string marca, string model, int an, double rulaj=0)
        {
            NumarInmatriculare = nr;
            Marca = marca;
            Model = model;
            An = an;
            Rulaj = rulaj;
        }
    }
}
