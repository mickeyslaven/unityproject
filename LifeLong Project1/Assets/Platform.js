#pragma strict
var platform : int;
var joystickl : GameObject;
var joystickr : GameObject;
var button1 : GameObject;
var button2 : GameObject;
var button3 : GameObject;
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
  
}

function Update () {
if (platform == 1){
joystickl.SetActive(true);
joystickr.SetActive(true);
button1.SetActive(true);
button2.SetActive(true);
button3.SetActive(true);

}

}

function Platform(){
return platform;
}