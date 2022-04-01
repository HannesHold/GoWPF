using CommunityToolkit.Mvvm.ComponentModel;
using GoWPFApplication.Enumerations;
using GoWPFApplication.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace GoWPFApplication.ViewModels
{
    public class VmMainWindow : ObservableObject
    {
        #region Constructors

        public VmMainWindow()
        {
            //We want to avoid view model instantiation if we are in designer mode
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }

            InitializeCommands();

            InitializeNodeToolBox();

            InitializeLinksToolBox();

            InitializeGraphLinksModel();
        }

        #endregion

        #region Properties

        private MoGraphLinks? _nodeToolBoxModel;

        public MoGraphLinks? NodeToolBoxModel
        {
            get => _nodeToolBoxModel;
            private set => SetProperty(ref _nodeToolBoxModel, value);
        }

        private ObservableCollection<MoNodeData>? _nodeToolBoxModelNodesSource;

        public ObservableCollection<MoNodeData>? NodeToolBoxModelNodesSource
        {
            get => _nodeToolBoxModelNodesSource;
            private set => SetProperty(ref _nodeToolBoxModelNodesSource, value);
        }

        private MoGraphLinks? _linkToolBoxModel;

        public MoGraphLinks? LinkToolBoxModel
        {
            get => _linkToolBoxModel;
            private set => SetProperty(ref _linkToolBoxModel, value);
        }

        private ObservableCollection<MoNodeData>? _linkToolBoxModelNodesSource;

        public ObservableCollection<MoNodeData>? LinkToolBoxModelNodesSource
        {
            get => _linkToolBoxModelNodesSource;
            private set => SetProperty(ref _linkToolBoxModelNodesSource, value);
        }

        private MoGraphLinks? _graphLinksModel;

        public MoGraphLinks? GraphLinksModel
        {
            get => _graphLinksModel;
            private set => SetProperty(ref _graphLinksModel, value);
        }

        private ObservableCollection<MoNodeData>? _graphLinksModelNodesSource;

        public ObservableCollection<MoNodeData>? GraphLinksModelNodesSource
        {
            get => _graphLinksModelNodesSource;
            private set => SetProperty(ref _graphLinksModelNodesSource, value);
        }

        private ObservableCollection<MoLinkData>? _graphLinksModelLinksSource;

        public ObservableCollection<MoLinkData>? GraphLinksModelLinksSource
        {
            get => _graphLinksModelLinksSource;
            private set => SetProperty(ref _graphLinksModelLinksSource, value);
        }

        #endregion

        #region Methods

        private void InitializeCommands()
        {

        }

        private void InitializeNodeToolBox()
        {
            var nodeToolBoxModel = new MoGraphLinks(GraphLinksSettingTargets.NodesToolBoxModel);

            //Fill data
            NodeToolBoxModelNodesSource = new ObservableCollection<MoNodeData>();

            for (int i = 0; i < 10; i++)
            {
                var newNodedata = new MoNodeData()
                {
                    Text = $"NT {i}"
                };

                newNodedata.GenerateNodeVisual();
                NodeToolBoxModelNodesSource.Add(newNodedata);
            }

            //Set model properties
            nodeToolBoxModel.HasUndoManager = false;
            nodeToolBoxModel.NodesSource = NodeToolBoxModelNodesSource;
            nodeToolBoxModel.Modifiable = false;

            //Assign model
            NodeToolBoxModel = nodeToolBoxModel;
        }

        private void InitializeLinksToolBox()
        {
            var linksToolBoxModel = new MoGraphLinks(GraphLinksSettingTargets.LinksToolBoxModel);

            //Fill data
            LinkToolBoxModelNodesSource = new ObservableCollection<MoNodeData>();

            for (int i = 0; i < 10; i++)
            {
                var newNodedata = new MoNodeData()
                {
                    Text = $"LT {i}"
                };

                newNodedata.GenerateLinkVisual();
                LinkToolBoxModelNodesSource.Add(newNodedata);
            }

            //Set model properties
            linksToolBoxModel.HasUndoManager = false;
            linksToolBoxModel.NodesSource = LinkToolBoxModelNodesSource;
            linksToolBoxModel.Modifiable = false;

            //Assign model
            LinkToolBoxModel = linksToolBoxModel;
        }

        private void InitializeGraphLinksModel()
        {
            var graphLinksModel = new MoGraphLinks(GraphLinksSettingTargets.GraphLinksModel);

            //Initialize data sources
            GraphLinksModelNodesSource = new ObservableCollection<MoNodeData>();
            GraphLinksModelLinksSource = new ObservableCollection<MoLinkData>();

            //Set model properties          
            graphLinksModel.NodesSource = GraphLinksModelNodesSource;
            graphLinksModel.LinksSource = GraphLinksModelLinksSource;
            graphLinksModel.HasUndoManager = true;
            graphLinksModel.Modifiable = true;

            //Assign model
            GraphLinksModel = graphLinksModel;
        }

        #endregion

        #region Commands

        #endregion
    }
}
