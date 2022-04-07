using GoWPFApplication.Models;
using Northwoods.GoXam;
using Northwoods.GoXam.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GoWPFApplication.Controls
{
    public class CustomPartManager : PartManager
    {
        #region Constructors

        public CustomPartManager()
        {
            UpdatesRouteDataPoints = true;
        }

        #endregion

        #region Overrides

        // copy Route.Points to MyLinkData
        public override ICopyDictionary CopyParts(IEnumerable<Part> coll, IDiagramModel destmodel)
        {
            ICopyDictionary dict = base.CopyParts(coll, destmodel);
            foreach (object data in dict.SourceCollection.Links)
            {
                MoLinkData origdata = data as MoLinkData;
                Link origlink = FindLinkForData(origdata, this.Diagram.Model);
                if (origlink != null && origlink.Route.PointsCount > 1)
                {
                    // copy the MyLinkData.Points
                    MoLinkData copieddata = dict.FindCopiedLink(origdata) as MoLinkData;
                    if (copieddata != null)
                    {
                        copieddata.Points = new List<Point>(origlink.Route.Points);
                        // now transfer to the Link.Route.Points
                        Link copiedlink = FindLinkForData(copieddata, this.Diagram.Model);
                        if (copiedlink != null)
                        {
                            copiedlink.Route.Points = (IList<Point>)copieddata.Points;
                        }
                    }
                }
            }
            return dict;
        }

        // use MyLinkData.Points, if any, when creating a Link
        protected override void OnLinkAdded(Link link)
        {
            base.OnLinkAdded(link);
            MoLinkData data = link.Data as MoLinkData;
            if (data != null && data.Points != null)
            {
                link.Route.Points = (IList<Point>)data.Points;
            }
        }

        // this supports undo/redo of link route reshaping
        protected override void UpdateRouteDataPoints(Link link)
        {
            if (!this.UpdatesRouteDataPoints) return;   // in coordination with Load_Click and LoadLinkRoutes, above
            MoLinkData data = link.Data as MoLinkData;
            if (data != null)
            {
                data.Points = new List<Point>(link.Route.Points);
            }
        }

        #endregion
    }
}
