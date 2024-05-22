using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class RegisterPanel : BaseCustomPanel
    {
        public ISubUI codeSubUI { get; private set; }
        public RegisterNormalSubUI normalSubUI { get; private set; }
        public RegisterPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(BackBtnListener);
            codeSubUI = new RegisterCodeSubUI(transform.FindObject<Transform>("CodeArea"),this);
            normalSubUI = new RegisterNormalSubUI(transform.FindObject<Transform>("NormalArea"), this);
            AddSubUI(codeSubUI);
            AddSubUI(normalSubUI);
        }

        private void BackBtnListener() {
            GameCenter.Instance.ShowPanel<LoginPanel>();
        }

        public override void Show()
        {
            base.Show();
            codeSubUI.Show();
            normalSubUI.Hide();
        }
    }
}
