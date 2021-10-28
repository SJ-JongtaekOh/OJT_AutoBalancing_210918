using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BulletScript : MonoBehaviour
{
    public MonsterPropertieScript myTargetScript;//내 타겟 스크립트
    public Vector3 targetPos;//목표가 죽었을 시 목표 위치 저장하는 변수

    public GameObject objMyParent;//부모의 오브젝트
    public MonsterPropertieScript myParentScript;//불러온 부모의 스크립트

    public float myDamage;//나의 대미지
    public float moveSpeed;//총알의 속도
    public bool isFinishedAttackMotion; // 부모가 죽더라도 부모가 공격모션을 끝내서 True가 되면 일단 총알은 쭉 발사하게 만들기 위한 Bool값
    public string myName;//부모의 속성
    public float criticalPercent;//크리티컬 확률
    public float criticalPower;//크리티컬 파워

    //public object Targets { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
     
        if (objMyParent != null)
        {
            myParentScript = objMyParent.GetComponent<MonsterPropertieScript>();//부모로부터 정보 받아오기 위해 캐싱
        }

       //if (MyParent.CompareTag ("EnemyTeam"))
       //{
       //     this.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
       //}

        myName = myParentScript.myMonsterName;

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isFinishedAttackMotion == true)//부모가 총을 쏘는 모션까지 했다면
        {

            if (myTargetScript != null) // 가는도중 타겟이 죽지 않았다면
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, myTargetScript.transform.position, moveSpeed * Time.deltaTime);
            }

            else if (myTargetScript == null) // 가는도중 타겟이 죽었다면 죽었던 위치까진 멈추지말고 도달하세요
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, targetPos, moveSpeed * Time.deltaTime);
            }

        }

        else if (isFinishedAttackMotion == false) //아직 출발을안하였고
        {
            if (objMyParent != null)//부모가 살아있으면
            {
                this.transform.position = objMyParent.transform.position; //출발도 안했고 부모도 살아있으면 부모를 계속 따라다니세요
            }

            else//부모가 죽었으면 없어지세요
            {
                Destroy(this.gameObject);
            }

        }
        //else // 부모도 죽고 부모의 죽은 위치에 있다면
        //{
        //    if (MyParent == null)
        //    {
        //        Destroy(this.gameObject);
        //    }
        //}

        if (objMyParent == null && myTargetScript != null) //부모가죽고 내 목표물이 죽지 않았을때
        {
            if (isFinishedAttackMotion == true && this.transform.position == myTargetScript.transform.position) // 때렸다면
            {
                if (this.myName != "Mato" && this.myName !="Crape")//마토나 크레이프가 아니라면
                {
                    MonsterPropertieScript cacheTarget;//공격할 타겟의 스크립트
                    cacheTarget = myTargetScript.GetComponent<MonsterPropertieScript>();

                    GameObject objDamage;//데미지 텍스트의 Canvas 찾기
                    objDamage = Instantiate(myTargetScript.objMyDamagecanv, cacheTarget.objMyDamagecanv.transform.position, Quaternion.identity,
                    myTargetScript.objMyDamagecanv.transform);

                    TextMeshProUGUI tmpTarget;//데미지 텍스트 찾기
                    tmpTarget = objDamage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();//.text = "" + (pasteHp - Hp);

                    float atkRandomCritical;//크리티컬 확률 계산을 위한 변수
                    atkRandomCritical = Random.Range(0, 100);

                    int finalDamage;//공격 식을 다 계산한 변수 
                    float evasionRandom = Random.Range(1, 100);//회피했는지 계산을 위한 변수

                    if (cacheTarget.evasion > evasionRandom)//회피했다면
                    {
                        if (tmpTarget != null)
                        {
                           tmpTarget.text = "evasion";
                            
                        }
                    }

                    else//회피하지 못했다면
                    {               

                        if (atkRandomCritical > criticalPercent)//크리티컬이 터지지 않았다면
                        {
                            finalDamage = (int)((myDamage * 100 / (100 + cacheTarget.defense)) + ((myDamage * 100 / (100 + cacheTarget.defense)) * Random.Range(-0.05f, 0.06f)));
                        }

                        else//크리티컬이 터졌다면
                        {
                            finalDamage = (int)((myDamage * 100 / (100 + cacheTarget.defense)) + ((myDamage * 100 / (100 + cacheTarget.defense))
                           * Random.Range(-0.05f, 0.06f) + myDamage * criticalPower));

                            if (tmpTarget != null)
                            {
                                tmpTarget.color = new Color(1, 1, 0);

                            }
                        }


                        if (cacheTarget.shieldDefense <= 0)//쉴드가 없다면 
                        {
                            cacheTarget.healthPoint -= finalDamage;
                        }

                        if (cacheTarget.shieldDefense > 0)//쉴드가 있다면
                        {
                            cacheTarget.shieldDefense -= finalDamage;

                            if (cacheTarget.shieldDefense < 0)
                            {
                                cacheTarget.healthPoint += cacheTarget.shieldDefense;
                                cacheTarget.shieldDefense = 0;

                            }

                            //CashingMyTarget.PasteshieldDef = CashingMyTarget.shieldDef;
                        }

                        if (cacheTarget.myMonsterName == "Watteron")//워터론을 때렸다면
                        {
                            if (myParentScript.shieldDefense <= 0)
                            {
                                myParentScript.healthPoint -= cacheTarget.attackPower * cacheTarget.skillPower1;

                            }

                            if (myParentScript.shieldDefense > 0)
                            {
                                myParentScript.shieldDefense -= cacheTarget.attackPower * cacheTarget.skillPower1;

                                if (myParentScript.shieldDefense < 0)
                                {
                                    myParentScript.healthPoint += myParentScript.shieldDefense;
                                    myParentScript.shieldDefense = 0;

                                }

                                myParentScript.pasteHealthPoint = myParentScript.healthPoint;
                                //CashingMyTarget.PasteshieldDef = CashingMyTarget.shieldDef;
                            }

                        }

                        if (cacheTarget.myMonsterName == "PineArs")//파인아스라면
                        {
                            if (cacheTarget.isPinearsAttackedFirst == false)
                            {
                                cacheTarget.objMyTaget = this.objMyParent;
                                cacheTarget.isPinearsAttackedFirst = true;
                            }

                        }
                        // TargetText.text = "" + finalDamage;
                        cacheTarget.pasteHealthPoint = cacheTarget.healthPoint;

                        if (tmpTarget != null)
                        {
                            tmpTarget.text = "" + finalDamage;
                        }
                    }
                        Destroy(objDamage, 2);

                }
                else if (this.myName == "Mato")//마토라면 공격
                {
                    AtkMatoBulletByMonster();
                }

                else if(this.myName == "Crape")//크레이프라면 크레이프 공격
                {
                    AtkCrapeBulletByMonster();
                }
                Destroy(this.gameObject);// 이 총알은 없어집니다.

            }

        }

        else if (myTargetScript != null && this.transform.position == myTargetScript.transform.position )//부모가 죽지않고 목표물이 살아있을 때
        {

            if (this.myName != "Mato" && this.myName !=  "Crape")
            {
                MonsterPropertieScript cacheTarget;//공격 목표 스크립트
                cacheTarget = myTargetScript.GetComponent<MonsterPropertieScript>();
                
                GameObject objDamage;//텍스트가 나올 Canvas 잡아주기
                objDamage = Instantiate(myParentScript.objDamageTmpcanv, cacheTarget.objMyDamagecanv.transform.position, Quaternion.identity, myTargetScript.objMyDamagecanv.transform);

                TextMeshProUGUI tmpTarget;//데미지 텍스트 생성하기 위한 변수
                tmpTarget = objDamage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();//.text = "" + (pasteHp - Hp);

                Destroy(tmpTarget, 2);

                float atkRandomCritical;
                atkRandomCritical = Random.Range(0, 100);//크리티컬이 터졌는지 검사하기 위한 변수
                int finalDamage;//공격식을 계산한 최종 대미지 값

                float evasionRandom = Random.Range(1, 100);//회피가 되었는지 검사하기 위한변수

                if (cacheTarget.evasion >= evasionRandom)//회피했다면
                {
                    tmpTarget.text = "evasion";
                }

                else//회피하지 못했다면
                {

                    if (atkRandomCritical > criticalPercent)//크리티컬이 터지지 않았다면
                    {
                        finalDamage = (int)((myDamage * 100 / (100 + cacheTarget.defense)) + ((myDamage * 100 / (100 + cacheTarget.defense)) * Random.Range(-0.05f, 0.06f)));
                    }

                    else//크리티컬이 터졌다면
                    {
                        finalDamage = (int)((myDamage * 100 / (100 + cacheTarget.defense)) + ((myDamage * 100 / (100 + cacheTarget.defense))
                        * Random.Range(-0.05f, 0.06f) + myDamage * criticalPower));
                        tmpTarget.color = new Color(1, 1, 0);
                    }

                    if (cacheTarget.shieldDefense <= 0)//쉴드량이 없다면 hp깍기
                    {
                        cacheTarget.healthPoint -= finalDamage;
                    }

                    if (cacheTarget.shieldDefense > 0)//쉴드량이 있다면 쉴드부터 깎기
                    {

                        cacheTarget.shieldDefense -= finalDamage;

                        if (cacheTarget.shieldDefense < 0)
                        {
                            cacheTarget.healthPoint += cacheTarget.shieldDefense;
                            cacheTarget.shieldDefense = 0;
                        }
                    }
                    
                    if (cacheTarget.myMonsterName == "Watteron")//워터론을 때렸다면 반사대미지 입기
                    {
                        if (myParentScript.shieldDefense <= 0)
                        {
                            myParentScript.healthPoint -= cacheTarget.attackPower * cacheTarget.skillPower1;

                        }

                        if (myParentScript.shieldDefense > 0)
                        {
                            myParentScript.shieldDefense -= cacheTarget.attackPower * cacheTarget.skillPower1;

                            if (myParentScript.shieldDefense < 0)
                            {
                                myParentScript.healthPoint += myParentScript.shieldDefense;
                                myParentScript.shieldDefense = 0;

                            }

                            myParentScript.pasteHealthPoint= myParentScript.healthPoint;
                            //CashingMyTarget.PasteshieldDef = CashingMyTarget.shieldDef;
                        }

                    }

                    if (cacheTarget.myMonsterName == "PineArs")//파인아스를 때렸다면 파인아스의 첫번째 타깃 되기
                    {
                        if (cacheTarget.isPinearsAttackedFirst == false)
                        {
                            cacheTarget.objMyTaget = this.gameObject;
                            cacheTarget.isPinearsAttackedFirst = true;
                        }

                    }

                    tmpTarget.text = "" + finalDamage;
                    cacheTarget.pasteHealthPoint = cacheTarget.healthPoint;
                }

            }
            else if (this.myName == "Mato")//마토라면
            {
                AtkMatoBulletByMonster();
            }

            else if(this.myName == "Crape")//크레이프라면
            {
                AtkCrapeBulletByMonster();
            }

            if(this.myName == "StelladeLucy")//스텔라라면
            {
                HaveStellaStar();
            }

            this.transform.position = objMyParent.transform.position;//부모의 위치로 돌아옵니다.
            isFinishedAttackMotion = false;//부모의 공격시간이 다 될때까지 기다리세요

            if (objMyParent == null) // 적에 맞은 순간에 부모가 죽을때는 사라지세요
            {
                Destroy(this.gameObject);
            }
          
        }

        else if(myTargetScript == null && this.transform.position == targetPos && objMyParent != null)// 목표물이 죽었을 때 목표물의 시체에 도달했다면
        {
            //그냥 돌아오세요
            this.transform.position = objMyParent.transform.position;//부모의 위치로 돌아옵니다.
            isFinishedAttackMotion = false;//부모의 공격시간이 다 될때까지 기다리세요

        }
        else if((myTargetScript == null && this.transform.position == targetPos && objMyParent == null))//내 타겟이 죽고 내 부모도 죽었다면
        {

            isFinishedAttackMotion = false;
        }

    }

    public void AtkMatoBulletByMonster()//마토의 총알이 몬스터 때릴때
    {
     
          List<MonsterPropertieScript> listEnemy = new List<MonsterPropertieScript>();//여러명을 동시에 때리는 경우 스크립트 

        for (int i = 0; i < myParentScript.listMyEnemyobj.Count; i++)
        {
                if(myParentScript.listMyEnemyobj[i] == null)
                {

                   continue;
                }

                //if (MyParentEnemy.MyEnemy[i] != null)
                else
                {
                    //Debug.Log("실행0");
                    if (myParentScript.listMyEnemyobj[i].transform.position.x <= this.transform.position.x + 1f &&
                        myParentScript.listMyEnemyobj[i].transform.position.x >= this.transform.position.x - 1f &&
                        myParentScript.listMyEnemyobj[i].transform.position.y <= this.transform.position.y + 1f &&
                        myParentScript.listMyEnemyobj[i].transform.position.y >= this.transform.position.y - 1f)//범위 임의값
                    {
                           listEnemy.Add(myParentScript.listMyEnemyobj[i].GetComponent<MonsterPropertieScript>());

                    }
                }

        }

        
        for (int i = 0; i < listEnemy.Count; i++)
        {
             if (listEnemy.Count > 5)//5명 이상 들어왔다면 나머지 삭제
             {
                listEnemy.RemoveAt(Random.Range(0, listEnemy.Count));
                   
             }
        }


        if (listEnemy.Count <= 5)//5명 모두 들어왔다면 공격
        {
            List<MonsterPropertieScript> listCacheTarget = new List<MonsterPropertieScript>();//목표 다중타겟
            List<TextMeshProUGUI> listTargetTmp = new List<TextMeshProUGUI>();//타겟이 맞았을 때 텍스트가 나올 변수
            List<GameObject> listDamageObj = new List<GameObject>();//텍스트가 나오는 캔버스
            List<float> listAtkRandomCritical = new List<float>();//크리티컬이 터졌는지에 대한 변수
            List<int> listFinalDamage = new List<int>();//공격식을 적용한 후 데미지
            List<float> evasionRandom = new List<float>();//회피가 되었는지 체크하기 위한 변수

            for (int i = 0; i < listEnemy.Count; i++)
            {
              
                listCacheTarget.Add(listEnemy[i].GetComponent<MonsterPropertieScript>()); //= MyTarget.GetComponent<MonsterPropertiesScript>();
                listDamageObj.Add(Instantiate(myParentScript.objDamageTmpcanv, listCacheTarget[i].objMyDamagecanv.transform.position, Quaternion.identity, myTargetScript.objMyDamagecanv.transform));
                listTargetTmp.Add(listDamageObj[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());//...
                listAtkRandomCritical.Add(Random.Range(0, 100));
                evasionRandom.Add(Random.Range(1, 100));
               
            }

            for (int i = 0; i < listCacheTarget.Count; i++)
            {
                if (listCacheTarget[i].evasion >= evasionRandom[i])
                {
                    listTargetTmp[i].text = "evasion";
                    listFinalDamage.Add(0);

                }

                else
                {

                    if (listAtkRandomCritical[i] > criticalPercent)//크리티컬이 안터졌다면
                    {
                        listFinalDamage.Add((int)((myDamage * 100 / (100 + listCacheTarget[i].defense)) + ((myDamage * 100 / (100 + listCacheTarget[i].defense))
                            * Random.Range(-0.05f, 0.06f))));
                    }

                    else if (listAtkRandomCritical[i] <= criticalPercent)//크리티컬이 터졌다면
                    {
                        listFinalDamage.Add((int)((myDamage * 100 / (100 + listCacheTarget[i].defense)) + ((myDamage * 100 / (100 + listCacheTarget[i].defense))
                        * Random.Range(-0.05f, 0.06f) + myDamage * criticalPower)));
                        listTargetTmp[i].color = new Color(255, 255, 0);
                    }

                    if (listCacheTarget[i].shieldDefense <= 0)//쉴드가 업다면 hp깎기
                    {
                        listCacheTarget[i].healthPoint -= listFinalDamage[i];
                    }

                    else if (listCacheTarget[i].shieldDefense > 0)//쉴드가 있다면 쉴드부터 깎기
                    {
                        listCacheTarget[i].shieldDefense -= listFinalDamage[i];

                        if (listCacheTarget[i].shieldDefense < 0)
                        {
                            listCacheTarget[i].healthPoint += listCacheTarget[i].shieldDefense;
                            listCacheTarget[i].shieldDefense = 0;

                        }

                        //CashingMyTarget[i].PasteshieldDef = CashingMyTarget[i].shieldDef;

                    }

                    if (listCacheTarget[i].myMonsterName == "Watteron")//워터론의 반사대미지
                    {
                        if (myParentScript.shieldDefense <= 0)
                        {
                            myParentScript.healthPoint -= listCacheTarget[i].attackPower * listCacheTarget[i].skillPower1;

                        }

                        if (myParentScript.shieldDefense > 0)
                        {
                            myParentScript.shieldDefense -= listCacheTarget[i].attackPower * listCacheTarget[i].skillPower1;

                            if (myParentScript.shieldDefense < 0)
                            {
                                myParentScript.healthPoint += myParentScript.shieldDefense;
                                myParentScript.shieldDefense = 0;

                            }

                            myParentScript.pasteHealthPoint = myParentScript.healthPoint;
                            //CashingMyTarget.PasteshieldDef = CashingMyTarget.shieldDef;
                        }

                    }

                    if (listCacheTarget[i].myMonsterName == "PineArs")//파인아스 의 타겟 되기
                    {
                        if (listCacheTarget[i].isPinearsAttackedFirst == false)
                        {
                            listCacheTarget[i].objMyTaget = this.gameObject;
                            listCacheTarget[i].isPinearsAttackedFirst = true;
                        }

                    }

                    listCacheTarget[i].pasteHealthPoint = listCacheTarget[i].healthPoint;
                    listTargetTmp[i].text = "" + listFinalDamage[i];
                }
                Destroy(listDamageObj[i], 2);
            
            }
            listEnemy.RemoveRange(0, listEnemy.Count);
            listCacheTarget.RemoveRange(0, listCacheTarget.Count);
           // MytargetTextColor.RemoveRange(0, MytargetTextColor.Count);
        }

    }

    public void AtkCrapeBulletByMonster()
    {     
            MonsterPropertieScript cacheTarget;//공격 목표의 스크립트
            cacheTarget = myTargetScript.GetComponent<MonsterPropertieScript>();
            List<GameObject> listDamageObj = new List<GameObject>();//텍스트가 나올 캔버스
            List<TextMeshProUGUI> listTargetTmp = new List<TextMeshProUGUI>();//데미지가 몇 떳는지 필드에 나오게 하기위한 텍스트
            float finalDamage;
            float evasionRandom = Random.Range(1, 100);
            float atkRandomCritical;
            atkRandomCritical = Random.Range(0, 100);

            for (int i = 0; i < 3; i++)
            {
                    listDamageObj.Add(Instantiate(myParentScript.objDamageTmpcanv, cacheTarget.objMyDamagecanv.transform.position, Quaternion.identity, myTargetScript.objMyDamagecanv.transform));
                    listTargetTmp.Add(listDamageObj[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());//.text = "" + (pasteHp - Hp);
                    Destroy(listDamageObj[i], 2);

            }

            if (cacheTarget.evasion >= evasionRandom)//회피했다면
            {
                listTargetTmp[0].text = "evasion";
                listTargetTmp[1].text = "evasion";
                listTargetTmp[2].text = "evasion";

            }

            else
            {
                if (atkRandomCritical > criticalPercent)//크리티컬이 터지지 않았다면
                {
                    finalDamage = (int)((myDamage * 100 / (100 + cacheTarget.defense)) + ((myDamage * 100 / (100 + cacheTarget.defense)) * Random.Range(-0.05f, 0.06f)));
                }

                else
                {
                    finalDamage = (int)((myDamage * 100 / (100 + cacheTarget.defense)) + ((myDamage * 100 / (100 + cacheTarget.defense))
                       * Random.Range(-0.05f, 0.06f) + myDamage * criticalPower));
                }

                if (cacheTarget.shieldDefense <= 0)//쉴드가 없다면 hp부터 깎는 다단히트 
                {
                    cacheTarget.healthPoint -= finalDamage;
                    cacheTarget.healthPoint -= finalDamage * 0.5f;
                    cacheTarget.healthPoint -= finalDamage * 0.3f;
                }

                if (cacheTarget.shieldDefense > 0)//쉴드가 있다면 쉴드부터 깎는 다단히트
                {

                    cacheTarget.shieldDefense -= finalDamage;
                    cacheTarget.shieldDefense -= finalDamage * 0.5f;
                    cacheTarget.shieldDefense -= finalDamage * 0.3f;


                    if (cacheTarget.shieldDefense < 0)
                    {
                        cacheTarget.healthPoint += cacheTarget.shieldDefense;
                        cacheTarget.shieldDefense = 0;

                    }

                }

                if (cacheTarget.myMonsterName == "Watteron")//워터론이라면 대미지 반사 입기
                {
                    if (myParentScript.shieldDefense <= 0)
                    {
                        myParentScript.healthPoint -= cacheTarget.attackPower * cacheTarget.skillPower1;

                    }

                    if (myParentScript.shieldDefense > 0)
                    {
                         myParentScript.shieldDefense -= cacheTarget.attackPower * cacheTarget.skillPower1;

                        if (myParentScript.shieldDefense < 0)
                        {
                            myParentScript.healthPoint += myParentScript.shieldDefense;
                            myParentScript.shieldDefense = 0;
                        }

                        myParentScript.pasteHealthPoint = myParentScript.healthPoint;
                        
                    }

                }

            if (cacheTarget.myMonsterName == "PineArs")//파인아스의 타겟되기
            {
                if (cacheTarget.isPinearsAttackedFirst == false)
                {
                    cacheTarget.objMyTaget = this.gameObject;
                    cacheTarget.isPinearsAttackedFirst = true;
                }

            }


                listTargetTmp[0].text = "" + finalDamage;
                listTargetTmp[1].text = "" + Mathf.Round(finalDamage * 0.5f);
                listTargetTmp[2].text = "" + Mathf.Round(finalDamage * 0.3f);
                cacheTarget.pasteHealthPoint = cacheTarget.healthPoint;
            }
    }

    public void HaveStellaStar()//스텔라의 별이 머리위로 뜨게
    {
     
        myParentScript.listStellaStarobj.Add(Instantiate(myParentScript.objStellaBullet, new Vector2(myTargetScript.transform.position.x + ((myParentScript.listStellaStarobj.Count -1) *0.25f),
            myTargetScript.transform.position.y + 1f), Quaternion.identity, myTargetScript.transform));//맞은 적 부모로 
      
    }
}
