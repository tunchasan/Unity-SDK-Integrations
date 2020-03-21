//
//  WhatsAppShare.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 13/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "WhatsAppShare.h"
#import "UIHandler.h"

@implementation WhatsAppShare

@synthesize documentInteractionController;
@synthesize shareCompletionHandler;
@synthesize didCompleteSharing;

- (id)init
{
	self	= [super init];
	
	if (self)
	{
		self.documentInteractionController	= nil;
		self.shareCompletionHandler			= nil;
		self.didCompleteSharing				= NO;
	}
	
	return self;
}

- (void)dealloc
{
	// Release
	self.documentInteractionController	= nil;
	self.shareCompletionHandler			= nil;
	
	[super dealloc];
}

#pragma mark - Static Methods

+ (bool)IsWhatsAppServiceAvailable
{
	bool available	= [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"whatsapp://app"]];
	NSLog(@"[WhatsAppShare] is available: %d", available);
	
	return available;
}

#pragma mark - Sharing Methods

- (void)shareTextMessage:(NSString *)message
			  completion:(WhatsAppShareCompletionHandler)completion
{
	NSLog(@"[WhatsAppShare] sharing message: %@", message);
	bool canShare	= [WhatsAppShare IsWhatsAppServiceAvailable];
	
	// Open app to share
	if (canShare)
	{
		NSString *messageURLString	= [NSString stringWithFormat:@"whatsapp://send?text=%@", message];
		NSURL *messageURL         	= [NSURL URLWithString:[messageURLString stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding]];

		[[UIApplication sharedApplication] openURL:messageURL];
	}
	
	// Completion block
	if (completion != NULL)
		completion(canShare);
}

- (void)shareImage:(UIImage *)image
		completion:(WhatsAppShareCompletionHandler)completion
{
	NSLog(@"[WhatsAppShare] sharing image");
	
	// Sharing image failed as its null
	if (![WhatsAppShare IsWhatsAppServiceAvailable] || image == NULL)
	{
		NSLog(@"[WhatsAppShare] sharing failed");
		
		if (completion != NULL)
			completion(NO);
		
		return;
	}
	
	NSError *error	 	= nil;
	NSURL *documentURL 	= [[NSFileManager defaultManager] URLForDirectory:NSDocumentDirectory
																 inDomain:NSUserDomainMask
														appropriateForURL:nil
																   create:NO
																	error:&error];
	
	if (!documentURL)
	{
		NSLog(@"[WhatsAppShare] sharing failed");
		
		if (completion != NULL)
			completion(NO);
		
		return;
	}
	
	NSURL *tempFile		= [documentURL URLByAppendingPathComponent:@"whatsAppTmp.wai"];
	NSData *imageData	= UIImageJPEGRepresentation(image, 1.0);
	
	if (![imageData writeToURL:tempFile options:NSDataWritingAtomic error:&error])
	{
		NSLog(@"[WhatsAppShare] sharing failed");
		
		if (completion != NULL)
			completion(NO);
		
		return;
	}
	
	// Set initial values
	self.didCompleteSharing			= NO;
	self.shareCompletionHandler		= completion;
    
	// Create interaction controller
	self.documentInteractionController          = [UIDocumentInteractionController interactionControllerWithURL:tempFile];
	self.documentInteractionController.UTI      = @"net.whatsapp.image";
	self.documentInteractionController.delegate = self;
	
	// Present view
	CGRect menuRect;
	menuRect.origin			= [[UIHandler Instance] popoverPoint];
	menuRect.size			= CGSizeMake(1, 1);
	[self.documentInteractionController presentOpenInMenuFromRect:menuRect
														   inView:UnityGetGLView()
														 animated:YES];
}

#pragma mark - Delegate

- (void)documentInteractionController:(UIDocumentInteractionController *)controller
		willBeginSendingToApplication:(NSString *)application
{
	self.didCompleteSharing	= YES;
}

- (void)documentInteractionControllerDidDismissOpenInMenu:(UIDocumentInteractionController *)controller
{
	NSLog(@"[WhatsAppShare] did dismiss menu, sharing is completed: %d", self.didCompleteSharing);
	
	// Completion block
    if ([self shareCompletionHandler] != NULL)
	{
		self.shareCompletionHandler(self.didCompleteSharing);
		[shareCompletionHandler release], shareCompletionHandler = nil;
	}
    
    // Release document interactor
    self.documentInteractionController  = NULL;
}

@end
