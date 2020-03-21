using UnityEngine;
using System.Collections;
using System;
using System.IO;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

using DownloadTexture = VoxelBusters.Utility.DownloadTexture;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Provides cross-platform interface to various sharing services such as posting content to social media sites, sending items via email or SMS, and more.
	/// </summary>
	/// <example>
	/// The following code example shows how to compose mail with screenshot as attachment.
	/// <code>
	/// using UnityEngine;
	/// using System.Collections;
	/// using VoxelBusters.NativePlugins;
	/// 
	/// public class ExampleClass : MonoBehaviour 
	/// {
	/// 	public void ShareViaEmail ()
	/// 	{
	/// 		if (NPBinding.Sharing.IsMailServiceAvailable())
	/// 		{
	/// 			// Create new instance and populate fields
	/// 			MailShareComposer	_composer	= new MailShareComposer();
	/// 			_composer.Subject				= "Sample email";
	/// 
	/// 			// Adding screenshot as attachment
	/// 			_composer.AttachScreenShot();
	/// 
	/// 			// Show composer
	/// 			NPBinding.Sharing.ShowView(_composer, OnFinishedSharing);
	/// 		}
	/// 		else
	/// 		{
	/// 			// Device doesn't support sending emails
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
	public partial class Sharing : MonoBehaviour 
	{
		#region Constants
		
		protected const string kSharingFeatureDeprecatedMethodInfo	= "This method is deprecated. Instead of this use ShowView.";

		#endregion

		#region Methods

		/// <summary>
		/// Shows the native view for sharing specified contents.
		/// </summary>
		/// <param name="_shareView">The object represents the composer that you want to use for sharing items.</param>
		/// <param name="_onCompletion">Callback that will be called after operation is completed.</param>
		public void ShowView (IShareView _shareView, SharingCompletion _onCompletion)
		{
			StartCoroutine(ShowViewCoroutine(_shareView, _onCompletion));
		}

		private IEnumerator ShowViewCoroutine (IShareView _shareView, SharingCompletion _onCompletion)
		{
			while (!_shareView.IsReadyToShowView)
				yield return null;

			// Pause unity player
			this.PauseUnity();
			
			// Cache callback
			OnSharingFinished	= _onCompletion;

			if (_shareView is MailShareComposer)
			{
				ShowMailShareComposer((MailShareComposer)_shareView);
			}
			else if (_shareView is MessageShareComposer)
			{
				ShowMessageShareComposer((MessageShareComposer)_shareView);
			}
			else if (_shareView is WhatsAppShareComposer)
			{
				ShowWhatsAppShareComposer((WhatsAppShareComposer)_shareView);
			}
			else if (_shareView is FBShareComposer)
			{
				ShowFBShareComposer((FBShareComposer)_shareView);
			}
			else if (_shareView is TwitterShareComposer)
			{
				ShowTwitterShareComposer((TwitterShareComposer)_shareView);
			}
			else 
			{
				ShowShareSheet((ShareSheet)_shareView);
			}
		}

		protected virtual void ShowShareSheet (ShareSheet _shareSheet)
		{}

		#endregion
	
		#region Deprecated Social Network Methods

		private eShareOptions[] m_socialNetworkExcludedList	= new eShareOptions[] { eShareOptions.MESSAGE,
			eShareOptions.MAIL,
			eShareOptions.WHATSAPP
		};
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void ShareTextMessageOnSocialNetwork (string _message, SharingCompletion _onCompletion) 
		{
			ShareMessage(_message, m_socialNetworkExcludedList, _onCompletion);
		}
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void ShareURLOnSocialNetwork (string _message, string _URLString, SharingCompletion _onCompletion) 
		{
			ShareURL(_message, _URLString, m_socialNetworkExcludedList, _onCompletion);
		}
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void ShareScreenShotOnSocialNetwork (string _message, SharingCompletion _onCompletion)
		{
			ShareScreenShot(_message, m_socialNetworkExcludedList, _onCompletion);
		}
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void ShareImageOnSocialNetwork (string _message, Texture2D _texture, SharingCompletion _onCompletion)
		{
			ShareImage(_message, _texture, m_socialNetworkExcludedList, _onCompletion);
		}
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void ShareImageOnSocialNetwork (string _message, string _imagePath, SharingCompletion _onCompletion)
		{
			ShareImageAtPath(_message, _imagePath, m_socialNetworkExcludedList, _onCompletion);
		}
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void ShareImageOnSocialNetwork (string _message, byte[] _imageByteArray, SharingCompletion _onCompletion)
		{
			Share(_message, null, _imageByteArray, m_socialNetworkExcludedList, _onCompletion);
		}
		
		#endregion

		#region Deprecated Share Methods

		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void ShareMessage (string _message, eShareOptions[] _excludedOptions, SharingCompletion _onCompletion)
		{
			Share(_message, null, null, _excludedOptions, _onCompletion);
		}

		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void ShareURL (string _message, string _URLString, eShareOptions[] _excludedOptions, SharingCompletion _onCompletion)
		{
			if (string.IsNullOrEmpty(_URLString))
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[Sharing] ShareURL, URL is null/empty");
			}
			
			Share(_message, _URLString, null, _excludedOptions, _onCompletion);
		}

		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void ShareScreenShot (string _message, eShareOptions[] _excludedOptions, SharingCompletion _onCompletion)
		{
			// First capture screenshot
			StartCoroutine(TextureExtensions.TakeScreenshot((_texture)=>{
				
				// Share image
				ShareImage(_message, _texture, _excludedOptions, _onCompletion);
			}));
		}

		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void ShareImage (string _message, Texture2D _texture, eShareOptions[] _excludedOptions, SharingCompletion _onCompletion)
		{
			byte[] _imageByteArray	= null;
			
			if (_texture != null)
			{
				_imageByteArray	= _texture.EncodeToPNG();
			}
			else
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[Sharing] ShareImage, texure is null");
			}
			
			Share(_message, null, _imageByteArray, _excludedOptions, _onCompletion);
		}

		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void ShareImageAtPath (string _message, string _imagePath, eShareOptions[] _excludedOptions, SharingCompletion _onCompletion)
		{
			URL 			_imagePathURL	= URL.FileURLWithPath(_imagePath);
			DownloadAsset 	_newDownload	= new DownloadAsset(_imagePathURL, true);
			_newDownload.OnCompletion		= (WWW _www, string _error)=>{

				byte[]		_imageData		= null;

				if (string.IsNullOrEmpty(_error))
				{
					_imageData		= _www.bytes;
				}
				else
				{
					DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[Sharing] The operation could not be completed. Error=" + _error);
				}
				
				Share(_message, null, _imageData, _excludedOptions, _onCompletion);
			};
			_newDownload.StartRequest();
		}

		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void Share (string _message, string _URLString, byte[] _imageByteArray, eShareOptions[] _excludedOptions, SharingCompletion _onCompletion)
		{
			string _excludedOptionsJsonString	= null;
			
			if (_excludedOptions != null)
				_excludedOptionsJsonString	= _excludedOptions.ToJSON();
			
			Share(_message, _URLString, _imageByteArray, _excludedOptionsJsonString, _onCompletion);
		}

		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		protected virtual void Share (string _message, string _URLString, byte[] _imageByteArray, string _excludedOptionsJsonString, SharingCompletion _onCompletion)
		{
			// Pause unity player
			this.PauseUnity();
			
			// Cache callback
			OnSharingFinished	= _onCompletion;
		}

		#endregion
	}
}