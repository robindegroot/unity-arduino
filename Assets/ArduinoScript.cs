﻿using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class ArduinoScript : MonoBehaviour
{

    SerialPort stream = new SerialPort("COM4", 9600); 
    float[] lastRot = { 0, 0, 0 }; //Need the last rotation to tell how far to spin the camera


    void Start()
    {
        stream.Open(); //Open the Serial Stream.
    }

    // Update is called once per frame
    void Update()
    {
        string treatValue = stream.ReadLine();
        Debug.Log(treatValue);

        string[] vec3 = treatValue.Split(','); //My arduino script returns a 3 part value (IE: 12,30,18)
        if (vec3[0] != "" && vec3[1] != "" && vec3[2] != "") //Check if all values are recieved
        {
            transform.Rotate(                  //Rotate the camera based on the new values
                                          float.Parse(vec3[0]) - lastRot[0],
                                          float.Parse(vec3[1]) - lastRot[1],
                                          float.Parse(vec3[2]) - lastRot[2],
                                          Space.Self
                                    );
            lastRot[0] = float.Parse(vec3[0]);  //Set new values to last time values for the next loop
            lastRot[1] = float.Parse(vec3[1]);
            lastRot[2] = float.Parse(vec3[2]);
            stream.BaseStream.Flush(); //Clear the serial information so we assure we get new information.
        }
    }

    void OnGUI()
    {
        string newString = "Connected: " + transform.rotation.x + ", " + transform.rotation.y + ", " + transform.rotation.z;
        GUI.Label(new Rect(10, 10, 300, 100), newString); //Display new values
                                                          // Though, it seems that it outputs the value in percentage O-o I don't know why.
    }
}