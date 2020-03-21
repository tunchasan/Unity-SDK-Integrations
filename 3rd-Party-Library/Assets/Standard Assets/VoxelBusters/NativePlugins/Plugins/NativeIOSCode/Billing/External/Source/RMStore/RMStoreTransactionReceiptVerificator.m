//
//  RMStoreTransactionReceiptVerificator.m
//  RMStore
//
//  Created by Hermes Pique on 7/31/13.
//  Copyright (c) 2013 Robot Media SL (http://www.robotmedia.net)
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

#import "RMStoreTransactionReceiptVerificator.h"
#import "StoreReceiptServerVerificator.h"

#ifdef DEBUG
#define RMStoreLog(...) NSLog(@"RMStore: %@", [NSString stringWithFormat:__VA_ARGS__]);
#else
#define RMStoreLog(...)
#endif

@interface RMStoreTransactionReceiptVerificator ()

@property (nonatomic, strong) StoreReceiptServerVerificator *serverVerificator;

@end

@implementation RMStoreTransactionReceiptVerificator

@synthesize serverVerificator;

- (id)init
{
	self	= [super init];
	
	if (self != NULL)
	{
		// Initialize
		self.serverVerificator	= [[StoreReceiptServerVerificator alloc] init];
	}
	
	return self;
}

#pragma mark - Verificator Methods

- (void)setCustomServerURLString:(NSString *)URLString
{
	[[self serverVerificator] setCustomServerURLString:URLString];
}

- (void)setSharedSecretKey:(NSString *)newKey
{
	[[self serverVerificator] setSharedSecretKey:newKey];
}

- (void)verifyTransaction:(SKPaymentTransaction*)transaction
                           success:(void (^)())successBlock
                           failure:(void (^)(NSError *error))failureBlock
{    
	[[self serverVerificator] verifyReceiptData:transaction.transactionReceipt
										success:successBlock
										failure:failureBlock];
}

@end