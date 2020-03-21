#if USES_SHARING && UNITY_IOS
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	public partial class SharingIOS : Sharing 
	{
		private enum eSocialShareResult 
		{
			CANCELLED,
			DONE,
			FAILED
		}

		#region Native Callback Methods

		private void SocialShareFinished (string _data)
		{
			IDictionary _dataDict	= JSONUtility.FromJSON(_data) as IDictionary;
			string		_result		= _dataDict.GetIfAvailable<int>("result").ToString();
			eSocialShareServiceType _serviceType	= (eSocialShareServiceType)_dataDict.GetIfAvailable<int>("service-type");	

			if (_serviceType == eSocialShareServiceType.FB)
				FBShareFinished(_result);
			else if (_serviceType == eSocialShareServiceType.TWITTER)
				TwitterShareFinished(_result);
		}

		#endregion
	}
}
#endif