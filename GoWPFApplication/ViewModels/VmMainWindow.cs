using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoWPFApplication.Enumerations;
using GoWPFApplication.Models;
using Northwoods.GoXam;
using Northwoods.GoXam.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        private Node _selectedToolBoxNode;

        public Node SelectedToolBoxNode
        {
            get { return _selectedToolBoxNode; }
            set 
            { 
                _selectedToolBoxNode = value;
                OnPropertyChanged();
            }
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

        private Link _selectedToolBoxLink;

        public Link SelectedToolBoxLink
        {
            get { return _selectedToolBoxLink; }
            set
            {
                _selectedToolBoxLink = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MoLinkData>? _linkToolBoxModelLinksSource;

        public ObservableCollection<MoLinkData>? LinkToolBoxModelLinksSource
        {
            get => _linkToolBoxModelLinksSource;
            private set => SetProperty(ref _linkToolBoxModelLinksSource, value);
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
            SelectInToolBoxCommand = new RelayCommand<object?>(ExecuteSelectInToolBox, CanExecuteSelectInToolBox);
        }

        private void InitializeNodeToolBox()
        {
            var nodeToolBoxModel = new MoGraphLinks(GraphLinksSettingTargets.NodesToolBoxModel);

            //Fill data
            NodeToolBoxModelNodesSource = new ObservableCollection<MoNodeData>();

            for (int i = 0; i < 80; i++)
            {
                var newNodedata = new MoNodeData()
                {
                    Text = $"NT {i}"
                };

                // Select first node in the nodes tool box
                if (i == 0) newNodedata.IsSelected = true;
                
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
            LinkToolBoxModelLinksSource = new ObservableCollection<MoLinkData>();

            for (int i = 0; i < 80; i++)
            {
                var newLinkData = new MoLinkData()
                {
                    Text = $"LT {i}",
                    Points = new ObservableCollection<Point>() { new Point(0, 0), new Point(40, 40) } 
                };

                // Select first node in the links tool box
                if (i == 0) newLinkData.IsSelected = true;

                newLinkData.GenerateLinkVisual();
                LinkToolBoxModelLinksSource.Add(newLinkData);
            }

            //Workarround to get correct non-overlapping items in the links toolbox
            var nodesSource = new ObservableCollection<MoNodeData>();

            var newNodedata = new MoNodeData()
            {
                Text = $"Temp"
            };

            nodesSource.Add(newNodedata);
            linksToolBoxModel.NodesSource = nodesSource;

            //Set model properties
            linksToolBoxModel.HasUndoManager = false;
            linksToolBoxModel.LinksSource = LinkToolBoxModelLinksSource;
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

            //Subscribe to events
            graphLinksModel.Changed += GraphLinksModel_Changed;

            //Assign model
            GraphLinksModel = graphLinksModel;
        }

        private void GraphLinksModel_Changed(object? sender, ModelChangedEventArgs e)
        {
            var addedNodes = new List<ModelChangedEventArgs>();
            var addedLinks = new List<ModelChangedEventArgs>();

            if (e.Change == ModelChange.CommittedTransaction)
            {
                UndoManager.CompoundEdit? cedit = e.OldValue as UndoManager.CompoundEdit;

                if (cedit != null)
                {
                    foreach (ModelChangedEventArgs edit in cedit.Edits)
                    {
                        switch (edit.Change)
                        {
                            case ModelChange.Property:

                                if (edit.PropertyName == nameof(MoNodeData.Location))
                                {
                                    // Update stuff in the backend concerning the nodes location

                                    continue;
                                }

                                if (edit.PropertyName == nameof(MoNodeData.Width))
                                {
                                    // Update stuff in the backend concerning the nodes width

                                    continue;
                                }

                                break;
                            case ModelChange.ChangedNodesSource:
                                break;
                            case ModelChange.ChangedNodeKeyPath:
                                break;
                            case ModelChange.ChangedNodeCategoryPath:
                                break;
                            case ModelChange.ChangedNodeIsGroupPath:
                                break;
                            case ModelChange.ChangedGroupNodePath:
                                break;
                            case ModelChange.ChangedMemberNodesPath:
                                break;
                            case ModelChange.ChangedNodeIsLinkLabelPath:
                                break;
                            case ModelChange.ChangedLinksSource:
                                break;
                            case ModelChange.ChangedLinkFromPath:
                                break;
                            case ModelChange.ChangedLinkToPath:
                                break;
                            case ModelChange.ChangedFromNodesPath:
                                break;
                            case ModelChange.ChangedToNodesPath:
                                break;
                            case ModelChange.ChangedLinkLabelNodePath:
                                break;
                            case ModelChange.ChangedLinkFromParameterPath:
                                break;
                            case ModelChange.ChangedLinkToParameterPath:
                                break;
                            case ModelChange.ChangedLinkCategoryPath:
                                break;
                            case ModelChange.ChangedName:
                                break;
                            case ModelChange.ChangedDataFormat:
                                break;
                            case ModelChange.ChangedModifiable:
                                break;
                            case ModelChange.ChangedCopyingGroupCopiesMembers:
                                break;
                            case ModelChange.ChangedCopyingLinkCopiesLabel:
                                break;
                            case ModelChange.ChangedRemovingGroupRemovesMembers:
                                break;
                            case ModelChange.ChangedRemovingLinkRemovesLabel:
                                break;
                            case ModelChange.ChangedValidCycle:
                                break;
                            case ModelChange.ChangedValidUnconnectedLinks:
                                break;
                            case ModelChange.AddedNode:
                                break;
                            case ModelChange.RemovingNode:
                                break;
                            case ModelChange.RemovedNode:
                                addedNodes.Add(edit);
                                break;
                            case ModelChange.ChangedNodeKey:
                                break;
                            case ModelChange.AddedLink:
                                addedLinks.Add(edit);
                                break;
                            case ModelChange.RemovingLink:
                                break;
                            case ModelChange.RemovedLink:
                                break;
                            case ModelChange.ChangedLinkFromPort:
                                break;
                            case ModelChange.ChangedLinkToPort:
                                break;
                            case ModelChange.ChangedLinkLabelKey:
                                break;
                            case ModelChange.ChangedFromNodeKeys:
                                break;
                            case ModelChange.AddedFromNodeKey:
                                break;
                            case ModelChange.RemovedFromNodeKey:
                                break;
                            case ModelChange.ChangedToNodeKeys:
                                break;
                            case ModelChange.AddedToNodeKey:
                                break;
                            case ModelChange.RemovedToNodeKey:
                                break;
                            case ModelChange.ChangedGroupNodeKey:
                                break;
                            case ModelChange.ChangedLinkGroupNodeKey:
                                break;
                            case ModelChange.ChangedMemberNodeKeys:
                                break;
                            case ModelChange.AddedMemberNodeKey:
                                break;
                            case ModelChange.RemovedMemberNodeKey:
                                break;
                            case ModelChange.ChangedParentNodeKey:
                                break;
                            case ModelChange.ChangedChildNodeKeys:
                                break;
                            case ModelChange.AddedChildNodeKey:
                                break;
                            case ModelChange.RemovedChildNodeKey:
                                break;
                            case ModelChange.ChangedNodeCategory:
                                break;
                            case ModelChange.ChangedLinkCategory:
                                break;
                            case ModelChange.StartedTransaction:
                                break;
                            case ModelChange.CommittedTransaction:
                                break;
                            case ModelChange.RolledBackTransaction:
                                break;
                            case ModelChange.StartingUndo:
                                break;
                            case ModelChange.StartingRedo:
                                break;
                            case ModelChange.FinishedUndo:
                                break;
                            case ModelChange.FinishedRedo:
                                break;
                            case ModelChange.InvalidateRelationships:
                                break;
                            case ModelChange.ReplacedReference:
                                break;
                            case ModelChange.ClearedUndoManager:
                                break;
                            case ModelChange.None:
                                break;
                            default:
                                break;
                        }
                    }
                }

                // Handle added nodes
                foreach (var node in addedNodes)
                {

                }

                // Handle added links
                foreach (var link in addedLinks)
                {
                    // Apply visual to the link
                    var linkData = link.Data as MoLinkData;
                    if (linkData is not null)
                    {
                        if (SelectedToolBoxLink is not null)
                        {
                            var selectedToolBoxLink = SelectedToolBoxLink.Data as MoLinkData;

                            if (selectedToolBoxLink is not null)
                            {
                                linkData.BackColor = selectedToolBoxLink.BackColor;
                                linkData.ForeColor = selectedToolBoxLink.ForeColor;
                                linkData.FromArrow = selectedToolBoxLink.FromArrow;
                                linkData.FromArrowSacle = selectedToolBoxLink.FromArrowSacle;
                                linkData.ToArrow = selectedToolBoxLink.ToArrow;
                                linkData.ToArrowSacle = selectedToolBoxLink.ToArrowSacle;
                                linkData.Thickness = selectedToolBoxLink.Thickness;
                                linkData.DashArray = selectedToolBoxLink.DashArray;
                            }
                        }
                    }
                }

            }
        }

        #endregion

        #region Commands

        #region SelectInToolBoxCommand

        private RelayCommand<object?>? _selectInToolBoxCommand;

        public RelayCommand<object?>? SelectInToolBoxCommand
        {
            get { return _selectInToolBoxCommand; }
            private set { _selectInToolBoxCommand = value; }
        }

        private void ExecuteSelectInToolBox(object? parameter)
        {
            if (Enum.TryParse(parameter?.ToString(), true, out InputGestures inputGesture))
            {
                switch (inputGesture)
                {
                    case InputGestures.CtrlPlus1:
                        var node1 = NodeToolBoxModelNodesSource?.Skip(0).Take(1).FirstOrDefault();
                        if (node1 is not null)
                        {
                            NodeToolBoxModelNodesSource?.ToList().ForEach(n => n.IsSelected = false);
                            node1.IsSelected = true;
                        }
                        break;
                    case InputGestures.CtrlPlus2:
                        var node2 = NodeToolBoxModelNodesSource?.Skip(1).Take(1).FirstOrDefault();
                        if (node2 is not null)
                        {
                            NodeToolBoxModelNodesSource?.ToList().ForEach(n => n.IsSelected = false);
                            node2.IsSelected = true;
                        }
                        break;
                    case InputGestures.CtrlPlus3:
                        var node3 = NodeToolBoxModelNodesSource?.Skip(2).Take(1).FirstOrDefault();
                        if (node3 is not null)
                        {
                            NodeToolBoxModelNodesSource?.ToList().ForEach(n => n.IsSelected = false);
                            node3.IsSelected = true;
                        }
                        break;
                    case InputGestures.CtrlPlus4:
                        var node4 = NodeToolBoxModelNodesSource?.Skip(3).Take(1).FirstOrDefault();
                        if (node4 is not null)
                        {
                            NodeToolBoxModelNodesSource?.ToList().ForEach(n => n.IsSelected = false);
                            node4.IsSelected = true;
                        }
                        break;
                    case InputGestures.CtrlPlus5:
                        var node5 = NodeToolBoxModelNodesSource?.Skip(4).Take(1).FirstOrDefault();
                        if (node5 is not null)
                        {
                            NodeToolBoxModelNodesSource?.ToList().ForEach(n => n.IsSelected = false);
                            node5.IsSelected = true;
                        }
                        break;
                    case InputGestures.CtrlPlus6:
                        var node6 = NodeToolBoxModelNodesSource?.Skip(5).Take(1).FirstOrDefault();
                        if (node6 is not null)
                        {
                            NodeToolBoxModelNodesSource?.ToList().ForEach(n => n.IsSelected = false);
                            node6.IsSelected = true;
                        }
                        break;
                    case InputGestures.CtrlPlus7:
                        var node7 = NodeToolBoxModelNodesSource?.Skip(6).Take(1).FirstOrDefault();
                        if (node7 is not null)
                        {
                            NodeToolBoxModelNodesSource?.ToList().ForEach(n => n.IsSelected = false);
                            node7.IsSelected = true;
                        }
                        break;
                    case InputGestures.CtrlPlus8:
                        var node8 = NodeToolBoxModelNodesSource?.Skip(7).Take(1).FirstOrDefault();
                        if (node8 is not null)
                        {
                            NodeToolBoxModelNodesSource?.ToList().ForEach(n => n.IsSelected = false);
                            node8.IsSelected = true;
                        }
                        break;
                    case InputGestures.CtrlPlus9:
                        var node9 = NodeToolBoxModelNodesSource?.Skip(8).Take(1).FirstOrDefault();
                        if (node9 is not null)
                        {
                            NodeToolBoxModelNodesSource?.ToList().ForEach(n => n.IsSelected = false);
                            node9.IsSelected = true;
                        }
                        break;
                    case InputGestures.CtrlPlus0:
                        var node10 = NodeToolBoxModelNodesSource?.Skip(9).Take(1).FirstOrDefault();
                        if (node10 is not null)
                        {
                            NodeToolBoxModelNodesSource?.ToList().ForEach(n => n.IsSelected = false);
                            node10.IsSelected = true;
                        }
                        break;
                    case InputGestures.AltPlus1:
                        var link1 = LinkToolBoxModelLinksSource?.Skip(0).Take(1).FirstOrDefault();
                        if (link1 is not null)
                        {
                            LinkToolBoxModelLinksSource?.ToList().ForEach(n => n.IsSelected = false);
                            link1.IsSelected = true;
                        }
                        break;
                    case InputGestures.AltPlus2:
                        var link2 = LinkToolBoxModelLinksSource?.Skip(1).Take(1).FirstOrDefault();
                        if (link2 is not null)
                        {
                            LinkToolBoxModelLinksSource?.ToList().ForEach(n => n.IsSelected = false);
                            link2.IsSelected = true;
                        }
                        break;
                    case InputGestures.AltPlus3:
                        var link3 = LinkToolBoxModelLinksSource?.Skip(2).Take(1).FirstOrDefault();
                        if (link3 is not null)
                        {
                            LinkToolBoxModelLinksSource?.ToList().ForEach(n => n.IsSelected = false);
                            link3.IsSelected = true;
                        }
                        break;
                    case InputGestures.AltPlus4:
                        var link4 = LinkToolBoxModelLinksSource?.Skip(3).Take(1).FirstOrDefault();
                        if (link4 is not null)
                        {
                            LinkToolBoxModelLinksSource?.ToList().ForEach(n => n.IsSelected = false);
                            link4.IsSelected = true;
                        }
                        break;
                    case InputGestures.AltPlus5:
                        var link5 = LinkToolBoxModelLinksSource?.Skip(4).Take(1).FirstOrDefault();
                        if (link5 is not null)
                        {
                            LinkToolBoxModelLinksSource?.ToList().ForEach(n => n.IsSelected = false);
                            link5.IsSelected = true;
                        }
                        break;
                    case InputGestures.AltPlus6:
                        var link6 = LinkToolBoxModelLinksSource?.Skip(5).Take(1).FirstOrDefault();
                        if (link6 is not null)
                        {
                            LinkToolBoxModelLinksSource?.ToList().ForEach(n => n.IsSelected = false);
                            link6.IsSelected = true;
                        }
                        break;
                    case InputGestures.AltPlus7:
                        var link7 = LinkToolBoxModelLinksSource?.Skip(6).Take(1).FirstOrDefault();
                        if (link7 is not null)
                        {
                            LinkToolBoxModelLinksSource?.ToList().ForEach(n => n.IsSelected = false);
                            link7.IsSelected = true;
                        }
                        break;
                    case InputGestures.AltPlus8:
                        var link8 = LinkToolBoxModelLinksSource?.Skip(7).Take(1).FirstOrDefault();
                        if (link8 is not null)
                        {
                            LinkToolBoxModelLinksSource?.ToList().ForEach(n => n.IsSelected = false);
                            link8.IsSelected = true;
                        }
                        break;
                    case InputGestures.AltPlus9:
                        var link9 = LinkToolBoxModelLinksSource?.Skip(8).Take(1).FirstOrDefault();
                        if (link9 is not null)
                        {
                            LinkToolBoxModelLinksSource?.ToList().ForEach(n => n.IsSelected = false);
                            link9.IsSelected = true;
                        }
                        break;
                    case InputGestures.AltPlus0:
                        var link10 = LinkToolBoxModelLinksSource?.Skip(9).Take(1).FirstOrDefault();
                        if (link10 is not null)
                        {
                            LinkToolBoxModelLinksSource?.ToList().ForEach(n => n.IsSelected = false);
                            link10.IsSelected = true;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private bool CanExecuteSelectInToolBox(object? parameter)
        {
            return true;
        }

        #endregion

        #endregion
    }
}
