using LibrarieModele;
using NivelStocareData;
using System.Collections.Generic;

namespace Transport
{
    class Program
    {
        public static void Main()
        {
            AdministrareTransportMemorie adminTransport = new AdministrareTransportMemorie();
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
                            soferNou = null;
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
                            masinaNoua = null;
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
                            Masina? mUtilizata = adminTransport.GetMasina(nrM) ?? new Masina();
                            bool succes = adminTransport.ModificaDateSofer(idCautat, rutaNoua, kmNoi, mUtilizata);

                            if (succes)
                                Console.WriteLine("Datele soferului (km si trasee) au fost actualizate!");
                            else
                                Console.WriteLine("Soferul cu ID-ul specificat nu a fost gasit.");
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

            Sofer sofer = new Sofer(0, nume, prenume);
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
            return new Masina(0, nr, marca, model, an, rulaj);
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

        public static void AdaugareInterval(AdministrareTransportMemorie admin)
        {
            Console.Write("ID Sofer:");
            int.TryParse(Console.ReadLine(), out int id);
            Console.Write("Numar inmatriculare:");
            string nr = Console.ReadLine() ?? string.Empty;
            bool ok = admin.AddIntervalLucru(id, nr, DateTime.Now, DateTime.Now.AddHours(8));
            if (ok)
                Console.WriteLine("Interval adaugat in jurnal.");
            else
                Console.WriteLine("Eroare: Soferul sau Masina nu au fost gasite!");
        }

        public static void AfisareJurnale(List<IntervalLucru> lista)
        {
            foreach (var i in lista)
                Console.WriteLine($"timp start: {i.Start}, sofer: {i.SoferActual.Nume}, masina: {i.MasinaActuala.NumarInmatriculare}");
        }
    }
}