using LibrarieModele;
using System.Collections.Generic;
using System;

namespace NivelStocareData
{
    public interface IStocareData
    {
        void AddSofer(Sofer sofer);
        List<Sofer> GetListaSoferi();
        Sofer GetSofer(string nume, string prenume);
        List<Sofer> GetSoferi(string nume);
        bool ModificaDateSofer(int id, string ruta, double km, Masina masina);

        void AddMasina(Masina masina);
        List<Masina> GetListaMasini();
        Masina GetMasina(string nr);

        void AddIntervalLucru(IntervalLucru i);
        List<IntervalLucru> GetListaJurnale();
        bool AddIntervalLucru(int idSofer, string nr, DateTime start, DateTime stop, TipCursa t, StareInterval s);
    }
}