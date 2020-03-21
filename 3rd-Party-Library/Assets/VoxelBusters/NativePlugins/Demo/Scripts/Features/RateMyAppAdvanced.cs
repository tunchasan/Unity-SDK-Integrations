using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.NativePlugins;

public class RateMyAppAdvanced : MonoBehaviour, IRateMyAppDelegate
{

	// Use this for initialization
	void Awake () 
	{
		NPBinding.Utility.RateMyApp.Delegate = this;
	}


	

	#region IRateMyAppDelegate implementation
	public bool CanShowRateMyAppDialog ()
	{
		//Rate dialog met the required conditions.. Return Yes, if you want to show it. Else False.
		return true;
	}
	public void OnBeforeShowingRateMyAppDialog ()
	{
		// This will be called when its about to show the dialog.

		NPSettings.Utility.RateMyApp.Message = "My own localised message";

	}
	#endregion
}
