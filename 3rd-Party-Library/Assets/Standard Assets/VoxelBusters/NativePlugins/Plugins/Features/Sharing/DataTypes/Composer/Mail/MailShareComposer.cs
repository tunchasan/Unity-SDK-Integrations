using UnityEngine;
using System.Collections;
using System.IO;
using VoxelBusters.Utility;
using VoxelBusters.UASUtils;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// Provides an interface to compose and send email messages.
	/// </summary>
	/// <description>
	/// You can present this view with prepopulated fields such as subject, body, recipients, attachments etc.
	/// The user can edit the initial contents you specify and choose to send the email or cancel the operation.
	/// </description>
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
	public class MailShareComposer : ShareImageUtility, IShareView
	{
		#region Fields

		private		bool	m_attachmentDownloadInProgress	= false;

		#endregion

		#region Properties

		/// <summary>
		/// The text that appears in the subject line of the email.
		/// </summary>
		public string Subject
		{
			get;
			set;
		}

		/// <summary>
		/// The body text of the email
		/// </summary>
		public string Body
		{
			get;
			set;
		}

		/// <summary>
		/// A bool value used to identify whether the email body contains HTML content or not.
		/// </summary>
		/// <value><c>true</c> if email body contains HTML content; otherwise, <c>false</c>.</value>
		public bool IsHTMLBody
		{
			get;
			set;
		}

		/// <summary>
		/// The recipients to include in the email’s “To” field.
		/// </summary>
		public string[] ToRecipients
		{
			get;
			set;
		}

		/// <summary>
		/// The recipients to include in the email’s “Cc” field.
		/// </summary>
		public string[] CCRecipients
		{
			get;
			set;
		}

		/// <summary>
		/// The recipients to include in the email’s “Bcc” field.
		/// </summary>
		public string[] BCCRecipients
		{
			get;
			set;
		}

		/// <summary>
		/// The raw data of the attachment. (read-only)
		/// </summary>
		public byte[] AttachmentData
		{
			get;
			private set;
		}
		
		/// <summary>
		/// The filename of the data attached in the email. (read-only)
		/// </summary>
		public string AttachmentFileName
		{
			get;
			private set;
		}

		/// <summary>
		/// The MIME type of the data attached in the email. (read-only)
		/// </summary>
		public string MimeType
		{
			get;
			private set;
		}

		public bool IsReadyToShowView 
		{
			get
			{
				return !(ImageAsyncDownloadInProgress || m_attachmentDownloadInProgress);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MailShareComposer"/> class.
		/// </summary>
		public MailShareComposer ()
		{}

		#endregion

		#region Methods

		/// <summary>
		/// Adds specified image as an attachment of the email.
		/// </summary>
		/// <param name="_texture">Unity texture object that has to be added as an attachment.</param>
		public override void AttachImage (Texture2D _texture)
		{
			if (_texture != null)
			{
				AttachmentData		= _texture.EncodeToPNG();
				AttachmentFileName	= "ShareImage.png";
				MimeType			= MIMEType.kPNG;
			}
			else
			{
				AttachmentData		= null;
				AttachmentFileName	= null;
				MimeType			= null;
			}
		}
		
		/// <summary>
		/// Adds specified file as an attachment of the email.
		/// </summary>
		/// <param name="_attachmentPath">Path of the file that has to be added as an attachment.</param>
		/// <param name="_MIMEType">The MIME type of the file attached in the email.</param>
		public void AddAttachmentAtPath (string _attachmentPath, string _MIMEType)
		{
			// Mark download in progress
			m_attachmentDownloadInProgress		= true;

			// Start downloading
			DownloadAsset _request	= new DownloadAsset(URL.FileURLWithPath(_attachmentPath), true);
			_request.OnCompletion	= (WWW _www, string _error) => {

				// Reset download progress
				m_attachmentDownloadInProgress	= false;

				// Process data
				if (string.IsNullOrEmpty(_error))
				{
					AddAttachment(_www.bytes, Path.GetFileName(_attachmentPath), _MIMEType);
				}
				else
				{
					DebugUtility.Logger.LogWarning(Constants.kDebugTag, "[Sharing] The operation could not be completed. Error=" + _error);
				}
			};
			_request.StartRequest();
		}

		/// <summary>
		/// Adds the specified data as an attachment of the email.
		/// </summary>
		/// <param name="_attachmentData">Raw data of the file that has to be added as an attachment.</param>
		/// <param name="_attachmentFileName">The filename of the data attached in the email.</param>
		/// <param name="_MIMEType">The MIME type of the data attached in the email.</param>
		public void AddAttachment (byte[] _attachmentData, string _attachmentFileName, string _MIMEType)
		{
			AttachmentData		= _attachmentData;
			AttachmentFileName	= _attachmentFileName;
			MimeType			= _MIMEType;
		}

		#endregion
	}
}