using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

using ROS2;
using ROS2.Utils;

using Newtonsoft.Json;

public class UserStringHash : MonoBehaviour
{
    public struct sentMessage
    {
        //structure of the message sent to node manager
        public string Obj_id;
        public string User_id;
        public string Function;
        public List<float> Position;
    }

    public struct receivedMessage
    {
        //structure of the message received by each user
        public string Obj_id;
        public float[] Position;
        //args[0-2] -> Position x,y,z
    }

    public GameObject Sphere;
    public String sphereUID = "Sphere";

    public GameObject Square;
    public String squareUID = "Square";

    public string userUID = "user2";

    INode listenerNode;
    INode talkerNode;

    IPublisher<std_msgs.msg.String> chatterPub;
    ISubscription<std_msgs.msg.String> chatterSub;

    std_msgs.msg.String msgSent = new std_msgs.msg.String();

    IDictionary<string, GameObject> objectsID2GameObjects = new Dictionary<string, GameObject>();
    IDictionary<string, Vector3> objectsID2Positions = new Dictionary<string, Vector3>();
    bool _mousePressed;
    string _selectedObject;

    float frameRate = 0.2f;

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

        objectsID2GameObjects.Add(sphereUID, Sphere);
        objectsID2GameObjects.Add(squareUID, Square);
        objectsID2Positions.Add(sphereUID, new Vector3(1.5f, 0f, 5));
        objectsID2Positions.Add(squareUID, new Vector3(1.5f, 0f, 5));

        talkerNode = RCLdotnet.CreateNode("talker");
        listenerNode = RCLdotnet.CreateNode("listener");

        //change type of message
        chatterPub = talkerNode.CreatePublisher<std_msgs.msg.String>("UserReports", QosProfile.Profile.Default);
        chatterSub = listenerNode.CreateSubscription<std_msgs.msg.String>("ManagerNodeCommands",
        msg =>
        {
            ActivityReceived(JsonConvert.DeserializeObject<receivedMessage>(msg.Data));
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
                        Debug.Log("Object Grabbed: " + hit.transform.name); //edit to recognize new objects
                        _mousePressed = true;
                        _selectedObject = hit.transform.name;
                        createMessage("GrabObject", _selectedObject, userUID);
                        //hit.transform.GetInstanceID(); in the future

                    }
                    else
                    {
                        Debug.Log("Object Created");
                        _mousePressed = true;
                        createMessage("CreateObject", null, userUID, ray.GetPoint(5));
                    }

                }
                else //ChangePosition
                {
                    Debug.Log("Change Position");
                    var mousePosition = Input.mousePosition;
                    mousePosition.z = 5;
                    Vector3 Point = Camera.main.ScreenToWorldPoint(mousePosition);
                    var result = Point - objectsID2Positions[_selectedObject];
                    createMessage("ChangePosition", _selectedObject, userUID, result);
                }
            }
            else //ReleaseObject
            {
                if (_mousePressed == true)
                {
                    Debug.Log("Release Object");
                    _mousePressed = false;
                    createMessage("ReleaseObject", _selectedObject, userUID);
                }
            }

            yield return new WaitForSeconds(frameRate);
        }
    }

    void Update()
    {
        RCLdotnet.SpinOnce(listenerNode, 0);
    }

    void createMessage(string function, string selected_obj, string user_id)
    {
        sentMessage msg = new sentMessage();
        msg.Function = function;
        msg.Obj_id = selected_obj;
        msg.User_id = user_id;

        std_msgs.msg.String rosMsg = new std_msgs.msg.String();
        rosMsg.Data = JsonConvert.SerializeObject(msg);
        chatterPub.Publish(rosMsg);
    }
    void createMessage(string function, string selected_obj, string user_id, Vector3 pose)
    {
        sentMessage msg = new sentMessage();
        msg.Function = function;
        msg.Obj_id = selected_obj;
        msg.User_id = user_id;
        msg.Position = new List<float>() { pose.x, pose.y, pose.z };

        std_msgs.msg.String rosMsg = new std_msgs.msg.String();
        rosMsg.Data = JsonConvert.SerializeObject(msg);
        chatterPub.Publish(rosMsg);
    }


    void ActivityReceived(receivedMessage msg)
    {
        //Debug.Log("Message Detected!");
        if (objectsID2GameObjects.ContainsKey(msg.Obj_id))
        {
            Debug.Log("Selected");
            objectsID2GameObjects[msg.Obj_id].transform.position = new Vector3(msg.Position[0], msg.Position[1], msg.Position[2]);
            objectsID2Positions[msg.Obj_id] = new Vector3(msg.Position[0], msg.Position[1], msg.Position[2]);
        }
        else
        {
            //add object to dictionary with position
            //generate object in unity
            Debug.Log("Received Created Object!");
            GameObject tempObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tempObject.transform.SetParent(Sphere.transform.parent);
            tempObject.transform.name = msg.Obj_id;
            _selectedObject = msg.Obj_id;
            objectsID2GameObjects.Add(msg.Obj_id, GameObject.CreatePrimitive(PrimitiveType.Sphere));
            objectsID2Positions.Add(msg.Obj_id, new Vector3(msg.Position[0], msg.Position[1], msg.Position[2]));
            objectsID2GameObjects[msg.Obj_id].transform.position = new Vector3(msg.Position[0], msg.Position[1], msg.Position[2]);
        }

    }

}

