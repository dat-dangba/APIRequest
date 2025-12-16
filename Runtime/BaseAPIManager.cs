using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace APIRequest
{
    public abstract class BaseAPIManager<I> : MonoBehaviour where I : MonoBehaviour
    {
        public static I Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = FindAnyObjectByType<I>();
            DontDestroyOnLoad(gameObject);
        }

        protected abstract string GetBaseUrl();
        protected abstract Dictionary<string, string> GetHeaders();

        protected virtual string GetUrlRequest(string path)
        {
            return $"{GetBaseUrl()}/{path}";
        }

        // -------------------------------------------------------
        // CORE REQUEST — hỗ trợ mọi method
        // -------------------------------------------------------
        protected virtual async Task<string> SendRequest(
            string path,
            string method,
            string bodyJson = null)
        {
            string url = GetUrlRequest(path);
            UnityWebRequest req;

            if (method == UnityWebRequest.kHttpVerbGET)
            {
                req = UnityWebRequest.Get(url);
            }
            else if (method == UnityWebRequest.kHttpVerbPOST)
            {
                req = new UnityWebRequest(url, method);
                byte[] body = Encoding.UTF8.GetBytes(bodyJson ?? "");
                req.uploadHandler = new UploadHandlerRaw(body);
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/json");
            }
            else if (method == UnityWebRequest.kHttpVerbPUT)
            {
                req = UnityWebRequest.Put(url, bodyJson ?? "");
                req.SetRequestHeader("Content-Type", "application/json");
            }
            else if (method == UnityWebRequest.kHttpVerbDELETE)
            {
                req = UnityWebRequest.Delete(url);
            }
            else
            {
                throw new Exception("Method not supported: " + method);
            }

            // Headers
            if (GetHeaders().Count > 0)
            {
                foreach (var kv in GetHeaders())
                    req.SetRequestHeader(kv.Key, kv.Value);
            }

            // Send async (không block UI)
            var op = req.SendWebRequest();
            while (!op.isDone)
                await Task.Yield();

            if (req.result != UnityWebRequest.Result.Success)
                throw new Exception($"HTTP Error: {req.error}, Status: {req.responseCode}");

            return req.downloadHandler.text;
        }

        // -------------------------------------------------------
        // JSON.NET: parse mọi loại T
        // -------------------------------------------------------
        public virtual async void GET<T>(
            string path,
            Action<T> onSuccess,
            Action<string> onError)
        {
            try
            {
                string json = await SendRequest(path, UnityWebRequest.kHttpVerbGET);
                T result = JsonConvert.DeserializeObject<T>(json);
                onSuccess?.Invoke(result);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.ToString());
            }
        }

        public virtual async void POST<T>(
            string path,
            object body,
            Action<T> onSuccess,
            Action<string> onError)
        {
            try
            {
                string bodyJson = JsonConvert.SerializeObject(body);
                string json = await SendRequest(path, UnityWebRequest.kHttpVerbPOST, bodyJson);
                T result = JsonConvert.DeserializeObject<T>(json);
                onSuccess?.Invoke(result);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.ToString());
            }
        }

        public virtual async void PUT<T>(
            string path,
            object body,
            Action<T> onSuccess,
            Action<string> onError)
        {
            try
            {
                string bodyJson = JsonConvert.SerializeObject(body);
                string json = await SendRequest(path, UnityWebRequest.kHttpVerbPUT, bodyJson);
                T result = JsonConvert.DeserializeObject<T>(json);
                onSuccess?.Invoke(result);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.ToString());
            }
        }

        public virtual async void DELETE(
            string path,
            Action onSuccess,
            Action<string> onError)
        {
            try
            {
                await SendRequest(path, UnityWebRequest.kHttpVerbDELETE);
                onSuccess?.Invoke();
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.ToString());
            }
        }
    }
}