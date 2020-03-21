using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Internal
{
	public class NotifyNPSettingsOnValueChangeAttribute : ExecuteOnValueChangeAttribute 
	{
		#region Constructors

		public  NotifyNPSettingsOnValueChangeAttribute () : base ("OnPropertyModified")
		{}

		#endregion
	}
}