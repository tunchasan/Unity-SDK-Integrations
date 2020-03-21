//
//  SLComposeViewController+Extensions.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 02/12/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "SLComposeViewController+Extensions.h"

@implementation SLComposeViewController (Extensions)

- (BOOL)shouldAutorotate
{
	return [UnityGetGLViewController() shouldAutorotate];
}

- (UIInterfaceOrientationMask)supportedInterfaceOrientations
{
	return [UnityGetGLViewController() supportedInterfaceOrientations];
}

@end
