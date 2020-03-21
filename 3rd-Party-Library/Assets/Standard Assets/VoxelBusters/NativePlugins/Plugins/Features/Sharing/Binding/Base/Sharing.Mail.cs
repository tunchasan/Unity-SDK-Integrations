using UnityEngine;
using System.Collections;
using System.IO;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class Sharing : MonoBehaviour 
	{
		#region Methods

		/// <summary>
		/// Determines whether the current device is able to send email.
		/// </summary>
		/// <returns><c>true</c> if the device can send email; otherwise, <c>false</c>.</returns>
		public virtual bool IsMailServiceAvailable ()
		{
			bool _isAvailable	= false;
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Sharing] Is service available=" + _isAvailable);
			
			return _isAvailable;
		}

		protected virtual void ShowMailShareComposer (MailShareComposer _composer)
		{
			if (!IsMailServiceAvailable())
			{
				MailShareFinished(MailShareFailedResponse());
				return;
			}
		}

		#endregion

		#region Deprecated Methods
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void SendPlainTextMail (string _subject, string _body, string[] _recipients, 
		                               SharingCompletion _onCompletion) 
		{
			SendMail(_subject, _body, false, null, string.Empty, 
			         string.Empty, _recipients, _onCompletion);
		}
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void SendHTMLTextMail (string _subject, string _htmlBody, string[] _recipients, 
		                              SharingCompletion _onCompletion) 
		{
			SendMail(_subject, _htmlBody, true, null, string.Empty, 
			         string.Empty, _recipients, _onCompletion);
		}
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void SendMailWithScreenshot (string _subject, string _body, bool _isHTMLBody, 
		                                    string[] _recipients, SharingCompletion _onCompletion) 
		{
			// First capture frame
			StartCoroutine(TextureExtensions.TakeScreenshot((_texture)=>{
				// Convert texture into byte array
				byte[] _imageByteArray	= _texture.EncodeToPNG();
				
				SendMail(_subject, _body, _isHTMLBody, _imageByteArray, 
				         MIMEType.kPNG , "Screenshot.png", _recipients, _onCompletion);
			}));
		}

		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void SendMailWithTexture (string _subject, string _body, bool _isHTMLBody, 
		                                 Texture2D _texture, string[] _recipients, SharingCompletion _onCompletion) 
		{
			byte[] _imageByteArray	= null;
			string _mimeType		= null;
			string _attachmentName	= null;
			
			// Convert texture into byte array
			if (_texture != null)
			{
				_imageByteArray	= _texture.EncodeToPNG();
				_attachmentName	= "texture.png";
				_mimeType		= MIMEType.kPNG;
			}
			else
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[Sharing] Sending mail with no attachments, attachment is null");
			}
			
			SendMail(_subject, _body, _isHTMLBody, _imageByteArray, 
			         _mimeType, _attachmentName, _recipients, _onCompletion);
		}
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public void SendMailWithAttachment (string _subject, string _body, bool _isHTMLBody, 
		                                    string _attachmentPath, string _mimeType, string[] _recipients, SharingCompletion _onCompletion) 
		{
			DownloadAsset _request	= new DownloadAsset(URL.FileURLWithPath(_attachmentPath), true);
			_request.OnCompletion	= (WWW _www, string _error) => {

				byte[] _attachmentByteArray	= null;
				string _filename			= null;
				
				if (string.IsNullOrEmpty(_error))
				{
					_attachmentByteArray	= _www.bytes;
					_filename				= Path.GetFileName(_attachmentPath);
				}
				else
				{
					DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[Sharing] The operation could not be completed. Error=" + _error);
				}

				SendMail(_subject, _body, _isHTMLBody, _attachmentByteArray,
				         _mimeType, _filename, _recipients, _onCompletion);
			};
			_request.StartRequest();
		}
		
		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public virtual void SendMail (string _subject, string _body, bool _isHTMLBody, byte[] _attachmentByteArray, 
		                              string _mimeType, string _attachmentFileNameWithExtn, string[] _recipients, SharingCompletion _onCompletion)
		{
			// Pause unity player
			this.PauseUnity();
			
			// Cache callback
			OnSharingFinished	= _onCompletion;
			
			// Cant send mail
			if (!IsMailServiceAvailable())
			{
				MailShareFinished(MailShareFailedResponse());
				return;
			}
		}
		
		#endregion
	}
}