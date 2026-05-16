using UnityEngine;
public class GunAttack : MonoBehaviour
{
    [SerializeField] private AudioClip audioGun;
    private void OnMouseDown(){
        //Debug.Log("Click");
        Player.obj.gun = true;
        SoundManager.Instance.ExecuteSound(audioGun);
        CombatController.obj.PlayerGunAttack();
    }
}