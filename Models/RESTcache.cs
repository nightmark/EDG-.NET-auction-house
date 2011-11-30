using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace AuctionHouse.Models {
    public class RESTcache<K, V> {
        private System.Xml.XmlDocument config;
        private String serverURL;

        public RESTcache() {
            config = new System.Xml.XmlDocument();

            config.Load(HttpContext.Current.Server.MapPath("~/App_Data/EDGsettings.xml"));

            string hostname = config.SelectSingleNode("server/hostname").InnerText;
            string port = config.SelectSingleNode("server/port").InnerText;
            string endpointMapping = config.SelectSingleNode("server/endpointMapping").InnerText;
            string cacheName = config.SelectSingleNode("server/cacheName").InnerText;
            serverURL = "http://" + hostname + ":" + port + endpointMapping + "/" + cacheName;  
        }

        public string getServer() {
            return serverURL;
        }

        public static String encode(V key) {
            MemoryStream memoryStream = new MemoryStream();
            new BinaryFormatter().Serialize(memoryStream, key);
            return System.Convert.ToBase64String(memoryStream.ToArray());
        }

        public static V decode(String key) {
            MemoryStream memoryStream = new MemoryStream(System.Convert.FromBase64String(key));
            try {
                return (V)new BinaryFormatter().Deserialize(memoryStream);
            }
            catch (SerializationException e) {
                return default(V);
            }
        }

        private String toStringKey(K key) {
            if (key is String) {
                return key as String;
            }
            else {
                throw new NotSupportedException("RESTful cache only supports string keys");
            }
        }

        private String doOperation(String method, String key, V value){
            String url = key == null ? serverURL : (serverURL + "/" + key);
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = method;
            request.ContentType = "text/plain";
            if (method.Equals("PUT")) {
                StreamWriter output = new StreamWriter(request.GetRequestStream());                
                output.Write(encode(value));
                output.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StringBuilder responseBody = new StringBuilder();
            Stream responseBodyStream = response.GetResponseStream();
            int read = 0;
            int bufferSize = 1024*8;
            byte[] buffer = new byte[bufferSize];
            while((read = responseBodyStream.Read(buffer, 0, bufferSize)) != 0){
                responseBody.Append(System.Text.Encoding.Default.GetString(buffer, 0, read));                
            }
            response.Close();
            return responseBody.ToString();            
        }

        public V remove(K key) {
            String stringKey = toStringKey(key);
            V value = default(V);
            String stringValue = doOperation("DELETE", stringKey.ToString(), value);
            return (V)decode(stringValue);
        }

        public V get(K key) {
            String stringKey = toStringKey(key);
            String stringValue = doOperation("GET", stringKey, default(V));
            return decode(stringValue);
        }

        public V put(K key, V value) {
            String stringKey = toStringKey(key);
            String stringValue = doOperation("PUT", stringKey, value);
            return (V)decode(stringValue);
        }

        public V replace(K key, V value) {
            return put(key, value);
        }
    }

}