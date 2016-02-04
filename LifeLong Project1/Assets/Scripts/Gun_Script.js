#pragma strict
var damage : int;
var CurrentItem : boolean;
var data : Data;
var ammo1 : int;
var platform : int;
var data1 : Platform;
function Start () {
data1 = gameObject.Find("Data").GetComponent(Platform);
platform = data1.Platform();
damage = 50;
data = gameObject.Find("Data").GetComponent(Data);
}

function Update () {
var hit : RaycastHit;
var ray : Ray = Camera.main.ScreenPointToRay(Vector3(Screen.width*0.5, Screen.height*0.5, 0));
if (platform == 3 || platform == 2){
if (CurrentItem == true){
ammo1 = data.Ammo();
if (ammo1 >= 1){
if (Input.GetMouseButtonDown){
ammo1 -= 1;
data.SetAmmo(ammo1);
//var hit : RaycastHit;
//var ray : Ray = Camera.main.ScreenPointToRay(Vector3(Screen.width*0.5, Screen.height*0.5, 0));
if(Physics.Raycast (ray, hit, 100)){
hit.transform.SendMessage("GunDamage", damage, SendMessageOptions.DontRequireReceiver);
}
}
}
}
}
if (platform == 1){
if (CurrentItem == true){
ammo1 = data.Ammo();
if (ammo1 >= 1){
if (UltimateButtonJAVA.GetButtonDown( "Shoot" )){
ammo1 -= 1;
data.SetAmmo(ammo1);
//var hit : RaycastHit;
//var ray : Ray = Camera.main.ScreenPointToRay(Vector3(Screen.width*0.5, Screen.height*0.5, 0));
if(Physics.Raycast (ray, hit, 100)){
hit.transform.SendMessage("GunDamage", damage, SendMessageOptions.DontRequireReceiver);
}
}
}
}








}




}



function SetItem (){
CurrentItem = true;
}

function PickUp1(object : float){
data.AddGun();
Destroy (gameObject);
}