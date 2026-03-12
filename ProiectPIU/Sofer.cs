using System.Collections.Generic; // l-am adaugat ca sa pot folosi List

namespace Transport
{
    public class Sofer
    {
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public double KmParcursi { get; set; }
        public List<string> Trasee { get; set; }

        public Sofer(string nume, string prenume)
        {
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
    }
}
