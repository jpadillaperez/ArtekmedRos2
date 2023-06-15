using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ROS2;
using ROS2.Utils;

using Microsoft.MixedReality.Toolkit.UI;

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

    IDictionary<string, GameObject> objectsID2GameObjects = new Dictionary<string, GameObject>();
    IDictionary<string, Vector3> objectsID2Positions = new Dictionary<string, Vector3>();
    
    bool _mousePressed;
    string _selectedObject = "";
    GameObject _selectedGameObject;

    float frameRate = 0.04f;


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
        objectsID2Positions.Add(sphereUID, new Vector3(0.1f, 0f, 0.3f));
        objectsID2Positions.Add(squareUID, new Vector3(-0.1f, 0f, 0.3f));

        talkerNode = RCLdotnet.CreateNode("talkerHololens");
        listenerNode = RCLdotnet.CreateNode("listenerHololens");

        //change type of message
        chatterPub = talkerNode.CreatePublisher<customized_msgs.msg.Communication>("UserReports");
        chatterSub = listenerNode.CreateSubscription<customized_msgs.msg.Communication>("ManagerNodeCommands",
        msg =>
        {
            Debug.Log(msg.Obj_id);
            Debug.Log(msg.Reporter_id);
            ActivityReceived(msg);
        });

        StartCoroutine(Interaction());
    }

    public void InteractionStarted(ManipulationEventData eventReceived)
    {
        _selectedObject = eventReceived.ManipulationSource.transform.name;
        _selectedGameObject = eventReceived.ManipulationSource;
        encryptMessage("GrabObject", _selectedObject);
    }

    public void InteractionEnded(ManipulationEventData eventReceived)
    {
        _selectedObject = "";
        //_selectedGameObject = null;
        encryptMessage("ReleaseObject", eventReceived.ManipulationSource.transform.name);
        
    }


    IEnumerator Interaction()
    {
        while (true)
        {
            if (_selectedObject != ""){
                encryptMessage("ChangePosition", _selectedObject, _selectedGameObject.transform.position);
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

    void encryptMessage(String function, string selected_obj, Vector3 position)
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
        if (objectsID2GameObjects.ContainsKey(msg.Obj_id) && (msg.Reporter_id != userUID))
        {
            //if (!msg.active)
            //{
                //...
            //}
            //else
            //{
                Debug.Log("Position Received: " + msg.Position[0] + msg.Position[1] + msg.Position[2]);
                Debug.Log(msg.Reporter_id + " vs " + userUID);
                objectsID2GameObjects[msg.Obj_id].transform.position = new Vector3(msg.Position[0], msg.Position[1], msg.Position[2]);
                objectsID2Positions[msg.Obj_id] = new Vector3(msg.Position[0], msg.Position[1], msg.Position[2]);
            //}

        }
        else
        {
            //... Create objects
        }
    }

}