using LibrarieModele;

namespace Transport
{
    class Program
    {
        static void Main(string[] args)
        {
            Masina m1 = new Masina("SV 12 AXP", "Volvo", "S60", 2004, 200000);
            Sofer s1 = new Sofer("Salvatore", "Stefan");

            Console.WriteLine("--Date initiale--");
            Console.WriteLine($"Masina: {m1.NumarInmatriculare}, {m1.Marca}, {m1.Model}, {m1.An}, {m1.Rulaj}km");
            Console.WriteLine($"Sofer: {s1.Nume} {s1.Prenume}");
            Console.WriteLine();

            DateTime start = DateTime.Now;
            DateTime stop = DateTime.Now.AddHours(8);

            IntervalLucru tura = new IntervalLucru(s1, m1, start, stop);

            s1.AdaugaTraseu("Falticeni - Suceava", 22, m1);
            s1.AdaugaTraseu("Suceava - Bucuresti", 461, m1);

            Console.WriteLine($"Istoric trasee pentru soferul {s1.Nume} {s1.Prenume}");
            foreach (var ruta in s1.Trasee)
            {
                Console.WriteLine(ruta);
            }
            Console.WriteLine();

            Console.WriteLine("--Date masina si sofer dupa inregistrarea curselor--");
            Console.WriteLine($"Masina: {m1.NumarInmatriculare}, {m1.Marca}, {m1.Model}, {m1.An}, {m1.Rulaj}km");
            Console.WriteLine($"Sofer: {s1.Nume} {s1.Prenume}");
            Console.WriteLine();


            Console.WriteLine("Apasa pentru a inchide");
            Console.ReadKey();
        }
    }
}