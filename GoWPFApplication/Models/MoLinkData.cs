using Northwoods.GoXam.Model;
using System;

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

        #region Link visual


        #endregion

        #endregion
    }
}
