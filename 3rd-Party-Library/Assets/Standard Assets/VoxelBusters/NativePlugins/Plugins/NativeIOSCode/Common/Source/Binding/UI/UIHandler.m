//
//  UIHandler.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 17/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "UIHandler.h"

@implementation UIHandler

#define kAlertDialogClosed			"AlertDialogClosed"
#define kSingleFieldDialogClosed	"SingleFieldPromptDialogClosed"
#define kLoginPromptDialogClosed	"LoginPromptDialogClosed"

#define tAlertView            		99
#define tSingleFieldPrompt    		100
#define tLoginPrompt         		101

@synthesize popoverPoint;

#pragma mark - Popover

- (void)setPopoverPoint:(CGPoint)newPoint
{
	NSLog(@"[UIHandler] setting popover point to %@", NSStringFromCGPoint(newPoint));
	popoverPoint = newPoint;
}

#pragma mark - UI Dialogs

- (void)showAlertViewWithTitle:(NSString *)title
                       message:(NSString *)message
                       buttons:(NSArray *)buttons
                  andCallerTag:(NSString*)cTag
{
	HybridAlert *alert	= [[HybridAlert createAlertViewWithTitle:title message:message buttons:buttons] retain];
	
	// Set properties
	[alert setTag:cTag];
	[alert setDelegate:self];
	
	// Display it
	[alert presentFromViewController:UnityGetGLViewController()];
}

- (void)showSingleFieldPromptWithTitle:(NSString *)title
                               message:(NSString *)message
                       placeHolderText:(NSString *)placeholder
                         ofSecuredType:(BOOL)useSecureText
                            andButtons:(NSArray *)buttons
{
	HybridAlert *alert	= [[HybridAlert createAlertViewWithTitle:title message:message buttons:buttons] retain];
	
	// Set properties
	[alert setDelegate:self];
	
	if (useSecureText)
		[alert setStyle:HybridAlertStyleSecureTextInput];
	else
		[alert setStyle:HybridAlertStylePlainTextInput];
	
	// Set placeholder text
	if (placeholder != NULL)
		[[alert textFieldAtIndex:0] setPlaceholder:placeholder];
	
	// Display it
	[alert presentFromViewController:UnityGetGLViewController()];
}

- (void)showLoginPromptWithTitle:(NSString *)title
                         message:(NSString *)message
                 placeHolderText:(NSString *)placeholder1 :(NSString *)placeholder2
                      andButtons:(NSArray *)buttons
{
	HybridAlert *alert	= [[HybridAlert createAlertViewWithTitle:title message:message buttons:buttons] retain];
	
	// Set properties
	[alert setDelegate:self];
	[alert setStyle:HybridAlertStyleLoginAndPasswordInput];
	
	// Set placeholder text
	if (placeholder1 != NULL)
		[[alert textFieldAtIndex:0] setPlaceholder:placeholder1];

	if (placeholder2 != NULL)
		[[alert textFieldAtIndex:1] setPlaceholder:placeholder2];
	
	// Display it
	[alert presentFromViewController:UnityGetGLViewController()];
}

#pragma mark - Delegate

#define kButtonPressed	@"button-pressed"
#define kCaller			@"caller"
#define kInputText		@"input"
#define kUsernameText	@"username"
#define kPasswordText	@"password"


- (void)alert:(HybridAlert *)alert clickedButton:(NSString *)button
{
    NSLog(@"[Dialogs] view was closed by pressing button %@", button);
    
    // Alertview
    if (alert.style == HybridAlertStyleDefault)
    {
		// Notify unity
		NSMutableDictionary *dataDict 	= [NSMutableDictionary dictionary];
		dataDict[kButtonPressed]   		= button;
        dataDict[kCaller]       		= [alert tag];
        
        NotifyEventListener(kAlertDialogClosed, ToJsonCString(dataDict));
    }
	// Login prompt
	else if (alert.style == HybridAlertStyleLoginAndPasswordInput)
	{
		NSString *usernameText    		= [alert textFieldAtIndex:0].text;
		NSString *passwordText     	 	= [alert textFieldAtIndex:1].text;
		
		// Notify unity
		NSMutableDictionary *dataDict  	= [NSMutableDictionary dictionary];
		dataDict[kButtonPressed]   		= button;

		if (usernameText)
			dataDict[kUsernameText]		= usernameText;
		
		if (passwordText)
			dataDict[kPasswordText]		= passwordText;
		
		NotifyEventListener(kLoginPromptDialogClosed, ToJsonCString(dataDict));
	}
	// Single field prompt
	else
    {
        NSString *promptText       	 	= [alert textFieldAtIndex:0].text;
	
		// Notify unity
		NSMutableDictionary *dataDict	= [NSMutableDictionary dictionary];
		dataDict[kButtonPressed]   		= button;
		
		if (promptText)
			dataDict[kInputText]		= promptText;
        
        NotifyEventListener(kSingleFieldDialogClosed, ToJsonCString(dataDict));
    }
	
	[alert release];
}
    
@end
