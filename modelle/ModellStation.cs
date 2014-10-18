using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;

using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WienerLinien.modelle
{
    class ModellStation : INotifyPropertyChanged
    {

        wienerlinienEntities db;

        private DelegateCommand abfragenCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        private String stati;
        private Station station;
        private List<Steig> ergebnis;

        private String north;
        private String ost;
        private BitmapImage map;
        private Visibility VtbLinien;
        private Visibility VtbAbfahrtszeiten;
        private Visibility VtbAbfahrtszeitenausgabe;
        private Visibility VlbLinien;
        private Visibility VloadingAnimation;
        private Visibility Vborder;
        private Visibility VscrollViewer;
        private String text;
        public ModellStation()
        {
            db = new wienerlinienEntities();
            ergebnis = new List<Steig>();
        }
        public String loadingTextBlock
        {
            get { return text; }
            set { text = value; }
        }
        public String Stati
        {
            get { return this.stati; }
            set { this.stati = value; }
        }

        public Visibility tbLinien
        {
            get { return VtbLinien; }
            set { VtbLinien = value; }
        }
        public Visibility tbAbfahrtszeiten
        {
            get { return VtbAbfahrtszeiten; }
            set { VtbAbfahrtszeiten = value; }
        }
        public Visibility tbAbfahrtszeitenausgabe
        {
            get { return VtbAbfahrtszeitenausgabe; }
            set { VtbAbfahrtszeitenausgabe = value; }
        }
        public Visibility lbLinie
        {
            get { return VlbLinien; }
            set { VlbLinien = value; }
        }
        public Visibility loadingAnimation
        {
            get { return VloadingAnimation; }
            set { VloadingAnimation = value; }
        }
        public Visibility border
        {
            get { return Vborder; }
            set { Vborder = value; }
        }
        public Visibility scrollViewer
        {
            get { return VscrollViewer; }
            set { VscrollViewer = value; }
        }


        public List<Steig> Ergebnis
        {
            get { return this.ergebnis; }

        }

        public BitmapImage Map
        {
            get { return this.map; }
            set { map = value; }
        }

        public ICommand AbfragenCommand
        {
            get
            {
                if (abfragenCommand == null)
                {
                    abfragenCommand = new DelegateCommand(AbfrageStation);
                }
                return abfragenCommand; ;
            }
        }

        public void AbfrageStation(object param)
        {
            String st = (String)param;
            notVisible();
            station = new Station(st);
            station.sucheHaltId();
            station.sucheSteige();

            Abfrage abfrage = new Abfrage();
            abfrage.ReadyEvent += verarbeitung_Event;
            abfrage.AbfrageHaltestelle(station);
        }


        private void verarbeitung_Event(object sender, ErgebnisEvent e)
        {
            List<String> ergebnisJSON = e.ergebnisBack;

            JSON json;
            ergebnis = new List<Steig>();

            foreach (var x in ergebnisJSON)
            {
                json = new JSON(x);

                List<Steig> list = json.jsonumwandeln();

                foreach (var i in list)
                {
                    ergebnis.Add(i);
                    Console.WriteLine(i.NameSteig);

                }

                //für einmalige Linieninfo benötigt
                ergebnis.Sort(delegate(Steig xSteig, Steig ySteig)
                { return xSteig.NameSteig.CompareTo(ySteig.NameSteig); });
            }


            north = ergebnis[0].CoordNorth;
            ost = ergebnis[0].CoordOst;

            for (int ind = 0; ind < ergebnis.Count(); ind++)
            {
                if (ind + 1 < ergebnis.Count())
                {
                    if (ergebnis[ind].NameSteig.Equals(ergebnis[ind + 1].NameSteig))
                    {
                        ergebnis[ind].Ausgabe = ergebnis[ind].Ausgabe + "\n" + ergebnis[ind + 1].Ausgabe;
                        ergebnis.Remove(ergebnis[ind + 1]);
                    }
                }
            }

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Map = new BitmapImage(new Uri("http://maps.googleapis.com/maps/api/staticmap?size=300x300&scale=4&zoom=17&markers=%20icon:http://chart.apis.google.com/chart?chst=d_map_pin_icon%26chld=cafe%257C996600|" + ost + "," + north + "|&sensor=true"))));
            Visible();
            PropertyChanged(this, new PropertyChangedEventArgs("Ergebnis"));
            PropertyChanged(this, new PropertyChangedEventArgs("Map"));
        }

        public void notVisible()
        {
            tbAbfahrtszeiten = System.Windows.Visibility.Hidden;
            tbAbfahrtszeitenausgabe = System.Windows.Visibility.Hidden;
            tbLinien = System.Windows.Visibility.Hidden;
            lbLinie = System.Windows.Visibility.Hidden;
            border = System.Windows.Visibility.Hidden;
            scrollViewer = System.Windows.Visibility.Hidden;
            //loadingAnimation = System.Windows.Visibility.Visible;
            loadingTextBlock = "Loading...";
            PropertyChanged(this, new PropertyChangedEventArgs("tbAbfahrtszeiten"));
            PropertyChanged(this, new PropertyChangedEventArgs("tbAbfahrtszeitenausgabe"));
            PropertyChanged(this, new PropertyChangedEventArgs("tbLinien"));
            PropertyChanged(this, new PropertyChangedEventArgs("lbLinie"));
            PropertyChanged(this, new PropertyChangedEventArgs("border"));
            PropertyChanged(this, new PropertyChangedEventArgs("scrollViewer"));
            PropertyChanged(this, new PropertyChangedEventArgs("loadingTextBlock"));
            //PropertyChanged(this, new PropertyChangedEventArgs("loadingAnimation"));;
        }

        public void Visible()
        {
            tbAbfahrtszeiten = System.Windows.Visibility.Visible;
            tbAbfahrtszeitenausgabe = System.Windows.Visibility.Visible;
            tbLinien = System.Windows.Visibility.Visible;
            lbLinie = System.Windows.Visibility.Visible;
            border = System.Windows.Visibility.Visible;
            scrollViewer = System.Windows.Visibility.Visible;
            //loadingAnimation = System.Windows.Visibility.Hidden;
            loadingTextBlock = "";
            PropertyChanged(this, new PropertyChangedEventArgs("tbAbfahrtszeiten"));
            PropertyChanged(this, new PropertyChangedEventArgs("tbAbfahrtszeitenausgabe"));
            PropertyChanged(this, new PropertyChangedEventArgs("tbLinien"));
            PropertyChanged(this, new PropertyChangedEventArgs("lbLinie"));
            PropertyChanged(this, new PropertyChangedEventArgs("border"));
            PropertyChanged(this, new PropertyChangedEventArgs("scrollViewer"));
            PropertyChanged(this, new PropertyChangedEventArgs("loadingTextBlock"));
            //PropertyChanged(this, new PropertyChangedEventArgs("loadingAnimation"));
        }



    }
}
