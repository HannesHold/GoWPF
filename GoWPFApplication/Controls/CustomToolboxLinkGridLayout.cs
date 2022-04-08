using Northwoods.GoXam;
using Northwoods.GoXam.Layout;
using System.Collections.Generic;
using System.Globalization;

namespace GoWPFApplication.Controls
{
    public class CustomToolboxLinkGridLayout : GridLayout
    {
        #region Constructors

        public CustomToolboxLinkGridLayout()
        {
            // There is not such a comparer :/
            Comparer = new ToolboxLinkComparer();
        }

        #endregion

    }

    public class ToolboxLinkComparer : IComparer<Node>
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
