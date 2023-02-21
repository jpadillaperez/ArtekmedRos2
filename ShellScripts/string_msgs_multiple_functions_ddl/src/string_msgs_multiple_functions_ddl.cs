using System;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Diagnostics;
using Newtonsoft.Json;

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

    public class GameObject
    {
        public Position position = new Position();
        public string isLockedBy = "";
    }

    public struct receivedMessage
    {
        //structure of the message received in node manager
        public string Obj_id;
        public string User_id;
        public string Function;
        public string[] Position;
    }

    public struct sentMessage
    {
        //structure of the message received in each user
        public string User_id;
        public string Obj_id;
        public float[] Position;
        //public bool? Active;
        public bool Active;
    }

    public class string_msgs_multiple_functions_ddl
    {
        public static IPublisher<std_msgs.msg.String> chatterPub;
        public static ISubscription<std_msgs.msg.String> chatterSub;

        public static IDictionary<string, GameObject> objects = new Dictionary<string, GameObject>();

        public static void Main(string[] args)
        {

            RCLdotnet.Init();

            string_msgs_multiple_functions_ddl manager = new string_msgs_multiple_functions_ddl();

            GameObject Sphere = new GameObject();
            Sphere.position = new Position();
            String sphereUID = "Sphere";

            GameObject Square = new GameObject();
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

            chatterPub = talkerNode.CreatePublisher<std_msgs.msg.String>("ManagerReplies");
            Console.WriteLine("Publisher created!");
            chatterSub = listenerNode.CreateSubscription<std_msgs.msg.String>("UserActions",
            msg =>
            {
                manager.applyFunctionality(JsonConvert.DeserializeObject<receivedMessage>(msg.Data));
                //Thread.Sleep(50);
            });
            Console.WriteLine("Subscriber created!");

            //Listening to changes from users
            RCLdotnet.Spin(listenerNode);
        }

        private void applyFunctionality(receivedMessage msg)
        {
            Console.WriteLine("Received Message!");

            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(msg.Function, BindingFlags.NonPublic | BindingFlags.Instance);
            theMethod.Invoke(this, new object[] { msg.Obj_id, msg.User_id, msg.Position });
        }

        private void ChangePosition(string object_id, string user_id, string[] args)
        {
            if (objects[object_id].isLockedBy == user_id)
            {
                objects[object_id].position.x = objects[object_id].position.x + float.Parse(args[0], CultureInfo.InvariantCulture.NumberFormat);
                objects[object_id].position.y = objects[object_id].position.y + float.Parse(args[1], CultureInfo.InvariantCulture.NumberFormat);

                string_msgs_multiple_functions_ddl manager = new string_msgs_multiple_functions_ddl();
                std_msgs.msg.String msg = manager.GenerateMessage(object_id);
                Console.WriteLine("Output message: ");
                Console.WriteLine(msg.Data);
                chatterPub.Publish(msg);
            }
        }

        private std_msgs.msg.String GenerateMessage(string obj_id)
        {
            sentMessage msg = new sentMessage();
            msg.Obj_id = obj_id;
            msg.Position = new float[3]{objects[obj_id].position.x, objects[obj_id].position.y, objects[obj_id].position.z};
            msg.Active = true;

            std_msgs.msg.String rosMsg = new std_msgs.msg.String();
            rosMsg.Data = JsonConvert.SerializeObject(msg);
            return rosMsg;
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

        private void CreateObject(string _, string user_id, string[] position){
            
            string object_id = Guid.NewGuid().ToString();

            GameObject tempObj = new GameObject();
            tempObj.position = new Position();
            tempObj.position.x = float.Parse(position[0], CultureInfo.InvariantCulture.NumberFormat);
            tempObj.position.y = float.Parse(position[1], CultureInfo.InvariantCulture.NumberFormat);
            tempObj.position.z = 5f;
            tempObj.isLockedBy = user_id;

            objects.Add(object_id, tempObj);

            string_msgs_multiple_functions_ddl manager = new string_msgs_multiple_functions_ddl();
            std_msgs.msg.String msg = manager.GenerateMessage(object_id);

            chatterPub.Publish(msg);
        }

        private void DeleteObject(string object_id, string user_id, string[] _)
        {
            //delete object from dictionary with position
            sentMessage msg = new sentMessage();
            msg.Obj_id = object_id;
            msg.Position = new float[3]{0,0,0};
            msg.Active = false;

            std_msgs.msg.String rosMsg = new std_msgs.msg.String();
            rosMsg.Data = JsonConvert.SerializeObject(msg);
            objects.Remove(object_id);

            Console.WriteLine(rosMsg.Data);
            chatterPub.Publish(rosMsg);

        }
        private void UserJoin(string object_id, string __, string[] _)
        {
            //Console.WriteLine("Adding User");
            string user_id = Guid.NewGuid().ToString();

            sentMessage msg = new sentMessage();
            msg.Obj_id = "";
            msg.User_id = user_id;

            std_msgs.msg.String rosMsg = new std_msgs.msg.String();
            rosMsg.Data = JsonConvert.SerializeObject(msg);
            
            Console.WriteLine(rosMsg.Data);
            chatterPub.Publish(rosMsg);
        }
    }
}
