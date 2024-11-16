

using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Webiat
{
    public class AIReplyService
    {
        private List<Content> history;//聊天历史
        ClientWebSocket webSocket0;
        CancellationToken cancellation;
        string hostUrl = "https://spark-api.xf-yun.com/v4.0/chat";
        // 应用APPID（必须为webapi类型应用，并开通星火认知大模型授权）
        const string x_appid = "a57a99c7";
        // 接口密钥（webapi类型应用开通星火认知大模型后，控制台--我的应用---星火认知大模型---相应服务的apikey）
        const string api_secret = "Y2U0ZGFmZWQyY2Y4MzU3NzFiOWY5NGEw";
        // 接口密钥（webapi类型应用开通星火认知大模型后，控制台--我的应用---星火认知大模型---相应服务的apisecret）
        const string api_key = "458c4011ecf01c8c62706633232aaa3c";
        
        public AIReplyService() {history=new List<Content>(); }

        public async Task<string> reply(string input,int maxlength)
        {
            return await Tasker(input,maxlength);
        }
        async public Task<string> Tasker(string input,int maxlength)
        {
            
            string authUrl = GetAuthUrl();
            string url = authUrl.Replace("http://", "ws://").Replace("https://", "wss://");
            using (webSocket0 = new ClientWebSocket())
            {
                try
                {
                    await webSocket0.ConnectAsync(new Uri(url), cancellation);

                    JsonRequest request = new JsonRequest();
                    request.header = new Header()
                                    {
                                        app_id = x_appid,
                                        uid = "12345"
                                    };
                    request.parameter = new Parameter()
                                        {
                                            chat = new Chat()
                                            {
                                                domain = "4.0Ultra",//模型领域，默认为星火通用大模型
                                                temperature = 0.5,//温度采样阈值，用于控制生成内容的随机性和多样性，值越大多样性越高；范围（0，1）
                                                max_tokens = 300,//生成内容的最大长度，范围（0，4096）
                                            }
                                        };
                    List<Content> content1 = new List<Content>(history.ToArray());
                    content1.AddRange(new List<Content>
                    {
                        new Content() { role = "system",content = "你是猫娘，每句话之间必须加一个'喵'"},
                        new Content() { role = "user", content = input+",要求字数不超过"+maxlength},
                        // new Content() { role = "assistant", content = "....." }, // AI的历史回答结果，这里省略了具体内容，可以根据需要添加更多历史对话信息和最新问题的内容。
                    });
                    request.payload = new Payload()
                                        {
                                            message = new Message()
                                            {
                                                text = content1
                                            }
                                        };

                    string jsonString = JsonConvert.SerializeObject(request);
                    //连接成功，开始发送数据
                    

                    var frameData2 = System.Text.Encoding.UTF8.GetBytes(jsonString.ToString());

                    
                    webSocket0.SendAsync(new ArraySegment<byte>(frameData2), WebSocketMessageType.Text, true, cancellation);
                   
                    // 接收流式返回结果进行解析
                    byte[] receiveBuffer = new byte[1024];
                    WebSocketReceiveResult result = await webSocket0.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), cancellation);
                    String resp = "";
                    while (!result.CloseStatus.HasValue)
                    {
                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
                            //将结果构造为json
                            
                            JObject jsonObj = JObject.Parse(receivedMessage);
                            int code = (int)jsonObj["header"]["code"];
                            
                            
                            if(0==code){
                                int status = (int)jsonObj["payload"]["choices"]["status"];
                                

                                JArray textArray = (JArray)jsonObj["payload"]["choices"]["text"];
                                string content = (string)textArray[0]["content"];
                                resp += content;

                                if(status != 2){
                                    //Console.WriteLine($"已接收到数据： {receivedMessage}");
                                }
                                else{
                                    /*Console.WriteLine($"最后一帧： {receivedMessage}");
                                    int totalTokens = (int)jsonObj["payload"]["usage"]["text"]["total_tokens"];
                                    Console.WriteLine($"整体返回结果： {resp}");
                                    Console.WriteLine($"本次消耗token数： {totalTokens}");*/
                                    break;
                                }

                            }else{
                                Console.WriteLine($"请求报错： {receivedMessage}");
                            }
                                

                        }
                        else if (result.MessageType == WebSocketMessageType.Close)
                        {
                            Console.WriteLine("已关闭WebSocket连接");
                            break;
                        }

                        result = await webSocket0.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), cancellation);
                    }
                    history_add(input,resp);
                    return resp;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return "";
        }
        string GetAuthUrl()
        {
            string date = DateTime.UtcNow.ToString("r");

            Uri uri = new Uri(hostUrl);
            StringBuilder builder = new StringBuilder("host: ").Append(uri.Host).Append("\n").//
                Append("date: ").Append(date).Append("\n").//
                Append("GET ").Append(uri.LocalPath).Append(" HTTP/1.1");

            string sha = HMACsha256(api_secret, builder.ToString());
            string authorization = string.Format("api_key=\"{0}\", algorithm=\"{1}\", headers=\"{2}\", signature=\"{3}\"", api_key, "hmac-sha256", "host date request-line", sha);
            //System.Web.HttpUtility.UrlEncode

            string NewUrl = "https://" + uri.Host + uri.LocalPath;

            string path1 = "authorization" + "=" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authorization));
            date = date.Replace(" ", "%20").Replace(":", "%3A").Replace(",", "%2C");
            string path2 = "date" + "=" + date;
            string path3 = "host" + "=" + uri.Host;

            NewUrl = NewUrl + "?" + path1 + "&" + path2 + "&" + path3;
            return NewUrl;
        }
        string HMACsha256(string apiSecretIsKey, string buider)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(apiSecretIsKey);
            System.Security.Cryptography.HMACSHA256 hMACSHA256 = new System.Security.Cryptography.HMACSHA256(bytes);
            byte[] date = System.Text.Encoding.UTF8.GetBytes(buider);
            date = hMACSHA256.ComputeHash(date);
            hMACSHA256.Clear();

            return Convert.ToBase64String(date);

        }
        //构造请求体
        public class JsonRequest
        {
            public Header header { get; set; }
            public Parameter parameter { get; set; }
            public Payload payload { get; set; }
        }

        public class Header
        {
            public string app_id { get; set; }
            public string uid { get; set; }
        }

        public class Parameter
        {
            public Chat chat { get; set; }
        }

        public class Chat
        {
            public string domain { get; set; }
            public double temperature { get; set; }
            public int max_tokens { get; set; }
        }

        public class Payload
        {
            public Message message { get; set; }
        }

        public class Message
        {
            public List<Content> text { get; set; }
        }

        public class Content
        {
            public string role { get; set; }
            public string content { get; set; }
        }
        public void history_add(string ask,string answer)
        {
            history.Add(new Content(){role="user",content=ask});
            history.Add(new Content(){role="assistant",content=answer});
        }

        public void history_clear()
        {
            history.Clear();
        }
    }
}
