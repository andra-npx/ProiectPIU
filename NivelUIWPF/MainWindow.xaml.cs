using LibrarieModele;
using NivelStocareData;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NivelUIWPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // ─── Constante pentru validare ────────────────────────────────────
        private const int NUME_MIN_LUNGIME = 2;
        private const int NUME_MAX_LUNGIME = 50;
        private const int AN_MIN_FABRICATIE = 1900;
        private const int AN_MAX_FABRICATIE = 2025;
        private const double RULAJ_MIN = 0;
        private const double RULAJ_MAX = 2_000_000;
        private const int NR_INMATR_MIN_LUNGIME = 4;
        private const int NR_INMATR_MAX_LUNGIME = 10;
        private const double KM_MIN = 0.1;
        private const double KM_MAX = 100_000;
        private const int RUTA_MIN_LUNGIME = 2;

        // ─── Brushes refolosite ───────────────────────────────────────────
        private static readonly SolidColorBrush BrushEroare = new SolidColorBrush(Color.FromRgb(229, 62, 62));
        private static readonly SolidColorBrush BrushValid = new SolidColorBrush(Color.FromRgb(56, 161, 105));
        private static readonly SolidColorBrush BrushNormal = new SolidColorBrush(Color.FromRgb(74, 85, 104));

        // ─── Date & stare ─────────────────────────────────────────────────
        IStocareData admin = new AdministrareTransportFisierText("in.txt");

        private bool _editareMasina = false; // true = modi­ficam, false = adaugam
        private bool _ignoraSchimbareCategorii = false; // guard pentru reentranta in SelectionChanged

        public ObservableCollection<Sofer> ListaSoferi { get; set; }
        public ObservableCollection<Sofer> ListaSoferiAfisata { get; set; }
        public ObservableCollection<Masina> ListaMasini { get; set; }
        public ObservableCollection<Masina> ListaMasiniAfisata { get; set; }
        public ObservableCollection<IntervalLucru> ListaJurnale { get; set; }

        private Sofer _soferCurent = new Sofer();
        private Masina _masinaCurenta = new Masina();

        public Sofer SoferCurent
        {
            get => _soferCurent;
            set { _soferCurent = value; OnPropertyChanged(nameof(SoferCurent)); }
        }
        public Masina MasinaCurenta
        {
            get => _masinaCurenta;
            set { _masinaCurenta = value; OnPropertyChanged(nameof(MasinaCurenta)); }
        }

        public Sofer SoferSelectatGrid { get; set; }
        public Masina MasinaSelectataGrid { get; set; }

        public Array Culori => Enum.GetValues(typeof(CuloareMasina));
        public Array TipuriCursa => Enum.GetValues(typeof(TipCursa));
        public Array StariCursa => Enum.GetValues(typeof(StareInterval));

        public Sofer JurnalSofer { get; set; }
        public Masina JurnalMasina { get; set; }
        public TipCursa JurnalTip { get; set; }
        public StareInterval JurnalStare { get; set; }

        // Radio buttons experienta
        public bool ExpFara { get; set; } = true;
        public bool ExpIncepator { get; set; }
        public bool ExpMediu { get; set; }
        public bool ExpAvansat { get; set; }
        public bool ExpExpert { get; set; }

        // ─── Constructor ─────────────────────────────────────────────────
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            // Încarcă datele doar după ce fereastra este gata afișată
            this.Loaded += (s, e) => LoadData();
        }

        // ─── Incarcare date ───────────────────────────────────────────────
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

            // Actualizare contoare
            if (txtNrSoferi != null)
                txtNrSoferi.Text = $"{ListaSoferiAfisata.Count} înregistrări";
            if (txtNrMasini != null)
                txtNrMasini.Text = $"{ListaMasiniAfisata.Count} înregistrări";

            SetStatus($"Date reîncărcate. {soferi.Count} șoferi, {masini.Count} mașini.");
        }

        // ═════════════════════════════════════════════════════════════════
        // VALIDARE – metode helper
        // ═════════════════════════════════════════════════════════════════

        /// <summary>
        /// Setează culoarea Label-ului și mesajul de eroare.
        /// </summary>
        private void SetValidare(Label label, TextBox textBox, TextBlock eroare,
                                 bool valid, string mesajEroare = "")
        {
            if (valid)
            {
                label.Foreground = BrushValid;
                textBox.Style = (Style)FindResource("TextBoxValid");
                eroare.Text = "";
            }
            else
            {
                label.Foreground = BrushEroare;
                textBox.Style = (Style)FindResource("TextBoxEroare");
                eroare.Text = mesajEroare;
            }
        }

        private void ResetValidare(Label label, TextBox textBox, TextBlock eroare)
        {
            label.Foreground = BrushNormal;
            textBox.Style = (Style)FindResource(typeof(TextBox));  // style default
            eroare.Text = "";
        }

        // ─── Validari sofer ───────────────────────────────────────────────
        private bool ValidareNumeSofer()
        {
            string v = txtNumeSofer.Text.Trim();
            bool ok = v.Length >= NUME_MIN_LUNGIME && v.Length <= NUME_MAX_LUNGIME;
            SetValidare(lblNumeSofer, txtNumeSofer, errNumeSofer, ok,
                        $"Numele trebuie să aibă între {NUME_MIN_LUNGIME} și {NUME_MAX_LUNGIME} caractere.");
            return ok;
        }

        private bool ValidarePrenumeSofer()
        {
            string v = txtPrenumeSofer.Text.Trim();
            bool ok = v.Length >= NUME_MIN_LUNGIME && v.Length <= NUME_MAX_LUNGIME;
            SetValidare(lblPrenumeSofer, txtPrenumeSofer, errPrenumeSofer, ok,
                        $"Prenumele trebuie să aibă între {NUME_MIN_LUNGIME} și {NUME_MAX_LUNGIME} caractere.");
            return ok;
        }

        private bool ValidareRuta()
        {
            string v = txtRutaNoua.Text.Trim();
            bool ok = v.Length >= RUTA_MIN_LUNGIME;
            SetValidare(lblRuta, txtRutaNoua, errRuta, ok,
                        $"Ruta trebuie să aibă cel puțin {RUTA_MIN_LUNGIME} caractere.");
            return ok;
        }

        private bool ValidareKm()
        {
            bool ok = double.TryParse(txtKmNoi.Text, out double km)
                      && km >= KM_MIN && km <= KM_MAX;
            SetValidare(lblKm, txtKmNoi, errKm, ok,
                        $"KM trebuie să fie un număr între {KM_MIN} și {KM_MAX}.");
            return ok;
        }

        // ─── Validari masina ──────────────────────────────────────────────
        private bool ValidareNrInmatr()
        {
            string v = txtNrInmatr.Text.Trim();
            bool ok = v.Length >= NR_INMATR_MIN_LUNGIME && v.Length <= NR_INMATR_MAX_LUNGIME;
            SetValidare(lblNrInmatr, txtNrInmatr, errNrInmatr, ok,
                        $"Nr. înmatriculare: {NR_INMATR_MIN_LUNGIME}–{NR_INMATR_MAX_LUNGIME} caractere.");
            return ok;
        }

        private bool ValidareMarca()
        {
            string v = txtMarca.Text.Trim();
            bool ok = v.Length >= NUME_MIN_LUNGIME && v.Length <= NUME_MAX_LUNGIME;
            SetValidare(lblMarca, txtMarca, errMarca, ok,
                        $"Marca trebuie să aibă între {NUME_MIN_LUNGIME} și {NUME_MAX_LUNGIME} caractere.");
            return ok;
        }

        private bool ValidareModel()
        {
            string v = txtModel.Text.Trim();
            bool ok = v.Length >= NUME_MIN_LUNGIME && v.Length <= NUME_MAX_LUNGIME;
            SetValidare(lblModel, txtModel, errModel, ok,
                        $"Modelul trebuie să aibă între {NUME_MIN_LUNGIME} și {NUME_MAX_LUNGIME} caractere.");
            return ok;
        }

        private bool ValidareAn()
        {
            bool ok = int.TryParse(txtAn.Text, out int an)
                      && an >= AN_MIN_FABRICATIE && an <= AN_MAX_FABRICATIE;
            SetValidare(lblAn, txtAn, errAn, ok,
                        $"Anul trebuie să fie între {AN_MIN_FABRICATIE} și {AN_MAX_FABRICATIE}.");
            return ok;
        }

        private bool ValidareRulaj()
        {
            bool ok = double.TryParse(txtRulaj.Text, out double r)
                      && r >= RULAJ_MIN && r <= RULAJ_MAX;
            SetValidare(lblRulaj, txtRulaj, errRulaj, ok,
                        $"Rulajul trebuie să fie între {RULAJ_MIN} și {RULAJ_MAX} km.");
            return ok;
        }

        private bool ValidareMasinaFormular()
            => ValidareNrInmatr() & ValidareMarca() & ValidareModel()
               & ValidareAn() & ValidareRulaj();

        // ═════════════════════════════════════════════════════════════════
        // GESTIUNE ȘOFERI
        // ═════════════════════════════════════════════════════════════════

        private void TxtNumeSofer_TextChanged(object s, TextChangedEventArgs e) => ValidareNumeSofer();
        private void TxtPrenumeSofer_TextChanged(object s, TextChangedEventArgs e) => ValidarePrenumeSofer();
        private void TxtRutaNoua_TextChanged(object s, TextChangedEventArgs e) => ValidareRuta();
        private void TxtKmNoi_TextChanged(object s, TextChangedEventArgs e) => ValidareKm();

        private void LstCategorii_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_ignoraSchimbareCategorii) return;

            // Dacă e selectat "Toate", debifăm restul și selectăm doar "Toate"
            bool toateSelectat = lstCategorii.SelectedItems.Cast<ListBoxItem>()
                                             .Any(i => i.Tag?.ToString() == "Toate");
            if (toateSelectat)
            {
                _ignoraSchimbareCategorii = true;
                try
                {
                    var toateItem = lstCategorii.Items.Cast<ListBoxItem>()
                                                .First(i => i.Tag?.ToString() == "Toate");
                    lstCategorii.SelectedItems.Clear();
                    lstCategorii.SelectedItems.Add(toateItem);
                }
                finally
                {
                    _ignoraSchimbareCategorii = false;
                }
            }
        }

        private CategoriiPermis GetCategoriiSelectate()
        {
            CategoriiPermis cat = CategoriiPermis.Niciuna;
            foreach (ListBoxItem item in lstCategorii.SelectedItems)
            {
                switch (item.Tag?.ToString())
                {
                    case "B": cat |= CategoriiPermis.B; break;
                    case "C": cat |= CategoriiPermis.C; break;
                    case "D": cat |= CategoriiPermis.D; break;
                    case "E": cat |= CategoriiPermis.E; break;
                    case "Toate": cat = CategoriiPermis.Toate; break;
                }
            }
            return cat;
        }

        private NivelExperienta GetExperienta()
        {
            if (ExpExpert) return NivelExperienta.Expert;
            if (ExpAvansat) return NivelExperienta.Avansat;
            if (ExpMediu) return NivelExperienta.Mediu;
            if (ExpIncepator) return NivelExperienta.Incepator;
            return NivelExperienta.FaraExperienta;
        }

        // CREATE șofer
        private void BtnSalveazaSofer_Click(object sender, RoutedEventArgs e)
        {
            bool numeOk = ValidareNumeSofer();
            bool prenumeOk = ValidarePrenumeSofer();
            if (!numeOk || !prenumeOk)
            {
                SetStatus("❌ Corectați erorile de validare înainte de salvare.");
                return;
            }

            SoferCurent.Experienta = GetExperienta();
            SoferCurent.Categorii = GetCategoriiSelectate();
            admin.AddSofer(SoferCurent);

            SetStatus($"✅ Șoferul '{SoferCurent.Nume} {SoferCurent.Prenume}' a fost salvat.");
            ResetFormularSofer();
            LoadData();
        }

        private void BtnResetSofer_Click(object sender, RoutedEventArgs e) => ResetFormularSofer();

        private void ResetFormularSofer()
        {
            SoferCurent = new Sofer();
            ExpFara = true; ExpIncepator = false; ExpMediu = false;
            ExpAvansat = false; ExpExpert = false;
            lstCategorii.SelectedItems.Clear();
            dtpDataAngajare.SelectedDate = null;
            ResetValidare(lblNumeSofer, txtNumeSofer, errNumeSofer);
            ResetValidare(lblPrenumeSofer, txtPrenumeSofer, errPrenumeSofer);
            OnPropertyChanged("");
        }

        private void TxtCautSofer_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = txtCautSofer.Text.ToLower();
            var filtered = ListaSoferi
                .Where(s => (s.Nume + " " + s.Prenume).ToLower().Contains(search))
                .ToList();
            ListaSoferiAfisata.Clear();
            foreach (var s in filtered) ListaSoferiAfisata.Add(s);
            txtNrSoferi.Text = $"{ListaSoferiAfisata.Count} înregistrări";
        }

        // UPDATE șofer (adauga traseu + km)
        private void BtnModificaSofer_Click(object sender, RoutedEventArgs e)
        {
            if (SoferSelectatGrid == null)
            {
                SetStatus("⚠️ Selectați un șofer din tabel.");
                return;
            }
            bool rutaOk = ValidareRuta();
            bool kmOk = ValidareKm();
            if (!rutaOk || !kmOk) { SetStatus("❌ Corectați erorile."); return; }

            double.TryParse(txtKmNoi.Text, out double km);
            Masina m = cmbMasinaTraseu.SelectedItem as Masina ?? ListaMasini.FirstOrDefault();
            admin.ModificaDateSofer(SoferSelectatGrid.IdSofer, txtRutaNoua.Text, km, m);

            txtRutaNoua.Text = "";
            txtKmNoi.Text = "";
            cmbMasinaTraseu.SelectedIndex = -1;
            ResetValidare(lblRuta, txtRutaNoua, errRuta);
            ResetValidare(lblKm, txtKmNoi, errKm);
            SetStatus($"✅ Datele șoferului ID {SoferSelectatGrid.IdSofer} au fost actualizate.");
            LoadData();
        }

        // DELETE șofer
        private void BtnStergeSofer_Click(object sender, RoutedEventArgs e)
        {
            if (SoferSelectatGrid == null)
            {
                SetStatus("⚠️ Selectați un șofer din tabel.");
                return;
            }
            var r = MessageBox.Show(
                $"Sigur doriți să ștergeți șoferul '{SoferSelectatGrid.Nume} {SoferSelectatGrid.Prenume}'?",
                "Confirmare ștergere", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (r == MessageBoxResult.Yes)
            {
                admin.StergeSofer(SoferSelectatGrid.IdSofer);
                SetStatus($"🗑️ Șoferul a fost șters.");
                SoferSelectatGrid = null;
                txtSoferSelectatInfo.Text = "Selectați un șofer din tabel →";
                LoadData();
            }
        }

        // Selectie sofer in DataGrid
        private void GridSoferi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SoferSelectatGrid = gridSoferi.SelectedItem as Sofer;
            if (SoferSelectatGrid != null)
                txtSoferSelectatInfo.Text = $"✏️ Editare: {SoferSelectatGrid.Nume} {SoferSelectatGrid.Prenume} (ID: {SoferSelectatGrid.IdSofer})";
            else
                txtSoferSelectatInfo.Text = "Selectați un șofer din tabel →";
        }

        // ═════════════════════════════════════════════════════════════════
        // GESTIUNE MAȘINI  (CRUD COMPLET)
        // ═════════════════════════════════════════════════════════════════

        private void TxtNrInmatr_TextChanged(object s, TextChangedEventArgs e) => ValidareNrInmatr();
        private void TxtMarca_TextChanged(object s, TextChangedEventArgs e) => ValidareMarca();
        private void TxtModel_TextChanged(object s, TextChangedEventArgs e) => ValidareModel();
        private void TxtAn_TextChanged(object s, TextChangedEventArgs e) => ValidareAn();
        private void TxtRulaj_TextChanged(object s, TextChangedEventArgs e) => ValidareRulaj();

        private void LstOptiuni_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Actualizare vizuala (calcul se face la salvare)
        }

        private OptiuniMasina GetOptiuniSelectate()
        {
            OptiuniMasina opt = OptiuniMasina.Niciuna;
            foreach (ListBoxItem item in lstOptiuni.SelectedItems)
            {
                switch (item.Tag?.ToString())
                {
                    case "AerConditionat": opt |= OptiuniMasina.AerConditionat; break;
                    case "Navigatie": opt |= OptiuniMasina.Navigatie; break;
                    case "CutieAutomata": opt |= OptiuniMasina.CutieAutomata; break;
                    case "ScauneIncalzite": opt |= OptiuniMasina.ScauneIncalzite; break;
                    case "SenzoriParcare": opt |= OptiuniMasina.SenzoriParcare; break;
                }
            }
            return opt;
        }

        // CREATE sau UPDATE mașină (același buton, mod controlat prin _editareMasina)
        private void BtnSalveazaMasina_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidareMasinaFormular())
            {
                SetStatus("❌ Corectați erorile de validare înainte de salvare.");
                return;
            }

            MasinaCurenta.Optiuni = GetOptiuniSelectate();
            MasinaCurenta.DataAchizitie = dtpDataAchizitie.SelectedDate;

            if (_editareMasina)
            {
                // UPDATE
                bool ok = admin.ModificaMasina(MasinaCurenta);
                SetStatus(ok
                    ? $"✅ Mașina '{MasinaCurenta.NumarInmatriculare}' a fost actualizată."
                    : "❌ Eroare la actualizare – mașina nu a fost găsită.");
            }
            else
            {
                // CREATE
                admin.AddMasina(MasinaCurenta);
                SetStatus($"✅ Mașina '{MasinaCurenta.NumarInmatriculare}' a fost salvată.");
            }

            ResetFormularMasina();
            LoadData();
        }

        private void BtnResetMasina_Click(object sender, RoutedEventArgs e) => ResetFormularMasina();

        // DELETE mașină
        private void BtnStergeMasina_Click(object sender, RoutedEventArgs e)
        {
            if (MasinaSelectataGrid == null)
            {
                SetStatus("⚠️ Selectați o mașină din tabel.");
                return;
            }
            var r = MessageBox.Show(
                $"Sigur doriți să ștergeți mașina '{MasinaSelectataGrid.NumarInmatriculare}'?",
                "Confirmare ștergere", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (r == MessageBoxResult.Yes)
            {
                admin.StergeMasina(MasinaSelectataGrid.IdMasina);
                SetStatus($"🗑️ Mașina '{MasinaSelectataGrid.NumarInmatriculare}' a fost ștearsă.");
                MasinaSelectataGrid = null;
                ResetFormularMasina();
                LoadData();
            }
        }

        // READ – selectie in DataGrid → populare formular (pregătire pentru UPDATE)
        private void GridMasini_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MasinaSelectataGrid = gridMasini.SelectedItem as Masina;
            if (MasinaSelectataGrid == null) return;

            // Copiem datele în formular
            MasinaCurenta = new Masina
            {
                IdMasina = MasinaSelectataGrid.IdMasina,
                NumarInmatriculare = MasinaSelectataGrid.NumarInmatriculare,
                Marca = MasinaSelectataGrid.Marca,
                Model = MasinaSelectataGrid.Model,
                An = MasinaSelectataGrid.An,
                Rulaj = MasinaSelectataGrid.Rulaj,
                Culoare = MasinaSelectataGrid.Culoare,
                Optiuni = MasinaSelectataGrid.Optiuni,
                DataAchizitie = MasinaSelectataGrid.DataAchizitie
            };
            OnPropertyChanged(nameof(MasinaCurenta));

            // Populam ListBox optiuni
            lstOptiuni.SelectedItems.Clear();
            foreach (ListBoxItem item in lstOptiuni.Items)
            {
                string tag = item.Tag?.ToString() ?? "";
                bool selectat = tag switch
                {
                    "AerConditionat" => MasinaCurenta.Optiuni.HasFlag(OptiuniMasina.AerConditionat),
                    "Navigatie" => MasinaCurenta.Optiuni.HasFlag(OptiuniMasina.Navigatie),
                    "CutieAutomata" => MasinaCurenta.Optiuni.HasFlag(OptiuniMasina.CutieAutomata),
                    "ScauneIncalzite" => MasinaCurenta.Optiuni.HasFlag(OptiuniMasina.ScauneIncalzite),
                    "SenzoriParcare" => MasinaCurenta.Optiuni.HasFlag(OptiuniMasina.SenzoriParcare),
                    _ => false
                };
                if (selectat) lstOptiuni.SelectedItems.Add(item);
            }

            dtpDataAchizitie.SelectedDate = MasinaCurenta.DataAchizitie;

            // Schimbam modul in editare
            _editareMasina = true;
            txtTitluFormularMasina.Text = "✏️ Modificare Mașină";
            btnSalveazaMasina.Content = "💾 Salvează Modificările";
            btnSalveazaMasina.Background = new SolidColorBrush(Color.FromRgb(56, 161, 105));
            btnStergeMasina.Visibility = Visibility.Visible;

            SetStatus($"📝 Modificare mașină: {MasinaSelectataGrid.NumarInmatriculare}");
        }

        private void ResetFormularMasina()
        {
            MasinaCurenta = new Masina();
            _editareMasina = false;
            lstOptiuni.SelectedItems.Clear();
            dtpDataAchizitie.SelectedDate = null;

            txtTitluFormularMasina.Text = "Adăugare Mașină Nouă";
            btnSalveazaMasina.Content = "💾 Salvează";
            btnSalveazaMasina.Background = new SolidColorBrush(Color.FromRgb(221, 107, 32));
            btnStergeMasina.Visibility = Visibility.Collapsed;

            ResetValidare(lblNrInmatr, txtNrInmatr, errNrInmatr);
            ResetValidare(lblMarca, txtMarca, errMarca);
            ResetValidare(lblModel, txtModel, errModel);
            ResetValidare(lblAn, txtAn, errAn);
            ResetValidare(lblRulaj, txtRulaj, errRulaj);
            OnPropertyChanged(nameof(MasinaCurenta));
        }

        private void TxtCautMasina_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = txtCautMasina.Text.ToLower();
            var filtered = ListaMasini
                .Where(m => m.NumarInmatriculare.ToLower().Contains(search)
                         || m.Marca.ToLower().Contains(search)
                         || m.Model.ToLower().Contains(search))
                .ToList();
            ListaMasiniAfisata.Clear();
            foreach (var m in filtered) ListaMasiniAfisata.Add(m);
            txtNrMasini.Text = $"{ListaMasiniAfisata.Count} înregistrări";
        }

        // ═════════════════════════════════════════════════════════════════
        // GESTIUNE JURNAL
        // ═════════════════════════════════════════════════════════════════

        private void BtnAdaugaJurnal_Click(object sender, RoutedEventArgs e)
        {
            if (JurnalSofer == null || JurnalMasina == null)
            {
                SetStatus("⚠️ Selectați șoferul și mașina pentru jurnal.");
                return;
            }
            admin.AddIntervalLucru(JurnalSofer.IdSofer, JurnalMasina.NumarInmatriculare,
                                   DateTime.Now, DateTime.Now.AddHours(8),
                                   JurnalTip, JurnalStare);
            SetStatus("✅ Intrare jurnal adăugată.");
            LoadData();
        }

        private void BtnStergeJurnal_Click(object sender, RoutedEventArgs e)
        {
            if (gridJurnale.SelectedItem is IntervalLucru interval)
            {
                var r = MessageBox.Show("Sigur doriți să ștergeți această intrare din jurnal?",
                    "Confirmare", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (r == MessageBoxResult.Yes)
                {
                    admin.StergeIntervalLucru(interval);
                    SetStatus("🗑️ Intrarea din jurnal a fost ștearsă.");
                    LoadData();
                }
            }
            else
                SetStatus("⚠️ Selectați o intrare din jurnal.");
        }

        // ═════════════════════════════════════════════════════════════════
        // HANDLERS MENIU
        // ═════════════════════════════════════════════════════════════════

        private void MenuReincarcaDate_Click(object s, RoutedEventArgs e) => LoadData();
        private void MenuIesire_Click(object s, RoutedEventArgs e) => Application.Current.Shutdown();

        private void MenuAdaugaSofer_Click(object s, RoutedEventArgs e)
        {
            // Navigam pe tabul Soferi si resetam formularul
            ((TabControl)((DockPanel)Content).Children.OfType<TabControl>().First()).SelectedIndex = 0;
            ResetFormularSofer();
            txtNumeSofer.Focus();
        }

        private void MenuModificaSofer_Click(object s, RoutedEventArgs e)
        {
            if (SoferSelectatGrid == null)
                MessageBox.Show("Selectați un șofer din tabel.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                txtRutaNoua.Focus();
        }

        private void MenuStergeSofer_Click(object s, RoutedEventArgs e) => BtnStergeSofer_Click(s, e);
        private void MenuCautaSofer_Click(object s, RoutedEventArgs e) => txtCautSofer.Focus();

        private void MenuAdaugaMasina_Click(object s, RoutedEventArgs e)
        {
            ((TabControl)((DockPanel)Content).Children.OfType<TabControl>().First()).SelectedIndex = 1;
            ResetFormularMasina();
            txtNrInmatr.Focus();
        }

        private void MenuModificaMasina_Click(object s, RoutedEventArgs e)
        {
            if (MasinaSelectataGrid == null)
                MessageBox.Show("Selectați o mașină din tabel.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuStergeMasina_Click(object s, RoutedEventArgs e) => BtnStergeMasina_Click(s, e);
        private void MenuCautaMasina_Click(object s, RoutedEventArgs e) => txtCautMasina.Focus();

        private void MenuAdaugaJurnal_Click(object s, RoutedEventArgs e)
        {
            ((TabControl)((DockPanel)Content).Children.OfType<TabControl>().First()).SelectedIndex = 2;
        }

        private void MenuStergeJurnal_Click(object s, RoutedEventArgs e) => BtnStergeJurnal_Click(s, e);

        private void MenuDespre_Click(object s, RoutedEventArgs e)
            => MessageBox.Show("Sistem Gestiune Transport v2.0\n\nCRUD complet pentru Șoferi și Mașini.\nValidare cu culori, DatePicker, ListBox, meniu complet.",
                               "Despre", MessageBoxButton.OK, MessageBoxImage.Information);

        // ─── Status bar helper ────────────────────────────────────────────
        private void SetStatus(string msg) => txtStatus.Text = msg;

        // ─── INotifyPropertyChanged ───────────────────────────────────────
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}