using UnityEngine;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Collections;

public class LeaderboardManager : MonoBehaviour
{

    public static LeaderboardManager Instance;

#if UNITY_ANDROID
    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        /*PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        // enables saving game progress.
        .EnableSavedGames()
        // requests the email address of the player be available.
        // Will bring up a prompt for consent.
        .RequestEmail()
        // requests a server auth code be generated so it can be passed to an
        //  associated back end server application and exchanged for an OAuth token.
        .RequestServerAuthCode(false)
        // requests an ID token be generated.  This OAuth token can be used to
        //  identify the player to other services such as Firebase.
        .RequestIdToken()
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
        if (!Social.localUser.authenticated)
        {
            Debug.Log("Login attempt");
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Login success");
                    Social.ReportScore(PlayerPrefs.GetInt("HighScore", 0), "CgkIk4zv-fsfEAIQAA", (bool successScore) =>
                    {
                        if (successScore)
                            Debug.Log("Report score success");
                        else
                            Debug.Log("Report score fail");
                    });
                }
                else
                {
                    Debug.Log("Login fail");
                }
            });
        }
        */
    }

    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

#endif
}
