using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine;

public class GooglePlay : MonoBehaviour {

	void Start(){
		
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			// enables saving game progress.
			.EnableSavedGames()
			// registers a callback to handle game invitations received while the game is not running.
//			.WithInvitationDelegate(<callback method>)
//			// registers a callback for turn based match notifications received while the
//			// game is not running.
//			.WithMatchDelegate(<callback method>)
//			// require access to a player's Google+ social graph (usually not needed)
			//.RequireGooglePlus()
			.Build();

		PlayGamesPlatform.InitializeInstance(config);
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate();

		Social.localUser.Authenticate((bool success) => {
			// handle success or failure
		});
	}
		
}