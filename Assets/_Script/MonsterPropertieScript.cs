using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 public enum AttackerType
{
    ShotAttacker,//근거리 공격이라면
    longAttacker,//원거리 공격이라면
    OnlySkillAttacker,//스킬로만 공격한다면
    SkillAttacker//스킬로도 공격하고 평타원거리로도 공격한다면
}

public enum MonsterName//CSVIndex 찾기
{

    //-3 기존 index -3
    C_Wrr_01,
    C_Wrr_02 = 20,
    B_Arch_01 = 220,
    C_Arch_02 = 60,
    C_Tnk_01 = 80,
    C_Skl_02 = 140,
    B_Wrr_01 = 180,
    B_Tnk_01 = 260,
    C_Skl_03 = 160,
    B_Arch_02 = 240,
    B_Tnk_02 = 280,
    B_Wrr_02 = 200,
    A_Arch_01 = 400,
    C_Tnk_02 = 100,
    C_Arch_01 = 40,
    B_Skl_01 = 300,
    B_Skl_02 = 323,
    B_Skl_03 = 340,
    A_Wrr_01 = 360,
    A_Wrr_02 = 380,
    A_Arch_02 = 420,
    A_Tnk_01 = 440,
    A_Tnk_02 = 460,
    A_Skl_01 = 480,
    A_Skl_02 = 500,
    A_Skl_03 = 520,
    S_Wrr_01 = 540,
    S_Tnk_01 = 580,
    C_Skl_01 = 120,
    S_Skl_01 = 600,
    S_Arch_01 = 560

}

public class MonsterPropertieScript : MonoBehaviour
{
    [Header("int,float")]
    public int myLevel;//현재 레벨
    public float attackPower;//공격력
    public float healthPoint;//체력
    public float attackSpeed;//공격속도
    public float attackSpeedCount;//공격속도 타이머
    public float defense;//방어력
    public float moveSpeed;//이동속도
    public float skillRadius;//스킬 범위
    public float skillDuration;//스킬 지속시간
   // public float skillDurationTimer;//스킬 지속시간 타이머
    public float criticalPercent;//크리티컬 확률
    public float criticalPower;//크리티컬 공격력
    public float skillPower1;//스킬 공격력
    public float skillPower2;//스킬 2공격력
    public float skillCoolTime;//스킬 쿨타임
    public float skillCoolTimeCount;//스킬 쿨타임 타이머
    public float skillEffect1;//스킬 효과1
    public float skillEffect2;//스킬 효과2
    public float shieldDefense;//쉴드 방어력
    public float pasteShieldDefense;//현재 쉴드 방어력이 닳았는지 체크하는 변수
    public float attackDistance;//공격범위
    public float targetDistanceXY;//내 목표물의 DIstnace값
    public string myMonsterName;//내 몬스터가 무엇인지
    //public float attackSpeedCount;공격속도 타이머
    public float pasteHealthPoint;//현재 피가 닳았는지 체크하는 변수
    public float maxHealthPoint;//최대 채력이 몇이였는지 
    public int atkCount;//몇대 때렸는지 
    public float stunCoolTime;//스턴 쿨타임 
    public float moveCount;//얼만큼 이동했는지(두리곤 공격버프를 위한)
    public float evasion;//회피력
    public float knocBackMoved;//코코에게 넉백을 당하는 수치 or 오스틴이 공격했을 때 넉백 하는 수치
    public float lioGasCount;//리오의 가스 타이머
    public bool isLeamonDebuffActive; // 이미 레이몬의 디버프가 걸려있다면 
    public bool isGrapSkillActive;//그랩으로 당기고 있다면
    public bool isKnockBackSkillActive;//넉백중이라면
    public bool isPinearsAttackedFirst;//파인아스가 처음맞았다면
    public bool isInLioGas;//Lio의 가스에 이미 맞았다면 
    [Header("ObjectList")]
    public GameManager gameManagerScript;//게임매니저
    public GameObject objMyTaget;//내 타겟
    public GameObject objMyFirstBullet;//내총알
    public GameObject objDamageTmpcanv;//TextMeshealthPointro
    public GameObject objMyDamagecanv;//DamageText의 위치 잡아주기 위한 오브젝트
    public List<BulletScript> listMyBulletPoolScript = new List<BulletScript>();//공격속도가 너무 빨라 총알을 다시 사용할 수 없을 때 대비하기 위한 리스트
    public List<FloorSkill> listMyFloorSkillPoolScript = new List<FloorSkill>();//장판 사용이 너무 빨라 다시 사용할 수 없을 때 대비하기 위한 리스트
    public List<GameObject> listMyEnemyobj;// 내 적의 오브젝트 리스트
    public List<float> listMyEnemyPosDistance = new List<float>(); //내 적들의 거리 리스트S
    //public List<Transform> allTeamsObjects = new List<Transform>();
    //public List<float> allTeamPosXY = new List<float>();
    public GameObject objShieldEffect;//Hp대신 방어막 수치가 있을 때 간단한 이펙트
    public GameObject objGrapMonster;//그랩에 끌려오는 몬스터의 오브젝트
    public Scrollbar scrbarHealthPoint;//현재 남아있는 Hp의 퍼센트를 보여주기 위한 스크롤바 
    public List<GameObject> listStellaStarobj;// 스텔라 데 루시의 중첩된 별들의 리스트 
    public GameObject objStellaBullet;//중첩된 별들이 몹 위에 보이기 위한 별 오브젝트
    [Header("Enum")]
    public AttackerType myAttackType;//내 공격타입
    public MonsterName stateMonsterName;//Csv 파일 내 이름 
    public State nowState;//애니메이터 작동을 위한 현재 상태
    public int MyScore;
    // Start is called before the first frame update

    private void Awake()
    {
       
        gameManagerScript = FindObjectOfType<GameManager>();
      
    }
    void Start()
    {
        nowState = State.Idle;
        //attackSpeedTimer = OriginalattackSpeed;
       
        if(myLevel == 0)//입력받은 레밸이 없다면 자동으로 1
        {
            myLevel = 1;
        }

        // Damagetext.transform.GetChild(0).GetComponent<TextMeshProUGUI>()
        

        //healthPointScroll = this.transform.GetChild(0).transform.GetChild(0).GetComponent<Scrollbar>();

        if (this.transform.position.y > 0)//처음 시작할때 위에있으면 적
        {
            if (this.gameObject.name != "ShotChillyBomb(Clone)" && this.gameObject.name != "ShotPome(Clone)")//죽어서 소환된 몬스터들은 따로 태그를 정해줌
            {
                this.tag = "EnemyTeam";               
            }
              
        }

        else // 아래있으면 우리팀
        {
            if (this.gameObject.name != "ShotChillyBomb(Clone)" && this.gameObject.name != "ShotPome(Clone)")//죽어서 소환된 몬스터들은 따로 태그를 정해줌
            {
                this.tag = "MyTeam";
            }
            this.transform.localRotation = Quaternion.Euler(this.transform.rotation.x, -180, 0);
            scrbarHealthPoint.GetComponent<RectTransform>().localRotation = Quaternion.Euler(this.transform.rotation.x, -180, 0);
            scrbarHealthPoint.GetComponent<Scrollbar>().handleRect.GetComponent<Image>().color = Color.red;
            
        }

        if (this.gameObject.name == "ShotChillyBomb(Clone)" || this.gameObject.name == "ShotPome(Clone)")
        {
            if(this.CompareTag("MyTeam"))//우리팀이면 hp바가 빨강색
            {
                scrbarHealthPoint.GetComponent<Scrollbar>().handleRect.GetComponent<Image>().color = Color.red;
            }
            else if(this.CompareTag("EnemyTeam"))//적팀이 초록색
            {
                scrbarHealthPoint.GetComponent<Scrollbar>().handleRect.GetComponent<Image>().color = Color.green;
            }
        }

            FindMyState(myMonsterName);//이름과 레벨에 맞게 능력치 부여 받기
    }

    void FixedUpdate()
    {
        
        if(listMyEnemyobj.Count > 0)// 내 적이 살아있을 때
        {
            if (objMyTaget == null)//처음 시작했을 때 무작위로 적 넣어서 비교값 만들어 주기
            {
                if (myMonsterName != "PineArs")//파인아스가 아니라면 무작위 비교값 넣어주기
                {
                    objMyTaget = listMyEnemyobj[Random.Range(0, listMyEnemyobj.Count)];
                }

                else if(myMonsterName == "PineArs")//파인아스라면 일단 적 넣지 말고 hp부터 닳게하기
                {                  
                   if (isPinearsAttackedFirst == false)//파인아스가 맞지 않았다면
                   {
                        if (healthPoint / maxHealthPoint > 0.7f)//70프로 까지 hp 까이기
                        {
                            healthPoint -= 70 * Time.deltaTime;
                            this.pasteHealthPoint = healthPoint;
                        }

                       else if (healthPoint / maxHealthPoint <= 0.7f)//한번도 맞지 않고 hp가 70퍼 까지 닳았다면 적 넣어주기
                       {
                          objMyTaget = listMyEnemyobj[Random.Range(0, listMyEnemyobj.Count)];
                           
                       }
                   }

                   if(isPinearsAttackedFirst == true && objMyTaget == null)//파인아스를 때린 적이 죽었다면 랜덤으로 타겟을 잡아줌
                   {
                        isPinearsAttackedFirst = false;
                        objMyTaget = listMyEnemyobj[Random.Range(0, listMyEnemyobj.Count)];
                   }
                }
            }

            else // 내 타겟이 살아있다면
            {
                if (myMonsterName != "BruceApp" && myMonsterName != "BloodyBerry")//유닛들의 가까운 적 목표 잡기
                {
                    TargetCaught();
                  
                }

                else if (myMonsterName == "BruceApp" || myMonsterName == "BloodyBerry")// 원거리 적 먼저 목표 잡기
                {
                    TargetLongAttackerCaught();
                  
                }
              
            }

            for(int i = 0; i< listMyEnemyobj.Count; i++)//적이 죽었다면 리스트 크기를 줄이기 위한 For문
            {
                if(listMyEnemyobj[i] == null)// 적이 죽었다면 리스트에서 바로 삭제
                {
                    listMyEnemyobj.RemoveAt(i);
                }
            }     
        }
    
        if (this.healthPoint <= 0)//healthPoint가 다 닳면 죽기
        {
            nowState = State.die;

            if(myMonsterName == "StelladeLucy") // 스텔라루시가 죽었으면 위에 쌓여있는 별 없애기
            {
                for(int i = 0; i< listStellaStarobj.Count;i++)
                {
                    Destroy(listStellaStarobj[i]);
                }

                listStellaStarobj.RemoveRange(0, listStellaStarobj.Count);
            }
        }

        if(objMyTaget == null && listMyEnemyobj.Count == 0 && gameManagerScript.isStartedBattle == true)//내 타겟이 죽고 타겟이 잡히지 않는다면 공격을 다시 0 해줘야함
        {
            attackSpeedCount = 0;

            if (nowState != State.die)
            nowState = State.Idle;      
        }

        if (myMonsterName == "Laemon" && objMyTaget != null)//레이몬 스킬 쿨 타임 돌게하기
        {
            skillCoolTimeCount += Time.deltaTime;           

            if(skillCoolTime <= skillCoolTimeCount)
            {
                skillCoolTimeCount = 0;
                //스킬발동애니메이터 등등
                nowState = State.Skill;       
            }
        }

        if (myMonsterName == "Shine" && objMyTaget != null)//샤인의 스킬 쿨 타임 돌게하기
        {
            skillCoolTimeCount += Time.deltaTime;

            if (skillCoolTime <= skillCoolTimeCount)
            {
                skillCoolTimeCount = 0;
                nowState = State.Skill;
            }
        }

        if (myMonsterName == "Lio" && objMyTaget != null)//리오의 스킬 쿨타임 돌게하기
        {
            skillCoolTimeCount += Time.deltaTime;

            if (skillCoolTime <= skillCoolTimeCount)
            {
                skillCoolTimeCount = 0;
                nowState = State.Skill;

            }

        }

        if (isKnockBackSkillActive == true)//코코한테 밀렸거나 오스틴이 공격중이라면 
        {

            if (this.transform.position.x > 0.5f && this.transform.position.x < 4.5f)//맵 밖으로 나가지 않게 하기 위한 if문 맵의 끝 수치임
            {
                this.transform.Translate(Vector2.right * knocBackMoved * Time.deltaTime);
            }

            knocBackMoved -= 10 * Time.deltaTime;

            if (knocBackMoved < 0)//넉백 힘이 남아있다면
            {
                if (myMonsterName == "OSteen")
                {
                    knocBackMoved = 3;
                }
                else
                {
                    knocBackMoved = 4;
                }
                isKnockBackSkillActive = false;
            }
        }

        if(this.myMonsterName =="Loe")//로에가 2대 치면 스킬발동을 위한 if문 
        {
            if (nowState != State.Attack && listMyEnemyobj.Count > 0)//공격중이 아니라면 스킬카운트가 돌아야함 
            {
                skillCoolTime += Time.deltaTime;
                
                if (skillCoolTime > attackSpeed)
                {
                    atkCount +=1;
                    skillCoolTime = 0;

                }
            }

            if (atkCount >= 2)//2회 공격이 되었다면 로에 스킬을 발동하세요
            {

                if (nowState != State.Attack)
                {
                    nowState = State.Skill;
                }

                if (gameManagerScript.isStartedBattle == true)
                {
                    IsLoeHealedTeam();
                }
            }

        }

        if (stunCoolTime > 0)//스턴시간이 있다면 강제로 Idle상태
        {
            stunCoolTime -= Time.deltaTime;

            if (nowState != State.die)
            {
                nowState = State.Idle;
            }
            if(stunCoolTime <=0)
            {
                stunCoolTime = 0;
            }

        }

        if(lioGasCount > 0)//리오의 가스에 맞은상태 
        {
            lioGasCount -= Time.deltaTime;

            if (lioGasCount <= 0)//가스가 끝나면
            {
                lioGasCount = 0;
                moveSpeed = gameManagerScript.listCsvMoveSpeed[myLevel - 1 + (int)stateMonsterName];
                attackSpeed = gameManagerScript.listCsvAttackSpeed[myLevel - 1 + (int)stateMonsterName];
               isInLioGas = false;
            }

        }

        scrbarHealthPoint.size = (float)(pasteHealthPoint / maxHealthPoint);//healthPoint게이지 수치

        if (this.CompareTag("EnemyTeam"))//Hp가 몬스터 Canvas에 달려있어서 몬스터가 목표물 위치에 따라 보는 방향이 달라져 Scrollbar의 방향 역시 달라져서 최종적으로 hp바가 오른쪽에서 왼쪽으로 다는게 보이게 하기위한 if문
        { 

            if (this.transform.rotation.y == -1) // 오른쪽을 보고 있다면
            {
                
                scrbarHealthPoint.direction = Scrollbar.Direction.RightToLeft;
            }

            else// 왼쪽을 보고 있다면
            {
                scrbarHealthPoint.direction = Scrollbar.Direction.LeftToRight;
            }
        }

        else
        {
            if(this.transform.rotation.y == -1) //오른쪽을 보고 있다면
            {
                scrbarHealthPoint.direction = Scrollbar.Direction.LeftToRight;
            }

            else//왼쪽을 보고 있다면
            {
                scrbarHealthPoint.direction = Scrollbar.Direction.RightToLeft;
            }

        }

       if (stateMonsterName == MonsterName.C_Skl_03 && shieldDefense > 0)//호머가 방어막 수치가 남아있다면 간단한 Image 켜주기
       {
            objShieldEffect.gameObject.SetActive(true);
       } // 방어력 있을때 임시 이펙트
       else if(stateMonsterName == MonsterName.C_Skl_03 && shieldDefense <= 0)//호머의 방어막 수치가 없다면 Image 꺼주기
       {
            objShieldEffect.gameObject.SetActive(false);
       }//방어력 없을 때 꺼주기

        //DamageTextMake();

        if (myMonsterName == "StelladeLucy"&& listMyEnemyobj.Count > 0)//스텔라의 스킬 쿨타임 돌게 하기 위함
        {
            skillCoolTimeCount += Time.deltaTime;

            for (int i = 0; i < listStellaStarobj.Count; i++)//스텔라가 별을 터트렸다면 없애주세요
            {
                if (listStellaStarobj[i] == null)
                {
                    listStellaStarobj.RemoveAt(i); 
                }
            }

            if (skillCoolTimeCount > skillCoolTime)//스킬 쿨타임이 다되었다면
            {
                skillCoolTimeCount = 0;

                nowState = State.Skill;

            }
        }

    }

    public void TargetLongAttackerCaught()//원거리 우선 타겟 잡아주기 
    {
        List<MonsterPropertieScript> listLongAttacker = new List<MonsterPropertieScript>();//원거리 적들 넣어주기

        for (int i = 0; i < listMyEnemyobj.Count; i++)//목표물을 잡아주게 하기위한 For문
        {
            if (listMyEnemyobj[i] == null) // Index 터지는거 방지
            {
                listMyEnemyobj.RemoveAt(i);

            }
           
            else//원거리 적 우선 잡기
            {
                if (listMyEnemyobj[i].GetComponent<MonsterPropertieScript>().myAttackType == AttackerType.longAttacker ||
                    listMyEnemyobj[i].GetComponent<MonsterPropertieScript>().myAttackType == AttackerType.OnlySkillAttacker)
                {
                    listLongAttacker.Add(listMyEnemyobj[i].GetComponent<MonsterPropertieScript>());

                }
            }

        }

        if (listLongAttacker.Count > 0)//원거리 적이 살아 있다면
        {
            if (objMyTaget.GetComponent<MonsterPropertieScript>().myAttackType == AttackerType.ShotAttacker)//원거리 살아있는 적들 리스트에 넣어주기
            {
                objMyTaget = listLongAttacker[Random.Range(0, listLongAttacker.Count)].gameObject;
            }

            targetDistanceXY = Vector2.Distance(this.transform.position, objMyTaget.transform.position);

            for (int i = 0; i < listLongAttacker.Count; i++)//거리 비교해서 목표물 넣어주기 위한 for문
            {

                listMyEnemyPosDistance[i] = Vector2.Distance(this.transform.position, listLongAttacker[i].transform.position);

                if (listMyEnemyPosDistance[i] < targetDistanceXY)
                {
                    objMyTaget = listLongAttacker[i].gameObject;

                }

            }

        }

        else//원거리 적이 없다면 가까운애 때리기
        {
            for (int i = 0; i < listMyEnemyobj.Count; i++)//거리를 비교해 list에 넣어주기
            {
                if (listMyEnemyobj[i] == null)
                {
                    listMyEnemyobj.RemoveAt(i);
                }
                else
                {
                    listMyEnemyPosDistance[i] = Vector2.Distance(this.transform.position, listMyEnemyobj[i].transform.position);
                }
            }

            targetDistanceXY = Vector2.Distance(this.transform.position, objMyTaget.transform.position);

            for (int i = 0; i < listMyEnemyobj.Count; i++)
            {
                if (listMyEnemyPosDistance[i] < targetDistanceXY)
                {
                    objMyTaget = listMyEnemyobj[i];

                }
            }
        }

        if (nowState != State.die)
        {
            if (objMyTaget.transform.position.x < this.transform.position.x)
            {
                this.transform.localRotation = Quaternion.Euler(this.transform.rotation.x, 0, 0);
            }

            else if (objMyTaget.transform.position.x > this.transform.position.x)
            {
                this.transform.localRotation = Quaternion.Euler(this.transform.rotation.x, -180, 0);
            }

            if (targetDistanceXY > attackDistance)//사정거리 바깥에 있다면 움직이기
            {

                if (stunCoolTime <= 0)//스턴이 아닐때만 움직이기
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, objMyTaget.transform.position, moveSpeed * Time.deltaTime);

                    nowState = State.Walk;
                }
            }

            if (attackDistance >= targetDistanceXY)//사정거리 안에 있다면 멈추고 공격모션을 취하기
            {

                attackSpeedCount += Time.deltaTime;

                if (attackSpeedCount > attackSpeed)//공격속도가 다 되었으면 때리기
                {
                    nowState = State.Attack;
                    attackSpeedCount = 0;
                }

            }
        }

    }

    public void TargetCaught()//기본 타겟 잡아주기
    {
       
        for (int i = 0; i < listMyEnemyobj.Count; i++)//적들 전부 비교군 리스트에 넣어주기
        {
            if (listMyEnemyobj[i] == null)
            {
                listMyEnemyobj.RemoveAt(i);
            }
            else
            {
                listMyEnemyPosDistance[i] = Vector2.Distance(this.transform.position, listMyEnemyobj[i].transform.position);
            }
        }

        targetDistanceXY = Vector2.Distance(this.transform.position, objMyTaget.transform.position);

        for (int i = 0; i < listMyEnemyobj.Count; i++)
        {
            
            if(myMonsterName =="Coco")//코코는 거리 상관없이 공격력 쌘 애 한테 갑니다.
            {
                if (objMyTaget != null)// 타겟이 갑자기 사라지면 나가세요 
                {
                    break;
                }

                else if(objMyTaget.GetComponent<MonsterPropertieScript>().attackPower < listMyEnemyobj[i].GetComponent<MonsterPropertieScript>().attackPower) //공격력 쌘 에 잡아주기
                {

                    objMyTaget = listMyEnemyobj[i];
                }            

            }

            else if(myMonsterName == "Gus" && listMyEnemyPosDistance[i] > targetDistanceXY)//거스의 타겟은 제일 먼 유닛부터 
            {

                objMyTaget = listMyEnemyobj[i];
            }


            else if (listMyEnemyPosDistance[i] < targetDistanceXY && myMonsterName != "Gus")
            {
                if (isPinearsAttackedFirst == false)//파인아스를 때린 친구가 없다면
                {
                    objMyTaget = listMyEnemyobj[i];
                }
            }
        }

        if (nowState != State.die)//죽지 않았다면
        {
            if (objMyTaget.transform.position.x < this.transform.position.x)//목표물의 위치를 고려하여 고개 돌리기 
            {
                this.transform.localRotation = Quaternion.Euler(this.transform.rotation.x, 0, 0);
            }

            else if (objMyTaget.transform.position.x > this.transform.position.x)//목표물의 위치를 고려하여 고개 돌리기
            {
                this.transform.localRotation = Quaternion.Euler(this.transform.rotation.x, -180, 0);
            }


            if (targetDistanceXY > attackDistance)//사정거리 바깥에 있다면 움직이기
            {
                if (isKnockBackSkillActive == false) // 넉백중이 아닐때
                {
                    if (stunCoolTime <= 0)//스턴이 아닐 때
                    {
                        this.transform.position = Vector3.MoveTowards(this.transform.position, objMyTaget.transform.position, moveSpeed * Time.deltaTime);
                    }
                }

                if (nowState != State.Skill)
                {
                    nowState = State.Walk;
                   
                }

                if (myMonsterName == "Durigon") // 두리곤의 이동속도 증가 버프
                {

                    MoveDurigonCounted();
                }

            }

            if (attackDistance >= targetDistanceXY)//사정거리 안에 있다면 멈추고 공격모션을 취하기
            {
                if (nowState != State.Skill && nowState != State.die && myMonsterName!="Loe")//로에가 공격하지 않아도 스킬을 발동하기 위한 if문
                {

                    attackSpeedCount += Time.deltaTime;

                    if (attackSpeedCount > attackSpeed)
                    {
                        nowState = State.Attack;
                        attackSpeedCount = 0;
                    }

                }

                else if(nowState != State.die && myMonsterName == "Loe")//로에가 공격했다면 스킬 쿨타임 초기화
                {
                    nowState = State.Attack;
                    attackSpeedCount = 0;
                    skillCoolTime = 0;
                }
            }
        }

        if (isGrapSkillActive == true && objGrapMonster != null)//그랩스킬을 사용했다면
        {
            Vector3 grapLeftpos = new Vector3();// = new Vector3(this.transform.position.x -1f, this.transform.position.y,this.transform.position.z);

            if(this.transform.rotation.y == 0) //왼쪽을 보고있다면 왼쪽으로 끌기
            {
                grapLeftpos = new Vector3(this.transform.position.x - 1f, this.transform.position.y, this.transform.position.z);

            }

            else //오른쪽을 보고 있다면 오른쪽으로 끌기
            {
                grapLeftpos = new Vector3(this.transform.position.x + 1f, this.transform.position.y, this.transform.position.z);
            }


            objGrapMonster.GetComponent<CircleCollider2D>().enabled = false; //그랩일땐 모든 유닛 무시

            objGrapMonster.transform.position = Vector3.MoveTowards(objGrapMonster.transform.position, grapLeftpos, 3f *Time.deltaTime);

            if(objGrapMonster.transform.position == grapLeftpos) //위치에 왔다면 초기화
            {
                isGrapSkillActive = false;
                objGrapMonster.GetComponent<CircleCollider2D>().enabled = true;//그랩 풀림
                objGrapMonster = null;
                
            }
        }
    }

   

    public void AttackEnemy()//목표 공격하기
    {
     
        if (myAttackType == AttackerType.ShotAttacker) // 근거리 공격이라면
        {
            if (objMyTaget != null && myMonsterName == "ChillyBomb")//페퍼팡의 공격
            {
                IsChillyBombAttackedMonster();
            }

            else if (objMyTaget != null && myMonsterName == "Brocc")//브로콜리의 공격
            {
                IsBrocoliAttackedMonster();
            }

            else if (objMyTaget != null && myMonsterName == "Vocad")//보카드의 공격
            {
                IsVocadAttackedMonster();
            }

            else if (objMyTaget != null && myMonsterName == "Durigon")//두리곤의 공격
            {
                IsDurigonAttackedMonster();
            }

            else if (objMyTaget != null && myMonsterName != "Brocc") //일반유닛의 일반공격
            {             
                MonsterPropertieScript CacheTarget;//적 넣어주기
                CacheTarget = objMyTaget.GetComponent<MonsterPropertieScript>();

                GameObject objDamageTmp;//텍스트가 나올 캔버스

                if(CacheTarget == null)
                {
                    return;
                }

                objDamageTmp = Instantiate(objDamageTmpcanv, CacheTarget.objMyDamagecanv.transform.position, Quaternion.identity,
                CacheTarget.objMyDamagecanv.transform);

                TextMeshProUGUI tempTarget;//데미지 텍스트 소환해주기 위한 변수
                tempTarget = objDamageTmp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();//.text = "" + (pastehealthPoint - healthPoint);

                float atkRandomCritical;//크리티컬 확률
                atkRandomCritical = Random.Range(0, 100);

                float evasionRandom;//회피확률
                evasionRandom = Random.Range(1, 100);

                int finalDamage; //공격 계산식 다 끝난 변수

                if (CacheTarget.evasion >= evasionRandom)
                {

                    tempTarget.text = "evasion";
                }

                else
                {
                    
                    if (atkRandomCritical > criticalPercent)//크리티컬 안터졌으면
                    {
                         finalDamage = (int)((attackPower * 100 / (100 + CacheTarget.defense)) + ((attackPower * 100 / (100 + CacheTarget.defense))
                            * Random.Range(-0.05f, 0.06f)));
                    }

                    else // 크리티컬 터졌으면
                    {
                         finalDamage = (int)((attackPower * 100 / (100 + CacheTarget.defense)) + ((attackPower * 100 / (100 + CacheTarget.defense))
                         * Random.Range(-0.05f, 0.06f) + attackPower * criticalPower));
                        tempTarget.color = new Color(255, 255, 0);
                    }

                    if (CacheTarget.shieldDefense <= 0)
                    {
                        CacheTarget.healthPoint -= finalDamage;
                    }

                    if (CacheTarget.shieldDefense > 0)
                    {
                         CacheTarget.shieldDefense -= finalDamage;

                        if (CacheTarget.shieldDefense < 0)
                        {
                             CacheTarget.healthPoint += CacheTarget.shieldDefense;
                             CacheTarget.shieldDefense = 0;

                        }
                            //CashingMyTarget.PasteShieldDef = CashingMyTarget.ShieldDef;
                    }
                     
                    if(CacheTarget.myMonsterName == "Watteron")//워터론의 대미지 반사
                    {
                        if (this.shieldDefense <= 0)
                        {
                            this.healthPoint -= CacheTarget.attackPower * CacheTarget.skillPower1;

                        }

                        if (this.shieldDefense > 0)
                        {
                            this.shieldDefense -= CacheTarget.attackPower * CacheTarget.skillPower1;

                            if (this.shieldDefense < 0)
                            {
                                this.healthPoint += this.shieldDefense;
                                this.shieldDefense = 0;

                            }

                            this.pasteHealthPoint = healthPoint;
                           
                        }

                    }

                    if( CacheTarget.myMonsterName =="PineArs")//파인아스의 공격자 찾기
                    {
                        if(CacheTarget.isPinearsAttackedFirst == false)
                        {
                            CacheTarget.objMyTaget = this.gameObject;
                            CacheTarget.isPinearsAttackedFirst = true;
                        }

                    }
                        CacheTarget.pasteHealthPoint = CacheTarget.healthPoint;                     
                        tempTarget.text = "" + finalDamage;
   
                }
               
                if (this.myMonsterName == "Homer")//호머의 공격 카운트 세기
                {
                    atkCount += 1;
                    
                }

                else if (this.myMonsterName == "Coco")//코코의 밀기
                {

                    CacheTarget.isKnockBackSkillActive = true;
                    CacheTarget.knocBackMoved = 4;

                }

                else if (this.myMonsterName == "Loe")//로에의 공격카운트 세기
                {
                    if (skillCoolTime > 0 && atkCount > 0)//공격하면 스킬카운트 초기화 
                    {
                        skillCoolTime = 0;
                        atkCount = 0;

                    }

                    atkCount += 1;

                }
                else if (this.myMonsterName == "Cacter")//2대 때리면 스턴1초
                {

                    atkCount += 1;

                    if (atkCount == 2)
                    {
                        atkCount = 0;
                        CacheTarget.stunCoolTime = skillPower1;

                    }

                }

                if (atkCount >= 3 && this.myMonsterName == "Homer")//만약 호머가 3대 때렸다면 방어막 생성
                {
                    atkCount = 0;
                    shieldDefense = pasteShieldDefense;
                }

             
                Destroy(objDamageTmp, 2);
            }
           
        }

        else if (myAttackType == AttackerType.longAttacker)//원거리 공격이라면
        {

            if (listMyBulletPoolScript.Count == 0)//총알이 하나도 없다면 무조건 하나를 즉시 생성하세요
            {
                listMyBulletPoolScript.Add(Instantiate(objMyFirstBullet, this.transform.position, Quaternion.identity,gameManagerScript.transform).GetComponent<BulletScript>());

                if(myMonsterName == "RobinCorn")//로빈콘이라면 총알을 하나 더 생성하세요
                {
                   // Vector3 SecondBull = new Vector3(this.transform.position.x + 0.1f, this.transform.position.y, this.transform.position.z); 

                    listMyBulletPoolScript.Add(Instantiate(objMyFirstBullet, this.transform.position, Quaternion.identity, gameManagerScript.transform).GetComponent<BulletScript>());
                }
            }

            else if(listMyBulletPoolScript.Count > 0)//총알이 하나라도 있다면
            {
                int CountBullet = -1;

                for(int i = 0; i < listMyBulletPoolScript.Count; i++)
                {
                    if(listMyBulletPoolScript[i].transform.position != this.transform.position)
                    {
                        CountBullet += 1;

                    }

                }

                if (CountBullet == listMyBulletPoolScript.Count) // 총알이 모두 나가있다면 총알을 하나 생성하세요 
                { 
                    listMyBulletPoolScript.Add(Instantiate(objMyFirstBullet, this.transform.position, Quaternion.identity).GetComponent<BulletScript>());
                }

            }

            for(int i = 0; i< listMyBulletPoolScript.Count; i++)//총알에게 정보 넘겨주기
            {
                if (this.transform.position == listMyBulletPoolScript[i].transform.position)//총알이 탄창에 있다면
                {
                    if (objMyTaget != null)//총알에게 정보값 넣어주기
                    {

                        listMyBulletPoolScript[i].myTargetScript = objMyTaget.GetComponent<MonsterPropertieScript>();
                        listMyBulletPoolScript[i].objMyParent = this.gameObject;
                        listMyBulletPoolScript[i].myDamage = attackPower;
                        listMyBulletPoolScript[i].isFinishedAttackMotion = true;
                        listMyBulletPoolScript[i].targetPos = objMyTaget.transform.position;
                        listMyBulletPoolScript[i].criticalPercent = this.criticalPercent;
                        listMyBulletPoolScript[i].criticalPower = this.criticalPower;
                        listMyBulletPoolScript[i].myParentScript = this;
                        listMyBulletPoolScript[i].transform.position = new Vector2(this.transform.position.x + i * 0.3f, this.transform.position.y + i *0.1f);
                        listMyBulletPoolScript[i].moveSpeed = 4;
                        
                    }
                }

            }

            if(myMonsterName == "Laemon")//내가 레이몬이라면
            {

                if(listMyFloorSkillPoolScript.Count==0)//바닥이 없으면 하나 생성한다
                {
                    listMyFloorSkillPoolScript.Add(this.gameObject.transform.GetChild(1).GetComponent<FloorSkill>());
                }

                for (int i = 0; i < listMyFloorSkillPoolScript.Count; i++)//바닥에게 정보 넘겨주기
                {
                    if (this.transform.position == listMyFloorSkillPoolScript[i].transform.position)//바닥스킬이 준비 되었다면
                    {
                        if (objMyTaget != null && nowState == State.Skill)//스킬에게 정보값 넣어주기
                        {

                            listMyFloorSkillPoolScript[i].myTargetScript = objMyTaget.GetComponent<MonsterPropertieScript>();
                            listMyFloorSkillPoolScript[i].objMyParent = this.gameObject;
                            listMyFloorSkillPoolScript[i].myDamage = attackPower;
                            listMyFloorSkillPoolScript[i].isFinishedAttackMotion = true;
                            listMyFloorSkillPoolScript[i].myTargetPos = objMyTaget.transform.position;
                            listMyFloorSkillPoolScript[i].skillTimer = this.skillDuration;
                            listMyFloorSkillPoolScript[i].myRadius = this.skillRadius;
                            listMyFloorSkillPoolScript[i].criticalPercent = this.criticalPercent;
                            listMyFloorSkillPoolScript[i].criticalPower = this.skillRadius;
                            listMyFloorSkillPoolScript[i].skillPower = this.skillPower1;
                            listMyFloorSkillPoolScript[i].transform.parent = gameManagerScript.transform;

                        }

                    }

                }

               // MyFloorSkillPool.RemoveAt(0);
            }

            if (myMonsterName == "Shine" && nowState == State.Skill)//내가 샤인이고 스킬발동 타이밍 이라면
            {

                if (objGrapMonster == null)//그랩몬스터가 없다면 아무나 넣어주기
                {
                    objGrapMonster = listMyEnemyobj[Random.Range(0, listMyEnemyobj.Count)];
                }
              

                for(int i = 0; i < listMyEnemyobj.Count; i++)//거리 비교해서 그랩대상 넣어주기
                {
                    float GrapDistance = Vector2.Distance(objGrapMonster.transform.position, this.transform.position);

                    if (listMyEnemyPosDistance[i] > GrapDistance)
                    {
                        objGrapMonster = listMyEnemyobj[i];
                    }

                }
                isGrapSkillActive = true;

            }

            if(myMonsterName == "ChecheRiry")//내가 체체 리리라면
            {
                atkCount += 1;//때리면 hitCount가 하나식 증가합니다.

                if(atkCount >= 3)//3대를 때렸다면
                {
                    Vector2[] Dot = new Vector2[4];//체체 리리의 사각형 범위 담아줄 변수

                    if (objMyTaget != null) // 체체 리리 3타 범위 잡아주기
                    {

                        float angleRotational = Mathf.Atan2(objMyTaget.transform.position.y - this.transform.position.y, objMyTaget.transform.position.x - this.transform.position.x) * 180 / Mathf.PI;

                        Vector2 v3Source = this.transform.position;   // 중심이 되는 오브젝트        
                        Vector2 v3Distance = Vector2.up * 0.5f;    // 체리의 양옆 0.5씩 
                        Vector2 v3Distance2 = Vector2.down * 0.5f;//체리 양옆 0.5씩
                        Vector2 v3Distance3 = Vector2.right * 5.5f;//뻗어나가는 사거리 5.5
                        Quaternion qRotate = Quaternion.Euler(0f, 0, angleRotational);  // 회전할 각도(Y축 기준 회전. 필요에따라 수정 필요)

                        Vector2 v3TargetPoint = qRotate * v3Distance;  // 원점을 기준으로 거리와 각도를 연산한 후, 벡터
                        Vector2 v3TargetPoint2 = qRotate * v3Distance2;// 원점을 기준으로 거리와 각도를 연산한 후, 벡터
                        Vector2 v3TargetPoint3 = qRotate * v3Distance3;//반대편 지점 그려주기

                        Vector2 v3Dest = new Vector2();
                        Vector2 v3Dest2 = new Vector2();
                        Vector2 v3Dest3 = new Vector2();
                        Vector2 v3Dest4 = new Vector2();
                        Vector2 v3Dest5 = new Vector2();

                        v3Dest = v3Source + v3TargetPoint;    // 중심이 되는 오브젝트에서 해당 거리와 각도만큼 이동한 곳의 좌표
                        v3Dest2 = v3Source + v3TargetPoint2;
                        v3Dest3 = v3Source + v3TargetPoint3;

                        //Instantiate(tt, v3Dest, qRotate);
                        //Instantiate(tt, v3Dest2, qRotate);
                        //Instantiate(tt, v3Dest3, qRotate);

                        Vector2 v3Source2 = v3Dest3;
                        float angleRotational2 = Mathf.Atan2(this.transform.position.y - v3Dest3.y, this.transform.position.x - v3Dest3.x) * 180 / Mathf.PI;//아크탄젠트를 이용하여 항상 바라보게 해주기

                        Quaternion BRotate = Quaternion.Euler(0f, 0, angleRotational2);

                        Vector2 v3TargetPoint4 = BRotate * v3Distance;
                        Vector2 v3TargetPoint5 = BRotate * v3Distance2;

                        v3Dest4 = v3Source2 + v3TargetPoint4;
                        v3Dest5 = v3Source2 + v3TargetPoint5;

                        //Instantiate(tt, v3Dest4, BRotate);
                        //Instantiate(tt, v3Dest5, BRotate);

                        Dot[0] = v3Dest;//시계방향 or 반시계방향으로 넣어주기 대각선으로 넣어버리면 작동안됨
                        Dot[1] = v3Dest5;
                        Dot[2] = v3Dest4;
                        Dot[3] = v3Dest2;

                    }

                    for (int i = 0; i < listMyEnemyobj.Count; i++)//4개의 점을 구했다면 공격하기
                    {
                        CheckRangebyCheche(listMyEnemyobj[i], Dot);
                    }

                }
            }


        }

        else if (myAttackType == AttackerType.OnlySkillAttacker)//스킬로만 공격하는 타입이라면
        {
            if (listMyFloorSkillPoolScript.Count == 0 && myMonsterName != "Ragon")//장판이 하나도 없다면 무조건 하나를 즉시 생성하세요
            {
                listMyFloorSkillPoolScript.Add(Instantiate(objMyFirstBullet, this.transform.position, Quaternion.identity, gameManagerScript.transform).GetComponent<FloorSkill>());
            }

            else if(listMyFloorSkillPoolScript.Count == 0 && myMonsterName == "Ragon")//라곤이라면 하늘에서 생성합니다.
            {
                //Vector2 Meteor = new Vector2();
                listMyFloorSkillPoolScript.Add(Instantiate(objMyFirstBullet, new Vector2(2.5f,3.5f), Quaternion.identity, gameManagerScript.transform).GetComponent<FloorSkill>());

            }

            if(listMyFloorSkillPoolScript[listMyFloorSkillPoolScript.Count-1] == null)//index 터지기 방지
            {
                listMyFloorSkillPoolScript.RemoveAt(listMyFloorSkillPoolScript.Count - 1);
            }

            for (int i = 0; i < listMyFloorSkillPoolScript.Count; i++)//바닥에게 정보 넘겨주기
            {
                if (this.transform.position == listMyFloorSkillPoolScript[i].transform.position && myMonsterName !="Ragon" )//바닥이 준비되었다면
                {
                    if (objMyTaget != null)
                    {

                        listMyFloorSkillPoolScript[i].myTargetScript = objMyTaget.GetComponent<MonsterPropertieScript>();
                        listMyFloorSkillPoolScript[i].objMyParent = this.gameObject;
                        listMyFloorSkillPoolScript[i].myDamage = attackPower;
                        listMyFloorSkillPoolScript[i].isFinishedAttackMotion = true;
                        listMyFloorSkillPoolScript[i].myTargetPos = objMyTaget.transform.position;
                        listMyFloorSkillPoolScript[i].skillTimer = this.skillDuration;
                        listMyFloorSkillPoolScript[i].myRadius = this.skillRadius;
                        listMyFloorSkillPoolScript[i].criticalPercent = this.criticalPercent;
                        listMyFloorSkillPoolScript[i].criticalPower = this.skillRadius;
                        listMyFloorSkillPoolScript[i].skillPower = this.skillPower1;

                    }

                }

                else if ( myMonsterName == "Ragon")//라곤의 총알이 하늘위에 있다면
                {
                    if (objMyTaget != null)
                    {
                        listMyFloorSkillPoolScript[i].myTargetScript = objMyTaget.GetComponent<MonsterPropertieScript>();
                        listMyFloorSkillPoolScript[i].objMyParent = this.gameObject;
                        listMyFloorSkillPoolScript[i].myDamage = attackPower;
                        listMyFloorSkillPoolScript[i].isFinishedAttackMotion = true;
                        listMyFloorSkillPoolScript[i].myTargetPos = objMyTaget.transform.position;
                        listMyFloorSkillPoolScript[i].skillTimer = this.skillDuration;
                        listMyFloorSkillPoolScript[i].myRadius = this.skillRadius;
                        listMyFloorSkillPoolScript[i].criticalPercent = this.criticalPercent;
                        listMyFloorSkillPoolScript[i].criticalPower = this.skillRadius;
                        listMyFloorSkillPoolScript[i].skillPower = this.skillPower1;                   
                    }

                }

            }

        }

    }

    //public void DamageTextMake()
    //{
    //    GameObject Damagetext;


    //    Damagetext = Instantiate(DamageTextCanvas, DamageCanvas.transform.position, Quaternion.identity, DamageCanvas.transform);
    //    Damagetext.transform.GetChild(0).GetComponent<TextMeshealthPointroUGUI>().text = "" + (pastehealthPoint - healthPoint);

    //    Destroy(Damagetext, 2);

    //    pastehealthPoint = healthPoint;


    //}

    public void CheckRangebyCheche(GameObject _objtarget, Vector2[] _arrdot)//리리의 공격 범위 설정 및 공격
    {
        int crosses = 0;

        listMyBulletPoolScript[0].isFinishedAttackMotion = false;
        
        for (int i = 0; i < _arrdot.Length; i++) //점들의 갯수
        {
            int j = (i + 1) % _arrdot.Length;

            //점 B가 선분 (p[i], p[j])의 y좌표 사이에 있음
            if ((_arrdot[i].y > _objtarget.transform.position.y) != (_arrdot[j].y > _objtarget.transform.position.y))
            {
                //atX는 점 B를 지나는 수평선과 선분 (p[i], p[j])의 교점
                double atX = (_arrdot[j].x - _arrdot[i].x) * (_objtarget.transform.position.y - _arrdot[i].y) / (_arrdot[j].y - _arrdot[i].y) + _arrdot[i].x;
                //atX가 오른쪽 반직선과의 교점이 맞으면 교점의 개수를 증가시킨다.
                if (_objtarget.transform.position.x < atX)
                {
                    crosses++;

                }             
            }
        }

          if(crosses %2 >0) // 점 4개의 범위 안에 적이 있다면 
          {
                MonsterPropertieScript CacheTarget;//들어온 적 스크립트
                CacheTarget = _objtarget.GetComponent<MonsterPropertieScript>();

                GameObject objDamage;//데미지 텍스트 캔버스

                if (CacheTarget == null)
                {
                    return;
                }

                objDamage = Instantiate(objDamageTmpcanv, CacheTarget.objMyDamagecanv.transform.position, Quaternion.identity,
                CacheTarget.objMyDamagecanv.transform);


                TextMeshProUGUI tmpTarget;//데미지 텍스트
                tmpTarget = objDamage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();//.text = "" + (pastehealthPoint - healthPoint);

                float atkRandomCritical;//크리티컬확률
                atkRandomCritical = Random.Range(0, 100);

                float evasionRandom;//회피확률
                evasionRandom = Random.Range(1, 100);

                int finalDamage;

                if (CacheTarget.evasion >= evasionRandom)//회피했다면
                {

                    tmpTarget.text = "evasion";
                }

                else//회피하지 못하였다면
                {

                    if (atkRandomCritical > criticalPercent)//크리티컬 안터졌으면
                    {
                        finalDamage = (int)((attackPower * 100 / (100 + CacheTarget.defense)) + ((attackPower * 100 / (100 + CacheTarget.defense))
                           * Random.Range(-0.05f, 0.06f)));
                    }

                    else // 크리티컬 터졌으면
                    {

                        finalDamage = (int)((attackPower * 100 / (100 + CacheTarget.defense)) + ((attackPower * 100 / (100 + CacheTarget.defense))
                        * Random.Range(-0.05f, 0.06f) + attackPower * criticalPower));
                        tmpTarget.color = new Color(255, 255, 0);
                    }

                    if (CacheTarget.shieldDefense <= 0)//방어막이 없다면
                    {
                        CacheTarget.healthPoint -= finalDamage;

                    }

                    if (CacheTarget.shieldDefense > 0)//방어막이 있다면
                    {
                        CacheTarget.shieldDefense -= finalDamage;

                        if (CacheTarget.shieldDefense < 0)
                        {
                            CacheTarget.healthPoint += CacheTarget.shieldDefense;
                            CacheTarget.shieldDefense = 0;

                        }

                        //CashingMyTarget.PasteShieldDef = CashingMyTarget.ShieldDef;
                    }


                    if (CacheTarget.myMonsterName == "Watteron")//워터론을 때렸을때 대미지 반사
                    {
                        if (this.shieldDefense <= 0)
                        {
                            this.healthPoint -= CacheTarget.attackPower * CacheTarget.skillPower1;

                        }

                        if (this.shieldDefense > 0)
                        {
                            this.shieldDefense -= CacheTarget.attackPower * CacheTarget.skillPower1;

                            if (this.shieldDefense < 0)
                            {
                                this.healthPoint += this.shieldDefense;
                                this.shieldDefense = 0;

                            }

                            this.pasteHealthPoint = healthPoint;
                            //CashingMyTarget.PasteShieldDef = CashingMyTarget.ShieldDef;
                        }
                        this.pasteHealthPoint = this.healthPoint;
                      }


                    if (CacheTarget.myMonsterName == "PineArs")//때린상대가 파인아스라면 파인아스의 타겟이됨
                    {
                        if (CacheTarget.isPinearsAttackedFirst == false)
                        {
                            CacheTarget.objMyTaget = this.gameObject;
                            CacheTarget.isPinearsAttackedFirst = true;
                        }

                    }


                    CacheTarget.pasteHealthPoint = CacheTarget.healthPoint;
                    tmpTarget.text = "" + finalDamage;
                    Destroy(objDamage, 2);
                }

            
         
          }

              
          
       
        atkCount = 0;//공격카운트 초기화

    } // 

    public void MoveDurigonCounted()//두리곤 이동비례한 공격력 버프
    {
        if (moveCount < 3)
        {
            moveCount += Time.deltaTime;

        }
       
    }


    public void FindMyState(string _myName)//CSV파일의 능력치 담아주기
    {

        if (_myName == "Momo")//모모라면
        {
            stateMonsterName = MonsterName.B_Arch_01;
            myAttackType = AttackerType.longAttacker;
            MyScore = 1;
        }

        else if (_myName == "Carode")//캐럿이라면
        {
            stateMonsterName = MonsterName.C_Wrr_01;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 1;
        }

        else if (_myName == "Mato")//마토라면
        {
            stateMonsterName = MonsterName.C_Arch_02;
            myAttackType = AttackerType.longAttacker;
            MyScore = 1;

        }

        else if (_myName == "Dubeen")//듀콩이라면
        {
            stateMonsterName = MonsterName.C_Wrr_02;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 1;
        }

        else if (_myName == "ChefTeto")//쉐프테토라면
        {
            stateMonsterName = MonsterName.C_Tnk_01;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 1;

        }

        else if (_myName == "Gurulic")//갈릭이라면
        {
            stateMonsterName = MonsterName.C_Skl_02;
            myAttackType = AttackerType.OnlySkillAttacker;
            healthPoint = gameManagerScript.listCsvHealthPoint[myLevel - 1 + (int)stateMonsterName];
            MyScore = 1;
        }

        else if (_myName == "BruceApp")//애뵤라면
        {
            stateMonsterName = MonsterName.B_Wrr_01;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 2;

        }

        else if (_myName == "Poldier")//호저라면
        {
            stateMonsterName = MonsterName.B_Tnk_01;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 2;

        }

        else if (_myName == "Homer")//호머라면
        {
            stateMonsterName = MonsterName.C_Skl_03;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 1;

        }

        else if (_myName == "Crape")//크레이프 라면
        {
            stateMonsterName = MonsterName.B_Arch_02;
            myAttackType = AttackerType.longAttacker;
            MyScore = 2;
        }

        else if (_myName == "Pome")//포메라면
        {
            stateMonsterName = MonsterName.B_Tnk_02;
            myAttackType = AttackerType.ShotAttacker;
            healthPoint = gameManagerScript.listCsvHealthPoint[myLevel - 1 + (int)stateMonsterName];
            attackPower = gameManagerScript.listCsvAttackPower[myLevel - 1 + (int)stateMonsterName];
            attackSpeed = gameManagerScript.listCsvAttackSpeed[myLevel - 1 + (int)stateMonsterName];
            moveSpeed = gameManagerScript.listCsvMoveSpeed[myLevel - 1 + (int)stateMonsterName];
            attackDistance = gameManagerScript.listCsvAttackRange[myLevel - 1 + (int)stateMonsterName];
            defense = gameManagerScript.listCsvDefend[myLevel - 1 + (int)stateMonsterName];
            criticalPercent = gameManagerScript.listCsvCriticalPercent[myLevel - 1 + (int)stateMonsterName];
            criticalPower = gameManagerScript.listCsvCriticalPower[myLevel - 1 + (int)stateMonsterName];
            skillPower1 = gameManagerScript.listCsvSkillPower1[myLevel - 1 + (int)stateMonsterName];
            skillPower2 = gameManagerScript.listCsvSkillPower2[myLevel - 1 + (int)stateMonsterName];
            //ShieldDef = healthPoint * skillPower1;
            //PasteShieldDef = ShieldDef;
            pasteHealthPoint = healthPoint;
            maxHealthPoint = healthPoint;
            MyScore = 2;

            if (this.gameObject.name == "ShotPome(Clone)")//꼬마 포메라면 
            {
                this.attackPower = attackPower * 0.33f;
                this.healthPoint = healthPoint * 0.3f;


            }

        }

        else if (_myName == "Brocc")//브로콜리라면
        {
            stateMonsterName = MonsterName.B_Wrr_02;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 2;

        }

        else if (_myName == "Gus")//거스라면
        {
            stateMonsterName = MonsterName.A_Arch_01;
            myAttackType = AttackerType.longAttacker;
            MyScore = 3;
        }

        else if (_myName == "Nach")//나치라면
        {
            stateMonsterName = MonsterName.C_Wrr_02;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 1;

        }

        else if (_myName == "RobinCorn")//로빈콘이라면
        {
            stateMonsterName = MonsterName.B_Arch_01;
            myAttackType = AttackerType.longAttacker;
            MyScore = 1;

        }

        else if (_myName == "ChillyBomb")//페퍼팡이라면
        {
            stateMonsterName = MonsterName.B_Skl_01;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 2;

        }

        else if (_myName == "Laemon")//레이몬이라면
        {
            stateMonsterName = MonsterName.B_Skl_02;
            myAttackType = AttackerType.longAttacker;
            MyScore = 2;

        }

        else if (_myName == "Shine")//샤인이라면
        {
            stateMonsterName = MonsterName.B_Skl_03;
            myAttackType = AttackerType.longAttacker;
            MyScore = 2;
        }

        else if (_myName == "Vocad")//보카드라면
        {
            stateMonsterName = MonsterName.A_Wrr_01;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 3;
        }
        else if (_myName == "Durigon")//두리곤
        {
            stateMonsterName = MonsterName.A_Wrr_02;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 3;
        }

        else if (_myName == "OSteen")//오스틴
        {
            stateMonsterName = MonsterName.A_Arch_02;
            myAttackType = AttackerType.longAttacker;
            knocBackMoved = 3;
            MyScore = 3;

        }

        else if (_myName == "Watteron")//워터론
        {
            stateMonsterName = MonsterName.A_Tnk_01;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 3;

        }

        else if (_myName == "Coco")//코코
        {
            stateMonsterName = MonsterName.A_Tnk_02;
            myAttackType = AttackerType.ShotAttacker;
            knocBackMoved = 4;
            MyScore = 3;
        }

        else if (_myName == "Loe")//로에
        {
            stateMonsterName = MonsterName.A_Skl_01;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 3;

        }
        else if (_myName == "Cacter")//캐터
        {
            stateMonsterName = MonsterName.A_Skl_02;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 3;

        }

        else if (_myName == "Ragon")//라곤
        {
            stateMonsterName = MonsterName.A_Skl_03;
            myAttackType = AttackerType.OnlySkillAttacker;
            MyScore = 3;

        }
        else if (_myName == "BloodyBerry")//블러디베리
        {
            stateMonsterName = MonsterName.S_Wrr_01;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 4;

        }

        else if (_myName == "PineArs")//파인아스
        {
            stateMonsterName = MonsterName.S_Tnk_01;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 4;
        }

        else if (_myName == "Lio")//리오
        {
            stateMonsterName = MonsterName.C_Skl_01;
            myAttackType = AttackerType.ShotAttacker;
            MyScore = 1;

        }

        else if (_myName == "StelladeLucy")//스텔라데루시
        {
            stateMonsterName = MonsterName.S_Skl_01;
            myAttackType = AttackerType.longAttacker;
            MyScore = 4;


        }

        else if (_myName == "ChecheRiry")//체체리리
        {
            stateMonsterName = MonsterName.S_Arch_01;
            myAttackType = AttackerType.longAttacker;
            MyScore = 4;

        }

        healthPoint = gameManagerScript.listCsvHealthPoint[myLevel - 1 + (int)stateMonsterName];
        attackPower = gameManagerScript.listCsvAttackPower[myLevel - 1 + (int)stateMonsterName];
        attackSpeed = gameManagerScript.listCsvAttackSpeed[myLevel - 1 + (int)stateMonsterName];
        moveSpeed = gameManagerScript.listCsvMoveSpeed[myLevel - 1 + (int)stateMonsterName];
        attackDistance = gameManagerScript.listCsvAttackRange[myLevel - 1 + (int)stateMonsterName];
        defense = gameManagerScript.listCsvDefend[myLevel - 1 + (int)stateMonsterName];
        criticalPercent = gameManagerScript.listCsvCriticalPercent[myLevel - 1 + (int)stateMonsterName];
        criticalPower = gameManagerScript.listCsvCriticalPower[myLevel - 1 + (int)stateMonsterName];
        if (_myName != "Loe")
        {
            skillPower1 = gameManagerScript.listCsvSkillCoolTime[myLevel - 1 + (int)stateMonsterName];
        }
        skillPower1 = gameManagerScript.listCsvSkillPower1[myLevel - 1 + (int)stateMonsterName];
        skillPower2 = gameManagerScript.listCsvSkillPower2[myLevel - 1 + (int)stateMonsterName];
        skillCoolTime = gameManagerScript.listCsvSkillCoolTime[myLevel - 1 + (int)stateMonsterName];
        skillEffect1 = gameManagerScript.listCsvSkillEffect1[myLevel - 1 + (int)stateMonsterName];
        skillEffect2 = gameManagerScript.listCsvSkillEffect2[myLevel - 1 + (int)stateMonsterName];
        skillRadius = gameManagerScript.listCsvSkillRadius[myLevel - 1 + (int)stateMonsterName];
        skillDuration = gameManagerScript.listCsvSkillDuration[myLevel - 1 + (int)stateMonsterName];
        pasteHealthPoint = healthPoint;
        maxHealthPoint = healthPoint;
        attackSpeedCount = attackSpeed - 0.1f;

        if (_myName == "Homer")//호머의 처음 쉴드방어 생성
        {
            shieldDefense = healthPoint * skillPower1;
            pasteShieldDefense = shieldDefense;
        }

        if (_myName == "ShotChillyBomb")//꼬마 페퍼팡의 능력치 부여
        {
            healthPoint = 1;
            moveSpeed = 0;
            attackPower = 0;
            defense = 0;
            stateMonsterName = MonsterName.B_Skl_01;
            myAttackType = AttackerType.ShotAttacker;
            pasteHealthPoint = healthPoint;
            maxHealthPoint = healthPoint;
        }

        if(_myName == "StelladeLucy")//스텔라 쿨타임 
        {
            skillCoolTime = 5;
        }

        else if(_myName =="Loe")//로에의 스킬파워
        {
            skillPower1 = 0.24f;
        }
    }

    public void IsBrocoliAttackedMonster()//브로콜리의 몬스터 공격 함수
    {
        //var list = new List<MonsterPropertiesScript>();
        List<MonsterPropertieScript> listEnemy = new List<MonsterPropertieScript>();

        for (int i = 0; i < listMyEnemyobj.Count; i++)//때릴 몬스터를 정해주기 위한 For문
        {
            if (listMyEnemyobj[i] == null)
            {

                continue;
            }

            else
            {
                //Debug.Log("실행0");
                if (listMyEnemyPosDistance[i] <= targetDistanceXY +0.01f)//사정거리 안에 있는 모든 몬스터들의 리스트 가져오기
                {
                    listEnemy.Add(listMyEnemyobj[i].GetComponent<MonsterPropertieScript>());

                }
            }

        }


        for (int i = 0; i < listEnemy.Count; i++)//2명 이상 넣는다면 그다음부터는 삭제
        {
            if (listEnemy.Count > 2)
            {
                listEnemy.RemoveAt(Random.Range(0, listEnemy.Count));

            }
        }


        if (listEnemy.Count <= 2)//1명 혹은 2명을 때릴 때 
        {
            List<MonsterPropertieScript> listCacheTarget = new List<MonsterPropertieScript>();//떄려줄 몬스터 스크립트들
            List<TextMeshProUGUI> listTargetTmp = new List<TextMeshProUGUI>();//데미지 텍스트
            List<GameObject> listObjDamageTmp = new List<GameObject>();//텍스트 나오는 캔버스
            List<float> listAtkRandomCritical = new List<float>();//크리티컬 확률
            List<int> listFinalDamage = new List<int>();//공격식 다 끝낸 데미지
            List<float> listEvasionRandom = new List<float>();//회피확률
            for (int i = 0; i < listEnemy.Count; i++)
            {
                //temp[i].healthPoint -= myDamage; 
                listCacheTarget.Add(listEnemy[i].GetComponent<MonsterPropertieScript>()); //= MyTarget.GetComponent<MonsterPropertiesScript>();
                listObjDamageTmp.Add(Instantiate(objDamageTmpcanv, listCacheTarget[i].objMyDamagecanv.transform.position, Quaternion.identity, listCacheTarget[i].objMyDamagecanv.transform));
                listTargetTmp.Add(listObjDamageTmp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());//...
                listAtkRandomCritical.Add(Random.Range(0, 100));
                listEvasionRandom.Add(Random.Range(1,100));

            }

            for (int i = 0; i < listCacheTarget.Count; i++)//때릴 몬스터를 검사한다
            {
                if (listCacheTarget[i].evasion >= listEvasionRandom[i])//적이 회피했다면
                {
                    listTargetTmp[i].text = "evasion";
                }

                else//회피하지 않았다면
                {
                    if (listAtkRandomCritical[i] > criticalPercent)//크리티컬이 안터졌다면
                    {
                        listFinalDamage.Add((int)((attackPower * 100 / (100 + listCacheTarget[i].defense)) + ((attackPower * 100 / (100 + listCacheTarget[i].defense))
                            * Random.Range(-0.05f, 0.06f))));

                    }


                    else if (listAtkRandomCritical[i] <= criticalPercent)//크리티컬이 터졌다면
                    {
                        listFinalDamage.Add((int)((attackPower * 100 / (100 + listCacheTarget[i].defense)) + ((attackPower * 100 / (100 + listCacheTarget[i].defense))
                        * Random.Range(-0.05f, 0.06f) + attackPower * criticalPower)));

                        listTargetTmp[i].color = new Color(255, 255, 0);
                    }


                    if (listCacheTarget[i].shieldDefense <= 0)//방어막이 없다면
                    {
                        listCacheTarget[i].healthPoint -= listFinalDamage[i];
                    }

                    else if (listCacheTarget[i].shieldDefense > 0)//방어막이 있다면
                    {
                        listCacheTarget[i].shieldDefense -= listFinalDamage[i];

                        if (listCacheTarget[i].shieldDefense < 0)//방어막이 깨졋다면
                        {
                            listCacheTarget[i].healthPoint += listCacheTarget[i].shieldDefense;
                            listCacheTarget[i].shieldDefense = 0;

                        }

                    }

                    if (listCacheTarget[i].myMonsterName == "Watteron")//때린 상태가 워터론이였다면 반사대미지 입음
                    {
                        if (this.shieldDefense <= 0)
                        {
                            this.healthPoint -= listCacheTarget[i].attackPower * listCacheTarget[i].skillPower1;

                        }

                        if (this.shieldDefense > 0)
                        {
                            this.shieldDefense -= listCacheTarget[i].attackPower * listCacheTarget[i].skillPower1;

                            if (this.shieldDefense < 0)
                            {
                                this.healthPoint += this.shieldDefense;
                                this.shieldDefense = 0;

                            }

                            this.pasteHealthPoint = healthPoint;
                            //CashingMyTarget.PasteShieldDef = CashingMyTarget.ShieldDef;
                        }

                    }
                    if (listCacheTarget[i].myMonsterName == "PineArs")//때린상대가 파인아스라면 파인아스의 타겟이됨
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
                Destroy(listObjDamageTmp[i], 2);

            }
           
        }

    }//브로콜리 공격 끝 

    public void IsChillyBombAttackedMonster()//페퍼팡 공격
    {
        //List<MonsterPropertieScript> listEnemy = new List<MonsterPropertieScript>();//적들 넣어주기

        List<MonsterPropertieScript> CacheTarget = new List<MonsterPropertieScript>();//적들 오브젝트 캐싱하여 넣어주기
        for (int i = 0; i < listMyEnemyobj.Count; i++)
        {
            if (listMyEnemyobj[i] == null)
            {

                continue;
            }

            else
            {
                if (listMyEnemyPosDistance[i] < skillRadius)//공격범위
                {
                    CacheTarget.Add(listMyEnemyobj[i].GetComponent<MonsterPropertieScript>());
                }
            }
        }

            
            List<TextMeshProUGUI> listTargetTmp = new List<TextMeshProUGUI>();//데미지텍스트
            List<GameObject> listDamageObj = new List<GameObject>();//데미지 텍스트의 캔버스
            List<float> listAtkRandomCritical = new List<float>();//크리티컬 확률
            List<int> listFinalDamage = new List<int>();//공격식 다 계산한 후 최종 대미지
             float evasionRandom = Random.Range(1, 100);//회피율

            for (int i = 0; i < CacheTarget.Count; i++)//공격범위 안에 있는 모든 몬스터 공격과 데미지 띄우기
            {
                //temp[i].healthPoint -= myDamage; 
               // CacheTarget.Add(CacheTarget[i].GetComponent<MonsterPropertieScript>()); //= MyTarget.GetComponent<MonsterPropertiesScript>();
                listDamageObj.Add(Instantiate(objDamageTmpcanv, CacheTarget[i].objMyDamagecanv.transform.position, Quaternion.identity, CacheTarget[i].objMyDamagecanv.transform));
                listTargetTmp.Add(listDamageObj[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());//...
                listAtkRandomCritical.Add(Random.Range(0, 100));

            }

          for(int i = 0; i < CacheTarget.Count; i++)//공격범위 안에 있는 모두 공격
          {
            if(CacheTarget[i] == null)
            {
                CacheTarget.RemoveAt(i);
                continue;
            }


            if (CacheTarget[i].evasion >= evasionRandom)
            {
                listTargetTmp[i].text = "evasion";
            }
            

            else
            {
                if (CacheTarget[i] == null)
                {
                    CacheTarget.RemoveAt(i);
                    continue;
                }


                if (listAtkRandomCritical[i] > criticalPercent)//크리티컬이 안터졌다면
                {
                    listFinalDamage.Add((int)((attackPower) + ((attackPower)
                        * Random.Range(-0.05f, 0.06f))));
                }

                else if (listAtkRandomCritical[i] <= criticalPercent)//크리티컬이 터졌다면
                {
                    listFinalDamage.Add((int)((attackPower) + ((attackPower)
                    * Random.Range(-0.05f, 0.06f) + attackPower * criticalPower)));

                    listTargetTmp[i].color = new Color(1, 1, 0);
                }

                if (CacheTarget[i].shieldDefense <= 0)//쉴드가 없다면 hp 바로 닳기
                {
                    CacheTarget[i].healthPoint -= listFinalDamage[i];
                }

                else if (CacheTarget[i].shieldDefense > 0)//쉴드가 있다면 
                {
                    CacheTarget[i].shieldDefense -= listFinalDamage[i];

                    if (CacheTarget[i].shieldDefense < 0)
                    {
                        CacheTarget[i].healthPoint += CacheTarget[i].shieldDefense;
                        CacheTarget[i].shieldDefense = 0;

                    }
                }

                if (CacheTarget[i].myMonsterName == "Watteron")//워터론을 때렸을때 대미지 반사
                {
                    if (this.shieldDefense <= 0)
                    {
                        this.healthPoint -= CacheTarget[i].attackPower * CacheTarget[i].skillPower1;

                    }

                    if (this.shieldDefense > 0)
                    {
                        this.shieldDefense -= CacheTarget[i].attackPower * CacheTarget[i].skillPower1;

                        if (this.shieldDefense < 0)
                        {
                            this.healthPoint += this.shieldDefense;
                            this.shieldDefense = 0;

                        }

                        this.pasteHealthPoint = healthPoint;
                        //CashingMyTarget.PasteShieldDef = CashingMyTarget.ShieldDef;
                    }

                }

                CacheTarget[i].pasteHealthPoint = CacheTarget[i].healthPoint;
                listTargetTmp[i].text = "" + listFinalDamage[i];
            }
                Destroy(listDamageObj[i], 2);

          }

    }//페퍼팡 공격

    public void IsVocadAttackedMonster()//보카드 공격 함수
    {
        List<MonsterPropertieScript> listEnemy = new List<MonsterPropertieScript>();//적들 넣어주기

        listEnemy.Add(objMyTaget.GetComponent<MonsterPropertieScript>());

        float Target2;
        float Target3;

        for (int i = 0; i < listMyEnemyobj.Count; i++)
        {
          
            if (listMyEnemyobj[i] == null)//index 터지는것 방지
            {
               continue;
            }

            if (listMyEnemyobj[i] != null)
            {
                //Debug.Log("실행0");

                if(listMyEnemyobj[i] != objMyTaget)//때릴 몬스터 리스트 넣어주기
                {
                    Target2 = Vector2.Distance(listMyEnemyobj[i].transform.position, objMyTaget.transform.position);

                    if (skillEffect1 >= Target2)
                    {
                        listEnemy.Add(listMyEnemyobj[i].GetComponent<MonsterPropertieScript>());

                    }

                    else
                    {
                        continue;
                    }

                }

                if (listMyEnemyobj.Count > 2)//때릴 몬스터 리스트에 넣어주기
                {
                    if (listMyEnemyobj[i] != objMyTaget && listEnemy[1] != listMyEnemyobj[i].GetComponent<MonsterPropertieScript>())
                    {
                        Target3 = Vector2.Distance(listMyEnemyobj[i].transform.position, listEnemy[1].transform.position);                   
                        if (skillEffect2 >= Target3)//체인공격 1번 몬스터와 스킬 거리만큼 가까이 있는애 리스트에 넣음
                        {

                            listEnemy.Add(listMyEnemyobj[i].GetComponent<MonsterPropertieScript>());

                        }

                    }
                }

                if (listEnemy.Count == 3)//떄릴 몬스터가 3마리 다 넣어줬다면 없애주세요
                { 
                    break;
                }
            }

        }

        if (listEnemy.Count == 3)//3명 체인어택
        {

            List<TextMeshProUGUI> listTargetTmp = new List<TextMeshProUGUI>();//데미지 텍스트
            List<GameObject> listDamageObj = new List<GameObject>();// 데미지 텍스트 캔버스
            List<float> listAtkRandomCritical = new List<float>();//크리티컬 확률
            List<int> listFinalDamage = new List<int>();//공격식 끝낸 데미지 
            List<MonsterPropertieScript> listCacheTarget = new List<MonsterPropertieScript>();//공격할 대상 추려낸 리스트
            List<float> listEvasionRandom = new List<float>();//회피확률

            for (int i = 0; i < listEnemy.Count; i++)//몬스터의 공격 리시트 및 데미지 텍스트 띄우기 전 준비 for문
            {
             
                listCacheTarget.Add(listEnemy[i]); //= MyTarget.GetComponent<MonsterPropertiesScript>();
                listDamageObj.Add(Instantiate(objDamageTmpcanv, listCacheTarget[i].objMyDamagecanv.transform.position, Quaternion.identity, listCacheTarget[i].objMyDamagecanv.transform));
                listTargetTmp.Add(listDamageObj[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());//...
                listAtkRandomCritical.Add(Random.Range(0, 100));
                listEvasionRandom.Add(Random.Range(1, 100));

            }

            for (int i = 0; i < listCacheTarget.Count; i++)//몬스터를 때리기 위한 for문 
            {
                if(listCacheTarget[i]==null)//index 터짐 방지
                {
                    listCacheTarget.RemoveAt(i);
                    continue;
                }


                if (listCacheTarget[i].evasion >= listEvasionRandom[i]) // 회피했다면
                {
                    listTargetTmp[i].text = "evasion";
                }
                else// 회피 못했다면 공격하세요
                {
                 

                    if (listAtkRandomCritical[i] > criticalPercent)//크리티컬이 안터졌다면
                    {
                        listFinalDamage.Add((int)((attackPower * 100 / (100 + listCacheTarget[i].defense)) + ((attackPower * 100 / (100 + listCacheTarget[i].defense))
                            * Random.Range(-0.05f, 0.06f))));
                    }

                    else if (listAtkRandomCritical[i] <= criticalPercent)//크리티컬이 터졌다면
                    {
                        listFinalDamage.Add((int)((attackPower * 100 / (100 + listCacheTarget[i].defense)) + ((attackPower * 100 / (100 + listCacheTarget[i].defense))
                        * Random.Range(-0.05f, 0.06f) + attackPower * criticalPower)));
                        listTargetTmp[i].color = new Color(255, 255, 0);
                    }

                    if (listCacheTarget[i] == null)//인덱스 터짐 방지
                    {
                        listCacheTarget.RemoveAt(i);

                       
                        continue;
                    }

                    else if (listCacheTarget[i].shieldDefense <= 0)//쉴드가 없다면 hp닳기
                    {
                       listCacheTarget[i].healthPoint -= listFinalDamage[i];
                        
                    }

                    else if (listCacheTarget[i].shieldDefense > 0)//쉴드가 있다면 쉴드부터
                    {
                        listCacheTarget[i].shieldDefense -= listFinalDamage[i];

                        if (listCacheTarget[i].shieldDefense < 0)
                        {
                            listCacheTarget[i].healthPoint += listCacheTarget[i].shieldDefense;
                            listCacheTarget[i].shieldDefense = 0;

                        }

                    }



                    if (listCacheTarget[i].myMonsterName == "Watteron")//때린 상대가 워터론이라면 대미지 반사 입기
                    {
                        if (this.shieldDefense <= 0)
                        {
                            this.healthPoint -= listCacheTarget[i].attackPower * listCacheTarget[i].skillPower1;

                        }

                        if (this.shieldDefense > 0)
                        {
                            this.shieldDefense -= listCacheTarget[i].attackPower * listCacheTarget[i].skillPower1;

                            if (this.shieldDefense < 0)
                            {
                                this.healthPoint += this.shieldDefense;
                                this.shieldDefense = 0;

                            }

                            this.pasteHealthPoint = healthPoint;
                            //CashingMyTarget.PasteShieldDef = CashingMyTarget.ShieldDef;
                        }

                    }

                    if (listCacheTarget[i].myMonsterName == "PineArs")//내가 떄린상대가 파인아스라면 파인아스 타겟 되기
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
  
        }

        else if (listEnemy.Count == 2) // 둘중 한명 랜덤으로 공격 3타
        {
            List<TextMeshProUGUI> listTargetTmp = new List<TextMeshProUGUI>();//데미지 텍스트
            List<GameObject> listDamageObj = new List<GameObject>();//데미지 텍스트 캔버스
            List<int> listAtkRandomCritical = new List<int>();//크리티컬확률
            List<int> listFinalDamage = new List<int>();//공격식 끝낸 데미지
            int FinalAttackRandomUnit = Random.Range(0, 2);//2명중 한명 다시 때리기
            List<MonsterPropertieScript> listCacheTarget = new List<MonsterPropertieScript>();//공격대상
            List<float> listEvasionRandom = new List<float>();//회피확률

            for (int i = 0; i < listEnemy.Count; i++)//때리기 위한 준비 for문
            {
                //temp[i].healthPoint -= myDamage; 
                listCacheTarget.Add(listEnemy[i].GetComponent<MonsterPropertieScript>()); //= MyTarget.GetComponent<MonsterPropertiesScript>();
                listDamageObj.Add(Instantiate(objDamageTmpcanv, listCacheTarget[i].objMyDamagecanv.transform.position, Quaternion.identity, listCacheTarget[i].objMyDamagecanv.transform));
                listTargetTmp.Add(listDamageObj[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());//...
                listAtkRandomCritical.Add(Random.Range(0, 100));
                listEvasionRandom.Add(Random.Range(1, 100));
            }

            listAtkRandomCritical.Add(Random.Range(0, 100));   
            listDamageObj.Add(Instantiate(objDamageTmpcanv, listCacheTarget[FinalAttackRandomUnit].objMyDamagecanv.transform.position, Quaternion.identity, listCacheTarget[FinalAttackRandomUnit].objMyDamagecanv.transform));
            listTargetTmp.Add(listDamageObj[listDamageObj.Count -1].transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        

            for (int i = 0; i < listCacheTarget.Count; i++)//몬스터 때리기
            {
                if(listCacheTarget[i] == null)
                {
                    listCacheTarget.RemoveAt(i);
                    continue;
                }


                if (listCacheTarget[i].evasion >= listEvasionRandom[i])//회피했다면 
                {
                    listTargetTmp[i].text = "evasion";
                }

                else//회피를 못했다면
                {

                    if (listAtkRandomCritical[i] > criticalPercent)//크리티컬이 안터졌다면
                    {
                        listFinalDamage.Add((int)((attackPower * 100 / (100 + listCacheTarget[i].defense)) + ((attackPower * 100 / (100 + listCacheTarget[i].defense))
                            * Random.Range(-0.05f, 0.06f))));
                    }

                    else if (listAtkRandomCritical[i] <= criticalPercent)//크리티컬이 터졌다면
                    {
                        listFinalDamage.Add((int)((attackPower * 100 / (100 + listCacheTarget[i].defense)) + ((attackPower * 100 / (100 + listCacheTarget[i].defense))
                        * Random.Range(-0.05f, 0.06f) + attackPower * criticalPower)));

                        listTargetTmp[i].color = new Color(255, 255, 0);
                    }

                    if (listCacheTarget[i].shieldDefense <= 0)//hp바로 공격
                    {
                        listCacheTarget[i].healthPoint -= listFinalDamage[i];
                    }

                    else if (listCacheTarget[i].shieldDefense > 0)//쉴드부터 없애고 hp공격
                    {
                        listCacheTarget[i].shieldDefense -= listFinalDamage[i];

                        if (listCacheTarget[i].shieldDefense < 0)
                        {
                            listCacheTarget[i].healthPoint += listCacheTarget[i].shieldDefense;
                            listCacheTarget[i].shieldDefense = 0;

                        }

                    }

                    if (listCacheTarget[i].myMonsterName == "Watteron")//워터론의 공격반사 대미지 입기
                    {
                        if (this.shieldDefense <= 0)
                        {
                            this.healthPoint -= listCacheTarget[i].attackPower * listCacheTarget[i].skillPower1;

                        }

                        if (this.shieldDefense > 0)
                        {
                            this.shieldDefense -= listCacheTarget[i].attackPower * listCacheTarget[i].skillPower1;

                            if (this.shieldDefense < 0)
                            {
                                this.healthPoint += this.shieldDefense;
                                this.shieldDefense = 0;

                            }

                            this.pasteHealthPoint = healthPoint;
                            //CashingMyTarget.PasteShieldDef = CashingMyTarget.ShieldDef;
                        }

                    }
                }

                if (listCacheTarget[i].myMonsterName == "PineArs")//파인아스 첫번쨰 타겟 되기
                {
                    if (listCacheTarget[i].isPinearsAttackedFirst == false)
                    {
                        listCacheTarget[i].objMyTaget = this.gameObject;
                        listCacheTarget[i].isPinearsAttackedFirst = true;
                    }

                }

                listCacheTarget[i].pasteHealthPoint = listCacheTarget[i].healthPoint;
                listTargetTmp[i].text = "" + listFinalDamage[i];
                Destroy(listDamageObj[i], 2);

            }

            if (listAtkRandomCritical[listAtkRandomCritical.Count - 1] <= criticalPercent) // 크리티컬이 터졌다면
            {
                listFinalDamage.Add((int)((attackPower * 100 / (100 + listCacheTarget[FinalAttackRandomUnit].defense)) + ((attackPower * 100 / (100 + listCacheTarget[FinalAttackRandomUnit].defense))
                   * Random.Range(-0.05f, 0.06f) + attackPower * criticalPower)));
                  listTargetTmp[listTargetTmp.Count - 1].color = new Color(1, 1, 0);
            }

            else // 안터졌다면
            {
                listFinalDamage.Add((int)((attackPower * 100 / (100 + listCacheTarget[FinalAttackRandomUnit].defense)) + ((attackPower * 100 / (100 + listCacheTarget[FinalAttackRandomUnit].defense))
                      * Random.Range(-0.05f, 0.06f))));
            }

            if(listEnemy.Count != 2)
            {
                return;
            }

            
            if (listCacheTarget[FinalAttackRandomUnit].shieldDefense <= 0)//2타를 떄린 뒤 2명중 한명 랜덤 선택하여 때리기 
            {
                listCacheTarget[FinalAttackRandomUnit].healthPoint -= listFinalDamage[listFinalDamage.Count - 1];
            }

            else if (listCacheTarget[FinalAttackRandomUnit].shieldDefense > 0)
            {
                listCacheTarget[FinalAttackRandomUnit].shieldDefense -= listFinalDamage[listFinalDamage.Count - 1];

                if (listCacheTarget[FinalAttackRandomUnit].shieldDefense < 0)
                {
                    listCacheTarget[FinalAttackRandomUnit].healthPoint += listCacheTarget[listFinalDamage.Count - 1].shieldDefense;
                    listCacheTarget[FinalAttackRandomUnit].shieldDefense = 0;
                }
                //CashingMyTarget[i].PasteShieldDef = CashingMyTarget[i].ShieldDef;
            }

            if (listCacheTarget[FinalAttackRandomUnit].myMonsterName == "Watteron")//워터론의 공격 대미지 반사 맞기
            {
                if (this.shieldDefense <= 0)
                {
                    this.healthPoint -= listCacheTarget[FinalAttackRandomUnit].attackPower * listCacheTarget[FinalAttackRandomUnit].skillPower1;

                }

                if (this.shieldDefense > 0)
                {
                    this.shieldDefense -= listCacheTarget[FinalAttackRandomUnit].attackPower * listCacheTarget[FinalAttackRandomUnit].skillPower1;

                    if (this.shieldDefense < 0)
                    {
                        this.healthPoint += this.shieldDefense;
                        this.shieldDefense = 0;

                    }

                    this.pasteHealthPoint = healthPoint;
                    //CashingMyTarget.PasteShieldDef = CashingMyTarget.ShieldDef;
                }

            }

            if (listCacheTarget[FinalAttackRandomUnit].myMonsterName == "PineArs")//파인아스라면 첫번쨰 타깃 되기
            {
                if (listCacheTarget[FinalAttackRandomUnit].isPinearsAttackedFirst == false)
                {
                    listCacheTarget[FinalAttackRandomUnit].objMyTaget = this.gameObject;
                    listCacheTarget[FinalAttackRandomUnit].isPinearsAttackedFirst = true;
                }

            }

            listCacheTarget[FinalAttackRandomUnit].pasteHealthPoint = listCacheTarget[FinalAttackRandomUnit].healthPoint;
            listTargetTmp[listTargetTmp.Count-1].text = "" + listFinalDamage[listFinalDamage.Count-1];
            Destroy(listDamageObj[listDamageObj.Count -1], 2);
            
        }

        else if(listEnemy.Count == 1)//한놈만 3타 공격
        {

            MonsterPropertieScript CacheTarget;//공격당할 타겟
            CacheTarget = objMyTaget.GetComponent<MonsterPropertieScript>();
            GameObject listDamageObj = new GameObject();//타겟의 캔버스
            List<TextMeshProUGUI> listTargetTmp = new List<TextMeshProUGUI>();//타겟의 대미지 텍스트
            List<float> listfinalDamage = new List<float>();//공격식을 다 끝낸 공격
            List<float> listAtkCriticalRandom = new List<float>();//크리티컬 터졌는지
            float evasionRandom = Random.Range(1, 100); // 크리티컬 터졌는지

            for (int i = 0; i < 3; i++)
            {
                listDamageObj = (Instantiate(objDamageTmpcanv, CacheTarget.objMyDamagecanv.transform.position, Quaternion.identity, CacheTarget.objMyDamagecanv.transform));
                listTargetTmp.Add(listDamageObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>());//.text = "" + (pastehealthPoint - healthPoint);
                listAtkCriticalRandom.Add(Random.Range(0, 100));

            }

            for (int i = 0; i < 3; i++)
            {
                if (CacheTarget.evasion >= evasionRandom)
                {
                    listTargetTmp[i].text = "evasion";
                }

                else
                {

                    if (listAtkCriticalRandom[i] > criticalPercent)//크리티컬이 터지지 않았다면
                    {
                        listfinalDamage.Add((int)((attackPower * 100 / (100 + CacheTarget.defense)) + ((attackPower * 100 / (100 + CacheTarget.defense)) * Random.Range(-0.05f, 0.06f))));
                    }

                    else//크리티컬이 터졌다면
                    {
                        listfinalDamage.Add((int)((attackPower * 100 / (100 + CacheTarget.defense)) + ((attackPower * 100 / (100 + CacheTarget.defense))
                        * Random.Range(-0.05f, 0.06f) + attackPower * criticalPower)));

                        listTargetTmp[i].color = new Color(1, 1, 0);
                    }

                    if (CacheTarget.shieldDefense <= 0)//쉴드가 없으면 hp부터 깎기
                    {
                        CacheTarget.healthPoint -= listfinalDamage[i];

                    }

                    if (CacheTarget.shieldDefense > 0)//쉴드가 있다면 쉴드 까고 hp깎기
                    {
                        CacheTarget.shieldDefense -= listfinalDamage[i];

                        if (CacheTarget.shieldDefense < 0)
                        {
                            CacheTarget.healthPoint += CacheTarget.shieldDefense;
                            CacheTarget.shieldDefense = 0;
                        }

                    }

                    if (CacheTarget.myMonsterName == "Watteron")//워터론의 반사대미지
                    {
                        if (this.shieldDefense <= 0)
                        {
                            this.healthPoint -= CacheTarget.attackPower * CacheTarget.skillPower1;

                        }

                        if (this.shieldDefense > 0)
                        {
                            this.shieldDefense -= CacheTarget.attackPower * CacheTarget.skillPower1;

                            if (this.shieldDefense < 0)
                            {
                                this.healthPoint += this.shieldDefense;
                                this.shieldDefense = 0;

                            }
                            this.pasteHealthPoint = healthPoint;
                            //CashingMyTarget.PasteShieldDef = CashingMyTarget.ShieldDef;
                        }
                    }

                    if (CacheTarget.myMonsterName == "PineArs")//파인아스 깨우기
                    {
                        if (CacheTarget.isPinearsAttackedFirst == false)
                        {
                            CacheTarget.objMyTaget = this.gameObject;
                            CacheTarget.isPinearsAttackedFirst = true;

                        }
                    }

                    CacheTarget.pasteHealthPoint = CacheTarget.healthPoint;
                    listTargetTmp[i].text = "" + listfinalDamage[i];
                }
                Destroy(listDamageObj, 2);

            }

        }
    }//보카드 공격

    public void IsDurigonAttackedMonster()//두리안 공격
    {

        //var list = new List<MonsterPropertiesScript>();
        List<MonsterPropertieScript> listEnemy = new List<MonsterPropertieScript>();//두리안이 공격할 유닛 리스트
       
        float BufMovingCount = moveCount *0.5f;
        float BufattackPower = (attackPower * BufMovingCount) +attackPower;

        for (int i = 0; i < listMyEnemyobj.Count; i++)
        {
            if (listMyEnemyobj[i] == null)
            {

                continue;
            }

            //if (MyParentEnemy.MyEnemy[i] != null)
            else
            {
                //Debug.Log("실행0");
                if (listMyEnemyPosDistance[i] <= attackDistance)//거리안에 있는 몹들 전부 넣어주기
                {
                    listEnemy.Add(listMyEnemyobj[i].GetComponent<MonsterPropertieScript>());

                }
            }

        }

         List<MonsterPropertieScript> listCacheTarget = new List<MonsterPropertieScript>();//두리안이 공격할 리스트
         List<TextMeshProUGUI> listTargetTmp = new List<TextMeshProUGUI>();//데미지 텍스트
         List<GameObject> listDamageObj = new List<GameObject>();//공격받은 유닛의 캔버스
         List<float> listAtkRandomCritical = new List<float>();//크리티컬이 터졌는가
         List<int> listFinalDamage = new List<int>();//공격식 적용한 데미지
         List<float> listEvasionRandom= new List<float>();//회피했는지 판단여부

        for (int i = 0; i < listEnemy.Count; i++)//공격 직전 세팅하기
        {
                //temp[i].healthPoint -= myDamage; 
            listCacheTarget.Add(listEnemy[i].GetComponent<MonsterPropertieScript>()); //= MyTarget.GetComponent<MonsterPropertiesScript>();
            listDamageObj.Add(Instantiate(objDamageTmpcanv, listCacheTarget[i].objMyDamagecanv.transform.position, Quaternion.identity, listCacheTarget[i].objMyDamagecanv.transform));
            listTargetTmp.Add(listDamageObj[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());//...
            listAtkRandomCritical.Add(Random.Range(0, 100));
            listEvasionRandom.Add(Random.Range(1,100));

        }

        for (int i = 0; i < listCacheTarget.Count; i++)
        {
            if(listCacheTarget[i] == null)
            {
                listCacheTarget.RemoveAt(i);
                continue;
            }

            if (listCacheTarget[i].evasion >= listEvasionRandom[i])//회피했다면
            {
                listTargetTmp[i].text = "evasion";

            }

            else//회피하지 못하였다면
            {

                if (listAtkRandomCritical[i] > criticalPercent)//크리티컬이 안터졌다면
                {
                    listFinalDamage.Add((int)((BufattackPower * 100 / (100 + listCacheTarget[i].defense)) + ((BufattackPower * 100 / (100 + listCacheTarget[i].defense))
                        * Random.Range(-0.05f, 0.06f))));
                }

                else if (listAtkRandomCritical[i] <= criticalPercent)//크리티컬이 터졌다면
                {
                    listFinalDamage.Add((int)((BufattackPower * 100 / (100 + listCacheTarget[i].defense)) + ((BufattackPower * 100 / (100 + listCacheTarget[i].defense))
                      * Random.Range(-0.05f, 0.06f) + attackPower * criticalPower)));
                    listTargetTmp[i].color = new Color(255, 255, 0);
                }

                if (listCacheTarget[i] != null) 
                {

                    if (listCacheTarget[i].shieldDefense <= 0)//쉴드 없다면 hp부터 까세요
                    {
                        listCacheTarget[i].healthPoint -= listFinalDamage[i];
                    }

                    else if (listCacheTarget[i].shieldDefense > 0)//쉴드 먼저 까세요 
                    {
                        listCacheTarget[i].shieldDefense -= listFinalDamage[i];

                        if (listCacheTarget[i].shieldDefense < 0)
                        {
                            listCacheTarget[i].healthPoint += listCacheTarget[i].shieldDefense;
                            listCacheTarget[i].shieldDefense = 0;
                        }
                    }

                    if (listCacheTarget[i].myMonsterName == "Watteron")//워터론의 반사대미지
                    {
                        if (this.shieldDefense <= 0)
                        {
                            this.healthPoint -= listCacheTarget[i].attackPower * listCacheTarget[i].skillPower1;

                        }

                        if (this.shieldDefense > 0)
                        {
                            this.shieldDefense -= listCacheTarget[i].attackPower * listCacheTarget[i].skillPower1;

                            if (this.shieldDefense < 0)
                            {
                                this.healthPoint += this.shieldDefense;
                                this.shieldDefense = 0;
                            }

                            this.pasteHealthPoint = healthPoint;
                        }
                    }

                    if (listCacheTarget[i].myMonsterName == "PineArs")//파인아스 깨우기
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
            }
            Destroy(listDamageObj[i], 2);
        }
          
    } // 두리곤 공격

    public void IsLoeHealedTeam()//로에의 힐 스킬
    {
        //var list = new List<MonsterPropertiesScript>();
        List<MonsterPropertieScript> listMyTeam = new List<MonsterPropertieScript>();//우리팀 리스트
        List<MonsterPropertieScript> listTwoMemberHeal = new List<MonsterPropertieScript>();//hp가 제일 없는 몬스터 치유해주기
        List<float> listHealthPointPercent = new List<float>();//hp가 %로 제일 적은 유닛 선별해주기 위한 변수

        if (this.CompareTag("MyTeam"))//내가 우리팀일때 우리팀 불러오기
        {
           
            for (int i = 0; i < gameManagerScript.listCashingMyUnitScript.Count; i++)//일단 살아있는 우리팀 전체의 스크립트 가져오기
            {
                listMyTeam.Add(gameManagerScript.listCashingMyUnitScript[i]);

            }

            for (int i = 0; i < listMyTeam.Count; i++)
            {              
               listHealthPointPercent.Add(listMyTeam[i].scrbarHealthPoint.size);
            }

            listHealthPointPercent.Sort();//체력이 낮은 순서대로 정렬

            for (int i = 0; i < listHealthPointPercent.Count; i++)
            {
                if (listHealthPointPercent[i] <= 0)
                {
                    listHealthPointPercent.RemoveAt(i);
                  
                }              
            }         

        }

        else if (this.CompareTag("EnemyTeam"))//내가 적팀일때 적팀 리스트 불러오기
        {
            for (int i = 0; i < gameManagerScript.listCashingEnemyUnitScript.Count; i++) //일단 살아있는 우리팀 전체의 스크립트 가져오기
            {
                listMyTeam.Add(gameManagerScript.listCashingEnemyUnitScript[i]);

            }

            for (int i = 0; i < listMyTeam.Count; i++)//닳은체력 퍼센트로 넣어주기
            {
                listHealthPointPercent.Add(listMyTeam[i].scrbarHealthPoint.size);
            }

            listHealthPointPercent.Sort();

            for (int i = 0; i < listHealthPointPercent.Count; i++) 
            {
                if (listHealthPointPercent[i] <= 0)
                {
                    listHealthPointPercent.RemoveAt(i);
                }
            }
        }

        for (int z = 0; z < listMyTeam.Count; z++)
        {
           
            if (listTwoMemberHeal.Count < 2)//힐받을 2명 집어넣기
            {

                if (listHealthPointPercent[0] == listMyTeam[z].scrbarHealthPoint.size)
                {
                    listTwoMemberHeal.Add(listMyTeam[z]);           
                }

                else if (listHealthPointPercent[1] == listMyTeam[z].scrbarHealthPoint.size)
                {
                    listTwoMemberHeal.Add(listMyTeam[z]);
                }
            }
        }

        List<TextMeshProUGUI> listTargetTmp = new List<TextMeshProUGUI>();//힐 텍스트
        List<GameObject> listDamageObj = new List<GameObject>();//힐 받은 유닛 캔버스

        for (int i = 0; i < listTwoMemberHeal.Count; i++)
        {
            if (listTwoMemberHeal[i] == null)//인덱스 터짐 방지 
            {
                listTwoMemberHeal.RemoveAt(i);
                continue;
            }

            else//힐 해주기
            {
                listDamageObj.Add(Instantiate(objDamageTmpcanv, listTwoMemberHeal[i].objMyDamagecanv.transform.position, Quaternion.identity, listTwoMemberHeal[i].objMyDamagecanv.transform));
                listTargetTmp.Add(listDamageObj[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());//...
                listTargetTmp[i].color = new Color(0, 0, 1); 
                listTwoMemberHeal[i].healthPoint += attackPower * skillPower1;

                if (listTwoMemberHeal[i].healthPoint > listTwoMemberHeal[i].maxHealthPoint)//최대체력 이상으로 채워주지 마세요
                {
                    listTwoMemberHeal[i].healthPoint = listTwoMemberHeal[i].maxHealthPoint;
                }

                listTwoMemberHeal[i].pasteHealthPoint = listTwoMemberHeal[i].healthPoint;
            }

        }
        
        for (int i = 0; i < listTargetTmp.Count; i++)//힐 받은 애 Text나오게 하기 
        {
            if(listTargetTmp[i] == null)
            {
                listTargetTmp.RemoveAt(i);
            }

            listTargetTmp[i].text = "" + Mathf.RoundToInt(attackPower * skillPower1);
         
            Destroy(listDamageObj[i], 2);
        }
        atkCount = 0;    
        //TwoMemberHeal.RemoveRange(0, TwoMemberHeal.Count);

    }//로에 힐(만)

    private void OnTriggerStay2D(Collider2D collision)//충돌했으면
    {
        if (gameManagerScript.isStartedBattle == true)//게임이 시작되기 전에 서로 밀치지 마세요
        {

            MonsterPropertieScript Target = collision.GetComponent<MonsterPropertieScript>(); // 부딪힌 유닛스크립트 불러오기
            if (Target.transform.position.x > 0.5f && Target.transform.position.x < 4.5f && Target.transform.position.y < 2.5f && Target.transform.position.y > -4.9)//맵 테두리 벗어나지 마시오
            {

                if (this.CompareTag("MyTeam"))//내가 우리팀이라면 밑으로 밀고
                {
                    if (this.transform.position.y > Target.transform.position.y)
                    {

                        Target.transform.position = new Vector2(collision.transform.position.x, collision.transform.position.y - (Target.moveSpeed + 0.1f) * Time.deltaTime);//밑으로 내려가세요


                        if (this.transform.position.x < Target.transform.position.x)// 나보다 오른쪽에 있으면
                        {
                            if (this.transform.position.x + 1 > Target.transform.position.x)//안겹치게 오른쪽으로 가세요 어디를 향하고있든 사이즈보다 좀 더 크게 
                            {
                                Target.transform.position = new Vector2(Target.transform.position.x + (Target.moveSpeed + 0.2f) * Time.deltaTime, collision.transform.position.y);
                            }

                        }

                        else if (this.transform.position.x > Target.transform.position.x)//나보다 왼쪽에있으면
                        {

                            if (this.transform.position.x - 1f < Target.transform.position.x) // 안겹치게 왼쪽으로 일단가세요 사이즈보다 좀 더 크게
                            {
                                Target.transform.position = new Vector2(Target.transform.position.x - (Target.moveSpeed + 0.2f) * Time.deltaTime, collision.transform.position.y);
                            }

                        }
                    }
                }

                else if (this.CompareTag("EnemyTeam"))//내가 적팀이라면 위로 밀기
                {
                    if (this.transform.position.y < collision.transform.position.y)
                    {
                        collision.transform.position = new Vector2(collision.transform.position.x, collision.transform.position.y + (Target.moveSpeed + 0.1f) * Time.deltaTime); // 조금 올라가세요

                        if (this.transform.position.x < Target.transform.position.x)//나보다 오른쪽에 있다면
                        {
                            if (this.transform.position.x + 1f > Target.transform.position.x)//안겹치게 오른쪽으로 가세요 어디를 향하고있든 사이즈보다 좀 더 크게
                            {
                                Target.transform.position = new Vector2(Target.transform.position.x + (Target.moveSpeed + 0.2f) * Time.deltaTime, collision.transform.position.y);
                            }

                        }

                        else if (this.transform.position.x > Target.transform.position.x)//나보다 왼쪽에 있다면
                        {

                            if (this.transform.position.x - 1f < Target.transform.position.x)//안겹치게 왼쪽으로 가세요 어디를 향하고있든 사이즈보다 좀 더 크게
                            {

                                Target.transform.position = new Vector2(Target.transform.position.x - (Target.moveSpeed + 0.2f) * Time.deltaTime, collision.transform.position.y);

                            }
                        }
                    }
                }
            }
        }
    }
    public void SummonSmallPome() //작은 젤리 소환
    {
        if (myMonsterName == "Pome" && this.gameObject.name != "ShotPome(Clone)") // 내가 작은젤리가 아닐 때 
        {          
                MonsterPropertieScript MyChildPome;
                MyChildPome = Instantiate(gameManagerScript.shotPome.gameObject, this.transform.position, this.transform.rotation).GetComponent<MonsterPropertieScript>();
                MyChildPome.objMyTaget = objMyTaget;
                MyChildPome.tag = this.gameObject.tag;
                List<MonsterPropertieScript> listEnemysScript = new List<MonsterPropertieScript>();
               
              for (int i = 0; i < listMyEnemyobj.Count; i++)//작은젤리 기본셋팅
              {
                    listEnemysScript.Add(listMyEnemyobj[i].GetComponent<MonsterPropertieScript>());
                    MyChildPome.listMyEnemyobj.Add(listMyEnemyobj[i]);
                    MyChildPome.listMyEnemyPosDistance.Add(listMyEnemyPosDistance[i]);

                    listEnemysScript[i].listMyEnemyobj.Add(MyChildPome.gameObject);
                    listEnemysScript[i].listMyEnemyPosDistance.Add(0);

              }

            if(this.CompareTag("EnemyTeam"))//적이면 아군 리스트 넣어주기
            {
                gameManagerScript.listEnemyUnitObj.Add(MyChildPome.gameObject);
                gameManagerScript.listCashingEnemyUnitScript.Add(MyChildPome);
            }

            else if(this.CompareTag("MyTeam"))//우리팀이면 적 리스트 넣어주기
            {
                gameManagerScript.listMyUnitObj.Add(MyChildPome.gameObject);
                gameManagerScript.listCashingMyUnitScript.Add(MyChildPome);

            }
        }
    }

    public void LioFloorSummon()//독구름 장판 소환
    {
       listMyFloorSkillPoolScript.Add(Instantiate(objMyFirstBullet, this.transform.position, Quaternion.identity, gameManagerScript.transform).GetComponent<FloorSkill>());

          for (int i = 0; i < listMyFloorSkillPoolScript.Count; i++)//총알에게 정보 넘겨주기
          {
              if (this.transform.position == listMyFloorSkillPoolScript[i].transform.position)//총알(바닥)이 탄창에 있다면
              {
                  if (objMyTaget != null) // 목표물, 스텟 넣어주기
                  {
                        listMyFloorSkillPoolScript[i].myTargetScript = objMyTaget.GetComponent<MonsterPropertieScript>();
                        listMyFloorSkillPoolScript[i].objMyParent = this.gameObject;
                        listMyFloorSkillPoolScript[i].myDamage = attackPower;
                        listMyFloorSkillPoolScript[i].isFinishedAttackMotion = true;
                        listMyFloorSkillPoolScript[i].myTargetPos = objMyTaget.transform.position;
                        listMyFloorSkillPoolScript[i].skillTimer = this.skillDuration;
                        listMyFloorSkillPoolScript[i].myRadius = this.skillRadius;
                        listMyFloorSkillPoolScript[i].criticalPercent = this.criticalPercent;
                        listMyFloorSkillPoolScript[i].criticalPower = this.skillRadius;
                        listMyFloorSkillPoolScript[i].skillPower = this.skillPower1;
                        listMyFloorSkillPoolScript[i].skillPower2 = this.skillPower2;

                  }
              }
          }
    }
    public void UseStellarSkill()//스텔라 스킬, 공격 함수
    {
        
        List<MonsterPropertieScript> listStarMonster = new List<MonsterPropertieScript>();
        List<GameObject> listDamageObj = new List<GameObject>();
        List<TextMeshProUGUI> listTargetTmp = new List<TextMeshProUGUI>();
        List<float> listFinalDamage = new List<float>();
        List<float> listRandomCritical = new List<float>();
        float evasionRandom = Random.Range(1, 100);

        for (int i = 0; i < listStellaStarobj.Count; i++)
        {
            if(listStellaStarobj[i] == null)
            {
                listStellaStarobj.RemoveAt(i);
                continue;
            }

           listStarMonster.Add(listStellaStarobj[i].transform.parent.GetComponent<MonsterPropertieScript>());
           listDamageObj.Add(Instantiate(objDamageTmpcanv, listStarMonster[i].transform.position, 
           Quaternion.identity, listStarMonster[i].objMyDamagecanv.transform));
           listTargetTmp.Add(listDamageObj[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());
           listRandomCritical.Add(Random.Range(0, 100));
        }

            for(int i = 0; i<listStarMonster.Count;i++)
            {
                                 
                if(listStarMonster[i].evasion >= evasionRandom)//회피했다면
                {
                    listTargetTmp[i].text = "evasion";
                }

                if(listRandomCritical[i] > criticalPercent)//크리티컬이 안터졌다면
                {
                       listFinalDamage.Add((int)((attackPower * 100  *skillPower1 / (100 + listStarMonster[i].defense)) + ((attackPower * 100 / (100 + listStarMonster[i].defense)) * Random.Range(-0.05f, 0.06f))));

                 }

                else//크리티컬이 터졌다면
                {
                    listFinalDamage.Add((int)((attackPower * 100 * skillPower1 / (100 + listStarMonster[i].defense)) + ((attackPower * 100 / (100 + listStarMonster[i].defense))
                    * Random.Range(-0.05f, 0.06f) + attackPower * criticalPower)));

                    listTargetTmp[i].color = new Color(1, 1, 0);
                }

                if (listStarMonster[i].shieldDefense <= 0)//쉴드가 없다면 hp부터 깎기
                {
                    listStarMonster[i].healthPoint -= listFinalDamage[i];
                }


                if (listStarMonster[i].shieldDefense > 0)//쉴드가 있다면 쉴드부터 깎기
                {
                    listStarMonster[i].shieldDefense -= listFinalDamage[i];

                    if (listStarMonster[i].shieldDefense < 0)
                    {
                        listStarMonster[i].healthPoint += listStarMonster[i].shieldDefense;
                        listStarMonster[i].shieldDefense = 0;
                    }

                //CashingMyTarget.PasteShieldDef = CashingMyTarget.ShieldDef;
                }

                listStarMonster[i].pasteHealthPoint = listStarMonster[i].healthPoint;
                listTargetTmp[i].text = "" + listFinalDamage[i];
                Destroy(listDamageObj[i], 2);
            }

        for(int i = 0; i< listStellaStarobj.Count;i++)//터진 별들은 모두 없애주세요
        {
            Destroy(listStellaStarobj[i]);
        }

        listStellaStarobj.RemoveRange(0, listStellaStarobj.Count);//스텔라의 별의 리스트를 모두 초기화 시켜주세요
        
    }

    public void IsStellarBuffActive()//스텔라가 사용했다면 본인 버프
    {
        this.moveSpeed += moveSpeed * skillEffect2;
        this.attackSpeed = attackSpeed * skillEffect2;
        StartCoroutine(CoIsStellaBuffTimer(skillDuration));
        List<MonsterPropertieScript> StarsMonsters = new List<MonsterPropertieScript>();
        
        for (int i = 0; i < listStellaStarobj.Count; i++)
        {
            if(listStellaStarobj[i] == null)
            {
                listStellaStarobj.RemoveAt(i);
                continue;
            }

            StarsMonsters.Add(listStellaStarobj[i].transform.parent.GetComponent<MonsterPropertieScript>());
           
        }

        for (int i = 0; i < StarsMonsters.Count; i++)
        {
            if(StarsMonsters[i] == null)
            {
                StarsMonsters.RemoveAt(i);
                continue;
            }

            if (StarsMonsters[i].myMonsterName != "StelladeLucy")//스텔라 끼리 한테는 서로 디버프 스킬이 통하지 않아요
            {
                StarsMonsters[i].moveSpeed -= StarsMonsters[i].moveSpeed * skillEffect1;
                StarsMonsters[i].attackSpeed -= StarsMonsters[i].moveSpeed * skillEffect1;
                StartCoroutine(StarsMonsters[i].CoIsStellaDebuffTimer(skillDuration));
            }
            
            else
            {
                continue;
            }
        }
    }

    public IEnumerator CoIsStellaDebuffTimer(float _debufftimer)//스텔라의 디버프 초기화
    {
        yield return new WaitForSeconds(_debufftimer);

        moveSpeed = gameManagerScript.listCsvMoveSpeed[myLevel - 1 + (int)stateMonsterName];
        attackSpeed = gameManagerScript.listCsvAttackSpeed[myLevel - 1 + (int)stateMonsterName];
    }
    public IEnumerator CoIsStellaBuffTimer(float _bufftimer)//스텔라의 버프 초기화
    {
         yield return new WaitForSeconds(_bufftimer);
        moveSpeed = gameManagerScript.listCsvMoveSpeed[myLevel - 1 + (int)stateMonsterName];
        attackSpeed = gameManagerScript.listCsvAttackSpeed[myLevel - 1 + (int)stateMonsterName];

    }


}
