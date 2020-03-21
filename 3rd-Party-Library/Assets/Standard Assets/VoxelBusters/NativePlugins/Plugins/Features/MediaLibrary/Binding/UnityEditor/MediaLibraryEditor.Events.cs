using UnityEngine;
using System.Collections;

#if USES_MEDIA_LIBRARY && UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	public partial class MediaLibraryEditor : MediaLibrary
	{
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

		#endregion
	}
}
#endif