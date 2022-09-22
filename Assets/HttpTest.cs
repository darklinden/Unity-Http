using UnityEngine;
using Http;
using Newtonsoft.Json;
using System.Collections;

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

public class HttpTest : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        var request = Request<string>.Make();

        {
            // Test Get
            yield return request.Get("http://www.baidu.com");
            Log.D(request.Result);
        }

        {
            // Test Post
            yield return request.Post("http://www.baidu.com", "Hello World");
            Log.D(request.Result);
        }

        {
            // Test Post
            var loginRequest = Request<LoginRecvData>.Make();
            yield return loginRequest.PostWithForm("http://192.168.1.180:7001/login", new LoginSendData
            {
                channel = "test",
                account = "123456",
                password = "123456",
            });

            Log.D(JsonConvert.SerializeObject(loginRequest.Result));
        }
    }
}