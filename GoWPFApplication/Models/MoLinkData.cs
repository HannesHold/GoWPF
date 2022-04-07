using Northwoods.GoXam.Model;
using System;
using System.Reflection;
using System.Windows.Media;

namespace GoWPFApplication.Models
{
    public class MoLinkData : GraphLinksModelLinkData<string, string>
    {
        #region Constructors

        public MoLinkData()
        {
            Id = Guid.NewGuid();
            LabelNode = Id.ToString();
        }

        #endregion

        #region Properties

        private Guid _id;

        public Guid Id
        {
            get { return _id; }
            private set { _id = value; }
        }

        private bool _isSelected = false;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { if (_isSelected != value) { bool old = _isSelected; _isSelected = value; RaisePropertyChanged("IsSelected", old, value); } }
        }

        #region Link visual

        private string _backColor = "White";

        public string BackColor
        {
            get { return _backColor; }
            set { if (_backColor != value) { string old = _backColor; _backColor = value; RaisePropertyChanged("BackColor", old, value); } }
        }

        private string _foreColor = "Black";

        public string ForeColor
        {
            get { return _foreColor; }
            set { if (_foreColor != value) { string old = _foreColor; _foreColor = value; RaisePropertyChanged("ForeColor", old, value); } }
        }

        #endregion

        #endregion

        #region Methods

        private string RandomBrushString()
        {
            Brush? result = Brushes.Transparent;
            Random rnd = new Random();
            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            int random = rnd.Next(properties.Length);
            result = properties[random].GetValue(null, null) as Brush;

            return result is not null ? result.ToString() : Brushes.DeepPink.ToString();
        }

        public void GenerateLinkVisual()
        {
            BackColor = RandomBrushString();
            ForeColor = RandomBrushString();
        }

        #endregion
    }
}
