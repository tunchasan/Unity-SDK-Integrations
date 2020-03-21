//
//  SKPaymentTransaction+Serialization.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 06/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>

@interface SKPaymentTransaction (Serialization)

// Extended methods
- (NSString *)transactionReceiptInBase64Format;

// Related to conversion
- (id)toJsonObject;
- (const char *)toCString;

@end
