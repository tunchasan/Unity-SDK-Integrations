//
//  BillingBinding.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 13/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>

// Init
UIKIT_EXTERN void cpnpBillingInit (bool supportsReceiptValidation, const char* validateUsingServerURL, const char* sharedSecret);

// Product
UIKIT_EXTERN void cpnpBillingRequestForBillingProducts (const char* consumableProductIDs, const char* nonConsumableProductIDs);

// Purchase
UIKIT_EXTERN bool cpnpBillingCanMakePayments ();
UIKIT_EXTERN bool cpnpBillingIsProductPurchased (const char* productID);
UIKIT_EXTERN void cpnpBillingBuyProduct (const char* productID);
UIKIT_EXTERN void cpnpBillingRestoreCompletedTransactions ();
UIKIT_EXTERN void cpnpBillingFinishCompletedTransactions (const char* transactionIDs, bool isRestoreType);

UIKIT_EXTERN void cpnpBillingCustomReceiptVerificationFinished (const char* transactionID, int transactionState, int verificationState);