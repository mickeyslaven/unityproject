#pragma strict
//Calls The Object ID For The Clip Object The ID Is 2
var objectid = 2;
//Call the data var. We will use this to get the script data from the data object
var data : Data;





function Start () {
//Set data = to the Data script
data = gameObject.Find("Data").GetComponent(Data);
}





function PickUp1(object : float){
data.AddAmmo(12);
Destroy (gameObject);
}