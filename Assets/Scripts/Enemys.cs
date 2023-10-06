using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemys : MonoBehaviour
{
    public Enemy[] prefabs;

    public int baris = 4;
    public int kolom = 13;
    public AnimationCurve speed;

    public Bullet missilePrefab;

    public float missileAttackRate = 1.0f;

    public int amountKilled {  get; private set; }
    public int amountAlive => this.totalEnemy - this.amountKilled;

    public int totalEnemy => this.baris * this.kolom;
    public float percentKilled => (float)this.amountKilled / (float)this.totalEnemy;

    private Vector3 arah = Vector2.right;

    private void Awake()
    {
        for (int bar = 0; bar < this.baris; bar++)
        {
            float lebar = 2.0f * (this.kolom - 1);
            float tinggi = 2.0f * (this.baris - 1);
            Vector3 centering = new Vector2(-lebar / 2, -tinggi / 2);
            Vector3 posisiBaris = new Vector3(centering.x, centering.y + (bar * 2.0f), 0.0f);
            for(int kol = 0; kol < this.kolom; kol++)
            {
                Enemy enemy = Instantiate(this.prefabs[bar], this.transform);
                enemy.killed += EnemyKilled;
                Vector3 posisi = posisiBaris;
                posisi.x += kol * 2.0f;
                enemy.transform.localPosition = posisi;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), this.missileAttackRate, this.missileAttackRate);
    }

    private void Update()
    {
        this.transform.position += arah * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform enemy in this.transform)
        {
            if (!enemy.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (arah == Vector3.right && enemy.position.x >= (rightEdge.x - 1.0f))
            {
                AdvanceRow();
            }
            else if (arah == Vector3.left && enemy.position.x <= (leftEdge.x + 1.0f))
            {
                AdvanceRow();
            }
        }
    }

    private void AdvanceRow()
    {
        arah.x *= -1.0f;

        Vector3 posisi = this.transform.position;
        posisi.y -= 0.5f;
        this.transform.position = posisi;
    }

    private void MissileAttack()
    {
        foreach (Transform enemy in this.transform)
        {
            if (!enemy.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (Random.value < (1.0f / (float)this.amountAlive))
            {
                Instantiate(this.missilePrefab, enemy.position, Quaternion.identity);
                break;
            }
        }
    }

    private void EnemyKilled()
    {
        this.amountKilled++;

        if (this.amountKilled >= this.totalEnemy)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
