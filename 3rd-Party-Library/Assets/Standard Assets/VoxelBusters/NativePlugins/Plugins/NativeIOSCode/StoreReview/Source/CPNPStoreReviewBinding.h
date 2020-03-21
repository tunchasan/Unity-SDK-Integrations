//
//  CPNPStoreReviewBinding.h
//  Unity-iPhone
//
//  Created by Ashwin Kumar on 30/01/18.
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>

@interface CPNPStoreReviewBinding : NSObject

UIKIT_EXTERN bool cpnpUseDeepLinking();
UIKIT_EXTERN void cpnpRequestReview();

@end
