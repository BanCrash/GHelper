﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using GHelper.Annotations;
using GHelper.Event;
using GHelper.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GHelper.View
{
    public partial class DesktopApplicationView : UserControl, IApplicationView
    {
        
        public static readonly DependencyProperty ApplicationProperty = DependencyProperty.Register(
             nameof (Application),
             typeof (ApplicationViewModel),
             typeof (DesktopApplicationView),
             new PropertyMetadata(null));
        
        public ApplicationViewModel Application
        {
            get { return (ApplicationViewModel) GetValue(ApplicationProperty); }
            set
            {
                SetValue(ApplicationProperty, value);
                ResetAppearance();
                Application.PropertyChanged += RecordViewControls.NotifyOfUserChange;
            }
        }
        
        public GHubRecordViewModel GHubRecord
        {
            get { return Application; }
        }

        public event UserSavedEvent?         UserSaved;
        public event UserDeletedRecordEvent? UserDeletedRecord;

        public event PropertyChangedEventHandler? PropertyChanged;
        
        public DesktopApplicationView()
        {
            this.InitializeComponent();
            RecordViewControls.UserClickedSaveButton += () => { UserSaved?.Invoke(); };
            RecordViewControls.UserClickedDeleteButton += () => { UserDeletedRecord?.Invoke(GHubRecord); };
            RecordViewControls.DeleteButton.Visibility = Visibility.Collapsed;
        }

        void RecordView.SendRecordChangedNotification()
        {
            OnPropertyChanged(nameof(GHubRecord));
        }

        public void ResetAppearance()
        {
            RecordViewControls.ResetAppearance();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
