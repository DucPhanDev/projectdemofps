using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using PathologicalGames;
public class EnemyZombie : MonoBehaviour
{
    public Collider zombieCollider;
    public NavMeshAgent navAgent;
    public Transform targetTrans;
    public Animator enemyAnimator;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    public AudioSource audioSource;
    public List<AudioClip> lstAudioDeadClips;

    public bool isAlive;
    public float rateOfAttack;
    public float timeSinceLastAttack;

    public int _CurrentHp 
    { 
        get
        {
            return currentHp;
        }
        set
        {
            currentHp = value;
            if (currentHp <= 0)
                OnDead();
        }
    }
    public int _MaxHp
    {
        get
        {
            return maxHp;
        }
        set
        {
            maxHp = value;
        }
    }

    private int currentHp;
    private int maxHp;
    private int shader_DissolveAmountId;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        navAgent.enabled = true;
        rateOfAttack = 3f;
        timeSinceLastAttack = 0;
        navAgent.isStopped = false;
        zombieCollider.enabled = true;
        isAlive = true;
        currentHp = maxHp = 200;
        targetTrans = PlayerController.Instance.transform;

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        shader_DissolveAmountId = Shader.PropertyToID("_Amount");

        skinnedMeshRenderer.material.SetFloat(shader_DissolveAmountId, 0);
    }

    private void Update()
    {
        
        if(isAlive)
        {
            timeSinceLastAttack += Time.deltaTime;
            if (Vector3.Distance(this.transform.position, targetTrans.position) > 4.5f)
            {
                navAgent.SetDestination(targetTrans.position);
                enemyAnimator.SetBool("Attack", false);
                navAgent.isStopped = false;
            }
            else
            {
                transform.LookAt(targetTrans);
                navAgent.isStopped = true;
                enemyAnimator.SetBool("Attack", true);
                if(timeSinceLastAttack > rateOfAttack)
                {
                    timeSinceLastAttack = 0;
                    PlayerUIController.Instance.OnUpdateScore(-10);
                }
            }
        }
    }

    private void OnDead()
    {
        if(isAlive)
        {
            PlayerUIController.Instance.OnUpdateScore(30);
            audioSource.PlayOneShot(lstAudioDeadClips[Random.Range(0, lstAudioDeadClips.Count)]);
            enemyAnimator.SetTrigger("Dead");
            isAlive = false;
            zombieCollider.enabled = false;
            navAgent.isStopped = true;
            navAgent.enabled = false;

            skinnedMeshRenderer.material.DOFloat(1, shader_DissolveAmountId, 4f).OnComplete(() =>
            {
                InGameManager.Instance.OnUpdateWaveCount();
               
            }).SetDelay(2f);

            PoolManager.Pools["Zombie_Pool"].Despawn(this.transform, 5f);
        }
    }

    public void OnGotHit(int damage)
    {
        _CurrentHp -= damage;
    }
}
