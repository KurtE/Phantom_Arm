
//=============================================================================
//Project PhantomX Reactor Arm - Kurt's Arm acting like Backhoe
//=============================================================================

//=============================================================================
// Define Options
//=============================================================================
#define OPT_WRISTROT  
#define ARBOTIX_TO  1000      // if no message for a second probably turned off...
#define DEADZONE    3        // deadzone around center of joystick values

//=============================================================================
// Global Include files
//=============================================================================
#include <ax12.h>
#include <BioloidController.h>
#include <Commander.h>

//=============================================================================
//=============================================================================
/* Servo IDs */
enum {
  SID_BASE=1, SID_RSHOULDER, SID_LSHOULDER, SID_RELBOW, SID_LELBOW, SID_WRIST, SID_WRISTROT, SID_GRIP};
#if 0
static const byte pgm_axdIDs[] PROGMEM = {
  SID_BASE, 
  SID_RSHOULDER, SID_LSHOULDER, 
  SID_RELBOW, SID_LELBOW, 
  SID_WRIST, 
#ifdef OPT_WRISTROT
  SID_WRISTROT, 
#endif  
  SID_GRIP};
#endif

#define CNT_SERVOS  8 //(sizeof(pgm_axdIDs)/sizeof(pgm_axdIDs[0]))

// Define Ranges
#define BASE_MIN    0
#define BASE_MAX    1023

#define SHOULDER_MIN  205 
#define SHOULDER_MAX  810

#define ELBOW_MIN    210
#define ELBOW_MAX    900

#define WRIST_MIN    200
#define WRIST_MAX    830

#define WROT_MIN     0
#define WROT_MAX     1023

#define GRIP_MIN     0
#define GRIP_MAX     512
//=============================================================================
// Global Objects
//=============================================================================
Commander command;
BioloidController bioloid = BioloidController(1000000);

//=============================================================================
// Global Variables...
//=============================================================================
boolean         g_fArmActive = false;   // Is the arm logically on?
boolean         g_fBackHoe = true;      // Are we in Back hoe mode?  or in IK mode...

// Values for current servo values for the different joints
int             g_sBase;                // Current Base servo value
int             g_sShoulder;            // Current shoulder target 
int             g_sElbow;               // Current
int             g_sWrist;               // Current Wrist value
int             g_sWristRot;            // Current Wrist rotation
int             g_sGrip;                // Current Grip position

// Message informatino
unsigned long   ulLastMsgTime;          // Keep track of when the last message arrived to see if controller off
byte            buttonsPrev;            // will use when we wish to only process a button press once

//===================================================================================================
// Setup 
//====================================================================================================
void setup() {
  // Lets initialize the Commander
  uint8_t i;
  command.begin(38400);

  // Next initialize the Bioloid
  bioloid.poseSize = CNT_SERVOS;

  // Set the id for each of the servos
#if 0  // bugbug:: when I do this code only the base and shoulder servos work???
  for (i=0; i < CNT_SERVOS; i++) {
    bioloid.setId(i, pgm_axdIDs[i]);
  }
#endif
  // Read in the current positions...
  bioloid.readPose();
  // Start off to put arm to sleep...
  PutArmToSleep();
}
//===================================================================================================
//===================================================================================================
void loop() {
  int sBase, sShoulder, sElbow, sWrist, sWristRot, sGrip;
  boolean fChanged = false;
  if (command.ReadMsgs()) {
    // See if the Arm is active yet...
    if (g_fArmActive) {
      sBase = g_sBase;
      sShoulder = g_sShoulder;
      sElbow = g_sElbow; 
      sWrist = g_sWrist;
      sGrip = g_sGrip;
      sWristRot = g_sWristRot;

      // Going to use L6 in combination with the right joystick to control both the gripper and the 
      // wrist rotate...
      if (command.buttons & BUT_L6) {
        sGrip = min(max(sGrip + command.lookV/2, GRIP_MIN), GRIP_MAX);
        sWristRot = min(max(g_sWristRot + command.lookH/6, WROT_MIN), WROT_MAX);
        fChanged = (sGrip != g_sGrip) || (sWristRot != g_sWristRot);
      }
      else {
        // lets update positions with the 4 joystick values
        // First the base
        sBase = min(max(g_sBase + command.walkH/6, BASE_MIN), BASE_MAX);
        if (sBase != g_sBase)
          fChanged = true;

        // Now the Boom
        sShoulder = min(max(g_sShoulder + command.lookV/6, SHOULDER_MIN), SHOULDER_MAX);
        if (sShoulder != g_sShoulder)
          fChanged = true;

        // Now the Dipper 
        sElbow = min(max(g_sElbow + command.walkV/6, ELBOW_MIN), ELBOW_MAX);
        if (sElbow != g_sElbow)
          fChanged = true;

        // Bucket Curl
        sWrist = min(max(g_sWrist + command.lookH/6, WRIST_MIN), WRIST_MAX);
        if (sWrist != g_sWrist)
          fChanged = true;
      }

      if (fChanged) {
        MoveArmTo(sBase, sShoulder, sElbow, sWrist, 512, sGrip, 100, true);
      } 
      else if (bioloid.interpolating > 0) {
        bioloid.interpolateStep();
      }
    }
    else {
      g_fArmActive = true;
      MoveArmToHome();      
    }

    buttonsPrev = command.buttons;
    ulLastMsgTime = millis();    // remember when we last got a message...
  }
  else {
    if (bioloid.interpolating > 0) {
      bioloid.interpolateStep();
    }
    // error see if we exceeded a timeout
    if (g_fArmActive && ((millis() - ulLastMsgTime) > ARBOTIX_TO)) {
      PutArmToSleep();
    }
  }
} 

//===================================================================================================
// MoveArmToHome
//===================================================================================================
void MoveArmToHome(void) {
  MoveArmTo(512, 512, 330, 690, 512, 512, 500, true);
}

//===================================================================================================
// PutArmToSleep
//===================================================================================================
void PutArmToSleep(void) {
  g_fArmActive = false;
  MoveArmTo(512, 212, 212, 512, 512, 512, 1000, true);
}

//===================================================================================================
// MoveArmTo
//===================================================================================================
void MoveArmTo(int sBase, int sShoulder, int sElbow, int sWrist, int sWristRot, int sGrip, int wTime, boolean fWait) {

  // where to wait for last move to complete?
  bioloid.setNextPose(SID_BASE, sBase);

  bioloid.setNextPose(SID_RSHOULDER, sShoulder);
  bioloid.setNextPose(SID_LSHOULDER, 1024-sShoulder);

  bioloid.setNextPose(SID_RELBOW, sElbow);
  bioloid.setNextPose(SID_LELBOW, 1024-sElbow);

  bioloid.setNextPose(SID_WRIST, sWrist);

#ifdef OPT_WRISTROT
  bioloid.setNextPose(SID_WRISTROT, sWristRot); 
#endif  

  bioloid.setNextPose(SID_GRIP, sGrip);

  // Do at least the first movement
  bioloid.interpolateStep();

  // Save away the current positions...
  g_sBase = sBase;
  g_sShoulder = sShoulder;
  g_sElbow = sElbow;
  g_sWrist = sWrist;
  g_sWristRot = sWristRot;
  g_sGrip = sGrip;

  // Make sure the previous movement completed.
  while (bioloid.interpolating > 0) {
    bioloid.interpolateStep();
    delay(3);
  }

  // Now start the move
  bioloid.interpolateSetup(wTime);

  // And if asked to, wait for the previous move to complete...
  if (fWait) {
    while (bioloid.interpolating > 0) {
      bioloid.interpolateStep();
      delay(3);
    }
  }
}

//===================================================================================================
// ArmIK: compute the desired angles for each of the servos given a desired coordinate.  We will start
//   off doing floating point math as it is easier... Also we may go in with given servo angles
//   for the Wrist...
//===================================================================================================
#if 1
// Define some lengths and offsets used by the arm
#define BaseHeight          120    // about 120mm 
#define ShoulderLength      155    // Not sure yet what to do as the servo is not directly in line,  Probably best to offset the angle?
//                                 // X is about 140, y is about 40 so sqrt is Hyp is about 155, so maybe about 21 degrees offset
#define ShoulderServoOffset  72    // should offset us some...
#define ElbowLength          147   //Length of the Arm from Elbow Joint to Wrist Joint
#define WristLength         137    // Wrist length including Wrist rotate
#define RToD                57.295779  

boolean ArmIk(float WristX, float WristY, float z, int g, float wa, int wr) //Here's all the Inverse Kinematics to control the arm
{
  // First calculate the length from the shoulder to the wrist
  float IKSW = sqrt(((WristY-BaseHeight)*(WristY-BaseHeight))+(WristX*WristX));

  // Compute angle between SW lien and ground in radians.
  float A1 = atan((WristY-BaseHeight)/WristX);

  float A2 = acos((ShoulderLength*ShoulderLength-ElbowLength*ElbowLength+IKSW*IKSW)/((ShoulderLength*2)*IKSW));
  float Elbow = acos((ShoulderLength*ShoulderLength+ElbowLength*ElbowLength-IKSW*IKSW)/((ShoulderLength*2)*ElbowLength));
  float Shoulder = A1 + A2;
  Elbow = Elbow * RToD;
  Shoulder = Shoulder * RToD;
  if((int)Elbow <= 0 || (int)Shoulder <= 0)
    return false;

  float Wris = abs(wa - Elbow - Shoulder) - 90;
#if 0  
  Elb.write(180 - Elbow);
  Shldr.write(Shoulder);
  Wrist.write(180 - Wris);
  Base.write(z);
  WristR.write(wr);
  Gripper.write(g);
  WristY = tmpy;
  WristX = tmpx;
  Z = tmpz;
  WA = tmpwa;
  G = tmpg;
  WR = tmpwr;
#endif  
  return true;

}
#endif







