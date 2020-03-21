//
//  HybridActionSheet.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 11/02/16.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

@class HybridActionSheet;

@protocol HybridActionSheetDelegate <NSObject>
@optional

- (void)actionSheet:(HybridActionSheet *)actionSheet clickedButton:(NSString *)button;

@end

@interface HybridActionSheet : NSObject <UIActionSheetDelegate, UIPopoverPresentationControllerDelegate>

@property(nonatomic, assign) 	id<HybridActionSheetDelegate>	delegate;
@property(nonatomic)			CGRect							sourceRect;

// Set properties method
- (void)setTitle:(NSString *)title;
- (void)setCancelButtonTitle:(NSString *)button;
- (void)setDestructiveButtonTitle:(NSString *)button;
- (void)setOtherButtons:(NSArray *)buttons;

// Presentation methods
- (void)presentFromViewController:(UIViewController *)viewController animated: (BOOL)flag completion:(void (^ __nullable)(void))completion;

@end
