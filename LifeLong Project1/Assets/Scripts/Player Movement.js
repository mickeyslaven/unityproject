#pragma strict








//Public Variables

//Spawn Point
var Spawn : GameObject;
//Moving Speed
var movementSpeed : float = 1.0;
//Turning Speed
var mouseSensitivity : float = 1;
//How Far Can The Player Look Down?
var RotationUpDownrange : float = 45;
var platform : int;










//Private Variables

//Calls The CharacterController Which Controlles Char Movement
private var cc : CharacterController;
//Forward Moving Speed
private var forwardSpeed : float;
//Variable that Tells the Character Controller Where to Move
private var move : Vector3;
//Speed of moving left or right (Not Turning Left or Right MOVING)
private var sideSpeed : float;
//Left/Right Rotation Sensistivity
private var RotationLeftRight : float;
//Up/Down Rotation Sensistivity
private var RotationUpDown : float;










function Start () {
  #if UNITY_EDITOR
    Debug.Log("Unity Editor");
    platform = 0;
  #endif
    
  #if UNITY_IOS
    Debug.Log("Iphone");
    platform = 1;
  #endif

  #if UNITY_STANDALONE_OSX
    Debug.Log("Stand Alone OSX");
    platform = 2;
  #endif

  #if UNITY_STANDALONE_WIN
    Debug.Log("Stand Alone Windows");
    platform = 3;
  #endif    
transform.position = Spawn.transform.position;
cc = GetComponent("CharacterController");
RotationUpDown = 0;
}






function Update () {
if (platform == 0 || platform == 3){
RotationLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
transform.Rotate(0, RotationLeftRight, 0);
RotationUpDown -= Input.GetAxis("Mouse Y") * mouseSensitivity;
RotationUpDown = Mathf.Clamp(RotationUpDown, -RotationUpDownrange, RotationUpDownrange);
Camera.main.transform.localRotation = Quaternion.Euler ( RotationUpDown, 0, 0);
forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;

move = new Vector3( sideSpeed, 0, forwardSpeed);
move = transform.rotation * move;
cc.SimpleMove(move);
}
if (platform == 1) {
var joystickPosition : Vector2 = UltimateJoystickJAVA.GetPosition( "Movement" ) * movementSpeed;
var movementDirection : Vector3 = new Vector3( joystickPosition.x, 0, joystickPosition.y );
var rotationplayer : Vector2 = UltimateJoystickJAVA.GetPosition( "Rotation" ) * mouseSensitivity;
transform.Rotate(0, rotationplayer.x, 0);
RotationUpDown -= rotationplayer.y;

RotationUpDown = Mathf.Clamp(RotationUpDown, -RotationUpDownrange, RotationUpDownrange);
Camera.main.transform.localRotation = Quaternion.Euler ( RotationUpDown, 0, 0);
movementDirection = transform.rotation * movementDirection;
cc.SimpleMove(movementDirection);
}
}