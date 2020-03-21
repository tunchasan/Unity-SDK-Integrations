//
//  StoreReceiptServerVerificator.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 17/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "StoreReceiptServerVerificator.h"
#import "RMStore.h"
#import "NSData+Base64.h"

#ifdef DEBUG
#define StoreLog(...) NSLog(@"RMStore: %@", [NSString stringWithFormat:__VA_ARGS__]);
#else
#define StoreLog(...)
#endif

@implementation StoreReceiptServerVerificator

@synthesize customServerURLString;
@synthesize sharedSecretKey;

- (BOOL)useAppleItunesServer
{
	return (customServerURLString == NULL);
}

- (void)verifyReceiptData:(NSData *)receiptData
				  success:(void (^)())successBlock
				  failure:(void (^)(NSError *error))failureBlock
{
	if (receiptData == nil)
	{
		if (failureBlock != nil)
		{
			NSError *error = [NSError errorWithDomain:RMStoreErrorDomain code:0 userInfo:nil];
			failureBlock(error);
		}
		return;
	}
	
	NSString *receipt = [receiptData stringByBase64Encoding];
	if (receipt == nil)
	{
		if (failureBlock != nil)
		{
			NSError *error = [NSError errorWithDomain:RMStoreErrorDomain code:0 userInfo:nil];
			failureBlock(error);
		}
		return;
	}
	static NSString *receiptDataKey = @"receipt-data";
	NSDictionary *jsonReceipt = @{receiptDataKey : receipt};
	
	NSError *error;
	NSData *requestData = [NSJSONSerialization dataWithJSONObject:jsonReceipt options:0 error:&error];
	if (!requestData)
	{
		StoreLog(@"Failed to serialize receipt into JSON");
		if (failureBlock != nil)
		{
			failureBlock(error);
		}
		return;
	}

	static NSString *productionURL = @"https://buy.itunes.apple.com/verifyReceipt";
	
	if ([self useAppleItunesServer])
	{
		[self verifyRequestData:requestData
							url:productionURL
						success:successBlock
						failure:failureBlock];
	}
	else
	{
		[self verifyRequestData:requestData
							url:self.customServerURLString
						success:successBlock
						failure:failureBlock];
	}
}

- (void)verifyRequestData:(NSData*)requestData
					  url:(NSString*)urlString
				  success:(void (^)())successBlock
				  failure:(void (^)(NSError *error))failureBlock
{
	NSURL *url = [NSURL URLWithString:urlString];
	NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
	request.HTTPBody = requestData;
	static NSString *requestMethod = @"POST";
	request.HTTPMethod = requestMethod;
	
	dispatch_async(dispatch_get_global_queue( DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
		NSError *error;
		NSData *data = [NSURLConnection sendSynchronousRequest:request returningResponse:nil error:&error];
		dispatch_async(dispatch_get_main_queue(), ^{
			if (!data)
			{
				StoreLog(@"Server Connection Failed");
				NSError *wrapperError = [NSError errorWithDomain:RMStoreErrorDomain code:RMStoreErrorCodeUnableToCompleteVerification userInfo:@{NSUnderlyingErrorKey : error, NSLocalizedDescriptionKey : NSLocalizedStringFromTable(@"Connection to Apple failed. Check the underlying error for more info.", @"RMStore", @"Error description")}];
				if (failureBlock != nil)
				{
					failureBlock(wrapperError);
				}
				return;
			}
			NSError *jsonError;
			NSDictionary *responseJSON = [NSJSONSerialization JSONObjectWithData:data options:0 error:&jsonError];
			if (!responseJSON)
			{
				StoreLog(@"Failed To Parse Server Response");
				if (failureBlock != nil)
				{
					failureBlock(jsonError);
				}
			}
			
			static NSString *statusKey = @"status";
			NSInteger statusCode = [responseJSON[statusKey] integerValue];
			
			static NSInteger successCode = 0;
			static NSInteger sandboxCode = 21007;
			if (statusCode == successCode)
			{
				if (successBlock != nil)
				{
					successBlock();
				}
			}
			else if ([self useAppleItunesServer] && statusCode == sandboxCode)
			{
				StoreLog(@"Verifying Sandbox Receipt");
				// From: https://developer.apple.com/library/ios/#technotes/tn2259/_index.html
				// See also: http://stackoverflow.com/questions/9677193/ios-storekit-can-i-detect-when-im-in-the-sandbox
				// Always verify your receipt first with the production URL; proceed to verify with the sandbox URL if you receive a 21007 status code. Following this approach ensures that you do not have to switch between URLs while your application is being tested or reviewed in the sandbox or is live in the App Store.
				
				static NSString *sandboxURL = @"https://sandbox.itunes.apple.com/verifyReceipt";
				[self verifyRequestData:requestData url:sandboxURL success:successBlock failure:failureBlock];
			}
			else
			{
				StoreLog(@"Verification Failed With Code %ld", (long)statusCode);
				NSError *serverError = [NSError errorWithDomain:RMStoreErrorDomain code:statusCode userInfo:nil];
				if (failureBlock != nil)
				{
					failureBlock(serverError);
				}
			}
		});
	});
}

@end
