//
//  WebViewToolBar.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 19/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "WebViewToolBar.h"

@implementation WebViewToolBar

@synthesize backButton;
@synthesize stopButton;
@synthesize reloadButton;
@synthesize forwardButton;
@synthesize toolbarDelegate;

- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
   
    if (self)
    {
        // Set buttons
        self.backButton     		= [[[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemRewind
																			   target:self
																			   action:@selector(onPressingBack)] autorelease];
        
        self.stopButton     		= [[[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemStop
																			   target:self
																			   action:@selector(onPressingStop)] autorelease];
        
        self.reloadButton   		= [[[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemRefresh
																			   target:self
																			   action:@selector(onPressingReload)] autorelease];
        
        self.forwardButton  		= [[[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemFastForward
																			   target:self
																			   action:@selector(onPressingForward)] autorelease];
		
		UIBarButtonItem *flexispace	= [[[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemFlexibleSpace
																					 target:self
																					 action:Nil] autorelease];
        
		// Set items
		[self setItems:@[self.backButton, 		flexispace,
						 self.forwardButton, 	flexispace,
						 self.reloadButton, 	flexispace,
						 self.stopButton]];
		
        // Set style
        [self setBarStyle:UIBarStyleBlack];
		
		// Default properties
		[self setCanGoBack:NO];
		[self setCanGoForward:NO];
		[self setCanRefresh:YES];
    }
    return self;
}

- (void)dealloc
{
    self.backButton     = NULL;
    self.stopButton  	= NULL;
    self.reloadButton   = NULL;
    self.forwardButton	= NULL;
    
    [super dealloc];
}

#pragma mark - button states

- (void)setCanGoBack:(BOOL)canGoBack
{
    self.backButton.enabled     = canGoBack;
}

- (void)setCanStop:(BOOL)canStop
{
	self.stopButton.enabled		= canStop;
}

- (void)setCanRefresh:(BOOL)canRefresh
{
	self.reloadButton.enabled	= canRefresh;
}

- (void)setCanGoForward:(BOOL)canGoForward
{
	self.forwardButton.enabled	= canGoForward;
}

#pragma mark - button callbacks

- (void)onPressingBack
{
    if (toolbarDelegate != NULL)
        [toolbarDelegate onPressingBack];
}

- (void)onPressingStop
{
    if (toolbarDelegate != NULL)
        [toolbarDelegate onPressingStop];
}

- (void)onPressingReload
{
    if (toolbarDelegate != NULL)
        [toolbarDelegate onPressingReload];
}

- (void)onPressingForward
{
    if (toolbarDelegate != NULL)
        [toolbarDelegate onPressingForward];
}

@end
