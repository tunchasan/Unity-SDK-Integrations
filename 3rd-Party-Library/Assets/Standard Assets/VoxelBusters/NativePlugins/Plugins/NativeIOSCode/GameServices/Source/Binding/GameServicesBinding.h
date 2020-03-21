//
//  GameServicesBinding.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 27/05/15.
//
//

#import <Foundation/Foundation.h>

UIKIT_EXTERN bool isGameCenterAvailable ();

// UI methods
UIKIT_EXTERN void showLeaderboardView (const char* leaderboardID, int timeScope);
UIKIT_EXTERN void showAchievementView ();
UIKIT_EXTERN void showDefaultAchievementCompletionBanner (bool show);

// Achievement methods
UIKIT_EXTERN void loadAchievements ();
UIKIT_EXTERN void reportProgress (const char* achievementInfoJSON, double percentComplete);

// Achievement description methods
UIKIT_EXTERN void loadAchievementDescriptions ();
UIKIT_EXTERN void loadAchievementImage (const char* descriptionInfoJSON);
UIKIT_EXTERN char* getIncompleteAchievementDefaultImagePath ();
UIKIT_EXTERN char* getCompletedAchievementPlaceholderImagePath ();

// Leaderboard methods
UIKIT_EXTERN void loadLeaderboards ();
UIKIT_EXTERN void loadLeaderboardImage (const char* leaderboardInfoJSON);
UIKIT_EXTERN void loadScores(const char* leaderboardInfoJSON);

// Local Player methods
UIKIT_EXTERN bool isAuthenticated ();
UIKIT_EXTERN void authenticatePlayer ();
UIKIT_EXTERN void loadFriendPlayers ();

// Score methods
UIKIT_EXTERN void reportScore (const char* scoreInfoJSON, long value);

// Player methods
UIKIT_EXTERN void loadPlayers (const char* identifiersJSON);
UIKIT_EXTERN void loadPhoto (const char* playerInfoJSON, int photoSize);