using System;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using ROS2;
using ROS2.Utils;

using Newtonsoft.Json;

namespace ConsoleApplication
{
    public class Position
    {
        public float x;
        public float y;
        public float z;
    }

    public class Gameobject
    {
        public Position position = new Position();
        public string isLockedBy = "";
    }

    public struct receivedMessage
    {
        //structure of the message received in node manager
        public string object_id;
        public string function;
        public string user_id;

        public string[] position; //functions and arguments accordingly
    }

    public struct sentMessage
    {
        //structure of the message received in each user
        public string object_id;
        public string user_id;
        public float[] position;
        public bool active;
        //args[0-2] -> Position x,y,z
        //args[3-6] -> Rotation x,y,z,w
    }

    public class node_manager_function
    {
        public static IPublisher<std_msgs.msg.String> chatterPub;
        public static ISubscription<std_msgs.msg.String> chatterSub;

        public static IDictionary<string, Gameobject> objectsID2GameObjects = new Dictionary<string, Gameobject>();
        public static void Main(string[] args)
        {

            RCLdotnet.Init();

            node_manager_function manager = new node_manager_function();

            Gameobject Sphere = new Gameobject();
            Sphere.position = new Position();
            String sphereUID = "Sphere";

            Gameobject Square = new Gameobject();
            Square.position = new Position();
            String squareUID = "Square";

            Sphere.position.x = -1.5f;
            Sphere.position.y = 0f;
            Sphere.position.z = 5f;

            Square.position.x = 1.5f;
            Square.position.y = 0f;
            Square.position.z = 5f;

            objectsID2GameObjects.Add(sphereUID, Sphere);
            objectsID2GameObjects.Add(squareUID, Square);

            INode talkerNode = RCLdotnet.CreateNode("talker");
            INode listenerNode = RCLdotnet.CreateNode("listener");

            
            chatterPub = talkerNode.CreatePublisher<std_msgs.msg.String>("ManagerNodeCommands");
            Console.WriteLine("Initialized Publisher");
            chatterSub = listenerNode.CreateSubscription<std_msgs.msg.String>("UserReports",
            msg =>
            {
                //receivedMessage msgd = manager.decryptMessage(msg);
                //manager.applyFunctionality(msgd);

                manager.applyFunctionality(JsonConvert.DeserializeObject<receivedMessage>(msg.Data));
                //Thread.Sleep(50);
            });
            Console.WriteLine("Initialized Subscriber");

            //Listening to changes from users
            RCLdotnet.Spin(listenerNode);
        }

        private void applyFunctionality(receivedMessage msg)
        {
            Console.WriteLine("Received message");

            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(msg.function, BindingFlags.NonPublic | BindingFlags.Instance);
            theMethod.Invoke(this, new object[] { msg.object_id, msg.user_id, msg.position });
        }

        private std_msgs.msg.String GenerateMessage(string object_id, string user_id)
        {
            sentMessage msg = new sentMessage();
            msg.object_id = object_id;
            msg.user_id = user_id;
            msg.position = new float[3] { objectsID2GameObjects[object_id].position.x, objectsID2GameObjects[object_id].position.y, objectsID2GameObjects[object_id].position.z };
            msg.active = true;

            std_msgs.msg.String rosMsg = new std_msgs.msg.String();
            rosMsg.Data = JsonConvert.SerializeObject(msg);
            return rosMsg;
        }

        private void ChangePosition(string object_id, string user_id, string[] position)
        {
            //args: args[0]=user_id, args[1-2]=deltax-y
            if (objectsID2GameObjects[object_id].isLockedBy == user_id)
            {
                objectsID2GameObjects[object_id].position.x = float.Parse(position[0], CultureInfo.InvariantCulture.NumberFormat);
                objectsID2GameObjects[object_id].position.y = float.Parse(position[1], CultureInfo.InvariantCulture.NumberFormat);
                objectsID2GameObjects[object_id].position.z = float.Parse(position[2], CultureInfo.InvariantCulture.NumberFormat);

                node_manager_function manager = new node_manager_function();
                std_msgs.msg.String msg = manager.GenerateMessage(object_id, user_id);
                Console.WriteLine("Output message: ");
                Console.WriteLine(msg.Data);
                chatterPub.Publish(msg);
            }
        }

        private void GrabObject(string object_id, string user_id, string[] _)
        {
            if (objectsID2GameObjects[object_id].isLockedBy == "")
            {
                objectsID2GameObjects[object_id].isLockedBy = user_id;
            }
        }

        private void ReleaseObject(string object_id, string user_id, string[] _)
        {
            if (objectsID2GameObjects[object_id].isLockedBy == user_id)
            {
                objectsID2GameObjects[object_id].isLockedBy = "";
            }
        }
    }
}
