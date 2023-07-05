/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ROS2;
using ROS2.Utils;

public class UserScriptMulti : MonoBehaviour
{
    public GameObject Sphere;
    public String sphereUID = "Sphere";

    public GameObject Square;
    public String squareUID = "Square";

    public string userUID = "user1";

    INode listenerNode;
    INode talkerNode;

    IPublisher<customized_msgs.msg.Communication> chatterPub;
    ISubscription<customized_msgs.msg.Communication> chatterSub;

    customized_msgs.msg.Communication msgSent = new customized_msgs.msg.Communication();

    IDictionary<string, GameObject> objects = new Dictionary<string, GameObject>();

    bool _mousePressed;
    string _selectedObject;

    float frameRate = 0.04f;

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

        talkerNode = RCLdotnet.CreateNode("talkerHololens");
        listenerNode = RCLdotnet.CreateNode("listenerHololens");

        //change type of message
        chatterPub = talkerNode.CreatePublisher<customized_msgs.msg.Communication>("UserReports");
        chatterSub = listenerNode.CreateSubscription<customized_msgs.msg.Communication>("ManagerNodeCommands",
        msg =>
        {
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
                            encryptMessage("GrabObject", _selectedObject);
                        }

                        if (hit.transform.name == "Square")
                        {
                            _mousePressed = true;
                            _selectedObject = hit.transform.name;
                            encryptMessage("GrabObject", _selectedObject);

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
                        encryptMessage("ChangePosition", _selectedObject, new float[3]{result[0], result[1], result[2]});
                    }

                    if (_selectedObject == "Sphere")
                    {
                        var mousePosition = Input.mousePosition;
                        mousePosition.z = 5;
                        Vector3 Point = Camera.main.ScreenToWorldPoint(mousePosition);
                        var result = Point - _previousPositionSphere;
                        encryptMessage("ChangePosition", _selectedObject, new float[3]{result[0], result[1], result[2]});
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
                        encryptMessage("ReleaseObject",  _selectedObject);
                    }

                    if (_selectedObject == "Sphere")
                    {
                        _mousePressed = false;
                        encryptMessage("ReleaseObject", _selectedObject);
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

    void encryptMessage(String function, string selected_obj)
    {
        customized_msgs.msg.Communication rosMsg = new customized_msgs.msg.Communication();
        rosMsg.Obj_id = selected_obj;
        rosMsg.Function = function;
        rosMsg.Reporter_id = userUID;
        //rosMsg.position = new float[3]{0,0,0};

        Debug.Log(rosMsg.Function);
        Debug.Log(rosMsg.Obj_id);
        //Debug.Log(rosMsg.position);

        chatterPub.Publish(rosMsg);
    }

    void encryptMessage(String function, string selected_obj, float[] position)
    {
        customized_msgs.msg.Communication rosMsg = new customized_msgs.msg.Communication();
        rosMsg.Obj_id = selected_obj;
        rosMsg.Function = function;
        rosMsg.Reporter_id = userUID;
        rosMsg.Position = new List<float> {position[0], position[1], position[2]};

        Debug.Log(rosMsg.Function);
        Debug.Log(rosMsg.Obj_id);
        Debug.Log(rosMsg.Position);

        chatterPub.Publish(rosMsg);
    }


    void ActivityReceived(customized_msgs.msg.Communication msg)
    {
        if (objects.ContainsKey(msg.Obj_id))
        {
            if (_selectedObject == "Square")
            {
                objects[msg.Obj_id].transform.position = new Vector3(msg.Position[0], msg.Position[1], msg.Position[2]);
                _previousPositionSquare = new Vector3(msg.Position[0], msg.Position[1], msg.Position[2]);
            }

            if (_selectedObject == "Sphere")
            {
                objects[msg.Obj_id].transform.position = new Vector3(msg.Position[0], msg.Position[1], msg.Position[2]);
                _previousPositionSphere = new Vector3(msg.Position[0], msg.Position[1], msg.Position[2]);
            }

        }

    }

}
*/