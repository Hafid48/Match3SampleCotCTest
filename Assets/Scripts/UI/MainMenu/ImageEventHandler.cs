using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Match3Sample.UI.MainMenu
{
	public class ImageEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
	{
		#region Fields
		public Sprite normalSprite;
		public Sprite pressedSprite;
		public AudioSource audioSource;
		public AudioClip clickSFX;
		public string goToSceneName;
		private Image myImage;
        #endregion

        #region Game Setup
        void Start()
		{
			myImage = GetComponent<Image>();
		}
        #endregion

        #region Pointer Event Handlers
        public void OnPointerDown(PointerEventData data)
		{
			myImage.sprite = pressedSprite;
		}

		public void OnPointerUp(PointerEventData data)
		{
			myImage.sprite = normalSprite;
		}

		public void OnPointerClick(PointerEventData data)
		{
			StartCoroutine(LoadScene());
		}
        #endregion

        #region Load Scene Helper
        private IEnumerator LoadScene()
		{
			/*
			audioSource.PlayOneShot (clickSFX);
			yield return new WaitForSeconds (clickSFX.length);
			*/
			yield return new WaitForSeconds(.15f);
			SceneManager.LoadScene(goToSceneName);
		}
        #endregion
    }
}
