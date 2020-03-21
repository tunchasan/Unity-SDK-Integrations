using UnityEngine;

#if USES_GAME_SERVICES && UNITY_ANDROID
namespace VoxelBusters.NativePlugins
{
	using Internal;
	
	public sealed partial class GameServicesAndroid : GameServices 
	{
		#region Platform Native Info

		internal class Native
		{
			// Handler class name
			internal class Class
			{
				internal const string NAME			= "com.voxelbusters.nativeplugins.features.gameservices.GameServicesHandler";
			}
			
			// For holding method names
			internal class Methods
			{
				// Service query
				internal const string 	IS_SERVICE_AVAILABLE			=	"isServiceAvailable";
				internal const string 	REGISTER_SERVICE				=	"register";
				
				//Achievements
				internal const string 	REPORT_PROGRESS					=	"reportProgress";
				internal const string 	LOAD_ACHIEVEMENT_DESCRIPTIONS	=	"loadAchievementDescriptions";
				internal const string 	LOAD_ACHIEVEMENTS				=	"loadAchievements";
				internal const string 	SHOW_ACHIEVEMENTS_UI 			=	"showAchievementsUi";
				internal const string 	GET_ACHIEVEMENT_DATA 			=	"getAchievement";
				
				
				//Leaderboards
				internal const string 	LOAD_TOP_SCORES					=	"loadTopScores";
				internal const string 	LOAD_PLAYER_CENTERED_SCORES		=	"loadPlayerCenteredScores";
				internal const string 	LOAD_MORE_SCORES				=	"loadMoreScores";
				internal const string 	REPORT_SCORE 					=	"reportScore";
				internal const string 	SHOW_LEADERBOARD_UI				=	"showLeaderboardsUi";
	
				//User Details
				internal const string 	LOAD_USERS						=	"loadUsers";
				internal const string 	LOAD_LOCAL_USER_FRIENDS			=	"loadLocalUserFriends";
				internal const string 	AUTHENTICATE_LOCAL_USER			=	"authenticateLocalUser";
				internal const string 	IS_LOCAL_USER_AUTHENTICATED		=	"isSignedIn";
				internal const string 	SIGN_OUT_LOCAL_USER				=	"signOut";
				internal const string 	LOAD_PROFILE_PICTURE			=	"loadProfilePicture";

				//Others
				internal const string 	SET_SHOW_DEFAULT_ERROR_DIALOGS			=	"setShowDefaultErrorDialogs";
				internal const string 	LOAD_EXTERNAL_AUTHENTICATION_DETAILS	= 	"loadExternalAuthenticationDetails";
			}
		}

		#endregion


		#region  Native Access Variables
		
		internal static AndroidJavaObject  	Plugin
		{
			get;
			set;
		}
		
		#endregion
	}
}

#endif
