//
//  MailShare.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 13/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <MessageUI/MessageUI.h>

@interface MailShare : NSObject <MFMailComposeViewControllerDelegate, UINavigationControllerDelegate>

// Related to sharing
- (BOOL)canSendMail;
- (void)sendMailWithSubject:(NSString *)subject
					andBody:(NSString *)body
                     isHtml:(BOOL)isHTML
			   toRecipients:(NSArray *)toRecipients
			   ccRecipients:(NSArray *)ccRecipients
			  bccRecipients:(NSArray *)bccRecipients
        alongWithAttachment:(NSData *)attachmentData
					 ofType:(NSString *)mimeType
		   havingFileNameAs:(NSString *)filenamel;

@end
