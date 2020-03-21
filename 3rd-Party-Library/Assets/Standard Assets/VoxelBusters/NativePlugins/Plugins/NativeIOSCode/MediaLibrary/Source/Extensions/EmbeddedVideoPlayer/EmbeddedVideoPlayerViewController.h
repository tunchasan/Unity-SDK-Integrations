//
//  EmbeddedVideoPlayerViewController.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 21/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "EmbeddedVideoPlayer.h"

@class EmbeddedVideoPlayerViewController;

@protocol EmbeddedVideoPlayerViewControllerDelegate <NSObject>

@required
- (void)embeddedVideoPlayerViewController:(EmbeddedVideoPlayerViewController *)controller
                         didFinishPlaying:(MovieFinishReason)reason;
@end

@interface EmbeddedVideoPlayerViewController : UIViewController <EmbeddedVideoPlayerDelegate>

// Properties
@property (nonatomic, retain) 	EmbeddedVideoPlayer 					*videoPlayer;
@property (nonatomic, assign)	id <EmbeddedVideoPlayerViewControllerDelegate> 	delegate;

@end

