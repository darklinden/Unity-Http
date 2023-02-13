using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Http;

public class HttpTest : MonoBehaviour
{
    private void Start()
    {
        Test();
    }

    class LoginSendData
    {
        public string channel;
        public string account;
        public string password;
    }

    struct LoginRecvUserData
    {
        public string id;
        public string account;
        public string sign;
        public string timestamp;
    }

    struct LoginRecvData
    {
        public int code;
        public LoginRecvUserData data;
    }

    async UniTaskVoid Test()
    {
        // {
        //     // Test Get
        //     var result = await Request.Instance.AsyncGet<string>("http://127.0.0.1:8080/info.txt");
        //     Log.D(result);
        // }

        // {
        //     // Test Post
        //     var result = await Request.Instance.AsyncPost<string, string>("http://127.0.0.1:8080", "hello");
        //     Log.D(result);
        // }

        {
            // Test Post
            var result = await Request.Instance.AsyncPostJSON("http://192.168.1.213:8112/player/login", new LoginSendData
            {
                channel = "test",
                account = "user001",
                password = "123456",
            });
            Log.D(result);
        }
    }
}