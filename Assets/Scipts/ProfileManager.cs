using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using Firebase.Auth;
using UnityEngine.Networking;

namespace Prashant
{
    public class ProfileManager : MonoBehaviour
    {
        [Header("firebase")]
        Firebase.Auth.FirebaseAuth auth;


        [Space(1)]
        [Header("Profile Panel UI")]
        public Button profileButton;
        public GameObject profile;
       
        [Space(1)]
        [Header("Profile Panel enternal UI")]
        public List<Image> profileImage;
        public TextMeshProUGUI nameOfUser;
        public TextMeshProUGUI emailOfUser;
        public TextMeshProUGUI userId;
        public TextMeshProUGUI walletAddress;
        public Button copyTextButton;
        public PopupController popupController;
       
        //public TextMeshProUGUI missionProgress;

        [Space(1)]
        [Header("Profile Exit UI")]
        public Button profileBackButton;
        public string sceneName;


        [Space(1)]
        [Header("wellat UI")]
        public Button WellatButton;
        public GameObject wellatpanel;
        public Button wellatBackButton;

        [Header("Logout")]
        public Button logOut;

        GameObject currentSelected;

        private void Start()
        {
            
            auth = FirebaseAuth.DefaultInstance;

            profileBackButton.onClick.AddListener(OnProfileBackButtonClick);
            profileButton.onClick.AddListener(OnProfileButtonClick);
            
            WellatButton.onClick.AddListener(OnClickWalletButton);
            wellatBackButton.onClick.AddListener(OnClickWellatBackButton);

            logOut.onClick.AddListener(OnLogOutButtonClick);
            copyTextButton.onClick.AddListener(OnCopyButtonClick);
            StartCoroutine(LoadImage(PlayerPrefs.GetString("profileImage")));

        }


        // for copy text form unity
        public void OnCopyButtonClick()
        {
            CopyText(walletAddress.text);
            
        }

        private void CopyText(string text)
        {
            GUIUtility.systemCopyBuffer = text;
            Debug.Log("GUIUtility.systemCopyBuffer " + text);
            popupController.ShowPopup();
        }

        private void OnClickSwipCharacterPurchagePanel()
        {
          
            profile.SetActive(false);
        }

        private void OnClickWellatBackButton()
        {
            wellatpanel.SetActive(false);
        }

        private void OnClickWalletButton()
        {
            wellatpanel.SetActive(true);
        }

        private void OnTopButtonClick()
        {
            AudioManager.Instance.PlaySoundEffect("timeTick");
        }


      
        private void OnProfileBackButtonClick()
        {
            AudioManager.Instance.PlaySoundEffect("BackButtonClick");
            profile.SetActive(false);
        }

        public void OnProfileButtonClick()
        {
            profile.SetActive(true);

            nameOfUser.text = Utils.userName;
            emailOfUser.text = Utils.mail;
            userId.text = Utils.UserId;
            walletAddress.text = Utils.userWalletAccount;
        }

        private void OnLogOutButtonClick()
        {
            auth.SignOut();
            
            PlayerPrefs.SetInt(Utils.LOGGED, 0);

            Debug.Log("User logged out");

            GameManager.instance.newLoadingScreen.SetActive(true);

            StartCoroutine(LoadYourAsyncScene());

        }

        IEnumerator LoadYourAsyncScene()
        {

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                UnityEngine.Debug.Log(asyncLoad.progress);
                yield return null;
            }
        }


        public IEnumerator LoadImage(string imageUri)
        {
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUri))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Failed to load image: " + webRequest.error);
                    yield break;
                }

                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);

                foreach (var item in profileImage)
                {
                    item.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
        }
    }
}

