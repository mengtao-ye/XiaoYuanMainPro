using YFramework;

namespace Game
{
    public static class PlayerPrefsTools
    {
        /// <summary>
        /// 保存账号到本地
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public static void SaveLoginAccount(string account, string password)
        {
            byte[] accountBytes = EncryptionTools.Encryption(account.ToBytes());
            byte[] passwordBytes = EncryptionTools.Encryption(password.ToBytes());
            PlayerPrefsModule.Set(PlayerPrefsData.AutoLoginAccount, accountBytes);
            PlayerPrefsModule.Set(PlayerPrefsData.AutoLoginPassword, passwordBytes);
        }
        /// <summary>
        /// 获取本地账号
        /// </summary>
        /// <returns></returns>
        public static (string, string) GetLoginAccount()
        {
            string Account = null;
            string Password = null;
            if (PlayerPrefsModule.Contains(PlayerPrefsData.AutoLoginAccount))
            {
                Account = EncryptionTools.Decryption( PlayerPrefsModule.GetBytes(PlayerPrefsData.AutoLoginAccount)).ToStr();
                Password = EncryptionTools.Decryption(PlayerPrefsModule.GetBytes(PlayerPrefsData.AutoLoginPassword)).ToStr();
            }
            return (Account, Password);
        }
    }
}
