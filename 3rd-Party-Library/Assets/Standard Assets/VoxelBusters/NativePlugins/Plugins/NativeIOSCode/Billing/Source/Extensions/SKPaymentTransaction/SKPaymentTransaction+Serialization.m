//
//  SKPaymentTransaction+Serialization.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 06/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "SKPaymentTransaction+Serialization.h"
#import "NSData+Base64.h"

@implementation SKPaymentTransaction (Serialization)

- (NSString *)transactionReceiptInBase64Format
{
	NSBundle *bundle 		= [NSBundle mainBundle];
	NSData *receiptData		= NULL;
	
	// iOS 7 or later.
#ifdef __IPHONE_7_0
	if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"7.0"))
	{
		// Get the transaction receipt in the app bundle.
		receiptData	= [NSData dataWithContentsOfURL:[bundle appStoreReceiptURL]];
	}
	else
#endif
	{
		receiptData	= [self transactionReceipt];
	}

	if (receiptData != NULL)
	{
		NSString* receipt 	= [receiptData stringByBase64Encoding];
		return receipt;
	}
	
	return NULL;
}

#define kProductID				@"product-identifier"
#define kQuantity				@"quantity"
#define kTransactionDate		@"transaction-date"
#define kTransactionID			@"transaction-identifier"
#define kTransactionReceipt		@"transaction-receipt"
#define kTransactionState		@"transaction-state"
#define kError					@"error"

- (id)toJsonObject
{
    NSMutableDictionary *jsonDict	= [NSMutableDictionary dictionary];
	
	// Product id, quantity
	NSString *productID				= [[self payment] productIdentifier];
	jsonDict[kProductID]			= productID;
	jsonDict[kQuantity]				= [NSNumber numberWithInteger:[[self payment] quantity]];

	// Transaction date
	NSString *tDate					= [Utility ConvertNSDateToNSString:[self transactionDate]];
	
	if (tDate != NULL)
		jsonDict[kTransactionDate]	= tDate;
	
	// Transaction id
	jsonDict[kTransactionID]		= [self transactionIdentifier];
	
	// Transaction receipt
	NSString *tReceipt				= [self transactionReceiptInBase64Format];
	
	if (tReceipt != NULL)
		jsonDict[kTransactionReceipt]	= tReceipt;

	// Transaction state
	jsonDict[kTransactionState]		= [NSNumber numberWithInt:[self transactionState]];
	
	// Error
	jsonDict[kError]				= ([self transactionState] == SKPaymentTransactionStateFailed) ? [[self error] description] : kNSStringDefault;

	return jsonDict;
}

- (const char *)toCString
{
    return ToJsonCString([self toJsonObject]);
}

@end
