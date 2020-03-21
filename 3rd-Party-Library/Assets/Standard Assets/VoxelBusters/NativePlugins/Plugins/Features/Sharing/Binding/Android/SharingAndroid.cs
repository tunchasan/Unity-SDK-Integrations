#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingAndroid : Sharing 
	{	
		private enum eShareOptionsAndroid
		{
			UNDEFINED	= 0,
			MESSAGE,
			MAIL,
			FB,
			TWITTER,
			WHATSAPP,
			GOOGLE_PLUS, 
			INSTAGRAM
		}
		
		#region Constructors
		
		SharingAndroid()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(Native.Class.NAME);
		}
		
		#endregion

		#region Overriden API's
		
		protected override void ShowShareSheet (ShareSheet _shareSheet)
		{
			base.ShowShareSheet (_shareSheet);
			
			// Native method call
			int		_byteArrayLength	= _shareSheet.ImageData == null ? 0 : _shareSheet.ImageData.Length;

			SetAllowedOrientation();
			Plugin.Call(Native.Methods.SHARE, _shareSheet.Text, _shareSheet.URL, _shareSheet.ImageData, _byteArrayLength, _shareSheet.ExcludedShareOptions.ToJSON());
		}

		#endregion

		#region Helpers
		
		private void SetAllowedOrientation()
		{
			Debug.Log("orientation : " + Screen.orientation);
			Plugin.Call(Native.Methods.SET_ALLOWED_ORIENTATION, (int)Screen.orientation);
		}
		
		#endregion
		
		#region Deprecated Methods
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		protected override void Share (string _message, string _URLString, byte[] _imageByteArray, string _excludedOptionsJsonString, SharingCompletion _onCompletion)
		{
			base.Share(_message, _URLString, _imageByteArray, _excludedOptionsJsonString, _onCompletion);

			// Native method call
			int		_byteArrayLength	= _imageByteArray == null ? 0 : _imageByteArray.Length;
			
			Plugin.Call(Native.Methods.SHARE, _message, _URLString, _imageByteArray, _byteArrayLength, _excludedOptionsJsonString);
		}

		#endregion
	}
}
#endif



