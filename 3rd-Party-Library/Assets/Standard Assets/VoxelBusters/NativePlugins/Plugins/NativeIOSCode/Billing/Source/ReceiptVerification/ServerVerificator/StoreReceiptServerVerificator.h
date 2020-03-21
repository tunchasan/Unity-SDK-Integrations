//
//  StoreReceiptServerVerificator.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 17/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface StoreReceiptServerVerificator : NSObject

// Properties
@property(nonatomic, retain)	NSString	*customServerURLString;
@property(nonatomic, retain)	NSString	*sharedSecretKey;

// Methods
- (void)verifyReceiptData:(NSData *)receiptData
				  success:(void (^)())successBlock
				  failure:(void (^)(NSError *error))failureBlock;
@end
