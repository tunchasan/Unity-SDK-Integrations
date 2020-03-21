using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Provides an interface to compose and send text messages.
	/// </summary>
	/// <example>
	/// The following code example shows how to compose text message.
	/// <code>
	/// using UnityEngine;
	/// using System.Collections;
	/// using VoxelBusters.NativePlugins;
	/// 
	/// public class ExampleClass : MonoBehaviour 
	/// {
	/// 	public void ShareViaMessage ()
	/// 	{
	/// 		if (NPBinding.Sharing.IsMessagingServiceAvailable())
	/// 		{
	/// 			// Create new instance and populate fields
	/// 			MessageShareComposer	_composer	= new MessageShareComposer();
	///				_composer.Body						= "This is a test message.";
	/// 
	/// 			// Show composer
	/// 			NPBinding.Sharing.ShowView(_composer, OnFinishedSharing);
	/// 		}
	/// 		else
	/// 		{
	/// 			// Device doesn't support sending messages
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
	public class MessageShareComposer : IShareView
	{
		#region Properties

		/// <summary>
		/// The initial content of the message.
		/// </summary>
		public string Body
		{
			get;
			set;
		}
		
		/// <summary>
		/// An array of strings containing the initial recipients of the message.
		/// </summary>
		public string[] ToRecipients
		{
			get;
			set;
		}
		
		public bool IsReadyToShowView 
		{
			get
			{
				return true;
			}
		}
		
		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MessageShareComposer"/> class.
		/// </summary>
		public MessageShareComposer ()
		{}

		#endregion
	}
}