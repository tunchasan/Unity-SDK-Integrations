using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Demo
{
	public class UtilityDemo : NPDemoBase 
	{
		#region Properties

		[SerializeField]
		private int 		m_applicationBadgeNumber	= 2;

		#endregion

		#region Unity Methods
		
		protected override void Start ()
		{
			base.Start ();

			// Set additional info texts
			AddExtraInfoTexts(
				"For using RateMyApp feature, you have to enable it in NPSettings->Utility Settings.");
		}
		
		#endregion

		#region GUI Methods
		
		protected override void DisplayFeatureFunctionalities ()
		{
			base.DisplayFeatureFunctionalities ();
			
			if (GUILayout.Button("Get UUID"))
			{
				string _uuid = GetUUID();
				
				AddNewResult("New UUID = " + _uuid + ".");
			}
			
			if (GUILayout.Button("Open Store Link"))
			{
				string _appIdentifier = NPSettings.Application.StoreIdentifier;
				
				AddNewResult ("Opening store link with application id = " + _appIdentifier + ".");
				OpenStoreLink (_appIdentifier);
			}
			
			if (GUILayout.Button("Ask For Review Now"))
			{
				AskForReviewNow ();
			}	
			
			if (GUILayout.Button("Set Application Icon Badge Number"))
			{
				SetApplicationIconBadgeNumber ();
			}
			
			if (GUILayout.Button("Get Bundle Version"))
			{
				AddNewResult ("Application's bundle version is " + NPBinding.Utility.GetBundleVersion () + ".");
			}
			
			if (GUILayout.Button("Get Bundle Identifier"))
			{
				AddNewResult ("Application's bundle identifier is " + NPBinding.Utility.GetBundleIdentifier () + ".");
			}
		}
		
		#endregion

		#region API Methods

		private string GetUUID()
		{
			return NPBinding.Utility.GetUUID();
		}
		
		private void OpenStoreLink(string _applicationID)
		{
			NPBinding.Utility.OpenStoreLink(_applicationID);
		}
		
		private void AskForReviewNow()
		{
			if (NPSettings.Utility.RateMyApp.IsEnabled)
			{
				NPBinding.Utility.RateMyApp.AskForReviewNow();
			}
			else
			{
				AddNewResult("Enable RateMyApp feature in NPSettings.");
			}
		}

		private void SetApplicationIconBadgeNumber ()
		{
			NPBinding.Utility.SetApplicationIconBadgeNumber(m_applicationBadgeNumber);
		}

		#endregion
	}
}