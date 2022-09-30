using CotcSdk;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Match3Sample.Gameplay.Session
{
    [RequireComponent(typeof(CotcGameObject), typeof(CotcCoroutinesManager))]
    public class GameSession : MonoBehaviour
    {
        #region Properties
        public static GameSession Instance { get; private set; }
        public Cloud Cloud { get; private set; }
        public Gamer Gamer { get; private set; }
        public DomainEventLoop Loop { get; private set; }
        public bool IsActive { get { return Gamer != null; } }
        #endregion

        #region Game Setup
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        public void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            // Initiate getting the main Cloud object
            GetComponent<CotcGameObject>().GetCloud().Done(cloud =>
            {
                Cloud = cloud;
                // Retry failed HTTP requests once
                Cloud.HttpRequestFailedHandler = (HttpRequestFailedEventArgs e) =>
                {
                    if (e.UserData == null)
                    {
                        e.UserData = new object();
                        e.RetryIn(1000);
                    }
                    else
                        e.Abort();
                };
                Debug.Log("Setup done");
                ResumeOrLogin();
            });
        }
        #endregion

        #region Session Helpers
        public void Start(Gamer gamer, bool rememberMe)
        {
            if (IsActive)
                return;
            if (rememberMe)
            {
                PlayerPrefs.SetString("GID", gamer.GamerId);
                PlayerPrefs.SetString("GS", gamer.GamerSecret);
            }
            Gamer = gamer;
            Loop = Gamer.StartEventLoop();
            Loop.ReceivedEvent += OnLoopReceivedEvent;
            Debug.Log("Signed in successfully (ID = " + Gamer.GamerId + ")");
            SceneManager.LoadScene("MainMenu");
        }

        public void ResumeOrLogin()
        {
            if (PlayerPrefs.HasKey("GID") && PlayerPrefs.HasKey("GS"))
            {
                Cloud.ResumeSession(PlayerPrefs.GetString("GID"), PlayerPrefs.GetString("GS"))
                .Then(gamer => Start(gamer, false))
                .Catch(ex =>
                {
                    // The exception should always be CotcException
                    CotcException error = (CotcException)ex;
                    Debug.LogError("Failed to resume session: " + error.ErrorCode + " (" + error.HttpStatusCode + ")");
                });
            }
            else
                SceneManager.LoadScene("Authentication");
        }

        public void Stop()
        {
            if (!IsActive)
                return;
            Gamer = null;
            if (Loop != null)
                Loop.ReceivedEvent -= OnLoopReceivedEvent;
            PlayerPrefs.DeleteKey("GID");
            PlayerPrefs.DeleteKey("GS");
        }
        #endregion

        #region Events Loop
        public void OnLoopReceivedEvent(DomainEventLoop sender, EventLoopArgs e)
        {
            Debug.Log("Received event of type " + e.Message.Type + ": " + e.Message.ToJson());
        }
        #endregion
    }
}
