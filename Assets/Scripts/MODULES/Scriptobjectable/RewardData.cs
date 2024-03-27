using MANAGERS;
using MODULES.Soldiers;
using SCREENS;
using UnityEngine;

namespace MODULES.Scriptobjectable
{
    [CreateAssetMenu(fileName = "Reward data")]
    public class RewardData : ScriptableObject
    {
        public EnumController.REWARD eReward;
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
            if (eReward == EnumController.REWARD.victory_gem_easy
                || eReward == EnumController.REWARD.victory_gem_normal
                || eReward == EnumController.REWARD.victory_gem_nightmate)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound

                int _gem = GetVictoryGem();

                if (VictoryReward.Instance.bX2Gem)
                    DataController.Instance.playerData.Gem += 2 * _gem;
                else
                    DataController.Instance.playerData.Gem += _gem;


                DataController.Instance.SaveData();//save)
                return;
            }


            switch (eReward)
            {
                case EnumController.REWARD.ads_free_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;



                case EnumController.REWARD.ads_gift_pack_1:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.grenade).iCurrentValue += 1;
                    break;
                case EnumController.REWARD.ads_gift_pack_2:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.poison).iCurrentValue += 1;
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.freeze).iCurrentValue += 1;
                    break;
                case EnumController.REWARD.ads_gift_pack_3:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.big_bomb).iCurrentValue += 1;
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.poison).iCurrentValue += 1;
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.grenade).iCurrentValue += 1;
                    break;
                case EnumController.REWARD.ads_gift_pack_4:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.big_bomb).iCurrentValue += 1;
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.poison).iCurrentValue += 1;
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.freeze).iCurrentValue += 1;
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.grenade).iCurrentValue += 1;
                    break;


                #region CHECK-IN
                case EnumController.REWARD.check_in_d1_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d2_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d3_grenade:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.grenade).iCurrentValue += iValue;
                    break;
                case EnumController.REWARD.check_in_d4_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d5_freeze:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.freeze ).iCurrentValue += iValue;

                    break;
                case EnumController.REWARD.check_in_d6_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d7_grenade:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.grenade).iCurrentValue += iValue;
                    break;
                case EnumController.REWARD.check_in_d8_bigbomb:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.big_bomb ).iCurrentValue += iValue;

                    break;
                case EnumController.REWARD.check_in_d9_freeze:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.freeze ).iCurrentValue += iValue;

                    break;
                case EnumController.REWARD.check_in_d10_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d11_bigbomb:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.big_bomb).iCurrentValue += iValue;

                    break;
                case EnumController.REWARD.check_in_d12_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d13_grenade:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.grenade).iCurrentValue += iValue;
                    break;
                case EnumController.REWARD.check_in_d14_freeze:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.freeze ).iCurrentValue += iValue;

                    break;
                case EnumController.REWARD.check_in_d15_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d16_poison:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.poison).iCurrentValue += iValue;

                    break;
                case EnumController.REWARD.check_in_d17_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d18_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d19_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d20_grenade:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.grenade).iCurrentValue += iValue;
                    break;
                case EnumController.REWARD.check_in_d21_freeze:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.freeze ).iCurrentValue += iValue;

                    break;
                case EnumController.REWARD.check_in_d22_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d23_grenade:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.grenade).iCurrentValue += iValue;
                    break;
                case EnumController.REWARD.check_in_d24_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d25_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d26_freeze:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.freeze ).iCurrentValue += iValue;

                    break;
                case EnumController.REWARD.check_in_d27_poison:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.poison).iCurrentValue += iValue;

                    break;
                case EnumController.REWARD.check_in_d28_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d29_grenade:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.grenade).iCurrentValue += iValue;
                    break;
                case EnumController.REWARD.check_in_d30_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d31_bigbomb:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.big_bomb ).iCurrentValue += iValue;

                    break;
                case EnumController.REWARD.check_in_d32_grenade:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.grenade).iCurrentValue += iValue;
                    break;
                case EnumController.REWARD.check_in_d33_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d34_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                case EnumController.REWARD.check_in_d35_poison:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.TakeSupport(EnumController.SUPPORT.poison).iCurrentValue += iValue;
                    break;
                case EnumController.REWARD.check_in_d36_gem:
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    DataController.Instance.playerData.Gem += iValue;
                    break;
                #endregion


            }

            DataController.Instance.SaveData();//save)
        }




    }
}
