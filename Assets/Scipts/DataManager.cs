using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[System.Serializable]
public class DataManager
{
    public static bool isFirstTimeEnter = true;
    public static bool isOtherTimeEnter;
    public static string userName;
    public static int missionProgress;
    public static int lrdToken;
    public static int mldToken;
    public static int totalExp;
    public static int level;
    public static int dailyReward;
    public static int kValueForLavel;
    public static int healthBooster;
    public static int armorBooster;
    public static int ammo;
    public static int fuel;
    public static int dailyRewadClaimDay;
    public static string Reward_Claim_Datetime;
    public static long remainingSec;
    public static string timeAtFocusLost;
    public static bool isClaimed;
    public static bool isRewardClaimedSuccess;
    public static int currentSelectedCharacter;
    public static string purchasedData;
    public static string arenaData;
    public static string stroyData;
    public static string loadOutData;


    // static value for win lose
    public const int totalMission = 3;
    public const int expForTeamWin = 20;
    public const int expForTeamlost = -7;
    public const int expForPerKill = 1;
    public const int expForToper = 3;
    public const int lastArenaStats = 25;


    //sceneName;
    public const string sceneName = "";


    //public int lrdToken;
    //public int totalExp;
}
