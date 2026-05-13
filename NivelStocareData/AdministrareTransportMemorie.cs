using LibrarieModele;
using System.Collections.Generic;
using System.Linq;
using System;

namespace NivelStocareData
{
    public class AdministrareTransportMemorie : IStocareData
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

        public Sofer GetSofer(string nume, string prenume)
        {
            foreach (Sofer s in soferi)
            {
                if (s.Nume.ToUpper() == nume.ToUpper() && s.Prenume.ToUpper() == prenume.ToUpper())
                    return s;
            }
            return null;
        }

        public List<Sofer> GetSoferi(string nume)
        {
            return soferi.Where(s => s.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase)).ToList();
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

        public bool StergeSofer(int id)
        {
            var sofer = soferi.FirstOrDefault(s => s.IdSofer == id);
            if (sofer != null) { soferi.Remove(sofer); return true; }
            return false;
        }

        public Sofer GetSoferId(int id)
        {
            return soferi.FirstOrDefault(s => s.IdSofer == id);
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

        public Masina GetMasina(string nr)
        {
            return masini.FirstOrDefault(m => m.NumarInmatriculare.ToUpper() == nr.ToUpper());
        }

        public bool ModificaMasina(Masina masina)
        {
            var idx = masini.FindIndex(m => m.IdMasina == masina.IdMasina);
            if (idx >= 0) { masini[idx] = masina; return true; }
            return false;
        }

        public bool StergeMasina(int id)
        {
            var masina = masini.FirstOrDefault(m => m.IdMasina == id);
            if (masina != null) { masini.Remove(masina); return true; }
            return false;
        }

        public void AddIntervalLucru(IntervalLucru i)
        {
            jurnale.Add(i);
        }

        public List<IntervalLucru> GetListaJurnale()
        {
            return new List<IntervalLucru>(jurnale);
        }

        public bool AddIntervalLucru(int idSofer, string nr, DateTime start, DateTime stop, TipCursa t, StareInterval s)
        {
            Sofer soferGasit = soferi.FirstOrDefault(sf => sf.IdSofer == idSofer);
            Masina masinaGasita = masini.FirstOrDefault(m => m.NumarInmatriculare.Equals(nr, StringComparison.OrdinalIgnoreCase));

            if (soferGasit != null && masinaGasita != null)
            {
                IntervalLucru i = new IntervalLucru(soferGasit, masinaGasita, start, stop);
                i.Tip = t;
                i.Stare = s;
                AddIntervalLucru(i);
                return true;
            }
            return false;
        }

        public bool StergeIntervalLucru(IntervalLucru interval)
        {
            return jurnale.Remove(interval);
        }
    }
}
