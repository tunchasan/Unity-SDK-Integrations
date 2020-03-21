using UnityEngine;
using System.Collections;
using VoxelBusters.NativePlugins;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins.Demo
{
#if !USES_SHARING
	public class SharingDemo : NPDisabledFeatureDemo 
#else
	public class SharingDemo : NPDemoBase 
#endif
	{
		#region Properties

#pragma warning disable
		[SerializeField, Header("Message Sharing Properties")]
		private 	string			m_smsBody			= "SMS body holds text message that needs to be sent to recipients";
		[SerializeField]
		private 	string[] 		m_smsRecipients;
		
		[SerializeField, Header("Mail Sharing Properties")]
		private 	string			m_mailSubject		= "Demo Mail";
		[SerializeField]
		private 	string			m_plainMailBody		= "This is plain text mail.";
		[SerializeField]
		private 	string			m_htmlMailBody		= "<html><body><h1>Hello</h1></body></html>";
		[SerializeField]
		private 	string[] 		m_mailToRecipients;
		[SerializeField]
		private 	string[] 		m_mailCCRecipients;
		[SerializeField]
		private 	string[] 		m_mailBCCRecipients;

		[SerializeField, Header("Share Sheet Properties")]
		private 	eShareOptions[]	m_excludedOptions	= new eShareOptions[0];

		[SerializeField, Header("Share Properties ")]
		private 	string			m_shareMessage		= "share message";
		[SerializeField]
		private 	string			m_shareURL			= "http://www.google.com";

		[SerializeField, Tooltip ("This demo consideres image relative to Application.persistentDataPath")]
		private 	string 			m_shareImageRelativePath;
#pragma warning restore

		#endregion

#if !USES_SHARING
	}
#else
		#region Unity Methods

		protected override void Start ()
		{
			base.Start ();

			// Set additional info texts
			AddExtraInfoTexts(
				"When it comes to WhatsApp sharing, there is one major limitation on iOS platform i.e, you can either share only image or only text but not both. While sharing if both properties are set, then only image will be shared.");
		}

		#endregion

		#region GUI Methods
		
		protected override void DisplayFeatureFunctionalities ()
		{
			base.DisplayFeatureFunctionalities ();
			
			DrawMessageShareSection ();
			DrawMailShareSection ();
			DrawFBShareSection ();
			DrawTwitterShareSection ();
			DrawWhatsAppShareSection ();
			DrawSocialShareSheetSection ();
			DrawShareSheetSection ();
		}
		
		private void DrawMessageShareSection ()
		{	
			GUILayout.Label("Message Share", kSubTitleStyle);
			
			if (GUILayout.Button ("Is Messaging Available"))
			{
				AddNewResult("IsMessagingAvailable=" + IsMessagingServiceAvailable());
			}
			
			if (GUILayout.Button("Send Text Message"))
			{
				SendTextMessage();
			}
		}
		
		private void DrawMailShareSection ()
		{
			GUILayout.Label("Mail Share", kSubTitleStyle);
			
			if (GUILayout.Button("Is Mail Available"))
			{
				AddNewResult("Can Send Mail = " + IsMailServiceAvailable());
			}
			
			if (GUILayout.Button("Send Plain Text Mail"))
			{
				SendPlainTextMail();
			}
			
			if (GUILayout.Button("Send HTML Text Mail"))
			{
				SendHTMLTextMail();
			}
			
			if (GUILayout.Button("Send Mail With Screenshot"))
			{
				SendMailWithScreenshot();
			}
			
			if (GUILayout.Button("Send Mail With Attachment : Path"))
			{
				SendMailWithAttachment();
			}
		}

		private void DrawFBShareSection ()
		{
			GUILayout.Label("FB Share", kSubTitleStyle);
			
			if (GUILayout.Button("Is FB Share Service Available"))
			{
				AddNewResult("Can Share On FB = " + IsFBShareServiceAvailable());
			}
			
			if (GUILayout.Button("Share Text Message On FB"))
			{
				ShareTextMessageOnFB();
			}
			
			if (GUILayout.Button("Share URL On FB"))
			{
				ShareURLOnFB();
			}
			
			if (GUILayout.Button("Share Screenshot On FB"))
			{
				ShareScreenshotOnFB();
			}
			
			if (GUILayout.Button("Share Image On FB"))
			{
				ShareImageOnFB();
			}
		}

		private void DrawTwitterShareSection ()
		{
			GUILayout.Label("Twitter Share", kSubTitleStyle);
			
			if (GUILayout.Button("Is Twitter Share Service Available"))
			{
				AddNewResult("Can Share On Twitter = " + IsTwitterShareServiceAvailable());
			}
			
			if (GUILayout.Button("Share Text Message On Twitter"))
			{
				ShareTextMessageOnTwitter();
			}
			
			if (GUILayout.Button("ShareURLOnTwitter"))
			{
				ShareURLOnTwitter();
			}
			
			if (GUILayout.Button("Share Screenshot On Twitter"))
			{
				ShareScreenshotOnTwitter();
			}
			
			if (GUILayout.Button("Share Image On Twitter"))
			{
				ShareImageOnTwitter();
			}
		}
		
		private void DrawWhatsAppShareSection ()
		{
			GUILayout.Label("WhatsApp Share", kSubTitleStyle);
			
			if (GUILayout.Button("Is WhatsApp Service Available"))
			{
				AddNewResult("Can Share On WhatsApp = " + IsWhatsAppServiceAvailable());
			}
			
			if (GUILayout.Button("ShareTextMessageOnWhatsApp"))
			{
				ShareTextMessageOnWhatsApp();
			}
			
			if (GUILayout.Button("Share Screenshot On WhatsApp"))
			{
				ShareScreenshotOnWhatsApp();
			}
			
			if (GUILayout.Button("Share Image On WhatsApp"))
			{
				ShareImageOnWhatsApp();
			}
		}
		
		private void DrawSocialShareSheetSection ()
		{
			GUILayout.Label("Social Share Sheet", kSubTitleStyle);
			
			if (GUILayout.Button("Share Text Message On SocialNetwork"))
			{
				ShareTextMessageOnSocialNetwork();
			}
			
			if (GUILayout.Button("Share URL On SocialNetwork"))
			{
				ShareURLOnSocialNetwork();
			}
			
			if (GUILayout.Button("Share ScreenShot On SocialNetwork"))
			{
				ShareScreenShotOnSocialNetwork();
			}
			
			if (GUILayout.Button("Share Image On SocialNetwork"))
			{
				ShareImageOnSocialNetwork();
			}
		}
		
		private void DrawShareSheetSection ()
		{
			GUILayout.Label("Share Sheet", kSubTitleStyle);
			
			if (GUILayout.Button("Share Text Message Using ShareSheet"))
			{
				ShareTextMessageUsingShareSheet();
			}
			
			if (GUILayout.Button("Share URL Using ShareSheet"))
			{
				ShareURLUsingShareSheet();
			}
			
			if (GUILayout.Button("Share ScreenShot Using ShareSheet"))
			{
				ShareScreenShotUsingShareSheet();
			}
			
			if (GUILayout.Button("Share Image At Path Using ShareSheet"))
			{
				ShareImageAtPathUsingShareSheet();
			}
		}
		
		#endregion

		#region Message Sharing API Methods

		private bool IsMessagingServiceAvailable ()
		{
			return NPBinding.Sharing.IsMessagingServiceAvailable();
		}

		private void SendTextMessage ()
		{
			// Create composer
			MessageShareComposer	_composer	= new MessageShareComposer();
			_composer.Body						= m_smsBody;
			_composer.ToRecipients				= m_smsRecipients;

			// Show message composer
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);
		}
		
		#endregion
		
		#region Mail Sharing API Methods
		
		private bool IsMailServiceAvailable ()
		{
			return NPBinding.Sharing.IsMailServiceAvailable();
		}

		private void SendPlainTextMail ()
		{
			// Create composer
			MailShareComposer	_composer	= new MailShareComposer();
			_composer.Subject				= m_mailSubject;
			_composer.Body					= m_plainMailBody;
			_composer.IsHTMLBody			= false;
			_composer.ToRecipients			= m_mailToRecipients;
			_composer.CCRecipients			= m_mailCCRecipients;
			_composer.BCCRecipients			= m_mailBCCRecipients;
			
			// Show share view
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);
		}

		private void SendHTMLTextMail () 
		{
			// Create composer
			MailShareComposer	_composer	= new MailShareComposer();
			_composer.Subject				= m_mailSubject;
			_composer.Body					= m_htmlMailBody;
			_composer.IsHTMLBody			= true;
			_composer.ToRecipients			= m_mailToRecipients;
			_composer.CCRecipients			= m_mailCCRecipients;
			_composer.BCCRecipients			= m_mailBCCRecipients;

			// Show share view
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);
		}

		private void SendMailWithScreenshot ()
		{
			// Create composer
			MailShareComposer	_composer	= new MailShareComposer();
			_composer.Subject				= m_mailSubject;
			_composer.Body					= m_plainMailBody;
			_composer.IsHTMLBody			= false;
			_composer.ToRecipients			= m_mailToRecipients;
			_composer.CCRecipients			= m_mailCCRecipients;
			_composer.BCCRecipients			= m_mailBCCRecipients;
			_composer.AttachScreenShot();
			
			// Show share view
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);
		}

		private void SendMailWithAttachment ()
		{
			// Create composer
			MailShareComposer	_composer	= new MailShareComposer();
			_composer.Subject				= m_mailSubject;
			_composer.Body					= m_plainMailBody;
			_composer.IsHTMLBody			= false;
			_composer.ToRecipients			= m_mailToRecipients;
			_composer.CCRecipients			= m_mailCCRecipients;
			_composer.BCCRecipients			= m_mailBCCRecipients;
			_composer.AddAttachmentAtPath(GetImageFullPath(), MIMEType.kPNG);
			
			// Show share view
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);
		}

		#endregion

		#region FB Sharing API Methods

		private bool IsFBShareServiceAvailable ()
		{
			return NPBinding.Sharing.IsFBShareServiceAvailable();
		}

		private void ShareTextMessageOnFB ()
		{
			// Create composer
			FBShareComposer _composer	= new FBShareComposer();
			_composer.Text				= m_shareMessage;
			
			// Show share view
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);			
		}

		private void ShareURLOnFB ()
		{
			// Create share sheet
			FBShareComposer _composer	= new FBShareComposer();
			_composer.Text				= m_shareMessage;
			_composer.URL				= m_shareURL;
			
			// Show composer
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);
		}
		
		private void ShareScreenshotOnFB ()
		{
			// Create composer
			FBShareComposer _composer	= new FBShareComposer();
			_composer.Text				= m_shareMessage;
			_composer.AttachScreenShot();
			
			// Show share view
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);
		}
		
		private void ShareImageOnFB ()
		{
			// Create composer
			FBShareComposer _composer	= new FBShareComposer();
			_composer.Text				= m_shareMessage;
			_composer.AttachImageAtPath(GetImageFullPath());
			
			// Show share view
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);
		}

		#endregion

		#region Twitter Sharing API Methods
		
		private bool IsTwitterShareServiceAvailable ()
		{
			return NPBinding.Sharing.IsTwitterShareServiceAvailable();
		}
		
		private void ShareTextMessageOnTwitter ()
		{
			// Create composer
			TwitterShareComposer _composer	= new TwitterShareComposer();
			_composer.Text					= m_shareMessage;
			
			// Show share view
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);			
		}
		
		private void ShareURLOnTwitter ()
		{
			// Create share sheet
			TwitterShareComposer _composer	= new TwitterShareComposer();
			_composer.Text					= m_shareMessage;
			_composer.URL					= m_shareURL;
			
			// Show composer
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);			
		}
		
		private void ShareScreenshotOnTwitter ()
		{
			// Create composer
			TwitterShareComposer _composer	= new TwitterShareComposer();
			_composer.Text					= m_shareMessage;
			_composer.AttachScreenShot();
			
			// Show share view
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);			
		}
		
		private void ShareImageOnTwitter ()
		{
			// Create composer
			TwitterShareComposer _composer	= new TwitterShareComposer();
			_composer.Text					= m_shareMessage;
			_composer.AttachImageAtPath(GetImageFullPath());
			
			// Show share view
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);			
		}

		#endregion
		
		#region WhatsApp Sharing API Methods
		
		private bool IsWhatsAppServiceAvailable ()
		{
			return NPBinding.Sharing.IsWhatsAppServiceAvailable();
		}

		private void ShareTextMessageOnWhatsApp ()
		{
			// Create composer
			WhatsAppShareComposer _composer	= new WhatsAppShareComposer();
			_composer.Text					= m_shareMessage;

			// Show share view
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);			
		}

		private void ShareScreenshotOnWhatsApp ()
		{
			// Create composer
			WhatsAppShareComposer _composer	= new WhatsAppShareComposer();
			_composer.AttachScreenShot();
			
			// Show share view
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);	
		}
		
		private void ShareImageOnWhatsApp ()
		{
			// Create composer
			WhatsAppShareComposer _composer	= new WhatsAppShareComposer();
			_composer.AttachImageAtPath(GetImageFullPath());
			
			// Show share view
			NPBinding.Sharing.ShowView(_composer, FinishedSharing);	
		}
	
		#endregion
		
		#region Social Share Sheet API Methods
		
		private void ShareTextMessageOnSocialNetwork ()
		{
			// Create share sheet
			SocialShareSheet _shareSheet 	= new SocialShareSheet();	
			_shareSheet.Text				= m_shareMessage;

			// Show composer
			NPBinding.UI.SetPopoverPointAtLastTouchPosition();
			NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);
		}

		private void ShareURLOnSocialNetwork ()
		{
			// Create share sheet
			SocialShareSheet _shareSheet 	= new SocialShareSheet();	
			_shareSheet.Text				= m_shareMessage;
			_shareSheet.URL					= m_shareURL;
			
			// Show composer
			NPBinding.UI.SetPopoverPointAtLastTouchPosition();
			NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);
		}

		private void ShareScreenShotOnSocialNetwork ()
		{
			// Create share sheet
			SocialShareSheet _shareSheet 	= new SocialShareSheet();	
			_shareSheet.Text				= m_shareMessage;
			_shareSheet.AttachScreenShot();

			// Show composer
			NPBinding.UI.SetPopoverPointAtLastTouchPosition();
			NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);
		}
		
		private void ShareImageOnSocialNetwork ()
		{
			// Create share sheet
			SocialShareSheet _shareSheet 	= new SocialShareSheet();	
			_shareSheet.Text				= m_shareMessage;
			_shareSheet.AttachImageAtPath(GetImageFullPath());
			
			// Show composer
			NPBinding.UI.SetPopoverPointAtLastTouchPosition();
			NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);
		}
		
		#endregion
		
		#region Share Sheet API Methods

		private void ShareTextMessageUsingShareSheet ()
		{
			// Create share sheet
			ShareSheet _shareSheet 	= new ShareSheet();	
			_shareSheet.Text		= m_shareMessage;
			_shareSheet.ExcludedShareOptions	= m_excludedOptions;

			// Show composer
			NPBinding.UI.SetPopoverPointAtLastTouchPosition();
			NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);
		}
		
		private void ShareURLUsingShareSheet ()
		{
			// Create share sheet
			ShareSheet _shareSheet 	= new ShareSheet();	
			_shareSheet.Text		= m_shareMessage;
			_shareSheet.URL			= m_shareURL;
			_shareSheet.ExcludedShareOptions	= m_excludedOptions;

			// Show composer
			NPBinding.UI.SetPopoverPointAtLastTouchPosition();
			NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);
		}
		
		private void ShareScreenShotUsingShareSheet ()
		{
			// Create share sheet
			ShareSheet _shareSheet 	= new ShareSheet();	
			_shareSheet.Text		= m_shareMessage;
			_shareSheet.ExcludedShareOptions	= m_excludedOptions;
			_shareSheet.AttachScreenShot();

			// Show composer
			NPBinding.UI.SetPopoverPointAtLastTouchPosition();
			NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);
		}
		
		private void ShareImageAtPathUsingShareSheet ()
		{
			// Create share sheet
			ShareSheet _shareSheet 	= new ShareSheet();	
			_shareSheet.Text		= m_shareMessage;
			_shareSheet.ExcludedShareOptions	= m_excludedOptions;
			_shareSheet.AttachImageAtPath(GetImageFullPath());
			
			// Show composer
			NPBinding.UI.SetPopoverPointAtLastTouchPosition();
			NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);
		}

		#endregion
		
		#region API Callback Methods
		
		private void FinishedSharing (eShareResult _result)
		{
			AddNewResult("Finished sharing");
			AppendResult("Share Result = " + _result);
		}
		
		#endregion

		#region Misc. Methods

		private string GetImageFullPath ()
		{
			return Application.persistentDataPath + "/" + m_shareImageRelativePath;
		}

		#endregion
	}
#endif
}