using LibrarieModele;
using NivelStocareData;
using System.Collections.Generic;
using System.Linq;

namespace Transport
{
    class Program
    {
        public static void Main()
        {
            IStocareData adminTransport = new AdministrareTransportFisierText("in.txt");
            Sofer? soferNou = null;
            Masina? masinaNoua = null;
            string optiune;

            do
            {
                Console.WriteLine("CS. Citire informatii sofer de la tastatura");
                Console.WriteLine("CM. Citire informatii masina de la tastatura");
                Console.WriteLine("IS. Afisarea informatiilor despre ultimul sofer introdus");
                Console.WriteLine("IM. Afisarea informatiilor despre ultima masina introdusa");
                Console.WriteLine("AS. Afisare soferi din lista");
                Console.WriteLine("AM. Afisare masini din lista");
                Console.WriteLine("SS. Salvare sofer in lista");
                Console.WriteLine("SM. Salvare masina in lista");
                Console.WriteLine("FS. Cauta sofer dupa nume si prenume");
                Console.WriteLine("FM. Cauta masina dupa numar de inmatriculare");
                Console.WriteLine("NS. Cauta soferi dupa nume");
                Console.WriteLine("M. Modifica date sofer (km parcursi si trasee)");
                Console.WriteLine("AI. Adaugare interval de lucru");
                Console.WriteLine("AL. Afisare lista jurnale");
                Console.WriteLine("X. Inchidere program");

                Console.WriteLine("Alegeti o optiune");
                optiune = Console.ReadLine()?.ToUpper() ?? string.Empty;

                switch (optiune)
                {
                    case "CS":
                        soferNou = CitireSoferTastatura();
                        break;

                    case "CM":
                        masinaNoua = CitireMasinaTastatura();
                        break;

                    case "IS":
                        if (soferNou == null)
                        {
                            Console.WriteLine("Nu exista date de afisat!");
                        }
                        else
                        {
                            AfisareSofer(soferNou);
                        }
                        break;

                    case "IM":
                        if (masinaNoua == null)
                        {
                            Console.WriteLine("Nu exista date de afisat!");
                        }
                        else
                        {
                            AfisareMasina(masinaNoua);
                        }
                        break;

                    case "AS":
                        List<Sofer> listaSoferi = adminTransport.GetListaSoferi();
                        if (listaSoferi != null)
                        {
                            AfisareSoferi(listaSoferi);
                        }
                        else
                        {
                            Console.WriteLine("Nu exista date de afisat!");
                        }
                        break;

                    case "AM":
                        List<Masina> listaMasini = adminTransport.GetListaMasini();
                        if (listaMasini != null)
                        {
                            AfisareMasini(listaMasini);
                        }
                        else
                        {
                            Console.WriteLine("Nu exista date de afisat!");
                        }
                        break;

                    case "SS":
                        if (soferNou == null)
                        {
                            Console.WriteLine("Nu exista date de salvat!");
                        }
                        else
                        {
                            adminTransport.AddSofer(soferNou);
                            Console.WriteLine("Sofer salvat.");
                        }
                        break;


                    case "SM":
                        if (masinaNoua == null)
                        {
                            Console.WriteLine("Nu exista date de salvat!");
                        }
                        else
                        {
                            adminTransport.AddMasina(masinaNoua);
                            Console.WriteLine("Masina salvata.");
                        }
                        break;

                    case "FS":
                        Console.WriteLine("Introduceti numele cautat:");
                        string numeCautat = Console.ReadLine() ?? string.Empty;
                        Console.WriteLine("Introduceti prenumele cautat:");
                        string prenumeCautat = Console.ReadLine() ?? string.Empty;
                        Sofer? soferGasit = adminTransport.GetSofer(numeCautat, prenumeCautat);
                        if (soferGasit != null)
                        {
                            Console.WriteLine("Soferul a fost gasit!");
                            AfisareSofer(soferGasit);
                        }
                        else
                        {
                            Console.WriteLine("Soferul nu a fost gasit!");
                        }
                        break;

                    case "FM":
                        Console.WriteLine("Numar inmatriculare de cautat:");
                        string nrCautat = Console.ReadLine() ?? string.Empty;
                        Masina? masinaGasita = adminTransport.GetMasina(nrCautat);
                        if (masinaGasita != null)
                        {
                            Console.WriteLine($"Masina cu numarul de inmatriculare {nrCautat} a fost gasita!");
                            AfisareMasina(masinaGasita);
                        }
                        else
                        {
                            Console.WriteLine($"Masina cu numarul de inmatriculare {nrCautat} NU a fost gasita!");
                        }
                        break;

                    case "NS":
                        Console.WriteLine("Introduceti numele cautat:");
                        numeCautat = Console.ReadLine() ?? string.Empty;
                        List<Sofer> soferiGasiti = adminTransport.GetSoferi(numeCautat);
                        if (soferiGasiti.Count > 0)
                        {
                            Console.WriteLine($"Am gasit {soferiGasiti.Count} soferi cu numele {numeCautat}");
                            AfisareSoferi(soferiGasiti);
                        }
                        else
                        {
                            Console.WriteLine($"Nu a fost gasit niciun sofer cu numele {numeCautat}");
                        }
                        break;

                    case "M":
                        Console.WriteLine("Introduceti ID-ul soferului pentru modificare:");
                        if (int.TryParse(Console.ReadLine(), out int idCautat))
                        {
                            Console.Write("Denumire traseu nou:");
                            string rutaNoua = Console.ReadLine() ?? string.Empty;
                            Console.Write("Kilometri parcursi pe acest traseu:");
                            double.TryParse(Console.ReadLine(), out double kmNoi);
                            Console.Write("Nr. inmatriculare masina utilizata:");
                            string nrM = Console.ReadLine() ?? string.Empty;
                            Masina? mUtilizata = adminTransport.GetMasina(nrM);
                            if (mUtilizata == null)
                            {
                                Console.WriteLine("Eroare: Masina nu a fost gasita in baza de date!");
                            }
                            else
                            {
                                bool succes = adminTransport.ModificaDateSofer(idCautat, rutaNoua, kmNoi, mUtilizata);

                                if (succes)
                                    Console.WriteLine("Datele soferului (km si trasee) au fost actualizate!");
                                else
                                    Console.WriteLine("Soferul cu ID-ul specificat nu a fost gasit.");
                            }
                        }
                        break;

                    case "AI":
                        AdaugareInterval(adminTransport);
                        break;

                    case "AL":
                        AfisareJurnale(adminTransport.GetListaJurnale());
                        break;

                    case "X":
                        Console.WriteLine("Aplicatia va fi inchisa");
                        return;

                    default:
                        Console.WriteLine("Optiune inexistenta");
                        break;
                }

            } while (optiune.ToUpper() != "X");

            Console.ReadKey();
        }

        public static Sofer CitireSoferTastatura()
        {
            Console.WriteLine("Introduceti numele");
            string nume = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Introduceti prenumele");
            string prenume = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Alegeti optiunile (adunati valorile pentru optiuni multiple):");
            Console.WriteLine("Categorii permis (adunati: 1-B, 2-C, 4-D, 8-E, 16-Toate):");
            Console.Write("Suma optiunilor: ");
            int.TryParse(Console.ReadLine(), out int optiuniAlese);
            CategoriiPermis optiuniCategorii = (CategoriiPermis)optiuniAlese;

            Console.WriteLine("Nivel experienta (0-Fara, 1-Incepator, 2-Mediu, 3-Avansat, 4-Expert):");
            int.TryParse(Console.ReadLine(), out int optiuneNivel);
            NivelExperienta nivelAles = (NivelExperienta)optiuneNivel;

            Sofer sofer = new Sofer(0, nume, prenume);
            sofer.Categorii = optiuniCategorii;
            sofer.Experienta = nivelAles;

            return sofer;
        }

        public static void AfisareSofer(Sofer sofer)
        {
            Console.WriteLine(sofer.Info());
        }

        public static void AfisareSoferi(List<Sofer> soferi)
        {
            Console.WriteLine("Soferii sunt:");
            foreach (Sofer s in soferi)
            {
                AfisareSofer(s);
            }
        }

        public static Masina CitireMasinaTastatura()
        {
            Console.Write("Numar inmatriculare:");
            string nr = Console.ReadLine() ?? string.Empty;

            Console.Write("Marca:");
            string marca = Console.ReadLine() ?? string.Empty;

            Console.Write("Model:");
            string model = Console.ReadLine() ?? string.Empty;

            int an;
            while (true)
            {
                Console.Write("An fabricatie:");
                if (int.TryParse(Console.ReadLine(), out an) && an >= 1900)
                    break;
                Console.Write("Introduceti un an valid:");
            }

            double rulaj;
            while (true)
            {
                Console.Write("Rulaj:");
                if (double.TryParse(Console.ReadLine(), out rulaj) && rulaj >= 0)
                    break;
                Console.Write("Introduceti un numar pozitiv pentru rulaj:");
            }

            Console.WriteLine("Alegeti culoarea:");
            Console.WriteLine("1 - Rosu, 2 - Alb, 3 - Negru, 4 - Albastru, 5 - Gri");
            int.TryParse(Console.ReadLine(), out int optiuneCuloare);
            CuloareMasina culoareAleasa = (CuloareMasina)optiuneCuloare;

            Console.WriteLine("Alegeti optiunile (adunati valorile pentru optiuni multiple):");
            Console.WriteLine("1 - AerConditionat");
            Console.WriteLine("2 - Navigatie");
            Console.WriteLine("4 - CutieAutomata");
            Console.WriteLine("8 - ScauneIncalzite");
            Console.WriteLine("16 - SenzoriParcare");
            Console.Write("Suma optiunilor: ");
            int.TryParse(Console.ReadLine(), out int optiuniAlese);
            OptiuniMasina optiuniMasina = (OptiuniMasina)optiuniAlese;

            Masina m = new Masina(0, nr, marca, model, an, rulaj);
            m.Culoare = culoareAleasa;
            m.Optiuni = optiuniMasina;

            return m;
        }

        public static void AfisareMasina(Masina m)
        {
            Console.WriteLine(m.Info());
        }

        public static void AfisareMasini(List<Masina> masini)
        {
            Console.WriteLine("Masinile sunt:");
            foreach (Masina m in masini)
            {
                AfisareMasina(m);
            }
        }

        public static void AdaugareInterval(IStocareData admin)
        {
            Console.Write("ID Sofer:");
            int.TryParse(Console.ReadLine(), out int id);

            Console.Write("Numar inmatriculare:");
            string nr = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Tip cursa (0-FaraCursa, 1-Locala, 2-Nationala, 3-Internationala, 4-Speciala):");
            int.TryParse(Console.ReadLine(), out int t);

            Console.WriteLine("Stare (0-Inexistent, 1-Programat, 2-InDesfasurare, 3-Finalizat, 4-Anulat):");
            int.TryParse(Console.ReadLine(), out int s);

            bool ok = admin.AddIntervalLucru(id, nr, DateTime.Now, DateTime.Now.AddHours(8), (TipCursa)t, (StareInterval)s);
            if (ok)
            {
                var lista = admin.GetListaJurnale();
                var ultimul = lista.Last();
                ultimul.Tip = (TipCursa)t;
                ultimul.Stare = (StareInterval)s;
                Console.WriteLine("Interval adaugat in jurnal.");
            }
            else
                Console.WriteLine("Eroare: Soferul sau Masina nu au fost gasite!");
        }

        public static void AfisareJurnale(List<IntervalLucru> lista)
        {
            foreach (var i in lista)
                Console.WriteLine($"Timp start: {i.Start}, Sofer: {i.SoferActual.Nume} {i.SoferActual.Prenume}, Masina: {i.MasinaActuala.NumarInmatriculare}, Tip: {i.Tip}, Stare: {i.Stare}");
        }
    }
}