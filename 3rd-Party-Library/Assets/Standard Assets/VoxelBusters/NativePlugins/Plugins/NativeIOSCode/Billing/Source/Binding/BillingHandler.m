//
//  BillingHandler.mm
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 09/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "BillingHandler.h"
#import "SKProduct+Serialization.h"
#import "ReceiptVerificationManager.h"

@interface BillingHandler ()

// Properties
@property(nonatomic, retain)    NSSet              		*consumableProductIDs;
@property(nonatomic, retain)    NSSet              		*nonConsumableProductIDs;
@property(nonatomic, retain)    NSMutableSet      		*purchasedProductIDs;

@property(nonatomic, retain)    SKProductsRequest    	*productsRequest;
@property(nonatomic, retain)    NSArray                	*storeProductsList;
@property(nonatomic, retain)    NSMutableArray			*purchaseTransactionsList;
@property(nonatomic, retain)    NSMutableArray			*restoreTransactionsList;

@property(nonatomic)			BOOL					needsReceiptVerfication;
@property(nonatomic)			BOOL					verifiedAppLaunchTransactions;

@end

@implementation BillingHandler

#define kBillingProductRequestFinishedEvent		@"DidReceiveBillingProducts"
#define kPurchaseTransactionFinishedEvent		@"DidFinishProductPurchase"
#define kRestoreTransactionFinishedEvent		@"DidFinishRestoringPurchases"

#define kProductsKey							@"products"
#define kTransactionsKey						@"transactions"
#define kErrorKey								@"error"

@synthesize consumableProductIDs;
@synthesize nonConsumableProductIDs;
@synthesize purchasedProductIDs;

@synthesize productsRequest;
@synthesize storeProductsList;
@synthesize purchaseTransactionsList;
@synthesize restoreTransactionsList;

@synthesize needsReceiptVerfication;
@synthesize verifiedAppLaunchTransactions;

#pragma mark - LifeCycle Methods

+ (void)load
{
    [BillingHandler Instance];
}

- (id)init
{
    if ((self = [super init]))
    {
        // Initialize
		self.purchasedProductIDs   		= [NSMutableSet set];
		
		self.storeProductsList			= NULL;
		self.purchaseTransactionsList	= [NSMutableArray array];
		self.restoreTransactionsList	= [NSMutableArray array];
		
		self.verifiedAppLaunchTransactions	= NO;
		
		// Register for transaction callbacks
		[[SKPaymentQueue defaultQueue] addTransactionObserver:self];
    }
    return self;
}

- (void)dealloc
{
	// Unregister from callbacks
	[[SKPaymentQueue defaultQueue] removeTransactionObserver:self];

	// Release objects
	self.consumableProductIDs   	= nil;
	self.nonConsumableProductIDs	= nil;
	self.purchasedProductIDs   		= nil;
	
    self.productsRequest       		= nil;
	self.storeProductsList			= nil;
	self.purchaseTransactionsList	= nil;
	self.restoreTransactionsList	= nil;
	
    [super dealloc];
}

#pragma mark - Initialisation Methods

- (void)configureVerificationSettings:(BOOL)verifyReceipt
						  usingServer:(NSString *)serverURL
						 sharedSecret:(NSString *)secretKey
{
	NSLog(@"[BillingHandler] application supports receipt verification: %d", verifyReceipt);
	
	// Update properties
	self.needsReceiptVerfication	= verifyReceipt;
	
	// Set verifier properties
	ReceiptVerificationManager	*verificationManager	= [ReceiptVerificationManager Instance];
	
	[verificationManager setCustomServerURLString:serverURL];
	[verificationManager setSharedSecretKey:secretKey];
}

- (void)setConsummabledProducts:(NSSet *)consummableProductIDList andNonConsummableProducts:(NSSet *)nonConsummableProductIDList
{
	// Store product identifiers
	self.consumableProductIDs    	= consummableProductIDList;
	self.nonConsumableProductIDs 	= nonConsummableProductIDList;
	
	// Update purchase info
	[self reloadPurchaseHistory];
}

#pragma mark - SKProducts Methods

- (void)requestForBillingProducts
{
	NSLog(@"[BillingHandler] Requesting billing product details.");
	
	// If a request is already active, then cancel and destory it
	if (self.productsRequest != NULL)
	{
		[self.productsRequest cancel];
		self.productsRequest		= nil;
	}
	
	// Reset associated data
	self.storeProductsList			= nil;

	// Create product request
    NSSet *productIdentifiers       = [self.consumableProductIDs setByAddingObjectsFromSet:self.nonConsumableProductIDs];
    self.productsRequest            = [[[SKProductsRequest alloc] initWithProductIdentifiers:productIdentifiers] autorelease];
    self.productsRequest.delegate   = self;
    
    // Start request
    [self.productsRequest start];
}

- (void)onProductRequestFinished:(NSArray *)storeProducts error:(NSError *)error
{
	// Release product request
	self.productsRequest        	= nil;
	
	// Cache products info
	self.storeProductsList	    	= storeProducts;
	
	// Notify Unity
	NSMutableDictionary	*dataDict	= [NSMutableDictionary dictionary];
	
	if (error != NULL)
	{
		[dataDict setObject:[error description] forKey:kErrorKey];
	}
	
	if (storeProducts != NULL)
	{
		NSMutableArray *productsJSONList	= [NSMutableArray array];
		
		// Iterate through product details
		for (SKProduct *currentStoreProduct in storeProducts)
		{
			// Add product id and localized data
			[productsJSONList addObject:[currentStoreProduct toJsonObject]];
			NSLog(@"[BillingHandler] Loaded product with id: %@ title: %@.", currentStoreProduct.productIdentifier, currentStoreProduct.localizedTitle);
		}
		
		[dataDict setObject:productsJSONList forKey:kProductsKey];
	}
	
	NotifyEventListener([kBillingProductRequestFinishedEvent UTF8String], ToJsonCString(dataDict));
	
	// Also, invoke handler to finish off pending unfinished transactions
	if (!self.verifiedAppLaunchTransactions && error == NULL)
	{
		self.verifiedAppLaunchTransactions	= YES;
		
		// Verify
		if ([self.purchaseTransactionsList count] != 0)
		{
			[self verifyPurchaseTransactions];
		}
		
		if ([self.restoreTransactionsList count] != 0)
		{
			[self verifyRestoreTransactions];
		}
	}
}

#pragma mark - Buy Methods

- (BOOL)isProductPurchased:(NSString *)productID
{
    bool isPurchased	= [self.purchasedProductIDs containsObject:productID];
	NSLog(@"[BillingHandler] Product with id: %@ is already purchased: %d.", productID, isPurchased);
	
	return isPurchased;
}

- (void)buyProduct:(NSString *)productID quanity:(int)quantity
{
	// Find the store product matching given product identifier
    SKProduct *buyProduct = NULL;
    
    for (SKProduct *currentStoreProduct in self.storeProductsList)
    {
        if ([productID isEqualToString:currentStoreProduct.productIdentifier])
        {
            buyProduct = currentStoreProduct;
            break;
        }
    }
	
	// Initiate payment request
    if (buyProduct == NULL)
	{
		[self onBuyProductFailed:[NSString stringWithFormat:@"The requested operation could not be completed because product info for id: %@ not found.", productID]];
		
		return;
	}
	
	NSLog(@"[BillingHandler] Buying product with id: %@.", productID);
	SKMutablePayment *payment  = [SKMutablePayment paymentWithProduct:buyProduct];
	[payment setQuantity:quantity];
	
	[[SKPaymentQueue defaultQueue] addPayment:payment];
}

- (void)onBuyProductFailed:(NSString *)errorMessage
{
	NSError *error = [NSError errorWithDomain:kNativePluginsErrorDomain
										 code:0
									 userInfo:@{
												NSLocalizedDescriptionKey : errorMessage
												}];
	
	[self notifyUnityAboutTransactionEvent:kPurchaseTransactionFinishedEvent withTransactions:NULL error:error];
}

#pragma mark - Purchase History Methods

- (void)reloadPurchaseHistory
{
	// Clear in-memory information
	[self.purchasedProductIDs removeAllObjects];
	
	// Read stored info
	for (NSString *currentProductID in self.nonConsumableProductIDs)
	{
		BOOL isPurchased = [[NSUserDefaults standardUserDefaults] boolForKey:currentProductID];
		
		if (isPurchased)
		{
			NSLog(@"[BillingHandler] Previously purchased product id: %@.", currentProductID);
			[self.purchasedProductIDs addObject:currentProductID];
		}
	}
}

- (void)clearPurchaseHistory
{
	// Clear in-memory information
	[self.purchasedProductIDs removeAllObjects];

	// Remove stored info
	for (NSString *currentProductID in self.nonConsumableProductIDs)
		[[NSUserDefaults standardUserDefaults] removeObjectForKey:currentProductID];
	
	// Save changes
	[[NSUserDefaults standardUserDefaults] synchronize];
}

- (void)updatePurchaseHistory:(BillingTransactionInfo *)transactionInfo
{
	SKPaymentTransaction *transaction	= transactionInfo.transaction;

	// Check whether product is non consumable product
	NSString *productID					= transaction.payment.productIdentifier;
	if (![self.nonConsumableProductIDs containsObject:productID])
		return;
	
	// Only successfully validated transactions are considered
	if ([transactionInfo verificationState] != ReceiptVerificationStateSuccess)
		return;
	
	// Update user defaults, marking this product is purchased
	SKPaymentTransactionState transactionState	= transaction.transactionState;
	if (transactionState == SKPaymentTransactionStatePurchased || transactionState == SKPaymentTransactionStateRestored)
	{
		if (![self.purchasedProductIDs containsObject:productID])
		{
			[self.purchasedProductIDs addObject:productID];
			
			[[NSUserDefaults standardUserDefaults] setBool:YES forKey:productID];
			[[NSUserDefaults standardUserDefaults] synchronize];
		}
	}
}

#pragma mark - Transaction Methods

- (void)restoreCompletedTransactions
{
    NSLog(@"[BillingHandler] Requesting to restore completed transactions.");
    [[SKPaymentQueue defaultQueue] restoreCompletedTransactions];
}

- (void)finishCompletedTransactions:(NSArray *)transactionIDs ofRestoreType:(BOOL)isRestoreType
{
	NSMutableArray *transactionsList	= isRestoreType
	? restoreTransactionsList
	: purchaseTransactionsList;
	
	for (NSString *transactionID in transactionIDs)
	{
		NSPredicate *predicate	= [NSPredicate predicateWithFormat:@"SELF.transaction.transactionIdentifier == %@", transactionID];
		NSArray *filteredArray 	= [transactionsList filteredArrayUsingPredicate:predicate];
		
		if ([filteredArray count] > 0)
		{
			BillingTransactionInfo *transactionInfo	= filteredArray.firstObject;
			
			[self updatePurchaseHistory:transactionInfo];
			
			[[SKPaymentQueue defaultQueue] finishTransaction:transactionInfo.transaction];
			[transactionsList removeObject:transactionInfo];
		}
	}
}


- (void)customReceiptVerificationFinishedForTransactionWithID:(NSString *)transactionID
											 transactionState:(SKPaymentTransactionState)transactionState
											verificationState:(ReceiptVerificationState)verificationState
{
	NSString *eventName;
	NSMutableArray *transactionsList;
	
	if (transactionState == SKPaymentTransactionStateRestored)
	{
		eventName			= kRestoreTransactionFinishedEvent;
		transactionsList	= self.restoreTransactionsList;
	}
	else
	{
		eventName			= kPurchaseTransactionFinishedEvent;
		transactionsList	= self.purchaseTransactionsList;
	}
	
	NSPredicate *predicate	= [NSPredicate predicateWithFormat:@"SELF.transaction.transactionIdentifier == %@", transactionID];
	NSArray *filteredArray 	= [transactionsList filteredArrayUsingPredicate:predicate];
	
	if ([filteredArray count] > 0)
	{
		BillingTransactionInfo *transactionInfo	= filteredArray.firstObject;
		[transactionInfo setVerificationState:verificationState];
		
		[self notifyUnityAboutTransactionEvent:eventName withTransactions:filteredArray error:NULL];
	}
}

#pragma mark - Verify Transaction

- (void)verifyPurchaseTransactions
{
	NSArray *transactionListCopy = [[self.purchaseTransactionsList copy] autorelease];
	
	if ([self needsReceiptVerfication])
	{
		[self verifyReceiptForTransactions:transactionListCopy :^() {
			
			// Send purchase data to Unity
			[self notifyUnityAboutTransactionEvent:kPurchaseTransactionFinishedEvent withTransactions:transactionListCopy error:NULL];
		}];
	}
	else
	{
		// Send purchase data to Unity
		[self notifyUnityAboutTransactionEvent:kPurchaseTransactionFinishedEvent withTransactions:transactionListCopy error:NULL];
	}
}

- (void)verifyRestoreTransactions
{
	NSArray *transactionListCopy = [[self.restoreTransactionsList copy] autorelease];
	
	if ([self needsReceiptVerfication])
	{
		[self verifyReceiptForTransactions:transactionListCopy :^() {
			
			// Invoke handler
			[self notifyUnityAboutTransactionEvent:kRestoreTransactionFinishedEvent withTransactions:transactionListCopy error:NULL];
		}];
	}
	else
	{
		[self notifyUnityAboutTransactionEvent:kRestoreTransactionFinishedEvent withTransactions:transactionListCopy error:NULL];
	}
}

- (void)verifyReceiptForTransactions:(NSArray *)finishedTransactions :(void (^)())completionBlock
{
	if ([finishedTransactions count] == 0)
	{
		if (completionBlock != NULL)
			completionBlock();
		
		return;
	}
	
	// Retain, to own the object
	[finishedTransactions retain];
	
	// Reset all the transactions verification state
	for (BillingTransactionInfo *currentTransactionInfo in finishedTransactions)
		[currentTransactionInfo setVerificationState:ReceiptVerificationStateNotChecked];
	
	// Iterate through each transaction and verify it
	for (BillingTransactionInfo *currentTransactionInfo in finishedTransactions)
	{
		[[ReceiptVerificationManager Instance] verifyPurchase:currentTransactionInfo.transaction:^(BOOL success) {
			
			// Check status
			if (success)
				[currentTransactionInfo setVerificationState:ReceiptVerificationStateSuccess];
			else
				[currentTransactionInfo setVerificationState:ReceiptVerificationStateFailed];
			
			// If all transaction are verified then invoke completion handler
			BOOL isFinished	= [self finishedVerifyingAllTransactions:finishedTransactions];
			
			if (isFinished)
			{
				if (completionBlock != NULL)
					completionBlock();
				
				// Release array
				[finishedTransactions release];
			}
		}];
	}
}

- (BOOL)finishedVerifyingAllTransactions:(NSArray *)transactions
{
	for (BillingTransactionInfo *currentTransactionInfo in transactions)
	{
		if ([currentTransactionInfo verificationState] == ReceiptVerificationStateNotChecked)
			return NO;
	}
	
	return YES;
}

- (void)notifyUnityAboutTransactionEvent:(NSString *)eventName withTransactions:(NSArray *)transactions error:(NSError *)error
{
	NSMutableDictionary	*dataDict			= [NSMutableDictionary dictionary];
	
	if (error != NULL)
	{
		[dataDict setObject:[error description] forKey:kErrorKey];
	}
	
	if (transactions != NULL)
	{
		// Always safe to own transactions array
		[transactions retain];
		
		// Create JSON list
		NSMutableArray *transactionJSONList	= [NSMutableArray array];
		
		for (BillingTransactionInfo *currentTransactionInfo in transactions)
			[transactionJSONList addObject:[currentTransactionInfo toJsonObject]];
		
		[dataDict setObject:transactionJSONList forKey:kTransactionsKey];
		
		// Releasing ownership
		[transactions release];
	}
	
	// Notify Unity
	NotifyEventListener([eventName UTF8String], ToJsonCString(dataDict));
}

#pragma mark - SKProductsRequestDelegate Methods

- (void)productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response
{
	NSLog(@"[BillingHandler] Store products successfully loaded.");
	
	// Invoke handler
	[self onProductRequestFinished:response.products error:NULL];
}

- (void)request:(SKRequest *)request didFailWithError:(NSError *)error
{
    NSLog(@"[BillingHandler] Failed to load store products %@.", [error description]);
    
	// Invoke handler
	[self onProductRequestFinished:NULL error:error];
}

#pragma mark SKPaymentTransactionObserver Methods

- (void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray *)transactions
{
	NSMutableArray *newPurchaseTransactionsList	= [NSMutableArray array];
	NSMutableArray *newRestoreTransactionsList	= [NSMutableArray array];

	for (SKPaymentTransaction *transaction in transactions)
    {
		SKPaymentTransactionState transactionState	= [transaction transactionState];
		
		switch (transactionState)
		{
#ifdef __IPHONE_8_0
			case SKPaymentTransactionStateDeferred:
				break;
#endif
			case SKPaymentTransactionStatePurchasing:
				break;
				
			case SKPaymentTransactionStateRestored:
				[newRestoreTransactionsList addObject:[BillingTransactionInfo Create:transaction]];
				break;
			
			case SKPaymentTransactionStatePurchased:
			case SKPaymentTransactionStateFailed:
				[newPurchaseTransactionsList addObject:[BillingTransactionInfo Create:transaction]];
				break;
				
			default:
				NSLog(@"Unhandled transaction state.");
				break;
		}
	}
	
	// Update transaction list
	[self.purchaseTransactionsList addObjectsFromArray:newPurchaseTransactionsList];
	[self.restoreTransactionsList addObjectsFromArray:newRestoreTransactionsList];
	
	// Transactions will be handled only after getting product info
	if (self.storeProductsList == NULL)
		return;
	
	if ([newPurchaseTransactionsList count] != 0)
		[self verifyPurchaseTransactions];
}

- (void)paymentQueue:(SKPaymentQueue *)queue restoreCompletedTransactionsFailedWithError:(NSError *)error
{
	[self notifyUnityAboutTransactionEvent:kRestoreTransactionFinishedEvent withTransactions:NULL error:error];
}

- (void)paymentQueueRestoreCompletedTransactionsFinished:(SKPaymentQueue *)queue
{
	// Transactions will be handled only after getting product info
	if (self.storeProductsList == NULL)
		return;
	
	// We just retrieved new set of restorable products, so we are good to flush old history
	[self clearPurchaseHistory];
	
	// Verify all the purchases
	[self verifyRestoreTransactions];
}

- (BOOL)paymentQueue:(SKPaymentQueue *)queue shouldAddStorePayment:(SKPayment *)payment forProduct:(SKProduct *)product
{
  return YES;
}

@end
