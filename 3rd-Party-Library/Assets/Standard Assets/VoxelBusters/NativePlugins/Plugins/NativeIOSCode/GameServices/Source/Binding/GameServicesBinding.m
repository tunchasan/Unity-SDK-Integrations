//
//  GameServicesBinding.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 27/05/15.
//
//

#import "GameServicesBinding.h"
#import "GameServicesHandler.h"

bool isGameCenterAvailable ()
{
	return [[GameServicesHandler Instance] isGameCenterAvailable];
}

#pragma mark - UI Methods

void showLeaderboardView (const char* leaderboardID, int timeScope)
{
	[[GameServicesHandler Instance] showLeaderboardView:ConvertToNSString(leaderboardID) withTimeScope:(GKLeaderboardTimeScope)timeScope];
}

void showAchievementView ()
{
	[[GameServicesHandler Instance] showAchievementView];
}

void showDefaultAchievementCompletionBanner (bool show)
{
	[[GameServicesHandler Instance] setShowsCompletionBanner:show];
}

#pragma mark - Achievement Methods

void loadAchievements ()
{
	[[GameServicesHandler Instance] loadAchievements];
}

void reportProgress (const char* achievementInfoJSON, double percentComplete)
{
	[[GameServicesHandler Instance] reportAchievementProgress:FromJson(achievementInfoJSON) percentComplete:percentComplete];
}

#pragma mark - Achievement Description Methods

void loadAchievementDescriptions ()
{
	[[GameServicesHandler Instance] loadAchievementDescriptions];
}

void loadAchievementImage (const char* descriptionInfoJSON)
{
	[[GameServicesHandler Instance] loadAchievementImage:FromJson(descriptionInfoJSON)];
}

#pragma mark - Leaderboard Methods

void loadLeaderboards ()
{
	[[GameServicesHandler Instance] loadLeaderboards];
}

void loadScores(const char* leaderboardInfoJSON)
{
	[[GameServicesHandler Instance] loadScores:FromJson(leaderboardInfoJSON)];
}

#pragma mark - Local Player Methods

bool isAuthenticated ()
{
	return [[GameServicesHandler Instance] isAuthenticated];
}

void authenticatePlayer ()
{
	[[GameServicesHandler Instance] authenticatePlayer];
}

void loadFriendPlayers ()
{
	[[GameServicesHandler Instance] loadFriendPlayers];
}

#pragma mark - Score Methods

void reportScore (const char* scoreInfoJSON, long value)
{
	[[GameServicesHandler Instance] reportScore:FromJson(scoreInfoJSON) withValue:value];
 }

#pragma mark - Player Methods

void loadPlayers (const char* identifiersJSON)
{
	[[GameServicesHandler Instance] loadPlayers:FromJson(identifiersJSON)];
}

void loadPhoto (const char* playerInfoJSON, int photoSize)
{
	[[GameServicesHandler Instance] loadPhoto:FromJson(playerInfoJSON) withPhotoSize:(GKPhotoSize)photoSize];
}