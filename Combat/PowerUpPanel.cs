using UnityEngine;

public class PowerUpPanel : MonoBehaviour
{
    public static PowerUpPanel obj;

    void Awake() => obj = this;

    public void Show(){
        gameObject.SetActive(true);
        CombatController.obj.buttonAtack.SetActive(false);
        Time.timeScale = 0f; // pausa el juego
    }

    public void Hide(){
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ChooseHp(){
        Player.obj.hp += 15;
        Player.obj.lifeText.text = Player.obj.hp.ToString();//Actualizar hp de una vez
        //PowerUpManager.ApplyHpBoost();
        Hide();
        CombatController.obj.StartNextWave();
    }

    public void ChooseGun(){
        //PowerUpManager.ApplyGunBoost();
        Player.obj.attackGun += 2;
        Hide();
        CombatController.obj.StartNextWave();
    }

    public void ChooseMachete(){
        Player.obj.attackMachete += 2;
        //PowerUpManager.ApplyMacheteBoost();
        Hide();
        CombatController.obj.StartNextWave();
    }
}