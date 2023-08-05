using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using Firebase.Database;
using Prashant;

public class Web3AuthSample : MonoBehaviour
{
    Web3Auth web3Auth;

    [SerializeField]
    TMP_InputField emailAddressField;

    [SerializeField]
    TextMeshProUGUI errorText;

    /* [SerializeField]
     Dropdown verifierDropdown;*/

    [SerializeField]
    Button loginWithEmailButton;
    [SerializeField]
    Button loginWithGoogleButton;
    [SerializeField]
    Button loginWithFacebookButton;

    public System.Action onWeb3authLogin;
    public GameObject loading;
    public Web3AuthRes web3AuthUserInfo;

    GameObject internetCanvas;
    public bool isInternetAvailable = true;

    AuthenticationManager authenticationManager;


    private void Awake()
    {
        InternetChecker.MyInternet += MyListener;
        internetCanvas = FindObjectOfType<DontDestroyManager>().internetCanvas;
        authenticationManager = FindObjectOfType<AuthenticationManager>();
        web3Auth = FindObjectOfType<Web3Auth>();
    }

    void Start()
    {
        //AudioManager.Instance.PlayMusic("music_mountainous_journey");

        emailAddressField.onValueChanged.AddListener(OnEmailValueChanges);
        errorText.text = string.Empty;
        var loginConfigItem = new LoginConfigItem()
        {
            verifier = "your_verifierid_from_web3auth_dashboard",
            typeOfLogin = TypeOfLogin.GOOGLE,
            clientId = "your_clientid_from_google_or_etc"
        };

        //web3Auth = GetComponent<Web3Auth>();
        web3Auth.setOptions(new Web3AuthOptions()
        {
            whiteLabel = new WhiteLabelData()
            {
                name = "Web3Auth Sample App",
                logoLight = null,
                logoDark = null,
                defaultLanguage = "en",
                dark = true,
                theme = new Dictionary<string, string>
                {
                    { "primary", "#123456" }
                }
            }
            /* // If using your own custom verifier, uncomment this code. 
             ,
             loginConfig = new Dictionary<string, LoginConfigItem>
             {
                 {"CUSTOM_VERIFIER", loginConfigItem}
             }*/

        });
        web3Auth.onLogin += onLogin;
        web3Auth.onLogout += onLogout;

        /* emailAddressField.gameObject.SetActive(false);
         logoutButton.gameObject.SetActive(false);*/

        loginWithEmailButton.onClick.AddListener(LoginWithEmail);
        loginWithGoogleButton.onClick.AddListener(LoginWithGoogle);
        loginWithFacebookButton.onClick.AddListener(LoginWithFacebook);


        // verifierDropdown.AddOptions(verifierList.Select(x => x.name).ToList());
        //verifierDropdown.onValueChanged.AddListener(onVerifierDropDownChange);
    }

   

    private void OnEmailValueChanges(string arg0)
    {
        errorText.text = string.Empty;
    }

    private void onLogin(Web3AuthResponse response)
    {
        loading.SetActive(true);
        var web3AuthResponse = JsonConvert.SerializeObject(response, Formatting.Indented);
        Debug.Log(web3AuthResponse);

        web3AuthUserInfo = JsonConvert.DeserializeObject<Web3AuthRes>(web3AuthResponse);


        Prashant.Utils.privKey = web3AuthUserInfo.privKey;
        Prashant.Utils.ed25519PrivKey = web3AuthUserInfo.ed25519PrivKey;
        Prashant.Utils.error = web3AuthUserInfo.error;
        Prashant.Utils.sessionId = web3AuthUserInfo.sessionId;

        Prashant.Utils.mail = web3AuthUserInfo.userInfo.email;
        Prashant.Utils.userName = web3AuthUserInfo.userInfo.name;
        Prashant.Utils.profileImage = web3AuthUserInfo.userInfo.profileImage;
        Prashant.Utils.aggregateVerifier = web3AuthUserInfo.userInfo.aggregateVerifier;
        Prashant.Utils.verifier = web3AuthUserInfo.userInfo.verifier;
        Prashant.Utils.verifierId = web3AuthUserInfo.userInfo.verifierId;
        Prashant.Utils.typeOfLogin = web3AuthUserInfo.userInfo.typeOfLogin;
        Prashant.Utils.dappShare = web3AuthUserInfo.userInfo.dappShare;
        Prashant.Utils.idToken = web3AuthUserInfo.userInfo.idToken;
        Prashant.Utils.oAuthIdToken = web3AuthUserInfo.userInfo.oAuthIdToken;
        Prashant.Utils.oAuthAccessToken = web3AuthUserInfo.userInfo.oAuthAccessToken;


        PlayerPrefs.SetString("privKey", Prashant.Utils.privKey);
        PlayerPrefs.SetString("ed25519PrivKey", Prashant.Utils.ed25519PrivKey);
        PlayerPrefs.SetString("error", Prashant.Utils.error);
        PlayerPrefs.SetString("sessionId", Prashant.Utils.sessionId);

        // PlayerPrefs.SetInt(Utils.LOGGED, Utils.EM);
        PlayerPrefs.SetString("email", Prashant.Utils.mail);
        PlayerPrefs.SetString("userName", Prashant.Utils.userName);
        PlayerPrefs.SetString("profileImage", Prashant.Utils.profileImage);
        PlayerPrefs.SetString("aggregateVerifier", Prashant.Utils.aggregateVerifier);
        PlayerPrefs.SetString("verifier", Prashant.Utils.verifier);
        PlayerPrefs.SetString("verifierId", Prashant.Utils.verifierId);
        PlayerPrefs.SetString("typeOfLogin", Prashant.Utils.typeOfLogin);
        PlayerPrefs.SetString("dappShare", Prashant.Utils.dappShare);
        PlayerPrefs.SetString("idToken", Prashant.Utils.idToken);
        PlayerPrefs.SetString("oAuthIdToken", Prashant.Utils.oAuthIdToken);
        PlayerPrefs.SetString("oAuthAccessToken", Prashant.Utils.oAuthAccessToken);
        PlayerPrefs.Save();


        if (onWeb3authLogin != null)
        {
            onWeb3authLogin.Invoke();
            Debug.Log("triggered");
        }

        errorText.text = "";
    }

    private void onLogout()
    {


        // loginResponseText.text = "";
    }




    LoginVerifier loginVerifier;

    public void LoginWithEmail()
    {
        AudioManager.Instance.PlaySoundEffect("timeTick");

        CheckInternetConn();
        if (!isInternetAvailable)
        {
            return;
        }


        if (PlayerPrefs.GetInt(Prashant.Utils.LOGGED) == 1)
        {
            loading.SetActive(true);
            authenticationManager.Login(PlayerPrefs.GetString("email"), Prashant.Utils.password);
        }
        else
        {
            string mail = emailAddressField.text;


            if (mail.Equals(""))
            {
                errorText.text = "please enter email";
                return;
            }

            if (EmailValidation.IsValidEmail(mail))
            {
                loginVerifier = new LoginVerifier("Hosted Email Passwordless", Provider.EMAIL_PASSWORDLESS);
                login();
                errorText.text = "";
            }
            else
            {
                errorText.text = "please enter valid email";
            }
        }
    }

    public void LoginWithGoogle()
    {
        AudioManager.Instance.PlaySoundEffect("timeTick");

        CheckInternetConn();
        if (!isInternetAvailable)
        {
            return;
        }

        if (PlayerPrefs.GetInt(Prashant.Utils.LOGGED) == 1)
        {
            loading.SetActive(true);
            authenticationManager.Login(PlayerPrefs.GetString("email"), Prashant.Utils.password);
        }
        else
        {
            loginVerifier = new LoginVerifier("Google", Provider.GOOGLE);
            login();
        }

       
    }

    public void LoginWithFacebook()
    {
        AudioManager.Instance.PlaySoundEffect("timeTick");

        CheckInternetConn();
        if (!isInternetAvailable)
        {
            return;
        }

        if (PlayerPrefs.GetInt(Prashant.Utils.LOGGED) == 1)
        {
            loading.SetActive(true);
            authenticationManager.Login(PlayerPrefs.GetString("email"), Prashant.Utils.password);
        }
        else
        {
            loginVerifier = new LoginVerifier("Facebook", Provider.FACEBOOK);
            login();
        }

        
    }

    private void login()
    {
        var selectedProvider = loginVerifier.loginProvider;

        var options = new LoginParams()
        {
            loginProvider = selectedProvider
        };

        if (selectedProvider == Provider.EMAIL_PASSWORDLESS)
        {
            options.extraLoginOptions = new ExtraLoginOptions()
            {
                login_hint = emailAddressField.text
            };
        }

        web3Auth.login(options);
    }

    private void logout()
    {
        web3Auth.logout();
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
        web3Auth.onLogin -= onLogin;
        web3Auth.onLogout -= onLogout;
    }




    [System.Serializable]
    public class Web3AuthRes
    {
        public string privKey;
        public string ed25519PrivKey;
        public UserInfo userInfo;
        public string error;
        public string sessionId;
    }

    [System.Serializable]
    public class UserInfo
    {
        public string email;
        public string name;
        public string profileImage;
        public string aggregateVerifier;
        public string verifier;
        public string verifierId;
        public string typeOfLogin;
        public string dappShare;
        public string idToken;
        public string oAuthIdToken;
        public string oAuthAccessToken;
    }

}
