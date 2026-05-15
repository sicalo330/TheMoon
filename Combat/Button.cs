using UnityEngine;

public class Button : MonoBehaviour
{
    private void OnMouseDown(){
        Player.obj.TryDoubleAttack();
        CombatController.obj.PlayerAttack();
    }
}