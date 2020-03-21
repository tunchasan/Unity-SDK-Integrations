using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	public interface IRateMyAppDelegate 
	{
		#region Methods

		bool CanShowRateMyAppDialog ();
		void OnBeforeShowingRateMyAppDialog ();

		#endregion
	}
}