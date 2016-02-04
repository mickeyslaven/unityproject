#pragma strict
var ammo : int;
var clips = 0;
var gun : GameObject;
var totalammo : int;
var curclip : int;
var clip1 : int;
var clip2 : int;
var clip3 : int;
var clip4 : int;
var clip5 : int;
var clip6 : int;
var data : Messages;
function Start () {
DontDestroyOnLoad(transform.gameObject);
ammo = 0;
curclip = 1;
data = GameObject.FindWithTag("Overlay").GetComponent(Messages);
}
function TotalAmmo() {
return (totalammo);
}

function Ammo() {
return (ammo);
}

function Clips() {
return (clips);
}

function AddClips(amount : int) {
clips += amount;
Debug.Log("Clip Added");
}


function AddGun() 
{

gun.SetActive(true);
gun.GetComponent(Gun_Script).SetItem();

}
function AddAmmo(amount : int){
totalammo += amount;
Debug.Log("Ammo Added");
}

function SetAmmo(amount : int) {
if (curclip == 1){
clip1 = amount;
ammo = clip1;

}
if (curclip == 2){
clip2 = amount;
ammo = clip2;

}
if (curclip == 3){
clip3 = amount;
ammo = clip3;

}
if (curclip == 4){
clip4 = amount;
ammo = clip4;

}
if (curclip == 5){
clip5 = amount;
ammo = clip5;

}
if (curclip == 6){
clip6 = amount;
ammo = clip6;

}
}

function CurClip(amount : int){
if (amount == 1 && clips > 0){
curclip = 1;
ammo = clip1;
data.Set1(30);
}
if (amount == 2 && clips > 1){
curclip = 2;
ammo = clip2;
data.Set2(30);
}
if (amount == 3 && clips > 2){
curclip = 3;
ammo = clip3;
data.Set3(30);
}
if (amount == 4 && clips > 3){
curclip = 4;
ammo = clip4;
data.Set4(30);
}
if (amount == 5 && clips > 4){
curclip = 5;
ammo = clip5;
data.Set5(30);
}
if (amount == 6 && clips > 5){
curclip = 6;
ammo = clip6;
data.Set6(30);
}
}


function Load (){
if (ammo <= 11 && totalammo >= 1 && clips >= 1 && curclip == 1){
totalammo -= 1;
clip1 += 1;
ammo = clip1;
}
if (ammo <= 11 && totalammo >= 1 && clips >= 1 && curclip == 2){
totalammo -= 1;
clip2 += 1;
ammo = clip2;
}
if (ammo <= 11 && totalammo >= 1 && clips >= 1 && curclip == 4){
totalammo -= 1;
clip4 += 1;
ammo = clip4;
}
if (ammo <= 11 && totalammo >= 1 && clips >= 1 && curclip == 3){
totalammo -= 1;
clip3 += 1;
ammo = clip3;
}
if (ammo <= 11 && totalammo >= 1 && clips >= 1 && curclip == 5){
totalammo -= 1;
clip5 += 1;
ammo = clip5;
}
if (ammo <= 11 && totalammo >= 1 && clips >= 1 && curclip == 6){
totalammo -= 1;
clip6 += 1;
ammo = clip6;
}


}