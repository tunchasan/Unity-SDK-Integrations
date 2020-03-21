using UnityEngine;
using System.Collections;

#if UNITY_EDITOR && USES_NOTIFICATION_SERVICE
using UnityEditor;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public class EditorPushNotificationService : ScriptableWizard 
	{
		#region Properties

		[Multiline(10)]
		public string 			m_payload;

		#endregion

		#region Unity Methods

		public static void ShowWindow () 
		{
			ScriptableWizard.DisplayWizard<EditorPushNotificationService>("Push Notification Service", "Send");
		}

		private void OnWizardUpdate () 
		{
			helpString = "Enter push notification payload";

			if (string.IsNullOrEmpty(m_payload))
			{
				isValid		= false;
				errorString	= "Payload cant be empty";
			}
			else
			{
				isValid		= true;
				errorString	= string.Empty;
			}
		} 

		private void OnWizardCreate() 
		{
			EditorNotificationCenter.Instance.ReceivedRemoteNotication(m_payload);
		}

		#endregion
	}
}
#endif