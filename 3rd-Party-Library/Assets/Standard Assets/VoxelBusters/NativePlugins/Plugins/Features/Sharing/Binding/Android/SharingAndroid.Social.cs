#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingAndroid : Sharing 
	{	
		#region Overriden API's 
		
		public override bool IsFBShareServiceAvailable ()
		{
			bool _isAvailable	= Plugin.Call<bool>(Native.Methods.IS_SERVICE_AVAILABLE, (int)eShareOptionsAndroid.FB);
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Sharing:FB] Is service available=" + _isAvailable);

			return _isAvailable;
		}
		
		public override bool IsTwitterShareServiceAvailable ()
		{
			bool _isAvailable	= Plugin.Call<bool>(Native.Methods.IS_SERVICE_AVAILABLE, (int)eShareOptionsAndroid.TWITTER);
			
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Sharing:Twitter] Is service available=" + _isAvailable);
			
			return _isAvailable;
		}
		
		protected override void ShowFBShareComposer (FBShareComposer _composer)
		{
			base.ShowFBShareComposer(_composer);

			if (!IsFBShareServiceAvailable())
				return;
			
			// Native method call
			int		_dataArrayLength	= _composer.ImageData == null ? 0 : _composer.ImageData.Length;

			eShareOptionsAndroid[] _excludedShareOptions	=	new eShareOptionsAndroid[]{
				eShareOptionsAndroid.MAIL, eShareOptionsAndroid.MESSAGE, eShareOptionsAndroid.WHATSAPP, eShareOptionsAndroid.TWITTER, eShareOptionsAndroid.GOOGLE_PLUS,  eShareOptionsAndroid.INSTAGRAM
			};

			Plugin.Call(Native.Methods.SHARE, _composer.Text, _composer.URL, _composer.ImageData, _dataArrayLength, _excludedShareOptions.ToJSON());
		}
		
		protected override void ShowTwitterShareComposer (TwitterShareComposer _composer)
		{
			base.ShowTwitterShareComposer(_composer);
			
			if (!IsTwitterShareServiceAvailable())
				return;

			// Native method call
			int		_dataArrayLength	= _composer.ImageData == null ? 0 : _composer.ImageData.Length;
			
			eShareOptionsAndroid[] _excludedShareOptions	=	new eShareOptionsAndroid[]{
				eShareOptionsAndroid.MAIL, eShareOptionsAndroid.MESSAGE, eShareOptionsAndroid.WHATSAPP, eShareOptionsAndroid.FB, eShareOptionsAndroid.GOOGLE_PLUS,  eShareOptionsAndroid.INSTAGRAM
			};

			Plugin.Call(Native.Methods.SHARE, _composer.Text, _composer.URL, _composer.ImageData, _dataArrayLength, _excludedShareOptions.ToJSON());
		}
		
		#endregion
	}
}
#endif