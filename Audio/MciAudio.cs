using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Framework2D
{
    public class MciAudio
    {
        public const int MM_MCINOTIFY = 0x3B9;
        private static bool isOpen = false;
        private Form notifyForm;

        //mciSendString 
        [DllImport("winmm.dll")]
        private static extern long mciSendString(
            string command,
            StringBuilder returnValue,
            int returnLength,
            IntPtr winHandle);

        [DllImport("winmm.dll")]
        public static extern uint mciGetDeviceID(string alias);

        public MciAudio(Form frm)
        {
            isOpen = true;
            notifyForm = frm;
        }
        private  void CloseTrack(string trackName)
        {
            if (DeviceExists(trackName))
            {
                String playCommand = "Close " + trackName;
                mciSendString(playCommand, null, 0, IntPtr.Zero);
                //isOpen = false;
            }
        } 
        public void LoadMediaFile(string fileName, string trackName)
        {
            string playCommand = "";
          //  if (!DeviceExists(trackName))
            { 
             playCommand = "Open \"" + fileName +
                     "\" type mpegvideo alias " + trackName;
                isOpen = true;   
            }
          //  else
            {
           //  playCommand = "Load " + fileName + " " + trackName + "notify";
            }

            mciSendString(playCommand, null, 0, IntPtr.Zero);

        }
        public void PauseTrack(string trackName)
        {
           if (DeviceExists(trackName))
            {
                string playCommand = "Pause " + trackName + " notify";
                mciSendString(playCommand, null, 0, notifyForm.Handle);
            }
        }
        public void PlayTrack(string trackName)
        {
            if (DeviceExists(trackName))
            {
                string playCommand = "Play " + trackName + " notify";
                mciSendString(playCommand, null, 0, notifyForm.Handle);
            }
        }
        public void StopTrack(string trackName)
        {
            CloseTrack(trackName);
            //if (isOpen)
            //{
            //    string playCommand = "Stop " + trackName;
            //    mciSendString(playCommand, null, 0, IntPtr.Zero);
            //    playCommand = "Reset " + trackName;
            //    mciSendString(playCommand, null, 0, IntPtr.Zero);
            //    isOpen = false;
            //}
        }
        public bool DeviceExists(string alias)
        {
            return  mciGetDeviceID(alias) == 0 ? false : true;
        }

        public static void Close()
        {
            if (isOpen)
            {
                String playCommand = "Close All";
                mciSendString(playCommand, null, 0, IntPtr.Zero);
                isOpen = false;
            }
        }
        private static void OpenMediaFile(string fileName, string trackName)
        {

            string playCommand = "Open \"" + fileName +
                                "\" type mpegvideo alias " + trackName;
            mciSendString(playCommand, null, 0, IntPtr.Zero);
            isOpen = true;
        }
        public static void PlayMediaFile(string trackName, Form frm)
        {
            if (isOpen)
            {
                string playCommand = "Play " + trackName + " notify";
                mciSendString(playCommand, null, 0, frm.Handle);
            }
        }
        public static void Play(string fileName, Form notifyForm)
        {
            OpenMediaFile(fileName, "media");
            PlayMediaFile("media", notifyForm);
        }

    }
}
