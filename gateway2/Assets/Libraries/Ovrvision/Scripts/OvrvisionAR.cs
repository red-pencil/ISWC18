using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;


public class OvrvisionAR {

    const string DLL_Name = "ovrvision";

    ////////////// Ovrvision AR System //////////////
    [DllImport(DLL_Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    static extern System.IntPtr ovARCreate(System.IntPtr ovr, float arMeter);
    [DllImport(DLL_Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    static extern System.IntPtr ovARDestroy(System.IntPtr inst);
    [DllImport(DLL_Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    static extern void ovARRender(System.IntPtr ar, System.IntPtr ovr);
    [DllImport(DLL_Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    static extern int ovARGetData(System.IntPtr ar, System.IntPtr mdata, int datasize);
    [DllImport(DLL_Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    static extern bool ovARGetMarkerData(System.IntPtr ar, int id, System.IntPtr mdata);
    [DllImport(DLL_Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    static extern void ovARSetMarkerSize(System.IntPtr ar, int value);
    [DllImport(DLL_Name, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    static extern int ovARGetMarkerSize(System.IntPtr ar);


    System.IntPtr _instance;
    COvrvisionUnity _ovr;

    GCHandle marker;
    float[] markerGet;

    //Class
    public OvrvisionAR(COvrvisionUnity ovr,float arMeter)
    {
        markerGet = new float[10];
        marker = GCHandle.Alloc(markerGet, GCHandleType.Pinned);

        _ovr = ovr;
        _instance = ovARCreate(ovr.Instance,arMeter);
    }

    public void Destroy()
    {
        ovARDestroy(_instance);
        _instance = IntPtr.Zero;
        marker.Free();
    }

    public void Update()
    {
        ovARRender(_instance, _ovr.Instance);
    }
    public int OvrvisionGetAR(System.IntPtr mdata, int datasize)
    {
        return ovARGetData(_instance,mdata, datasize);
    }
    public bool OvrvisionGetARbyID( int ID, out Vector3 pos,out Quaternion rot)
    {
         pos = Vector3.zero;
        rot = Quaternion.identity;
        if (!ovARGetMarkerData(_instance, ID, marker.AddrOfPinnedObject()))
            return false;

        pos = new Vector3(markerGet[0], markerGet[1], markerGet[2]);
        rot = new Quaternion(markerGet[3], markerGet[4], markerGet[5], markerGet[6]);
        return true;
    }

    public void SetMarkerSize(int size)
    {
        ovARSetMarkerSize(_instance, size);
    }
    public int GetMarkerSize()
    {
        return ovARGetMarkerSize(_instance);
    }
}
