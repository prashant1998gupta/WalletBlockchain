using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Prashant
{
    static public class Utils
    {

        static public string REMEMBER_ME = "REMEMBER_ME";
        static public string LOGGED = "LOGGED"; //1: Email	2: Google	3:Facebook
        static public int NONE = 0;
        static public int EM = 1;
        static public int GG = 2;
        static public int FB = 3;
        static public int TW = 4;
        static public int AM = 5;
        static public int PH = 6;

        static public int CHANGE_PASS = 1;
        static public int RESET_PASS = 2;

        static public string userName;
        static public string mail;
        static public string UserId;
        static public string userWalletAccount;
        static public string password = "MetaSpace@12345!@#$%";


        static public string TUTORIALS = "TUTORIALS";

        public static string defaultPurchasedData = "{\"purchasedWeaponList\":[{\"weaponTypeName\":\"\",\"weapon_Name\":\"\",\"skinName\":\"\",\"weaponType\":0,\"weaponName\":0,\"gunCategory\":0,\"baseId\":0,\"weaponId\":0,\"skinId\":1011,\"discription\":\"\"},{\"weaponTypeName\":\"\",\"weapon_Name\":\"\",\"skinName\":\"\",\"weaponType\":0,\"weaponName\":0,\"gunCategory\":0,\"baseId\":0,\"weaponId\":0,\"skinId\":3011,\"discription\":\"\"},{\"weaponTypeName\":\"\",\"weapon_Name\":\"\",\"skinName\":\"\",\"weaponType\":0,\"weaponName\":0,\"gunCategory\":0,\"baseId\":0,\"weaponId\":0,\"skinId\":12011,\"discription\":\"\"}],\"purchasedCharacterList\":[{\"character_Name\":\"\",\"characterType\":1,\"characterName\":5,\"characterId\":5,\"discription\":\"\"},{\"character_Name\":\"\",\"characterType\":1,\"characterName\":8,\"characterId\":8,\"discription\":\"\"}]}";
        //static public string defaultarenaStatsData = "";
        //static public string defaultStroyStatsData = "";
        static public string defaultarenaStatsData = "{\"totalPlay\":0,\"totalWon\":0,\"totalLost\":0,\"totlaKills\":0,\"totalDei\":0,\"arenaMatchHistories\":[]}";
        static public string defaultStroyStatsData = "{\"totalStarWon\":0,\"totlaXpWon\":0,\"totalMisstionComplete\":0,\"health\":0,\"shield\":0,\"misstions\":[]}";
        static public string defaultLoadOutData = "{\"loadOutData\":[{\"weaponSkinId\":1011,\"weaponTypeId\":1010},{\"weaponSkinId\":3011,\"weaponTypeId\":3010},{\"weaponSkinId\":12011,\"weaponTypeId\":12010}]}";


        static public string privKey;
        static public string ed25519PrivKey;
        static public string error;
        static public string sessionId;


        static public string profileImage;
        static public string aggregateVerifier;
        static public string verifier;
        static public string verifierId;
        static public string typeOfLogin;
        static public string dappShare;
        static public string idToken;
        static public string oAuthIdToken;
        static public string oAuthAccessToken;


       /* public static string GenerateUniqueID()
        {
            long timestamp = System.DateTime.Now.Ticks;
            int randomNumber = Random.Range(0, 1000000);
            string uniqueID = $"{timestamp}-{randomNumber}";
            return uniqueID;
        }*/

        public static string GenerateUniqueID()
        {
            Guid guid = Guid.NewGuid();
            string uniqueID = guid.ToString();

            int startIndex = UnityEngine.Random.Range(0, uniqueID.Length - 8);  // Choose a random start index
            int endIndex = startIndex + UnityEngine.Random.Range(7, 9);  // Choose a random end index between 6 and 8

            string cutID = uniqueID.Substring(startIndex, endIndex - startIndex);
            //cutID = cutID.Replace(" ", string.Empty);   

            cutID = Regex.Replace(cutID, @"\s", string.Empty);

            char charToRemove = '-';
            cutID = cutID.Replace(charToRemove.ToString(), string.Empty);

            return cutID;
        }
    }
}





