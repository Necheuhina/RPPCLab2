using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Exception = System.Exception;

namespace StudyHttpClient
{
    public class KeyValueRepository : IKeyValueRepository
    {
        public bool Ping()
        {
            using var client = new WebClient();
            try
            {
                client.DownloadString($"https://tolltech.ru/study/Ping");
                return true;
            }
            catch (WebException e)
            {
                return false;
            }

        }
        public KeyValue Find(string key)
        {
            using var client = new WebClient();

            var responseStr = client.DownloadString($"https://tolltech.ru/study/Find?key={key}");
            return JsonConvert.DeserializeObject<KeyValue>(responseStr);
        }

        public void Create(KeyValue keyValue)
        {
            var requestBodyStr = JsonConvert.SerializeObject(keyValue);
            var requestBody = Encoding.UTF8.GetBytes(requestBodyStr);

            using var client = new WebClient();
            client.UploadData($"https://tolltech.ru/study/Create", requestBody);
        }

        public void Update(string key, string updatedValue)
        {
            try
            {
                using var client = new WebClient();
                client.UploadData($"https://tolltech.ru/study/Update?key={key}&value={updatedValue}", Array.Empty<byte>());
            }
            catch (WebException e)
            {
                if ((e.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new KeyValueRepositoryException();
                }

                throw;
            }
        }
    }
}