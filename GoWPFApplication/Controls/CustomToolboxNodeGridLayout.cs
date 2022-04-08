using Northwoods.GoXam;
using Northwoods.GoXam.Layout;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoWPFApplication.Controls
{
    public class CustomToolboxNodeGridLayout : GridLayout
    {
        #region Constructors

        public CustomToolboxNodeGridLayout()
        {
            Comparer = new ToolboxNodeComparer();
        }

        #endregion

    }

    public class ToolboxNodeComparer : IComparer<Node>
    {
        public int Compare(Node? a, Node? b)
        {
            int returnValue = 0;

            if (a is not null)
            {
                if (b is not null)
                {
                    returnValue = string.Compare(a.Text, b.Text, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase);
                }
                else
                {
                    returnValue = 1;
                }              
            }
            else if (b is not null)
            {
                returnValue = -1;
            }

            //System.Diagnostics.Debug.WriteLine($"a.Text={a?.Text}, b.Text={b?.Text}, returnValue={returnValue}");
            return returnValue;
        }
    }
}
