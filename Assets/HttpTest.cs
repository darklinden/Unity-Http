using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Http;

public class HttpTest : MonoBehaviour
{
    void Start()
    {
        Log.D("HttpTest Start");
        Test().Forget();
    }

    class LoginSendData
    {
        public string channel;
        public string account;
        public string password;
    }

    class LoginRecvUserData
    {
        public string id;
        public string account;
        public string sign;
        public string timestamp;
    }

    class LoginRecvData
    {
        public int code;
        public LoginRecvUserData data;
    }

    async UniTask Test()
    {
        Log.D("Test Start");
        var request = Request.Create();

        {
            Log.D("Test Get");
            // Test Post
            var result = await request.AsyncJsonGet<LoginRecvData>("https://baidu.com", new LoginSendData
            {
                channel = "test",
                account = "user001",
                password = "123456",
            });
            Log.D(result);
        }

        await UniTask.Delay(1000);

        {
            Log.D("Test Get");
            // Test Post
            var result = await request.AsyncJsonGet<LoginRecvData>("https://bing.com", new LoginSendData
            {
                channel = "test",
                account = "user001",
                password = "123456",
            });
            Log.D(result);
        }

        await UniTask.Delay(1000);

        {
            Log.D("Test Post");
            // Test Post
            var result = await request.AsyncJsonPost<LoginSendData, LoginRecvData>("https://baidu.com", new LoginSendData
            {
                channel = "test",
                account = "user001",
                password = "123456",
            });
            Log.D(result);
        }


        await UniTask.Delay(1000);

        {
            Log.D("Test Post");
            // Test Post
            var result = await request.AsyncJsonPost<LoginSendData, LoginRecvData>("https://bing.com", new LoginSendData
            {
                channel = "test",
                account = "user001",
                password = "123456",
            });
            Log.D(result);
        }
    }
}