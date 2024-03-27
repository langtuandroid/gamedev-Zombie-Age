using System.Collections;
using System.Collections.Generic;
using SCREENS;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace MANAGERS
{
    public class UIController : MonoBehaviour
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
            [FormerlySerializedAs("ePopup")] public POP_UP _popUpType;
            [FormerlySerializedAs("objPopup")] public GameObject _popUpObject;
        }


        public static UIController Instance;

        [FormerlySerializedAs("m_BlackCamera")] [SerializeField] private GameObject _vlackCamera;
        [FormerlySerializedAs("m_CanvasOfPopup")] [SerializeField] private Canvas _canvasPopUp;

        [FormerlySerializedAs("LIST_POP_UP")] public List<PopUp> _popUpList;
        private int _totalPopUps;
        void Awake()
        {
            if (Instance == null)
                Instance = this;

            else
                Destroy(gameObject);

            DontDestroyOnLoad(this.gameObject);

            _totalPopUps = _popUpList.Count;
        }

        
        public void PopUpShow(POP_UP _popup)
        {
            if (_cuurentScene != SCENE.Gameplay)
            {
                if (_popup == POP_UP.gameover) return;
                if (_popup == POP_UP.victory) return;
                if (_popup == POP_UP.pause) return;
            }

            _popUpList[(int)_popup]._popUpObject.SetActive(true);

            if (IsShowing(POP_UP.loading))
                _popUpList[(int)_popup]._popUpObject.transform.SetAsLastSibling();
            else
                _popUpList[(int)_popup]._popUpObject.transform.SetSiblingIndex(_totalPopUps - 1);


            if (_popup != POP_UP.loading)
                EventController.OnShowPopupInvoke(_popup);//event
        }
        public void PopUpShow(POP_UP _popup, float _timedelay)
        {
            StartCoroutine(ShowPopRoutine(_popup, _timedelay));
        }
        private IEnumerator ShowPopRoutine(POP_UP _popup, float _timedelay)
        {
            yield return new WaitForSeconds(_timedelay);
            PopUpShow(_popup);
        }
        
        public void HidePopup(POP_UP _popup)
        {
            _popUpList[(int)_popup]._popUpObject.SetActive(false);

            if (_popup != POP_UP.loading)
                EventController.OnHidePopupInvoke(_popup);//event
        }


        private void HideAll()
        {
            for (int i = 0; i < _totalPopUps; i++)
            {
                if (_popUpList[i]._popUpType != POP_UP.loading)
                    _popUpList[i]._popUpObject.SetActive(false);
            }
        }

        
        public bool IsShowing()
        {
            for (int i = 0; i < _totalPopUps; i++)
            {
                if (_popUpList[i]._popUpType != POP_UP.loading && _popUpList[i]._popUpObject.activeInHierarchy) return true;
            }
            return false;
        }

        private bool IsShowing(POP_UP _poup)
        {
            if (_popUpList[(int)_poup]._popUpObject.activeInHierarchy) return true;
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

        [FormerlySerializedAs("CURRENT_SCENE")] public SCENE _cuurentScene;
        private bool _isOnLoadingScene;
        public void LoadScene(SCENE _scene)
        {
            if (_isOnLoadingScene) return;
            _isOnLoadingScene = true;
            _cuurentScene = _scene;

            StartCoroutine(LoadSceneRoutine(_scene));
        }

        private IEnumerator LoadSceneRoutine(SCENE _scene)
        {
            PopUpShow(POP_UP.loading);
            Loading.Instance.Setup(Loading.FADE.fade_in);//loading 

            Time.timeScale = 1;
            yield return new WaitForSecondsRealtime(0.5f);
            HideAll();

            SceneManager.LoadScene(_scene.ToString(), LoadSceneMode.Single);
            yield return new WaitForSecondsRealtime(0.1f);
            
            PopUpShow(POP_UP.loading);
            Loading.Instance.Setup(Loading.FADE.face_out);//loading 
            _isOnLoadingScene = false;
            EventController.OnStartNewSceneInvoke();

            StopAllCoroutines();
        }


        #endregion
        
        public void SetCameraPopup(Camera _cam)
        {
            _canvasPopUp.renderMode = RenderMode.ScreenSpaceCamera;
            _canvasPopUp.worldCamera = _cam;
            _canvasPopUp.sortingOrder = 200;

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
                GameObject _newCam = Instantiate(_vlackCamera);
                _newCam.name = "BlackCamera";
            }
        }



    }
}