//
//  MediaLibraryBindings.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 10/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "MediaLibraryBindings.h"
#import "MediaLibraryHandler.h"

#pragma mark - Image Methods

bool isCameraSupported ()
{
    return [[MediaLibraryHandler Instance] isCameraSupported];
}

void setAllowsImageEditing (bool value)
{
	[[MediaLibraryHandler Instance] setAllowsImageEditing:value];
}

void pickImage (int source, float scaleFactor)
{
    [[MediaLibraryHandler Instance] pickImage:(eImageSource)source
								  scaleDownTo:scaleFactor];
}

void saveImageToGallery (UInt8* imgByteArray, int imgByteArrayLength)
{
    [[MediaLibraryHandler Instance] saveImageToGallery:[Utility CreateImageFromByteArray:imgByteArray
																				ofLength:imgByteArrayLength]];
}

#pragma mark - Video Methods

void playVideoUsingWebView (const char* embedHTML)
{
	[[MediaLibraryHandler Instance] playVideoUsingWebView:ConvertToNSString(embedHTML)];
}

void playVideoFromURL (const char* URLString)
{
	[[MediaLibraryHandler Instance] playVideoFromURL:ConvertToNSString(URLString)
										  fullscreen:YES];
}

void playVideoFromGallery ()
{
	[[MediaLibraryHandler Instance] playVideoFromGallery:YES];
}

void stopPlayingVideo ()
{
	[[MediaLibraryHandler Instance] stopPlayingVideo];
}