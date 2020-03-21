//
//  WebViewHandler.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 19/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "HandlerBase.h"
#import "CustomWebView.h"

@class UnityWebView;
@interface WebViewHandler : HandlerBase <UIWebViewDelegate>

// Handling webviews
- (UnityWebView *)createWebViewWithTag:(NSString*)tag;
- (void)destroyWebViewWithTag:(NSString *)tag;
- (void)showWebViewWithTag:(NSString *)tag;
- (void)dismissWebViewWithTag:(NSString *)tag;

// Loading, relaoding and stopping
- (void)loadRequest:(NSString *)URLStr usingWebViewWithTag:(NSString *)tag;
- (void)loadHTMLString:(NSString *)string baseURL:(NSString *)baseURLStr usingWebViewWithTag:(NSString *)tag;
- (void)loadData:(NSData *)data MIMEType:(NSString *)MIMEType
textEncodingName:(NSString *)textEncodingName
         baseURL:(NSString *)baseURLStr usingWebViewWithTag:(NSString *)tag;
- (void)evaluateJavaScript:(NSString *)js usingWebViewWithTag:(NSString *)tag;
- (void)reloadWebViewWithTag:(NSString *)tag;
- (void)stopLoadingWebViewWithTag:(NSString *)tag;

// Modifying properties
- (void)setCanDismiss:(BOOL)canDismiss forWebViewWithTag:(NSString *)tag;
- (void)setCanBounce:(BOOL)canBounce forWebViewWithTag:(NSString *)tag;
- (void)setControlType:(WebviewControlType)type forWebViewWithTag:(NSString *)tag;
- (void)setShowSpinnerOnLoad:(BOOL)show forWebViewWithTag:(NSString *)tag;
- (void)setAutoShowOnLoadFinish:(BOOL)autoShow forWebViewWithTag:(NSString *)tag;
- (void)setScalesPageToFit:(BOOL)scaleToFit forWebViewWithTag:(NSString *)tag;
- (void)setNormalisedFrame:(CGRect)normalisedFrame forWebViewWithTag:(NSString *)tag;
- (void)setBackgroundColor:(UIColor *)color forWebViewWithTag:(NSString *)tag;

// URL scheme
- (void)addNewURLScheme:(NSString *)newScheme forWebViewWithTag:(NSString *)tag;

// Related to schema, cache
- (void)clearCache;
- (void)clearCache:(NSString *)URL;

@end
