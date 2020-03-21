using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Defines sources available to pick an image.
	/// </summary>
	public enum eImageSource
	{
		/// <summary> Specifies the device's photo album as source for picking image.</summary>
		ALBUM,

		/// <summary> Specifies the device's camera as source for picking image.</summary>
		CAMERA,

		/// <summary> Specifies the device's photo album and camera as source for picking image.</summary>
		BOTH
	}

	/// <summary>
	/// Constants describing the reason that caused image picker view to dismiss.
	/// </summary>
	public enum ePickImageFinishReason
	{
		/// <summary> The image was selected from specified <see cref="eImageSource"/>.</summary>
		SELECTED,

		/// <summary> The user cancelled the operation.</summary>		
		CANCELLED,

		/// <summary> The image was not picked or saved, possibly due to an error.</summary>		
		FAILED
	}

	/// <summary>
	/// Constants describing the reason that caused video picker view to dismiss.
	/// </summary>
	public enum ePickVideoFinishReason
	{
		/// <summary> The video was selected from gallery.</summary>	
		SELECTED,

		/// <summary> The user cancelled the operation.</summary>	
		CANCELLED,
	
		/// <summary> The video was not picked, possibly due to an error.</summary>	
		FAILED
	}

	/// <summary>
	/// Constants describing the reason that caused video playback to end.
	/// </summary>
	public enum ePlayVideoFinishReason
	{
		/// <summary> The end of the video was reached.</summary>	
		PLAYBACK_ENDED,

		/// <summary> There was an error during playback.</summary>
		PLAYBACK_ERROR,

		/// <summary> The user exited without playing the complete video.</summary>
		USER_EXITED
	}
}