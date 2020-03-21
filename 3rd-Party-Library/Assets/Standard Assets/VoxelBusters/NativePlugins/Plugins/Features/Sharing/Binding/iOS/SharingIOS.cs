using UnityEngine;
using System.Collections;

#if USES_SHARING && UNITY_IOS
using System.Runtime.InteropServices;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	public partial class SharingIOS : Sharing 
	{
		#region Native Methods

		[DllImport("__Internal")]
		private static extern void share (string _message, string _URLString, byte[] _imageByteArray, int _byteArrayLength, string _excludedOptions);

		#endregion
	
		#region Methods

		protected override void ShowShareSheet (ShareSheet _shareSheet)
		{
			base.ShowShareSheet (_shareSheet);

			// Native method call
			int		_byteArrayLength	= _shareSheet.ImageData == null ? 0 : _shareSheet.ImageData.Length;

			share(_shareSheet.Text, _shareSheet.URL, _shareSheet.ImageData, 
			      _byteArrayLength, _shareSheet.ExcludedShareOptions.ToJSON());
		}

		#endregion

		#region Deprecated Methods

		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		protected override void Share (string _message, string _URLString, byte[] _imageByteArray, string _excludedOptionsJsonString, SharingCompletion _onCompletion)
		{
			base.Share(_message, _URLString, _imageByteArray, _excludedOptionsJsonString, _onCompletion);
			
			// Get image byte array length
			int _byteArrayLength	= 0;
			
			if (_imageByteArray != null)
				_byteArrayLength	= _imageByteArray.Length;
			
			share(_message, _URLString, _imageByteArray, _byteArrayLength, _excludedOptionsJsonString);
		}

		#endregion
	}
}
#endif