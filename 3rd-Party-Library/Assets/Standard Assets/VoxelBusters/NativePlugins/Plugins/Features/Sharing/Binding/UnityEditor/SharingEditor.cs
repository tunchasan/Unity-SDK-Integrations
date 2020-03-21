#if USES_SHARING && UNITY_EDITOR
using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingEditor : Sharing 
	{
		#region Methods

		protected override void ShowShareSheet (ShareSheet _shareSheet)
		{
			SharingFinished(SharingFailedResponse());
		}
		
		#endregion

		#region Deprecated Methods
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		protected override void Share (string _message, string _URLString, byte[] _imageByteArray, string _excludedOptionsJsonString, SharingCompletion _onCompletion)
		{
			base.Share (_message, _URLString, _imageByteArray, _excludedOptionsJsonString, _onCompletion);
			
			// Feature isnt supported
			DebugUtility.Logger.LogError(Constants.kDebugTag, Constants.kNotSupportedInEditor);
			
			// Post failed event
			SharingFinished(SharingFailedResponse());
		}

		#endregion
	}
}
#endif