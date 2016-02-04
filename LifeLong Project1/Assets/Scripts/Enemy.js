#pragma strict
var health : int;
function Start () {
health =100;
}



function Update () {
if (health <= 0){
Destroy (gameObject);
}
}




function GunDamage(damage : int){
health -= damage;


}