using MANAGERS;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.UI;

namespace MODULES
{
    public class FreeGem : MonoBehaviour
    {
        private Button buThis;
        [SerializeField] Text txtValue;

        // Start is called before the first frame update
        void Start()
        {
            RewardData _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.ads_free_gem);

            buThis = GetComponent<Button>();

            buThis.onClick.AddListener(() => SetButton(buThis));
            txtValue.text = "+" + _reward.iValue;
        }

        private void OnEnable()
        {
            gameObject.SetActive(false);
        }

        private void SetButton(Button _bu)
        {
            //for tutorial
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }


            if (_bu == buThis)
            {
            
            }

        }
    }
}
