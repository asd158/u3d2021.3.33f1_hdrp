using System;
using System.Runtime.InteropServices;

public static class HVidRecord
{
    public static IntPtr Open(int work_gpu, int v_width, int v_height, int framerate, int level, string export_dir)
    {
        return HVidRecBridge.hvid_record_open(work_gpu, v_width, v_height, framerate, level, export_dir);
    }

    public static int GetVideoFrameBuffsize(IntPtr intPtr)
    {
        return HVidRecBridge.hvid_record_get_vid_frame_buffsize(intPtr);
    }

    public static int GetVideoFrameCount(IntPtr intPtr)
    {
        return HVidRecBridge.hvid_record_get_vid_frame_count(intPtr);
    }

    public static bool WriteVideoRawdata(IntPtr intPtr, Byte[] vidbBuffer, bool isFinalBuff)
    {
        IntPtr intPPtr = Marshal.AllocHGlobal(vidbBuffer.Length * sizeof(char));
        int ret;
        try
        {
            ret = HVidRecBridge.hvid_record_write_vid(intPtr, intPPtr, vidbBuffer.Length, isFinalBuff);
        }
        finally
        {
            Marshal.FreeHGlobal(intPPtr);
        }

        return ret == 0;
    }

    public static bool WriteAudioRawdata(IntPtr intPtr, Byte[] audBuffer, bool isFinalBuff)
    {
        IntPtr intPPtr = Marshal.AllocHGlobal(audBuffer.Length * sizeof(char));
        int ret;
        try
        {
            ret = HVidRecBridge.hvid_record_write_aud(intPtr, intPPtr, audBuffer.Length, isFinalBuff);
        }
        finally
        {
            Marshal.FreeHGlobal(intPPtr);
        }

        return ret == 0;
    }

    public static bool Close(IntPtr intPtr)
    {
        int ret = HVidRecBridge.hvid_record_close(intPtr);
        return ret == 0;
    }
}