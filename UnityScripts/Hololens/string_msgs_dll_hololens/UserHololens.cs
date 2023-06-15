using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ROS2;
using ROS2.Utils;

using Newtonsoft.Json;

using Microsoft.MixedReality.Toolkit.UI;

public class UserHololens : MonoBehaviour
{
    public struct sentMessage
    {
        //structure of the message received in node manager
        public string object_id;
        public string user_id;
        public string function;
        public List<float> position;
    }

    public struct receivedMessage
    {
        //structure of the message received in each user
        public string object_id;
        public string user_id;
        public float[] position;
        public bool active;
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

    //IDictionary<string, GameObject> objects = new Dictionary<string, GameObject>();
    IDictionary<string, GameObject> objectsID2GameObjects = new Dictionary<string, GameObject>();
    IDictionary<string, Vector3> objectsID2Positions = new Dictionary<string, Vector3>();

    bool _mousePressed;
    static string _selectedObject = "";
    static GameObject _selectedGameObject;

    float frameRate = 0.04f;

    //it must be initialized from a .init file

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
        chatterPub = talkerNode.CreatePublisher<std_msgs.msg.String>("UserReports");
        chatterSub = listenerNode.CreateSubscription<std_msgs.msg.String>("ManagerNodeCommands",
        msg =>
        {
            ActivityReceived(JsonConvert.DeserializeObject<receivedMessage>(msg.Data));
        });

        StartCoroutine(Interaction());
    }

    static public void InteractionStarted(ManipulationEventData eventReceived)
    {
        _selectedObject = eventReceived.ManipulationSource.transform.name;
        _selectedGameObject = eventReceived.ManipulationSource;
        createMessage("GrabObject", _selectedObject, userUID);
    }

    static public void InteractionEnded(ManipulationEventData eventReceived)
    {
        _selectedObject = "";
        //_selectedGameObject = null;
        createMessage("ReleaseObject", eventReceived.ManipulationSource.transform.name, userUID);
        
    }


    IEnumerator Interaction()
    {
        while (true)
        {
            if (_selectedObject != ""){
                createMessage("ChangePosition", _selectedObject, userUID, _selectedGameObject.transform.position);
            }
            yield return new WaitForSeconds(frameRate);
        }
    }


    void Update()
    {
        RCLdotnet.SpinOnce(listenerNode, 0);
    }

    static void createMessage(string function, string selected_obj, string user_id)
    {
        //for grabbing and releasing
        sentMessage msg = new sentMessage();
        msg.function = function;
        msg.object_id = selected_obj;
        msg.user_id = user_id;

        std_msgs.msg.String rosMsg = new std_msgs.msg.String();
        rosMsg.Data = JsonConvert.SerializeObject(msg);
        chatterPub.Publish(rosMsg);
    }

    static void createMessage(string function, string selected_obj, string user_id, Vector3 pose)
    {
        //for changing position
        sentMessage msg = new sentMessage();
        msg.function = function;
        msg.object_id = selected_obj;
        msg.user_id = user_id;
        msg.position = new List<float>() { pose.x, pose.y, pose.z };

        std_msgs.msg.String rosMsg = new std_msgs.msg.String();
        rosMsg.Data = JsonConvert.SerializeObject(msg);
        chatterPub.Publish(rosMsg);
    }


    void ActivityReceived(receivedMessage msg)
    {
        if (objectsID2GameObjects.ContainsKey(msg.object_id) && (msg.user_id != userUID))
        {
            if (!msg.active)
            {
                //...
            }
            else
            {
                Debug.Log("Selected");
                objectsID2GameObjects[msg.object_id].transform.position = new Vector3(msg.position[0], msg.position[1], msg.position[2]);
                objectsID2Positions[msg.object_id] = new Vector3(msg.position[0], msg.position[1], msg.position[2]);
            }
        }
        else
        {
            //... Create objects
        }
    }
}
