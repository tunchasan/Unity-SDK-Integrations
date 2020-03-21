//
//  GameServicesHandler.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 27/05/15.
//
//

#import "HandlerBase.h"
#import <GameKit/GameKit.h>

@interface GameServicesHandler : HandlerBase <GKGameCenterControllerDelegate, UIPopoverControllerDelegate>

// Properties
@property(nonatomic, retain) NSMutableDictionary 	*descriptionInfoCache;
@property(nonatomic, retain) NSMutableDictionary 	*leaderboardInfoCache;
@property(nonatomic, retain) NSMutableDictionary 	*playerInfoCache;
@property(nonatomic, retain) UIPopoverController    *popoverController;
@property(nonatomic) bool showsCompletionBanner;

// Methods
- (BOOL)isGameCenterAvailable;

// UI methods
- (void)showLeaderboardView:(NSString *)leaderboardID withTimeScope:(GKLeaderboardTimeScope)timeScope;
- (void)showAchievementView;

// Achievements
- (void)loadAchievements;
- (void)reportAchievementProgress:(NSDictionary *)achievementInfoDict percentComplete:(double)percentComplete;

// Achievement description methods
- (void)loadAchievementDescriptions;
- (void)loadAchievementImage:(NSDictionary *)descriptionInfoDict;
- (NSString *)getIncompleteAchievementDefaultImagePath;
- (NSString *)getCompletedAchievementPlaceholderImagePath;

// Leaderboard methods
- (void)loadLeaderboards;
- (void)loadLeaderboardImage:(NSDictionary *)leaderboardInfoDict;
- (void)loadScores:(NSDictionary *)leaderboardInfoDict;

// Local Player methods
- (BOOL)isAuthenticated;
- (void)authenticatePlayer;
- (void)loadFriendPlayers;

// Score
- (void)reportScore:(NSDictionary *)scoreInfoDict withValue:(long)value;

// Player methods
- (void)loadPlayers:(NSArray *)identifiers;
- (void)loadPhoto:(NSDictionary *)playerInfoDict withPhotoSize:(GKPhotoSize)photoSize;
- (GKPlayer *)getPlayer:(NSString *)identifier;

@end
