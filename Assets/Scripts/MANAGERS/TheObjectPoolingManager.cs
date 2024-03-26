using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class TheObjectPoolingManager : MonoBehaviour
{
    #region Object on Pooler
    [System.Serializable]
    public class PoolingObject
    {
        public TheEnumManager.POOLING_OBJECT ePoollingObject;
        public GameObject objPrefab;
        public List<GameObject> LIST_POOL;
        private int iTotalObject;

        public void Init()
        {
            iTotalObject = LIST_POOL.Count;
            GameObject _temp;
            for (int i = 0; i < iTotalObject; i++)
            {
                _temp = Instantiate(objPrefab, Vector2.one * 100, Quaternion.identity);
                _temp.SetActive(false);
                LIST_POOL[i] = _temp;
            }
        }


        public GameObject GetObject()
        {
            for (int i = 0; i < iTotalObject; i++)
            {
                if (!LIST_POOL[i].activeInHierarchy)
                    return LIST_POOL[i];
            }
            //---------
            GameObject _temp = Instantiate(objPrefab, Vector2.one * 100, Quaternion.identity);
            _temp.SetActive(false);
            LIST_POOL.Add(_temp);
            iTotalObject++;
            return _temp;
        }
    }
    #endregion


    public static TheObjectPoolingManager Instance;
    public List<PoolingObject> LIST_POOLING_OBECJT;
    private int iTotalObjectPooling;


    [Space(30)]
    public GameObject prefabBigBlood;//explosion blood for boss
    public GameObject prefabBloodBoss;//explosion blood for boss
    public GameObject prefabExplosionBoss;//explosion blood for boss
    public GameObject preabFireOfHome_level1;
    public GameObject preabFireOfHome_level2;
    public GameObject preabFireOfHome_level3;

    [Header("Effects")]
    [Space(30)]
    public GameObject prefabWhitOfBigBom;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        //Init
        iTotalObjectPooling = LIST_POOLING_OBECJT.Count;
        for (int i = 0; i < iTotalObjectPooling; i++)
        {
            LIST_POOLING_OBECJT[i].Init();
        }
    }


    //Get obecjt Pooling
    public PoolingObject GetObjectPooling(TheEnumManager.POOLING_OBJECT _objectPooling)
    {
        for (int i = 0; i < iTotalObjectPooling; i++)
        {
            if(LIST_POOLING_OBECJT[i].ePoollingObject==_objectPooling)
            {
                return LIST_POOLING_OBECJT[i];
            }
        }
        return null;
    }



    
}
