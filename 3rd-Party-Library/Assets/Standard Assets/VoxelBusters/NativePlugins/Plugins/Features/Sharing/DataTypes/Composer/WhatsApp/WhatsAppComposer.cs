using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	using Internal;
	
	/// <summary>
	/// Provides an interface to share contents on WhatsApp.
	/// </summary>
	/// <example>
	/// The following code example shows how to compose a text message for sharing on WhatsApp.
	/// <code>
	/// using UnityEngine;
	/// using System.Collections;
	/// using VoxelBusters.NativePlugins;
	/// 
	/// public class ExampleClass : MonoBehaviour 
	/// {
	/// 	public void ShareViaWhatsApp ()
	/// 	{
	/// 		if (NPBinding.Sharing.IsWhatsAppServiceAvailable())
	/// 		{
	/// 			// Create new instance and populate fields
	/// 			WhatsAppShareComposer _composer	= new WhatsAppShareComposer();
	/// 			_composer.Text					= "This is a test message.";
	/// 			
	///				// Show composer
	/// 			NPBinding.Sharing.ShowView(_composer, FinishedSharing);
	/// 		}
	/// 		else
	/// 		{
	/// 			// Device doesn't support sharing on WhatsApp
	/// 		}
	/// 	}
	/// 
	/// 	private void OnFinishedSharing (eShareResult _result)
	/// 	{
	/// 		// Insert your code
	/// 	}
	/// }
	/// </code>
	/// </example>
	/// <remarks>
	/// \note At this point, WhatsApp allows external applications to share either text message or image, but not both.
	/// </remarks>
	public class WhatsAppShareComposer : ShareImageUtility, IShareView
	{
		#region Properties

		/// <summary>
		/// The string holds the message to be shared on WhatsApp. 
		/// </summary>
		public string Text
		{
			get;
			set;
		}
		
		/// <summary>
		/// The raw data of the image to be shared on WhatsApp. 
		/// </summary>
		public byte[] ImageData
		{
			get;
			private set;
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
		/// Initializes a new instance of the <see cref="WhatsAppShareComposer"/> class.
		/// </summary>
		public WhatsAppShareComposer ()
		{}

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