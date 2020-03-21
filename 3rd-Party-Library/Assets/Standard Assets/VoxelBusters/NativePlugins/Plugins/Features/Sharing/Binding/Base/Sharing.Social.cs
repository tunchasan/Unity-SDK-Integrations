using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class Sharing : MonoBehaviour 
	{
		#region Methods

		/// <summary>
		/// Determines whether the current device is able to post contents on Facebook.
		/// </summary>
		/// <returns><c>true</c> if the device can post on Facebook; otherwise, <c>false</c>.</returns>
		public virtual bool IsFBShareServiceAvailable ()
		{
			bool _isAvailable	= false;
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Sharing] Is service available=" + _isAvailable);
			
			return _isAvailable;
		}

		/// <summary>
		/// Determines whether the current device is able to post contents on Twitter.
		/// </summary>
		/// <returns><c>true</c> if the device can post on Twitter; otherwise, <c>false</c>.</returns>
		public virtual bool IsTwitterShareServiceAvailable ()
		{
			bool _isAvailable	= false;
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Sharing] Is service available=" + _isAvailable);
			
			return _isAvailable;
		}
		
		protected virtual void ShowFBShareComposer (FBShareComposer _composer)
		{
			if (!IsFBShareServiceAvailable())
			{
				FBShareFinished(FBShareFailedResponse());
				return;
			}
		}
		
		protected virtual void ShowTwitterShareComposer (TwitterShareComposer _composer)
		{
			if (!IsTwitterShareServiceAvailable())
			{
				TwitterShareFinished(TwitterShareFailedResponse());
				return;
			}
		}
		
		#endregion
	}
}