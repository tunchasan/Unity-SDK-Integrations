//
//  BillingBinding.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 13/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "BillingBinding.h"
#import "BillingHandler.h"

void cpnpBillingInit (bool verifyReceipt, const char* validateUsingServerURL, const char* sharedSecret)
{
	// Set needs verification
	[[BillingHandler Instance] configureVerificationSettings:verifyReceipt
												 usingServer:ConvertToNSString(validateUsingServerURL)
												sharedSecret:ConvertToNSString(sharedSecret)];
}

void cpnpBillingRequestForBillingProducts (const char* consumableProductIDs, const char* nonConsumableProductIDs)
{
	[[BillingHandler Instance] setConsummabledProducts:[NSSet setWithArray:ConvertToNSArray(consumableProductIDs)]
							 andNonConsummableProducts:[NSSet setWithArray:ConvertToNSArray(nonConsumableProductIDs)]];
	 
    // Requesting for store products
	[[BillingHandler Instance] requestForBillingProducts];
}

bool cpnpBillingCanMakePayments ()
{
	return [SKPaymentQueue canMakePayments];
}

bool cpnpBillingIsProductPurchased (const char* productID)
{
    return [[BillingHandler Instance] isProductPurchased:ConvertToNSString(productID)];
}

void cpnpBillingBuyProduct (const char* productID)
{
    [[BillingHandler Instance] buyProduct:ConvertToNSString(productID)
								  quanity:1];
}

void cpnpBillingRestoreCompletedTransactions ()
{
    [[BillingHandler Instance] restoreCompletedTransactions];
}

void cpnpBillingFinishCompletedTransactions (const char* transactionIDs, bool isRestoreType)
{
	[[BillingHandler Instance] finishCompletedTransactions:ConvertToNSArray(transactionIDs) ofRestoreType:isRestoreType];
}

void cpnpBillingCustomReceiptVerificationFinished (const char* transactionID, int transactionState, int verificationState)
{
	[[BillingHandler Instance] customReceiptVerificationFinishedForTransactionWithID:ConvertToNSString(transactionID)
																	transactionState:(SKPaymentTransactionState)transactionState
																   verificationState:(ReceiptVerificationState)verificationState];
}