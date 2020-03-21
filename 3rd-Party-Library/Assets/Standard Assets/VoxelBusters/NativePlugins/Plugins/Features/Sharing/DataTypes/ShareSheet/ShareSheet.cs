using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Provides an interface to display native view with various share services that your users can choose to use from your application.
	/// </summary>
	/// <description>
	/// It provides several share services such as posting content to social media sites, sending items via email or SMS, and more.
	/// </description>
	/// <example>
	/// The following code example demonstrates how to use share sheet.
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
	/// 		ShareSheet _shareSheet 	= new ShareSheet();	
	/// 		_shareSheet.Text		= "This is a test message.";
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
	public class ShareSheet : ShareImageUtility, IShareView
	{
		#region Fields

		private		eShareOptions[] 	m_excludedShareOptions;

		#endregion

		#region Properties

		/// <summary>
		/// The string contains the text message to be shared.
		/// </summary>
		public string Text
		{
			get;
			set;
		}

		/// <summary>
		/// The string contains the URL to be shared.
		/// </summary>
		public string URL
		{
			get;
			set;
		}

		/// <summary>
		/// The raw data of the image to be shared. (read-only)
		/// </summary>
		public byte[] ImageData
		{
			get;
			private set;
		}

		/// <summary>
		/// The list of services that should not be displayed.
		/// </summary>
		public virtual eShareOptions[] ExcludedShareOptions
		{
			get
			{
				return m_excludedShareOptions;
			}

			set
			{
				m_excludedShareOptions	= value;
			}
		}
		
		public bool IsReadyToShowView 
		{
			get
			{
				return !ImageAsyncDownloadInProgress;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ShareSheet"/> class.
		/// </summary>
		public ShareSheet ()
		{
			// Initialise properties
			Text		= null;
			URL			= null;
			ImageData	= null;
		}

		#endregion
		
		#region Methods

		/// <summary>
		/// Adds the specified image to the post.
		/// </summary>
		/// <param name="_texture">Unity texture object that has to be shared.</param>
		public override void AttachImage (Texture2D _texture)
		{
			if (_texture != null)
				ImageData	= _texture.EncodeToPNG();
			else
				ImageData	= null;
		}
		
		#endregion
	}
}