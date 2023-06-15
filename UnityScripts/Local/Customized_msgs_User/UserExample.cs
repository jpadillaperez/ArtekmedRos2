using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections;
using System.Globalization;

using UnityEngine;

using ROS2;
using ROS2.Utils;

public class UserExample : MonoBehaviour
{
    public GameObject Sphere;
    public String sphereUID = "Sphere";

    public GameObject Square;
    public String squareUID = "Square";

    public string userUID = "user2";

    INode listenerNode;
    INode talkerNode;

    IPublisher<customized_msgs.msg.Communication> chatterPub;
    ISubscription<customized_msgs.msg.Communication> chatterSub;

    customized_msgs.msg.Communication msgSent = new customized_msgs.msg.Communication();

    IDictionary<string, GameObject> objects = new Dictionary<string, GameObject>();

    bool _mousePressed;
    string _selectedObject;

    float frameRate = 0.05f;

    //it must be initialized from a .init file
    Vector3 _previousPositionSquare = new Vector3(1.5f, 0f, 5);
    Vector3 _previousPositionSphere = new Vector3(-1.5f, 0f, 5);
    void Start()
    {
        try
        {
            RCLdotnet.Init();
        }
        catch (UnsatisfiedLinkError e)
        {
            Debug.Log(e.ToString());
        }

        objects.Add(sphereUID, Sphere);
        objects.Add(squareUID, Square);

        talkerNode = RCLdotnet.CreateNode("talker");
        listenerNode = RCLdotnet.CreateNode("listener");

        //change type of message
        chatterPub = talkerNode.CreatePublisher<customized_msgs.msg.Communication>("UserReports");
        chatterSub = listenerNode.CreateSubscription<customized_msgs.msg.Communication>("ManagerNodeCommands",
        msg =>
        {

            Debug.Log("Message Received");
            ActivityReceived(msg);
        });

        StartCoroutine(Interaction());
    }

    IEnumerator Interaction()
    {
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                if (_mousePressed == false) //GrabObject
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        //these ifs are just temporal
                        if (hit.transform.name == "Sphere")
                        {
                            _mousePressed = true;
                            _selectedObject = hit.transform.name;
                            createMessage("GrabObject", _selectedObject, userUID);
                        }

                        if (hit.transform.name == "Square")
                        {
                            _mousePressed = true;
                            _selectedObject = hit.transform.name;
                            createMessage("GrabObject", _selectedObject, userUID);

                            //hit.transform.GetInstanceID(); in the future
                        }
                    }
                }
                else //ChangePosition
                {
                    if (_selectedObject == "Square")
                    {
                        var mousePosition = Input.mousePosition;
                        mousePosition.z = 5;
                        Vector3 Point = Camera.main.ScreenToWorldPoint(mousePosition);
                        var result = Point - _previousPositionSquare;
                        createMessage("ChangePosition", _selectedObject, userUID, result);
                    }

                    if (_selectedObject == "Sphere")
                    {
                        var mousePosition = Input.mousePosition;
                        mousePosition.z = 5;
                        Vector3 Point = Camera.main.ScreenToWorldPoint(mousePosition);
                        var result = Point - _previousPositionSphere;
                        createMessage("ChangePosition", _selectedObject, userUID, result);
                    }
                }
            }
            else //ReleaseObject
            {
                if (_mousePressed == true)
                {
                    if (_selectedObject == "Square")
                    {
                        _mousePressed = false;
                        createMessage("ReleaseObject", _selectedObject, userUID);
                    }

                    if (_selectedObject == "Sphere")
                    {
                        _mousePressed = false;
                        createMessage("ReleaseObject", _selectedObject, userUID);
                    }
                }
            }

            yield return new WaitForSeconds(frameRate);
        }
    }

    void Update()
    {
        RCLdotnet.SpinOnce(listenerNode, 0);
    }

    void createMessage(String function, string selected_obj, string user_id)
    {
        customized_msgs.msg.Communication msg = new customized_msgs.msg.Communication();
        msg.Function = function;
        msg.Obj_id = selected_obj;
        msg.Reporter_id = user_id;

        chatterPub.Publish(msg);
    }
    void createMessage(String function, string selected_obj, string user_id, Vector3 pose)
    {
        customized_msgs.msg.Communication msg = new customized_msgs.msg.Communication();
        msg.Function = function;
        msg.Obj_id = selected_obj;
        msg.Reporter_id = user_id;
        msg.Position = new List<float>() { pose.x, pose.y, pose.z };

        chatterPub.Publish(msg);
    }


    void ActivityReceived(customized_msgs.msg.Communication msg)
    {
        Debug.Log("Message Detected!");
        if (objects.ContainsKey(msg.Obj_id))
        {
            if (_selectedObject == "Square")
            {
                Debug.Log("Selected Square!");
                objects[msg.Obj_id].transform.position = new Vector3(msg.Position[0], msg.Position[1], msg.Position[2]);
                _previousPositionSquare = new Vector3(msg.Position[0], msg.Position[1], msg.Position[2]);
            }

            if (_selectedObject == "Sphere")
            {
                Debug.Log("Selected Square!");
                objects[msg.Obj_id].transform.position = new Vector3(msg.Position[0], msg.Position[1], msg.Position[2]);
                _previousPositionSphere = new Vector3(msg.Position[0], msg.Position[1], msg.Position[2]);
            }


        }

    }
}

