#if USES_SHARING && UNITY_IOS
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingIOS : Sharing 
	{
		#region Native Methods
		
		[DllImport("__Internal")]
		private static extern bool canSendMail ();
		
		[DllImport("__Internal")]
		private static extern void sendMail (string _subject, 		string _body,  			bool _isHTMLBody,
		                                     string _toRecipients,	string _ccRecipients, 	string _bccRecipients,
		                                     byte[] _attachmentByteArray, int _byteArrayLength, string _mimeType,
		                                     string _attachmentFileNameWithExtn);

		#endregion

		#region Methods

		public override bool IsMailServiceAvailable ()
		{
			bool _isAvailable	= canSendMail();
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Sharing:Mail] Is service available=" + _isAvailable);
			
			return _isAvailable;
		}

		protected override void ShowMailShareComposer (MailShareComposer _composer)
		{
			base.ShowMailShareComposer(_composer);

			if (!IsMailServiceAvailable())
				return;

			// Native method call
			int 	_dataArrayLength		= (_composer.AttachmentData == null) ? 0 : _composer.AttachmentData.Length;
			string	_toRecipientsJSONList	= (_composer.ToRecipients == null) ? null : _composer.ToRecipients.ToJSON();
			string	_CCRecipientsJSONList	= (_composer.CCRecipients == null) ? null : _composer.CCRecipients.ToJSON();
			string	_BCCRecipientsJSONList	= (_composer.BCCRecipients == null) ? null : _composer.BCCRecipients.ToJSON();

			sendMail(_composer.Subject, 		_composer.Body, 		_composer.IsHTMLBody,
			         _toRecipientsJSONList, 	_CCRecipientsJSONList, 	_BCCRecipientsJSONList,
			         _composer.AttachmentData, 	_dataArrayLength, 		_composer.MimeType,
			         _composer.AttachmentFileName);
		}

		#endregion

		#region Deprecated Methods

		[System.Obsolete(kSharingFeatureDeprecatedMethodInfo)]
		public override void SendMail (string _subject, string _body, bool _isHTMLBody, byte[] _attachmentByteArray, 
		                               string _mimeType, string _attachmentFileNameWithExtn, string[] _recipients, SharingCompletion _onCompletion)
		{
			base.SendMail(_subject, _body, _isHTMLBody, _attachmentByteArray, _mimeType, 
			              _attachmentFileNameWithExtn, _recipients, _onCompletion);
			
			if (IsMailServiceAvailable())
			{
				// Attachment data array length
				int		_attachmentByteArrayLength	= _attachmentByteArray == null ? 0 : _attachmentByteArray.Length;
				string	_toRecipientsJSONList		= (_recipients == null) ? null : _recipients.ToJSON();

				if (_attachmentByteArray != null)
					_attachmentByteArrayLength		= _attachmentByteArray.Length;
				
				sendMail(_subject, 				_body, 	_isHTMLBody,
				         _toRecipientsJSONList, null, 	null, 
				         _attachmentByteArray, 	_attachmentByteArrayLength,	_mimeType, 
				         _attachmentFileNameWithExtn);
			}
		}

		#endregion
	}
}
#endif