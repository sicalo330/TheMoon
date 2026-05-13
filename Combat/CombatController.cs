using UnityEngine;

public class CombatController : MonoBehaviour
{
    public static CombatController obj;

    public Enemy selectedEnemy;

    void Awake(){
        obj = this;
    }

    public void SelectEnemy(Enemy enemy)
    {
        if(selectedEnemy != null){
            selectedEnemy.Select(false);
        }

        //Se selecciona al único enemigo al que se ha hecho un click
        selectedEnemy = enemy;
        selectedEnemy.Select(true);
    }

    public void PlayerAttack(){
        if(selectedEnemy != null){
            Player.obj.Attack();
        }
    }
}