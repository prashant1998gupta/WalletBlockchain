using UnityEngine;
using Firebase.Auth;
using System.Linq;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using System.Collections;
using Firebase.Database;
using System.Threading.Tasks;
using System;
using System.Text.RegularExpressions;

namespace Prashant
{
    public class AuthenticationManager : MonoBehaviour
    {
        Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
        public static FirebaseAuth auth;
        public static FirebaseUser user;

        Web3AuthSample web3AuthScript;
        public string sceneName;

        public bool isInternetAvailable = true;
        GameObject internetCanvas;

        public static DatabaseReference databaseReference;

        private void Awake()
        {
            InternetChecker.MyInternet += MyListener;
            internetCanvas = FindObjectOfType<DontDestroyManager>().internetCanvas;

            web3AuthScript = FindObjectOfType<Web3AuthSample>();
            DatabaseManager.OnLoadToLobby += LoadScene;
            web3AuthScript.onWeb3authLogin += ButtonClicked;
            Debug.Log($"Awake and DatabaseManager.Instance.OnLoadToLobby {DatabaseManager.OnLoadToLobby.Method}");
        }

       

        private void Start()
        {
            // Initialize Firebase Authentication
            auth = FirebaseAuth.DefaultInstance;

            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                UnityEngine.Debug.Log("this is start");

                dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    Debug.Log("this is start");
                    InitializeFirebase();
                }
                else
                {
                    Debug.Log("this is start");
                }
            });
        }

        void InitializeFirebase()
        {
            auth = FirebaseAuth.DefaultInstance;

            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;


            //Email sign in auto when open app if remember me toggle checked
            if (PlayerPrefs.GetInt(Utils.LOGGED) == 1)
            {
                CheckInternetConn();
                if (!isInternetAvailable)
                {
                    return;
                }
                web3AuthScript.loading.SetActive(true);
                Login(PlayerPrefs.GetString("email"), Prashant.Utils.password);
            }

        }


        public void ButtonClicked()
        {
            string email = Prashant.Utils.mail;
            string password = Prashant.Utils.password;
            //CheckUserExists(email);
            CheckUserExists1(email, password);
        }

        private void CheckUserExists(string email)
        {
            // Check if the user already exists based on the email address
            auth.SignInWithEmailAndPasswordAsync(email, "123456").ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    string errorMessage;
                    // Get the error message
                    for (int i = 0; i < task.Exception.InnerExceptions.Count; i++)
                    {
                        errorMessage = task.Exception.InnerExceptions[i].Message;
                        Debug.LogError(errorMessage);

                        if (errorMessage.Contains("There is no user record corresponding to this identifier. The user may have been deleted"))
                        {
                            Debug.Log("User does not exist!");
                            // Proceed with sign-up functionality
                            // You can call the SignUp() method here
                           // SignUp(email, Prashant.Utils.password);
                        }
                        else
                        {
                            Debug.LogError("Failed to sign in: " + task.Exception);
                        }
                    }

                }
                else
                {
                    Debug.Log("User already exists!");
                    // Proceed with log-in functionality
                    // You can call the Login() method here
                    //Login(email, Prashant.Utils.password);
                }
            });
        }

        private void CheckUserExists1(string email, string password)
        {

            CheckInternetConn();
            if (!isInternetAvailable)
            {
                return;
            }

            // Check if the user already exists based on the email address
            auth.FetchProvidersForEmailAsync(email).ContinueWith(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.LogError("Failed to fetch providers for email: " + task.Exception);
                    return;
                }

                // Get the list of sign-in methods associated with the email
                var signInMethods = task.Result;

                Debug.Log($"this is result {task.Result.ToArray().Length}");
                if (signInMethods.ToArray().Length > 0)
                {
                    Debug.Log("User already exists!");
                    // Proceed with log-in functionality
                    // You can call the Login() method here
                    Login(email, password);
                }
                else
                {
                    SignUp(email, password);
                    Debug.Log("User does not exist!");
                    // Proceed with sign-up functionality
                    // You can call the SignUp() method here
                }
            });
        }


        public void SignUp(string email, string password)
        {
            // Create a new user account with email and password
            auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.LogError("Sign-up failed: " + task.Exception);
                    return;
                }

                // Sign-up successful
                FirebaseUser user = task.Result;
                Debug.Log("Sign-up successful! User ID: " + user.UserId);

                Utils.userName = Utils.userName + Utils.GenerateUniqueID();

                Utils.userName = Regex.Replace(Utils.userName, @"\s", string.Empty);
                //Utils.userName = Regex.Replace(Utils.userName, @"\s+", string.Empty);

                UpdateUserProfileAsync(email, password, Utils.userName);
                // Proceed with desired functionality
            });
        }

        public void UpdateUserProfileAsync(string email, string password, string username)
        {
            if (auth.CurrentUser == null)
            {
                return;
            }

            user = auth.CurrentUser;
            if (user != null)
            {
                Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
                {
                    DisplayName = username,
                    PhotoUrl = user.PhotoUrl,
                };

                user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
                {
                    if (task.IsCanceled)
                    {
                        UnityEngine.Debug.Log("UpdateUserProfileAsync encountered an error: " + task.Exception);

                        return;
                    }
                    if (task.IsFaulted)
                    {
                        UnityEngine.Debug.Log("UpdateUserProfileAsync encountered an error: " + task.Exception);

                        return;
                    }
                    if (task.IsCompleted)
                    {

                    }

                    user = auth.CurrentUser;
                    Login(email, password);
                });
            }
        }


        public void Login(string email, string password)
        {
            // Log in with email and password
            auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.LogError("Login failed: " + task.Exception);
                    return;
                }

                user = task.Result;
                Debug.Log("Login successful! User ID: " + user.UserId);

                user = auth.CurrentUser;

                //UnityEngine.Debug.Log($"this is loged in666666 {user.DisplayName}");

                PlayerPrefs.SetString("UserId", Prashant.Utils.UserId);

                if (user != null)
                {
                    UnityEngine.Debug.Log($"this si user info {user.DisplayName} and {user.Email}");

                    Utils.userName = user.DisplayName;
                    Utils.mail = user.Email;
                    Utils.UserId= user.UserId;

                    PlayerPrefs.SetInt(Utils.LOGGED, Utils.EM);
                    PlayerPrefs.Save();

                    DatabaseManager.Instance.CheckFirebaseDependencies(user);
                    DatabaseManager.Instance.StartCoroutine(DatabaseManager.Instance.LoadUserData());

                    // after succesfully load or create date than load sceen by using event in Database manager;
                    //LoadScene();
                }

                // Proceed with desired functionality
            });

        }

        public void LoadScene()
        {
            StartCoroutine(LoadYourAsyncScene());
        }

        IEnumerator LoadYourAsyncScene()
        {
            // The Application loads the Scene in the background as the current Scene runs.
            // This is particularly good for creating loading screens.
            // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
            // a sceneBuildIndex of 1 as shown in Build Settings.

            //SceneLoadingPanel.SetActive(true);

            // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("BlackPlanet_Login");
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                int progress = (int)Mathf.Clamp01(asyncLoad.progress / .9f);
                // loadingText.text = progress * 100f + "%";
                Debug.Log($"{progress}");
                yield return null;
            }
        }


        public void CheckInternetConn()
        {
            InternetChecker.ICInstance.StartInternetCheck();

            //yield return new WaitForSeconds(1f);
        }

        public void MyListener(bool isInternetAvailable)
        {
            if (isInternetAvailable)
            {
                Debug.Log("Internet is Available");
                internetCanvas.gameObject.SetActive(false);
                this.isInternetAvailable = isInternetAvailable;
            }
            else if (!isInternetAvailable)
            {
                Debug.Log("Internet is not Available");
                internetCanvas.gameObject.SetActive(true);
                this.isInternetAvailable = isInternetAvailable;
            }
        }

        void OnDisable()
        {
            InternetChecker.MyInternet -= MyListener;
            DatabaseManager.OnLoadToLobby -= LoadScene;
            web3AuthScript.onWeb3authLogin -= ButtonClicked;
        }

    }
}