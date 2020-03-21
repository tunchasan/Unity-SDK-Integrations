//
//  MediaLibraryHandler.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 10/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <MobileCoreServices/UTCoreTypes.h>
#import <MediaPlayer/MediaPlayer.h>
#import <AVFoundation/AVFoundation.h>
#import "HandlerBase.h"
#import "EmbeddedVideoPlayerViewController.h"
#import "HybridActionSheet.h"
#import <AVKit/AVKit.h>
enum eImageSource
{
    ALBUM	= 0,
    CAMERA,
    BOTH
};
typedef enum eImageSource eImageSource;

enum ePickImageFinishReason
{
	ePickImageFinishReasonSelected,
	ePickImageFinishReasonCancelled,
	ePickImageFinishReasonFailed
};
typedef enum ePickImageFinishReason ePickImageFinishReason;

enum ePickVideoFinishReason
{
	ePickVideoFinishReasonSelected,
	ePickVideoFinishReasonCancelled,
	ePickVideoFinishReasonFailed
};
typedef enum ePickVideoFinishReason ePickVideoFinishReason;

@interface MediaLibraryHandler : HandlerBase <UIAlertViewDelegate, UINavigationControllerDelegate, UIImagePickerControllerDelegate, HybridActionSheetDelegate, EmbeddedVideoPlayerViewControllerDelegate, UIPopoverControllerDelegate>

// Properties
@property(nonatomic)            float                  			scaleFactor;
@property(nonatomic)            BOOL							allowsFullScreenVideoPlayback;
@property(nonatomic)            BOOL							allowsImageEditing;
@property(nonatomic, assign)    AVPlayerViewController		    *moviePlayerVC;
@property(nonatomic, assign)    EmbeddedVideoPlayerViewController	*embeddedPlayerVC;

// Related to image
- (BOOL)isCameraSupported;
- (void)pickImage:(eImageSource)source scaleDownTo:(float)factor;
- (void)saveImageToGallery:(UIImage *)image;

// Related to video
- (void)playVideoUsingWebView:(NSString *)embedHTML;
- (void)playVideoFromURL:(NSString *)URLString fullscreen:(BOOL)fullscreen;
- (void)playVideoFromGallery:(BOOL)fullscreen;
- (void)stopPlayingVideo;

@end
