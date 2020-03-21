//
//  ReceiptVerificationManager.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 17/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "ReceiptVerificationManager.h"
#import "RMStore.h"
#import "RMStoreAppReceiptVerificator.h"
#import "RMStoreTransactionReceiptVerificator.h"

// Constants
const NSString *bundleIdentifier	= nil;
const NSString *bundleVersion		= nil;

@interface ReceiptVerificationManager ()

// Properties
@property(nonatomic, retain)	id<RMStoreReceiptVerificator> receiptVerificator;

@end

@implementation ReceiptVerificationManager

@synthesize receiptVerificator;

#pragma mark - Lifecycle Methods

- (id)init
{
	self	= [super init];
	
	if (self)
	{
		// Initialize
		[self configure];
	}
	
	return self;
}

- (void)dealloc
{
	// Release
	self.receiptVerificator	= nil;
	
	[super dealloc];
}

#pragma mark - Methods

- (void)configure
{
	// Base on OS set appropriate receipt verificator
	const BOOL iOS7OrHigher = floor(NSFoundationVersionNumber) > NSFoundationVersionNumber_iOS_6_1;
	
	if (iOS7OrHigher)
	{
		RMStoreAppReceiptVerificator *appReceiptVerificator = [[[RMStoreAppReceiptVerificator alloc] init] autorelease];
		[appReceiptVerificator setBundleIdentifier:[bundleIdentifier copy] ];
		[appReceiptVerificator setBundleVersion:[bundleVersion copy]];
		
		self.receiptVerificator	= appReceiptVerificator;
	}
	else
	{
		self.receiptVerificator	= [[[RMStoreTransactionReceiptVerificator alloc] init] autorelease];
	}
}

- (void)setCustomServerURLString:(NSString *)URLString
{
	[self.receiptVerificator setCustomServerURLString:URLString];
}

- (void)setSharedSecretKey:(NSString *)newKey
{
	[self.receiptVerificator setSharedSecretKey:newKey];
}

- (void)verifyPurchase:(SKPaymentTransaction *)transaction :(void (^)(BOOL success))completionBlock
{
	// If transaction is failed, then transaction need not be verified
	if ([transaction transactionState] == SKPaymentTransactionStateFailed)
	{
		if (completionBlock != NULL)
			completionBlock(TRUE);

		return;
	}
	
	// For successful transactions, execute receipt verification
	[self.receiptVerificator verifyTransaction:transaction success:^{
		if (completionBlock != NULL)
			completionBlock(TRUE);
	} failure:^(NSError *error) {
		if (completionBlock != NULL)
			completionBlock(FALSE);
	}];
}

@end
