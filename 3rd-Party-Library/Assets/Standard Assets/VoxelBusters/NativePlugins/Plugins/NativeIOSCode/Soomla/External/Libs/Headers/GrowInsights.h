/*
 * Copyright (C) 2012-2015 Soomla Inc. - All Rights Reserved
 *
 *   Unauthorized copying of this file, via any medium is strictly prohibited
 *   Proprietary and confidential
 *
 *   Written by Refael Dakar <refael@soom.la>
 */

#import <Foundation/Foundation.h>
@class PayInsights;
@class UserInsights;

/**
 Represents a manager class which is in charge of refreshing insights.
 */
@interface GrowInsights : NSObject

/**
 Gets the singleton instance of soomla insights
 @return The singleton instance
 */
+(GrowInsights*)getInstance;

/**
 Initializes the insights manager
 */
-(void)initialize;

/**
 Refreshed SoomlaInsights information from the server
 */
-(void)refreshInsights;

- (PayInsights*) getUserPayInsights;

/**
 Fetch the entire User Insights object
 */
- (UserInsights*)getUserInsights;

@end
