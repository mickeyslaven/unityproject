
var jump : int;
 var jumping : int;
 var myAnimator : Animator;
var me : Transform;
var player : Transform;
 
 function Start()
 {
 myAnimator = GetComponent(Animator);
 //Means its a zombie
 
  
 }
  function Jump(){
  
 if (jump == 1){
jumping = 1;
myAnimator.SetFloat("jumping", jumping);
}
else {
jumping = 0;
myAnimator.SetFloat("jumping", jumping);
}
  }
 function Update () {
 me.rotation = Quaternion.Slerp(me.rotation,
    Quaternion.LookRotation(player.position - me.position), 3*Time.deltaTime);
   
    var dist = Vector3.Distance(player.position, transform.position);
    if (dist <= 10){
    me.position += me.forward * 3 * Time.deltaTime; 
    }
  
 }