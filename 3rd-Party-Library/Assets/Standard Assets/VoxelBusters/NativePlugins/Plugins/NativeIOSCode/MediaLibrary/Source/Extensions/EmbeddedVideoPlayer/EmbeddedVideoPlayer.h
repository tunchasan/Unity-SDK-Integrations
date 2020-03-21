//
//  EmbeddedVideoPlayer.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 21/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <MediaPlayer/MediaPlayer.h>
#import "CustomWebView.h"

typedef enum : NSInteger {
    moviePlayerStateStopped,
    moviePlayerStatePlaying,
    moviePlayerStatePaused,
    moviePlayerStateInterrupted,
    moviePlayerStateSeekingForward,
    moviePlayerStateSeekingBackward
} MoviePlayerState;

typedef enum : NSInteger {
    MovieFinishReasonPlaybackEnded,
    MovieFinishReasonPlaybackError,
    MovieFinishReasonUserExited,
} MovieFinishReason;

@class EmbeddedVideoPlayer;

@protocol EmbeddedVideoPlayerDelegate <NSObject>

@required
- (void)embeddedVideoPlayer:(EmbeddedVideoPlayer *)player stateChangedTo:(MoviePlayerState)state;
- (void)embeddedVideoPlayer:(EmbeddedVideoPlayer *)player didFinishPlaying:(MovieFinishReason)reason;

@end



@interface EmbeddedVideoPlayer : CustomWebView

// Properties
@property(nonatomic, retain)	NSString							*embeddedHTMLString;
@property(nonatomic, assign)	id <EmbeddedVideoPlayerDelegate> 	delegate;
@property(nonatomic, readonly) 	MoviePlayerState 				playbackState;

// Loading Methods
- (void)setEmbeddedHTMLString:(NSString *)HTMLString;

// Player Methods
- (void)play;
- (void)pause;
- (void)stop;

@end
