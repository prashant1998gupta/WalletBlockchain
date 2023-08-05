using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Data 
{
    public bool isFirstTimeEnter;
    public bool isOtherTimeEnter;
    public string userName;
    public int missionProgress;
    public int lrdToken;
    public int mldToken;
    public int totalExp;
    public int level;
    public int kValueForLavel;
    public int healthBooster;
    public int armorBooster;
    public int ammo;
    public int fuel;
    public int dailyRewadClaimDay;
    public string Reward_Claim_Datetime;
    public long remainingSec;
    public string timeAtFocusLost;
    public bool isClaimed;
    public bool isRewardClaimedSuccess;
    public int currentSelectedCharacter;
    public string purchasedData;
    public string arenaData;
    public string stroyData;
    public string loadOutData;
   

    public Data(DatabaseManager databaseManager)
    {
        /*userName = databaseManager.userName;
        missionProgress = databaseManager.missionProgress;
        //DataManager.lrdToken = databaseManager.lrdToken;
        //DataManager.totalExp = databaseManager.totalExp;
        lrdToken = databaseManager.lrdToken;
        totalExp = databaseManager.totalExp;*/
        isFirstTimeEnter = DataManager.isFirstTimeEnter;
        isOtherTimeEnter = DataManager.isOtherTimeEnter;
        userName = DataManager.userName;
        missionProgress = DataManager.missionProgress;
        lrdToken = DataManager.lrdToken;
        mldToken = DataManager.mldToken;
        totalExp = DataManager.totalExp;
        level = DataManager.level;
        kValueForLavel = DataManager.kValueForLavel;
        armorBooster = DataManager.armorBooster;
        healthBooster = DataManager.healthBooster;
        ammo = DataManager.ammo;
        fuel = DataManager.fuel;
        dailyRewadClaimDay = DataManager.dailyRewadClaimDay;
        Reward_Claim_Datetime = DataManager.Reward_Claim_Datetime;
        remainingSec = DataManager.remainingSec;
        timeAtFocusLost = DataManager.timeAtFocusLost;
        isClaimed = DataManager.isClaimed;
        isRewardClaimedSuccess = DataManager.isRewardClaimedSuccess;
        currentSelectedCharacter = DataManager.currentSelectedCharacter;
        purchasedData = DataManager.purchasedData;
        arenaData = DataManager.arenaData;
        stroyData = DataManager.stroyData;
        loadOutData = DataManager.loadOutData;
    }
}
