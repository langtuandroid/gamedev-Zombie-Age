using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace MANAGERS
{
    public class DataController : MonoBehaviour
    {
        [Inject] private TutorialController _tutorialController;
        [Inject] private UIController _uiController;
        [Inject] private UpgradeController _upgradeController;
        [Inject] private DiContainer _diContainer;
        [Inject] private WeaponController _weaponController;
        
        public static DataController Instance;
        
        private void Awake()
        {
            Instance = this;
        }
        public enum Mode
        {
            Release,
            Debug,
        }
        
        #region DATA OF THE PLAYER
        [System.Serializable]
        public class PlayerData
        {
            [SerializeField] private EnumController.DIFFICUFT CURRENT_DIFFICUFT;
            public EnumController.DIFFICUFT Difficuft
            {
                get => CURRENT_DIFFICUFT;
                set => CURRENT_DIFFICUFT = value;
            }
            [FormerlySerializedAs("_currentLevel")] [FormerlySerializedAs("iCurrentLevel")] public int CurrentLevel;
            [FormerlySerializedAs("iGem")] public int Gem = 200;

            #region STAR
            
            [FormerlySerializedAs("LIST_STAR")] [SerializeField] private List<int> _starList = new List<int>();
            public List<int> StarList => _starList;
            
            public int NumOfStars(int _level)
            {
                if (_level < _starList.Count)
                {
                    return _starList[_level];
                }
                return -1;

            }
            
            public void SetStar(int _level, int _star)
            {
                if (_level < _starList.Count)
                {
                    if (NumOfStars(_level) >= _star) return;
                    _starList[_level] = _star;
                }
                else
                {
                    _starList.Add(_star);
                }
            }
            
            public int GetAllStars()
            {
                int _sum = 0;
                int _total = _starList.Count;
                if (_total == 0) return 0;


                for (int i = 0; i < _total; i++)
                {
                    if (_starList[i] > 0)
                        _sum += _starList[i];
                }
                return _sum;
            }
            #endregion
            
            public int CalculatePlayerLevel()
            {
                return _starList.Count;
            }

            #region CHECK UNLOCK LEVEL
            public bool IsLevelUnlocked(EnumController.DIFFICUFT _difficuft, int _level)
            {
                switch (_difficuft)
                {
                    case EnumController.DIFFICUFT.easy:
                        if (_level == 0) return true;
                        if (NumOfStars(_level - 1) > 0) return true;
                        break;
                    case EnumController.DIFFICUFT.normal:
                        if (NumOfStars(_level) == 1) return true;
                        break;
                    case EnumController.DIFFICUFT.nightmare:
                        if (NumOfStars(_level) == 2) return true;
                        break;
                }


                return false;
            }
            #endregion

             

            #region UNLOCK GUN           
            [FormerlySerializedAs("LIST_WEAPON")] public List<GunData.WeData> _weaponList = new ();
            public GunData.WeData TakeWeapon(EnumController.WEAPON _weapon)
            {
                int _total = _weaponList.Count;
                for (int i = 0; i < _total; i++)
                {
                    if (_weaponList[i].eWeapon == _weapon)
                        return _weaponList[i];
                }
                return null;
            }

            #endregion




            #region DEFENSE
            [FormerlySerializedAs("LIST_DEFENSE")] public List<DefenseData.DeData> _defenseList = new ();
            public DefenseData.DeData TakeDefense(EnumController.DEFENSE _defense)
            {
                int _total = _defenseList.Count;
                for (int i = 0; i < _total; i++)
                {
                    if (_defenseList[i].eDefense == _defense)
                        return _defenseList[i];
                }
                return null;
            }
            #endregion




            #region UPGRADE
            [FormerlySerializedAs("LIST_UPGRADE")] public List<UpgradeData.Updata> _upgradeList = new ();
            public UpgradeData.Updata TakeUpgarde(EnumController.UpgradeType _upgrade)
            {
                int _total = _upgradeList.Count;
                for (int i = 0; i < _total; i++)
                {
                    if (_upgradeList[i].eUpgrade == _upgrade)
                        return _upgradeList[i];
                }
                return null;
            }
            #endregion



            #region SUPPORT
            [FormerlySerializedAs("LIST_SUPPORT")] public List<SupportData.SuData> _supportList = new ();
            public SupportData.SuData TakeSupport(EnumController.SUPPORT _support)
            {
                int _total = _supportList.Count;

                for (int i = 0; i < _total; i++)
                {
                    if (_supportList[i]._support == _support)
                        return _supportList[i];
                }
                return null;
            }
            #endregion


            #region CHECK IN

            [FormerlySerializedAs("iCurrentDay")] public int _day = -1;
            [FormerlySerializedAs("iCheckInDay")] public int _checkDay;
            [FormerlySerializedAs("iCheckInMonth")] public int checkDay;
            [FormerlySerializedAs("iCheckInYear")] public int checkYear;

            public bool CheckInReady()
            {
                if (_day > Instance._revardList.Count) return false;

                if (ThisYear() > checkYear)
                {
                    _day++;
                    _checkDay = ThisDay();
                    checkDay = ThisMonth();
                    checkYear = ThisYear();
                    return true;
                }

                if (ThisMonth() > checkDay)
                {
                    _day++;
                    _checkDay = ThisDay();
                    checkDay = ThisMonth();
                    checkYear = ThisYear();
                    return true;
                }

                if (ThisDay() > _checkDay)
                {
                    _day++;
                    _checkDay = ThisDay();
                    checkDay = ThisMonth();
                    checkYear = ThisYear();
                    return true;
                }


                return false;
            }


            private int ThisDay()
            {
                System.DateTime _today = System.DateTime.Today;
                return _today.Day;
            }
            private int ThisMonth()
            {
                System.DateTime _today = System.DateTime.Today;
                return _today.Month;
            }
            private int ThisYear()
            {
                System.DateTime _today = System.DateTime.Today;
                return _today.Year;
            }
            #endregion
        }
        #endregion
        
        [FormerlySerializedAs("eMode")] [SerializeField] private Mode _mode;
       
        
        [FormerlySerializedAs("PRICE_CONFIG")] [Space(20)]
        [SerializeField] private PriceUnitConfig _priceData;
        [FormerlySerializedAs("prefabTUTORIAL_SYSTEM")] [SerializeField] private GameObject _tutorialSystemPrefab;


         [Tooltip("Config thu cong")]
         [FormerlySerializedAs("TOTAL_LEVEL_IN_GAME")] [SerializeField] private int _levelsTotal;
         
        [Space(30)]
        private PlayerData _playerData;
        public PlayerData playerData
        {
            get => _playerData;
            set => _playerData = value;
        }
        
        private string _pathOfPlayerData;
        public Mode mode => _mode;
        public PriceUnitConfig PriceData => _priceData;
        public int LevelsTotal => _levelsTotal;
        
        #region REWARD CONFIG FILE
        
        [Header("***CONFIG IAP FILES***")]
        [Space(30)]
        [FormerlySerializedAs("LIST_REWARD")] [SerializeField] private List<RewardData> _revardList;
        public RewardData GetReward(EnumController.REWARD _reward)
        {
            int _total = _revardList.Count;
            for (int i = 0; i < _total; i++)
            {
                if (_revardList[i].eReward == _reward)
                {
                    return _revardList[i];
                }
            }
            return null;
        }
        #endregion
        
        private void Start()
        {
            Construct();
        }

        private void Construct()
        {
            _playerData = new PlayerData();
            _diContainer.Inject(_playerData);

#if UNITY_EDITOR
            _pathOfPlayerData = Application.dataPath + "/Resources/Data/PlayerData.xml";
#elif UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE || UNITY_STANDALONE_WIN || UNITY_WEBGL
        _pathOfPlayerData = Application.persistentDataPath + "/PlayerData.xml";
#endif

            TakePlayerData();

            _weaponController.Construct();
            _upgradeController.Construct();


            if (playerData.GetAllStars() == 0)
            {
                //Instantiate(_tutorialSystemPrefab, new Vector3(0, 0, -2.0f), Quaternion.identity);//TUTORIAL SYSTEM  
            }
                

        }

        
        private void TakePlayerData()
        {
            if (PlayerPrefs.GetInt("firsttime") == 0)
            {
                PlayerPrefs.SetInt("firsttime", 1);
                PlayerPrefs.Save();
                if (File.Exists(_pathOfPlayerData))
                {
                    File.Delete(_pathOfPlayerData);
                }

            }
            
            Debug.Log(_pathOfPlayerData);
            if (!File.Exists(_pathOfPlayerData))
            {
                SaveData();
            }
            else
            {
                _playerData = DestroyPlayerData();
            }

        }
        private PlayerData DestroyPlayerData()
        {
            if (File.Exists(_pathOfPlayerData))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
                StringReader reader = new StringReader(EncryptionController.Decrypt(File.ReadAllText(_pathOfPlayerData)));
                PlayerData deserialized = (PlayerData)serializer.Deserialize(reader);
                reader.Close();
                Debug.Log("SERIALZER: GET PLAYER DATA DONE!");
                return deserialized;

            }
            else
            {
                Debug.Log("SERIALZER: NO FILE EXITS");
                return null;
            }
        }

        //SAVE DATA PLAYER
        public void SaveData()
        {
            if (_pathOfPlayerData.Equals("")) return;
            
            XmlSerializer serialzer = new XmlSerializer(typeof(PlayerData));

            StreamWriter writer = new StreamWriter(_pathOfPlayerData);

            serialzer.Serialize(writer.BaseStream, playerData);
            writer.Close();
            print("SERIALZER: SAVE DONE!!!!");

            //ma hoa
            string _data = File.ReadAllText(_pathOfPlayerData);
            _data = EncryptionController.Encrypt(_data);
            byte[] _byte = System.Text.Encoding.ASCII.GetBytes(_data);
            System.IO.File.WriteAllBytes(_pathOfPlayerData, _byte);
        }


        //Reset game
        public void ResetAll()
        {
            if (_tutorialController)
                Destroy(_tutorialController.gameObject);

            PlayerPrefs.DeleteAll();
            if (System.IO.File.Exists(_pathOfPlayerData))
            {
                System.IO.File.Delete(_pathOfPlayerData);
            }

            Construct();
            _uiController.LoadScene(UIController.SCENE.Menu);
        }

        public int GetStars()
        {
            switch (playerData.Difficuft)
            {
                case EnumController.DIFFICUFT.easy:
                    return 1;
                case EnumController.DIFFICUFT.normal:
                    return 2;
                case EnumController.DIFFICUFT.nightmare:
                    return 3;
            }
            return -1;
        }


        private void OnDestroy()
        {
            SaveData();
        }
    }
}
