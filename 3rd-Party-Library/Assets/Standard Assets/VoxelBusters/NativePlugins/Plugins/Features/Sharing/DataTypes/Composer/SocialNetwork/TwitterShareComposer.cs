using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Provides an interface to compose a post and publish it on Twitter.
	/// </summary>
	/// <example>
	/// The following code example shows how to compose a tweet message.
	/// <code>
	/// using UnityEngine;
	/// using System.Collections;
	/// using VoxelBusters.NativePlugins;
	/// 
	/// public class ExampleClass : MonoBehaviour 
	/// {
	/// 	public void ShareViaTwitter ()
	/// 	{
	/// 		if (NPBinding.Sharing.IsTwitterShareServiceAvailable())
	/// 		{
	/// 			// Create new instance and populate fields
	/// 			TwitterShareComposer _composer	= new TwitterShareComposer();
	/// 			_composer.Text					= "This is a test message.";
	/// 			
	///				// Show composer
	/// 			NPBinding.Sharing.ShowView(_composer, FinishedSharing);
	/// 		}
	/// 		else
	/// 		{
	/// 			// Device doesn't support posting on Twitter
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
	public class TwitterShareComposer : SocialShareComposerBase 
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="TwitterShareComposer"/> class.
		/// </summary>
		public TwitterShareComposer () : base (eSocialServiceType.TWITTER)
		{}

		#endregion
	}
}