Project Kurt's PhantomX Reactor Arm
 This code is setup to control a PhantomX Reactor Arm which is sold by Trossen
 Robotics: http:www.trossenrobotics.com/p/phantomx-ax-12-reactor-robot-arm.aspx
 This code uses a Arbotix Commander 2 to control it, also sold by Trossen 
 Robotics: http:www.trossenrobotics.com/p/arbotix-commander-gamepad-v2.aspx

  There are a couple of buttons that I am currently using for all modes, which include:
    Turning Commander on/off - When the Arm starts receiving valid packets, it assumes
    the commander is on and it turns on the servos moves the arm to the home position.  If
    the arm does not receive a valid packet for a time out period of time it assumes the 
    commander has been turned off and it moves the arm to a park position and then frees
    the servos.
  
    R1 - Cycles through the different Modes.
    R2 - Moves the arm to its home position.
    R3 - Toggles debug mode on or off (optional, turn off by un-defining DEBUG).  When on, it outputs
		debug information to the Serial port.  I use a simple VB forwarding app to forward messages from 
		commander to robot, and robot sent messages show up on terminal...

  The code currently has a few different modes of operation.
  Mode 1: 3d Cartesian IK code, which is based in part off of code by Michael E. Ferguson
    x is controlled by left joystick horizontal
    y by left joystick vertical
    z by right joystick vertical
    Grip angle by right joystick horizontal
    Gripper and Wrist rotate are controlled by holding L6 and right joystick.
  Mode 2: 3/2D cylindrical coordinate system.  More or less like 1: except
    Base Rotation control by left joystick horizontal
    Distance from base by left vertical
    ..
  Mode 3: Backhoe operation
    Left Horizontal - Rotate
    Left Vertical - Controls the dipper (elbow to wrist)
    Right Veritical - Controls Boom (Shoulder to Elbow)
    Right Horizontal - Bucket Curl...
    ...

  This code is a Work In Progress and is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
  FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License for more details.
  
  As part of this project I am also now including the sources for my VB Debug application.
  This application is not specific to the arm project, but can come in handy for any
  project that you are using the Arbotix controller board and the Robotix Commander and wish to
  have a way to display debug information on the computer screen.
  
  Kurt
