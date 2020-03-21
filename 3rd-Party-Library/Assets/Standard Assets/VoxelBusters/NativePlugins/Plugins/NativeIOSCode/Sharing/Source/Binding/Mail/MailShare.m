//
//  MailShare.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 13/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "MailShare.h"

@implementation MailShare

#define kMailShareFinished 		"MailShareFinished"

#pragma mark - Sharing

- (BOOL)canSendMail
{
    bool canSend	= [MFMailComposeViewController canSendMail];
	NSLog(@"[MailShare] can send mail: %d", canSend);
	
	return canSend;
}

- (void)sendMailWithSubject:(NSString *)subject
					andBody:(NSString *)body
					 isHtml:(BOOL)isHTML
			   toRecipients:(NSArray *)toRecipients
			   ccRecipients:(NSArray *)ccRecipients
			  bccRecipients:(NSArray *)bccRecipients
		alongWithAttachment:(NSData *)attachmentData
					 ofType:(NSString *)mimeType
		   havingFileNameAs:(NSString *)filename
{
	// Check if we can send mail, if not then send failed
	if (![self canSendMail])
	{
		[self onFinishingMailShare:MFMailComposeResultFailed];
		return;
	}
	
	// User mail composer to send mail
	MFMailComposeViewController* controller = [[[MFMailComposeViewController alloc] init] autorelease];
	
	// Set properties
	[controller setSubject:subject];
	[controller setMessageBody:body isHTML:isHTML];
	[controller setToRecipients:toRecipients];
	[controller setCcRecipients:ccRecipients];
	[controller setBccRecipients:bccRecipients];
	[controller setMailComposeDelegate:self];
	
	if (attachmentData)
		[controller addAttachmentData:attachmentData mimeType:mimeType fileName:filename];
	
	// Present it
	[UnityGetGLViewController() presentViewController:controller
											 animated:YES
										   completion:nil];
}

- (void)onFinishingMailShare:(MFMailComposeResult)result
{
	// Notify Unity
	NSString *resultStr	= [NSString stringWithFormat:@"%d", result];
	
	NotifyEventListener(kMailShareFinished, [resultStr UTF8String]);
}

#pragma mark - Delegate

- (void)mailComposeController:(MFMailComposeViewController *)controller
          didFinishWithResult:(MFMailComposeResult)result
                        error:(NSError *)error
{
	NSLog(@"[MailShare] did finish with result: %d", result);
	
	// Invoke handler
	[self onFinishingMailShare:result];
	
	// Dismiss
	[UnityGetGLViewController()  dismissViewControllerAnimated:YES
													completion:nil];
}

@end