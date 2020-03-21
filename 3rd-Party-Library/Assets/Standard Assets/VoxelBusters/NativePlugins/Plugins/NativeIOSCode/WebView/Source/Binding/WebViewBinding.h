//
//  WebViewBinding.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 11/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

struct WebViewFrame
{
    float x;
    float y;
    float width;
    float height;
};
typedef struct WebViewFrame WebViewFrame;

// Handling webviews
UIKIT_EXTERN void webviewCreate (const char* tag, WebViewFrame normalisedFrame);
UIKIT_EXTERN void webviewDestroy (const char* tag);
UIKIT_EXTERN void webviewShow (const char* tag);
UIKIT_EXTERN void webviewHide (const char* tag);

// Loading
UIKIT_EXTERN void webviewLoadRequest (const char* URL, const char* tag);
UIKIT_EXTERN void webviewLoadHTMLString (const char* HTMLString, const char* baseURL,
										 const char* tag);
UIKIT_EXTERN void webviewLoadData (UInt8* dataArray, 		int dataArrayLength,
								   const char* MIMEType, 	const char* textEncodingName,
								   const char* baseURL, 	const char* tag);
UIKIT_EXTERN void webviewEvaluateJavaScriptFromString (const char* javaScript, const char* tag);
UIKIT_EXTERN void webviewReload (const char* tag);
UIKIT_EXTERN void webviewStopLoading (const char* tag);

// Properties
UIKIT_EXTERN void webviewSetCanHide (bool canHide, const char* tag);
UIKIT_EXTERN void webviewSetCanBounce (bool canBounce, const char* tag);
UIKIT_EXTERN void webviewSetControlType (int type, const char* tag);
UIKIT_EXTERN void webviewSetShowSpinnerOnLoad (bool showSpinner, const char* tag);
UIKIT_EXTERN void webviewSetAutoShowOnLoadFinish (bool autoShow, const char* tag);
UIKIT_EXTERN void webviewSetScalesPageToFit (bool scaleToFit, const char* tag);
UIKIT_EXTERN void webviewSetNormalisedFrame (WebViewFrame normalisedFrame, const char* tag);
UIKIT_EXTERN void webviewSetBackgroundColor (float r, 			float g,
											 float b, 			float alpha,
											 const char* tag);

// URL scheme
UIKIT_EXTERN void webviewAddNewURLScheme (const char* _newURLScheme, const char* tag);

// Cache
UIKIT_EXTERN void webviewClearCache ();

// Misc methods
CGRect getNormalisedRect (WebViewFrame normalisedFrame);