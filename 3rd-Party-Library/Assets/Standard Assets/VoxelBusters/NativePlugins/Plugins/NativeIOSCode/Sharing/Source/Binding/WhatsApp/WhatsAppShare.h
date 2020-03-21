//
//  WhatsAppShare.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 13/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

typedef void (^WhatsAppShareCompletionHandler)(bool);

@interface WhatsAppShare : NSObject <UIDocumentInteractionControllerDelegate>

// Properties
@property(nonatomic, retain)    UIDocumentInteractionController *documentInteractionController;
@property(nonatomic, copy)		WhatsAppShareCompletionHandler	shareCompletionHandler;
@property(nonatomic)			bool							didCompleteSharing;

// Static methods
+ (bool)IsWhatsAppServiceAvailable;

// Related to sharing
- (void)shareTextMessage:(NSString *)message
			  completion:(WhatsAppShareCompletionHandler)completion;
- (void)shareImage:(UIImage *)image
		completion:(WhatsAppShareCompletionHandler)completion;

@end
