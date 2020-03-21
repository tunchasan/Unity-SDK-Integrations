using UnityEngine;
using System.Collections;

#if USES_MEDIA_LIBRARY && UNITY_ANDROID
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	public partial class MediaLibraryAndroid : MediaLibrary
	{
		#region Constants
		
		private const string 					kImagePath			= "image-path";
		private const string 					kFinishReason		= "finish-reason";

		//Pick finish reasons for image and video
		private const string 					kPickImageSelected	= "pick-image-selected";
		private const string 					kPickImageCancelled	= "pick-image-cancelled";
		private const string 					kPickImageFailed	= "pick-image-failed";

		private const string 					kPickVideoSelected	= "pick-video-selected";
		private const string 					kPickVideoCancelled	= "pick-video-cancelled";
		private const string 					kPickVideoFailed	= "pick-video-failed";
		
		//Play finish reasons
		private const string 					kPlayVideoEnded		= "play-video-ended";
		private const string 					kPlayVideoError		= "play-video-error";
		private const string 					kUserExited			= "user-exited";

		#endregion

		#region Constants Mapping For Parsing

		private static Dictionary<string, ePickImageFinishReason> kPickImageParseMap = new Dictionary<string, ePickImageFinishReason>()
		{
			{ kPickImageSelected, ePickImageFinishReason.SELECTED},
			{ kPickImageCancelled, ePickImageFinishReason.CANCELLED},
			{ kPickImageFailed, ePickImageFinishReason.FAILED}
		};

		private static Dictionary<string, ePickVideoFinishReason> kPickVideoParseMap = new Dictionary<string, ePickVideoFinishReason>()
		{
			{ kPickVideoSelected, ePickVideoFinishReason.SELECTED},
			{ kPickVideoCancelled, ePickVideoFinishReason.CANCELLED},
			{ kPickVideoFailed, ePickVideoFinishReason.FAILED}
		};

		private static Dictionary<string, ePlayVideoFinishReason> kPlayVideoParseMap = new Dictionary<string, ePlayVideoFinishReason>()
		{
			{ kPlayVideoEnded, ePlayVideoFinishReason.PLAYBACK_ENDED},
			{ kPlayVideoError, ePlayVideoFinishReason.PLAYBACK_ERROR},
			{ kUserExited, ePlayVideoFinishReason.USER_EXITED}
		};
		
		#endregion
		
		#region Parse Methods
		
		protected override void ParsePickImageFinishedData (IDictionary _infoDict, out string _selectedImagePath, out ePickImageFinishReason _finishReason)
		{
			_selectedImagePath	= _infoDict.GetIfAvailable<string>(kImagePath);
			_finishReason		=  kPickImageParseMap[_infoDict.GetIfAvailable<string>(kFinishReason)];
			
			// Selected image path is invalid
			if (string.IsNullOrEmpty(_selectedImagePath))
			{
				_selectedImagePath	= null;
				_finishReason		= ePickImageFinishReason.FAILED;
			}
		}
		
		protected override void ParsePickVideoFinishedData (string _reasonString, out ePickVideoFinishReason _finishReason)
		{
			_finishReason		= kPickVideoParseMap[_reasonString];
		}
		
		protected override void ParsePlayVideoFinishedData (string _reasonString, out ePlayVideoFinishReason _finishReason)
		{
			_finishReason		= kPlayVideoParseMap[_reasonString];
		}
		 
		#endregion
	}
}
#endif