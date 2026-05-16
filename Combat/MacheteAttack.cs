using UnityEngine;

public class MacheteAttack : MonoBehaviour
{
    private void OnMouseDown(){
        Player.obj.TryDoubleAttack();
        CombatController.obj.PlayerAttack();
    }
}