using System.Collections;
using System.Collections.Generic;
using SCREENS;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MANAGERS
{
    public class TheUiManager : MonoBehaviour
    {


        #region POP UP
        public enum POP_UP
        {
            setting,
            pause,
            note,
            about_us,
            shop,
            victory,
            gameover,
            rate,
            video_reward,
            loading,
            reward,
            check_in,
        }


        [System.Serializable]
        public class PopUp
        {
            public TheUiManager.POP_UP ePopup;
            public GameObject objPopup;
        }


        public static TheUiManager Instance;
        [SerializeField] GameObject m_BlackCamera;
        [SerializeField] Canvas m_CanvasOfPopup;

        public List<PopUp> LIST_POP_UP;
        private int iTotalPopup;
        // Start is called before the first frame update
        void Awake()
        {
            if (Instance == null)
                Instance = this;

            else
                Destroy(gameObject);

            DontDestroyOnLoad(this.gameObject);

            iTotalPopup = LIST_POP_UP.Count;
        }


        //SHOW POPUP
        public void ShowPopup(POP_UP _popup)
        {
            if (CURRENT_SCENE != SCENE.Gameplay)
            {
                if (_popup == POP_UP.gameover) return;
                if (_popup == POP_UP.victory) return;
                if (_popup == POP_UP.pause) return;
            }


            //---------------------------------------
            LIST_POP_UP[(int)_popup].objPopup.SetActive(true);

            if (isShowing(POP_UP.loading))
                LIST_POP_UP[(int)_popup].objPopup.transform.SetAsLastSibling();
            else
                LIST_POP_UP[(int)_popup].objPopup.transform.SetSiblingIndex(iTotalPopup - 1);


            if (_popup != POP_UP.loading)
                TheEventManager.PostEvent_OnShowPopup(_popup);//event
        }
        public void ShowPopup(POP_UP _popup, float _timedelay)
        {
            StartCoroutine(IeShowPop(_popup, _timedelay));
        }
        private IEnumerator IeShowPop(POP_UP _popup, float _timedelay)
        {
            yield return new WaitForSeconds(_timedelay);
            ShowPopup(_popup);
        }



        //CANCEL ALL COROUTINE
        //public void StopAllCoroutine()
        //{
        //    StopAllCoroutines();
        //}


        //HIDE POPUP
        public void HidePopup(POP_UP _popup)
        {


            LIST_POP_UP[(int)_popup].objPopup.SetActive(false);

            if (_popup != POP_UP.loading)
                TheEventManager.PostEvent_OnHidePopup(_popup);//event
        }


        //HIDE ALL
        public void HideAll()
        {
            for (int i = 0; i < iTotalPopup; i++)
            {
                if (LIST_POP_UP[i].ePopup != POP_UP.loading)
                    LIST_POP_UP[i].objPopup.SetActive(false);
            }
        }


        //IS SHOWING
        public bool isShowing()
        {
            for (int i = 0; i < iTotalPopup; i++)
            {
                if (LIST_POP_UP[i].ePopup != POP_UP.loading && LIST_POP_UP[i].objPopup.activeInHierarchy) return true;
            }
            return false;
        }

        public bool isShowing(POP_UP _poup)
        {
            if (LIST_POP_UP[(int)_poup].objPopup.activeInHierarchy) return true;
            return false;
        }


        #endregion


        #region SCENE
        public enum SCENE
        {
            Menu,
            Gameplay,
            Weapon,
            LevelSelection,
            Upgrade,
        }

        public SCENE CURRENT_SCENE;
        private bool bLoadingScene = false;
        public void LoadScene(SCENE _scene)
        {
            if (bLoadingScene) return;
            bLoadingScene = true;
            CURRENT_SCENE = _scene;

            StartCoroutine(IeLoadScene(_scene));
        }

        private IEnumerator IeLoadScene(SCENE _scene)
        {
            ShowPopup(POP_UP.loading);
            Loading.Instance.Setup(Loading.FADE.fade_in);//loading 

            Time.timeScale = 1;
            yield return new WaitForSecondsRealtime(0.5f);
            HideAll();

            SceneManager.LoadScene(_scene.ToString(), LoadSceneMode.Single);
            yield return new WaitForSecondsRealtime(0.1f);



            ShowPopup(POP_UP.loading);
            Loading.Instance.Setup(Loading.FADE.face_out);//loading 
            bLoadingScene = false;

            //event
            TheEventManager.PostEvent_OnStartNewScene();
            //---------------
            StopAllCoroutines();
        }


        #endregion


        public void LoadLink(string _link)
        {
            Application.OpenURL(_link);
        }

        //EMAIL
        public void ReportEmail()
        {
            string email = TheDataManager.Instance.GAME_INFO.strEmailReport;
            string _platform = "[Android]";

#if UNITY_IOS || UNITY_IPHONE
            _platform = "[iOS]";
#elif UNITY_EDITOR
        _platform = "[Editor]";
#endif

            string subject = _platform + "-" + TheDataManager.Instance.GAME_INFO.NAME_GAME + "[-ver" + Application.version + "]  Bug Report";
            string body = "Hi Bamgru, \n ...";
            Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
        }

        //CAMẺA
        public void SetCameraForPopupCanvas(Camera _cam)
        {
            m_CanvasOfPopup.renderMode = RenderMode.ScreenSpaceCamera;
            m_CanvasOfPopup.worldCamera = _cam;
            m_CanvasOfPopup.sortingOrder = 200;

            float _screenWidth = Screen.width;
            float _screenHeight = Screen.height;
            float ratioOfScreen = _screenWidth / _screenHeight;
            float targetRotio = 0;


            if (ratioOfScreen > (16f / 9f)) //Ty le 21:9
            {
                targetRotio = ((_screenHeight * 16) / (9)) / _screenWidth;
                _cam.rect = new Rect((1 - targetRotio) / 2.0f, 0, targetRotio, 1);

                CreateBackgroundCamera();

            }
            else if (ratioOfScreen < (16 / 9f)) // ty le 4:3
            {
                targetRotio = ((_screenWidth * 9) / (16)) / _screenHeight;
                _cam.rect = new Rect(0, (1 - targetRotio) / 2.0f, 1, targetRotio);

                CreateBackgroundCamera();

            }
            else // ty le 16:9
            {
                _cam.rect = new Rect(0, 0, 1, 1);
            }

        }
        private void CreateBackgroundCamera()
        {
            if (!GameObject.Find("BlackCamera"))
            {
                GameObject _newCam = Instantiate(m_BlackCamera);
                _newCam.name = "BlackCamera";
            }
        }



    }
}