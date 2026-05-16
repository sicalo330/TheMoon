using UnityEngine;
public class GunAttack : MonoBehaviour
{
    private void OnMouseDown(){
        //Debug.Log("Click");
        CombatController.obj.PlayerGunAttack();
    }
}