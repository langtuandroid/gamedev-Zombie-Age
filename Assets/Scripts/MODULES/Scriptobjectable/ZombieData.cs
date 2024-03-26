using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Zombie Data", menuName = "Zombie Data")]

public class ZombieData : ScriptableObject
{
    public string strName;
    public TheEnumManager.ZOMBIE eZombie;
    public bool bIsFlying;
    public GameObject objPrefab;


    [Space(20)]
    public bool bIsBoss;
    public bool bCanShot;//Có khả năng ném đồ vật từ xa...

    public int iHpBase;
    public float fSpeed;
    public float fDamage;
    public float fReloadAttackTime;




    //GET HP

    private float fBienThien;
    private float fDistanceBaseHp;

    private const float BienThien_EASY = 2.0f, BienThien_NORMAL = 3.0f, BienThien_HARD = 5.0f;
    private const float fDistanceBaseHp_Easy = 1.0f, fDistanceBaseHp_Normal = 1.5f, fDistanceBaseHp_Nightmare = 2.0f;
    //Đồ thị thể hiện: https://www.desmos.com/calculator/7cyr95uxr7

    public int GetHp(int _currenLevel, int _currentWave)
    {
        /// CONG THUC - EASY: 5*wave+4*level+20
        /// CONG THUC - NORMAL: 7*wave+4*level+30
        /// CONG THUC - HARD: 9*wave+4*level+40


        if (bIsBoss)
        {
            switch (TheDataManager.Instance.THE_DATA_PLAYER.CURRENT_DIFFICUFT)
            {
                case TheEnumManager.DIFFICUFT.easy:
                    return iHpBase;
                case TheEnumManager.DIFFICUFT.normal:
                    return (int)(iHpBase * 1.5f);
                case TheEnumManager.DIFFICUFT.nightmare:
                    return iHpBase * 2;

            }
        }

        //Công thức tính HP, websi vẽ đồ thị: www.desmos.com
        switch (TheDataManager.Instance.THE_DATA_PLAYER.CURRENT_DIFFICUFT)
        {
            case TheEnumManager.DIFFICUFT.easy:
                fBienThien = BienThien_EASY;
                fDistanceBaseHp = fDistanceBaseHp_Easy;
                break;
            case TheEnumManager.DIFFICUFT.normal:
                fBienThien = BienThien_NORMAL;
                fDistanceBaseHp = fDistanceBaseHp_Normal;
                break;
            case TheEnumManager.DIFFICUFT.nightmare:
                fBienThien = BienThien_HARD;
                fDistanceBaseHp = fDistanceBaseHp_Nightmare;
                break;

        }

        return (int)(fBienThien * _currentWave + 8 * _currenLevel + iHpBase * fDistanceBaseHp);
    }


    public float GetSpeed()
    {
        float _tempSpeed = fSpeed;
        if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.KIND_OF_UPGRADE.zombie_all_speed10).bEQUIPED)
        {
            _tempSpeed = _tempSpeed * 0.9f;
        }

        if (bIsFlying)
        {
            if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.KIND_OF_UPGRADE.zombie_airforce_speed20).bEQUIPED)
            {
                _tempSpeed = _tempSpeed * 0.8f;
            }
        }
        else
        {
            if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.KIND_OF_UPGRADE.zombie_infantry_speed20).bEQUIPED)
            {
                _tempSpeed = _tempSpeed * 0.8f;
            }
        }


        return _tempSpeed;
    }

    public float GetDamage()
    {
        if (TheUpgradeManager.Instance.GetUpgrade(TheEnumManager.KIND_OF_UPGRADE.zombie_damage20).bEQUIPED)
        {
            return fDamage * 0.8f;
        }
        return fDamage;
    }
}
