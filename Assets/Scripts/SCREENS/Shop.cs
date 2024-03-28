using System.Collections.Generic;
using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace SCREENS
{
    public class Shop : MonoBehaviour
    {
        [Inject] private UIController _uiController;
        [Inject] private SoundController _soundController;
        [Inject] private DiContainer _diContainer;
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
                _soundController.Play(SoundController.SOUND.ui_click_back);//sound
                _uiController.HidePopup(UIController.POP_UP.shop);
            }

        }

        #region CLASS IAP GEM
        [System.Serializable]
        public class Gem
        {
            [Inject] private SoundController _soundController;
            [Inject] private DataController _dataController; 
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
                _soundController.Play(SoundController.SOUND.ui_purchase);//sound

                _dataController.playerData.Gem += _shopData.iValueToAdd;
                _dataController.SaveData(); //save                                                       
                EventController.OnUpdatedBoardInvoke();  //update board

            }

        }

        public List<Gem> LIST_IAP_GEM;
        private void LoadListAddGems()
        {
            int _total = LIST_IAP_GEM.Count;
            for (int i = 0; i < _total; i++)
            {
                _diContainer.Inject(LIST_IAP_GEM[i]);
                LIST_IAP_GEM[i].Construct();
            }
        }
        #endregion
    }
}
