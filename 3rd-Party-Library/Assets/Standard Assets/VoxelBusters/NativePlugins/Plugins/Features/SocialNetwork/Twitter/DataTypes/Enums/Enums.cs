using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	///	Possible values for the result, when composer view is dismissed.
	/// </summary>
	public enum eTwitterComposerResult
	{
		/// <summary>
		/// The composer is dismissed without sending the Tweet (i.e. the user selects Cancel, or the account is unavailable).
		/// </summary>
		CANCELLED,

		/// <summary>
		/// The composer is dismissed and the message is being sent in the background.
		/// </summary>
		DONE
	}
}