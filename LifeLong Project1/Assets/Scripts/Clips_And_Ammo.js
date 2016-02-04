#pragma strict
var clip1 : int;
var clip2 : int;
var clip3 : int;
var clip4 : int;
var clip5 : int;
var clip6 : int;
var curclip : int;
var data : Data;
var data1 : Platform;
var platform : int;
var totalammo : int;
function Start () {
data1 = gameObject.Find("Data").GetComponent(Platform);
platform = data1.Platform();
data = gameObject.Find("Data").GetComponent(Data);
}

function Update () {
if (Input.GetButtonDown("1")){
curclip = 1;
data.CurClip(1);

}
if (Input.GetButtonDown("2")){
curclip = 2;
data.CurClip(2);
}
if (Input.GetButtonDown("3")){
curclip = 3;
data.CurClip(3);
}
if (Input.GetButtonDown("4")){
curclip = 4;
data.CurClip(4);
}
if (Input.GetButtonDown("5")){
curclip = 5;
data.CurClip(5);
}
if (Input.GetButtonDown("6")){
curclip = 6;
data.CurClip(6);
}
if (platform == 3){
if (Input.GetButtonDown("L")){
data.Load();
}
}
if (platform == 1){
if (UltimateButtonJAVA.GetButtonDown( "Load" )){
data.Load();
}
}
}
