using MANAGERS;
using MODULES.Soldiers;
using SCREENS;
using UnityEngine;

namespace MODULES.Scriptobjectable
{
    [CreateAssetMenu(fileName = "Reward data")]
    public class RewardData : ScriptableObject
    {
        public TheEnumManager.REWARD eReward;
        public Sprite sprIcon;
        public int iValue;
        public string strContent;

        //Công thức tính gem thưởng khi kết thúc game
        public int GetVictoryGem()
        {
            float _factorFloat = Soldier.Instance.DEFENSE_MANAGER.GetFactorDefense();
            int _gem = (int)(_factorFloat * iValue);
            if (_gem <= 10) _gem = 10;
            return _gem;
        }


        public void GetReward()
        {
            //vitory gem: Dựa vào khả năng chỉ số defense còn lại của soldier.
            if (eReward == TheEnumManager.REWARD.victory_gem_easy
                || eReward == TheEnumManager.REWARD.victory_gem_normal
                || eReward == TheEnumManager.REWARD.victory_gem_nightmate)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound

                int _gem = GetVictoryGem();

                if (VictoryReward.Instance.bX2Gem)
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += 2 * _gem;
                else
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += _gem;


                TheDataManager.Instance.SaveDataPlayer();//save)
                return;
            }


            switch (eReward)
            {
                case TheEnumManager.REWARD.ads_free_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;



                case TheEnumManager.REWARD.ads_gift_pack_1:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.grenade).iCurrentValue += 1;
                    break;
                case TheEnumManager.REWARD.ads_gift_pack_2:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.poison).iCurrentValue += 1;
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.freeze).iCurrentValue += 1;
                    break;
                case TheEnumManager.REWARD.ads_gift_pack_3:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.big_bomb).iCurrentValue += 1;
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.poison).iCurrentValue += 1;
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.grenade).iCurrentValue += 1;
                    break;
                case TheEnumManager.REWARD.ads_gift_pack_4:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.big_bomb).iCurrentValue += 1;
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.poison).iCurrentValue += 1;
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.freeze).iCurrentValue += 1;
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.grenade).iCurrentValue += 1;
                    break;


                #region CHECK-IN
                case TheEnumManager.REWARD.check_in_d1_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d2_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d3_grenade:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.grenade).iCurrentValue += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d4_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d5_freeze:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.freeze ).iCurrentValue += iValue;

                    break;
                case TheEnumManager.REWARD.check_in_d6_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d7_grenade:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.grenade).iCurrentValue += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d8_bigbomb:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.big_bomb ).iCurrentValue += iValue;

                    break;
                case TheEnumManager.REWARD.check_in_d9_freeze:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.freeze ).iCurrentValue += iValue;

                    break;
                case TheEnumManager.REWARD.check_in_d10_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d11_bigbomb:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.big_bomb).iCurrentValue += iValue;

                    break;
                case TheEnumManager.REWARD.check_in_d12_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d13_grenade:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.grenade).iCurrentValue += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d14_freeze:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.freeze ).iCurrentValue += iValue;

                    break;
                case TheEnumManager.REWARD.check_in_d15_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d16_poison:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.poison).iCurrentValue += iValue;

                    break;
                case TheEnumManager.REWARD.check_in_d17_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d18_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d19_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d20_grenade:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.grenade).iCurrentValue += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d21_freeze:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.freeze ).iCurrentValue += iValue;

                    break;
                case TheEnumManager.REWARD.check_in_d22_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d23_grenade:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.grenade).iCurrentValue += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d24_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d25_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d26_freeze:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.freeze ).iCurrentValue += iValue;

                    break;
                case TheEnumManager.REWARD.check_in_d27_poison:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.poison).iCurrentValue += iValue;

                    break;
                case TheEnumManager.REWARD.check_in_d28_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d29_grenade:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.grenade).iCurrentValue += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d30_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d31_bigbomb:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.big_bomb ).iCurrentValue += iValue;

                    break;
                case TheEnumManager.REWARD.check_in_d32_grenade:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.grenade).iCurrentValue += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d33_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d34_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d35_poison:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.GetSupport(TheEnumManager.SUPPORT.poison).iCurrentValue += iValue;
                    break;
                case TheEnumManager.REWARD.check_in_d36_gem:
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem += iValue;
                    break;
                #endregion


            }

            TheDataManager.Instance.SaveDataPlayer();//save)
        }




    }
}
