using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LibrarieModele;
using NivelStocareData;
using System.ComponentModel;

namespace NivelUIWPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        IStocareData admin = new AdministrareTransportFisierText("in.txt");

        public ObservableCollection<Sofer> ListaSoferi { get; set; }
        public ObservableCollection<Sofer> ListaSoferiAfisata { get; set; }
        public ObservableCollection<Masina> ListaMasini { get; set; }
        public ObservableCollection<Masina> ListaMasiniAfisata { get; set; }
        public ObservableCollection<IntervalLucru> ListaJurnale { get; set; }

        public Sofer SoferCurent { get; set; } = new Sofer();
        public Masina MasinaCurenta { get; set; } = new Masina();
        public Sofer SoferSelectatGrid { get; set; }

        public Array Culori => Enum.GetValues(typeof(CuloareMasina));
        public Array TipuriCursa => Enum.GetValues(typeof(TipCursa));
        public Array StariCursa => Enum.GetValues(typeof(StareInterval));

        public Sofer JurnalSofer { get; set; }
        public Masina JurnalMasina { get; set; }
        public TipCursa JurnalTip { get; set; }
        public StareInterval JurnalStare { get; set; }

        public bool ExpFara { get; set; } = true;
        public bool ExpIncepator { get; set; }
        public bool ExpMediu { get; set; }
        public bool ExpAvansat { get; set; }
        public bool ExpExpert { get; set; }

        public bool CatB { get; set; }
        public bool CatC { get; set; }
        public bool CatD { get; set; }
        public bool CatE { get; set; }
        public bool CatToate { get; set; }

        public bool OptAC { get; set; }
        public bool OptNavi { get; set; }
        public bool OptAuto { get; set; }
        public bool OptIncalzire { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            this.DataContext = this;
        }

        private void LoadData()
        {
            var soferi = admin.GetListaSoferi();
            var masini = admin.GetListaMasini();
            ListaSoferi = new ObservableCollection<Sofer>(soferi);
            ListaSoferiAfisata = new ObservableCollection<Sofer>(soferi);
            ListaMasini = new ObservableCollection<Masina>(masini);
            ListaMasiniAfisata = new ObservableCollection<Masina>(masini);
            ListaJurnale = new ObservableCollection<IntervalLucru>(admin.GetListaJurnale());
            OnPropertyChanged("");
        }

        // --- GESTIUNE ȘOFERI ---
        private void BtnSalveazaSofer_Click(object sender, RoutedEventArgs e)
        {
            if (ExpExpert) SoferCurent.Experienta = NivelExperienta.Expert;
            else if (ExpAvansat) SoferCurent.Experienta = NivelExperienta.Avansat;
            else if (ExpMediu) SoferCurent.Experienta = NivelExperienta.Mediu;
            else if (ExpIncepator) SoferCurent.Experienta = NivelExperienta.Incepator;

            SoferCurent.Categorii = CategoriiPermis.Niciuna;
            if (CatToate) SoferCurent.Categorii = CategoriiPermis.Toate;
            else
            {
                if (CatB) SoferCurent.Categorii |= CategoriiPermis.B;
                if (CatC) SoferCurent.Categorii |= CategoriiPermis.C;
                if (CatD) SoferCurent.Categorii |= CategoriiPermis.D;
                if (CatE) SoferCurent.Categorii |= CategoriiPermis.E;
            }

            admin.AddSofer(SoferCurent);
            SoferCurent = new Sofer();
            LoadData();
        }

        private void TxtCautSofer_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = txtCautSofer.Text.ToLower();
            var filtered = ListaSoferi.Where(s => (s.Nume + " " + s.Prenume).ToLower().Contains(search)).ToList();
            ListaSoferiAfisata.Clear();
            foreach (var s in filtered) ListaSoferiAfisata.Add(s);
        }

        private void BtnModificaSofer_Click(object sender, RoutedEventArgs e)
        {
            if (SoferSelectatGrid != null && double.TryParse(txtKmNoi.Text, out double km))
            {
                Masina m = cmbMasinaTraseu.SelectedItem as Masina;
                if (m == null) m = ListaMasini.FirstOrDefault();
                admin.ModificaDateSofer(SoferSelectatGrid.IdSofer, txtRutaNoua.Text, km, m);
                LoadData();
            }
        }

        // --- GESTIUNE MAȘINI ---
        private void BtnSalveazaMasina_Click(object sender, RoutedEventArgs e)
        {
            MasinaCurenta.Optiuni = OptiuniMasina.Niciuna;
            if (OptAC) MasinaCurenta.Optiuni |= OptiuniMasina.AerConditionat;
            if (OptNavi) MasinaCurenta.Optiuni |= OptiuniMasina.Navigatie;
            if (OptAuto) MasinaCurenta.Optiuni |= OptiuniMasina.CutieAutomata;
            if (OptIncalzire) MasinaCurenta.Optiuni |= OptiuniMasina.ScauneIncalzite;

            admin.AddMasina(MasinaCurenta);
            MasinaCurenta = new Masina();
            LoadData();
        }

        private void TxtCautMasina_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = txtCautMasina.Text.ToLower();
            var filtered = ListaMasini.Where(m => m.NumarInmatriculare.ToLower().Contains(search)).ToList();
            ListaMasiniAfisata.Clear();
            foreach (var m in filtered) ListaMasiniAfisata.Add(m);
        }

        // --- GESTIUNE JURNAL ---
        private void BtnAdaugaJurnal_Click(object sender, RoutedEventArgs e)
        {
            if (JurnalSofer != null && JurnalMasina != null)
            {
                admin.AddIntervalLucru(JurnalSofer.IdSofer, JurnalMasina.NumarInmatriculare, DateTime.Now, DateTime.Now.AddHours(8), JurnalTip, JurnalStare);
                LoadData();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}