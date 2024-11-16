using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace dpa.Library.Services;

public class AdviseService
{
    public AdviseService() { }
    
    public String[] getadvise()
    {
        //string[0] 新闻内容
        //string[1] 新闻图片
        //string[2] 新闻链接
        var url = "https://www.zgjtb.com/node_142.html";
        string strBuff = gethtml(url);
        //doc获取一个dom对象，即该html的内容
        var doc = new HtmlDocument();
        doc.LoadHtml(strBuff); 
        string x="";
        Random ran = new Random();
        int n1 = ran.Next(50);
        int n2 = 0;
        var anodes = doc.DocumentNode.SelectNodes("//a");
        foreach (var anode in anodes)
        {
           string href = anode.GetAttributeValue("href", "");
           if (href.Contains("content_"))
           {
               if (n2 == n1)
               {
                   x = href;
                   break;
               }
               else
               {
                   n2++;
               }
           }
       }
        url = x;
        strBuff = gethtml(url);
        //重复之前的过程
        doc = new HtmlDocument();
        doc.LoadHtml(strBuff);
        string content="";
        string picture = "";
        anodes = doc.DocumentNode.SelectNodes("//p");
        var anodes_img= doc.DocumentNode.SelectNodes("//img");
        foreach (var anode in anodes)
        {
            if (!anode.GetDirectInnerText().Equals(""))
            {
                content = anode.GetDirectInnerText();
                break;
            }
        }
        foreach (var anode in anodes_img)
        {
            string src = anode.GetAttributeValue("src", "");
            if (src.Contains("photo"))
            {
                picture = src;
                break;
            }
        }
        String[] strs= {content,picture,url};
        return strs;
    }

    private string gethtml(string url)
    {
        string strBuff = "";//定义文本字符串，用来保存下载的html
        int byteRead = 0; 
            
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
        //若成功取得网页的内容，则以System.IO.Stream形式返回，若失败则产生ProtoclViolationException错 误。在此正确的做法应将以下的代码放到一个try块中处理。这里简单处理 
        Stream reader = webResponse.GetResponseStream();
        ///返回的内容是Stream形式的，所以可以利用StreamReader类获取GetResponseStream的内容，并以StreamReader类的Read方法依次读取网页源程序代码每一行的内容，直至行尾（读取的编码格式：UTF8）
        StreamReader respStreamReader = new StreamReader(reader,Encoding.Default);
 
        ///分段，分批次获取网页源码
        char[] cbuffer = new char[256];
        byteRead = respStreamReader.Read(cbuffer,0,256);

        while (byteRead != 0)
        {
            string strResp = new string(cbuffer);
            strBuff = strBuff + strResp;
            byteRead = respStreamReader.Read(cbuffer, 0, 256);
        }
        return strBuff;
    }
}
