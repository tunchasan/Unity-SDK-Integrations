//
//  NPSharingBinding.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 13/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPSharingBinding.h"
#import "NPSharingHandler.h"

#define kWhatsAppShareFinished 	"WhatsAppShareFinished"

#pragma mark - Mail Share Methods

bool canSendMail ()
{
    return [[NPSharingHandler MailShare] canSendMail];
}

void sendMail (const char* subject, 		const char* body,  			bool isHTMLBody,
			   const char* toRecipients,	const char* ccRecipients, 	const char* bccRecipients,
			   UInt8* attachmentByteArray,	int byteArrayLength, 		const char* mimeType,
			   const char* attachmentFileNameWithExtn)
{
	[[NPSharingHandler MailShare] sendMailWithSubject:ConvertToNSString(subject)
											  andBody:ConvertToNSString(body)
											   isHtml:isHTMLBody
										 toRecipients:ConvertToNSArray(toRecipients)
										 ccRecipients:ConvertToNSArray(ccRecipients)
										bccRecipients:ConvertToNSArray(bccRecipients)
								  alongWithAttachment:[Utility CreateNSDataFromByteArray:attachmentByteArray ofLength:byteArrayLength]
											   ofType:ConvertToNSString(mimeType)
									 havingFileNameAs:ConvertToNSString(attachmentFileNameWithExtn)];
}

#pragma mark - Messaging Share Methods

bool isMessagingAvailable ()
{
    return [[NPSharingHandler MessagingShare] isMessagingAvailable];
}

void sendTextMessage (const char* body, const char* recipients)
{
    [[NPSharingHandler MessagingShare] sendMessageWithBody:ConvertToNSString(body)
											  toRecipients:ConvertToNSArray(recipients)];
}

#pragma mark - WhatsApp Share Methods

bool canShareOnWhatsApp ()
{
	return [WhatsAppShare IsWhatsAppServiceAvailable];
}

void shareTextMessageOnWhatsApp (const char* message)
{
	[[NPSharingHandler WhatsAppShare] shareTextMessage:ConvertToNSString(message)
											completion:^(bool completed) {
												
												// Notify unity
												NSString *completedStr	= [NSString stringWithFormat:@"%d", completed];
												
												NotifyEventListener(kWhatsAppShareFinished, [completedStr UTF8String]);
										  }];
}

void shareImageOnWhatsApp (UInt8* imageByteArray, int byteArrayLength)
{
	[[NPSharingHandler WhatsAppShare] shareImage:[Utility CreateImageFromByteArray:imageByteArray ofLength:byteArrayLength]
									  completion:^(bool completed) {
										  
										  // Notify unity
										  NSString *completedStr	= [NSString stringWithFormat:@"%d", completed];
										  
										  NotifyEventListener(kWhatsAppShareFinished, [completedStr UTF8String]);
									}];
}

#pragma mark - Social Share Methods

bool isSocialShareServiceAvailable (int serviceType)
{
	return [[NPSharingHandler SocialShare] isServiceTypeAvailable:(SocialShareServiceType)serviceType];
}

void socialShare (int serviceType, 			const char* message,    const char* URLString,
				  UInt8* imageByteArray,  	int byteArrayLength)
{
	[[NPSharingHandler SocialShare] share:(SocialShareServiceType)serviceType
							  withMessage:ConvertToNSString(message)
								  withURL:ConvertToNSString(URLString)
								 andImage:[Utility CreateImageFromByteArray:(void*)imageByteArray ofLength:byteArrayLength]];
}

#pragma mark - Share

void share (const char* message,    const char* URLString,	UInt8* imageByteArray,
			int byteArrayLength,	const char *excludedOptions)
{
    [[NPSharingHandler Instance] shareMessage:ConvertToNSString(message)
										  URL:ConvertToNSString(URLString)
									 andImage:[Utility CreateImageFromByteArray:(void*)imageByteArray ofLength:byteArrayLength]
						  withExcludedSharing:ConvertToNSArray(excludedOptions)];
}