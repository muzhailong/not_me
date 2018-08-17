using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace my
{
    class Config
    {
        public static String VIDEO_PATTERN = "*.MP4";
        public static String VIDEO_SUB_DIR = "video/";
        public static String V1_DIR = "1/";
        public static String V2_DIR = "2/";
        public static String V3_DIR = "3/";
        public static String OP_PATTERN = "*.txt";
        public static String HEAD_IMG_NAME = "head.jpg";

        public const int REMAIN_POINTS_COUNT = 5;
        public const int FORWARD_STEP = 30;

        public const float PATH_PLAYER_WIDTH = 200;
        public const float PATH_PLAYER_HEIGHT = 200;

        public const string CONNECT_TEXT = "已连接";
        public const string UNCONNECT_TEXT = "未连接";
        //串口
        public const string DEFAULT_PORT_NAME = "COM3";
        public const int DEFAULT_BAUD_RATE = 115200;
        public const Parity DEFAULT_PARITY = Parity.None;
        public const int DEFAULT_DATA_BITS = 8;
        public const StopBits DEFAULT_STOP_BITS = StopBits.One;

        public const float MIN_DISTANCE = 25;//误差值，值得继续试验
        public const float MIN_SMOOTH_DISTANCE = 5;//建议使用MIN_DISTANCE
        public const string POINTER_CORRECT_TXT = "正确";
        public const string POINTER_ERROR_TXT = "偏离";
        public const string POINTER_DEFAULT_TXT = "初始";

        public const int CORRECT_ERROR_QUEUE_NUMBER = 10;


        public const int FRAME_PERA_SPEED = 1;
    }
}
