#if USES_SHARING && UNITY_EDITOR
using UnityEngine;
using System.Collections;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingEditor : Sharing 
	{
		#region Methods

		public override bool IsMailServiceAvailable ()
		{	
			bool _canSendMail	= true;
			DebugUtility.Logger.Log(Constants.kDebugTag, "[Sharing:Mail] CanSendMail=" + _canSendMail);
			
			return _canSendMail;
		}

		protected override void ShowMailShareComposer (MailShareComposer _composer)
		{
			base.ShowMailShareComposer(_composer);
			
			if (!IsMailServiceAvailable())
				return;

			if (_composer.AttachmentData != null)
				DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[Sharing:Mail] Attachments are not supported in editor");

			string	_mailToAddress	= null;

			if (_composer.ToRecipients != null)
				_mailToAddress		= string.Join(",", _composer.ToRecipients);

			string	_mailToSubject	= EscapingString(_composer.Subject);
			string	_mailToBody		= EscapingString(_composer.Body);
			string	_mailToString	= string.Format("mailto:{0}?subject={1}&body={2}", _mailToAddress, _mailToSubject, _mailToBody);

			// Opens mail client
			Application.OpenURL(_mailToString);

			// Send event
			MailShareFinished(null);
		}

		private string EscapingString (string _inputString)
		{
			return WWW.EscapeURL(_inputString).Replace("+","%20");
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
				if (_attachmentByteArray != null)
					DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[Sharing:Mail] Attachments are not supported in editor");
				
				string	_mailToAddress	= null;
				
				if (_recipients != null)
					_mailToAddress		= string.Join(",", _recipients);
				
				string	_mailToSubject	= EscapingString(_subject);
				string	_mailToBody		= EscapingString(_body);
				string	_mailToString	= string.Format("mailto:{0}?subject={1}&body={2}", _mailToAddress, _mailToSubject, _mailToBody);
				
				// Opens mail client
				Application.OpenURL(_mailToString);
				
				// Send event
				MailShareFinished(null);
			}
		}
		
		#endregion
	}
}
#endif