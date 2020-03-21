using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public class RateMyAppGenericStoreBase : IRateMyAppKeysCollection, IRateMyAppOperationHandler
	{
		#region Constants

		private const string kIsFirstTimeLaunch		= "np-is-first-time-launch";
		private const string kVersionLastRated   	= "np-version-last-rated";
		private const string kShowPromptAfter		= "np-show-prompt-after";
		private const string kPromptLastShown		= "np-prompt-last-shown";
		private const string kDontShow	           	= "np-dont-show";
		private const string kAppUsageCount			= "np-app-usage-count";

		#endregion

		#region IRateMyAppKeysCollection Implementation

		public string IsFirstTimeLaunchKeyName
		{
			get
			{
				return kIsFirstTimeLaunch;
			}
		}

		public string VersionLastRatedKeyName
		{
			get
			{
				return kVersionLastRated;
			}
		}

		public string ShowPromptAfterKeyName
		{
			get
			{
				return kShowPromptAfter;
			}
		}

		public string PromptLastShownKeyName
		{
			get
			{
				return kPromptLastShown;
			}
		}

		public string DontShowKeyName
		{
			get
			{
				return kDontShow;
			}
		}

		public string AppUsageCountKeyName
		{
			get
			{
				return kAppUsageCount;
			}
		}

		#endregion

		#region IRateMyAppOperationHandler Implementation

		public void Execute(IEnumerator _routine)
		{
			NPBinding.Utility.StartCoroutine(_routine);
		}

		#endregion
	}
}