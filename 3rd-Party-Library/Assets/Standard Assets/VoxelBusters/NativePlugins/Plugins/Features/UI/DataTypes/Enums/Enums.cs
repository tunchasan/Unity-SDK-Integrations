using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Possible values that determine toast display time length.
	/// </summary>
	public enum eToastMessageLength
	{
		/// <summary>
		/// Show the toast message for a short period of time.
		/// </summary>
		SHORT,
		
		/// <summary>
		/// Show the toast message for a long period of time.
		/// </summary>
		LONG
	}
}