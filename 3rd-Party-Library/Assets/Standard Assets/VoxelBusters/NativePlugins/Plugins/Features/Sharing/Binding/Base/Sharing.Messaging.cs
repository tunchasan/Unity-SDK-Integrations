using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class Sharing : MonoBehaviour 
	{
		#region Methods

		/// <summary>
		/// Determines whether the current device is able to send text messages.
		/// </summary>
		/// <returns><c>true</c> if the device can send text messages; otherwise, <c>false</c>.</returns>
		public virtual bool IsMessagingServiceAvailable ()
		{
			bool _isAvailable	= false;
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Sharing] Is service available=" + _isAvailable);
			
			return _isAvailable;
		}

		protected virtual void ShowMessageShareComposer (MessageShareComposer _composer)
		{
			if (!IsMessagingServiceAvailable())
			{
				MessagingShareFinished(MessagingShareFailedResponse());
				return;
			}
		}

		#endregion

		#region Deprecated Methods

		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public virtual void SendTextMessage (string _body, string[] _recipients, SharingCompletion _onCompletion)
		{
			// Pause unity player
			this.PauseUnity();
			
			// Cache callback
			OnSharingFinished	= _onCompletion;
			
			// Messaging service isnt available
			if (!IsMessagingServiceAvailable())
			{
				MessagingShareFinished(MessagingShareFailedResponse());
				return;
			}
		}
		
		#endregion
	}
}
