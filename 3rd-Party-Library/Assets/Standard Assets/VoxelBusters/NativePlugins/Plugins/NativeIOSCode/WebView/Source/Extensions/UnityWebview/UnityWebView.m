//
//  UnityWebView.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 21/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "UnityWebView.h"

#define kShowing  						"WebViewDidShow"
#define kDismissed						"WebViewDidHide"
#define kDestroyed						"WebViewDidDestroy"
#define kDidStartLoad 					"WebViewDidStartLoad"
#define kDidFinishLoad 					"WebViewDidFinishLoad"
#define kDidFailLoadWithError			"WebViewDidFailLoadWithError"
#define kFinishedEvaluatingJavaScript	"WebViewDidFinishEvaluatingJS"
#define kReceivedMessage 				"WebViewDidReceiveMessage"

#define kURLKey							@"url"
#define kTagKey							@"tag"
#define kErrorKey						@"error"

@implementation UnityWebView

- (id)initWithFrame:(CGRect)frame tag:(NSString *)tag
{
	self = [super initWithFrame:frame tag:tag];
	
    if (self)
	{
		// set initial properties
		[[self webView] setOpaque:NO];
    }
	
    return self;
}

- (void)dealloc
{	
	// update unity
	NotifyEventListener(kDestroyed, [[self webviewTag] UTF8String]);
	
	[super dealloc];
}

#pragma mark - Override View Methods

- (void)show
{
	BOOL    currentlyShowing    = [self isShowing];
    if (!currentlyShowing)
	{
		// Add the view on the top of Unity view
		[super show];
		[UnityGetGLViewController().view addSubview:self];
		
		// Notify Unity
		NotifyEventListener(kShowing, [[self webviewTag] UTF8String]);
	}
}

- (void)dismiss
{
	BOOL	currentlyShowing	= [self isShowing];
	if (currentlyShowing)
	{
		// Removes view from super view
		[super dismiss];
		
		// Notify Unity
		NotifyEventListener(kDismissed, [[self webviewTag] UTF8String]);
	}
}

#pragma mark - Callback Methods

-(void)didFindMatchingURLScheme:(NSURL *)requestURL
{
    // Gather required info
    NSMutableDictionary *messageData    = [self parseURLScheme:requestURL];
    
    NSMutableDictionary *data   = [NSMutableDictionary dictionary];
    data[@"tag"]                = [self webviewTag];
    data[@"message-data"]       = messageData;
    
    // Send it to unity
    NotifyEventListener(kReceivedMessage, ToJsonCString(data));
}

-(void)didFinishEvaluatingJavaScriptWithResult:(id)result andError:(NSError *)error
{
    // Collect information
	NSMutableDictionary *data   = [NSMutableDictionary dictionary];
	data[@"tag"]                = [self webviewTag];
	if (result)
    {
		data[@"result"]			= result;
    }
    if (error)
    {
        data[@"error"]          = [error description];
    }

    // Send it to Unity
	NotifyEventListener(kFinishedEvaluatingJavaScript, ToJsonCString(data));
}

#pragma mark - WKNavigationDelegate Methods

- (void)webView:(WKWebView *)webView didStartProvisionalNavigation:(null_unspecified WKNavigation *)navigation
{
    [super webView:webView didStartProvisionalNavigation:navigation];
	
	// Notify unity
	NotifyEventListener(kDidStartLoad, [[self webviewTag] UTF8String]);
}

- (void)webView:(WKWebView *)webView didFinishNavigation:(null_unspecified WKNavigation *)navigation
{
    [super webView:webView didFinishNavigation:navigation];
	
	// Pack information to be shared with Unity
	NSMutableDictionary* data	= [NSMutableDictionary dictionary];
	data[kTagKey]               = [self webviewTag];
    if ([webView URL])
    {
		data[kURLKey]           = [[webView URL] absoluteString];
    }
	
	NotifyEventListener(kDidFinishLoad, ToJsonCString(data));
}

- (void)webView:(WKWebView *)webView didFailProvisionalNavigation:(WKNavigation *)navigation withError:(NSError *)error
{
    [super webView:webView didFailProvisionalNavigation:navigation withError:error];
    
    [self reportWebView:webView didFailWithError:error];
}

- (void)webView:(WKWebView *)webView didFailNavigation:(null_unspecified WKNavigation *)navigation withError:(NSError *)error
{
    [super webView:webView didFailNavigation:navigation withError:error];
    
    [self reportWebView:webView didFailWithError:error];
}

- (void)reportWebView:(WKWebView *)webView didFailWithError:(NSError *)error
{
    // Pack information
    NSMutableDictionary* data   = [NSMutableDictionary dictionary];
    data[kTagKey]               = [self webviewTag];
    if ([webView URL])
    {
        data[kURLKey]           = [[webView URL] absoluteString];
    }
    if (error)
    {
        data[kErrorKey]         = error.description;
    }
    
    NotifyEventListener(kDidFailLoadWithError, ToJsonCString(data));
}

@end
