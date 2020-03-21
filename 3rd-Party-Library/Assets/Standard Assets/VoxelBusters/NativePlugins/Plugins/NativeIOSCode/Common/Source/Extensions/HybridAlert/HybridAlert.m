//
//  HybridAlert.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 11/02/16.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "HybridAlert.h"

@interface HybridAlert ()

// Properties
#ifdef __IPHONE_8_0
@property(nonatomic, retain) 	UIAlertController 	*alertController;
#endif
@property(nonatomic, retain) 	UIAlertView 		*alertView;
@property(nonatomic)			BOOL				useAlertController;

@end

@implementation HybridAlert

@synthesize delegate;
@synthesize tag;
@synthesize style;
#ifdef __IPHONE_8_0
@synthesize alertController;
#endif
@synthesize alertView;
@synthesize useAlertController;

#pragma mark - Static Methods

+ (HybridAlert *)createAlertViewWithTitle:(NSString *)title
								  message:(NSString *)message
								  buttons:(NSArray *)buttons
{
	HybridAlert *newHybridAlert	= [[[HybridAlert alloc] init] autorelease];
	
	// Set properties
	[newHybridAlert setTitle:title];
	[newHybridAlert setMessage:message];
	[newHybridAlert setButtons:buttons];
	
	return newHybridAlert;
}

#pragma mark - Lifecycle Methods

- (id)init
{
	self 	= [super init];
	
	if (self)
	{
		self.useAlertController		= SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"8.0");
		
#ifdef __IPHONE_8_0
		if ([self useAlertController])
		{
			self.alertController	= [UIAlertController alertControllerWithTitle:NULL
																	   message:NULL
																preferredStyle:UIAlertControllerStyleAlert];
		}
		else
#endif
		{
			self.alertView			= [[[UIAlertView alloc] init] autorelease];
			self.alertView.delegate	= self;
		}
		
		self.delegate		= nil;
		self.tag			= nil;
		self.style			= HybridAlertStyleDefault;
	}
	
	return self;
}

- (void)dealloc
{
	self.tag				= nil;
#ifdef __IPHONE_8_0
	self.alertController	= nil;
#endif
	self.alertView			= nil;
	
	[super dealloc];
}

#pragma mark - Methods

- (void)setTitle:(NSString *)title
{
#ifdef __IPHONE_8_0
	if ([self useAlertController])
	{
		[self.alertController setTitle:title];
	}
	else
#endif
	{
		[self.alertView setTitle:title];
	}
}

- (void)setMessage:(NSString *)message
{
#ifdef __IPHONE_8_0
	if ([self useAlertController])
	{
		[self.alertController setMessage:message];
	}
	else
#endif
	{
		[self.alertView setMessage:message];
	}
}

- (void)setButtons:(NSArray *)buttons
{
#ifdef __IPHONE_8_0
	if ([self useAlertController])
	{
		for (NSString *buttonLabel in buttons)
		{
			UIAlertAction *buttonAction 	= [UIAlertAction actionWithTitle:buttonLabel
																	style:UIAlertActionStyleDefault
																  handler:^(UIAlertAction * action) {
																	  [self onButtonPressed:action.title];
																  }];
			
			[self.alertController addAction:buttonAction];
		}
	}
	else
#endif
	{
		for (NSString* button in buttons)
			[self.alertView addButtonWithTitle:button];
	}
}

- (void)presentFromViewController:(UIViewController *)viewController
{
#ifdef __IPHONE_8_0
	if ([self useAlertController])
	{
		[viewController presentViewController:self.alertController
									 animated:YES
								   completion:nil];
	}
	else
#endif
	{
		[[self alertView] show];
	}
}

- (void)setStyle:(HybridAlertStyle)newStyle
{
	style	= newStyle;
	
#ifdef __IPHONE_8_0
	if ([self useAlertController])
	{
		switch (newStyle)
		{
			case HybridAlertStylePlainTextInput:
				[[self alertController] addTextFieldWithConfigurationHandler:nil];
    break;
				
			case HybridAlertStyleSecureTextInput:
				[[self alertController] addTextFieldWithConfigurationHandler:^(UITextField * _Nonnull textField) {
					[textField setSecureTextEntry:YES];
				}];
    break;
				
			case HybridAlertStyleLoginAndPasswordInput:
				[[self alertController] addTextFieldWithConfigurationHandler:nil];
				[[self alertController] addTextFieldWithConfigurationHandler:^(UITextField * _Nonnull textField) {
					[textField setSecureTextEntry:YES];
				}];
    break;
				
			default:
    break;
		}
	}
	else
#endif
	{
		[[self alertView] setAlertViewStyle:(UIAlertViewStyle)newStyle];
	}
}

- (UITextField *)textFieldAtIndex:(NSInteger)index
{
#ifdef __IPHONE_8_0
	if ([self useAlertController])
	{
		return [[[self alertController] textFields] objectAtIndex:index];
	}
	else
#endif
	{
		return [[self alertView] textFieldAtIndex:index];
	}
}

#pragma mark - Callback Methods

- (void)onButtonPressed:(NSString *)button
{
	if (self.delegate  && [self.delegate respondsToSelector:@selector(alert:clickedButton:)])
	{
		[self.delegate alert:self clickedButton:button];
	}
}

- (void)alertView:(UIAlertView *)alertDialog clickedButtonAtIndex:(NSInteger)buttonIndex
{
	NSString *button	= [alertDialog buttonTitleAtIndex:buttonIndex];
	
	[self onButtonPressed:button];
}

@end
