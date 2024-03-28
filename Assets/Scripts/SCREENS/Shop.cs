using System.Collections.Generic;
using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SCREENS
{
    public class Shop : MonoBehaviour
    {
        [FormerlySerializedAs("buBack")] [SerializeField] private Button _backButton;

        private void Start()
        {
            _backButton.onClick.AddListener(() => AssignButton(_backButton));

            LoadListAddGems();

        }

        private void AssignButton(Button _bu)
        {
            if (_bu == _backButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_back);//sound
                UIController.Instance.HidePopup(UIController.POP_UP.shop);
            }

        }

        #region CLASS IAP GEM
        [System.Serializable]
        public class Gem
        {
            [FormerlySerializedAs("DATA")] public ShopData _shopData;
            [FormerlySerializedAs("txtName")] public Text _nametext;
            [FormerlySerializedAs("txtValue")] public Text _valueText;
            [FormerlySerializedAs("txtPrice")] public Text _priceText;
            [FormerlySerializedAs("txtSale")] public Text _scaleText;

            [FormerlySerializedAs("buBuy")] public Button _buyButton;


            public void Construct()
            {
                _nametext.text = _shopData.strProductName;

                _priceText.text = _shopData.fPriceDollar + " $";
            

                _valueText.text = "+" + _shopData.iValueToAdd;
                _buyButton.onClick.AddListener(() => Buy());

                if (_scaleText)
                    _scaleText.text = "+" + _shopData.iPrecentSale + "%";
            }

            private void Buy()
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound

                DataController.Instance.playerData.Gem += _shopData.iValueToAdd;
                DataController.Instance.SaveData(); //save                                                       
                EventController.OnUpdatedBoardInvoke();  //update board

            }

        }

        public List<Gem> LIST_IAP_GEM;
        private void LoadListAddGems()
        {
            int _total = LIST_IAP_GEM.Count;
            for (int i = 0; i < _total; i++)
            {
                LIST_IAP_GEM[i].Construct();
            }
        }
        #endregion
    }
}
