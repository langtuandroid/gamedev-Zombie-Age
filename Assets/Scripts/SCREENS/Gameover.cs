using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameover : MonoBehaviour
{
    [SerializeField]
    private Button buShop, buLevelSelection, buReplay;
    // Start is called before the first frame update
    void Start()
    {
        buReplay.onClick.AddListener(() => SetButton(buReplay));
        buShop.onClick.AddListener(() => SetButton(buShop));
        buLevelSelection.onClick.AddListener(() => SetButton(buLevelSelection));
    }

    //SET BUTTON
    private void SetButton(Button _bu)
    {
        if (_bu == buShop)
        {
            MusicManager.Instance.Play();
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
            TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.shop);
        }
        else if (_bu == buLevelSelection)
        {
            MusicManager.Instance.Play();
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
            TheUiManager.Instance.LoadScene(TheUiManager.SCENE.LevelSelection);
           
        }
        else if (_bu == buReplay)
        {
           // MusicManager.Instance.Play();
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
            TheUiManager.Instance.LoadScene(TheUiManager.SCENE.Gameplay);           
        }
    }
    private void OnEnable()
    {
        TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_game_over);//sound
        MusicManager.Instance.Stop();
    }

   
}
