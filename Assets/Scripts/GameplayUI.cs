using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    int txtTotalCrocos;
    public int txtCrocosKilled = 0;
    [SerializeField] Text crocosCount;
    string txtPiranhasLeft;

    int txtTotalTargets;
    public int txtTargetsDestroyed = 0;
    [SerializeField] Text targetsCount;
    [SerializeField] EnemyGenerator enemyGenerator;
    [SerializeField] TargetGenerator targetGenerator;
    [SerializeField] Animator doorAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        txtTotalTargets = targetGenerator.TotalTargets;
        targetsCount.text = (txtTargetsDestroyed + " / " + txtTotalTargets);

        txtTotalCrocos = enemyGenerator.TotalEnemies;
        crocosCount.text = (txtCrocosKilled + " / " + txtTotalCrocos);

        if(txtTargetsDestroyed >= txtTotalTargets)
        {
            doorAnim.SetTrigger("isOpen?");
        }
    }
}
