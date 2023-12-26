using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using File = UnityEngine.Windows.File;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform moveTarget;
    public Camera _mainCamera;
    private Vector3 _defaultPos;
    private RenderTexture _newRt;
    private readonly List<AsyncGPUReadbackRequest> _readbackQueue = new(10);
    private FileStream _dumpWriter;

    IEnumerator Start()
    {
        Application.targetFrameRate = 30;
        _defaultPos = moveTarget.localPosition;
        _newRt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.DefaultHDR, 0)
        {
            antiAliasing = 1
        };
        _mainCamera.targetTexture = _newRt;
        var vidFileName = Screen.width + "x" + Screen.height + ".argb32";
        _dumpWriter = new FileStream(vidFileName, FileMode.OpenOrCreate,
            FileAccess.Write, FileShare.None);
        for (var eof = new WaitForEndOfFrame();;)
        {
            yield return eof;
            var camRt = _mainCamera.targetTexture;
            var rtt = RenderTexture.GetTemporary(camRt.width, camRt.height, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(_mainCamera.targetTexture, rtt);
            _readbackQueue.Add(AsyncGPUReadback.Request(rtt));
            RenderTexture.ReleaseTemporary(rtt);
        }
    }

    private void OnDestroy()
    {
        _dumpWriter.Close();
        _dumpWriter.Dispose();
    }

    private void DumpVid()
    {
        while (_readbackQueue.Count > 0)
        {
            if (!_readbackQueue[0].done)
            {
                if (_readbackQueue.Count > 1 && _readbackQueue[1].done)
                {
                    _readbackQueue[0].WaitForCompletion();
                }
                else
                {
                    break;
                }
            }

            var req = _readbackQueue[0];
            _readbackQueue.RemoveAt(0);
            if (req.hasError)
            {
                continue;
            }

            var naa = req.GetData<byte>();
            var bb = naa.ToArray();
            _dumpWriter.Write(bb);
            _dumpWriter.Flush();
        }
    }

    void Update()
    {
        moveTarget.transform.localRotation *= Quaternion.Euler(0, 1, 0);
        DumpVid();
    }
}