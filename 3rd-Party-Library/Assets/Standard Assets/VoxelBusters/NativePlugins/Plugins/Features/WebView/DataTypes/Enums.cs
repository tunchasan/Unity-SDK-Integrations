using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Specifies the appearence options available for web view.
	/// </summary>
	public enum eWebviewControlType
	{
		/// <summary> 
		/// No controls are shown for web view. This appearence is ideal for banner ads like requirement. 
		/// </summary>
		NO_CONTROLS,

		/// <summary>
		/// This option creates a close button at top-right corner of the web view. On clicking this, web view gets dismissed. 
		/// </summary>
		/// <remarks>
		/// \note Incase if you want to permanetly remove web view instance, use <see cref="WebView.Destory"/>.
		/// </remarks>
		CLOSE_BUTTON,

		/// <summary>
		/// This option provides browser like appearence with 4 buttons for easy access to web view features.
		/// </summary>
		/// <description>
		/// It has Back and Forward buttons for navigating through the history.
		/// Reload button for reloading the current webpage contents.
		/// And finally, Done button for dismissing the web view.
		/// </description>
		TOOLBAR
	}
}
