using UnityEngine;

namespace Game
{
    public class Test : MonoBehaviour
    {
        private void Awake()
        {
            NormalVerticalScrollView normalScrollView = gameObject.AddComponent<NormalVerticalScrollView>();
            normalScrollView.Init();
            normalScrollView.SetSize(1498.456f);
        }
    } 
}