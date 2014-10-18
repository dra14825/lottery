using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WienerLinien.modelle
{
    class ModellAbout
    {
        private AboutWindow aw;
        private DelegateCommand aboutcommand;

        public ModellAbout()
        {
            aw = new AboutWindow();
        }

        public void Showd(object param)
        {
            aw.ShowDialog();
        }

        public ICommand Aboutcommand
        {
            get
            {
                if (aboutcommand == null)
                {
                    aboutcommand = new DelegateCommand(Showd);
                }
                return aboutcommand; ;
            }
        }
    }
}
