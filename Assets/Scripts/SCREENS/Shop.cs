using System.Collections.Generic;
using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.UI;

namespace SCREENS
{
    public class Shop : MonoBehaviour
    {
        [SerializeField]
        private Button buBack;

        // Start is called before the first frame update
        void Start()
        {
            buBack.onClick.AddListener(() => SetButton(buBack));

            Init_ListIapGem();

        }

        //SET BUTTON
        private void SetButton(Button _bu)
        {
            if (_bu == buBack)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_back);//sound
                TheUiManager.Instance.HidePopup(TheUiManager.POP_UP.shop);
            }

        }

        #region CLASS IAP GEM
        [System.Serializable]
        public class IAP_GEM
        {
            public ShopData DATA;
            public Text txtName;
            public Text txtValue;
            public Text txtPrice;
            public Text txtSale;

            public Button buBuy;


            public void Init()
            {
                txtName.text = DATA.strProductName;

                txtPrice.text = DATA.fPriceDollar.ToString() + " $";
            

                txtValue.text = "+" + DATA.iValueToAdd.ToString();
                buBuy.onClick.AddListener(() => Buy());

                if (txtSale)
                    txtSale.text = "+" + DATA.iPrecentSale + "%";
            }

            private void Buy()
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound

                TheDataManager.Instance.THE_DATA_PLAYER.iGem += DATA.iValueToAdd;
                TheDataManager.Instance.SaveDataPlayer(); //save                                                       
                TheEventManager.PostEvent_OnUpdatedBoard();  //update board

            }

        }

        public List<IAP_GEM> LIST_IAP_GEM;
        private void Init_ListIapGem()
        {
            int _total = LIST_IAP_GEM.Count;
            for (int i = 0; i < _total; i++)
            {
                LIST_IAP_GEM[i].Init();
            }
        }
        #endregion


    }
}
