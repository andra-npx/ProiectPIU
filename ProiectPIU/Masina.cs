namespace Transport
{
    public class Masina
    {
        public string NumarInmatriculare {  get; set; }
        public string Marca {  get; set; }
        public string Model { get; set; }
        public int An {  get; set; }
        public double Rulaj { get; set; }


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
