//
//  WebViewBinding.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 11/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "WebViewBinding.h"
#import	"WebViewHandler.h"
#import "CustomWebView.h"

#pragma mark - Handling webviews

void webviewCreate (const char* tag, WebViewFrame normalisedFrame)
{
    UnityWebView	*webview	= [[WebViewHandler Instance] createWebViewWithTag:ConvertToNSString(tag)];
	
	// Update frame
	[webview setNormalisedFrame:getNormalisedRect(normalisedFrame)];
}

void webviewDestroy (const char* tag)
{
    [[WebViewHandler Instance] destroyWebViewWithTag:ConvertToNSString(tag)];
}

void webviewShow (const char* tag)
{
    [[WebViewHandler Instance] showWebViewWithTag:ConvertToNSString(tag)];
}

void webviewHide (const char* tag)
{
    [[WebViewHandler Instance] dismissWebViewWithTag:ConvertToNSString(tag)];
}

#pragma mark - Loading

void webviewLoadRequest (const char* URL, const char* tag)
{
    [[WebViewHandler Instance] loadRequest:ConvertToNSString(URL)
                       usingWebViewWithTag:ConvertToNSString(tag)];
}

void webviewLoadHTMLString (const char* HTMLString, const char* baseURL, const char* tag)
{
    [[WebViewHandler Instance] loadHTMLString:ConvertToNSString(HTMLString)
                                      baseURL:ConvertToNSString(baseURL)
                          usingWebViewWithTag:ConvertToNSString(tag)];
}

void webviewLoadData (UInt8* dataArray, 	int dataArrayLength,
					  const char* MIMEType, const char* textEncodingName,
					  const char* baseURL, 	const char* tag)
{
    [[WebViewHandler Instance] loadData:[Utility CreateNSDataFromByteArray:(void*)dataArray ofLength:dataArrayLength]
							   MIMEType:ConvertToNSString(MIMEType)
					   textEncodingName:ConvertToNSString(textEncodingName)
								baseURL:ConvertToNSString(baseURL)
					usingWebViewWithTag:ConvertToNSString(tag)];
}

void webviewEvaluateJavaScriptFromString (const char* javaScript, const char* tag)
{
    [[WebViewHandler Instance] evaluateJavaScript:ConvertToNSString(javaScript)
                              usingWebViewWithTag:ConvertToNSString(tag)];
}

void webviewReload (const char* tag)
{
    [[WebViewHandler Instance] reloadWebViewWithTag:ConvertToNSString(tag)];
}

void webviewStopLoading (const char* tag)
{
    [[WebViewHandler Instance] stopLoadingWebViewWithTag:ConvertToNSString(tag)];
}

#pragma mark - Properties

void webviewSetCanHide (bool canHide, const char* tag)
{
    [[WebViewHandler Instance] setCanDismiss:canHide
						   forWebViewWithTag:ConvertToNSString(tag)];
}

void webviewSetCanBounce (bool canBounce, const char* tag)
{
    [[WebViewHandler Instance] setCanBounce:canBounce
						  forWebViewWithTag:ConvertToNSString(tag)];
}

void webviewSetControlType (int type, const char* tag)
{
	[[WebViewHandler Instance] setControlType:(WebviewControlType)type
							forWebViewWithTag:ConvertToNSString(tag)];
	
}

void webviewSetShowSpinnerOnLoad (bool showSpinner, const char* tag)
{
    [[WebViewHandler Instance] setShowSpinnerOnLoad:showSpinner
								  forWebViewWithTag:ConvertToNSString(tag)];
}
	 
void webviewSetAutoShowOnLoadFinish (bool autoShow, const char* tag)
{
    [[WebViewHandler Instance] setAutoShowOnLoadFinish:autoShow
									 forWebViewWithTag:ConvertToNSString(tag)];
}

void webviewSetScalesPageToFit (bool scaleToFit, const char* tag)
{
    [[WebViewHandler Instance] setScalesPageToFit:scaleToFit
                                forWebViewWithTag:ConvertToNSString(tag)];
}

void webviewSetNormalisedFrame (WebViewFrame normalisedFrame, const char* tag)
{
    [[WebViewHandler Instance] setNormalisedFrame:getNormalisedRect(normalisedFrame)
								forWebViewWithTag:ConvertToNSString(tag)];
	
}

void webviewSetBackgroundColor (float r, float g, float b, float alpha, const char* tag)
{
	[[WebViewHandler Instance] setBackgroundColor:[UIColor colorWithRed:r green:g blue:b alpha:alpha]
								forWebViewWithTag:ConvertToNSString(tag)];
}

#pragma mark - URL Scheme Methods

void webviewAddNewURLScheme (const char* _newURLScheme, const char* tag)
{
    [[WebViewHandler Instance] addNewURLScheme:ConvertToNSString(_newURLScheme)
                             forWebViewWithTag:ConvertToNSString(tag)];
}

#pragma mark - Cache Methods

void webviewClearCache ()
{
    [[WebViewHandler Instance] clearCache];
}

#pragma mark - Misc Methods

CGRect getNormalisedRect (WebViewFrame normalisedFrame)
{
	CGRect normalisedRect;
	normalisedRect.origin.x		= normalisedFrame.x;
	normalisedRect.origin.y		= normalisedFrame.y;
	normalisedRect.size.width	= normalisedFrame.width;
	normalisedRect.size.height	= normalisedFrame.height;
	
	return normalisedRect;
}
