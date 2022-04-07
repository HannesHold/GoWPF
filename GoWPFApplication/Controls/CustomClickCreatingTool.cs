using Northwoods.GoXam.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GoWPFApplication.Controls
{
    public class CustomClickCreatingTool : ClickCreatingTool
    {
        #region Constructors

        public CustomClickCreatingTool()
        {

        }

        #endregion

        #region Overrides

        public override void InsertNode(Point loc)
        {
            // Round to nearest tenth
            loc.X = Math.Round(loc.X / 10) * 10;
            loc.Y = Math.Round(loc.Y / 10) * 10;

            base.InsertNode(loc);
        }

        #endregion
    }
}
