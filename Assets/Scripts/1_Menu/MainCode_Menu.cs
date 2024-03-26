using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCode_Menu : MonoBehaviour
{
    [SerializeField] float fTimeToloading = 5.0f;
    private float fCountTime = 0;
    [SerializeField] Image imaLoadingRender;
    [SerializeField] GameObject objLogoGame;
    [SerializeField] GameObject objLoadingPopup;

   
    [Space(30)]   
    [SerializeField] Button buPlay;


    //Start is called before the first frame update
    void Start()
    {
        buPlay.onClick.AddListener(() => SetButton(buPlay));
        MusicManager.Instance.Play();
        TheUiManager.Instance.SetCameraForPopupCanvas(Camera.main);//set camera       
        objLogoGame.SetActive(false);
    }

    private void SetButton(Button _button)
    {
        if (_button == buPlay)
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
            TheUiManager.Instance.LoadScene(TheUiManager.SCENE.LevelSelection);
        }
    }

   



    private void Update()
    {
        if (fCountTime < fTimeToloading)
        {
            fCountTime += Time.deltaTime;
            imaLoadingRender.fillAmount = fCountTime / fTimeToloading;
            if (fCountTime >= fTimeToloading)
            {

                objLoadingPopup.SetActive(false);
                objLogoGame.SetActive(true);


            }
        }
    }
}
