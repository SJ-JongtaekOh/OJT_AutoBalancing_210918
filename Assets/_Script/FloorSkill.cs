using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
public class FloorSkill : MonoBehaviour
{
    public MonsterPropertieScript myTargetScript;//내 목표 스크립트
    public Vector3 myTargetPos;//내 목표가 죽었을 시  이동해야하는 위치

    public GameObject objMyParent;//내 부모
    public MonsterPropertieScript myParentScript;//부모의 스크립트

    public float myDamage;//내 데미지 
    public float moveSpeed;//이동속도
    public bool isFinishedAttackMotion; // 부모가 죽더라도 부모가 공격모션을 끝내서 True가 되면 일단 총알은 쭉 발사하게 만들기 위한 Bool값
   

    public float durationTimer;//지속시간 타이머
    public float skillTimer;//스킬 지속시간
    public float myRadius;//범위

    public int countDamage;//몇번 때렸는지 
    public int countPasteDamage;//지금 현재 때린 수랑 비교하기 

    public float criticalPercent;//크리티컬 확률
    public float criticalPower;//크리티컬 파워
    public float skillPower;//스킬 공격력
    public float skillPower2;//스킬공격력2
    public string myParentName;//총알의 속성 

    public List<MonsterPropertieScript> listEnemyScript = new List<MonsterPropertieScript>();//때려줄 적들의 리스트

    SpriteRenderer sprrdThisColor;//색 바꿔주기

    

    // Start is called before the first frame update
    void Start()
    {

        if(objMyParent != null)//부모 넣어주기
        myParentScript = objMyParent.GetComponent<MonsterPropertieScript>(); 

        this.transform.localScale = new Vector2(this.transform.localScale.x + myRadius, this.transform.localScale.y + myRadius);
        countDamage = countPasteDamage;
        sprrdThisColor = this.GetComponent<SpriteRenderer>();
        myParentName = myParentScript.myMonsterName;

       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (objMyParent == null)
        {
            if (myParentName != "Ragon")
            {
                Destroy(this.gameObject);
            }
        }

        if (isFinishedAttackMotion == true)
        {

            if (myTargetScript != null) // 가는도중 타겟이 죽지 않았다면
            {
                if (myParentScript.myMonsterName != "Ragon")//라곤이 아니라면 
                {
                    this.transform.position = myTargetPos;
                }

                else if (myParentScript.myMonsterName == "Ragon")//라곤의 공격은 맵 상단에서 떨어집니다.
                {
                    this.transform.position = Vector2.MoveTowards(this.transform.position, myTargetScript.transform.position, 10 * Time.deltaTime);
                }
            }

            else if (myTargetScript == null) // 가는도중 타겟이 죽었다면 죽었던 위치까진 멈추지말고 도달하세요
            {
                if (myParentScript.myMonsterName != "Ragon")
                {
                    this.transform.position = myTargetPos;
                }

                else if(myParentScript.myMonsterName == "Ragon")
                {
                    this.transform.position =  Vector2.MoveTowards(this.transform.position, myTargetPos, 10 * Time.deltaTime);
                }
            }

            if (myParentScript.myMonsterName == "Gurulic")//갈릭장판
            {
                IsGurlicFloorActive();
            }

            else if( myParentScript.myMonsterName == "Laemon")//레이몬장판
            {
                IsLaymonFloorActive();
            }

            else if(myParentScript.myMonsterName == "Ragon")//라곤장판
            {

                if (myTargetScript != null)//타겟이 안죽었으면 타겟한테 가세요
                {
                    if (this.transform.position == myTargetScript.transform.position)  
                    {
                        IsRagonFloorActive();

                    }
                }
                
                else if( myTargetScript == null)//타겟이 죽었으면 타겟이 죽었던 위치까지 
                {
                    if (this.transform.position == myTargetPos)
                        IsRagonFloorActive();
                }

            }
            else if (myParentScript.myMonsterName == "Lio")//리오장판
            {
                IsLioFloorActive();
            }

        }

       
        else if (isFinishedAttackMotion == false) //아직 출발을안하였고
        {
            if (objMyParent != null)//부모가 살아있으면
            {
                this.transform.position = objMyParent.transform.position; //출발도 안했고 부모도 살아있으면 부모를 계속 따라다니세요

               // SpriteRenderer thisColor = this.GetComponent<SpriteRenderer>();

            }

        }

    }

    public void IsGurlicFloorActive()//갈릭도사의 장판 활성화
    {
        durationTimer += Time.deltaTime;

        if (durationTimer >= 1 && countDamage < 2)//1초당 한번씩 2번 때림
        {
            countDamage += 1;
            durationTimer = 0;
        }

        if (durationTimer >= 0.5f && countDamage == 2)//마지막은 0.5초에 때리고 장판 회수
        {
            countDamage += 1;
            durationTimer = 0;
        }

        if (countDamage == 3)//3번을 때렸으면 장판회수
        {
            countDamage = 0;
            durationTimer = 0;

            if (objMyParent != null)
            {
                this.gameObject.transform.position = objMyParent.transform.position;
            }

            else
            {
                Destroy(this.gameObject);
            }

            isFinishedAttackMotion = false;
        }


        if (countPasteDamage != countDamage)//장판공격 횟수가 증가할 때 마다 적들 대미지 깎기
        {
            for (int i = 0; i < myParentScript.listMyEnemyobj.Count; i++)
            {
                if (myParentScript.listMyEnemyobj[i] == null)
                {
                    continue;
                }

                else
                {            
                    //Debug.Log("실행0");
                    if (myParentScript.listMyEnemyobj[i].transform.position.x <= this.transform.position.x + myRadius &&
                        myParentScript.listMyEnemyobj[i].transform.position.x >= this.transform.position.x - myRadius &&
                        myParentScript.listMyEnemyobj[i].transform.position.y <= this.transform.position.y + myRadius &&
                        myParentScript.listMyEnemyobj[i].transform.position.y >= this.transform.position.y - myRadius)//범위 안에 들어왔다면 공격리스트에 넣어라
                    {
                        listEnemyScript.Add(myParentScript.listMyEnemyobj[i].GetComponent<MonsterPropertieScript>());

                    }

                }

            }

            List<TextMeshProUGUI> listTargetTmp = new List<TextMeshProUGUI>();//데미지 Text 나오게 하기위한 변수
            List<GameObject> listDamageObj = new List<GameObject>();//데미지 캔버스들 모음
            List<float> listRandomCritical = new List<float>();//크리티컬이 떳는지 판별하기 위해 있는 임시변수
            List<int> listFinalDamage = new List<int>();//공격식 모두 계산한 데미지값
            float evasionRandom = Random.Range(0, 100);//회피가 됫는지 판별하기 위한 변수

            for (int i = 0; i < listEnemyScript.Count; i++)
            {
                if (listEnemyScript[i] == null)
                {
                    listEnemyScript.RemoveAt(i);
                    continue;
                }

                if (listEnemyScript[i].evasion >= evasionRandom)//회피했다면
                {
                    listTargetTmp[i].text = "evasion";
                }

                else//회피하지 못하였다면
                {
                    listDamageObj.Add(Instantiate(myParentScript.objDamageTmpcanv, listEnemyScript[i].objMyDamagecanv.transform.position, Quaternion.identity, listEnemyScript[i].objMyDamagecanv.transform));
                    listTargetTmp.Add(listDamageObj[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());//...
                    listRandomCritical.Add(Random.Range(0, 100));

                    if (listRandomCritical[i] > criticalPercent)//크리가 안터졌다면
                    {
                        listFinalDamage.Add((int)((myDamage * 100 / (100 + listEnemyScript[i].defense)) + ((myDamage * 100 / (100 + listEnemyScript[i].defense)) * Random.Range(-0.05f, 0.06f))));
                    }

                    else if (listRandomCritical[i] <= criticalPercent)//크리티컬이 터졌다면
                    {
                        listFinalDamage.Add((int)((myDamage * 100 / (100 + listEnemyScript[i].defense)) + ((myDamage * 100 / (100 + listEnemyScript[i].defense))
                       * Random.Range(-0.05f, 0.06f) + myDamage * criticalPower)));

                        listTargetTmp[i].color = new Color(255, 255, 0);
                    }

                    if (listEnemyScript[i].shieldDefense <= 0)//쉴드가 없다면 hp부터 깎기 
                    {
                        listEnemyScript[i].healthPoint -= listFinalDamage[i];
                    }

                    if (listEnemyScript[i].shieldDefense > 0)//쉴드가 있다면 쉴드부터 깎기
                    {

                        listEnemyScript[i].shieldDefense -= listFinalDamage[i];

                        if (listEnemyScript[i].shieldDefense < 0)
                        {
                            listEnemyScript[i].healthPoint += listEnemyScript[i].shieldDefense;
                            listEnemyScript[i].shieldDefense = 0;

                        }

                        listEnemyScript[i].pasteHealthPoint = listEnemyScript[i].shieldDefense;

                    }

                    if (listEnemyScript[i].myMonsterName == "Watteron")//워터론의 대미지 반사
                    {
                        if (myParentScript.shieldDefense <= 0)
                        {
                            myParentScript.healthPoint -= listEnemyScript[i].attackPower * listEnemyScript[i].skillPower1;

                        }

                        if (myParentScript.shieldDefense > 0)
                        {
                            myParentScript.shieldDefense -= listEnemyScript[i].attackPower * listEnemyScript[i].skillPower1;

                            if (myParentScript.shieldDefense < 0)
                            {
                                myParentScript.healthPoint += myParentScript.shieldDefense;
                                myParentScript.shieldDefense = 0;

                            }

                            myParentScript.pasteHealthPoint = myParentScript.healthPoint;
                            //CashingMyTarget.PasteshieldDef = CashingMyTarget.shieldDef;
                        }

                    }

                    if (listEnemyScript[i].myMonsterName == "PineArs")//파인아스의 첫번째 타겟 되기
                    {
                        if (listEnemyScript[i].isPinearsAttackedFirst == false)
                        {
                            listEnemyScript[i].objMyTaget = this.objMyParent;
                            listEnemyScript[i].isPinearsAttackedFirst = true;
                        }

                    }


                    listEnemyScript[i].pasteHealthPoint = listEnemyScript[i].healthPoint;

                    if (i == listEnemyScript.Count - 1) // 마지막까지때렸다면 회수하기
                    {
                        //EnemyList[i].Hp -= (int)((myDamage * 100 / (100 + EnemyList[i].Def)) + ((myDamage * 100 / (100 + EnemyList[i].Def)) * Random.Range(-0.05f, 0.06f)));
                        listEnemyScript.RemoveRange(0, listEnemyScript.Count);

                        if (objMyParent != null && countDamage == 3)
                        {
                            this.transform.position = objMyParent.transform.position;
                            isFinishedAttackMotion = false;

                        }

                    }

                    listTargetTmp[i].text = "" + listFinalDamage[i];
                    Destroy(listDamageObj[i], 2);
                }
            }

            countPasteDamage = countDamage;
        }
    }

    public void IsLaymonFloorActive()//레이몬의 장판 함수
    {
        durationTimer += Time.deltaTime;

        sprrdThisColor.color = new Color(sprrdThisColor.color.r, sprrdThisColor.color.g, sprrdThisColor.color.b, 155/255f);

        if(durationTimer >1)
        {
            durationTimer = 0;
            if (objMyParent != null)//1초가 지나고 부모가 있다면 장판회수
            {
                this.transform.position = objMyParent.transform.position;

                sprrdThisColor.color = new Color(sprrdThisColor.color.r, sprrdThisColor.color.g, sprrdThisColor.color.b, 0);

            }
            isFinishedAttackMotion = false;
        }


            for (int i = 0; i < myParentScript.listMyEnemyobj.Count; i++)//디버프를 걸기위한 준비하는 for문
            {
                if (myParentScript.listMyEnemyobj[i] == null)
                {

                    continue;
                }

                else
                {
                   

                    //Debug.Log("실행0");
                    if (myParentScript.listMyEnemyobj[i].transform.position.x <= this.transform.position.x + myRadius &&
                        myParentScript.listMyEnemyobj[i].transform.position.x >= this.transform.position.x - myRadius &&
                        myParentScript.listMyEnemyobj[i].transform.position.y <= this.transform.position.y + myRadius &&
                        myParentScript.listMyEnemyobj[i].transform.position.y >= this.transform.position.y - myRadius)//범위 안에 들어왔다면 공격리스트에 넣어라
                    {
                        listEnemyScript.Add(myParentScript.listMyEnemyobj[i].GetComponent<MonsterPropertieScript>());

                    }
                   
                }

            }

            for (int i = 0; i < listEnemyScript.Count; i++)
            {
                if (listEnemyScript[i] == null)
                {
                    listEnemyScript.RemoveAt(i);
                }

                else
                {

                    if (listEnemyScript[i].isLeamonDebuffActive == false)//범위안에 있는것들 방어력 깎기
                    {
                        listEnemyScript[i].defense = listEnemyScript[i].defense * skillPower;
                        listEnemyScript[i].isLeamonDebuffActive = true;
                    }

                    if (i == listEnemyScript.Count - 1) // 마지막까지때렸다면
                    {
                        //EnemyList[i].Hp -= (int)((myDamage * 100 / (100 + EnemyList[i].Def)) + ((myDamage * 100 / (100 + EnemyList[i].Def)) * Random.Range(-0.05f, 0.06f)));
                        listEnemyScript.RemoveRange(0, listEnemyScript.Count);

                    }

                }
            }

    }

    

    public void IsRagonFloorActive()//라곤의 장판 공격
    {

      
              for (int i = 0; i < myParentScript.listMyEnemyobj.Count; i++)
              {
                    if (myParentScript.listMyEnemyobj[i] == null)
                    {
                        continue;
                    }

                    else
                    {
                   
                        if (myParentScript.listMyEnemyobj[i].transform.position.x <= this.transform.position.x + myRadius &&
                            myParentScript.listMyEnemyobj[i].transform.position.x >= this.transform.position.x - myRadius &&
                            myParentScript.listMyEnemyobj[i].transform.position.y <= this.transform.position.y + myRadius &&
                            myParentScript.listMyEnemyobj[i].transform.position.y >= this.transform.position.y - myRadius)//범위 안에 들어왔다면 공격리스트에 넣어라
                        {
                            listEnemyScript.Add(myParentScript.listMyEnemyobj[i].GetComponent<MonsterPropertieScript>());
                    
                        }

                    }

              }

            List<TextMeshProUGUI> listTargetTmp = new List<TextMeshProUGUI>();//데미지 텍스트
            List<GameObject> listDamageObj = new List<GameObject>();//데미지 텍스트가 나올 위치
            List<float> listRandomCritical = new List<float>();//크리티컬 터졌는지 체크
            List<int> listFinalDamage = new List<int>();//공격식 다 적용
           
            for (int i = 0; i < listEnemyScript.Count; i++)
            {
                if (listEnemyScript[i] == null)
                {
                    listEnemyScript.RemoveAt(i);
                }
                
                 

                else//회피 못했나요
                {
                    listDamageObj.Add(Instantiate(myParentScript.objDamageTmpcanv, listEnemyScript[i].objMyDamagecanv.transform.position, Quaternion.identity, listEnemyScript[i].objMyDamagecanv.transform));
                    listTargetTmp.Add(listDamageObj[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());//...
                    listRandomCritical.Add(Random.Range(0, 100));

                    if (listRandomCritical[i] > criticalPercent)//크리가 안터졌다면
                    {
                        listFinalDamage.Add((int)((myDamage * 100 / (100)) + ((myDamage * 100 / (100 )) * Random.Range(-0.05f, 0.06f))));
                    }

                    else if (listRandomCritical[i] <= criticalPercent)//크리티컬이 터졌다면
                    {
                        listFinalDamage.Add((int)((myDamage * 100 / (100)) + ((myDamage * 100 / (100 ))
                       * Random.Range(-0.05f, 0.06f) + myDamage * criticalPower)));

                        listTargetTmp[i].color = new Color(255, 255, 0);
                    }

                    if (listEnemyScript[i].shieldDefense <= 0)//쉴드가 없으면 hp 깍기
                    {
                        listEnemyScript[i].healthPoint -= listFinalDamage[i];
                    }

                    if (listEnemyScript[i].shieldDefense > 0)//쉴드가 있으면 쉴드부터 깎기
                    {
                        listEnemyScript[i].shieldDefense -= listFinalDamage[i];

                        if (listEnemyScript[i].shieldDefense < 0)
                        {
                            listEnemyScript[i].healthPoint += listEnemyScript[i].shieldDefense;
                            listEnemyScript[i].shieldDefense = 0;

                        }

                        listEnemyScript[i].pasteHealthPoint = listEnemyScript[i].shieldDefense;

                    }

                    if (listEnemyScript[i].myMonsterName == "Watteron")//워터론이라면 대미지 반사 받기
                    {
                        if (myParentScript.shieldDefense <= 0)
                        {
                            myParentScript.healthPoint -= listEnemyScript[i].attackPower * listEnemyScript[i].skillPower1;

                        }

                        if (myParentScript.shieldDefense > 0)
                        {
                            myParentScript.shieldDefense -= listEnemyScript[i].attackPower * listEnemyScript[i].skillPower1;

                            if (myParentScript.shieldDefense < 0)
                            {
                                myParentScript.healthPoint += myParentScript.shieldDefense;
                                myParentScript.shieldDefense = 0;

                            }

                            myParentScript.pasteHealthPoint = myParentScript.healthPoint;
                            
                        }

                    }

                    if (listEnemyScript[i].myMonsterName == "PineArs")//파인아스라면 첫타자 되기
                    {
                        if (listEnemyScript[i].isPinearsAttackedFirst == false)
                        {
                        listEnemyScript[i].objMyTaget = this.objMyParent;
                           listEnemyScript[i].isPinearsAttackedFirst = true;
                        }

                    }

                     listEnemyScript[i].pasteHealthPoint = listEnemyScript[i].healthPoint;
                    listTargetTmp[i].text = "" + listFinalDamage[i];
                    Destroy(listDamageObj[i], 2);
                }
            }

        myParentScript.listMyFloorSkillPoolScript.Remove(this);
        Destroy(this.gameObject);

    }

    public void IsLioFloorActive()//리오의 장판공격
    {
        durationTimer += Time.deltaTime;

        sprrdThisColor.color = new Color(sprrdThisColor.color.r, sprrdThisColor.color.g, sprrdThisColor.color.b, 155 / 255f);

        if (durationTimer > 3)//3초가 지났으면 회수
        {

            durationTimer = 0;

            if (objMyParent != null)//투명해져서 맵에 안보이게 하기 부모가 회수할 때
            {
                this.transform.position = objMyParent.transform.position;

                sprrdThisColor.color = new Color(sprrdThisColor.color.r, sprrdThisColor.color.g, sprrdThisColor.color.b, 0);

            }
            isFinishedAttackMotion = false;
        }


        for (int i = 0; i < myParentScript.listMyEnemyobj.Count; i++)//디버프 범위 안에 있는 적들 넣어주기
        {
            if (myParentScript.listMyEnemyobj[i] == null)
            {

                continue;
            }

            else
            {
                //float Myx = this.transform.localScale.x / MyRadius;
                //Debug.Log("실행0");
                if (myParentScript.listMyEnemyobj[i].transform.position.x <= this.transform.position.x + myRadius &&
                    myParentScript.listMyEnemyobj[i].transform.position.x >= this.transform.position.x - myRadius &&
                    myParentScript.listMyEnemyobj[i].transform.position.y <= this.transform.position.y + myRadius &&
                    myParentScript.listMyEnemyobj[i].transform.position.y >= this.transform.position.y - myRadius)//범위 안에 들어왔다면 공격리스트에 넣어라
                {
                    listEnemyScript.Add(myParentScript.listMyEnemyobj[i].GetComponent<MonsterPropertieScript>());

                }

            }

        }

        for (int i = 0; i < listEnemyScript.Count; i++)
        {
            if (listEnemyScript[i] == null)
            {
                listEnemyScript.RemoveAt(i);
            }

            else
            {

                if (listEnemyScript[i].isInLioGas == false)//리오 디버프 묻혀주기
                {
                    //EnemyList[i].Def = EnemyList[i].Def * SkillPower;

                    listEnemyScript[i].attackSpeed += listEnemyScript[i].attackSpeed * skillPower;
                    listEnemyScript[i].moveSpeed = listEnemyScript[i].moveSpeed * skillPower2;
                    listEnemyScript[i].lioGasCount = 2;
                    listEnemyScript[i].isInLioGas = true;


                }

                if (i == listEnemyScript.Count - 1) // 마지막까지때렸다면
                {
                    //EnemyList[i].Hp -= (int)((myDamage * 100 / (100 + EnemyList[i].Def)) + ((myDamage * 100 / (100 + EnemyList[i].Def)) * Random.Range(-0.05f, 0.06f)));
                    listEnemyScript.RemoveRange(0, listEnemyScript.Count);

                }

            }
        }

    }


}
