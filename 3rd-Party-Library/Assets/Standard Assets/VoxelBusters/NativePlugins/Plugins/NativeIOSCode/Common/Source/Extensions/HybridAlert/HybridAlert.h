//
//  HybridAlert.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 11/02/16.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <UIKit/UIKit.h>

@class HybridAlert;

enum HybridAlertStyle
{
	HybridAlertStyleDefault = 0,
	HybridAlertStyleSecureTextInput,
	HybridAlertStylePlainTextInput,
	HybridAlertStyleLoginAndPasswordInput
};
typedef enum HybridAlertStyle HybridAlertStyle;

@protocol HybridAlertDelegate <NSObject>
@optional

- (void)alert:(HybridAlert *)alert clickedButton:(NSString *)button;

@end

@interface HybridAlert : NSObject <UIAlertViewDelegate>

@property(nonatomic, assign) 	id<HybridAlertDelegate>	delegate;
@property(nonatomic, retain)	NSString				*tag;
@property(nonatomic)			HybridAlertStyle		style;

// Static methods
+ (HybridAlert *)createAlertViewWithTitle:(NSString *)title
								  message:(NSString *)message
								  buttons:(NSArray *)buttons;

// Set properties method
- (void)setTitle:(NSString *)title;
- (void)setMessage:(NSString *)message;
- (void)setButtons:(NSArray *)buttons;
- (UITextField *)textFieldAtIndex:(NSInteger)index;

// Presentation methods
- (void)presentFromViewController:(UIViewController *)viewController;

@end
