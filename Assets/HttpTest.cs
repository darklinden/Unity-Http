using UnityEngine;
using Cysharp.Threading.Tasks;
using Http;
using Newtonsoft.Json;

public class HttpTest : MonoBehaviour
{
    private void Start()
    {
        Test();
    }

    struct LoginSendData
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

    async void Test()
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
            var result = await Request.Instance.AsyncPostForm<LoginSendData, LoginRecvData>("http://192.168.1.180:7001/login", new LoginSendData
            {
                channel = "test",
                account = "123456",
                password = "123456",
            });
            Log.D(JsonConvert.SerializeObject(result));
        }
    }
}