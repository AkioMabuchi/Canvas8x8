using UniRx;
using UnityEngine;

namespace Models
{
    public static class UserNameModel
    {
        private const string AccessKey = "username";
        private static readonly ReactiveProperty<string> _userName = new ReactiveProperty<string>();
        public static IReadOnlyReactiveProperty<string> UserName => _userName;
        
        static UserNameModel()
        {
            if (ES3.KeyExists(AccessKey))
            {
                _userName.Value = ES3.Load<string>(AccessKey);
            }
            else
            {
                _userName.Value = "Player" + UnityEngine.Random.Range(0, 10000).ToString("D4");
                ES3.Save(AccessKey, _userName.Value);
            }
        }

        public static void SetUserName(string userName)
        {
            _userName.Value = userName;
            ES3.Save(AccessKey, _userName.Value);
        }
    }
}
