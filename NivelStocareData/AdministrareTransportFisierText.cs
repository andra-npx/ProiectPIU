using LibrarieModele;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NivelStocareData
{
    public class AdministrareTransportFisierText : IStocareData
    {
        private string numeFisier;

        public AdministrareTransportFisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;
            File.Open(numeFisier, FileMode.OpenOrCreate).Close();
        }

        private void RescrieFisier(List<Sofer> soferi, List<Masina> masini, List<IntervalLucru> jurnale)
        {
            using (StreamWriter sw = new StreamWriter(numeFisier, false))
            {
                foreach (var s in soferi)
                    sw.WriteLine("S;" + s.ConversieLaSirPentruFisier());

                foreach (var m in masini)
                    sw.WriteLine("M;" + m.ConversieLaSirPentruFisier());

                foreach (var i in jurnale)
                {
                    sw.WriteLine(string.Format("J;{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}",
                        ";", i.SoferActual.IdSofer, i.MasinaActuala.NumarInmatriculare,
                        i.Start.ToString("o"), i.Stop.ToString("o"), (int)i.Tip, (int)i.Stare));
                }
            }
        }

        public void AddSofer(Sofer sofer)
        {
            sofer.IdSofer = GetListaSoferi().Count + 1;
            using (StreamWriter sw = new StreamWriter(numeFisier, true))
            {
                sw.WriteLine("S;" + sofer.ConversieLaSirPentruFisier());
            }
        }

        public List<Sofer> GetListaSoferi()
        {
            List<Sofer> soferi = new List<Sofer>();
            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    if (linie.StartsWith("S;"))
                        soferi.Add(new Sofer(linie.Substring(2)));
                }
            }
            return soferi;
        }

        public Sofer GetSofer(string nume, string prenume)
        {
            return GetListaSoferi().FirstOrDefault(s =>
                s.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase) &&
                s.Prenume.Equals(prenume, StringComparison.OrdinalIgnoreCase));
        }

        public List<Sofer> GetSoferi(string nume)
        {
            return GetListaSoferi().Where(s => s.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public bool ModificaDateSofer(int id, string ruta, double km, Masina masina)
        {
            var listaSoferi = GetListaSoferi();
            var listaMasini = GetListaMasini();
            var listaJurnale = GetListaJurnale();

            var soferDeModificat = listaSoferi.FirstOrDefault(s => s.IdSofer == id);

            if (soferDeModificat != null)
            {
                soferDeModificat.AdaugaTraseu(ruta, km, masina);
                RescrieFisier(listaSoferi, listaMasini, listaJurnale);
                return true;
            }
            return false;
        }

        public bool StergeSofer(int id)
        {
            var listaSoferi = GetListaSoferi();
            var sofer = listaSoferi.FirstOrDefault(s => s.IdSofer == id);
            if (sofer == null) return false;

            listaSoferi.Remove(sofer);
            RescrieFisier(listaSoferi, GetListaMasini(), GetListaJurnale());
            return true;
        }

        public void AddMasina(Masina masina)
        {
            masina.IdMasina = GetListaMasini().Count + 1;
            using (StreamWriter sw = new StreamWriter(numeFisier, true))
            {
                sw.WriteLine("M;" + masina.ConversieLaSirPentruFisier());
            }
        }

        public List<Masina> GetListaMasini()
        {
            List<Masina> masini = new List<Masina>();
            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    if (linie.StartsWith("M;"))
                        masini.Add(new Masina(linie.Substring(2)));
                }
            }
            return masini;
        }

        public Masina GetMasina(string nr)
        {
            return GetListaMasini().FirstOrDefault(m =>
                m.NumarInmatriculare.Equals(nr, StringComparison.OrdinalIgnoreCase));
        }

        public bool ModificaMasina(Masina masina)
        {
            var listaMasini = GetListaMasini();
            var idx = listaMasini.FindIndex(m => m.IdMasina == masina.IdMasina);
            if (idx < 0) return false;

            listaMasini[idx] = masina;
            RescrieFisier(GetListaSoferi(), listaMasini, GetListaJurnale());
            return true;
        }

        public bool StergeMasina(int id)
        {
            var listaMasini = GetListaMasini();
            var masina = listaMasini.FirstOrDefault(m => m.IdMasina == id);
            if (masina == null) return false;

            listaMasini.Remove(masina);
            RescrieFisier(GetListaSoferi(), listaMasini, GetListaJurnale());
            return true;
        }

        public void AddIntervalLucru(IntervalLucru i)
        {
            using (StreamWriter sw = new StreamWriter(numeFisier, true))
            {
                sw.WriteLine(string.Format("J;{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}",
                    ";",
                    i.SoferActual.IdSofer,
                    i.MasinaActuala.NumarInmatriculare,
                    i.Start.ToString("o"),
                    i.Stop.ToString("o"),
                    (int)i.Tip,
                    (int)i.Stare));
            }
        }

        public List<IntervalLucru> GetListaJurnale()
        {
            List<IntervalLucru> jurnale = new List<IntervalLucru>();
            var soferi = GetListaSoferi();
            var masini = GetListaMasini();

            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    if (linie.StartsWith("J;"))
                    {
                        var date = linie.Split(';');
                        int idS = int.Parse(date[1]);
                        string nrM = date[2];

                        var sGasit = soferi.FirstOrDefault(s => s.IdSofer == idS);
                        var mGasit = masini.FirstOrDefault(m => m.NumarInmatriculare == nrM);

                        if (sGasit != null && mGasit != null)
                        {
                            IntervalLucru interv = new IntervalLucru(sGasit, mGasit,
                                DateTime.Parse(date[3]), DateTime.Parse(date[4]));
                            interv.Tip = (TipCursa)int.Parse(date[5]);
                            interv.Stare = (StareInterval)int.Parse(date[6]);
                            jurnale.Add(interv);
                        }
                    }
                }
            }
            return jurnale;
        }

        public bool AddIntervalLucru(int idSofer, string nr, DateTime start, DateTime stop, TipCursa t, StareInterval s)
        {
            var sofer = GetListaSoferi().FirstOrDefault(sf => sf.IdSofer == idSofer);
            var masina = GetListaMasini().FirstOrDefault(m => m.NumarInmatriculare == nr);

            if (sofer != null && masina != null)
            {
                IntervalLucru nou = new IntervalLucru(sofer, masina, start, stop);
                nou.Tip = t;
                nou.Stare = s;
                AddIntervalLucru(nou);
                return true;
            }
            return false;
        }

        public bool StergeIntervalLucru(IntervalLucru interval)
        {
            var listaJurnale = GetListaJurnale();
            var entry = listaJurnale.FirstOrDefault(i =>
                i.SoferActual.IdSofer == interval.SoferActual.IdSofer &&
                i.MasinaActuala.IdMasina == interval.MasinaActuala.IdMasina &&
                i.Start == interval.Start);

            if (entry == null) return false;

            listaJurnale.Remove(entry);
            RescrieFisier(GetListaSoferi(), GetListaMasini(), listaJurnale);
            return true;
        }
    }
}