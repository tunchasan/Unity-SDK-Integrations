//
//  UIHandler.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 17/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "HandlerBase.h"
#import "HybridAlert.h"

@interface UIHandler : HandlerBase <HybridAlertDelegate>

// Properties
@property(nonatomic)	CGPoint		popoverPoint;

- (void)showAlertViewWithTitle:(NSString *)title
                       message:(NSString *)message
                       buttons:(NSArray *)buttons
                  andCallerTag:(NSString*)cTag;

- (void)showSingleFieldPromptWithTitle:(NSString *)title
                               message:(NSString *)message
                       placeHolderText:(NSString *)placeholder
                         ofSecuredType:(BOOL)useSecureText
                            andButtons:(NSArray *)buttons;

- (void)showLoginPromptWithTitle:(NSString *)title
                         message:(NSString *)message
                 placeHolderText:(NSString *)placeholder1 :(NSString *)placeholder2
                      andButtons:(NSArray *)buttons;

@end
