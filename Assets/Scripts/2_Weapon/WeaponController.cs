using System.Collections.Generic;
using MANAGERS;
using MODULES.Scriptobjectable;
using SCREENS;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _2_Weapon
{
    public class WeaponController : MonoBehaviour
    {

        [System.Serializable]
        public class PanelManager
        {
            
            [FormerlySerializedAs("buPanel")] [SerializeField] private Button _panelButton;
            [SerializeField] private GameObject objPanel;
            [SerializeField] private Vector3 vOriginalPosOfButton;

            public Button PanelButton => _panelButton;
            public void Construct()
            {
                vOriginalPosOfButton = _panelButton.transform.position;
                _panelButton.onClick.AddListener(() => Assign());
            }

            public void Assign()
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                int temp = Instance._listPanelManager.Count;
                for (int i = 0; i < temp; i++)
                {
                    if (_panelButton == Instance._listPanelManager[i]._panelButton)
                    {
                        _panelButton.image.color = Color.white;
                        _panelButton.transform.position = vOriginalPosOfButton;
                    }
                    else
                    {
                        Instance._listPanelManager[i]._panelButton.image.color = new Color32(160, 160, 160, 255);
                        Instance._listPanelManager[i]._panelButton.transform.position
                            = Instance._listPanelManager[i].vOriginalPosOfButton + new Vector3(0, 0.15f, 0);
                    }
                }

                //------------------------------------------
                int total = Instance._listPanelManager.Count;
                for (int i = 0; i < total; i++)
                {
                    if (Instance._listPanelManager[i] == this)
                    {
                        Instance._listPanelManager[i].objPanel.SetActive(true);
                    }
                    else
                    {
                        Instance._listPanelManager[i].objPanel.SetActive(false);
                    }
                }
            }

        }


        //MAIN PANEL WEAPON
        [System.Serializable]
        public class WeaponPanel
        {
            [FormerlySerializedAs("m_ScrollRect")] [SerializeField] private ScrollRect _scrollRect;
            [FormerlySerializedAs("objPrefab")] [SerializeField] private GameObject _weaponPrefab;
            [FormerlySerializedAs("GROUP_CONTAIN")] [SerializeField] private Transform _groupContain;
            [FormerlySerializedAs("LIST_TRACK")] [SerializeField] private List<Weapon> _trackWeapons;
            
            
            [Space(30)]
            [FormerlySerializedAs("CURRENT_TRACK")] [SerializeField] private Weapon _currentTrack;
            [FormerlySerializedAs("buUpgrade")] [SerializeField] private Button _upgradeButton;
            [FormerlySerializedAs("buBuyAmmo")] [SerializeField] private Button _ammoBuyButton;
            [FormerlySerializedAs("buUnlockNow")] [SerializeField] private Button _unlockButton;
            [FormerlySerializedAs("txtUnlocked")] [SerializeField] private Text _unlockedText;
            [FormerlySerializedAs("txtWeaponName")] [SerializeField] private Text _weaponNameText;
            [FormerlySerializedAs("txtAmmo")] [SerializeField] private Text _ammoText;
            [FormerlySerializedAs("txtPriceToUpgrade")] [SerializeField] private Text _upgradeCostText;
            [FormerlySerializedAs("txtPriceToBuyAmmo")] [SerializeField] private Text _ammoPriceText;
            [FormerlySerializedAs("txtPriceToUnlock")] [SerializeField] private Text _unlockPriceText;
            [FormerlySerializedAs("imaWeaponIcon")] [SerializeField] private Image _weaponImage;

            
            [Space(20)]
            [FormerlySerializedAs("imaBarOfDamage")][SerializeField] private Image _damageBar;
            [FormerlySerializedAs("imaBarOfDamage_NextLevel")] [SerializeField] private Image _damageBarNextLvl;
            [FormerlySerializedAs("imaBarOfFireRate")] [SerializeField] private Image _fireBar;
            [FormerlySerializedAs("imaLevelOfWeapon")] [SerializeField] private Image _levelOfWeaponImage;

            public void Construct()
            {
                int length = MANAGERS.WeaponController.Instance._weaponList.Count;
                for (int i = 0; i < length; i++)
                {
                    GameObject weaponnctance = Instantiate(_weaponPrefab);
                    _trackWeapons.Add(weaponnctance.GetComponent<Weapon>());
                    weaponnctance.GetComponent<Weapon>().Construct(MANAGERS.WeaponController.Instance._weaponList[i]);
                    weaponnctance.transform.SetParent(_groupContain);
                    weaponnctance.transform.localScale = Vector3.one;

                    weaponnctance.SetActive(true);
                }
                VisualiseTrack(_trackWeapons[0]);
                _groupContain.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1.0f;
                
                if (TutorialController.Instance && !TutorialController.Instance.GetTutorial(TutorialController.TUTORIAL.weapon)._isCompleted)
                {
                    UnlockScrollRect(false);
                }
            }


            public void VisualiseTrack(Weapon trackWeapon)
            {
                int length = _trackWeapons.Count;
                for (int i = 0; i < length; i++)
                {
                    if (_trackWeapons[i] == trackWeapon)
                    {
                        _trackWeapons[i].GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        _trackWeapons[i].GetComponent<Image>().color = Color.gray;
                    }
                }

                //----------------------------------------------
                _currentTrack = trackWeapon;
                _upgradeButton.onClick.RemoveAllListeners();
                _ammoBuyButton.onClick.RemoveAllListeners();
                _unlockButton.onClick.RemoveAllListeners();

                _upgradeButton.onClick.AddListener(() => UpgradeWeapon());
                _ammoBuyButton.onClick.AddListener(() => BuyAmmo());
                _unlockButton.onClick.AddListener(() => UnlockWeapon());

                //--------------------------------
                _weaponNameText.text = _currentTrack.GunData.strNAME;
                if (!_currentTrack.GunData.DATA.bIsDefaultGun)
                    _ammoText.text = _currentTrack.GunData.DATA.iCurrentAmmo.ToString();
                else
                    _ammoText.text = "";


                if (_currentTrack.GunData.bUNLOCKED)
                {
                    _unlockButton.gameObject.SetActive(false);
                    _upgradeButton.gameObject.SetActive(true);
                    _ammoBuyButton.gameObject.SetActive(true);

                    _unlockedText.text = "STATE: UNLOCKED";
                    _weaponImage.sprite = _currentTrack.GunData.sprIcon;
                }
                else
                {
                    _unlockButton.gameObject.SetActive(true);
                    _upgradeButton.gameObject.SetActive(false);
                    _ammoBuyButton.gameObject.SetActive(false);

                    if (_currentTrack.GunData.bIsOnlyCoinUnlock)
                    {
                        _unlockedText.text = "UNLOCKS ONLY WITH GEMS";
                    }
                    else
                    {
                        _unlockedText.text = "UNLOCKS ON LEVEL " + _currentTrack.GunData.iLevelToUnlock;
                    }
                    _weaponImage.sprite = _currentTrack.GunData.sprIcon_gray;
                }



                EnumController.ITEM_LEVEL _currentLevel = _currentTrack.GunData.DATA.eLevel;
                _damageBar.fillAmount = _currentTrack.GunData.GetDamage(_currentLevel) * 1.0f / 420;
                if (_currentLevel != EnumController.ITEM_LEVEL.level_7)
                    _damageBarNextLvl.fillAmount = _currentTrack.GunData.GetDamage(_currentLevel + 1) * 1.0f / 420;
                else _damageBarNextLvl.fillAmount = 0.0f;


                _fireBar.fillAmount = (1.0f / _currentTrack.GunData.fTimeloadOrBullet) / 80f;
                _levelOfWeaponImage.sprite = WeaponController.Instance._listStarSpriteForWeaponLevel[(int)_currentTrack.GunData.DATA.eLevel];

                _upgradeCostText.text = _currentTrack.GunData.GetPriceToUpgrade(_currentTrack.GunData.DATA.eLevel).ToString();
                _ammoPriceText.text = _currentTrack.GunData.iPriceToBuyAmmo.ToString();
                _unlockPriceText.text = _currentTrack.GunData.iPriceToUnlock.ToString();

                ButtonStates(_currentTrack);
            }

            //STATE OF BUTTONS (Upgrade + buy amoom)
            private void ButtonStates(Weapon _track)
            {
                if (_currentTrack.GunData.bUNLOCKED)
                {
                    if (_track.GunData.DATA.eLevel == EnumController.ITEM_LEVEL.level_7)
                    {
                        _upgradeButton.GetComponentInChildren<Text>().text = "MAX LEVEL";
                        _upgradeButton.image.color = Color.gray;
                    }
                    else
                    {
                        _upgradeButton.GetComponentInChildren<Text>().text = "UPGRADE";
                        _upgradeButton.image.color = Color.white;
                    }

                    if (_track.GunData.DATA.iCurrentAmmo < _track.GunData.iMaxAmmo)
                    {

                        _ammoBuyButton.GetComponentInChildren<Text>().text = "BUY AMMO";
                        _ammoBuyButton.image.color = Color.white;
                    }
                    else
                    {
                        _ammoBuyButton.GetComponentInChildren<Text>().text = "MAX";
                        _ammoBuyButton.image.color = Color.gray;
                    }

                }

            }


            //UNLOCK NOW
            private void UnlockWeapon()
            {
                _currentTrack.Unlock();
            }


            //UNPGRATE
            private void UpgradeWeapon()
            {
                //for tutorial
                if (TutorialController.Instance)
                {
                    if (!TutorialController.Instance.IsRightInput()) return;
                }

                if (!_currentTrack.GunData.bUNLOCKED)
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                    return;
                }
                if (_currentTrack.GunData.DATA.eLevel == EnumController.ITEM_LEVEL.level_7)
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                    return;
                }

                //main
                int _price = _currentTrack.GunData.GetPriceToUpgrade(_currentTrack.GunData.DATA.eLevel);
                if (DataController.Instance.playerData.Gem >= _price)
                {
                    DataController.Instance.playerData.Gem -= _price;//GEM
                    EventController.OnUpdatedBoardInvoke();//event--


                    _currentTrack.GunData.DATA.eLevel++;
                    DataController.Instance.playerData.TakeWeapon(_currentTrack.GunData.DATA.eWeapon).eLevel = _currentTrack.GunData.DATA.eLevel;
                    // TheDataManager.Instance.SaveDataPlayer();//save
                    VisualiseTrack(_currentTrack);
                }
                else
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                    UIController.Instance.PopUpShow(UIController.POP_UP.note);
                    Note.AssignNote(Note.NOTE.no_gem.ToString());
                    return;
                }
                SoundController.Instance.Play(SoundController.SOUND.ui_upgrade);//sound


                ButtonStates(_currentTrack);
            }


            //BUY AMMO
            private void BuyAmmo()
            {
                //for tutorial
                if (TutorialController.Instance)
                {
                    if (!TutorialController.Instance.IsRightInput()) return;
                }

                if (!_currentTrack.GunData.bUNLOCKED)
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                    return;
                }
                if (_currentTrack.GunData.iMaxAmmo == _currentTrack.GunData.DATA.iCurrentAmmo)
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                    return;
                }
                if (_currentTrack.GunData.DATA.bIsDefaultGun)
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                    return;

                }

                //main
                int _dis = _currentTrack.GunData.iMaxAmmo - _currentTrack.GunData.DATA.iCurrentAmmo;
                if (_dis >= _currentTrack.GunData.iAmmoToBuy)
                {
                    int _price = _currentTrack.GunData.iPriceToBuyAmmo;
                    if (DataController.Instance.playerData.Gem >= _price)
                    {
                        DataController.Instance.playerData.Gem -= _price;
                        EventController.OnUpdatedBoardInvoke();

                        //content
                        _currentTrack.GunData.DATA.iCurrentAmmo += _currentTrack.GunData.iAmmoToBuy;
                        _ammoText.text = _currentTrack.GunData.DATA.iCurrentAmmo.ToString();
                        SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    }
                    else
                    {
                        SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                        //khong du tien
                        UIController.Instance.PopUpShow(UIController.POP_UP.note);
                        Note.AssignNote(Note.NOTE.no_gem.ToString());
                    }

                }
                else if (_dis < _currentTrack.GunData.iAmmoToBuy && _dis > 0)
                {
                    float _UnitPrice = _currentTrack.GunData.iPriceToBuyAmmo * 1.0f / _currentTrack.GunData.iAmmoToBuy; // giá tiền cho mỗi viên đạn.
                    int _price = Mathf.CeilToInt(_dis * _UnitPrice);

                    if (DataController.Instance.playerData.Gem >= _price)
                    {
                        DataController.Instance.playerData.Gem -= _price;
                        EventController.OnUpdatedBoardInvoke();

                        //content
                        _currentTrack.GunData.DATA.iCurrentAmmo += _dis;
                        _ammoText.text = _currentTrack.GunData.DATA.iCurrentAmmo.ToString();
                        SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
                    }
                    else
                    {
                        SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                        //khong du tien
                        UIController.Instance.PopUpShow(UIController.POP_UP.note);
                        Note.AssignNote(Note.NOTE.no_gem.ToString());
                    }

                }

                //--------------------------
                if (WeaponController.Instance._weaponPicked.GetSubWeapon(_currentTrack.GunData) != null
                    && !_currentTrack.GunData.DATA.bIsDefaultGun)//hiển thị lên group equiped.
                    WeaponController.Instance._weaponPicked.GetSubWeapon(_currentTrack.GunData).AmmoText.text = _currentTrack.GunData.DATA.iCurrentAmmo.ToString();


                //--------------------------
                DataController.Instance.playerData.TakeWeapon(_currentTrack.GunData.DATA.eWeapon).iCurrentAmmo = _currentTrack.GunData.DATA.iCurrentAmmo;
                // TheDataManager.Instance.SaveDataPlayer();//save

                ButtonStates(_currentTrack);
            }

            //LOCK SCROLL RECT
            public void UnlockScrollRect(bool _active)
            {
                _scrollRect.vertical = _active;
            }
        }



        //MAIN PANEL DEFENSE
        [System.Serializable]
        public class DefencePanel
        {
            [FormerlySerializedAs("objPrefab")] [SerializeField] private GameObject _defencePrefab;
            [FormerlySerializedAs("GROUP_CONTAIN")] [SerializeField] private Transform _groupContain;
            [FormerlySerializedAs("LIST_TRACK")] [SerializeField] private List<Defense> _trackList;


            
            [Space(30)]
            [FormerlySerializedAs("CURRENT_TRACK")] [SerializeField] private Defense _currentTrack;
            [FormerlySerializedAs("buUpgrade")] [SerializeField] private Button _upgradeButton;
            [FormerlySerializedAs("buFix")] [SerializeField] private Button _fixButton;
            [FormerlySerializedAs("buUnlockNow")] [SerializeField] private Button _unlockButton;
            [FormerlySerializedAs("txtWeaponName")] [SerializeField] private Text _nameText;
            [FormerlySerializedAs("txtPriceToUpgrade")] [SerializeField] private Text _upgradePriceText;
            [FormerlySerializedAs("txtUnlockState")] [SerializeField] private Text _unlockStateText;
            [FormerlySerializedAs("txtPriteToUnlock")] [SerializeField] private Text _priceToUnlockPrice;
            [FormerlySerializedAs("imaWeaponIcon")] [SerializeField] private Image _icon;

            
            [Space(20)]
            [FormerlySerializedAs("imaBarOfDefense")][SerializeField] private Image _defenceBar;
            [FormerlySerializedAs("imaBarOfDefenseNextLevel")] [SerializeField] private Image _nextLevelBar;
            [FormerlySerializedAs("imaLevelOfDefense")] [SerializeField] private Image _defenceLevelImage;


            //INIT
            public void Construct()
            {
                int length = MANAGERS.WeaponController.Instance._defenceList.Count;
                for (int i = 0; i < length; i++)
                {
                    GameObject _new = Instantiate(_defencePrefab);
                    _trackList.Add(_new.GetComponent<Defense>());
                    _new.GetComponent<Defense>().Construct(MANAGERS.WeaponController.Instance._defenceList[i]);
                    _new.transform.SetParent(_groupContain);
                    _new.transform.localScale = Vector3.one;
                    _new.SetActive(true);
                }
                ViewTrack(_trackList[0]);
            }



            public void ViewTrack(Defense _track)
            {
                _upgradeButton.onClick.RemoveAllListeners();
                _upgradeButton.onClick.AddListener(() => Upgrade());

                _fixButton.onClick.RemoveAllListeners();
                _fixButton.onClick.AddListener(() => Fix());

                _unlockButton.onClick.RemoveAllListeners();
                _unlockButton.onClick.AddListener(() => Unlock());


                if (_track == null) return;

                int length = _trackList.Count;
                for (int i = 0; i < length; i++)
                {
                    if (_trackList[i] == _track)
                    {
                        _trackList[i].GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        _trackList[i].GetComponent<Image>().color = Color.gray;
                    }
                }
                
                _currentTrack = _track;

                if (_currentTrack.DefenseData.bUNLOCKED)
                {
                    _unlockStateText.text = "STATE: UNLOCKED";
                }
                else
                {
                    if (_currentTrack.DefenseData.bIsOnlyCoinUnlock)
                    {
                        _unlockStateText.text = "UNLOCKS ONLY WITH GEMS";
                    }
                    else
                    {
                        _unlockStateText.text = "UNLOCKS ON LEVEL " + _currentTrack.DefenseData.iLevelToUnlock;
                    }
                }


                _nameText.text = _currentTrack.DefenseData.strName;
                _icon.sprite = _currentTrack.DefenseData.sprIcon;


                EnumController.ITEM_LEVEL _currentLevel = _currentTrack.DefenseData.DATA.eLevel;


                _defenceBar.fillAmount = _currentTrack.DefenseData.GetDefense(_currentLevel) * 1.0f / 400;
                if (_currentLevel != EnumController.ITEM_LEVEL.level_7)
                    _nextLevelBar.fillAmount = _currentTrack.DefenseData.GetDefense(_currentLevel + 1) * 1.0f / 400;
                else
                    _nextLevelBar.fillAmount = 0;



                _defenceLevelImage.sprite = WeaponController.Instance._listStarSpriteForWeaponLevel[(int)_currentTrack.DefenseData.DATA.eLevel];
                _upgradePriceText.text = _currentTrack.DefenseData.GetPriceToUpgrade(_currentTrack.DefenseData.DATA.eLevel).ToString();
                _priceToUnlockPrice.text = _currentTrack.DefenseData.iPriteToUnlock.ToString();
                ButtonStates(_currentTrack);
            }


            //UNLOCK NOW
            private void Unlock()
            { //for tutorial
                if (TutorialController.Instance)
                {
                    if (!TutorialController.Instance.IsRightInput()) return;
                }

                _currentTrack.Unlock();
            }


            //STATE OF BUTTONS (Upgrade + buy amoom)
            private void ButtonStates(Defense _track)
            {
                if (!_track.DefenseData.bUNLOCKED)
                {
                    _unlockButton.gameObject.SetActive(true);
                    _upgradeButton.gameObject.SetActive(false);

                }
                else
                {
                    _unlockButton.gameObject.SetActive(false);
                    _upgradeButton.gameObject.SetActive(true);
                }

                if (_track.DefenseData.DATA.eLevel == EnumController.ITEM_LEVEL.level_7)
                {
                    _upgradeButton.GetComponentInChildren<Text>().text = "MAX";
                    _upgradeButton.image.color = Color.gray;
                }
                else
                {
                    _upgradeButton.GetComponentInChildren<Text>().text = "UPGRADE";
                    _upgradeButton.image.color = Color.white;
                }

            }
            private void Upgrade()
            { //for tutorial
                if (TutorialController.Instance)
                {
                    if (!TutorialController.Instance.IsRightInput()) return;
                }


                if (!_currentTrack.DefenseData.bUNLOCKED)
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                    return;
                }
                if (_currentTrack.DefenseData.DATA.eLevel == EnumController.ITEM_LEVEL.level_7)
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                    return;
                }



                int _price = _currentTrack.DefenseData.GetPriceToUpgrade(_currentTrack.DefenseData.DATA.eLevel);
                if (DataController.Instance.playerData.Gem >= _price)
                {
                    DataController.Instance.playerData.Gem -= _price;
                    EventController.OnUpdatedBoardInvoke();//event

                    //content
                    _currentTrack.DefenseData.DATA.eLevel++;
                    DataController.Instance.playerData.TakeDefense(_currentTrack.DefenseData.DATA.eDefense).eLevel = _currentTrack.DefenseData.DATA.eLevel;

                    ViewTrack(_currentTrack);
                    SoundController.Instance.Play(SoundController.SOUND.ui_upgrade);//sound
                }
                else
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                    UIController.Instance.PopUpShow(UIController.POP_UP.note);
                    Note.AssignNote(Note.NOTE.no_gem.ToString());

                }



                ButtonStates(_currentTrack);
            }


            //FIX
            private void Fix()
            {
                //for tutorial
                if (TutorialController.Instance)
                {
                    if (!TutorialController.Instance.IsRightInput()) return;
                }

            }
        }


        //MAIN PANEL: SUPPORT
        [System.Serializable]
        public class SupportPanel
        {
            [FormerlySerializedAs("LIST_TRACK")] [SerializeField] private List<Support> _trackList;

            //=================================
            
            [Space(30)]
            [FormerlySerializedAs("CURRENT_TRACK")][SerializeField] private Support _currentTrack;
            [FormerlySerializedAs("buBuy")] [SerializeField] private Button _buyButton;
            [FormerlySerializedAs("buUpgrade")] [SerializeField] private Button _upgradeButton;

            [FormerlySerializedAs("txtName")] [SerializeField] private Text _nameText;
            [FormerlySerializedAs("txtValue")] [SerializeField] private Text _valueText;
            [FormerlySerializedAs("txtContent")] [SerializeField] private Text _contentText;
            [FormerlySerializedAs("txtPriceToBuy")] [SerializeField] private Text _priceText;
            [FormerlySerializedAs("imaMainIcon")] [SerializeField] private Image _iconImage;

            public void Construct()
            {
                ViewTrack(_trackList[0]);
            }
            public void ViewTrack(Support trackSupport)
            {
                //STatus
                int _total = _trackList.Count;
                for (int i = 0; i < _total; i++)
                {
                    if (_trackList[i] == trackSupport)
                    {
                        _trackList[i].GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        _trackList[i].GetComponent<Image>().color = Color.gray;
                    }
                }


                _currentTrack = trackSupport;
                _upgradeButton.onClick.RemoveAllListeners();
                _upgradeButton.onClick.AddListener(() => Upgrade());
                _buyButton.onClick.RemoveAllListeners();
                _buyButton.onClick.AddListener(() => Buy());

                if (_currentTrack == null) return;
                if (_currentTrack.SupportData == null) return;


                //show info
                _nameText.text = _currentTrack.SupportData.strName;
                _valueText.text = _currentTrack.SupportData.DATA.iCurrentValue + "+1";
                _contentText.text = _currentTrack.SupportData.strContent;
                _priceText.text = _currentTrack.SupportData.iPrice.ToString();
                _iconImage.sprite = _currentTrack.SupportData.sprIcon;

                ButtonStates(_currentTrack);
            }

            //STATE OF BUTTONS (Upgrade + buy amoom)
            private void ButtonStates(Support _track)
            {


                if (_track.SupportData.DATA.iCurrentValue >= _track.SupportData.iMaxValue)
                {
                    _buyButton.GetComponentInChildren<Text>().text = "MAX";
                    _buyButton.image.color = Color.gray;
                    _valueText.text = _currentTrack.SupportData.DATA.iCurrentValue.ToString();
                }
                else
                {
                    _buyButton.GetComponentInChildren<Text>().text = "BUY";
                    _buyButton.image.color = Color.white;
                }

            }

            //UPGRADE
            public void Upgrade()
            {
                //for tutorial
                if (TutorialController.Instance)
                {
                    if (!TutorialController.Instance.IsRightInput()) return;
                }

                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
            }

            //BUY
            private void Buy()
            {
                //for tutorial
                if (TutorialController.Instance)
                {
                    if (!TutorialController.Instance.IsRightInput()) return;
                }

                if (_currentTrack.SupportData.DATA.iCurrentValue >= _currentTrack.SupportData.iMaxValue)
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                    return;
                }


                int _price = _currentTrack.SupportData.iPrice;
                if (DataController.Instance.playerData.Gem >= _price)
                {

                    DataController.Instance.playerData.Gem -= _price;
                    EventController.OnUpdatedBoardInvoke();//event 

                    //content
                    _currentTrack.SupportData.DATA.iCurrentValue++;
                    _valueText.text = _currentTrack.SupportData.DATA.iCurrentValue + "+1";
                    _currentTrack.ShowData();
                    SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
               
                }
                else
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                    UIController.Instance.PopUpShow(UIController.POP_UP.note);
                    Note.AssignNote(Note.NOTE.no_gem.ToString());
                }
                ButtonStates(_currentTrack);
            }
        }

        //WEAPON EQUIPED CLASS
        [System.Serializable]
        public class WeaponPicked
        {
            //WEAPON
            [System.Serializable]
            public class SubWeapon
            {
                
                [FormerlySerializedAs("_gundata")] [SerializeField] private GunData _gunData;
                [SerializeField] private Button _button;
                [FormerlySerializedAs("txtAmmo")] [SerializeField] private Text _ammoText;

                public GunData GunData => _gunData;
                public Text AmmoText => _ammoText;
                private void Remove()
                {
                    //for tutorial
                    if (TutorialController.Instance)
                    {
                        if (!TutorialController.Instance.IsRightInput()) return;
                    }

                    if (_gunData.DATA.bIsDefaultGun) return;

                    EventController.OnUnEquipedWeaponInvoke(_gunData);//event
                    _gunData = null;
                    _button.transform.Find("Icon").GetComponent<Image>().color = Color.white * 0.0f;
                    _button.transform.Find("Close").GetComponent<Image>().color = Color.white * 0.0f;
                    _ammoText.text = "";
                    SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                }

                public void Construct(GunData _gun)
                {
                    _gunData = _gun;

                    if (_gun)
                    {
                        Image _gunIcon = _button.transform.Find("Icon").GetComponent<Image>();
                        _gunIcon.color = Color.white * 1.0f;
                        _gunIcon.sprite = _gunData.sprIcon;
                        _button.transform.Find("Close").GetComponent<Image>().color = Color.white;
                        _ammoText.text = _gun.DATA.iCurrentAmmo.ToString();
                    }
                    else
                    {
                        _button.transform.Find("Icon").GetComponent<Image>().color =
                            Color.white * 0.0f;
                        _button.transform.Find("Close").GetComponent<Image>().color =
                            Color.white * 0.0f;
                        _ammoText.text = "";
                    }

                    if (_gunData.DATA.bIsDefaultGun)
                    {
                        _button.transform.Find("Close").GetComponent<Image>().color = Color.white * 0.0f;
                        _ammoText.text = "";
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
                    if (LIST_BUTTON_WEAPON_EQUITPED[i].GunData == _gun)
                        return LIST_BUTTON_WEAPON_EQUITPED[i];
                }
                return null;
            }
            public void AddTakenWeapon(GunData gundata)
            {
                if (GetSubWeapon(gundata) != null)
                {

                    return;
                }

                SoundController.Instance.Play(SoundController.SOUND.ui_equiped);//sound
                //---------------------
                int _totalTrack = LIST_BUTTON_WEAPON_EQUITPED.Count;
                for (int i = 0; i < _totalTrack; i++)
                {
                    if (LIST_BUTTON_WEAPON_EQUITPED[i].GunData == null)
                    {
                        LIST_BUTTON_WEAPON_EQUITPED[i].Construct(gundata);
                        EventController.OnoEquipedWeaponInvoke(gundata);//event

                        return;
                    }
                }
            }
        }


        //WEAPON EQUIPED CLASS
        [System.Serializable]
        public class DefencePicked
        {

            //DEFENSE
            [System.Serializable]
            public class SubDefense
            {
                [SerializeField] private DefenseData _data;
                [SerializeField] private Button _button;
                public DefenseData DefenseData => _data;
                
                private void Remove()
                {
                    if (_data == null)
                    {
                        SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                        return;
                    }
                    if (_data.DATA.bDefault)
                    {
                        SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                        return;
                    }

                    _button.image.sprite = Instance._defencePicked._emptySprite;
                    EventController.OnRemoveDefenseInvoke(_data);//event
                    _data = null;
                    _button.transform.Find("Icon").GetComponent<Image>().color = Color.white * 0.0f;
                    _button.transform.Find("Close").GetComponent<Image>().color = Color.white * 0.0f;
                    SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                }

                public void Construct(DefenseData _defense)
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
            
            [FormerlySerializedAs("sprEmpty")] [SerializeField] private Sprite _emptySprite;
            [FormerlySerializedAs("LIST_BUTTON_DEFENSE_EQUITPED")] [SerializeField] private List<SubDefense> _listOfPickedDefence;
            public SubDefense GetSubWeapon(DefenseData _defense)
            {
                int total = _listOfPickedDefence.Count;
                for (int i = 0; i < total; i++)
                {
                    if (_listOfPickedDefence[i].DefenseData == _defense)
                        return _listOfPickedDefence[i];
                }
                return null;
            }
            public void AddTakenDefense(DefenseData _defense)
            {
                if (GetSubWeapon(_defense) != null)
                    return;


                //---------------------
                int totalTrack = _listOfPickedDefence.Count;
                for (int i = 0; i < totalTrack; i++)
                {
                    if (_listOfPickedDefence[i].DefenseData == null)
                    {
                        _listOfPickedDefence[i].Construct(_defense);
                        return;
                    }
                }


            }
        }



        //==============================================================================================//
         
        public static WeaponController Instance;
        [Space(30)]
        [FormerlySerializedAs("LIST_PANEL_MANAGER")] [SerializeField] private List<PanelManager> _listPanelManager;
        private void Construct()
        {
            int total = _listPanelManager.Count;
            for (int i = 0; i < total; i++)
            {
                _listPanelManager[i].Construct();
            }
            _listPanelManager[0].Assign();

            //for tutorial
            if (TutorialController.Instance && !TutorialController.Instance.GetTutorial(TutorialController.TUTORIAL.weapon)._isCompleted)
            {
                _listPanelManager[1].PanelButton.enabled = false;
                _listPanelManager[2].PanelButton.enabled = false;
            }
        }


        [FormerlySerializedAs("m_MainPanelWeapon")] [Space(30)]
        [SerializeField] private WeaponPanel _weaponPanel;
        [FormerlySerializedAs("m_MainPanelDefense")] [SerializeField] private DefencePanel _defencePanel;
        [FormerlySerializedAs("m_MainPanelSupport")] [SerializeField] private SupportPanel _supportPanel;

        [FormerlySerializedAs("m_mainEquitpedWeapon")] [Space(20)]
        [SerializeField] private WeaponPicked _weaponPicked;
        [FormerlySerializedAs("m_mainEquitpedDefense")] [SerializeField] private DefencePicked _defencePicked;

        public DefencePanel defencePanel => _defencePanel;
        public DefencePicked defencePicked => _defencePicked;
        public SupportPanel supportPanel => _supportPanel;
        public WeaponPanel weaponPanel => _weaponPanel;
        public WeaponPicked weaponPicked => _weaponPicked;


        
        [Space(30)]
        [FormerlySerializedAs("buBack")][SerializeField] private Button _backButton;
        [FormerlySerializedAs("buStart")] [SerializeField] private Button _startButton;
        [FormerlySerializedAs("LIST_STAR_SPRITE_FOR_WEAPON_LEVEL")] [SerializeField] private List<Sprite> _listStarSpriteForWeaponLevel;


        private void Awake()
        {
            if (Instance == null) Instance = this;

            UIController.Instance.SetCameraPopup(Camera.main);//set camera
        }


        private void Start()
        {

            Construct();

            _backButton.onClick.AddListener(() => ButtonInit(_backButton));
            _startButton.onClick.AddListener(() => ButtonInit(_startButton));


            _weaponPanel.Construct();
            _defencePanel.Construct();
            _supportPanel.Construct();
            EqupList();


        }
        
        private void ButtonInit(Button button)
        {   //for tutorial
            if (TutorialController.Instance)
            {
                if (!TutorialController.Instance.IsRightInput()) return;
            }


            if (button == _backButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_back);//sound
                UIController.Instance.LoadScene(UIController.SCENE.LevelSelection);
            }
            else if (button == _startButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                SoundController.Instance.Play(SoundController.SOUND.sfx_zombie_gruzz_boss);
                UIController.Instance.LoadScene(UIController.SCENE.Gameplay);
            }
        }


        private void EqupList()
        {

            int total = MANAGERS.WeaponController.Instance.equipedWeaponList.Count;
            for (int i = 0; i < total; i++)
            {
                _weaponPicked.AddTakenWeapon(MANAGERS.WeaponController.Instance.equipedWeaponList[i]);
            }

            total = MANAGERS.WeaponController.Instance._defenceList.Count;
            for (int i = 0; i < total; i++)
            {
                if (MANAGERS.WeaponController.Instance._defenceList[i].DATA.bEquiped)
                    _defencePicked.AddTakenDefense(MANAGERS.WeaponController.Instance._defenceList[i]);
            }

        }


        private void OnDisable()
        {
            DataController.Instance.SaveData();//save
        }
    }
}
