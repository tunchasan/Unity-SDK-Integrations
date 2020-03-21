using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils.Demo;

namespace VoxelBusters.NativePlugins.Demo
{
	public class NPDisabledFeatureDemo : DemoSubMenu 
	{
		#region Fields

		private		string		m_enableFeatureInfoText;

		#endregion

		#region Unity Methods

		protected override void Start ()
		{
			base.Start ();

			// Update info texts
			string _featureName			= gameObject.name;
			m_enableFeatureInfoText		= string.Format("For accessing {0} functionalities, you must enable this feature in NPSettings->Application Settings->Supported Features. " +
			                                         "Once you are done, please don't forget to save the changes.", _featureName);
		}

		#endregion

		#region GUI Methods
		
		protected override void OnGUIWindow ()
		{
			base.OnGUIWindow ();

			GUILayout.Box(m_enableFeatureInfoText);
			GUILayout.FlexibleSpace();
			DrawPopButton();
		}

		#endregion
	}
}