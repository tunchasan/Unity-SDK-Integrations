//
//  ReceiptVerificationManager.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 17/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>
#import "Singleton.h"

@interface ReceiptVerificationManager : Singleton

// Set properties
- (void)setCustomServerURLString:(NSString *)URLString;
- (void)setSharedSecretKey:(NSString *)newKey;

// Verify methods
- (void)verifyPurchase:(SKPaymentTransaction *)transaction :(void (^)(BOOL success))completionBlock;

@end
