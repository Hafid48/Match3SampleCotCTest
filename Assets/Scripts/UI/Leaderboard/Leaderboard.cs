using CotcSdk;
using UnityEngine;
using UnityEngine.UI;
using Match3Sample.Gameplay.Session;

namespace Match3Sample.UI.Leaderboard
{
    public class Leaderboard : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private Transform scoresParent;
        #endregion

        #region Game Setup
        private void Start()
        {
            FetchLeaderboard();
        }
        #endregion

        #region Fetch and Display Leaderboard
        private void FetchLeaderboard()
        {
            GameSession.Instance.Gamer.Scores.BestHighScores("LeaderboardTest")
                .Then(scores => DisplayLeaderboard(scores))
                .Catch(ex =>
                {
                    // The exception should always be CotcException
                    CotcException error = (CotcException)ex;
                    Debug.LogError("Failed to get profile: " + error.ErrorCode + " (" + error.HttpStatusCode + ")");
                });
        }

        private void DisplayLeaderboard(PagedList<Score> scores)
        {
            for (int i = 0; i < scores.Count; i++)
                scoresParent.GetChild(i).GetComponent<Text>().text = "#" + scores[i].Rank + " " + scores[i].GamerInfo["profile"]["displayName"] + " : " + scores[i].Value;
        }
        #endregion

    }
}
