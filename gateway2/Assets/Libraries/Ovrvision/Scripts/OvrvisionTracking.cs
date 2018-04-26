using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class OvrvisionTracking  {

    const string DLL_Name = "ovrvision";


    [DllImport(DLL_Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    static extern System.IntPtr ovTrackCreate(System.IntPtr ovr);
    [DllImport(DLL_Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    static extern System.IntPtr ovTrackDestroy(System.IntPtr inst);
    [DllImport(DLL_Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    static extern void ovTrackRender(System.IntPtr t, System.IntPtr ovr, bool calib, bool point);
    [DllImport(DLL_Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    static extern int ovGetTrackData(System.IntPtr t, System.IntPtr mdata);
    [DllImport(DLL_Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    static extern void ovTrackingCalibReset(System.IntPtr t);

    System.IntPtr _instance;
    COvrvisionUnity _ovr;
    //Class
    public OvrvisionTracking(COvrvisionUnity ovr)
    {
        _ovr = ovr;
        _instance = ovTrackCreate(ovr.Instance);
    }

    public void Destroy()
    {
        ovTrackDestroy(_instance);
        _instance = IntPtr.Zero;
    }

    public void Update(bool calib, bool point=false)
    {
        ovTrackRender(_instance, _ovr.Instance,calib,point);
    }
    public int OvrvisionGetTrackingVec3(System.IntPtr mdata)
    {
        return ovGetTrackData(_instance,mdata);
    }
    public void OvrvisionTrackReset()
    {
        ovTrackingCalibReset(_instance);
    }
}
