//
//  EmbeddedVideoPlayerViewController.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 21/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "EmbeddedVideoPlayerViewController.h"

@interface EmbeddedVideoPlayerViewController ()

// Internal properties
@property (nonatomic) BOOL 	onFocus;
@property (nonatomic) BOOL	playbackHasFailedBeforeOnFocus;

@end

@implementation EmbeddedVideoPlayerViewController

@synthesize videoPlayer;
@synthesize delegate;

@synthesize onFocus;
@synthesize playbackHasFailedBeforeOnFocus;

#pragma mark - View Controller Methods

- (id)init
{
	self	= [super init];
	
	if (self)
	{
		// Add video player view
		self.videoPlayer		 	= [[[EmbeddedVideoPlayer alloc] init] autorelease];
		self.videoPlayer.delegate	= self;
		self.onFocus				= NO;
		self.playbackHasFailedBeforeOnFocus	= NO;
	}
	
	return self;
}

- (void)dealloc
{
	// Unregister delegate
	[self.videoPlayer setDelegate:nil];
	
	// Release
	self.videoPlayer	= nil;
	
	[super dealloc];
}

- (void)loadView
{
	[super loadView];
	
	// Add video player view
	[self.view addSubview:self.videoPlayer];
	
	// Set video player properties
	[self.videoPlayer setFrame:GetApplicationBounds()];
}

- (void)viewDidAppear:(BOOL)animated
{
	[super viewDidAppear:animated];
	
	// Mark as on focus
	[self setOnFocus:YES];
	
	// Check if we have any pending error
	if ([self playbackHasFailedBeforeOnFocus])
	{
		[self embeddedVideoPlayer:[self videoPlayer] didFinishPlaying:MovieFinishReasonPlaybackError];
		return;
	}
}

- (void)viewDidDisappear:(BOOL)animated
{
	// Reset flags
	[self setOnFocus:NO];
	[self setPlaybackHasFailedBeforeOnFocus:NO];
}

#pragma mark - Rotation Methods

- (BOOL)prefersStatusBarHidden
{
	return YES;
}

- (BOOL)shouldAutorotate
{
	return YES;
}

- (UIInterfaceOrientationMask)supportedInterfaceOrientations
{
	return UIInterfaceOrientationMaskAll;
}

#pragma mark - Video Player Delegates

- (void)embeddedVideoPlayer:(EmbeddedVideoPlayer *)player stateChangedTo:(MoviePlayerState)state
{
	NSLog(@"[EmbeddedPlayer] New state %lu", state);
}

- (void)embeddedVideoPlayer:(EmbeddedVideoPlayer *)player didFinishPlaying:(MovieFinishReason)reason {
	// Check if are player controller is on focus or not
	if (![self onFocus]) {
		[self setPlaybackHasFailedBeforeOnFocus:YES];
		return;
	}
	
	// Send callback
	if ([self delegate] != nil && [[self delegate] conformsToProtocol:@protocol(EmbeddedVideoPlayerViewControllerDelegate)]) {
		[[self delegate] embeddedVideoPlayerViewController:self didFinishPlaying:reason];
	}
}

@end
