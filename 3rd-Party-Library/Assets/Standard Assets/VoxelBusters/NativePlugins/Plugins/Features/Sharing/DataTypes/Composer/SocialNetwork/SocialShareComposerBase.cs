using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Base class to compose a post and share it on social network.
	/// </summary>
	public class SocialShareComposerBase : ShareImageUtility, IShareView
	{
		#region Properties

		public eSocialServiceType ServiceType
		{
			get;
			private set;
		}

		/// <summary>
		/// The string contains the text message to be posted.
		/// </summary>
		public string Text
		{
			get;
			set;
		}
		/// <summary>
		/// The string contains the URL to be posted.
		/// </summary>
		public string URL
		{
			get;
			set;
		}

		/// <summary>
		/// The raw data of the image to be posted. (read-only)
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

		private SocialShareComposerBase ()
		{}

		protected SocialShareComposerBase (eSocialServiceType _serviceType)
		{
			ServiceType	= _serviceType;
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