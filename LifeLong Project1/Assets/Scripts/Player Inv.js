#pragma strict
var object = 1;
var platform : int;
var data1 : Platform;
function Start(){
data1 = gameObject.Find("Data").GetComponent(Platform);
platform = data1.Platform();
}
function Update () {
if(Input.GetButtonDown("E")){
PickUp();
}
if (platform == 1){
if (UltimateButtonJAVA.GetButtonDown( "PickUp" )){
PickUp();
}
}
}


function PickUp(){
var hit : RaycastHit;
var ray : Ray = Camera.main.ScreenPointToRay(Vector3(Screen.width*0.5, Screen.height*0.5, 0));
if(Physics.Raycast (ray, hit, 100)){
hit.transform.SendMessage("PickUp1", object, SendMessageOptions.DontRequireReceiver);
}
}



