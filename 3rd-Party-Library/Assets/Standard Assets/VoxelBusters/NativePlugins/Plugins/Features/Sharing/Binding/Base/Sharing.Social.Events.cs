using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;
	
	public partial class Sharing : MonoBehaviour 
	{
		#region Native Callback Methods

		protected void FBShareFinished (string _reasonString)
		{
			// Resume unity player
			this.ResumeUnity();
			
			eShareResult _shareResult;
			
			// Parse received data
			ParseFBShareFinishedResponse(_reasonString, out _shareResult);
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Sharing:Events] FB share finished, Result=" + _shareResult);
			
			// Trigger event
			if (OnSharingFinished != null)
				OnSharingFinished(_shareResult);
		}

		protected void TwitterShareFinished (string _reasonString)
		{
			// Resume unity player
			this.ResumeUnity();
			
			eShareResult _shareResult;
			
			// Parse received data
			ParseTwitterShareFinishedResponse(_reasonString, out _shareResult);
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Sharing:Events] Twitter share finished, Result=" + _shareResult);

			// Trigger event
			if (OnSharingFinished != null)
				OnSharingFinished(_shareResult);
		}
		
		#endregion
		
		#region Parse Methods
		
		protected virtual void ParseFBShareFinishedResponse (string _resultString, out eShareResult _shareResult)
		{
			_shareResult	= eShareResult.CLOSED;
		}
		
		protected virtual void ParseTwitterShareFinishedResponse (string _resultString, out eShareResult _shareResult)
		{
			_shareResult	= eShareResult.CLOSED;
		}
		
		#endregion
		
		#region Response Methods
		
		protected virtual string FBShareFailedResponse ()
		{
			return string.Empty;
		}
		
		protected virtual string TwitterShareFailedResponse ()
		{
			return string.Empty;
		}
		
		#endregion
	}
}