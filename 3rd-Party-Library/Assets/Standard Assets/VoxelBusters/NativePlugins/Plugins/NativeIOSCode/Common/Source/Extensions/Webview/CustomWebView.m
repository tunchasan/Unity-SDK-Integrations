//
//  CustomWebView.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 19/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "CustomWebView.h"

@implementation CustomWebView

#define cToolBarHeight  44

// Reference: https://github.com/mopub/mopub-ios-sdk/blob/master/MoPubSDK/Internal/HTML/MPWebView.m
static NSString *const kMoPubScalesPageToFitScript = @"var meta = document.createElement('meta'); meta.setAttribute('name', 'viewport'); meta.setAttribute('content', 'width=device-width, initial-scale=1.0, user-scalable=no'); document.getElementsByTagName('head')[0].appendChild(meta);";

@synthesize isShowing = _isShowing;
@synthesize canDismiss;
@synthesize canBounce;
@synthesize controlType;
@synthesize showSpinnerOnLoad;
@synthesize autoShowOnLoadFinish;
@synthesize scalesPageToFit;
@synthesize allowMediaPlayback;
@synthesize normalisedFrame;

@synthesize closeButton;
@synthesize loadingSpinner;
@synthesize webView;
@synthesize toolbar;
@synthesize webviewTag;
@synthesize URLSchemeList;

+ (id)CreateWithFrame:(CGRect)frame tag:(NSString *)tag
{
	return [[[self alloc] initWithFrame:frame tag:tag] autorelease];
}

- (id)initWithFrame:(CGRect)frame
{
	return [self initWithFrame:frame tag:@"no-tag"];
}

- (id)initWithFrame:(CGRect)frame tag:(NSString *)tag
{
    self = [super initWithFrame:frame];
    
    if (self)
    {
		self.webviewTag = tag;
		
        // create depedent components
		[self createWebView];
		[self createToolbar];
		[self createCloseButton];
        [self createLoadingSpinner];
		
		// set default properties
		_isShowing						= NO;
		self.URLSchemeList  			= [NSMutableArray array];
		self.canDismiss					= YES;
		self.canBounce					= YES;
		self.controlType				= WebviewControlTypeCloseButton;
		self.showSpinnerOnLoad			= YES;
		self.autoShowOnLoadFinish		= YES;
		self.scalesPageToFit			= YES;
		self.allowMediaPlayback			= YES;
		[self setBackgroundColor:[UIColor whiteColor]];
		
		// register as observer
		[[UIDeviceOrientationManager Instance] setObserver:self];
    }
    
    return self;
}

- (void)dealloc
{
	// remove observer
	[[UIDeviceOrientationManager Instance] removeObserver:self];
    
    // reset webview properties
	[self.webView setUIDelegate:nil];
    [self.webView setNavigationDelegate:nil];
    [[self.webView scrollView] setDelegate: nil];
    [self.webView stopLoading];
   
	// release
	self.closeButton	= NULL;
    self.loadingSpinner	= NULL;
	self.webView		= NULL;
    self.toolbar     	= NULL;
    self.URLSchemeList	= NULL;
    self.webviewTag     = NULL;
    
    [super dealloc];
}

- (void)createWebView
{
    // create configuration object
    WKWebViewConfiguration *webConfig   = [[[WKWebViewConfiguration alloc] init] autorelease];
    if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"10.0"))
    {
        [webConfig setMediaTypesRequiringUserActionForPlayback:WKAudiovisualMediaTypeAll];
    }
    else
    {
        [webConfig setMediaPlaybackRequiresUserAction:WKAudiovisualMediaTypeAll];
    }
    
    [webConfig setAllowsInlineMediaPlayback:YES];

    // setup webview
    WKWebView *webView	        = [[[WKWebView alloc] initWithFrame:CGRectZero configuration:webConfig] autorelease];
    webView.UIDelegate          = self;
    webView.navigationDelegate  = self;
    webView.opaque              = false;
    [self addSubview:webView];
    
    // save reference
    [self setWebView:webView];
}

- (void)createToolbar
{
    // setup toolbar
    WebViewToolBar *toolBar = [[[WebViewToolBar alloc] init] autorelease];
    [toolBar setToolbarDelegate:self];
    [self addSubview:toolBar];

	// save reference
    [self setToolbar:toolBar];
}

- (void)createCloseButton
{
	UIImage *closeBtnImage	= [UIImage imageNamed:@"close_button.png"];
    
    // create button
	UIButton *closeButton	= [UIButton buttonWithType:UIButtonTypeCustom];
    [closeButton setFrame:CGRectMake(0, 0, closeBtnImage.size.width, closeBtnImage.size.height)];
    [closeButton setImage:closeBtnImage forState:UIControlStateNormal];
    [closeButton addTarget:self action:@selector(onPressingCloseButton:) forControlEvents:UIControlEventTouchUpInside];
    [self addSubview:closeButton];
    
    // store reference
    [self setCloseButton:closeButton];
}

- (void)createLoadingSpinner
{
    // setup spinner view
	UIActivityIndicatorView *indicator	= [[[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleGray] autorelease];
	[indicator setHidesWhenStopped:YES];
    [self addSubview:indicator];

	// cache reference
    [self setLoadingSpinner:indicator];
}

#pragma mark - Properties

- (void)setCanDismiss:(BOOL)dismiss
{
	canDismiss		= dismiss;
	
	// update button state
	[[self closeButton] setEnabled:dismiss];
	[[self toolbar] setCanStop:dismiss];
}

- (void)setCanBounce:(BOOL)bounces
{
	canBounce		= bounces;
	
	// update webview property
	[[[self webView] scrollView] setBounces:bounces];
}

- (void)setControlType:(WebviewControlType)newType
{
    controlType      = newType;
	
	// by default toolbar and close button are hidden
	[[self toolbar] setHidden:YES];
	[[self closeButton] setHidden:YES];
	
    if (newType == WebviewControlTypeToolbar)
	{
		[[self toolbar] setHidden:NO];
	}
	else if (newType == WebviewControlTypeCloseButton)
	{
		[[self closeButton] setHidden:NO];
	}
	
	[self updateWebViewFrame];
}

- (void)setShowSpinnerOnLoad:(BOOL)show
{
    showSpinnerOnLoad = show;
    
    // show activity indicator if webview is loading
    if ([[self webView] isLoading] && showSpinnerOnLoad)
	{
		[self showLoadingSpinner];
	}
	else
	{
		[self hideLoadingSpinner];
	}
}

- (BOOL)scalesPageToFit
{
    return ([[[self webView] scrollView] delegate] != NULL);
}

- (void)setScalesPageToFit:(BOOL)scales
{
    // Reference: https://github.com/mopub/mopub-ios-sdk/blob/master/MoPubSDK/Internal/HTML/MPWebView.m
    WKWebView *webView  = [self webView];
    if (scalesPageToFit)
    {
        [[webView scrollView] setDelegate: nil];
        [[[webView configuration] userContentController] removeAllUserScripts];
    }
    else
    {
        // Make sure the scroll view can't scroll (prevent double tap to zoom)
        [[webView scrollView] setDelegate: self];
        
        // Inject the user script to scale the page if needed
        if (webView.configuration.userContentController.userScripts.count == 0)
        {
            WKUserScript *viewportScript = [[WKUserScript alloc] initWithSource:kMoPubScalesPageToFitScript
                                                                  injectionTime:WKUserScriptInjectionTimeAtDocumentEnd
                                                               forMainFrameOnly:YES];
            [[[webView configuration] userContentController] addUserScript:viewportScript];
        }
    }
}

- (void)setBackgroundColor:(UIColor *)backgroundColor
{
	[super setBackgroundColor:[UIColor clearColor]];
	
	// use same value for webview
	[[self webView] setBackgroundColor:backgroundColor];
}

- (void)setAllowMediaPlayback:(BOOL)allow
{
	allowMediaPlayback	= allow;
	
	// update webview property
	[[[self webView] configuration] setAllowsInlineMediaPlayback:allow];
}

#pragma mark - View

- (void)setFrame:(CGRect)frame
{
	// recalculate new normalised rect and update the changes in view
	[self setNormalisedFrame:ConvertToNormalisedRect(frame)];
}

- (void)setNormalisedFrame:(CGRect)newFrame
{
	normalisedFrame	= newFrame;

    [self updateWebViewFrame];
}

- (void)updateWebViewFrame
{
	// first, set this views frame
	[super setFrame:ConvertToApplicationSpace([self normalisedFrame])];
	
	// update internal subview size based on style
	CGRect viewFrame		= [self frame];
	CGSize viewSize			= viewFrame.size;
	CGRect webviewFrame;

	if ([self controlType] == WebviewControlTypeToolbar)
	{
		webviewFrame.origin	= CGPointMake(0, 0);
		webviewFrame.size   = CGSizeMake(viewSize.width, viewSize.height - cToolBarHeight);
	}
	else if ([self controlType] == WebviewControlTypeCloseButton)
	{
		CGSize closeButtonSize;
		
		if (self.closeButton != NULL)
		{
			CALayer* closeBtnLayer	= [[self closeButton] layer];
			
			// cache button size
			closeButtonSize	= [[self closeButton] frame].size;
			
			// update anchor and position
			[closeBtnLayer setAnchorPoint:CGPointMake(1, 0)];
			[closeBtnLayer setPosition:CGPointMake(CGRectGetMaxX(viewFrame), 0)];
		}
		
		// set webview origin and size
		webviewFrame.origin	= CGPointMake(0, 0);
		webviewFrame.size	= viewSize;
	}
	else
	{
		// set webview origin and size
		webviewFrame.origin	= CGPointMake(0, 0);
		webviewFrame.size	= viewSize;
	}
	
	// assign new frame
	[[self webView] setFrame:webviewFrame];
	
	// update toolbar position
	if (self.toolbar)
	{
		[[self toolbar] setFrame:CGRectMake(0, CGRectGetMaxY(webviewFrame), viewSize.width,	cToolBarHeight)];
	}
	
	// set anchor to (0.5, 0.5) and update spinner position
	if (self.loadingSpinner)
	{
		[[[self loadingSpinner] layer] setAnchorPoint:CGPointMake(0.5, 0.5)];
		[[[self loadingSpinner] layer] setPosition:CGPointMake(CGRectGetMidX(webviewFrame), CGRectGetMidY(webviewFrame))];
	}
}

- (void)show
{
    NSLog(@"[CustomWebView] show %@", [self webviewTag]);
	
	// update property value
	_isShowing	= YES;
}

- (void)dismiss
{
	NSLog(@"[CustomWebView] dismiss %@", [self webviewTag]);
	
    // update property value
	_isShowing	= NO;
	
	// stop request
	[self stopLoading];
	
	// remove view from parent
	[self removeFromSuperview];
}

- (void)layoutSubviews
{
	[self updateWebViewFrame];
}

#pragma mark - Load

- (void)loadRequest:(NSURLRequest *)request
{
	[[self webView] loadRequest:request];
}

- (void)loadHTMLString:(NSString *)string baseURL:(NSURL *)baseURL
{
	[[self webView] loadHTMLString:string
						   baseURL:baseURL];
}

- (void)loadData:(NSData *)data MIMEType:(NSString *)MIMEType textEncodingName:(NSString *)textEncodingName baseURL:(NSURL *)baseURL
{
    WKWebView *webView  = [self webView];
    [webView loadData:data MIMEType:MIMEType characterEncodingName:textEncodingName baseURL:baseURL];
}

- (void)evaluateJavaScript:(NSString *)script
{
    WKWebView *webView  = [self webView];
    [webView evaluateJavaScript:script completionHandler:^(id _Nullable result, NSError * _Nullable error) {
        // Invoke handler
        [self didFinishEvaluatingJavaScriptWithResult:result andError:error];
    }];
}

- (void)reload
{
	[[self webView] reload];
}

- (void)stopLoading
{
	// stops loading webview
	[[self webView] stopLoading];
	
	// hide activity spinner views
	[self hideLoadingSpinner];
	[[UIApplication sharedApplication] setNetworkActivityIndicatorVisible:NO];
}

#pragma mark - URL Scheme

- (void)addNewURLScheme:(NSString *)scheme
{
    [self.URLSchemeList addObject:scheme];
}

- (BOOL)shouldStartLoadRequestWithURLScheme:(NSString *)URLScheme
{
	return NO;
}

#pragma mark - Callbacks

- (void)didFindMatchingURLScheme:(NSURL *)requestURL
{
    NSLog(@"[CustomWebView] found matching URL scheme: %@", [requestURL scheme]);
}

- (void)didFinishEvaluatingJavaScriptWithResult:(id)result andError:(NSError *)error
{
    NSLog(@"[CustomWebView] did finish evaluating script, tag %@, result %@", [self webviewTag], result);
}

#pragma mark - Process URL's

#define kHost		@"host"
#define kArguments	@"arguments"
#define kURLScheme	@"url-scheme"
#define kURL		@"url"

- (NSMutableDictionary *)parseURLScheme:(NSURL *)requestURL
{
	NSString *scheme  	= [requestURL scheme];
	NSString *query		= [requestURL query];
	NSString *host		= [requestURL host];
	
	// Get arguments
	NSMutableDictionary *argsDict	= [NSMutableDictionary dictionary];
	NSArray *queryParts				= [query componentsSeparatedByString:@"&"] ;
	
	if ([queryParts count] > 0)
	{
		for (NSString *keyValuePair in queryParts)
		{
			NSArray *kvParts	= [keyValuePair componentsSeparatedByString:@"="];
			
			if ([kvParts count] == 2)
			{
				NSString *key		= [kvParts objectAtIndex:0];
				NSString *value		= [kvParts objectAtIndex:1];
				argsDict[key]		= value;
			}
		}
	}
	
	// Allot scheme data dictionary
	NSMutableDictionary *messageDict	= [NSMutableDictionary dictionary];
	messageDict[kURL]					= [requestURL absoluteString];
	messageDict[kURLScheme]				= scheme;
	messageDict[kHost]					= IsNullOrEmpty(host) ? kNSStringDefault : host;
	messageDict[kArguments] 			= argsDict;
	
	return messageDict;
}

#pragma mark - Toolbar Delegate

- (void)onPressingBack
{
    NSLog(@"[CustomWebView] user pressed go back");
   
	// Go back
	[[self webView] goBack];
}

- (void)onPressingStop
{
    NSLog(@"[CustomWebView] user pressed done");
	
	// Remove from view
	[self dismiss];
}

- (void)onPressingReload
{
    NSLog(@"[CustomWebView] user pressed reload");
    
	// Reload
	[[self webView] reload];
}

- (void)onPressingForward
{
    NSLog(@"[CustomWebView] user pressed go forward");
   
	// Go forward
	[[self webView] goForward];
}

#pragma mark - Button Callback

- (void)onPressingCloseButton:(id)sender
{
	[self onPressingStop];
}

#pragma mark - Misc.

- (void)showLoadingSpinner
{
	[self.loadingSpinner setHidden:NO];
	[self.loadingSpinner startAnimating];
}

- (void)hideLoadingSpinner
{
	[self.loadingSpinner setHidden:YES];
	[self.loadingSpinner stopAnimating];
}

- (void)updateViewBasedOnLoadState
{
    WKWebView *webView  = [self webView];
    
    // Update indictator state
    if ([webView isLoading])
    {
        [UIApplication sharedApplication].networkActivityIndicatorVisible   = YES;
        [self hideLoadingSpinner];

		// Show spinner if required
		if (self.showSpinnerOnLoad)
		{
			[self showLoadingSpinner];
		}
    }
    else
    {
        [UIApplication sharedApplication].networkActivityIndicatorVisible   = NO;
		[self hideLoadingSpinner];
	}

    // Update toolbar state
    WebViewToolBar *toolBar = [self toolbar];
    [toolBar setCanGoBack: webView.canGoBack];
    [toolbar setCanGoForward: webView.canGoForward];
}

#pragma mark - WKNavigationDelegate Methods

- (void)webView:(WKWebView *)webView didStartProvisionalNavigation:(null_unspecified WKNavigation *)navigation
{
    NSLog(@"[CustomWebView] did start loading, tag %@", [self webviewTag]);
    
    [self updateViewBasedOnLoadState];
}

- (void)webView:(WKWebView *)webView didFinishNavigation:(null_unspecified WKNavigation *)navigation
{
    NSLog(@"[CustomWebView] did finish loading, tag %@", [self webviewTag]);
    
    [self updateViewBasedOnLoadState];
    
    // Show webview, if auto show is enabled
    if ([self autoShowOnLoadFinish])
    {
        [self show];
    }
}

- (void)webView:(WKWebView *)webView didFailProvisionalNavigation:(WKNavigation *)navigation withError:(NSError *)error
{
    NSLog(@"[CustomWebView] did fail loading, tag %@", [self webviewTag]);
    
    [self updateViewBasedOnLoadState];
}

- (void)webView:(WKWebView *)webView didFailNavigation:(null_unspecified WKNavigation *)navigation withError:(NSError *)error
{
    NSLog(@"[CustomWebView] did fail loading, tag %@", [self webviewTag]);
    
    [self updateViewBasedOnLoadState];
}

- (void)webView:(WKWebView *)webView decidePolicyForNavigationAction:(WKNavigationAction *)navigationAction decisionHandler:(void (^)(WKNavigationActionPolicy))decisionHandler
{
    NSURL       *requestURL         = [[navigationAction request] URL];
    NSString    *currentURLScheme   = [requestURL scheme];
    
    if ([self.URLSchemeList indexOfObject:currentURLScheme] != NSNotFound)
    {
        [self didFindMatchingURLScheme:requestURL];
        
        // Check if we need to load this URL
        if (![self shouldStartLoadRequestWithURLScheme:currentURLScheme])
        {
            decisionHandler(WKNavigationActionPolicyCancel);
            return;
        }
    }
    
    decisionHandler(WKNavigationResponsePolicyAllow);
}

#pragma mark - UIScrollViewDelegate Methods

// Reference: https://github.com/mopub/mopub-ios-sdk/blob/master/MoPubSDK/Internal/HTML/MPWebView.m
// Avoid double tap to zoom in WKWebView
// Delegate is only set when scalesPagesToFit is set to NO
- (UIView *)viewForZoomingInScrollView:(UIScrollView *)scrollView
{
    return nil;
}

#pragma mark - Orientation Observer

- (void)didRotateToOrientation:(UIDeviceOrientation)toOrientation fromOrientation:(UIDeviceOrientation)fromOrientation
{
	[self setNeedsLayout];
}

@end
