using CotcSdk;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Match3Sample.Gameplay.Session;

namespace Match3Sample.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private Text playerNameText;
        [SerializeField]
        private Text playerRankText;
        #endregion

        #region Game Setup
        private void Start()
        {
            DisplayPlayerInfo();
        }
        #endregion

        #region Display Player Info UI
        public void DisplayPlayerInfo()
        {
            // Display profile name
            GameSession.Instance.Gamer.Profile.Get()
                .Then(gamerProfile => playerNameText.text = "Name : " + gamerProfile["displayName"])
                .Catch(ex =>
                {
                    // The exception should always be CotcException
                    CotcException error = (CotcException)ex;
                    Debug.LogError("Failed to get profile: " + error.ErrorCode + " (" + error.HttpStatusCode + ")");
                });
            // First get the best score of the player and check what rank he/she based on that
            GameSession.Instance.Gamer.Scores.PagedCenteredScore("LeaderboardTest", 1)
                .Then(scores => playerRankText.text = "Rank : " + scores[0].Rank)
                .Catch(ex =>
                {
                    // The exception should always be CotcException
                    CotcException error = (CotcException)ex;
                    Debug.LogError("Failed to get player rank: " + error.ErrorCode + " (" + error.HttpStatusCode + ")");
                });

        }
        #endregion

        #region LogOut
        public void LogOut()
        {
            if (GameSession.Instance.IsActive)
            {
                GameSession.Instance.Cloud.Logout(GameSession.Instance.Gamer)
                .Then(dummy => LoggedOut())
                .Catch(ex =>
                {
                    // The exception should always be CotcException
                    CotcException error = (CotcException)ex;
                    Debug.LogError("Failed to log out: " + error.ErrorCode + " (" + error.HttpStatusCode + ")");
                });

            }
        }

        public void LoggedOut()
        {
            GameSession.Instance.Stop();
            SceneManager.LoadScene("Authentication");
        }
        #endregion
    }
}
