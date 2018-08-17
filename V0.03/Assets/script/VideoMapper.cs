using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LitJson;

namespace my
{
    public class VideoMapper
    {
        private Dictionary<string, VideoItem> mp;
        public VideoMapper(String dir)
        {
            String[] dirs = Directory.GetDirectories(dir);
            mp = new Dictionary<string, VideoItem>();
            foreach (string item in dirs)
            {
                mp.Add(item.Substring(item.LastIndexOf("/")+1), buildVideoItem(item+"/"));
            }
        }

        public Dictionary<string, VideoItem> getMp()
        {
            return mp;
        }

        private VideoItem buildVideoItem(String dir)
        {
            VideoItem vi = new VideoItem();
            vi.v1=new Video(
                Directory.GetFiles(dir +Config.VIDEO_SUB_DIR + Config.V1_DIR, Config.VIDEO_PATTERN)[0]);
            vi.v2 = new Video(
                Directory.GetFiles(dir + Config.VIDEO_SUB_DIR + Config.V2_DIR, Config.VIDEO_PATTERN)[0]);
            vi.opPath= 
                Directory.GetFiles(dir + Config.VIDEO_SUB_DIR + Config.V3_DIR, Config.OP_PATTERN)[0];

            JsonData jd=JsonMapper.ToObject(File.ReadAllText(dir + "info.txt"));
            vi.userId = jd["userId"].ToString();
            vi.userName = jd["userName"].ToString();
            vi.imgPath = dir +Config.HEAD_IMG_NAME;
            vi.allTime = int.Parse(jd["all_time"].ToString());
            return vi;
        }
    }
}
