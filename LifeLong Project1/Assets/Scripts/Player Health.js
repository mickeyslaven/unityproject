#pragma strict
var health : int;
function Start () {

}

function Update () {

}
function Dead (damage : int){
Debug.Log("Dead");
Application.LoadLevel(damage);

}