using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTracker : MonoBehaviour {

    public enum ControllerID
    {
        None,
        Left,
        Right
    }

    public ControllerID Controller;



    // Use this for initialization
    void Start () {
	}

    void Process()
    {
        var c = OVRInput.Controller.LTouch;
        if (Controller == ControllerID.Left)
            c = OVRInput.Controller.LTouch;
        else if (Controller == ControllerID.Right)
            c = OVRInput.Controller.RTouch;
        else return;
        transform.position = OVRInput.GetLocalControllerPosition(c);
        transform.rotation = OVRInput.GetLocalControllerRotation(c);
   
    }

    // Update is called once per frame
    void Update()
    {
        Process();
    }

}
