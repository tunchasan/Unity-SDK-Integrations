//
//  WebViewHandler.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 19/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "WebViewHandler.h"
#import "UnityWebView.h"
#import "NSURL+Extensions.h"

@interface WebViewHandler ()

// Properties
@property(nonatomic, retain)    NSMutableDictionary     *webViewContainer;

@end

@implementation WebViewHandler

@synthesize webViewContainer;

- (id)init
{
    self    = [super init];
    
    // Initailisation
    if (self)
    {
        self.webViewContainer   = [NSMutableDictionary dictionary];
    }
    
    return self;
}

- (void)dealloc
{
    // Remove all webviews attached to view
    for (NSString *key in self.webViewContainer.keyEnumerator)
    {
		UnityWebView *webView	= [self getWebViewWithTag:key];
		
		if (webView)
     	   [webView dismiss];
    }

    // Release web container
    [self.webViewContainer removeAllObjects];
    self.webViewContainer       = NULL;
    
    [super dealloc];
}

#pragma mark - Handling Webviews

- (UnityWebView *)createWebViewWithTag:(NSString*)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
   
    // Create new webview and save it
    if (webView == NULL)
    {
        UnityWebView *webView    = [UnityWebView CreateWithFrame:CGRectZero tag:tag];
		NSLog(@"[WebViewHandler] creating webview with tag: %@", tag);
		
		// Add it to webview collection
        [self.webViewContainer setObject:webView forKey:tag];
    }
    
    return webView;
}

- (UnityWebView *)getWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self.webViewContainer objectForKey:tag];
	NSLog(@"[WebViewHandler] found webview: %@ for tag: %@", webView, tag);
	
    return webView;
}

- (void)destroyWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
    
    if (webView != NULL)
    {
		NSLog(@"[WebViewHandler] destroying webview with tag: %@", tag);
        
        // Remove webview from view
        [webView dismiss];
		
        // Remove webview from container
        [self.webViewContainer removeObjectForKey:tag];
    }
}

- (void)showWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
    
    if (webView != NULL)
    {
		NSLog(@"[WebViewHandler] showing webview with tag: %@", tag);

		// Show webview
		[webView show];
    }
}

- (void)dismissWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
    
    // Couldnt find webview with given tag
    if (webView != NULL)
    {
		NSLog(@"[WebViewHandler] dismiss webview with tag: %@", tag);
		
		// Dismiss webview
		[webView dismiss];
    }
}

#pragma mark - Loading

- (void)loadRequest:(NSString *)URLStr usingWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
    
    // Couldnt find webview with given tag
    if (webView == NULL)
        return;
    
    // URL is null
    if (IsNullOrEmpty(URLStr))
    {
        NSLog(@"[WebViewHandler] url cant be null or empty");
        return;
    }

	NSLog(@"[WebViewHandler] loading request with url: %@", URLStr);
	
	// Get URL
    NSURL* url              = [NSURL createURLWithString:URLStr];
    NSURLRequest* request   = [[[NSURLRequest alloc] initWithURL:url] autorelease];
   
    // Start loading
	[webView loadRequest:request];
}

- (void)loadHTMLString:(NSString *)string baseURL:(NSString *)baseURLStr usingWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
    
    // Couldnt find webview with given tag
    if (webView == NULL)
        return;
	
	NSLog(@"[WebViewHandler] loading request with string: %@ baseURL: %@", string, baseURLStr);
	
	// Get base URL
	NSURL* baseURL      = NULL;

    if (baseURLStr != NULL)
        baseURL = [NSURL createURLWithString:baseURLStr];
	
    // Load HTML string
    [webView loadHTMLString:string baseURL:baseURL];
}

- (void)loadData:(NSData *)data MIMEType:(NSString *)MIMEType
textEncodingName:(NSString *)textEncodingName
         baseURL:(NSString *)baseURLStr usingWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
    
    // Couldnt find webview with given tag
    if (webView == NULL)
        return;
    
	NSLog(@"[WebViewHandler] loading data of type: %@", MIMEType);
	
	// Get base URL
    NSURL* baseURL  = NULL;
    
    if (baseURLStr != NULL)
        baseURL = [NSURL createURLWithString:baseURLStr];
    
    // Encoding
    NSStringEncoding stringEncoding = NSUTF8StringEncoding;
    
    if (textEncodingName)
    {
        CFStringEncoding encoding = CFStringConvertIANACharSetNameToEncoding((CFStringRef)textEncodingName);
        
        if (encoding != kCFStringEncodingInvalidId)
        {
            stringEncoding = CFStringConvertEncodingToNSStringEncoding(encoding);
        }
    }
    
    // Load data
    [webView loadData:data
			 MIMEType:MIMEType
	 textEncodingName:[NSString localizedNameOfStringEncoding:stringEncoding]
			  baseURL:baseURL];
}

- (void)evaluateJavaScript:(NSString *)js usingWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
    
    // Couldnt find webview with given tag
    if (webView == NULL)
        return;
	
	NSLog(@"[WebViewHandler] evaluating js from string: %@", js);
    
	// Evaluate js
   [webView evaluateJavaScript:js];
}

- (void)reloadWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
    
	// Couldnt find webview with given tag
    if (webView == NULL)
        return;
	
    NSLog(@"[WebViewHandler] reloading webview");
	
    // Reload
    [webView reload];
}

- (void)stopLoadingWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
    
    // Couldnt find webview with given tag
    if (webView == NULL)
        return;
	
	 NSLog(@"[WebViewHandler] stopping webview load");
    
    // Stop loading
    [webView stopLoading];
}

#pragma mark - properties

- (void)setCanDismiss:(BOOL)canDismiss forWebViewWithTag:(NSString *)tag
{
	UnityWebView *webView    = [self getWebViewWithTag:tag];
    
    // Couldnt find webview with given tag
    if (webView == NULL)
        return;
	
	NSLog(@"[WebViewHandler] setCanDismiss: %d", canDismiss);
    
    // Set can dismiss
    [webView setCanDismiss:canDismiss];
}

- (void)setCanBounce:(BOOL)canBounce forWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
    
    // Couldnt find webview with given tag
    if (webView == NULL)
        return;
	
	NSLog(@"[WebViewHandler] setBounces: %d", canBounce);
    
    // Set bounce property
    [webView setCanBounce:canBounce];
}

- (void)setControlType:(WebviewControlType)type forWebViewWithTag:(NSString *)tag;
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
   
    // Couldnt find webview with given tag
    if (webView == NULL)
        return;
	
	NSLog(@"[WebViewHandler] setControlType: %d", type);

    // Set control type
	[webView setControlType:type];
}

- (void)setShowSpinnerOnLoad:(BOOL)show forWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
    
    // Couldnt find webview with given tag
    if (webView == NULL)
        return;
	
	NSLog(@"[WebViewHandler] setShowSpinnerOnLoad: %d", show);
    
    // Set show loading indicator
    [webView setShowSpinnerOnLoad:show];
}

- (void)setAutoShowOnLoadFinish:(BOOL)autoShow forWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
    
    // Couldnt find webview with given tag
    if (webView == NULL)
        return;
	
	NSLog(@"[WebViewHandler] setAutoShowOnLoadFinish: %d", autoShow);
    
    // Set auto show when load complete
    [webView setAutoShowOnLoadFinish:autoShow];
}

- (void)setScalesPageToFit:(BOOL)scaleToFit forWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
    
    // Couldnt find webview with given tag
    if (webView == NULL)
        return;
	
    NSLog(@"[WebViewHandler] setScalesPageToFit: %d", scaleToFit);
	
    // Set value to scale page to fit
    [webView setScalesPageToFit:scaleToFit];
}

- (void)setNormalisedFrame:(CGRect)normalisedFrame forWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
    
    // Couldnt find webview with given tag
    if (webView == NULL)
        return;
	
	// Setting normalised frame
    [webView setNormalisedFrame:normalisedFrame];
	NSLog(@"[WebViewHandler] Setting new frame: %@", NSStringFromCGRect(webView.frame));
}

- (void)setBackgroundColor:(UIColor *)color forWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
    
    // Couldnt find webview with given tag
    if (webView == NULL)
        return;
	
    NSLog(@"[WebViewHandler] setBackgroundColor: %@", color);
	
    // Set bg color
    [webView setBackgroundColor:color];
}

#pragma mark - URL scheme

- (void)addNewURLScheme:(NSString *)newScheme forWebViewWithTag:(NSString *)tag
{
    UnityWebView *webView    = [self getWebViewWithTag:tag];
	
    // Couldnt find webview with given tag
    if (webView == NULL)
        return;
	
	NSLog(@"[WebViewHandler] addNewURLScheme: %@", newScheme);
    
    // Add new scheme
    [webView addNewURLScheme:newScheme];
}

#pragma mark - cache

- (void)clearCache
{
	NSLog(@"[WebViewHandler] clearing cache");
	
    for (NSHTTPCookie *cookie in [[NSHTTPCookieStorage sharedHTTPCookieStorage] cookies])
    {
        [[NSHTTPCookieStorage sharedHTTPCookieStorage] deleteCookie:cookie];
    }
}

- (void)clearCache:(NSString *)URL
{
	NSLog(@"[WebViewHandler] clearing cache for URL: %@", URL);
	
    for (NSHTTPCookie *cookie in [[NSHTTPCookieStorage sharedHTTPCookieStorage] cookies])
    {
        if ([[cookie domain] isEqualToString:URL])
        {
            [[NSHTTPCookieStorage sharedHTTPCookieStorage] deleteCookie:cookie];
        }
    }
}

@end
