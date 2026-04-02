namespace LibrarieModele
{
    public class Sofer
    {
        public int IdSofer { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public double KmParcursi { get; set; }
        public List<string> Trasee { get; set; }
        public Sofer()
        {
            Nume = string.Empty;
            Prenume=string.Empty;
            KmParcursi = 0;
            Trasee = new List<string>();
        }
        public Sofer(int id, string nume, string prenume)
        {
            IdSofer = id;
            Nume = nume;
            Prenume = prenume;
            KmParcursi = 0;
            Trasee = new List<string>();
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

            return $"ID: {IdSofer} Sofer: {Nume} {Prenume}  Km Parcursi: {KmParcursi}  Trasee: {ruteFormatate}";
        }
    }
}
