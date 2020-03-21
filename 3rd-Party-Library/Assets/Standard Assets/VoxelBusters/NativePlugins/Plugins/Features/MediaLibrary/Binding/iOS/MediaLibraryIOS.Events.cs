using UnityEngine;
using System.Collections;

#if USES_MEDIA_LIBRARY && UNITY_IOS
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	public partial class MediaLibraryIOS : MediaLibrary
	{
		private enum MPMovieFinishReason
		{
			MPMovieFinishReasonPlaybackEnded,
			MPMovieFinishReasonPlaybackError,
			MPMovieFinishReasonUserExited
		}

		#region Constants
		
		private const string 	kImagePath		= "image-path";
		private const string 	kFinishReason	= "finish-reason";
		
		#endregion

		#region Parse Methods

		protected override void ParsePickImageFinishedData (IDictionary _infoDict, out string _selectedImagePath, out ePickImageFinishReason _finishReason)
		{
			_selectedImagePath	= _infoDict.GetIfAvailable<string>(kImagePath);
			_finishReason		=  (ePickImageFinishReason)_infoDict.GetIfAvailable<int>(kFinishReason);
		}

		protected override void ParsePickVideoFinishedData (string _reasonString, out ePickVideoFinishReason _finishReason)
		{
			_finishReason		= (ePickVideoFinishReason)int.Parse(_reasonString);
		}

		protected override void ParsePlayVideoFinishedData (string _reasonString, out ePlayVideoFinishReason _finishReason)
		{
			_finishReason		= ConvertToPlayVideoFinishReason((MPMovieFinishReason)int.Parse(_reasonString));
		}

		#endregion

		#region Static Methods

		private static ePlayVideoFinishReason ConvertToPlayVideoFinishReason (MPMovieFinishReason _mpFinishReason)
		{
			switch (_mpFinishReason)
			{
			case MPMovieFinishReason.MPMovieFinishReasonPlaybackEnded:
				return ePlayVideoFinishReason.PLAYBACK_ENDED;

			case MPMovieFinishReason.MPMovieFinishReasonPlaybackError:
				return ePlayVideoFinishReason.PLAYBACK_ERROR;

			case MPMovieFinishReason.MPMovieFinishReasonUserExited:
				return ePlayVideoFinishReason.USER_EXITED;
			}

			return (ePlayVideoFinishReason)0;
		}

		#endregion
	}
}
#endif