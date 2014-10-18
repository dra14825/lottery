using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WienerLinien.modelle
{
    class ModellLinie
    {
        private String name;
        private int linNr;
        private Boolean fehler;
        private wienerlinienEntities db;
        private List<String> stationen;

        private DelegateCommand abfragenCommand;
        

        public ModellLinie()
        {
            //Name = n;
            db = new wienerlinienEntities();
            stationen = new List<String>();
            fehler = false;
        }

        public String Name
        {
            get { return this.name; }
            set
            {
                if (value != null)
                { this.name = value; }
            }
        }

        public Boolean Fehler
        {
            get { return this.fehler; }
            set { this.fehler = value; }
        }

        public ICommand AbfragenCommand
        {
            get
            {
                if (abfragenCommand == null)
                {
                    abfragenCommand = new DelegateCommand(linienNr);
                }
                return abfragenCommand; ;
            }
        }

        public void linienNr(Object param)
        {
            Name = (String)param;

            var l = from lin in db.wienerlinien_ogd_linien
                    where lin.l_bezeichnung.Equals(Name)
                    select lin.l_id;
            try
            {
                linNr = (int)l.First();
            }
            catch(Exception e)
            {
                Fehler = true;
                MessageBox.Show("Diese Linie gibt es nicht!");
                return;
            }
            var steig = from st in db.wienerlinien_ogd_steige
                    where st.st_l_linienid == linNr
                    select st.st_h_haltestellenid;

            foreach(var halt in steig)
            {
                
                var h = from ha in db.wienerlinien_ogd_haltestellen
                        where ha.h_id == halt
                        select ha.h_name;

                stationen.Add(h.First());
            }

            String aus = Ausgabe();
            MessageBox.Show(aus);
        }

        public String Ausgabe()
        {
            String a = "";

            for(int i = 0; i < stationen.Count(); i++)
            {
                if(i < stationen.Count()/2)
                a += stationen[i] + "\n";
            }

            return a;

        }

    }
}
