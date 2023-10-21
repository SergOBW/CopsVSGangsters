using System;
using Abstract;
using UnityEngine;

namespace Yandex.Plugins.Login
{
    public class LoginManager : IMechanic
    {
        public static LoginManager Instance;
        private string nickName;
        private string avatarUrl;
        

        public event Action<string> OnNickNameChanged;
        public event Action<string> OnAvatarChanged;
        
        
        public bool isLogin { get; private set; }


        public void Initialize()
        {
            Instance = this;
        }
        
        public void Login()
        {
            if (AddManager.Instance != null)
            {
                AddManager.Instance.Login();
            }
        }

        public void SetName(string name)
        {
            nickName = name;
            Debug.Log("New nickname = " + nickName);
            OnNickNameChanged?.Invoke(nickName);
        }

        public void SetAvatar(string avatar)
        {
            avatarUrl = avatar;
            Debug.Log("New avatar link = " + avatarUrl);
            OnAvatarChanged?.Invoke(avatarUrl);
            isLogin = true;
        }

        public void LoginFirstTime()
        {
            isLogin = true;
            if (AddManager.Instance != null)
            {
                AddManager.Instance.LoadFromExternStorage();
            }
        }

        public void SetLoginType(string type)
        {
            if (type == "lite")
            {
                isLogin = false;
            }
            else
            {
                isLogin = true;
                if (AddManager.Instance != null)
                {
                    AddManager.Instance.LoadFromExternStorage();
                }
            }
            Debug.Log("Set login type = " + type);
        }
        
    }
}
