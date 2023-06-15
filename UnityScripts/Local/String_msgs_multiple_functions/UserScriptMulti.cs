using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ROS2;
using ROS2.Utils;

public class UserScriptMulti : MonoBehaviour
{
    public struct sentMessage
    {
        //structure of the message received in node manager
        public string object_id;
        public string user_id;

        public Dictionary<string, float[]> execution; //functions and arguments accordingly
    }

    public struct receivedMessage
    {
        //structure of the message received in each user
        public string object_id;
        public float[] args;
        //args[0-2] -> Position x,y,z
    }

    public GameObject Sphere;
    public String sphereUID = "Sphere";

    public GameObject Square;
    public String squareUID = "Square";

    public string userUID = "user1";

    INode listenerNode;
    INode talkerNode;

    IPublisher<std_msgs.msg.String> chatterPub;
    ISubscription<std_msgs.msg.String> chatterSub;

    std_msgs.msg.String msgSent = new std_msgs.msg.String();

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

        talkerNode = RCLdotnet.CreateNode("talker");
        listenerNode = RCLdotnet.CreateNode("listener");

        //change type of message
        chatterPub = talkerNode.CreatePublisher<std_msgs.msg.String>("UserReports");
        chatterSub = listenerNode.CreateSubscription<std_msgs.msg.String>("ManagerNodeCommands",
        msg =>
        {
            receivedMessage msgd = decryptMessage(msg);
            ActivityReceived(msgd);
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
                            encryptMessage("GrabObject", new string[] { _selectedObject, userUID });
                        }

                        if (hit.transform.name == "Square")
                        {
                            _mousePressed = true;
                            _selectedObject = hit.transform.name;
                            encryptMessage("GrabObject", new string[] { _selectedObject, userUID });

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
                        encryptMessage("ChangePosition", new string[] { _selectedObject, userUID, result[0].ToString(), result[1].ToString(), result[2].ToString() });
                    }

                    if (_selectedObject == "Sphere")
                    {
                        var mousePosition = Input.mousePosition;
                        mousePosition.z = 5;
                        Vector3 Point = Camera.main.ScreenToWorldPoint(mousePosition);
                        var result = Point - _previousPositionSphere;
                        encryptMessage("ChangePosition", new string[] { _selectedObject, userUID, result[0].ToString(), result[1].ToString(), result[2].ToString() });
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
                        encryptMessage("ReleaseObject", new string[] { _selectedObject, userUID });
                    }

                    if (_selectedObject == "Sphere")
                    {
                        _mousePressed = false;
                        encryptMessage("ReleaseObject", new string[] { _selectedObject, userUID });
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

    void encryptMessage(String function, string[] args)
    {
        string jsonString = function + "!";
        foreach (var arg in args)
        {
            jsonString = jsonString + arg + ";";
        }

        std_msgs.msg.String rosMsg = new std_msgs.msg.String();
        rosMsg.Data = jsonString;
        Debug.Log(jsonString);
        chatterPub.Publish(rosMsg);
    }


    void ActivityReceived(receivedMessage msg)
    {
        if (objects.ContainsKey(msg.object_id))
        {
            if (_selectedObject == "Square")
            {
                objects[msg.object_id].transform.position = new Vector3(msg.args[0], msg.args[1], msg.args[2]);
                _previousPositionSquare = new Vector3(msg.args[0], msg.args[1], msg.args[2]);
            }

            if (_selectedObject == "Sphere")
            {
                objects[msg.object_id].transform.position = new Vector3(msg.args[0], msg.args[1], msg.args[2]);
                _previousPositionSphere = new Vector3(msg.args[0], msg.args[1], msg.args[2]);
            }

        }

    }

    receivedMessage decryptMessage(std_msgs.msg.String stringMsg)
    {
        //structure of the message received from node manager
        //object_id?posx,posy,posz

        receivedMessage msg = new receivedMessage();
        string[] result = stringMsg.Data.Split('?', StringSplitOptions.RemoveEmptyEntries);
        msg.object_id = result[0]; //object_id

        string[] buffer = result[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
        float[] buffer_float = new float[buffer.Length]; //arguments
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer_float[i] = float.Parse(buffer[i]);
        }
        msg.args = buffer_float;


        //receivedMessage msg = JsonUtility.FromJson<receivedMessage>(stringMsg.Data);

        return msg;
    }

}