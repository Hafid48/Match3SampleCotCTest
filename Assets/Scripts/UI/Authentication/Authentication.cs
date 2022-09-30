using CotcSdk;
using UnityEngine;
using UnityEngine.UI;
using Match3Sample.Gameplay.Session;

namespace Match3Sample.UI.Authentication
{
	public class Authentication : MonoBehaviour
	{
        #region Fields
        [SerializeField]
		private InputField emailInputTextField;
		[SerializeField]
		private InputField passwordInputTextField;
		[SerializeField]
		private Text errorText;
		[SerializeField]
		private Toggle rememberMeToggle;
        #endregion

        #region Login/SignIn
        public void OnSignInTapped()
		{
			DoLoginEmail();
		}

		// Log in by e-mail
		private void DoLoginEmail()
		{
			// You may also not provide a .Catch handler and use .Done instead of .Then. In that
			// case the Promise.UnhandledException handler will be called instead of the .Done
			// block if the call fails.
			GameSession.Instance.Cloud.Login(
				network: LoginNetwork.Email.Describe(),
				credentials: Bundle.CreateObject("id", emailInputTextField.text, "secret", passwordInputTextField.text))
				.Then(gamer => GameSession.Instance.Start(gamer, rememberMeToggle.isOn))
				.Catch(ex =>
				{
					// The exception should always be CotcException
					CotcException error = (CotcException)ex;
					errorText.text = @"Failed to login: " + error.ErrorCode + "(" + error.HttpStatusCode + ")";
					Debug.LogError("Failed to login: " + error.ErrorCode + " (" + error.HttpStatusCode + ")");
				});
		}
        #endregion
    }
}
