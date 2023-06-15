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

        public string[] args; //functions and arguments accordingly
    }

    public struct sentMessage
    {
        //structure of the message received in each user
        public string object_id;
        public string user_id;
        public float[] args;
        //args[0-2] -> Position x,y,z
        //args[3-6] -> Rotation x,y,z,w
    }

    public class node_manager_function
    {
        public static IPublisher<std_msgs.msg.String> chatterPub;
        public static ISubscription<std_msgs.msg.String> chatterSub;

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

            
            chatterPub = talkerNode.CreatePublisher<std_msgs.msg.String>("ManagerNodeCommands");
            Console.WriteLine("Initialized Publisher");
            chatterSub = listenerNode.CreateSubscription<std_msgs.msg.String>("UserReports",
            msg =>
            {
                receivedMessage msgd = manager.decryptMessage(msg);
                manager.applyFunctionality(msgd);
                //Thread.Sleep(50);
            });
            Console.WriteLine("Initialized Publisher");

            //Listening to changes from users
            RCLdotnet.Spin(listenerNode);
        }

        private void applyFunctionality(receivedMessage msg)
        {
            Console.WriteLine("Received something");
            if (objects.ContainsKey(msg.object_id))
            {
                Type thisType = this.GetType();
                MethodInfo theMethod = thisType.GetMethod(msg.function, BindingFlags.NonPublic | BindingFlags.Instance);
                theMethod.Invoke(this, new object[] { msg.object_id, msg.user_id, msg.args });
            }
        }

        private std_msgs.msg.String GenerateMessage(string obj_id, string user_id)
        {
            string msg = user_id + "!" + obj_id + "?" + objects[obj_id].position.x.ToString() + "," + objects[obj_id].position.y.ToString() + "," + objects[obj_id].position.z.ToString();

            //string msg = obj_id + "?" + objects[obj_id].position.x.ToString() + "," + objects[obj_id].position.y.ToString() + "," + objects[obj_id].position.z.ToString();
            std_msgs.msg.String rosMsg = new std_msgs.msg.String();
            rosMsg.Data = msg;

            return rosMsg;
        }

        private void ChangePosition(string object_id, string user_id, string[] args)
        {
            //args: args[0]=user_id, args[1-2]=deltax-y
            if (objects[object_id].isLockedBy == user_id)
            {
                objects[object_id].position.x = float.Parse(args[0], CultureInfo.InvariantCulture.NumberFormat);
                objects[object_id].position.y = float.Parse(args[1], CultureInfo.InvariantCulture.NumberFormat);
                objects[object_id].position.z = float.Parse(args[2], CultureInfo.InvariantCulture.NumberFormat);

                node_manager_function manager = new node_manager_function();
                std_msgs.msg.String msgOut = manager.GenerateMessage(object_id, user_id);
                chatterPub.Publish(msgOut);
            }
        }

        private void GrabObject(string object_id, string user_id, string[] _)
        {
            if (objects[object_id].isLockedBy == "")
            {
                objects[object_id].isLockedBy = user_id;
            }
        }

        private void ReleaseObject(string object_id, string user_id, string[] _)
        {
            if (objects[object_id].isLockedBy == user_id)
            {
                objects[object_id].isLockedBy = "";
            }
        }

        private receivedMessage decryptMessage(std_msgs.msg.String stringMsg)
        {
            receivedMessage msg = new receivedMessage();
            string[] str = stringMsg.Data.Split('!', StringSplitOptions.RemoveEmptyEntries);


            List<string> param = new List<string>();

            foreach (string sub in str[1].Split(';', StringSplitOptions.RemoveEmptyEntries))
            {
                param.Add(sub);
            }

            msg.object_id = param[0];
            msg.user_id = param[1];
            msg.function = str[0];
            msg.args = param.ToArray().Skip(2).ToArray();

            Console.WriteLine("Received Order");
            Console.WriteLine("Function: " + msg.function);
            Console.WriteLine("Object ID: " + msg.object_id);
            Console.WriteLine("User ID: " + msg.user_id + "\n");

            return msg;
        }
    }
}
