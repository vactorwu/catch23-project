using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Collections.Generic;

namespace StockTrader
{
    public class RadioGroup
    {
        public RadioButton selected;
        public string name;
    }

	public partial class RadioButton
	{
        private bool _selected;
        private RadioGroup _group;

        private Storyboard Select;
        private Storyboard DeSelect;
        private static Dictionary<string,RadioGroup> Groups =  new Dictionary<string,RadioGroup>();

		public RadioButton()
		{
			this.InitializeComponent();
            Select = (Storyboard)FindResource("Select");
            DeSelect = (Storyboard)FindResource("DeSelect");
            LayoutRoot.MouseDown += new System.Windows.Input.MouseButtonEventHandler(onMouseDown);
		}

        void onMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Selected = true;
        }

        public string Group
        {
            get
            {
                return _group != null ? _group.name : null;
            }
            set
            {
                if (Groups.ContainsKey(value))
                    _group = Groups[value];
                else
                {
                    Groups[value] = _group = new RadioGroup();
                    _group.name = value;
                }
            }
        }

        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                if ((_selected = value))
                {
                    Select.Begin(this);
                    if (_group != null && _group.selected != null && _group.selected != this)
                        _group.selected.Selected = false;
                    _group.selected = this;
                }
                else
                    DeSelect.Begin(this);

            }
        }
	}
}