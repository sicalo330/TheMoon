using UnityEngine;
public class GunAttack : MonoBehaviour
{
    private void OnMouseDown(){
        //Debug.Log("Click");
        Player.obj.gun = true;
        CombatController.obj.PlayerGunAttack();
    }
}