﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using GHelper.Annotations;
using GHelper.Event;
using GHelper.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace GHelper.View
{
	public partial class ApplicationView : UserControl, IApplicationView
	{
	    public static readonly DependencyProperty ApplicationProperty = DependencyProperty.Register(
		    nameof (Application),
		    typeof (ApplicationViewModel),
		    typeof (ApplicationView),
		    new PropertyMetadata(null)
	    );

	    public ApplicationViewModel Application
	    {
		    get { return (ApplicationViewModel) GetValue(ApplicationProperty); }
		    set
		    {
			    SetValue(ApplicationProperty, value);
			    ResetAppearance();
			    DetermineNameViewStyle();
			    Application.PropertyChanged += RecordViewControls.NotifyOfUserChange;
		    }
	    }

	    public GHubRecordViewModel GHubRecord
	    {
		    get { return Application; }
	    }

	    public event UserSavedEvent?              UserSaved;
	    public event UserDeletedRecordEvent?      UserDeletedRecord;
	    public event PropertyChangedEventHandler? PropertyChanged;

	    public ApplicationView()
        {
	        InitializeComponent();
	        RecordViewControls.UserClickedSaveButton += () => { UserSaved?.Invoke(); };
	        RecordViewControls.UserClickedDeleteButton += () => { UserDeletedRecord?.Invoke(GHubRecord); };
        }

	    void RecordView.SendRecordChangedNotification()
	    {
		    OnPropertyChanged(nameof(GHubRecord));
	    }

	    protected void HandleNameChange(object sender, RoutedEventArgs routedEventInfo)
        {
	        RecordView.ChangeName(this, sender);
        }

	    protected void HandleNameChange(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs eventInfo)
        {
	        RecordView.ChangeName(this, eventInfo.Element);
        }
	    
	    private void SetNewCustomImage(object sender, RoutedEventArgs routedEventInfo)
	    {

	    }

	    public void ResetAppearance()
	    {
		    RecordViewControls.ResetAppearance();
	    }

	    public void DetermineNameViewStyle()
	    {
		    IApplicationView.DetermineNameViewStyle(Application, NameTextBox);
	    }
	    
	    [NotifyPropertyChangedInvocator]
	    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	    {
		    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	    }
	}
}