//
//  CPNPStoreReviewBinding.m
//  Unity-iPhone
//
//  Created by Ashwin Kumar on 30/01/18.
//

#import "CPNPStoreReviewBinding.h"

@implementation CPNPStoreReviewBinding

bool cpnpUseDeepLinking()
{
    return SYSTEM_VERSION_LESS_THAN(@"10.3");
}

void cpnpRequestReview()
{
    [SKStoreReviewController requestReview];
}

@end
