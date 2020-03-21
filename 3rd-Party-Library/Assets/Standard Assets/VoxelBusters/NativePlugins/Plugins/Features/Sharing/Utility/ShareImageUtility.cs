using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using DownloadTexture = VoxelBusters.Utility.DownloadTexture;

namespace VoxelBusters.NativePlugins.Internal
{
	public abstract class ShareImageUtility 
	{
		#region Fields

		private		DownloadTexture		m_downloadTexture;
		private		IEnumerator			m_takeScreenShotCoroutine;

		#endregion

		#region Properties

		protected bool ImageAsyncDownloadInProgress
		{
			get;
			set;
		}

		#endregion

		#region Abstract Methods

		public abstract void AttachImage (Texture2D _texture);

		#endregion

		#region Methods

		/// <summary>
		/// Captures the screenshot and adds the image for sharing.
		/// </summary>
		public void AttachScreenShot ()
		{
			// Stop existing requests
			StopAsyncRequests();

			// Mark async operation in progress
			ImageAsyncDownloadInProgress	= true;

			// Start loading screenshot data
			m_takeScreenShotCoroutine		= TextureExtensions.TakeScreenshot((_texture)=>{
				
				// Share image
				AttachImage(_texture);
				
				// Set properties
				ImageAsyncDownloadInProgress	= false;
			});

			NPBinding.Instance.StartCoroutine(m_takeScreenShotCoroutine);
		}

		/// <summary>
		/// Adds the specified image for sharing.
		/// </summary>
		/// <param name="_imagePath">Path of the image to be shared.</param>
		public void AttachImageAtPath (string _imagePath)
		{
			// Stop existing requests
			StopAsyncRequests();

			// Mark async operation in progress
			ImageAsyncDownloadInProgress	= true;

			// Start downloading
			URL 	_imagePathURL			= URL.FileURLWithPath(_imagePath);
			m_downloadTexture				= new DownloadTexture(_imagePathURL, true, false);
			m_downloadTexture.OnCompletion	= (Texture2D _texture, string _error)=>{

				// Share image
				AttachImage(_texture);

				// Set properties
				ImageAsyncDownloadInProgress	= false;
			};

			m_downloadTexture.StartRequest();
		}

		protected void StopAsyncRequests ()
		{
			if (m_downloadTexture != null)
			{				
				// Abort request
				m_downloadTexture.Abort();

				// Reset
				m_downloadTexture	= null;
			}

			if (m_takeScreenShotCoroutine != null)
			{
				// Stop coroutine
				NPBinding.Instance.StopCoroutine(m_takeScreenShotCoroutine);

				// Reset
				m_takeScreenShotCoroutine	= null;
			}
		}

		#endregion
	}
}