using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Provides an interface to display native view with various social share services that your users can choose to use from your application.
	/// </summary>
	/// <example>
	/// The following code example demonstrates how to use share sheet with social network services only.
	/// <code>
	/// using UnityEngine;
	/// using System.Collections;
	/// using VoxelBusters.NativePlugins;
	/// 
	/// public class ExampleClass : MonoBehaviour 
	/// {
	/// 	public void ShareViaShareSheet ()
	/// 	{
	/// 		// Create new instance and populate fields
	/// 		SocialShareSheet _shareSheet 	= new SocialShareSheet();
	/// 		_shareSheet.Text				= "This is a test message.";
	/// 
	/// 		// On iPad, popover view is used to show share sheet. So we need to set its position
	/// 		NPBinding.UI.SetPopoverPointAtLastTouchPosition();
	/// 
	///			// Show composer
	/// 		NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);
	/// 	}
	/// 
	/// 	private void OnFinishedSharing (eShareResult _result)
	/// 	{
	/// 		// Insert your code
	/// 	}
	/// }
	/// </code>
	/// </example>
	public class SocialShareSheet : ShareSheet
	{
		#region Properties

		public override eShareOptions[] ExcludedShareOptions
		{
			get
			{
				return base.ExcludedShareOptions;
			}

			set
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[Sharing] Not allowed.");
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SocialShareSheet"/> class.
		/// </summary>
		public SocialShareSheet () 
		{
			base.ExcludedShareOptions	= new eShareOptions[]{
				eShareOptions.WHATSAPP,
				eShareOptions.MAIL,
				eShareOptions.MESSAGE
			};
		}
		
		#endregion
	}
}