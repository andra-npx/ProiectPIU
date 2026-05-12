using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

    public class IntervalLucru : INotifyPropertyChanged
    {
        // Evenimentul necesar pentru notificarea UI-ului (Lab 10) [cite: 923]
        public event PropertyChangedEventHandler PropertyChanged;

        // Metodă pentru invocarea evenimentului (Lab 10) [cite: 950, 953]
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Backing fields pentru proprietățile cu binding [cite: 939]
        private Sofer soferActual;
        private Masina masinaActuala;
        private DateTime start;
        private DateTime stop;
        private TipCursa tip;
        private StareInterval stare;

        public Sofer SoferActual
        {
            get => soferActual;
            set { soferActual = value; OnPropertyChanged(); }
        }

        public Masina MasinaActuala
        {
            get => masinaActuala;
            set { masinaActuala = value; OnPropertyChanged(); }
        }

        public DateTime Start
        {
            get => start;
            set { start = value; OnPropertyChanged(); }
        }

        public DateTime Stop
        {
            get => stop;
            set { stop = value; OnPropertyChanged(); }
        }

        public TipCursa Tip
        {
            get => tip;
            set { tip = value; OnPropertyChanged(); }
        }

        public StareInterval Stare
        {
            get => stare;
            set { stare = value; OnPropertyChanged(); }
        }

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