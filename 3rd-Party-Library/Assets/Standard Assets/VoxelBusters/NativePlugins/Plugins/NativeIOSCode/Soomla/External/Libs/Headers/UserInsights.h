/*
 * Copyright (C) 2012-2015 Soomla Inc. - All Rights Reserved
 *
 *   Unauthorized copying of this file, via any medium is strictly prohibited
 *   Proprietary and confidential
 *
 *   Written by Refael Dakar <refael@soom.la>
 */

#import <Foundation/Foundation.h>

/**
 Represents the UserInsights and related classes
 */

typedef enum {
    Action=0,
    Adventure=1,
    Arcade=2,
    Board=3,
    Card=4,
    Casino=5,
    Casual=6,
    Educational=7,
    Family=8,
    Music=9,
    Puzzle=10,
    Racing=11,
    Role_Playing=12,
    Simulation=13,
    Sports=14,
    Strategy=15,
    Trivia=16,
    Word=17
} Genre;

typedef enum {
    _12am_6am=0,
    _6am_12pm=1,
    _12pm_6pm=2,
    _6pm_12am=3
} DayQuarter;

@interface PayInsights : NSObject {
    NSArray* payRankByGenre;
    NSArray* timeOfPurchase;
}

@property(nonatomic) NSArray* payRankByGenre;
@property(nonatomic) NSArray* timeOfPurchase;

- (instancetype)initWithDictionary:(NSDictionary*)payInsightsDict;
- (int)payRankForGenre:(Genre)genre;
- (double)purchaseLikelihoodForTime:(DayQuarter)dayQuarter;
- (NSDictionary*)toDictionary;

@end


@interface UserInsights : NSObject {
    PayInsights* payInsights;
}

@property(nonatomic, strong) PayInsights *payInsights;

- (instancetype)initWithDictionary:(NSDictionary*)userInsightsDict;
- (NSDictionary*)toDictionary;

@end

