using LibrarieModele;
using System.Collections.Generic;

namespace NivelStocareData
{
    public class AdministrareTransportMemorie
    {
        private List<Sofer> soferi;
        private List<Masina> masini;
        private List<IntervalLucru> jurnale;

        public AdministrareTransportMemorie()
        {
            soferi = new List<Sofer>();
            masini = new List<Masina>();
            jurnale = new List<IntervalLucru>();
        }
        public void AddSofer(Sofer sofer)
        {
            sofer.IdSofer = soferi.Count + 1;
            soferi.Add(sofer);
        }
        public List<Sofer> GetListaSoferi()
        {
            return new List<Sofer>(soferi);
        }
        public void AddMasina(Masina masina)
        {
            masina.IdMasina = masini.Count + 1;
            masini.Add(masina);
        }
        public List<Masina> GetListaMasini()
        {
            return new List<Masina>(masini);
        }
        public void AddIntervalLucru(IntervalLucru i)
        {
            jurnale.Add(i);
        }
        public List<IntervalLucru> GetListaJurnale()
        {
            return new List<IntervalLucru>(jurnale);
        }

        public Sofer? GetSofer(string nume, string prenume)
        {
            foreach (Sofer s in soferi)
            {
                if (s.Nume.ToUpper() == nume.ToUpper() && s.Prenume.ToUpper() == prenume.ToUpper())
                {
                    return s;
                }
            }
            return null;
        }
        public List<Sofer>? GetSoferi(string nume)
        {
            List<Sofer> rezultate = new List<Sofer>();

            foreach (Sofer s in soferi)
            {
                if (s.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase))
                {
                    rezultate.Add(s);
                }
            }
            return rezultate;
        }

        public bool ModificaDateSofer(int id, string ruta, double km, Masina masina)
        {
            foreach (Sofer s in soferi)
            {
                if (s.IdSofer == id)
                {
                    s.AdaugaTraseu(ruta, km, masina); 
                    return true;
                }
            }
            return false;
        }
        public Masina? GetMasina(string nr)
        {
            foreach (Masina m in masini)
            { 
                if (m.NumarInmatriculare.ToUpper() == nr.ToUpper())
                {
                    return m; 
                }
            }
            return null; 
        }

        public Sofer? GetSoferId(int id)
        {
            foreach(Sofer s in soferi)
            {
                if(s.IdSofer==id)
                {
                    return s;
                }
            }
            return null;
        }

        public bool AddIntervalLucru(int idSofer, string nr, DateTime start, DateTime stop)
        {
            Sofer? s = GetSoferId(idSofer);
            Masina? m = GetMasina(nr);

            if(s!=null && m!=null)
            {
                IntervalLucru i = new IntervalLucru(s, m, start, stop);
                AddIntervalLucru(i);
                return true;
            }
            return false;
        }
    }
}
