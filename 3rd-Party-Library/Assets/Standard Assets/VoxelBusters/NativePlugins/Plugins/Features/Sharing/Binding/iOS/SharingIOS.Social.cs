#if USES_SHARING && UNITY_IOS
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingIOS : Sharing 
	{
		#region Native Methods
		
		[DllImport("__Internal")]
		private static extern bool isSocialShareServiceAvailable (int _serviceType);
		
		[DllImport("__Internal")]
		private static extern void socialShare (int _serviceType, 			string _message,    string _URLString,
		                                        byte[] _imageByteArray,  	int _byteArrayLength);
		
		#endregion
		
		#region Overriden API's 
		
		public override bool IsFBShareServiceAvailable ()
		{
			bool _isAvailable	= isSocialShareServiceAvailable((int)eSocialShareServiceType.FB);
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Sharing:FB] Is service available=" + _isAvailable);
			
			return _isAvailable;
		}
		
		public override bool IsTwitterShareServiceAvailable ()
		{
			bool _isAvailable	= isSocialShareServiceAvailable((int)eSocialShareServiceType.TWITTER);
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

			socialShare((int)eSocialShareServiceType.FB, 	_composer.Text, 	_composer.URL,
			            _composer.ImageData, 				_dataArrayLength);
		}
		
		protected override void ShowTwitterShareComposer (TwitterShareComposer _composer)
		{
			base.ShowTwitterShareComposer(_composer);
			
			if (!IsTwitterShareServiceAvailable())
				return;

			// Native method call
			int		_dataArrayLength	= _composer.ImageData == null ? 0 : _composer.ImageData.Length;
			
			socialShare((int)eSocialShareServiceType.TWITTER,	_composer.Text, 	_composer.URL,
			            _composer.ImageData, 					_dataArrayLength);
		}
		
		#endregion

		#region Nested Types

		private enum eSocialShareServiceType
		{
			FB		= 0,
			TWITTER
		}
			
		#endregion
	}
}
#endif