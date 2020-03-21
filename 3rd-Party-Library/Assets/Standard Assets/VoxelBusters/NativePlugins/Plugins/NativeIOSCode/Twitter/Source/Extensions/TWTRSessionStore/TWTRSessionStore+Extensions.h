//
//  TWTRSessionStore+Extensions.h
//  Unity-iPhone
//
//  Created by Ashwin kumar on 01/06/16.
//
//

#import <Foundation/Foundation.h>
#import <TwitterKit/TwitterKit.h>

@interface TWTRSessionStore (Extensions)

// Methods
- (NSDictionary *)getSessionDictionaryWithUserID:(NSString *)userID;

@end
