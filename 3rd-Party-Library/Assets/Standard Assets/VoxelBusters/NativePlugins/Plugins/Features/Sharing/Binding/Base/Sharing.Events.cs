using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class Sharing : MonoBehaviour 
	{
		#region Delegates

		/// <summary>
		/// Delegate that will be called after share action is complete.
		/// </summary>
		/// <param name="_result">The result of the user’s action. This value will always be <see cref="eShareResult.CLOSED"/>, as its not possible to find out the share action selected by Android user.</param>
		public delegate void SharingCompletion (eShareResult _result);

		#endregion

		#region Events
		
		protected SharingCompletion	OnSharingFinished;
		
		#endregion

		#region Native Callback Methods
		
		protected void SharingFinished (string _reasonString)
		{
			// Resume unity player
			this.ResumeUnity();
			
			eShareResult _shareResult;
			
			// Parse received data
			ParseSharingFinishedData(_reasonString, out _shareResult);
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Sharing:Events] Sharing finished, Result=" + _shareResult);
			
			// Trigger event
			if (OnSharingFinished != null)
				OnSharingFinished(_shareResult);
		}
		
		#endregion

		#region Parse Methods

		protected virtual void ParseSharingFinishedData (string _resultString, out eShareResult _shareResult)
		{
			_shareResult	= eShareResult.CLOSED;
		}

		#endregion

		#region Response Methods
		
		protected virtual string SharingFailedResponse ()
		{
			return string.Empty;
		}
		
		#endregion
	}
}