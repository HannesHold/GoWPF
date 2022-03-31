using Northwoods.GoXam.Model;
using System;

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


        #endregion

        #region Link visual


        #endregion

        #endregion

        #region Methods

        public void GenerateNodeVisual()
        {

        }

        public void GenerateLinkVisual()
        {

        }

        #endregion
    }
}
