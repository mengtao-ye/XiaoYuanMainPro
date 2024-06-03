namespace Game
{
    public static partial class AppTools
    {
        public static void ShowLoading()
        {
            GameCenter.Instance.ShowLogUI<LoadingLogUI>();
        }
        public static void HideLoading()
        {
            GameCenter.Instance.HideLogUI<LoadingLogUI>();
        }
    }
}
