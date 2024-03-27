using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using MODULES.Scriptobjectable;
using UnityEngine;

namespace MANAGERS
{
    public class TheDataManager : MonoBehaviour
    {
        public enum MODE
        {
            Release,
            Debug,
        }


        #region DATA OF THE PLAYER
        [System.Serializable]
        public class ThePlayerData
        {
            public TheEnumManager.DIFFICUFT CURRENT_DIFFICUFT;


            // FISRT GAME
            public bool bFirstGame;//lan dau choi

            //HP
            public int iCurrentLevel;
            public int iGem = 200;



            #region STAR


            public List<int> LIST_STAR = new List<int>();



            //Get star
            public int GetStar(int _level)
            {
                if (_level < LIST_STAR.Count)
                {
                    return LIST_STAR[_level];
                }
                return -1;

            }


            //Set star
            public void SetStar(int _level, int _star)
            {
                if (_level < LIST_STAR.Count)
                {
                    if (GetStar(_level) >= _star) return;
                    LIST_STAR[_level] = _star;
                }
                else
                {
                    LIST_STAR.Add(_star);
                }
            }

            //count total star
            public int GetTotalStar()
            {
                int _sum = 0;
                int _total = LIST_STAR.Count;
                if (_total == 0) return 0;


                for (int i = 0; i < _total; i++)
                {
                    if (LIST_STAR[i] > 0)
                        _sum += LIST_STAR[i];
                }
                return _sum;
            }
            #endregion

            //Count total level with star
            public int GetTotalPlayerLevel()//tong level nguoi choi da choi.
            {

                return LIST_STAR.Count;

            }


            #region CHECK UNLOCK LEVEL
            public bool IsUnlockLevel(TheEnumManager.DIFFICUFT _difficuft, int _level)
            {
                switch (_difficuft)
                {
                    case TheEnumManager.DIFFICUFT.easy:
                        if (_level == 0) return true;
                        if (GetStar(_level - 1) > 0) return true;
                        break;
                    case TheEnumManager.DIFFICUFT.normal:
                        if (GetStar(_level) == 1) return true;
                        break;
                    case TheEnumManager.DIFFICUFT.nightmare:
                        if (GetStar(_level) == 2) return true;
                        break;
                }


                return false;
            }
            #endregion



            #region UNLOCK GUN           
            public List<GunData.WeData> LIST_WEAPON = new List<GunData.WeData>();
            public GunData.WeData GetWeapon(TheEnumManager.WEAPON _weapon)
            {
                int _total = LIST_WEAPON.Count;
                for (int i = 0; i < _total; i++)
                {
                    if (LIST_WEAPON[i].eWeapon == _weapon)
                        return LIST_WEAPON[i];
                }
                return null;
            }

            #endregion




            #region DEFENSE
            public List<DefenseData.DeData> LIST_DEFENSE = new List<DefenseData.DeData>();
            public DefenseData.DeData GetDefense(TheEnumManager.DEFENSE _defense)
            {
                int _total = LIST_DEFENSE.Count;
                for (int i = 0; i < _total; i++)
                {
                    if (LIST_DEFENSE[i].eDefense == _defense)
                        return LIST_DEFENSE[i];
                }
                return null;
            }
            #endregion




            #region UPGRADE
            public List<UpgradeData.Updata> LIST_UPGRADE = new List<UpgradeData.Updata>();
            public UpgradeData.Updata GetUpgarde(TheEnumManager.KIND_OF_UPGRADE _upgrade)
            {
                int _total = LIST_UPGRADE.Count;
                for (int i = 0; i < _total; i++)
                {
                    if (LIST_UPGRADE[i].eUpgrade == _upgrade)
                        return LIST_UPGRADE[i];
                }
                return null;
            }
            #endregion



            #region SUPPORT
            public List<SupportData.SuData> LIST_SUPPORT = new List<SupportData.SuData>();
            public SupportData.SuData GetSupport(TheEnumManager.SUPPORT _support)
            {
                int _total = LIST_SUPPORT.Count;

                for (int i = 0; i < _total; i++)
                {
                    if (LIST_SUPPORT[i]._support == _support)
                        return LIST_SUPPORT[i];
                }
                return null;
            }
            #endregion


            #region CHECK IN

            public int iCurrentDay = -1;
            public int iCheckInDay;
            public int iCheckInMonth;
            public int iCheckInYear;

            public bool IsReadyToGetCheckIn()
            {
                if (iCurrentDay > TheDataManager.Instance.LIST_REWARD.Count) return false;

                if (GetCurrentYear() > iCheckInYear)
                {
                    iCurrentDay++;
                    iCheckInDay = GetCurrentDay();
                    iCheckInMonth = GetCurrentMonth();
                    iCheckInYear = GetCurrentYear();
                    return true;
                }

                if (GetCurrentMonth() > iCheckInMonth)
                {
                    iCurrentDay++;
                    iCheckInDay = GetCurrentDay();
                    iCheckInMonth = GetCurrentMonth();
                    iCheckInYear = GetCurrentYear();
                    return true;
                }

                if (GetCurrentDay() > iCheckInDay)
                {
                    iCurrentDay++;
                    iCheckInDay = GetCurrentDay();
                    iCheckInMonth = GetCurrentMonth();
                    iCheckInYear = GetCurrentYear();
                    return true;
                }


                return false;
            }


            private int GetCurrentDay()
            {
                System.DateTime _today = System.DateTime.Today;
                return _today.Day;
            }
            private int GetCurrentMonth()
            {
                System.DateTime _today = System.DateTime.Today;
                return _today.Month;
            }
            private int GetCurrentYear()
            {
                System.DateTime _today = System.DateTime.Today;
                return _today.Year;
            }



            #endregion



        }
        #endregion





        public static TheDataManager Instance;
        public MODE eMode;

        [Space(30)]
        [SerializeField] List<InfoGame> LIST_INFO_GAME;
        private InfoGame _mainGameInfo;
        public InfoGame GAME_INFO
        {
            get
            {
                return _mainGameInfo;
            }
        }
        private InfoGame GetInfoGame(TheEnumManager.PLATFROM _platform)
        {
            foreach (var item in LIST_INFO_GAME)
            {
                if (item.ePlatform == _platform) return item;
            }
            return null;
        }



        [Space(20)]
        public PriceUnitConfig PRICE_CONFIG;
        [SerializeField] GameObject prefabTUTORIAL_SYSTEM;


        [Tooltip("Config thu cong")]
        public int TOTAL_LEVEL_IN_GAME;


        [Space(30)]
        private ThePlayerData thisPlayerData;
        public ThePlayerData THE_DATA_PLAYER
        {
            get
            {
                return thisPlayerData;
            }
            set
            {
                thisPlayerData = value;
            }
        }


        private string PATH_OF_PLAYER_DATA_XML;


        #region IAP CONFIG FILE
        [Header("***CONFIG IAP FILES***")]
        [Space(30)]
        public List<ShopData> LIST_IAP_CONFIG_FILE;
        public ShopData GetIAPConfigFile(TheEnumManager.KIND_OF_IAP _product)
        {
            int _total = LIST_IAP_CONFIG_FILE.Count;
            for (int i = 0; i < _total; i++)
            {
                if (LIST_IAP_CONFIG_FILE[i].eKindOfIap == _product)
                {
                    return LIST_IAP_CONFIG_FILE[i];
                }
            }
            return null;
        }
        #endregion


        #region REWARD CONFIG FILE
        [Header("***CONFIG IAP FILES***")]
        [Space(30)]
        public List<RewardData> LIST_REWARD;
        public RewardData GetReward(TheEnumManager.REWARD _reward)
        {
            int _total = LIST_REWARD.Count;
            for (int i = 0; i < _total; i++)
            {
                if (LIST_REWARD[i].eReward == _reward)
                {
                    return LIST_REWARD[i];
                }
            }
            return null;
        }
        #endregion



        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(this);

#if UNITY_ANDROID
        _mainGameInfo = GetInfoGame(TheEnumManager.PLATFROM.android);
#elif UNITY_IOS || UNITY_IPHONE
            _mainGameInfo = GetInfoGame(TheEnumManager.PLATFROM.ios);
#else
        _mainGameInfo = GetInfoGame(TheEnumManager.PLATFROM.info_default);
#endif

        }


        private void Start()
        {
            Init();
        }


        public void Init()
        {
            //LOAD PLAYER DATA
            thisPlayerData = new ThePlayerData();

#if UNITY_EDITOR
            PATH_OF_PLAYER_DATA_XML = Application.dataPath + "/Resources/Data/PlayerData.xml";
#elif UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE || UNITY_STANDALONE_WIN || UNITY_WEBGL
        PATH_OF_PLAYER_DATA_XML = Application.persistentDataPath + "/PlayerData.xml";
#endif
            //load dat
            LoadDataPlayer();

            TheWeaponManager.Instance.Init();
            TheUpgradeManager.Instance.Init();


            //TUTORIAL
            if (THE_DATA_PLAYER.GetTotalStar() == 0)
                Instantiate(prefabTUTORIAL_SYSTEM, new Vector3(0, 0, -2.0f), Quaternion.identity);//TUTORIAL SYSTEM  

        }



        //READ DATA PLAYER
        private void LoadDataPlayer()
        {

            if (PlayerPrefs.GetInt("firsttime") == 0)
            {
                PlayerPrefs.SetInt("firsttime", 1);
                PlayerPrefs.Save();
                if (File.Exists(PATH_OF_PLAYER_DATA_XML))
                {
                    File.Delete(PATH_OF_PLAYER_DATA_XML);
                }

            }



            Debug.Log(PATH_OF_PLAYER_DATA_XML);
            // THE_DATA_PLAYER = new ThePlayerData();
            if (!File.Exists(PATH_OF_PLAYER_DATA_XML))
            {
                SaveDataPlayer();
            }
            else
            {
                thisPlayerData = DeserialzerPlayerData();
            }

        }
        private ThePlayerData DeserialzerPlayerData()
        {


            if (File.Exists(PATH_OF_PLAYER_DATA_XML))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ThePlayerData));
                StringReader reader = new StringReader(TheEncryptionManager.DecryptData(File.ReadAllText(PATH_OF_PLAYER_DATA_XML)));
                ThePlayerData deserialized = (ThePlayerData)serializer.Deserialize(reader);
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
        public void SaveDataPlayer()
        {
            if (PATH_OF_PLAYER_DATA_XML.Equals("")) return;

            ThePlayerData _thePlayerData = new ThePlayerData();
            XmlSerializer serialzer = new XmlSerializer(typeof(ThePlayerData));

            StreamWriter writer = new StreamWriter(PATH_OF_PLAYER_DATA_XML);

            serialzer.Serialize(writer.BaseStream, THE_DATA_PLAYER);
            writer.Close();
            print("SERIALZER: SAVE DONE!!!!");

            //ma hoa
            string _data = File.ReadAllText(PATH_OF_PLAYER_DATA_XML);
            _data = TheEncryptionManager.EncryptData(_data);
            byte[] _byte = System.Text.Encoding.ASCII.GetBytes(_data);
            System.IO.File.WriteAllBytes(PATH_OF_PLAYER_DATA_XML, _byte);
        }


        //Reset game
        public void ResetGame()
        {
            if (TheTutorialManager.Instance)
                Destroy(TheTutorialManager.Instance.gameObject);

            PlayerPrefs.DeleteAll();
            if (System.IO.File.Exists(PATH_OF_PLAYER_DATA_XML))
            {
                System.IO.File.Delete(PATH_OF_PLAYER_DATA_XML);
            }

            Init();
            TheUiManager.Instance.LoadScene(TheUiManager.SCENE.Menu);
        }



        //TÍNH STAR CHO GAME
        public int CalculateStar()
        {
            switch (TheDataManager.Instance.THE_DATA_PLAYER.CURRENT_DIFFICUFT)
            {
                case TheEnumManager.DIFFICUFT.easy:
                    return 1;
                case TheEnumManager.DIFFICUFT.normal:
                    return 2;
                case TheEnumManager.DIFFICUFT.nightmare:
                    return 3;
            }
            return -1;
        }


        private void OnDestroy()
        {
            SaveDataPlayer();
            // Instance = null;
        }
    }
}
