using dpa.Library.Models;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;

namespace dpa.Library.Services;

public class CloudService
{
    public CloudService() { }

    public void upload(Exercise exercise)
    {
        if (!File.Exists("exercises.txt"))
        {
            FileStream fs = File.Create("exercises.txt");//创建文件
            fs.Close();
        }
        System.IO.FileInfo myFile = new System.IO.FileInfo("exercises.txt"); 
        StreamWriter sw = myFile.AppendText();
 
        string[] strs = { "——————————————————————————————————————————————————————————————————————", 
            System.DateTime.Now.ToString("d"),
            "问题ID："+exercise.Id,exercise.question,
            "A、"+exercise.item1,
            "B、"+exercise.item2,
            "C、"+exercise.item3,
            "D、"+exercise.item4,
            "答案："+exercise.answer
        };            
        foreach (var s in strs) 
        { 
            sw.WriteLine(s); 
        } 
        sw.Close();
        UpLoading("exercises.txt","exercises");
    }
    private bool UpLoading(string upLoadFile,string name)
    {
        bool bresult = false;
        Mac mac = new Mac("14fmGDDaqnWkrpCS9nHO5a0mLgV_31TPGsPRYV4q", "xMdComIANqf8mFOdy3T0gefAttpWGjcv9nCIweCi");
        string key = name;//文件名称
     
        string filePath = upLoadFile;//文件路径
     
        PutPolicy putPolicy = new PutPolicy();
        putPolicy.Scope = "drivingexercise:" + key;
        putPolicy.SetExpires(3600);
        putPolicy.DeleteAfterDays = 1;
        string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
        Config config = new Config();
        config.Zone = Zone.ZONE_CN_South;
        config.UseHttps = true;
        config.UseCdnDomains = true;
        config.ChunkSize = ChunkUnit.U512K;
        FormUploader target = new FormUploader(config);
        HttpResult result = target.UploadFile(filePath, key, token, null);
     
        string back = result.Code.ToString();
        //LogHelper.WriteLog_LocalTxt("result:" + result);
        if (result.Code.ToString() == "200")
            bresult = true;
     
        return bresult;
    }
}
