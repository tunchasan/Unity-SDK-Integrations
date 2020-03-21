using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
namespace VoxelBusters.NativePlugins
{
	public partial class UIAndroid : UI 
	{
		#region Platform Native Info
		
		private class Native
		{
			// Handler class name
			internal class Class
			{
				internal const string NAME								= "com.voxelbusters.nativeplugins.features.ui.UiHandler";
			}
			
			// For holding method names
			internal class Methods
			{			
				internal const string SHOW_ALERT_DIALOG					= "showAlertDialogWithMultipleButtons";
				internal const string SHOW_SINGLE_FIELD_PROMPT			= "showSingleFieldPromptDialog";
				internal const string SHOW_LOGIN_PROMPT 				= "showLoginPromptDialog";
				internal const string SHOW_TOAST 						= "showToast";
			}
		}
		
		#endregion
		
		#region  Native Access Variables
		
		private AndroidJavaObject  	Plugin
		{
			get;
			set;
		}
		
		#endregion
	}
}
#endif