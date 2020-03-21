//
//  WebViewToolBar.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 19/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <UIKit/UIKit.h>

@protocol WebViewToolBarDelegate <NSObject>

- (void)onPressingBack;
- (void)onPressingStop;
- (void)onPressingReload;
- (void)onPressingForward;

@end

@interface WebViewToolBar : UIToolbar

// Properties
@property(nonatomic, retain)    UIBarButtonItem             *backButton;
@property(nonatomic, retain)    UIBarButtonItem             *stopButton;
@property(nonatomic, retain)    UIBarButtonItem             *reloadButton;
@property(nonatomic, retain)    UIBarButtonItem             *forwardButton;
@property(nonatomic, assign)    id <WebViewToolBarDelegate> toolbarDelegate;

// Related to button states
- (void)setCanGoBack:(BOOL)canGoBack;
- (void)setCanStop:(BOOL)canStop;
- (void)setCanRefresh:(BOOL)canRefresh;
- (void)setCanGoForward:(BOOL)canGoForward;

@end
