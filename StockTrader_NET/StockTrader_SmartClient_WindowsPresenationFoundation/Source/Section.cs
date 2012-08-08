using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Media.Effects;

namespace StockTrader
{
    public class Section : UserControl
    {
        public event EventHandler ChangeParents;

        public void Cleanup()
        {
            if (ChangeParents != null)
                ChangeParents(this, null);
        }

        public virtual void onSectionReady()
        {
        }

        public virtual void onSectionRemove()
        {
        }
    }
}
