using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
	public interface IRateMyAppKeysCollection
	{
		#region Properties

		string IsFirstTimeLaunchKeyName
		{
			get;
		}

		string VersionLastRatedKeyName
		{
			get;
		}

		string ShowPromptAfterKeyName
		{
			get;
		}

		string PromptLastShownKeyName
		{
			get;
		}

		string DontShowKeyName
		{
			get;
		}

		string AppUsageCountKeyName
		{
			get;
		}

		#endregion
	}
}