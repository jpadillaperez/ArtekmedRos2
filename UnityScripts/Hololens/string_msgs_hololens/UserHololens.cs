using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ROS2;
using ROS2.Utils;

using Microsoft.MixedReality.Toolkit.UI;

public class UserHololens : MonoBehaviour
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

        public string user_worker;
        public float[] args;
        //args[0-2] -> Position x,y,z
    }

    public GameObject Sphere;
    public String sphereUID = "Sphere";

    public GameObject Square;
    public String squareUID = "Square";

    public static string userUID = "user2";

    INode listenerNode;
    INode talkerNode;

    public static IPublisher<std_msgs.msg.String> chatterPub;
    public static ISubscription<std_msgs.msg.String> chatterSub;

    std_msgs.msg.String msgSent = new std_msgs.msg.String();

    IDictionary<string, GameObject> objects = new Dictionary<string, GameObject>();

    bool _mousePressed;
    static string _selectedObject;
    static GameObject _selectedGameObject;

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
            Debug.Log("Message received!");
            ActivityReceived(msgd);
        });

        StartCoroutine(Interaction());
    }

    static public void InteractionStarted(ManipulationEventData eventReceived)
    {
        Debug.Log(eventReceived.ManipulationSource.transform.name);
        if (eventReceived.ManipulationSource.transform.name == "Sphere")
        {
            _selectedObject = eventReceived.ManipulationSource.transform.name;
            encryptMessage("GrabObject", new string[] { _selectedObject, userUID });
            _selectedGameObject = eventReceived.ManipulationSource;
        }

        if (eventReceived.ManipulationSource.transform.name == "Square")
        {
            _selectedObject = eventReceived.ManipulationSource.transform.name;
            encryptMessage("GrabObject", new string[] { _selectedObject, userUID });
            _selectedGameObject = eventReceived.ManipulationSource;
        }
    }

    static public void InteractionEnded(ManipulationEventData eventReceived)
    {
        if (eventReceived.ManipulationSource.transform.name == "Sphere")
        {
            _selectedObject = eventReceived.ManipulationSource.transform.name;
            encryptMessage("ReleaseObject", new string[] { _selectedObject, userUID });
            _selectedObject = "";
        }

        if (eventReceived.ManipulationSource.transform.name == "Square")
        {
            _selectedObject = eventReceived.ManipulationSource.transform.name;
            encryptMessage("ReleaseObject", new string[] { _selectedObject, userUID });
            _selectedObject = "";
        }
    }


    IEnumerator Interaction()
    {
        while (true)
        {
            if (_selectedObject == "Sphere" || _selectedObject == "Square")
            {
                encryptMessage("ChangePosition", new string[] { _selectedObject, userUID, _selectedGameObject.transform.position[0].ToString(), _selectedGameObject.transform.position[1].ToString(), _selectedGameObject.transform.position[2].ToString() });
            }
            yield return new WaitForSeconds(frameRate);
        }
    }


    void Update()
    {
        RCLdotnet.SpinOnce(listenerNode, 0);
    }

    public static void encryptMessage(String function, string[] args)
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
        if (objects.ContainsKey(msg.object_id) && (msg.user_worker != userUID))
        {
            if (msg.object_id == "Square")
            {
                objects[msg.object_id].transform.position = new Vector3(msg.args[0], msg.args[1], msg.args[2]);
            }

            if (msg.object_id == "Sphere")
            {
                objects[msg.object_id].transform.position = new Vector3(msg.args[0], msg.args[1], msg.args[2]);
            }
        }

    }

    receivedMessage decryptMessage(std_msgs.msg.String stringMsg)
    {
        //structure of the message received from node manager
        //object_id?posx,posy,posz
        receivedMessage msg = new receivedMessage();
        string[] usersending = stringMsg.Data.Split('!', StringSplitOptions.RemoveEmptyEntries);
        msg.user_worker = usersending[0]; //object_id

        string[] result = usersending[1].Split('?', StringSplitOptions.RemoveEmptyEntries);
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
