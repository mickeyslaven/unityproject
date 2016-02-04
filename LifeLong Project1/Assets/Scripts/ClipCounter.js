#pragma strict
private var data : Data;
function Start () {

data = GameObject.Find("Data").GetComponent(Data);

}

function Update () {
gameObject.GetComponent(GUIText).text = "Clips: " + data.Clips().ToString();
}