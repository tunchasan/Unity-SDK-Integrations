//
//  HybridActionSheet.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 11/02/16.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "HybridActionSheet.h"

@interface HybridActionSheet ()

// Properties
#ifdef __IPHONE_8_0
@property(nonatomic, retain) 	UIAlertController 	*alertController;
#endif
@property(nonatomic, retain) 	UIActionSheet 		*actionSheet;
@property(nonatomic)			BOOL				useAlertController;

@end

@implementation HybridActionSheet

@synthesize delegate;
@synthesize sourceRect;
#ifdef __IPHONE_8_0
@synthesize alertController;
#endif
@synthesize actionSheet;
@synthesize useAlertController;

#pragma mark - Lifecycle Methods

- (id)init
{
	self 	= [super init];
	
	if (self)
	{
		self.useAlertController			= SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"8.0");
		
#ifdef __IPHONE_8_0
		if ([self useAlertController])
		{
			self.alertController		= [UIAlertController alertControllerWithTitle:NULL
																		message:NULL
																 preferredStyle:UIAlertControllerStyleActionSheet];
		}
		else
#endif
		{
			self.actionSheet			= [[[UIActionSheet alloc] init] autorelease];
			self.actionSheet.delegate	= self;
		}
		
		self.delegate		= nil;
	}
	
	return self;
}

- (void)dealloc
{
#ifdef __IPHONE_8_0
	self.alertController	= nil;
#endif
	self.actionSheet		= nil;
	
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
		[self.actionSheet setTitle:title];
	}
}

- (void)setCancelButtonTitle:(NSString *)button
{
#ifdef __IPHONE_8_0
	if ([self useAlertController])
	{
		UIAlertAction *buttonAction 	= [UIAlertAction actionWithTitle:button
																style:UIAlertActionStyleCancel
															  handler:^(UIAlertAction * _Nonnull action) {
																  [self onButtonPressed:action.title];
															  }];
		
		[self.alertController addAction:buttonAction];
	}
	else
#endif
	{
		[self.actionSheet addButtonWithTitle:button];
		[self.actionSheet setCancelButtonIndex:[self.actionSheet numberOfButtons] - 1];
	}
}

- (void)setDestructiveButtonTitle:(NSString *)button
{
#ifdef __IPHONE_8_0
	if ([self useAlertController])
	{
		UIAlertAction *buttonAction 	= [UIAlertAction actionWithTitle:button
																style:UIAlertActionStyleDestructive
															  handler:^(UIAlertAction * _Nonnull action) {
																  [self onButtonPressed:action.title];
															  }];
		
		[self.alertController addAction:buttonAction];
	}
	else
#endif
	{
		[self.actionSheet addButtonWithTitle:button];
		[self.actionSheet setDestructiveButtonIndex:[self.actionSheet numberOfButtons] - 1];
	}
}

- (void)setOtherButtons:(NSArray *)buttons
{
#ifdef __IPHONE_8_0
	if ([self useAlertController])
	{
		for (NSString *buttonLabel in buttons)
		{
			UIAlertAction *buttonAction 	= [UIAlertAction actionWithTitle:buttonLabel
																	style:UIAlertActionStyleDefault
																  handler:^(UIAlertAction * _Nonnull action) {
																	  [self onButtonPressed:action.title];
																  }];
			
			[self.alertController addAction:buttonAction];
		}
	}
	else
#endif
	{
		for (NSString* button in buttons)
			[self.actionSheet addButtonWithTitle:button];
	}
}

- (void)presentFromViewController:(UIViewController *)viewController animated: (BOOL)flag completion:(void (^ __nullable)(void))completion
{
#ifdef __IPHONE_8_0
	if ([self useAlertController])
	{
		UIPopoverPresentationController *popover = self.alertController.popoverPresentationController;
		
		if (popover)
		{
			popover.sourceView 					= viewController.view;
			popover.sourceRect 					= [self sourceRect];
			popover.permittedArrowDirections 	= UIPopoverArrowDirectionAny;
			popover.delegate					= self;
		}
		
		[viewController presentViewController:self.alertController animated:flag completion:completion];
	}
	else
#endif
	{
		if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad)
		{
			[[self actionSheet] showFromRect:[self sourceRect]
									  inView:[viewController view]
									animated:flag];
		}
		else
		{
			[[self actionSheet] showInView:[viewController view]];
		}
	}
}

#pragma mark - Callback Methods

- (void)onButtonPressed:(NSString *)button
{
	if (self.delegate  && [self.delegate respondsToSelector:@selector(actionSheet:clickedButton:)])
	{
		[self.delegate actionSheet:self clickedButton:button];
	}
}

- (void)popoverPresentationControllerDidDismissPopover:(UIPopoverPresentationController *)popoverPresentationController
{
	[self onButtonPressed:nil];
}

- (void)actionSheet:(UIActionSheet *)actionSheet didDismissWithButtonIndex:(NSInteger)buttonIndex
{
	NSString *buttonPressed	= NULL;
	
	if (buttonIndex >= 0)
	{
		buttonPressed = [actionSheet buttonTitleAtIndex:buttonIndex];
	}
	
	[self onButtonPressed:buttonPressed];
}

@end
