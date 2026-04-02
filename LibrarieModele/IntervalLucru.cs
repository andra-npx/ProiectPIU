using System;

namespace LibrarieModele
{
    public enum TipCursa
    {
        FaraCursa = 0,
        Locala = 1,
        Nationala = 2,
        Internationala = 3,
        Speciala = 4
    }

    public enum StareInterval
    {
        Inexistent = 0,
        Programat = 1,
        InDesfasurare = 2,
        Finalizat = 3,
        Anulat = 4
    }
    public class IntervalLucru
    {
        public Sofer SoferActual {  get; set; }
        public Masina MasinaActuala { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public TipCursa Tip { get; set; }
        public StareInterval Stare { get; set; }
        public IntervalLucru()
        {
            SoferActual = new Sofer();   
            MasinaActuala = new Masina(); 
            Start = DateTime.Now;
            Stop = DateTime.Now;
            Tip = TipCursa.FaraCursa;
            Stare = StareInterval.Inexistent;
        }
        public IntervalLucru(Sofer sofer, Masina masina, DateTime start, DateTime stop)
        {
            SoferActual = sofer;
            MasinaActuala = masina;
            Start = start;
            Stop = stop;
            Tip = TipCursa.Locala;
            Stare = StareInterval.Programat;
        }
    }
}
