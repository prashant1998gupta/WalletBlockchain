using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.Auth;
using System;



public class DatabaseManager : GenericSingleton<DatabaseManager>
{

    #region FirebaseVariables

    public FirebaseDatabase database;
    //FirebaseAuth auth;  
    FirebaseUser user;

    #endregion

    //public Action onFirstTimeDataSave;
    public static Action OnLoadToLobby;
   


    private void Start()
    {
        //auth = FirebaseAuth.DefaultInstance;
        Debug.Log("Initialized Successfully");
        Debug.Log($"Awake and DatabaseManager.Instance.OnLoadToLobby {OnLoadToLobby.Method}");
    }


    public void CheckFirebaseDependencies(FirebaseUser user)
    {

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {

            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                {

                    InitializeFirebase(user);

                }
                else
                    AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }

            if (task.Exception != null)
            {
                Debug.LogError($"Failed to initialize firebase with {task.Exception}");
                return;
            }




            Debug.Log("Initialized Successfully");

        });
    }


    private void InitializeFirebase(FirebaseUser user)
    {
        this.user = user;
    }

    public void AddToInformation(string str)
    { 
       // infoText.text += "\n" + str;

    }



    public IEnumerator LoadUserData()
    {
        var dbTask = Prashant.AuthenticationManager.databaseReference.Child("users").Child(Prashant.AuthenticationManager.user.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);
        if (dbTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
        }
        else if (dbTask.Result.Value == null)
        {
            //Resetting Data
            Debug.Log("ResetDataTriggered");
            DataManager.userName = Prashant.Utils.userName;
            DataManager.kValueForLavel = 100;
            DataManager.lrdToken = 1000;
            DataManager.totalExp = 50;
            DataManager.isClaimed = true;
            DataManager.isRewardClaimedSuccess = true;
            DataManager.isFirstTimeEnter = true;
            DataManager.isOtherTimeEnter = false;
            DataManager.currentSelectedCharacter = 5;
            DataManager.purchasedData = Prashant.Utils.defaultPurchasedData;
            DataManager.arenaData = Prashant.Utils.defaultarenaStatsData;
            DataManager.stroyData = Prashant.Utils.defaultStroyStatsData;
            DataManager.loadOutData = Prashant.Utils.defaultLoadOutData;

            CreateUserSaveProfile();
            SaveandUpdateUserData();
            Debug.Log("LoadUserDataCoroutine: Null");
        }
        else
        {
            // Data has been retrived
            DataSnapshot snapshot = dbTask.Result;
            DataManager.isFirstTimeEnter = bool.Parse(snapshot.Child("isFirstTimeEnter").Value.ToString());
            DataManager.isOtherTimeEnter = bool.Parse(snapshot.Child("isOtherTimeEnter").Value.ToString());
            DataManager.userName = (string)snapshot.Child("userName").Value;
            DataManager.missionProgress = int.Parse(snapshot.Child("missionProgress").Value.ToString());
            DataManager.totalExp = int.Parse(snapshot.Child("totalExp").Value.ToString());
            DataManager.lrdToken = int.Parse(snapshot.Child("lrdToken").Value.ToString());
            DataManager.mldToken = int.Parse(snapshot.Child("mldToken").Value.ToString());
            DataManager.level = int.Parse(snapshot.Child("level").Value.ToString());
            DataManager.kValueForLavel = int.Parse(snapshot.Child("kValueForLavel").Value.ToString());
            DataManager.healthBooster = int.Parse(snapshot.Child("healthBooster").Value.ToString());
            DataManager.fuel = int.Parse(snapshot.Child("fuel").Value.ToString());
            DataManager.armorBooster = int.Parse(snapshot.Child("armorBooster").Value.ToString());
            DataManager.ammo = int.Parse(snapshot.Child("ammo").Value.ToString());
            DataManager.dailyRewadClaimDay = int.Parse(snapshot.Child("dailyRewadClaimDay").Value.ToString());
            DataManager.remainingSec = long.Parse(snapshot.Child("remainingSec").Value.ToString());
            DataManager.Reward_Claim_Datetime = (string)snapshot.Child("Reward_Claim_Datetime").Value;
            DataManager.timeAtFocusLost = (string)snapshot.Child("timeAtFocusLost").Value;
            DataManager.isClaimed = bool.Parse(snapshot.Child("isClaimed").Value.ToString());
            DataManager.isRewardClaimedSuccess = bool.Parse(snapshot.Child("isRewardClaimedSuccess").Value.ToString());
            DataManager.currentSelectedCharacter = int.Parse(snapshot.Child("currentSelectedCharacter").Value.ToString());
            DataManager.purchasedData = (string)snapshot.Child("purchasedData").Value;
            DataManager.arenaData = (string)snapshot.Child("arenaData").Value;
            DataManager.stroyData = (string)snapshot.Child("stroyData").Value;
            DataManager.loadOutData = (string)snapshot.Child("loadOutData").Value;


        }
        Debug.Log("userName " + DataManager.userName);
        Debug.Log("missionProgress " + DataManager.missionProgress);
        Debug.Log("totalExp " + DataManager.totalExp);
        Debug.Log("lrdToken " + DataManager.lrdToken);
        Debug.Log("level " + DataManager.level);
        Debug.Log("kValueForLevel " + DataManager.kValueForLavel);



        // this is call because we call in same scene 

        /* if (onFirstTimeDataSave != null)
         {
             onFirstTimeDataSave.Invoke();
             Debug.Log("triggered");
         }*/
        Debug.Log($"Awake and DatabaseManager.Instance.OnLoadToLobby {OnLoadToLobby}");
        if (OnLoadToLobby != null)
        {
            OnLoadToLobby.Invoke();
            Debug.Log("triggered");
        }
        else
        {
            Debug.Log("triggered is null");
        }
    }

    public void CreateUserSaveProfile()
    {
        StartCoroutine(UpdateUsernameAuth(Prashant.AuthenticationManager.user.DisplayName));
        StartCoroutine(UpdateUsernameDatabase(Prashant.AuthenticationManager.user.DisplayName));

    }
    private IEnumerator UpdateUsernameAuth(string _username)
    {
        // Create the user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        // Call the firebase auth update user profile function passing the profile with the username]
        //user = auth.CurrentUser;
        var profileTask = user.UpdateUserProfileAsync(profile);

        //Wait until the task complete
        yield return new WaitUntil(predicate: () => profileTask.IsCompleted);

        if (profileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");
        }
        else
        {
            // Auth username is now updated
        }
    }

    public void SaveandUpdateUserData()
    {
        StartCoroutine(UpdateSaveData());
    }

    public IEnumerator UpdateSaveData()
    {
        string json = SaveData.SaveNow(this);
      
        var dbTask = Prashant.AuthenticationManager.databaseReference.Child("users").Child(Prashant.AuthenticationManager.user.UserId).SetRawJsonValueAsync(json);
        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);
        if (dbTask.Exception != null)
        {
            Debug.Log(message: $"Failed to register task with{ dbTask.Exception} 11");
        }
        else
        {
           // Debug.Log(message: $"Failed to register task with{ dbTask.Exception} 22");
        }


    }

    private IEnumerator UpdateUsernameDatabase(string _userName)
    {
        var dbTask = Prashant.AuthenticationManager.databaseReference.Child("users").Child(Prashant.AuthenticationManager.user.UserId).Child("username").SetValueAsync(_userName);

        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);
        if (dbTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
        }
        else
        {
            // Database username is now updated
        }
    }

   
}

