using UnityEngine;

public class MacheteAttack : MonoBehaviour
{
    private void OnMouseDown(){
        Player.obj.machetazo = true;
        Player.obj.TryDoubleAttack();
        CombatController.obj.PlayerAttack();
    }
}