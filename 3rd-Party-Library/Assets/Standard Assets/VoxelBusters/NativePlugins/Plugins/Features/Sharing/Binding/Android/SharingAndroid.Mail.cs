#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingAndroid : Sharing 
	{
		#region Methods

		public override bool IsMailServiceAvailable()
		{
			bool _canSendMail	= Plugin.Call<bool>(Native.Methods.IS_SERVICE_AVAILABLE, (int)eShareOptionsAndroid.MAIL);
			if(!_canSendMail)
			{
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[Sharing:Mail] CanSendMail=" + _canSendMail);
			}

			return _canSendMail;
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
			
			Plugin.Call(Native.Methods.SEND_MAIL, _composer.Subject, _composer.Body,
			            _composer.IsHTMLBody, _composer.AttachmentData, _dataArrayLength,
			            _composer.MimeType, _composer.AttachmentFileName, _toRecipientsJSONList, _CCRecipientsJSONList, _BCCRecipientsJSONList);
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
				// Find attachment data array length
				int		_attachmentByteArrayLength	= _attachmentByteArray == null ? 0 : _attachmentByteArray.Length;
				string	_toRecipientsJSONList		= (_recipients == null) ? null : _recipients.ToJSON();

				Plugin.Call(Native.Methods.SEND_MAIL, _subject, _body,
				            _isHTMLBody, _attachmentByteArray, _attachmentByteArrayLength,
				            _mimeType, _attachmentFileNameWithExtn, _toRecipientsJSONList, null, null);
			}
		}

		#endregion
	}
}
#endif