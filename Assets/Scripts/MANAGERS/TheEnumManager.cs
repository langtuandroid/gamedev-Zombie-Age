namespace MANAGERS
{
    public class TheEnumManager
    {

        public enum PLATFROM
        {
            android,
            ios,
            facebook,
            info_default,
        }


        #region DIFFICUFT
        public enum DIFFICUFT
        {
            easy,
            normal,
            nightmare,
        }
        #endregion

        #region SOLDIER STATUS
        public enum SOLDIER_STATUS
        {
            idie,
            shooting,
            walk,
        }
        #endregion


        #region ZOMBIE 
        public enum ZOMBIE
        {
            zombie_1,
            zombie_2,
            zombie_3,
            zombie_4,
            zombie_5,
            zombie_6,
            dog,
            spdier_blue,
            spider_yellow,
            ruoi,
            muoi,
            boss_mug,
            boss_soldier,
            boss_frog,
        }
        public enum ZOMBIE_STATUS
        {
            moving,
            die,
            attack,
        }

        public enum KIND_OF_ITEM_FOR_ZOMBIE
        {
            not,
            hat,
            weapon,
            shield,

        }
        public enum HAT_OF_ZOMBIE
        {
            NO_HAT,
            Hat_Ong,
            Hat_MuBaoHiem,
            Hat_MuThep,
            Hat_MuPhot,
            Hat_MatNaGo,
            Hat_Viking,

        }
        public enum WEAPON_OF_ZOMBIE
        {
            NO_WEAPON,
            weapon_GayGo,
            Weapon_ChuyThep,
            Weapon_ChuyGo,
            Weapon_Cole,

        }
        public enum SHIELD_OF_ZOMBIE
        {
            NO_SHIELD,
            shield_wood,
            shield_steel,

        }



        #endregion


        #region POOL
        public enum POOLING_OBJECT
        {
            bullet,
            exploison_bullet,
            bullet_of_shotgun,
            bullet_of_bazoka,
            blood_of_zombie,
            zombie_blood_exploison,
            bullet_fire,
            smock_of_bazoka,
            main_exploison,

            //-----------support
            support_grenade,
            support_freeze,
            support_bigbom,
            support_poison,

            //----------------
            effect_freeze,

            //BULLET OF ZOMBIE
            bullet_of_zombie,

            main_exploison_small,

            fire_of_enemies,

            //remove item
            remove_item,
            text_blood,

            freeze_range,

            bullet_bezier_of_zombie,
        }

        #endregion


        #region LEVEL of GUN
        public enum WEAPON
        {
            colt_python,
            shotgun,
            shotgun2barrel,
            fn_p90,
            ak47,
            m16,
            m134,
            firegun,
            bazoka,
            stun_gun,
        }
        public enum ITEM_LEVEL
        {
            level_1,
            level_2,
            level_3,
            level_4,
            level_5,
            level_6,
            level_7,

        }
        #endregion


        #region IAP
        public enum KIND_OF_IAP
        {
            pack_1,
            pack_2,
            pack_3,
            pack_4,
            pack_5,
            pack_6,
            watch_ad_to_free_gem,
        }
        #endregion


        #region DEFENSE
        public enum DEFENSE
        {
            home,
            metal,
            thorn, //chông gai
        }
        #endregion

        #region SKILL
        public enum SUPPORT
        {
            grenade, // nổ lmà mất máu zombie 1 vùng.
            freeze, // đóng băng or làm chậm di chuyển
            poison, // mất máu từ từ
            big_bomb, // bomb hat nhan
        }
        #endregion

        #region UPGRADE

        public enum KIND_OF_UPGRADE
        {
            freeze_time50,//Tăng thời gian đóng băng lênn 50%
            freeze_range25,//Tăng phạm vi sát thương lên 25%

            posion_damage50,//Tăng sát thương lên 50%
            posion_range25,//Tăng phạm vi sát thương lên 25%
            poision_time25,//Tăng thời gian nhiễm độc len 25%

            grenade_damage25,//Tăng sát thương của grenade lên 25%
            grenade_range25,//Tăng phạm vi của grenade lênn 25%


            bigbom_damage25,//Tăng sát thương của big bom lên 25%


            weapon_damage5,//Tăng sát thương của all weapon lên 5%
            weapon_damage_10,//Tăng sát thương của all weapon lên 10%

            home_defense25,//Tăng sức mạnh phòng thủ của home lên 25%
            home_defense15,//Tăng sức mạng phòng thủ của home lên 15%

            metal_defense25,//Tang sức mạng phòng thủ của metal lên 25%
            metal_defense15,//Tăng sức mạnh phòng thủ của metal lên 15%

            thorn_defense25,//Tang sức mạng phòng thủ của thorn lên 25%
            thorn_defense15,//Tăng sức mạnh phòng thủ của thorn lên 15%

            zombie_all_speed10,//Giảm khả năng di chuyện của tất cả zombie xuống 10%
            zombie_infantry_speed20,//Giảm khả năng di chuyển của bộ binh 20%
            zombie_airforce_speed20, // Giảm khả năng di chuyển của không quân 20%

            zombie_damage20, //Giảm sát thương của zombie xuống 20%.
        }
        #endregion


        #region REWARD

        public enum REWARD
        {
            //watch ads
            ads_free_gem,
            ads_gift_pack_1,
            ads_gift_pack_2,
            ads_gift_pack_3,
            ads_gift_pack_4,

            //unlock gun
            unlock_gun_shotgun,
            unlock_gun_ar15,
            unlock_gun_m16,
            unlock_gun_ak47,
            unlock_gun_glock,
            unlock_gun_double_shotgun,
            unlock_gun_electric,
            unlock_gun_firegun,
            unlock_gun_minigun,
            unlock_gun_bazoka,

            //defense
            unlock_defense_metal,
            unlock_defense_thorn,


            //vitory gem
            victory_gem_easy,
            victory_gem_normal,
            victory_gem_nightmate,

            //Checkin
            check_in_d1_gem,
            check_in_d2_gem,
            check_in_d3_grenade,
            check_in_d4_gem,
            check_in_d5_freeze,
            check_in_d6_gem,
            check_in_d7_grenade,
            check_in_d8_bigbomb,
            check_in_d9_freeze,
            check_in_d10_gem,
            check_in_d11_bigbomb,
            check_in_d12_gem,
            check_in_d13_grenade,
            check_in_d14_freeze,
            check_in_d15_gem,
            check_in_d16_poison,
            check_in_d17_gem,
            check_in_d18_gem,
            check_in_d19_gem,
            check_in_d20_grenade,
            check_in_d21_freeze,
            check_in_d22_gem,
            check_in_d23_grenade,
            check_in_d24_gem,
            check_in_d25_gem,
            check_in_d26_freeze,
            check_in_d27_poison,
            check_in_d28_gem,
            check_in_d29_grenade,
            check_in_d30_gem,
            check_in_d31_bigbomb,
            check_in_d32_grenade,
            check_in_d33_gem,
            check_in_d34_gem,
            check_in_d35_poison,
            check_in_d36_gem,
        }
        #endregion
    }
}
