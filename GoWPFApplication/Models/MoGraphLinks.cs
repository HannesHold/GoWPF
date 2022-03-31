using CommunityToolkit.Mvvm.ComponentModel;
using GoWPFApplication.Enumerations;
using Northwoods.GoXam.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GoWPFApplication.Models
{
    public class MoGraphLinks : GraphLinksModel<MoNodeData, string, string, MoLinkData>, INotifyPropertyChanged
    {
        #region Constructors

        public MoGraphLinks(GraphLinksSettingTargets settingTarget)
        {
            Settings = new MoGraphLinksSettings(settingTarget);
        }

        #endregion

        #region Properties

        private MoGraphLinksSettings? _settings;

        public MoGraphLinksSettings? Settings
        {
            get { return _settings; }
            set 
            { 
                _settings = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
