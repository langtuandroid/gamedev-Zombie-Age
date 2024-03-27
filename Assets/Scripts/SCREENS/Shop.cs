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
                SoundController.Instance.Play(SoundController.SOUND.ui_click_back);//sound
                UIController.Instance.HidePopup(UIController.POP_UP.shop);
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
                SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound

                DataController.Instance.playerData.Gem += DATA.iValueToAdd;
                DataController.Instance.SaveData(); //save                                                       
                EventController.OnUpdatedBoardInvoke();  //update board

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
