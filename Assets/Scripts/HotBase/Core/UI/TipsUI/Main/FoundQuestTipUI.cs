using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class FoundQuestTipUI : BaseCustomTipsUI
    {
        private Text mQuestText;
        private InputField mQuestIF;
        private FoundData mFoundData;
        public FoundQuestTipUI()
        {
        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("CloseBtn").onClick.AddListener(Hide);
            mQuestText = transform.FindObject<Text>("Quest");
            mQuestIF = transform.FindObject<InputField>("QuestIF");
            transform.FindObject<Button>("ComfineBtn").onClick.AddListener(ComfineBtnListener);
        }

        public override void Hide()
        {
            base.Hide();
            mQuestIF.text = string.Empty;
        }

        private void ComfineBtnListener() {
            if (mQuestIF.text == string.Empty) 
            {
                AppTools.ToastNotify("请输入答案");
                return;
            }
            if (mQuestIF.text == mFoundData.result)
            {
                Hide();
                GameCenter.Instance.ShowTipsUI<ContactTipUI>((ui)=>
                {
                    ui.SetData(mFoundData.contactType,mFoundData.contact);
                });
            }
            else 
            {
                AppTools.ToastNotify("回答错误");
            }
        }
        public void SetData( FoundData foundData) 
        {
            mFoundData = foundData;
            mQuestText.text = mFoundData.quest;
        }
    }
}
