using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public enum State
{
    Idle,
    Walk,
    Attack,
    die,
    Skill

}
public class MonsterAnimation : MonoBehaviour
{  
    //public SkeletonMecanim Skeletonanimator;
    public Animator animMe;//내 애니메이터
    //public AnimationReferenceAsset[] AnimClip;
    public MonsterPropertieScript myStateScript; //내 정보 스크립트
    //public bool AlreadyHit;
    public bool isSkillActive;//스킬 사용하고 있는지 판단 여부를 위한 변수
    public Vector2 posSave;//블러디베리 위치저장
    public bool isSavedBloodBerryPos;//블러디베리 위치 저장 Bool값 


    // Start is called before the first frame update

    private void Awake()
    {
        animMe = GetComponent<Animator>();
        myStateScript = GetComponent<MonsterPropertieScript>();
    }
    void Start()
    {       
        animMe.SetFloat("AttackSpeed", 1);      
    }    
    // Update is called once per frame

    private void FixedUpdate()
    {
        
        if (myStateScript.nowState == State.Idle)//내 상태가 아이들 일 때
        {       
            animMe.SetTrigger("Idle");
            animMe.ResetTrigger("Attack");
            animMe.ResetTrigger("Skill");
        }

        else
        {
            animMe.ResetTrigger("Idle");
            
        }

        if (myStateScript.listMyEnemyobj.Count > 0)//적이 있을 때 
        {

            if (myStateScript.nowState == State.Walk)//상태가 워크면 워크 애니메이터 동작
            {
                //animMe.SetFloat("State", 0.1f);
                animMe.SetTrigger("Walk");
            }

            else if (myStateScript.nowState == State.Attack)//상태가 공격이면 공격 애니메이터 동작
            {
                animMe.SetTrigger("Attack");
                animMe.ResetTrigger("Skill");

                myStateScript.attackSpeedCount = 0;

                if (myStateScript.myMonsterName == "BloodyBerry")//블러디 베리면 블러디베리의 돌아올 위치 저장
                {
                    SavePosBloddyBerry();

                }

                if (animMe.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animMe.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {

                    if (myStateScript.myMonsterName == "Ragon")//내가만약 라곤이라면 
                    {
                        myStateScript.AttackEnemy();
                    }

                    else if (myStateScript.myMonsterName != "ChecheRiry")//체체엔 리리는 idle상태가 된다
                    {
                        myStateScript.nowState = State.Idle;

                    }

                    animMe.Play("Attack", 0, 0);//공격 애니메이터 초기화
                    myStateScript.evasion = 0;

                }               
            }

            //else if (myStateScript.nowState == State.die)//내가 죽은상태면 
            //{
            //    animMe.SetTrigger("Die");

            //    if (animMe.GetCurrentAnimatorStateInfo(0).IsName("Die") && animMe.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            //    {
            //        if (myStateScript.myMonsterName == "ChillyBomb")
            //        {
            //            SummonChillyBomb();
            //        }

            //        Destroy(this.gameObject);//애니메이션이 다 재생되고 난 뒤 없어지세요

            //    }

            //}

            else if (myStateScript.nowState == State.Skill)//내가 스킬 쓰고 있는 상태라면 
            {
                animMe.SetTrigger("Skill");
                animMe.ResetTrigger("Attack");

                if (animMe.GetCurrentAnimatorStateInfo(0).IsName("Skill") && animMe.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)//스킬 애니메이터의 중간이 지나갔을때             
                {
                    if (isSkillActive == false)
                    {               

                        if (myStateScript.myMonsterName != "Loe" &&  myStateScript.myMonsterName != "Ragon" && myStateScript.myMonsterName != "Lio")
                        {
                            myStateScript.AttackEnemy();
                        }

                       

                    }

                    isSkillActive = true;
                }

                if (animMe.GetCurrentAnimatorStateInfo(0).IsName("Skill") && animMe.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)//스킬애니메이터가 다 재생된 후
                {

                    if (myStateScript.healthPoint > 0 && myStateScript.objMyTaget != null)
                    {
                        myStateScript.nowState = State.Attack;
                        myStateScript.skillCoolTimeCount = 0;

                    }

                }

            }
        }

        if(myStateScript.nowState == State.die)//죽었을 때 
        {
            animMe.SetTrigger("Die");

            if (animMe.GetCurrentAnimatorStateInfo(0).IsName("Die") && animMe.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                if (myStateScript.myMonsterName == "ChillyBomb")
                {
                    SummonChillyBomb();
                }

                else
                {
                    Destroy(this.gameObject);
                }
            }

        }
    }

    public void SummonChillyBomb()//꼬마 페퍼팡 소환
    {
        MonsterPropertieScript MyChildPome;//꼬마 페퍼팡 스크립트
        string MyTag = this.gameObject.tag;// 내 테그 그대로 물려주기
        MyChildPome = Instantiate(myStateScript.gameManagerScript.shotChill.gameObject, this.transform.position, this.transform.rotation).GetComponent<MonsterPropertieScript>();
        MyChildPome.objMyTaget = myStateScript.objMyTaget;
        MyChildPome.gameObject.tag = MyTag;

       

        List<MonsterPropertieScript> listMyEnemy = new List<MonsterPropertieScript>();//내 적들 그대로 물려주기

        for (int i = 0; i < myStateScript.listMyEnemyobj.Count; i++)
        {
            listMyEnemy.Add(myStateScript.listMyEnemyobj[i].GetComponent<MonsterPropertieScript>());
            MyChildPome.listMyEnemyobj.Add(myStateScript.listMyEnemyobj[i]);
            MyChildPome.listMyEnemyPosDistance.Add(myStateScript.listMyEnemyPosDistance[i]);

            listMyEnemy[i].listMyEnemyobj.Add(MyChildPome.gameObject);
            listMyEnemy[i].listMyEnemyPosDistance.Add(0);

        }

        if (this.CompareTag("EnemyTeam"))//꼬마 페퍼팡에게 내 팀들 넣어주기
        {
            myStateScript.gameManagerScript.listEnemyUnitObj.Add(MyChildPome.gameObject);
            myStateScript.gameManagerScript.listCashingEnemyUnitScript.Add(MyChildPome);
        }

        else if (this.CompareTag("MyTeam"))//꼬마 페퍼팡에게 내 적들 넣어주기
        {
            myStateScript.gameManagerScript.listMyUnitObj.Add(MyChildPome.gameObject);
            myStateScript.gameManagerScript.listCashingMyUnitScript.Add(MyChildPome);

        }

        Destroy(this.gameObject);
    }

    public void attack() // 바꿀수없음 animator의 이벤트와 연결되어 있기때문
    {
      //  AlreadyHit = false;

        if (myStateScript.myMonsterName != "Ragon")//라곤이 아니라면 공격
        {
            if (animMe.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                myStateScript.AttackEnemy();
              
            }
        }

        if (myStateScript.myMonsterName == "OSteen")//오스틴이라면 백어택
        {
            if (animMe.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                myStateScript.evasion = 100;
                if (this.transform.position.x > 0.5f && this.transform.position.x < 4.5f)
                {
                    myStateScript.isKnockBackSkillActive = true;
                }
            }
        }

        
    }
    
    public void skill()//바꿀수없음 Animator 이벤트와 연결되어 있기때문
    {
        isSkillActive = false;

        if (animMe.GetCurrentAnimatorStateInfo(0).IsName("Skill"))//스킬을 발동하고 있다면
        {
            if(myStateScript.myMonsterName == "Lio")
            {
                myStateScript.LioFloorSummon();//리오의 장판
            }

            else if (myStateScript.myMonsterName == "StelladeLucy")
            {
                myStateScript.IsStellarBuffActive();//스텔라의 스킬발동
            }

        }

        if (myStateScript.myMonsterName == "ChillyBomb")//페퍼팡은 죽는다
        {
            myStateScript.nowState = State.die;
            animMe.SetTrigger("Die");           
            SummonChillyBomb();
            myStateScript.AttackEnemy();
           
        }

    }

    public void end()//바꿀수없음 Animator 이벤트와 연결되어 있기때문
    {
        if(myStateScript.myMonsterName == "Ragon")
        {
            if (animMe.GetCurrentAnimatorStateInfo(0).IsName("Attack"))//공격 애니메이터 모션이 끝나면 그때 공격
            {
                myStateScript.AttackEnemy();

            }
        }

        if (myStateScript.myMonsterName == "OSteen" || myStateScript.myMonsterName == "BloodyBerry")
        {
            if (animMe.GetCurrentAnimatorStateInfo(0).IsName("Attack"))//공격을 다 맞췄으면 회피율은 0 
            {
                myStateScript.evasion = 0;
            }
        }

        if(myStateScript.myMonsterName == "Pome")//포메가 죽으면 작은 포메 소환
        {
            if (animMe.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                myStateScript.SummonSmallPome();
            }
        }

        if(myStateScript.myMonsterName == "StelladeLucy")//스킬에니메이션이 끝나면 그때 발동
        {
            if (animMe.GetCurrentAnimatorStateInfo(0).IsName("Skill"))
            {
                myStateScript.UseStellarSkill();

            }


        }
      
        if (myStateScript.nowState != State.die)//죽은 상태가 아니라면 
        {
            if (animMe.GetCurrentAnimatorStateInfo(0).IsName("Attack") || animMe.GetCurrentAnimatorStateInfo(0).IsName("Skill"))//공격하거나 스킬상태에서 체체엔 리리라면
            {
                if (myStateScript.myMonsterName != "ChecheRiry")
                {

                    myStateScript.nowState = State.Idle;//아이들 상태로 되돌아옴
                    animMe.SetTrigger("Idle");
                 
                }

            }

        }

        if (myStateScript.nowState == State.die)//죽는모션이 끝났으면 
        {
           if(animMe.GetCurrentAnimatorStateInfo(0).IsName("Die"))
           {

                if (myStateScript.myMonsterName == "ChillyBomb")//페퍼팡
                {
                    SummonChillyBomb();//소환하고 죽어
                }

                else
                {
                    Destroy(this.gameObject);//죽어
                }

            }
        }
    }
    public void SavePosBloddyBerry()//블러디 베리의 공격하고 다시 되돌아올 위치 저장
    {
        if (isSavedBloodBerryPos == false)
        {
            posSave = this.transform.position;
            isSavedBloodBerryPos = true;
        }

        if (animMe.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animMe.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f
             && animMe.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animMe.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.75f)
         {
                       
            myStateScript.evasion = 50;

            if (myStateScript.objMyTaget != null)
            {
                this.gameObject.transform.position = new Vector2(myStateScript.objMyTaget.transform.position.x + 1f, myStateScript.objMyTaget.transform.position.y);
            }
         }

        else if(animMe.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animMe.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.25f
            || animMe.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animMe.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
        {
            this.transform.position = posSave;
            isSavedBloodBerryPos = false;
     
        }
    }

}
