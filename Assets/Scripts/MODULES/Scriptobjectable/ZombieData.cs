using MANAGERS;
using UnityEngine;

namespace MODULES.Scriptobjectable
{
    [CreateAssetMenu(fileName = "Zombie Data", menuName = "Zombie Data")]

    public class ZombieData : ScriptableObject
    {
        public string strName;
        public EnumController.ZOMBIE eZombie;
        public bool bIsFlying;
        public GameObject objPrefab;


        [Space(20)]
        public bool bIsBoss;
        public bool bCanShot;//Có khả năng ném đồ vật từ xa...

        public int iHpBase;
        public float fSpeed;
        public float fDamage;
        public float fReloadAttackTime;

        private float fBienThien;
        private float fDistanceBaseHp;

        private const float BienThien_EASY = 2.0f, BienThien_NORMAL = 3.0f, BienThien_HARD = 5.0f;
        private const float fDistanceBaseHp_Easy = 1.0f, fDistanceBaseHp_Normal = 1.5f, fDistanceBaseHp_Nightmare = 2.0f;

        public int GetHp(int _currenLevel, int _currentWave)
        {
            if (bIsBoss)
            {
                switch (DataController.Instance.playerData.Difficuft)
                {
                    case EnumController.DIFFICUFT.easy:
                        return iHpBase;
                    case EnumController.DIFFICUFT.normal:
                        return (int)(iHpBase * 1.5f);
                    case EnumController.DIFFICUFT.nightmare:
                        return iHpBase * 2;

                }
            }
            
            switch (DataController.Instance.playerData.Difficuft)
            {
                case EnumController.DIFFICUFT.easy:
                    fBienThien = BienThien_EASY;
                    fDistanceBaseHp = fDistanceBaseHp_Easy;
                    break;
                case EnumController.DIFFICUFT.normal:
                    fBienThien = BienThien_NORMAL;
                    fDistanceBaseHp = fDistanceBaseHp_Normal;
                    break;
                case EnumController.DIFFICUFT.nightmare:
                    fBienThien = BienThien_HARD;
                    fDistanceBaseHp = fDistanceBaseHp_Nightmare;
                    break;

            }

            return (int)(fBienThien * _currentWave + 8 * _currenLevel + iHpBase * fDistanceBaseHp);
        }


        public float GetSpeed()
        {
            float _tempSpeed = fSpeed;
            if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.zombie_all_speed10).bEQUIPED)
            {
                _tempSpeed = _tempSpeed * 0.9f;
            }

            if (bIsFlying)
            {
                if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.zombie_airforce_speed20).bEQUIPED)
                {
                    _tempSpeed = _tempSpeed * 0.8f;
                }
            }
            else
            {
                if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.zombie_infantry_speed20).bEQUIPED)
                {
                    _tempSpeed = _tempSpeed * 0.8f;
                }
            }


            return _tempSpeed;
        }

        public float GetDamage()
        {
            if (UpgradeController.Instance.GetUpgrade(EnumController.UpgradeType.zombie_damage20).bEQUIPED)
            {
                return fDamage * 0.8f;
            }
            return fDamage;
        }
    }
}
