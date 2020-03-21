//
//  BillingTransactionInfo.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 31/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>

enum ReceiptVerificationState
{
	ReceiptVerificationStateNotChecked,
	ReceiptVerificationStateSuccess,
	ReceiptVerificationStateFailed
};
typedef enum ReceiptVerificationState ReceiptVerificationState;

@interface BillingTransactionInfo : NSObject

// Properties
@property(nonatomic, retain)	SKPaymentTransaction		*transaction;
@property(nonatomic)			ReceiptVerificationState	verificationState;

// Static methods
+ (id)Create:(SKPaymentTransaction *)transaction;

// Related to conversion
- (id)toJsonObject;
- (const char *)toCString;

@end
