using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MANAGERS
{
    public class ObjectPoolController : MonoBehaviour
    {
        #region Object on Pooler
        [System.Serializable]
        public class ObjectPool
        {
            [FormerlySerializedAs("ePoollingObject")] [SerializeField] private EnumController.POOLING_OBJECT _poollingType;
            [FormerlySerializedAs("objPrefab")] [SerializeField] private GameObject _prefab;
            [FormerlySerializedAs("LIST_POOL")] [SerializeField] private List<GameObject> _poolList;
            private int _totalObjects;

            public EnumController.POOLING_OBJECT poolingType => _poollingType;

            public void Construct()
            {
                _totalObjects = _poolList.Count;
                GameObject _temp;
                for (int i = 0; i < _totalObjects; i++)
                {
                    _temp = Instantiate(_prefab, Vector2.one * 100, Quaternion.identity);
                    _temp.SetActive(false);
                    _poolList[i] = _temp;
                }
            }


            public GameObject Get()
            {
                for (int i = 0; i < _totalObjects; i++)
                {
                    if (!_poolList[i].activeInHierarchy)
                        return _poolList[i];
                }
                //---------
                GameObject _temp = Instantiate(_prefab, Vector2.one * 100, Quaternion.identity);
                _temp.SetActive(false);
                _poolList.Add(_temp);
                _totalObjects++;
                return _temp;
            }
        }
        #endregion


        public static ObjectPoolController Instance;
        [FormerlySerializedAs("LIST_POOLING_OBECJT")] [SerializeField] private List<ObjectPool> _poolList;
        private int _totalPools;


        [Space(30)] 
        [FormerlySerializedAs("prefabBigBlood")] public GameObject _bigBloodPrefab;
        [FormerlySerializedAs("prefabBloodBoss")] public GameObject _bossBloodPrefab;
        [FormerlySerializedAs("prefabExplosionBoss")] public GameObject _explosionBossPrefab;
        [FormerlySerializedAs("preabFireOfHome_level1")] public GameObject _fireOnHomeLevel1;
        [FormerlySerializedAs("preabFireOfHome_level2")] public GameObject _fireOnHomeLevel2;
        [FormerlySerializedAs("preabFireOfHome_level3")] public GameObject _fireOnHomeLevel3;

        
        [Header("Effects")]
        [Space(30)]
        [FormerlySerializedAs("prefabWhitOfBigBom")] public GameObject _bigExplosionPrefab;
        
        void Awake()
        {
            if (Instance == null)
                Instance = this;
            _totalPools = _poolList.Count;
            for (int i = 0; i < _totalPools; i++)
            {
                _poolList[i].Construct();
            }
        }
        
        public ObjectPool GetObjectPool(EnumController.POOLING_OBJECT _objectPooling)
        {
            for (int i = 0; i < _totalPools; i++)
            {
                if(_poolList[i].poolingType==_objectPooling)
                {
                    return _poolList[i];
                }
            }
            return null;
        }



    
    }
}
