using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MainCode_Weapon : MonoBehaviour
{
    public enum PANEL
    {
        weapon,
        defense,
        exploisives,
    }


    //PANEL MANAGER
    [System.Serializable]
    public class PanelManager
    {
        public Button buPanel;
        public GameObject objPanel;
        public Vector3 vOriginalPosOfButton;
        public void Init()
        {
            vOriginalPosOfButton = buPanel.transform.position;
            buPanel.onClick.AddListener(() => ButtonThis());


        }

        public void ButtonThis()
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
            //status of button
            int _temp = Instance.LIST_PANEL_MANAGER.Count;
            for (int i = 0; i < _temp; i++)
            {
                if (buPanel == Instance.LIST_PANEL_MANAGER[i].buPanel)
                {
                    buPanel.image.color = Color.white;
                    buPanel.transform.position = vOriginalPosOfButton;
                }
                else
                {
                    Instance.LIST_PANEL_MANAGER[i].buPanel.image.color = new Color32(160, 160, 160, 255);
                    Instance.LIST_PANEL_MANAGER[i].buPanel.transform.position
                        = Instance.LIST_PANEL_MANAGER[i].vOriginalPosOfButton + new Vector3(0, 0.15f, 0);
                }
            }

            //------------------------------------------
            int _total = Instance.LIST_PANEL_MANAGER.Count;
            for (int i = 0; i < _total; i++)
            {
                if (Instance.LIST_PANEL_MANAGER[i] == this)
                {
                    Instance.LIST_PANEL_MANAGER[i].objPanel.SetActive(true);
                }
                else
                {
                    Instance.LIST_PANEL_MANAGER[i].objPanel.SetActive(false);
                }
            }
        }

    }


    //MAIN PANEL WEAPON
    [System.Serializable]
    public class MainPanelWeapon
    {
        [SerializeField] ScrollRect m_ScrollRect;
        [SerializeField] GameObject objPrefab;
        [SerializeField] Transform GROUP_CONTAIN;
        public List<TrackWeapon> LIST_TRACK;


        [Space(30)]
        [SerializeField] TrackWeapon CURRENT_TRACK;
        [SerializeField] Button buUpgrade, buBuyAmmo, buUnlockNow;
        [SerializeField] Text txtUnlocked, txtWeaponName, txtAmmo;
        [SerializeField] Text txtPriceToUpgrade, txtPriceToBuyAmmo, txtPriceToUnlock;
        [SerializeField] Image imaWeaponIcon;

        [Space(20)]
        [SerializeField] Image imaBarOfDamage;
        [SerializeField] Image imaBarOfDamage_NextLevel;
        [SerializeField] Image imaBarOfFireRate;
        [SerializeField] Image imaLevelOfWeapon;




        //INIT
        public void Init()
        {

            int length = TheWeaponManager.Instance.LIST_WEAPON.Count;
            for (int i = 0; i < length; i++)
            {
                GameObject _new = Instantiate(objPrefab);
                LIST_TRACK.Add(_new.GetComponent<TrackWeapon>());
                _new.GetComponent<TrackWeapon>().Init(TheWeaponManager.Instance.LIST_WEAPON[i]);
                _new.transform.SetParent(GROUP_CONTAIN);
                _new.transform.localScale = Vector3.one;

                _new.SetActive(true);
            }
            ShowTrack(LIST_TRACK[0]);
            GROUP_CONTAIN.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1.0f;

            //for tutorial
            if (TheTutorialManager.Instance && !TheTutorialManager.Instance.GetTutorial(TheTutorialManager.TUTORIAL.weapon).bCompleted)
            {
                UnlockScrollRect(false);
            }
        }


        public void ShowTrack(TrackWeapon _trackweapon)
        {
            int length = LIST_TRACK.Count;
            for (int i = 0; i < length; i++)
            {
                if (LIST_TRACK[i] == _trackweapon)
                {
                    LIST_TRACK[i].GetComponent<Image>().color = Color.white;
                }
                else
                {
                    LIST_TRACK[i].GetComponent<Image>().color = Color.gray;
                }
            }

            //----------------------------------------------
            CURRENT_TRACK = _trackweapon;
            buUpgrade.onClick.RemoveAllListeners();
            buBuyAmmo.onClick.RemoveAllListeners();
            buUnlockNow.onClick.RemoveAllListeners();

            buUpgrade.onClick.AddListener(() => Upgrade());
            buBuyAmmo.onClick.AddListener(() => BuyAmmo());
            buUnlockNow.onClick.AddListener(() => UnlockNow());

            //--------------------------------
            txtWeaponName.text = CURRENT_TRACK.GUN_DATA.strNAME;
            if (!CURRENT_TRACK.GUN_DATA.DATA.bIsDefaultGun)
                txtAmmo.text = CURRENT_TRACK.GUN_DATA.DATA.iCurrentAmmo.ToString();
            else
                txtAmmo.text = "";


            if (CURRENT_TRACK.GUN_DATA.bUNLOCKED)
            {
                buUnlockNow.gameObject.SetActive(false);
                buUpgrade.gameObject.SetActive(true);
                buBuyAmmo.gameObject.SetActive(true);

                txtUnlocked.text = "STATE: UNLOCKED";
                imaWeaponIcon.sprite = CURRENT_TRACK.GUN_DATA.sprIcon;
            }
            else
            {
                buUnlockNow.gameObject.SetActive(true);
                buUpgrade.gameObject.SetActive(false);
                buBuyAmmo.gameObject.SetActive(false);

                if (CURRENT_TRACK.GUN_DATA.bIsOnlyCoinUnlock)
                {
                    txtUnlocked.text = "UNLOCKS ONLY WITH GEMS";
                }
                else
                {
                    txtUnlocked.text = "UNLOCKS ON LEVEL " + CURRENT_TRACK.GUN_DATA.iLevelToUnlock;
                }
                imaWeaponIcon.sprite = CURRENT_TRACK.GUN_DATA.sprIcon_gray;
            }



            TheEnumManager.ITEM_LEVEL _currentLevel = CURRENT_TRACK.GUN_DATA.DATA.eLevel;
            imaBarOfDamage.fillAmount = CURRENT_TRACK.GUN_DATA.GetDamage(_currentLevel) * 1.0f / 420;
            if (_currentLevel != TheEnumManager.ITEM_LEVEL.level_7)
                imaBarOfDamage_NextLevel.fillAmount = CURRENT_TRACK.GUN_DATA.GetDamage(_currentLevel + 1) * 1.0f / 420;
            else imaBarOfDamage_NextLevel.fillAmount = 0.0f;


            imaBarOfFireRate.fillAmount = (1.0f / CURRENT_TRACK.GUN_DATA.fTimeloadOrBullet) / 80f;
            imaLevelOfWeapon.sprite = MainCode_Weapon.Instance.LIST_STAR_SPRITE_FOR_WEAPON_LEVEL[(int)CURRENT_TRACK.GUN_DATA.DATA.eLevel];

            txtPriceToUpgrade.text = CURRENT_TRACK.GUN_DATA.GetPriceToUpgrade(CURRENT_TRACK.GUN_DATA.DATA.eLevel).ToString();
            txtPriceToBuyAmmo.text = CURRENT_TRACK.GUN_DATA.iPriceToBuyAmmo.ToString();
            txtPriceToUnlock.text = CURRENT_TRACK.GUN_DATA.iPriceToUnlock.ToString();

            StateOfButtons(CURRENT_TRACK);
        }

        //STATE OF BUTTONS (Upgrade + buy amoom)
        private void StateOfButtons(TrackWeapon _track)
        {
            if (CURRENT_TRACK.GUN_DATA.bUNLOCKED)
            {
                if (_track.GUN_DATA.DATA.eLevel == TheEnumManager.ITEM_LEVEL.level_7)
                {
                    buUpgrade.GetComponentInChildren<Text>().text = "MAX LEVEL";
                    buUpgrade.image.color = Color.gray;
                }
                else
                {
                    buUpgrade.GetComponentInChildren<Text>().text = "UPGRADE";
                    buUpgrade.image.color = Color.white;
                }

                if (_track.GUN_DATA.DATA.iCurrentAmmo < _track.GUN_DATA.iMaxAmmo)
                {

                    buBuyAmmo.GetComponentInChildren<Text>().text = "BUY AMMO";
                    buBuyAmmo.image.color = Color.white;
                }
                else
                {
                    buBuyAmmo.GetComponentInChildren<Text>().text = "MAX";
                    buBuyAmmo.image.color = Color.gray;
                }

            }

        }


        //UNLOCK NOW
        private void UnlockNow()
        {
            CURRENT_TRACK.UnlockNow();
        }


        //UNPGRATE
        private void Upgrade()
        {
            //for tutorial
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }

            if (!CURRENT_TRACK.GUN_DATA.bUNLOCKED)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                return;
            }
            if (CURRENT_TRACK.GUN_DATA.DATA.eLevel == TheEnumManager.ITEM_LEVEL.level_7)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                return;
            }

            //main
            int _price = CURRENT_TRACK.GUN_DATA.GetPriceToUpgrade(CURRENT_TRACK.GUN_DATA.DATA.eLevel);
            if (TheDataManager.Instance.THE_DATA_PLAYER.iGem >= _price)
            {
                TheDataManager.Instance.THE_DATA_PLAYER.iGem -= _price;//GEM
                TheEventManager.PostEvent_OnUpdatedBoard();//event--


                CURRENT_TRACK.GUN_DATA.DATA.eLevel++;
                TheDataManager.Instance.THE_DATA_PLAYER.GetWeapon(CURRENT_TRACK.GUN_DATA.DATA.eWeapon).eLevel = CURRENT_TRACK.GUN_DATA.DATA.eLevel;
                // TheDataManager.Instance.SaveDataPlayer();//save
                ShowTrack(CURRENT_TRACK);
            }
            else
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.note);
                Note.SetNote(Note.NOTE.no_gem.ToString());
                return;
            }
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_upgrade);//sound


            StateOfButtons(CURRENT_TRACK);
        }


        //BUY AMMO
        private void BuyAmmo()
        {
            //for tutorial
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }

            if (!CURRENT_TRACK.GUN_DATA.bUNLOCKED)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                return;
            }
            if (CURRENT_TRACK.GUN_DATA.iMaxAmmo == CURRENT_TRACK.GUN_DATA.DATA.iCurrentAmmo)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                return;
            }
            if (CURRENT_TRACK.GUN_DATA.DATA.bIsDefaultGun)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                return;

            }

            //main
            int _dis = CURRENT_TRACK.GUN_DATA.iMaxAmmo - CURRENT_TRACK.GUN_DATA.DATA.iCurrentAmmo;
            if (_dis >= CURRENT_TRACK.GUN_DATA.iAmmoToBuy)
            {
                int _price = CURRENT_TRACK.GUN_DATA.iPriceToBuyAmmo;
                if (TheDataManager.Instance.THE_DATA_PLAYER.iGem >= _price)
                {
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem -= _price;
                    TheEventManager.PostEvent_OnUpdatedBoard();

                    //content
                    CURRENT_TRACK.GUN_DATA.DATA.iCurrentAmmo += CURRENT_TRACK.GUN_DATA.iAmmoToBuy;
                    txtAmmo.text = CURRENT_TRACK.GUN_DATA.DATA.iCurrentAmmo.ToString();
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                }
                else
                {
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                    //khong du tien
                    TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.note);
                    Note.SetNote(Note.NOTE.no_gem.ToString());
                }

            }
            else if (_dis < CURRENT_TRACK.GUN_DATA.iAmmoToBuy && _dis > 0)
            {
                float _UnitPrice = CURRENT_TRACK.GUN_DATA.iPriceToBuyAmmo * 1.0f / CURRENT_TRACK.GUN_DATA.iAmmoToBuy; // giá tiền cho mỗi viên đạn.
                int _price = Mathf.CeilToInt(_dis * _UnitPrice);

                if (TheDataManager.Instance.THE_DATA_PLAYER.iGem >= _price)
                {
                    TheDataManager.Instance.THE_DATA_PLAYER.iGem -= _price;
                    TheEventManager.PostEvent_OnUpdatedBoard();

                    //content
                    CURRENT_TRACK.GUN_DATA.DATA.iCurrentAmmo += _dis;
                    txtAmmo.text = CURRENT_TRACK.GUN_DATA.DATA.iCurrentAmmo.ToString();
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
                }
                else
                {
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                    //khong du tien
                    TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.note);
                    Note.SetNote(Note.NOTE.no_gem.ToString());
                }

            }

            //--------------------------
            if (MainCode_Weapon.Instance.m_mainEquitpedWeapon.GetSubWeapon(CURRENT_TRACK.GUN_DATA) != null
                && !CURRENT_TRACK.GUN_DATA.DATA.bIsDefaultGun)//hiển thị lên group equiped.
                MainCode_Weapon.Instance.m_mainEquitpedWeapon.GetSubWeapon(CURRENT_TRACK.GUN_DATA).txtAmmo.text = CURRENT_TRACK.GUN_DATA.DATA.iCurrentAmmo.ToString();


            //--------------------------
            TheDataManager.Instance.THE_DATA_PLAYER.GetWeapon(CURRENT_TRACK.GUN_DATA.DATA.eWeapon).iCurrentAmmo = CURRENT_TRACK.GUN_DATA.DATA.iCurrentAmmo;
            // TheDataManager.Instance.SaveDataPlayer();//save

            StateOfButtons(CURRENT_TRACK);
        }

        //LOCK SCROLL RECT
        public void UnlockScrollRect(bool _active)
        {
            m_ScrollRect.vertical = _active;
        }
    }



    //MAIN PANEL DEFENSE
    [System.Serializable]
    public class MainPanelDefense
    {
        [SerializeField] GameObject objPrefab;
        [SerializeField] Transform GROUP_CONTAIN;
        public List<TrackDefense> LIST_TRACK;


        [Space(30)]
        [SerializeField] TrackDefense CURRENT_TRACK;
        [SerializeField] Button buUpgrade, buFix, buUnlockNow;
        [SerializeField] Text txtWeaponName, txtPriceToUpgrade, txtUnlockState, txtPriteToUnlock;
        [SerializeField] Image imaWeaponIcon;

        [Space(20)]
        [SerializeField] Image imaBarOfDefense;
        [SerializeField] Image imaBarOfDefenseNextLevel;
        [SerializeField] Image imaLevelOfDefense;


        //INIT
        public void Init()
        {
            int length = TheWeaponManager.Instance.LIST_DEFENSE.Count;
            for (int i = 0; i < length; i++)
            {
                GameObject _new = Instantiate(objPrefab);
                LIST_TRACK.Add(_new.GetComponent<TrackDefense>());
                _new.GetComponent<TrackDefense>().Init(TheWeaponManager.Instance.LIST_DEFENSE[i]);
                _new.transform.SetParent(GROUP_CONTAIN);
                _new.transform.localScale = Vector3.one;
                _new.SetActive(true);
            }
            ShowTrack(LIST_TRACK[0]);
        }



        public void ShowTrack(TrackDefense _track)
        {
            buUpgrade.onClick.RemoveAllListeners();
            buUpgrade.onClick.AddListener(() => Upgrade());

            buFix.onClick.RemoveAllListeners();
            buFix.onClick.AddListener(() => Fix());

            buUnlockNow.onClick.RemoveAllListeners();
            buUnlockNow.onClick.AddListener(() => UnlockNow());


            if (_track == null) return;

            int length = LIST_TRACK.Count;
            for (int i = 0; i < length; i++)
            {
                if (LIST_TRACK[i] == _track)
                {
                    LIST_TRACK[i].GetComponent<Image>().color = Color.white;
                }
                else
                {
                    LIST_TRACK[i].GetComponent<Image>().color = Color.gray;
                }
            }


            // MainCode_Weapon.Instance.GetPanel(PANEL.defense).ShowChooseImage(TRACK_DEFENSE.buThis.image);
            CURRENT_TRACK = _track;

            if (CURRENT_TRACK.DEFENSE_DATA.bUNLOCKED)
            {
                txtUnlockState.text = "STATE: UNLOCKED";
            }
            else
            {
                if (CURRENT_TRACK.DEFENSE_DATA.bIsOnlyCoinUnlock)
                {
                    txtUnlockState.text = "UNLOCKS ONLY WITH GEMS";
                }
                else
                {
                    txtUnlockState.text = "UNLOCKS ON LEVEL " + CURRENT_TRACK.DEFENSE_DATA.iLevelToUnlock;
                }
            }


            txtWeaponName.text = CURRENT_TRACK.DEFENSE_DATA.strName;
            imaWeaponIcon.sprite = CURRENT_TRACK.DEFENSE_DATA.sprIcon;


            TheEnumManager.ITEM_LEVEL _currentLevel = CURRENT_TRACK.DEFENSE_DATA.DATA.eLevel;


            imaBarOfDefense.fillAmount = CURRENT_TRACK.DEFENSE_DATA.GetDefense(_currentLevel) * 1.0f / 400;
            if (_currentLevel != TheEnumManager.ITEM_LEVEL.level_7)
                imaBarOfDefenseNextLevel.fillAmount = CURRENT_TRACK.DEFENSE_DATA.GetDefense(_currentLevel + 1) * 1.0f / 400;
            else
                imaBarOfDefenseNextLevel.fillAmount = 0;



            imaLevelOfDefense.sprite = MainCode_Weapon.Instance.LIST_STAR_SPRITE_FOR_WEAPON_LEVEL[(int)CURRENT_TRACK.DEFENSE_DATA.DATA.eLevel];
            txtPriceToUpgrade.text = CURRENT_TRACK.DEFENSE_DATA.GetPriceToUpgrade(CURRENT_TRACK.DEFENSE_DATA.DATA.eLevel).ToString();
            txtPriteToUnlock.text = CURRENT_TRACK.DEFENSE_DATA.iPriteToUnlock.ToString();
            StateOfButtons(CURRENT_TRACK);
        }


        //UNLOCK NOW
        private void UnlockNow()
        { //for tutorial
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }

            CURRENT_TRACK.UnlockNow();
        }


        //STATE OF BUTTONS (Upgrade + buy amoom)
        private void StateOfButtons(TrackDefense _track)
        {
            if (!_track.DEFENSE_DATA.bUNLOCKED)
            {
                buUnlockNow.gameObject.SetActive(true);
                buUpgrade.gameObject.SetActive(false);

            }
            else
            {
                buUnlockNow.gameObject.SetActive(false);
                buUpgrade.gameObject.SetActive(true);
            }

            if (_track.DEFENSE_DATA.DATA.eLevel == TheEnumManager.ITEM_LEVEL.level_7)
            {
                buUpgrade.GetComponentInChildren<Text>().text = "MAX";
                buUpgrade.image.color = Color.gray;
            }
            else
            {
                buUpgrade.GetComponentInChildren<Text>().text = "UPGRADE";
                buUpgrade.image.color = Color.white;
            }

        }

        //UPGRADE
        private void Upgrade()
        { //for tutorial
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }


            if (!CURRENT_TRACK.DEFENSE_DATA.bUNLOCKED)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                return;
            }
            if (CURRENT_TRACK.DEFENSE_DATA.DATA.eLevel == TheEnumManager.ITEM_LEVEL.level_7)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                return;
            }



            int _price = CURRENT_TRACK.DEFENSE_DATA.GetPriceToUpgrade(CURRENT_TRACK.DEFENSE_DATA.DATA.eLevel);
            if (TheDataManager.Instance.THE_DATA_PLAYER.iGem >= _price)
            {
                TheDataManager.Instance.THE_DATA_PLAYER.iGem -= _price;
                TheEventManager.PostEvent_OnUpdatedBoard();//event

                //content
                CURRENT_TRACK.DEFENSE_DATA.DATA.eLevel++;
                TheDataManager.Instance.THE_DATA_PLAYER.GetDefense(CURRENT_TRACK.DEFENSE_DATA.DATA.eDefense).eLevel = CURRENT_TRACK.DEFENSE_DATA.DATA.eLevel;

                ShowTrack(CURRENT_TRACK);
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_upgrade);//sound
            }
            else
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.note);
                Note.SetNote(Note.NOTE.no_gem.ToString());

            }



            StateOfButtons(CURRENT_TRACK);
        }


        //FIX
        private void Fix()
        {
            //for tutorial
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }

        }
    }


    //MAIN PANEL: SUPPORT
    [System.Serializable]
    public class MainPanelSupport
    {
        public List<TrackSupport> LIST_TRACK;

        //=================================
        [Space(30)]
        public TrackSupport CURRENT_TRACK;
        public Button buBuy, buUpgrade;

        [SerializeField] Text txtName, txtValue, txtContent;
        [SerializeField] Text txtPriceToBuy;
        public Image imaMainIcon;



        //INIT
        public void Init()
        {
            ShowTrack(LIST_TRACK[0]);
        }
        public void ShowTrack(TrackSupport _tracksupport)
        {
            //STatus
            int _total = LIST_TRACK.Count;
            for (int i = 0; i < _total; i++)
            {
                if (LIST_TRACK[i] == _tracksupport)
                {
                    LIST_TRACK[i].GetComponent<Image>().color = Color.white;
                }
                else
                {
                    LIST_TRACK[i].GetComponent<Image>().color = Color.gray;
                }
            }


            CURRENT_TRACK = _tracksupport;
            buUpgrade.onClick.RemoveAllListeners();
            buUpgrade.onClick.AddListener(() => Upgrade());
            buBuy.onClick.RemoveAllListeners();
            buBuy.onClick.AddListener(() => Buy());

            if (CURRENT_TRACK == null) return;
            if (CURRENT_TRACK.SUPPORT_DATA == null) return;


            //show info
            txtName.text = CURRENT_TRACK.SUPPORT_DATA.strName;
            txtValue.text = CURRENT_TRACK.SUPPORT_DATA.DATA.iCurrentValue + "+1";
            txtContent.text = CURRENT_TRACK.SUPPORT_DATA.strContent;
            txtPriceToBuy.text = CURRENT_TRACK.SUPPORT_DATA.iPrice.ToString();
            imaMainIcon.sprite = CURRENT_TRACK.SUPPORT_DATA.sprIcon;

            StateOfButtons(CURRENT_TRACK);
        }

        //STATE OF BUTTONS (Upgrade + buy amoom)
        private void StateOfButtons(TrackSupport _track)
        {


            if (_track.SUPPORT_DATA.DATA.iCurrentValue >= _track.SUPPORT_DATA.iMaxValue)
            {
                buBuy.GetComponentInChildren<Text>().text = "MAX";
                buBuy.image.color = Color.gray;
                txtValue.text = CURRENT_TRACK.SUPPORT_DATA.DATA.iCurrentValue.ToString();
            }
            else
            {
                buBuy.GetComponentInChildren<Text>().text = "BUY";
                buBuy.image.color = Color.white;
            }

        }

        //UPGRADE
        public void Upgrade()
        {
            //for tutorial
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }

            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
        }

        //BUY
        private void Buy()
        {
            //for tutorial
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }

            if (CURRENT_TRACK.SUPPORT_DATA.DATA.iCurrentValue >= CURRENT_TRACK.SUPPORT_DATA.iMaxValue)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                return;
            }


            int _price = CURRENT_TRACK.SUPPORT_DATA.iPrice;
            if (TheDataManager.Instance.THE_DATA_PLAYER.iGem >= _price)
            {

                TheDataManager.Instance.THE_DATA_PLAYER.iGem -= _price;
                TheEventManager.PostEvent_OnUpdatedBoard();//event 

                //content
                CURRENT_TRACK.SUPPORT_DATA.DATA.iCurrentValue++;
                txtValue.text = CURRENT_TRACK.SUPPORT_DATA.DATA.iCurrentValue + "+1";
                CURRENT_TRACK.ShowInfo();
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
               
            }
            else
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.note);
                Note.SetNote(Note.NOTE.no_gem.ToString());
            }
            StateOfButtons(CURRENT_TRACK);
        }
    }

    //WEAPON EQUIPED CLASS
    [System.Serializable]
    public class MainEquipedWeapon
    {
        //WEAPON
        [System.Serializable]
        public class SubWeapon
        {
            public GunData _gundata;
            public Button _button;
            public Text txtAmmo;
            //REMOVE GUN FROM LIST
            private void Remove()
            {
                //for tutorial
                if (TheTutorialManager.Instance)
                {
                    if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
                }

                if (_gundata.DATA.bIsDefaultGun) return;

                TheEventManager.Weapon_OnRemoveFromEquipedWeaponList(_gundata);//event
                _gundata = null;
                _button.transform.Find("Icon").GetComponent<Image>().color = Color.white * 0.0f;
                _button.transform.Find("Close").GetComponent<Image>().color = Color.white * 0.0f;
                txtAmmo.text = "";
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
            }


            //INIT
            public void Init(GunData _gun)
            {
                _gundata = _gun;

                if (_gun)
                {
                    Image _gunIcon = _button.transform.Find("Icon").GetComponent<Image>();
                    _gunIcon.color = Color.white * 1.0f;
                    _gunIcon.sprite = _gundata.sprIcon;
                    _button.transform.Find("Close").GetComponent<Image>().color = Color.white;
                    txtAmmo.text = _gun.DATA.iCurrentAmmo.ToString();
                }
                else
                {
                    _button.transform.Find("Icon").GetComponent<Image>().color =
                       Color.white * 0.0f;
                    _button.transform.Find("Close").GetComponent<Image>().color =
                       Color.white * 0.0f;
                    txtAmmo.text = "";
                }

                if (_gundata.DATA.bIsDefaultGun)
                {
                    _button.transform.Find("Close").GetComponent<Image>().color = Color.white * 0.0f;
                    txtAmmo.text = "";
                }

                _button.onClick.RemoveAllListeners();
                _button.onClick.AddListener(() => Remove());
            }
        }
        public List<SubWeapon> LIST_BUTTON_WEAPON_EQUITPED;
        public SubWeapon GetSubWeapon(GunData _gun)
        {
            int _total = LIST_BUTTON_WEAPON_EQUITPED.Count;
            for (int i = 0; i < _total; i++)
            {
                if (LIST_BUTTON_WEAPON_EQUITPED[i]._gundata == _gun)
                    return LIST_BUTTON_WEAPON_EQUITPED[i];
            }
            return null;
        }
        public void AddEquipedWeapon(GunData gundata)
        {
            if (GetSubWeapon(gundata) != null)
            {

                return;
            }

            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_equiped);//sound
            //---------------------
            int _totalTrack = LIST_BUTTON_WEAPON_EQUITPED.Count;
            for (int i = 0; i < _totalTrack; i++)
            {
                if (LIST_BUTTON_WEAPON_EQUITPED[i]._gundata == null)
                {
                    LIST_BUTTON_WEAPON_EQUITPED[i].Init(gundata);
                    TheEventManager.Weapon_OnAddToEquipedWeaponList(gundata);//event

                    return;
                }
            }


        }


    }


    //WEAPON EQUIPED CLASS
    [System.Serializable]
    public class MainEquipedDefense
    {

        //DEFENSE
        [System.Serializable]
        public class SubDefense
        {
            public DefenseData _data;
            public Button _button;

            //REMOVE GUN FROM LIST
            private void Remove()
            {
                if (_data == null)
                {
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                    return;
                }
                if (_data.DATA.bDefault)
                {
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                    return;
                }

                _button.image.sprite = Instance.m_mainEquitpedDefense.sprEmpty;
                TheEventManager.Defense_OnRemoveToEquipedDefenseList(_data);//event
                _data = null;
                _button.transform.Find("Icon").GetComponent<Image>().color = Color.white * 0.0f;
                _button.transform.Find("Close").GetComponent<Image>().color = Color.white * 0.0f;
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
            }


            //INIT
            public void Init(DefenseData _defense)
            {
                _data = _defense;

                if (_data)
                {
                    _button.image.sprite = _data.sprIcon;
                    _button.transform.Find("Close").GetComponent<Image>().color = Color.white;

                }
                else
                {

                    _button.transform.Find("Close").GetComponent<Image>().color =
                       Color.white * 0.0f;

                }

                if (_data.DATA.bDefault)
                {
                    _button.transform.Find("Close").GetComponent<Image>().color = Color.white * 0.0f;

                }

                _button.onClick.RemoveAllListeners();
                _button.onClick.AddListener(() => Remove());
            }
        }


        public Sprite sprEmpty;
        public List<SubDefense> LIST_BUTTON_DEFENSE_EQUITPED;
        public SubDefense GetSubWeapon(DefenseData _defense)
        {
            int _total = LIST_BUTTON_DEFENSE_EQUITPED.Count;
            for (int i = 0; i < _total; i++)
            {
                if (LIST_BUTTON_DEFENSE_EQUITPED[i]._data == _defense)
                    return LIST_BUTTON_DEFENSE_EQUITPED[i];
            }
            return null;
        }
        public void AddEquipedDefense(DefenseData _defense)
        {
            if (GetSubWeapon(_defense) != null)
                return;


            //---------------------
            int _totalTrack = LIST_BUTTON_DEFENSE_EQUITPED.Count;
            for (int i = 0; i < _totalTrack; i++)
            {
                if (LIST_BUTTON_DEFENSE_EQUITPED[i]._data == null)
                {
                    LIST_BUTTON_DEFENSE_EQUITPED[i].Init(_defense);
                    return;
                }
            }


        }
    }



    //==============================================================================================//
    public static MainCode_Weapon Instance;
    [Space(30)]
    public List<PanelManager> LIST_PANEL_MANAGER;
    private void Init_PanelManager()
    {
        int _total = LIST_PANEL_MANAGER.Count;
        for (int i = 0; i < _total; i++)
        {
            LIST_PANEL_MANAGER[i].Init();
        }
        LIST_PANEL_MANAGER[0].ButtonThis();

        //for tutorial
        if (TheTutorialManager.Instance && !TheTutorialManager.Instance.GetTutorial(TheTutorialManager.TUTORIAL.weapon).bCompleted)
        {
            LIST_PANEL_MANAGER[1].buPanel.enabled = false;
            LIST_PANEL_MANAGER[2].buPanel.enabled = false;
        }
    }


    [Space(30)]
    public MainPanelWeapon m_MainPanelWeapon;
    public MainPanelDefense m_MainPanelDefense;
    public MainPanelSupport m_MainPanelSupport;

    [Space(20)]
    public MainEquipedWeapon m_mainEquitpedWeapon;//cac weapon đc cập nhật để dùng trong gameplay
    public MainEquipedDefense m_mainEquitpedDefense;//cac weapon đc cập nhật để dùng trong gameplay




    [Space(30)]
    [SerializeField]
    private Button buBack;
    [SerializeField]
    private Button buStart;
    public List<Sprite> LIST_STAR_SPRITE_FOR_WEAPON_LEVEL;


    private void Awake()
    {
        if (Instance == null) Instance = this;

        TheUiManager.Instance.SetCameraForPopupCanvas(Camera.main);//set camera
    }


    // Start is called before the first frame update
    void Start()
    {

        Init_PanelManager();

        buBack.onClick.AddListener(() => SetButton(buBack));
        buStart.onClick.AddListener(() => SetButton(buStart));


        m_MainPanelWeapon.Init();
        m_MainPanelDefense.Init();
        m_MainPanelSupport.Init();
        SetupEquipList();


    }





    //SET BUTTON
    private void SetButton(Button _bu)
    {   //for tutorial
        if (TheTutorialManager.Instance)
        {
            if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
        }


        if (_bu == buBack)
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_back);//sound
            TheUiManager.Instance.LoadScene(TheUiManager.SCENE.LevelSelection);
        }
        else if (_bu == buStart)
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_zombie_gruzz_boss);
            TheUiManager.Instance.LoadScene(TheUiManager.SCENE.Gameplay);
        }
    }


    private void SetupEquipList()
    {
        //Weapon
        int _total = TheWeaponManager.Instance.LIST_EQUIPED_WEAPON.Count;
        for (int i = 0; i < _total; i++)
        {
            m_mainEquitpedWeapon.AddEquipedWeapon(TheWeaponManager.Instance.LIST_EQUIPED_WEAPON[i]);
        }

        //defense
        _total = TheWeaponManager.Instance.LIST_DEFENSE.Count;
        for (int i = 0; i < _total; i++)
        {
            if (TheWeaponManager.Instance.LIST_DEFENSE[i].DATA.bEquiped)
                m_mainEquitpedDefense.AddEquipedDefense(TheWeaponManager.Instance.LIST_DEFENSE[i]);
        }

    }


    private void OnDisable()
    {
        TheDataManager.Instance.SaveDataPlayer();//save
    }



}
