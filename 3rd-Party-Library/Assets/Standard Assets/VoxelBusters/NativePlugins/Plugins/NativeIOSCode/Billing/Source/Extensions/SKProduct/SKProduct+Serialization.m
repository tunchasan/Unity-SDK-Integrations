//
//  SKProduct+Serialization.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 06/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "SKProduct+Serialization.h"

@implementation SKProduct (Serialization)

#define kTitle				@"localized-title"
#define kProductID			@"product-identifier"
#define kDescription		@"localized-description"
#define kPrice				@"price"
#define kLocalizedPrice		@"localized-price"
#define kCurrencyCode		@"currency-code"
#define kCurrencySymbol		@"currency-symbol"

- (id)toJsonObject
{
    NSMutableDictionary *jsonDict	= [NSMutableDictionary dictionary];
    jsonDict[kProductID]  			= [self productIdentifier];
    jsonDict[kTitle]     			= [self localizedTitle];
    jsonDict[kDescription] 			= [self localizedDescription];
	
	// Price formatter
	NSNumberFormatter *priceFormatter 	= [[[NSNumberFormatter alloc] init] autorelease];
    [priceFormatter setFormatterBehavior:NSNumberFormatterBehavior10_4];
    [priceFormatter setNumberStyle:NSNumberFormatterCurrencyStyle];
    [priceFormatter setLocale:self.priceLocale];
	
	jsonDict[kPrice]				= self.price;
    jsonDict[kLocalizedPrice]		= [priceFormatter stringFromNumber:self.price];
	jsonDict[kCurrencyCode]			= [priceFormatter currencyCode];
	jsonDict[kCurrencySymbol]		= [priceFormatter currencySymbol];
	
	return jsonDict;
}

- (const char *)toCString
{
    return ToJsonCString([self toJsonObject]);
}

@end
