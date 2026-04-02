namespace LibrarieModele
{
    public class IntervalLucru
    {
        public Sofer SoferActual {  get; set; }
        public Masina MasinaActuala { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public IntervalLucru()
        {
            SoferActual = new Sofer();   
            MasinaActuala = new Masina(); 
            Start = DateTime.Now;
            Stop = DateTime.Now;
        }
        public IntervalLucru(Sofer sofer, Masina masina, DateTime start, DateTime stop)
        {
            SoferActual = sofer;
            MasinaActuala = masina;
            Start = start;
            Stop = stop;
        }
    }
}
