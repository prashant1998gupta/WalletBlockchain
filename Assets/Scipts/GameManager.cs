using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace Prashant
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        public static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject gameManager = new GameObject("GameplayManager");
                    gameManager.AddComponent<GameManager>();
                }
                return instance;
            }
        }
        #endregion;

        #region Variables
     
        public GameObject newLoadingScreen;
        public GameObject popUpScreen;

        #endregion



        #region Monobehaviour Methods

        private void Awake()
        {
            instance = this;
        }

        #endregion

      
        public void SwitchSceneAsName(string sceneName)
        {
            newLoadingScreen.SetActive(true);
            StartCoroutine(LoadYourAsyncScene(sceneName));
          
        }

        public void Quit()
        {
            Application.Quit();
            
        }

        IEnumerator LoadYourAsyncScene(string sceneName)
        {
            // The Application loads the Scene in the background as the current Scene runs.
            // This is particularly good for creating loading screens.
            // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
            // a sceneBuildIndex of 1 as shown in Build Settings.

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
               
                yield return null;
            }
        }
    }
}

