using UnityEngine;

public class MacheteAttack : MonoBehaviour
{
    private void OnMouseDown(){
        Player.obj.machetazo = true;
        StartCoroutine(CameraShake.obj.Shake());
        Player.obj.TryDoubleAttack();
        CombatController.obj.PlayerAttack();
    }
}