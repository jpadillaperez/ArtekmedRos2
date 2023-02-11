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

namespace ConsoleApplication
{    
    public class Position{
        public float x;
        public float y;
        public float z;
    }

    public class Gameobject{
        public Position position = new Position();
        public string isLockedBy = "";
    }

    public class node_manager_function
    {
        public static IPublisher<customized_msgs.msg.Communication> chatterPub;
        public static ISubscription<customized_msgs.msg.Communication> chatterSub;

        public static IDictionary<string, Gameobject> objects = new Dictionary<string, Gameobject>();

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

            objects.Add(sphereUID, Sphere);
            objects.Add(squareUID, Square);

            INode talkerNode = RCLdotnet.CreateNode("talker");
            INode listenerNode = RCLdotnet.CreateNode("listener");

            chatterPub = talkerNode.CreatePublisher<customized_msgs.msg.Communication>("ManagerNodeCommands");
            chatterSub = listenerNode.CreateSubscription<customized_msgs.msg.Communication>("UserReports",
            msg =>
            {
                Console.WriteLine("Received Order");
                Console.WriteLine("Function: " + msg.Function);
                Console.WriteLine("Object ID: " + msg.Obj_id);
                Console.WriteLine("User ID: " + msg.Reporter_id + "\n");
                
                manager.applyFunctionality(msg);
                //Thread.Sleep(50);
            });

            //Listening to changes from users
            RCLdotnet.Spin(listenerNode);
        }

        //Once a message is received
        private void applyFunctionality(customized_msgs.msg.Communication msg)
        {
            //Console.WriteLine("Received something");
            if (objects.ContainsKey(msg.Obj_id))
            {
                Type thisType = this.GetType();
                MethodInfo theMethod = thisType.GetMethod(msg.Function, BindingFlags.NonPublic | BindingFlags.Instance);
                theMethod.Invoke(this, new object[] { msg.Obj_id, msg.Reporter_id, msg.Position}); //notice that the third argument received is actually a List<float>
            } 
        }

        //Functionalities
        private void ChangePosition(string object_id, string user_id, List<float> pose)
        {
            //args: args[0]=user_id, args[1-2]=deltax-y
            if (objects[object_id].isLockedBy == user_id)
            {
                objects[object_id].position.x = objects[object_id].position.x + pose[0];
                objects[object_id].position.y = objects[object_id].position.y + pose[1];

                node_manager_function manager = new node_manager_function();
                customized_msgs.msg.Communication msg = manager.GenerateMessage(object_id);

                Console.WriteLine("Position Changed");
                chatterPub.Publish(msg);
            }
        }

        private customized_msgs.msg.Communication GenerateMessage(string obj_id)
        {
            customized_msgs.msg.Communication msg = new customized_msgs.msg.Communication();

            msg.Obj_id = obj_id;
            msg.Position = new List<float> {objects[obj_id].position.x, objects[obj_id].position.y, objects[obj_id].position.z };

            return msg;
        }

        private void GrabObject(string object_id, string user_id, List<float> _)
        {
            if (objects[object_id].isLockedBy == "")
            {
                objects[object_id].isLockedBy = user_id;
            }
        }

        private void ReleaseObject(string object_id, string user_id, List<float> _)
        {
            if (objects[object_id].isLockedBy == user_id)
            {
                objects[object_id].isLockedBy = "";
            }
        }

    }
}
