using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text.Json;

using UnityEngine;

using ROS2;
using ROS2.Utils;

public class UserScript : MonoBehaviour
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
            _previousPositionSquare = Square.transform.position; //super temporal, it should wait for the response of the manager

        });

        StartCoroutine(Interaction());
    }

    IEnumerator Interaction()
    {
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                //print("mouse detected");
                if (_mousePressed == false)
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
                            //hit.transform.GetInstanceID(); in the future
                            //print("Interaction detected!");
                            //print(hit.point);
                            var result = hit.point - _previousPositionSphere;
                            print("Movement: " + result);
                            encryptMessage(_selectedObject, result); //probably not needed
                                                                     //encryptMessage(_selectedObject, hit.point);
                                                                     //_previousPositionSphere = Sphere.transform.position; //super temporal, it should wait for the response of the manager
                        }

                        if (hit.transform.name == "Square")
                        {
                            _mousePressed = true;
                            _selectedObject = hit.transform.name;

                            //hit.transform.GetInstanceID(); in the future

                            //print("Interaction detected!");
                            //print(hit.point);
                            var result = hit.point - _previousPositionSquare;
                            print("Movement: " + result);
                            encryptMessage(_selectedObject, result); //probably not needed
                                                                     //encryptMessage(_selectedObject, hit.point);
                                                                     //_previousPositionSquare = Square.transform.position; //super temporal, it should wait for the response of the manager

                        }
                    }
                }
                else
                {
                    if (_selectedObject == "Square")
                    {
                        //Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        var mousePosition = Input.mousePosition;
                        mousePosition.z = 5;
                        Vector3 Point = Camera.main.ScreenToWorldPoint(mousePosition);

                        //print("hold interaction");
                        //print(Point);
                        var result = Point - _previousPositionSquare;
                        print("Movement: " + result);
                        encryptMessage(_selectedObject, result);
                    }

                    if (_selectedObject == "Sphere")
                    {
                        //Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        var mousePosition = Input.mousePosition;
                        mousePosition.z = 5;
                        Vector3 Point = Camera.main.ScreenToWorldPoint(mousePosition);

                        //print("hold interaction");
                        //print(Point);
                        var result = Point - _previousPositionSphere;
                        print("Movement: " + result);
                        encryptMessage(_selectedObject, result);
                    }
                }
            }
            else
            {
                _mousePressed = false;
            }

            yield return new WaitForSeconds(frameRate); //because right now it is reporting before the object is updated
        }
    }

    void Update()
    {
        //Receiving a change in position
        RCLdotnet.SpinOnce(listenerNode, 0);


        //Example by now (not ok, it shouldn't change anything)

    }

    void encryptMessage(String UID, Vector3 Position)
    {
        //This would be the message sent
        //sentMessage msg = new sentMessage();
        //msg.object_id = UID;
        //msg.user_id = userUID;
        //msg.execution.Add("ChangePosition", new float[] {Position.x, Position.y, Position.z});


        //Convert it to Json
        //string jsonString = JsonUtility.ToJson(msg);

        
        //But we're actually sending just a string with the following order
        //object_id?function!argument1;argument2;argument3/function!argument1;argument2;argument3
        //string jsonString = UID + "?" + "ChangePosition" + "!" + Position.x.ToString() + ";" + Position.y.ToString() + ";" + Position.z.ToString();

        //user_id#object_id?function!argument1;argument2;argument3/function!argument1;argument2;argument3
        string jsonString = userUID + "#" + UID + "?" + "ChangePosition" + "!" + Position.x.ToString() + ";" + Position.y.ToString() + ";" + Position.z.ToString();


        std_msgs.msg.String rosMsg = new std_msgs.msg.String();
        rosMsg.Data = jsonString;
        //Publishing message
        chatterPub.Publish(rosMsg);
    }

    void ActivityReceived(receivedMessage msg)
    {
        if (objects.ContainsKey(msg.object_id))
        {
            if (_selectedObject == "Square")
            {
                objects[msg.object_id].transform.position = new Vector3(msg.args[0], msg.args[1], msg.args[2]);
                //objects[msg.object_id].transform.rotation = new Quaternion(msg.args[3], msg.args[4], msg.args[5], msg.args[6]);

                //_previousPositionSquare = Square.transform.position; //super temporal, it should wait for the response of the manager
                //_previousPositionSphere = Sphere.transform.position; //super temporal, it should wait for the response of the manager

                _previousPositionSquare = new Vector3(msg.args[0], msg.args[1], msg.args[2]);
            }

            if (_selectedObject == "Sphere")
            {
                objects[msg.object_id].transform.position = new Vector3(msg.args[0], msg.args[1], msg.args[2]);
                //objects[msg.object_id].transform.rotation = new Quaternion(msg.args[3], msg.args[4], msg.args[5], msg.args[6]);

                //_previousPositionSquare = Square.transform.position; //super temporal, it should wait for the response of the manager
                //_previousPositionSphere = Sphere.transform.position; //super temporal, it should wait for the response of the manager

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