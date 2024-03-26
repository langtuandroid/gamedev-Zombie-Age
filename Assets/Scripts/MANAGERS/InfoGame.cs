using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INFO GAME
[CreateAssetMenu(fileName = "INFO GAME 2", menuName = "NEW INFO GAME 2")]
public class InfoGame : ScriptableObject
{
    public TheEnumManager.PLATFROM ePlatform;

    public string NAME_GAME;

    [Header("_________________[NEW GAME]________________")]
    [Space(50)]
    public string strLinkMoreGame;
    public string strLinkLike;
    public string strFacebook;
    public Sprite sprLogoPNG;

    [Header("---------------EMAL----------------")]
    public string strAddress;
    public string strEmailReport;
    public string strEmailUser;



    [Space(10)]
    [Header("---------------ADMOB----------------")]
    public string strAdmob_IdPublisher;
    public string strAdmob_Id_Banner;
    public string strAdmob_Id_Interstitital;
    public string strAdmob_Id_VideoReward;
    public string strTestDevicesId;

    [Space(10)]
    [Header("---------------UNITY ADS----------------")]
    public string strUnityAds_Id;
}
