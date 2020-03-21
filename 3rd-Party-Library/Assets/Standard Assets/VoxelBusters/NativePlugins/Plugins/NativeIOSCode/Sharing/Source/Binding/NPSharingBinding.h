//
//  NPSharingBinding.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 13/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

// Mail share methods
UIKIT_EXTERN bool canSendMail ();
UIKIT_EXTERN void sendMail (const char* subject, 		const char* body,  			bool isHTMLBody,
							const char* toRecipients,	const char* ccRecipients, 	const char* bccRecipients,
							UInt8* attachmentByteArray,	int byteArrayLength, 		const char* mimeType,
							const char* attachmentFileNameWithExtn);

// Messaging share methods
UIKIT_EXTERN bool isMessagingAvailable ();
UIKIT_EXTERN void sendTextMessage (const char* body, const char* recipients);

// WhatsApp share methods
UIKIT_EXTERN bool canShareOnWhatsApp ();
UIKIT_EXTERN void shareTextMessageOnWhatsApp (const char* message);
UIKIT_EXTERN void shareImageOnWhatsApp (UInt8* imageByteArray, int byteArrayLength);

// Social share methods
UIKIT_EXTERN bool isSocialShareServiceAvailable (int serviceType);
UIKIT_EXTERN void socialShare (int serviceType, 		const char* message,    const char* URLString,
							   UInt8* imageByteArray,  	int byteArrayLength);

// Share methods
UIKIT_EXTERN void share (const char* message,   const char* URLString,	UInt8* imageByteArray,
						 int byteArrayLength,	const char *excludedOptions);