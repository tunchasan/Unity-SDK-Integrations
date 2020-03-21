//
//  BillingHandler.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 09/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//  Credits: http://www.raywenderlich.com/21081/introduction-to-in-app-purchases-in-ios-6-tutorial
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>
#import "HandlerBase.h"
#import "BillingTransactionInfo.h"

@interface BillingHandler : HandlerBase <SKProductsRequestDelegate, SKPaymentTransactionObserver>

// Static
- (void)configureVerificationSettings:(BOOL)verifyReceipt
						  usingServer:(NSString *)serverURL
						 sharedSecret:(NSString *)secretKey;

// Related to products
- (void)setConsummabledProducts:(NSSet *)consummableProductIDList
	  andNonConsummableProducts:(NSSet *)nonConsummableProductIDList;
- (void)requestForBillingProducts;
- (BOOL)isProductPurchased:(NSString *)productID;
- (void)buyProduct:(NSString *)productID quanity:(int)quantity;

// Related to transaction
- (void)restoreCompletedTransactions;
- (void)finishCompletedTransactions:(NSArray *)transactionIDs ofRestoreType:(BOOL)isRestoreType;

- (void)customReceiptVerificationFinishedForTransactionWithID:(NSString *)transactionID
											 transactionState:(SKPaymentTransactionState)transactionState
											verificationState:(ReceiptVerificationState)verificationState;

@end
