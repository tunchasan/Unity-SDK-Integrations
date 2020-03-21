//
//  EmbeddedVideoPlayer.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 21/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "EmbeddedVideoPlayer.h"

const NSString *kEVPlayerScheme					= @"embeddedplayer";

const NSString *kEVPlayerEventOnReady			= @"OnReady";
const NSString *kEVPlayerEventOnStateChange		= @"OnStateChange";
const NSString *kEVPlayerEventOnError			= @"OnError";

const NSString *kEVPlayerStateUnstartedCode		= @"UNSTARTED";
const NSString *kEVPlayerStateEndedCode			= @"ENDED";
const NSString *kEVPlayerStatePlayingCode		= @"PLAYING";
const NSString *kEVPlayerStatePausedCode		= @"PAUSED";
const NSString *kEVPlayerStateBufferingCode		= @"BUFFERING";
const NSString *kEVPlayerStateCuedCode			= @"CUED";
const NSString *kEVPlayerStateUserExitedCode	= @"USER_EXITED";
const NSString *kEVPlayerStateUnknownCode		= @"UNKNOWN";

const NSString *kEVPlayerCommandStopVideo		= @"stopVideo();";
const NSString *kEVPlayerCommandPlayVideo		= @"playVideo();";
const NSString *kEVPlayerCommandPauseVideo		= @"pauseVideo();";
const NSString *kEVPlayerCommandGetState		= @"getPlayerState();";
const NSString *kEVPlayerCommandResizeView		= @"resizePlayer();";

@interface EmbeddedVideoPlayer ()

// Properties
@property(nonatomic)			BOOL		playbackHasEnded;
@property(nonatomic, retain)	NSString*	playerInternalStateCode;

@end

@implementation EmbeddedVideoPlayer

@synthesize embeddedHTMLString;
@synthesize delegate;
@synthesize playbackState = _playbackState;

@synthesize playbackHasEnded;
@synthesize playerInternalStateCode;

- (id)init
{
	return [self initWithFrame:CGRectZero];
}

- (id)initWithFrame:(CGRect)frame
{
	return [self initWithFrame:frame tag:@"video-player"];
}

- (id)initWithFrame:(CGRect)frame tag:(NSString *)tag
{
	self = [super initWithFrame:CGRectZero tag:tag];
	
    if (self)
	{
		// change webview properties
		WKWebView *webView	= [self webView];
		[[webView scrollView] setScrollEnabled:NO];
		[webView setAutoresizesSubviews:YES];
		[webView setAutoresizingMask:(UIViewAutoresizingFlexibleHeight | UIViewAutoresizingFlexibleWidth)];
        
        // update configuration
        WKWebViewConfiguration *webConfig   = [webView configuration];
        [webConfig setAllowsInlineMediaPlayback:YES];
        [webConfig setMediaTypesRequiringUserActionForPlayback:WKAudiovisualMediaTypeNone];
		
		// Set loading spinner properties
		[[self loadingSpinner] setActivityIndicatorViewStyle:UIActivityIndicatorViewStyleWhite];
		[self setShowSpinnerOnLoad:YES];
		
		// Set other properties
		[self setCanBounce:NO];
		[self setAutoShowOnLoadFinish:NO];
		[self setControlType:WebviewControlTypeCloseButton];
		[self setBackgroundColor:[UIColor blackColor]];
		
		// Add schema
		[self addNewURLScheme:(NSString *)kEVPlayerScheme];
		
		// Reset player to default state
		[self reset];
	}
	
    return self;
}

- (void)dealloc
{
	// Release
	self.embeddedHTMLString			= nil;
	self.playerInternalStateCode	= nil;
	
	[super dealloc];
}

#pragma mark - Player Methods

- (void)reset
{
	// Default state value
	self.playbackHasEnded			= false;
	self.playerInternalStateCode	= (NSString *)kEVPlayerStateUnstartedCode;
}

- (void)play
{
	// Dint set html string
	if (![self embeddedHTMLString])
	{
		[self onFinishedPlaying:MovieFinishReasonPlaybackEnded];
		return;
	}
	
	// Reset
	[self reset];
	
	// Start load request
	[self loadHTMLString:[self embeddedHTMLString] baseURL:[[NSBundle mainBundle] bundleURL]];
}

- (void)pause
{
	[self evaluateJavaScript:(NSString *)kEVPlayerCommandPauseVideo];
}

- (void)stop
{
	[self stopLoading];
	[self evaluateJavaScript:(NSString *)kEVPlayerCommandStopVideo];
	
	// Forcibly sending this state code, as youtube can stay on any non playing state when stop is called
	[self setPlayerInternalStateCode:(NSString *)kEVPlayerStateUserExitedCode];
}

- (void)setPlayerInternalStateCode:(NSString *)newStateCode {
	[playerInternalStateCode release], playerInternalStateCode = NULL;
	// Set new value
	playerInternalStateCode		= [newStateCode retain];
	
	// Update playback state
	if ([newStateCode isEqualToString:(NSString *)kEVPlayerStatePlayingCode]) {
		_playbackState	= moviePlayerStatePlaying;
	} else if ([newStateCode isEqualToString:(NSString *)kEVPlayerStatePausedCode]) {
		_playbackState	= moviePlayerStatePaused;
	} else if ([newStateCode isEqualToString:(NSString *)kEVPlayerStateEndedCode]) {
		_playbackState	= moviePlayerStateStopped;
		[self onFinishedPlaying:MovieFinishReasonPlaybackEnded];
	} else if ([newStateCode isEqualToString:(NSString *)kEVPlayerStateUserExitedCode]) {
		_playbackState	= moviePlayerStateStopped;
		[self onFinishedPlaying:MovieFinishReasonUserExited];
	} else {
		_playbackState	= moviePlayerStateStopped;
	}
}

#pragma mark - Button Action Methods

- (void)onPressingCloseButton:(id)sender
{
	NSLog(@"[VideoEmbeddedView] pressed close");
	
	// Stop player
	[self stop];
}

#pragma mark - Callback Methods

- (void)didFindMatchingURLScheme:(NSURL *)requestURL
{
	NSMutableDictionary *parsedDict	= [self parseURLScheme:requestURL];
	NSMutableDictionary *argsDict	= [parsedDict objectForKey:@"arguments"];
	NSString *host					= [parsedDict objectForKey:@"host"];
	NSString *value					= [argsDict objectForKey:@"value"];
	
	if (!value)
    {
		value   = kNSStringDefault;
    }
	
	if ([host isEqualToString:(NSString *)kEVPlayerEventOnReady])
	{
		// Play video
		[self evaluateJavaScript:(NSString *)kEVPlayerCommandPlayVideo];
        [self evaluateJavaScript:@"window.innerWidth;"];
	}
	else if ([host isEqualToString:(NSString *)kEVPlayerEventOnStateChange])
	{
		[self setPlayerInternalStateCode:value];
	}
	else if ([host isEqualToString:(NSString *)kEVPlayerEventOnError])
	{
		[self onError];
	}
}

- (void)didRotateToOrientation:(UIDeviceOrientation)toOrientation fromOrientation:(UIDeviceOrientation)fromOrientation
{
    [super didRotateToOrientation:toOrientation fromOrientation:fromOrientation];
    [self evaluateJavaScript:(NSString *)kEVPlayerCommandResizeView];
}

- (void)onFinishedPlaying:(MovieFinishReason)reason
{
    // Check if callback finished was already sent
    if ([self playbackHasEnded])
        return;
    
    // Mark as playback ended
    [self setPlaybackHasEnded:YES];
    
    // Send callback to observers
    if ([self delegate] && [[self delegate] conformsToProtocol:@protocol(EmbeddedVideoPlayerDelegate)])
    {
        [[self delegate] embeddedVideoPlayer:self didFinishPlaying:reason];
    }
}

#pragma mark - WKNavigationDelegate Methods

- (void)webView:(WKWebView *)webView didFailProvisionalNavigation:(WKNavigation *)navigation withError:(NSError *)error
{
    [super webView:webView didFailProvisionalNavigation:navigation withError:error];
    
    [self onError];
}

- (void)webView:(WKWebView *)webView didFailNavigation:(null_unspecified WKNavigation *)navigation withError:(NSError *)error
{
    [super webView:webView didFailNavigation:navigation withError:error];
    
    [self onError];
}

- (void)onError
{
	// Send player state to unknown and send callback
	[self setPlayerInternalStateCode:(NSString *)kEVPlayerStateUnknownCode];
	[self onFinishedPlaying:MovieFinishReasonPlaybackError];
}

@end
