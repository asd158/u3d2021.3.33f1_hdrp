using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform moveTarget;

    private Vector3 _defaultPos;

    void Start()
    {
        Application.targetFrameRate = 30;
        _defaultPos = moveTarget.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        moveTarget.transform.localRotation *= Quaternion.Euler(0, 1, 0);
        if (Time.frameCount % 150 == 0)
        {
            ScreenCapture.CaptureScreenshot("ss_" + Time.frameCount + ".png");
        }
    }
}