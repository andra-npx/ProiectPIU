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

        public Sofer GetSofer(string nume, string prenume)
        {
            return GetListaSoferi().FirstOrDefault(s => s.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase) && s.Prenume.Equals(prenume, StringComparison.OrdinalIgnoreCase));
        }

        public List<Sofer> GetSoferi(string nume)
        {
            return GetListaSoferi().Where(s => s.Nume.Equals(nume, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public Masina GetMasina(string nr)
        {
            return GetListaMasini().FirstOrDefault(m => m.NumarInmatriculare.Equals(nr, StringComparison.OrdinalIgnoreCase));
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

                // Am scos bara "/" care dădea eroare și am lăsat "false" pentru a rescrie fișierul
                using (StreamWriter sw = new StreamWriter(numeFisier, false))
                {
                    foreach (var s in listaSoferi)
                        sw.WriteLine("S;" + s.ConversieLaSirPentruFisier());

                    foreach (var m in listaMasini)
                        sw.WriteLine("M;" + m.ConversieLaSirPentruFisier());

                    foreach (var i in listaJurnale)
                    {
                        sw.WriteLine(string.Format("J;{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}",
                            ";", i.SoferActual.IdSofer, i.MasinaActuala.NumarInmatriculare,
                            i.Start.ToString("o"), i.Stop.ToString("o"), (int)i.Tip, (int)i.Stare));
                    }
                }
                return true;
            }
            return false;
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
                            IntervalLucru interv = new IntervalLucru(sGasit, mGasit, DateTime.Parse(date[3]), DateTime.Parse(date[4]));
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
            var sofer = GetListaSoferi().FirstOrDefault(s => s.IdSofer == idSofer);
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
    }
}