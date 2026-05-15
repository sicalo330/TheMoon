using UnityEngine;

public class Button : MonoBehaviour
{
    private void OnMouseDown(){
        CombatController.obj.backGroundAttack.SetActive(true);
        Player.obj.TryDoubleAttack();
        CombatController.obj.PlayerAttack();
        CombatController.obj.backGroundAttack.SetActive(false);
    }
}