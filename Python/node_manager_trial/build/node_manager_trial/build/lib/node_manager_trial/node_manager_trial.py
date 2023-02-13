import json
import uuid

import rclpy
from rclpy.node import Node
from std_msgs.msg import String


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
Sphere.position = Position(-1.5, 0, 5)
sphereUID = "Sphere"

Square = Gameobject()
Square.position = Position(1.5, 0, 5)
squareUID = "Square"

objects = {sphereUID: Sphere, squareUID: Square}

class node_manager(Node):
    def __init__(self):
        super().__init__('node_manager')
        self.subscription_ = self.create_subscription(String, "UserReports" , self.applyFunctionality, 10)
        print("Subscriber created")
        self.publisher_ = self.create_publisher(String, 'ManagerNodeCommands', 10)
        print("Publisher created")

        self.commands = { 'ChangePosition': self.ChangePosition, 'GrabObject': self.GrabObject, 'ReleaseObject': self.ReleaseObject, 'CreateObject': self.CreateObject}

    def applyFunctionality(self, json_msg):
        print("Received Message!")
        msg = receivedMessage(**json.loads(json_msg.data))
        print(json_msg)

        func = self.commands[msg.Function]
        func(msg.Obj_id, msg.User_id, msg.Position)

    def ChangePosition(self, object_id, user_id, pose):
        if (objects[object_id].isLockedBy == user_id):
            objects[object_id].position.x = objects[object_id].position.x + pose[0]
            objects[object_id].position.y = objects[object_id].position.y + pose[1]
            msg = self.GenerateMessage(object_id)
            print("Position Changed")
            self.publisher_.publish(msg)

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

    def CreateObject(self, _, user_id, position):
        #add object to dictionary with position
        object_id = str(uuid.uuid4())
        tempObj = Gameobject()
        tempObj.position = Position(position[0], position[1], 5)
        objects[object_id] = tempObj
        objects[object_id].isLockedBy = user_id

        msg = self.GenerateMessage(object_id)
        print("Created object")
        self.publisher_.publish(msg)


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