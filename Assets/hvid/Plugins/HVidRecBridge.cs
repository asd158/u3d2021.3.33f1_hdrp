using System;
using System.Runtime.InteropServices;

public static class HVidRecBridge
{
    private const string LIBNAME = "hvidrec";

    [DllImport(LIBNAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern
        IntPtr hvid_record_open(int work_gpu, int v_width, int v_height, int framerate, int level, string export_dir);

    [DllImport(LIBNAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hvid_record_get_vid_frame_buffsize(IntPtr inst_id);

    [DllImport(LIBNAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hvid_record_get_vid_frame_count(IntPtr inst_id);

    [DllImport(LIBNAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hvid_record_write_vid(IntPtr inst_id, IntPtr vid_buff, int vid_buff_size, bool is_final);

    [DllImport(LIBNAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hvid_record_write_aud(IntPtr inst_id, IntPtr aud_buff, int aud_buff_size, bool is_final);

    [DllImport(LIBNAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hvid_record_close(IntPtr inst_id);
}