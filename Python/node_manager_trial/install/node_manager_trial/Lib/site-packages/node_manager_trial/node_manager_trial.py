import rclpy
from rclpy.node import Node

from std_msgs.msg import String

import json


class Position:
    def __init__(self, x=0, y=0, z=0):
        self.x = x
        self.y = y
        self.z = z

class Gameobject:
    def __init__(self):
        self.position = Position()
        self.isLockedBy = ""

    def setPosition(self, x, y, z):
        self.position = Position(x,y,z)


class receivedMessage:
#structure of the message sent to node manager
    def __init__(self, Obj_id, User_id, Function, Position):
        self.Obj_id = Obj_id
        self.User_id = User_id
        self.Function = Function
        self.Position = Position

class sentMessage:
    #structure of the message received by each user
    def __init__(self, Obj_id, Position):
        self.Obj_id = Obj_id
        self.Position = Position


Sphere = Gameobject()
Sphere.position = Position()
sphereUID = "Sphere"

Square = Gameobject()
Square.position = Position()
squareUID = "Square"

Sphere.position.x = -1.5
Sphere.position.y = 0
Sphere.position.z = 5

Square.position.x = 1.5
Square.position.y = 0
Square.position.z = 5

objects = {sphereUID: Sphere, squareUID: Square}

class node_manager(Node):
    def __init__(self):
        super().__init__('node_manager')
        self.subscription_ = self.create_subscription(String, "UserReports" , self.applyFunctionality, 10)
        print("Subscriber created")
        self.publisher_ = self.create_publisher(String, 'ManagerNodeCommands', 10)
        print("Publisher created")

        self.commands = { 'ChangePosition': self.ChangePosition, 'GrabObject': self.GrabObject, 'ReleaseObject': self.ReleaseObject}

    def applyFunctionality(self, json_msg):
        print("Received Message!")
        msg = receivedMessage(**json.loads(json_msg.data))
        print(json_msg)

        if msg.Obj_id in objects:
            func = self.commands[msg.Function]
            func(msg.Obj_id, msg.User_id, msg.Position)

    def ChangePosition(self, object_id, user_id, pose):
        if (objects[object_id].isLockedBy == user_id):
            objects[object_id].position.x = objects[object_id].position.x + pose[0]
            objects[object_id].position.y = objects[object_id].position.y + pose[1]
            msg = self.GenerateMessage(object_id)
            print("Position Changed")
            self.publisher_.publish(msg)
            print("Sent Message!")

    def GenerateMessage(self, Obj_id):
        msg = sentMessage(Obj_id, [objects[Obj_id].position.x, objects[Obj_id].position.y, objects[Obj_id].position.z] )
        rosMsg = String()
        rosMsg.data = json.dumps(msg, default=lambda o: o.__dict__, sort_keys=True, indent=4)
        return rosMsg

    def GrabObject(self, object_id, user_id, _):
        if (objects[object_id].isLockedBy == ""):
            objects[object_id].isLockedBy = user_id

    def ReleaseObject(self, object_id, user_id, _):
        if (objects[object_id].isLockedBy == user_id):
            objects[object_id].isLockedBy = ""



def main(args=None):

    rclpy.init(args=args)
    manager = node_manager()
    rclpy.spin(manager)

    # Destroy the node explicitly
    # (optional - otherwise it will be done automatically
    # when the garbage collector destroys the node object)
    #pub.destroy_node()
    #sub.destroy_node()
    #rclpy.shutdown()


if __name__ == '__main__':
    main()