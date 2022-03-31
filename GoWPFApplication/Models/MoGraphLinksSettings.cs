using CommunityToolkit.Mvvm.ComponentModel;
using GoWPFApplication.Enumerations;

namespace GoWPFApplication.Models
{
    public class MoGraphLinksSettings : ObservableObject
    {
        #region Constructors

        public MoGraphLinksSettings(GraphLinksSettingTargets settingTarget)
        {
            SettingTarget = settingTarget;
            InitializeDefaultSettings();
        }

        #endregion

        #region Properties

        private GraphLinksSettingTargets _settingTarget;

        public GraphLinksSettingTargets SettingTarget
        {
            get => _settingTarget;
            private set => SetProperty(ref _settingTarget, value);
        }

        #region Settings

        private bool _allowDragOut;

        public bool AllowDragOut
        {
            get => _allowDragOut;
            set => SetProperty(ref _allowDragOut, value);
        }

        private bool _allowDrop;

        public bool AllowDrop
        {
            get => _allowDrop;
            set => SetProperty(ref _allowDrop, value);
        }

        private bool _gridVisible;
        public bool GridVisible
        {
            get => _gridVisible;
            set => SetProperty(ref _gridVisible, value);
        }

        private double _initialScale;
        public double InitialScale
        {
            get => _initialScale;
            set => SetProperty(ref _initialScale, value);
        }

        private bool _gridSnapEnabled;
        public bool GridSnapEnabled
        {
            get => _gridSnapEnabled;
            set => SetProperty(ref _gridSnapEnabled, value);
        }       

        #endregion

        #endregion

        #region Methods

        private void InitializeDefaultSettings()
        {
            switch (SettingTarget)
            {
                case GraphLinksSettingTargets.NodesToolBoxModel:
                    AllowDragOut = true;
                    AllowDrop = false;
                    GridVisible = false;
                    InitialScale = 1;
                    GridSnapEnabled = true;
                    break;
                case GraphLinksSettingTargets.LinksToolBoxModel:
                    AllowDragOut = true;
                    AllowDrop = false;
                    GridVisible = false;
                    InitialScale = 1;
                    GridSnapEnabled = true;
                    break;
                case GraphLinksSettingTargets.GraphLinksModel:
                    AllowDragOut = false;
                    AllowDrop = true;
                    GridVisible = true;
                    InitialScale = 2;
                    GridSnapEnabled = true;
                    break;
            }
        }

        #endregion
    }
}
