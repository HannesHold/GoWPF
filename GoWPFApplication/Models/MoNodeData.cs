using Northwoods.GoXam;
using Northwoods.GoXam.Model;
using System;
using System.Reflection;
using System.Windows.Media;

namespace GoWPFApplication.Models
{
    public class MoNodeData : GraphLinksModelNodeData<string>
    {
        #region Constructors

        public MoNodeData()
        {
            Id = Guid.NewGuid();
            Key = Id.ToString();
        }

        #endregion

        #region Properties

        private Guid _id;

        public Guid Id
        {
            get { return _id; }
            private set { _id = value; }
        }

        #region Node visual

        private NodeFigure _figure = NodeFigure.Rectangle;

        public NodeFigure Figure
        {
            get { return _figure; }
            set { if (_figure != value) { NodeFigure old = _figure; _figure = value; RaisePropertyChanged("Figure", old, value); } }
        }

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

        private double _width = 40;

        public double Width
        {
            get { return _width; }
            set { if (_width != value) { double old = _width; _width = value; RaisePropertyChanged("Width", old, value); } }
        }

        private double _height = 40;

        public double Height
        {
            get { return _height; }
            set { if (_height != value) { double old = _height; _height = value; RaisePropertyChanged("Height", old, value); } }
        }

        private double _angle;

        public double Angle
        {
            get { return _angle; }
            set { if (_angle != value) { double old = _angle; _angle = value; RaisePropertyChanged("Angle", old, value); } }
        }

        #endregion

        #region Link visual


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
        public void GenerateNodeVisual()
        {
            BackColor = RandomBrushString();
            ForeColor = RandomBrushString();
        }

        public void GenerateLinkVisual()
        {
            BackColor = RandomBrushString();
            ForeColor = RandomBrushString();
        }

        #endregion
    }
}
