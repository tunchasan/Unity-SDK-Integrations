using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Provides an interface to compose a post and share it on Facebook.
	/// </summary>
	/// <example>
	/// The following code example shows how to compose a post with a URL.
	/// <code>
	/// using UnityEngine;
	/// using System.Collections;
	/// using VoxelBusters.NativePlugins;
	/// 
	/// public class ExampleClass : MonoBehaviour 
	/// {
	/// 	public void ShareViaFB ()
	/// 	{
	/// 		if (NPBinding.Sharing.IsFBShareServiceAvailable())
	/// 		{
	/// 			// Create new instance and populate fields
	/// 			FBShareComposer _composer	= new FBShareComposer();
	/// 			_composer.URL				= "www.voxelbusters.com";
	/// 			
	///				// Show composer
	/// 			NPBinding.Sharing.ShowView(_composer, FinishedSharing);
	/// 		}
	/// 		else
	/// 		{
	/// 			// Device doesn't support posting on FB
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
	/// \note As per FB guidelines, the message field of the composer has to be manually entered by the user.
	/// Prefilling the text message parameter, even if they can edit or delete that content before sharing is not supported anymore.
	/// </remarks>
	public class FBShareComposer : SocialShareComposerBase 
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FBShareComposer"/> class.
		/// </summary>
		public FBShareComposer () : base (eSocialServiceType.FB)
		{}
		
		#endregion
	}
}