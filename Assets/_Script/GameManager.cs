using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isStartedBattle; //게임시작 버튼을 눌렀을 때 판별하기 위한 Bool값
    public int unitNumber;//지금 나온 유닛 수
    public int unitType; //현재까지 만든 유닛의 수
    public float countGameOver;
    public int myTeamUnitScroe;
    public int enemyTeamUnitScore;

    [Header("List")]
    private List<int> listFinalUnitNumber = new List<int>();//유닛이 총 몇개가 소환되었는지 위한 리스트
    private List<int> listEnemyFinalUnitNumber = new List<int>();// 적 유닛이 총 몇개가 소환되었는지 리스트
    public List<GameObject> listUnitobj =new List<GameObject>();//현재까지 총 만든 유닛들의 리스트
    public List<GameObject> listMyUnitSitobj = new List<GameObject>();//내 유닛들의 소환될 자리 O 오브젝트들
    public List<GameObject> listEnemyUnitSitobj = new List<GameObject>();//적 유닛들이 소환될 자리 O 오브젝트들
    public List<GameObject> listMyUnitObj = new List<GameObject>();//내 유닛들의 게임오브젝트 리스트
    public List<GameObject> listEnemyUnitObj = new List<GameObject>();// 적 유닛들의 게임오브젝트 리스트
    public List<MonsterPropertieScript> listCashingMyUnitScript = new List<MonsterPropertieScript>();//내 유닛들의 정보 스크립트 리스트
    public List<MonsterPropertieScript> listCashingEnemyUnitScript = new List<MonsterPropertieScript>();//적 유닛들의 정보 스크립트 리스트
   //public List<GameObject> MyTeamHitInfo = new List<GameObject>();
   //public List<GameObject> EnemyTeamHitInfo = new List<GameObject>();
    //public List<MonsterPropertiesScript> MyTeamAttacker = new List<MonsterPropertiesScript>();
    //public List<MonsterPropertiesScript> EnemyTeamAttacker = new List<MonsterPropertiesScript>();
   

    [Header("Canvas")]
    public Transform trGamecanv;//게임의 전체적인 UI캔버스

    public List<Dictionary<string, string>> listDataCsvdic = new List<Dictionary<string, string>>();//읽을 Csv파일 리스트
    [Header("CSVList")]
    public List<float> listCsvHealthPoint = new List<float>();//체력 리스트
    public List<float> listCsvAttackPower = new List<float>();//공격력 리스트
    public List<float> listCsvDefend = new List<float>();//방어력 리스트
    public List<float> listCsvAttackRange = new List<float>();//공격 범위 리스트
    public List<float> listCsvAttackSpeed = new List<float>();//공격 속도 리스트
    public List<float> listCsvMoveSpeed = new List<float>();//이동속도 리스트
    public List<float> listCsvCriticalPercent = new List<float>();//크리티컬 퍼센트
    public List<float> listCsvCriticalPower = new List<float>();//크리티컬 공격 리스트
    public List<float> listCsvSkillDuration = new List<float>();//스킬 지속시간 리스트
    public List<float> listCsvSkillRadius = new List<float>();// 스킬의 범위 리스트
    public List<float> listCsvSkillPower1 = new List<float>();//스킬파워 1의 리스트
    public List<float> listCsvSkillPower2 = new List<float>();//스킬파워 2의 리스트
    public List<float> listCsvSkillCoolTime = new List<float>();//스킬 쿨타임 리스트
    public List<float> listCsvSkillEffect1 = new List<float>();//스킬 효과 리스트
    public List<float> listCsvSkillEffect2 = new List<float>();//스킬 효과2 리스트
    [Header("CharacterInput")]
    public GameObject objCharbtn;//상단의 버튼 3개의 오브젝트들을 모은 빈 오브젝트 게임이 시작되면 단순히 끄기 위한 오브젝트이다.
    public InputField infiMonsterLevel;//직접 유닛의 레벨을 입력하여 소환하게 만들어 주기 위한 Input
    public InputField infiMonsterIndex;//직접 어떤 종류의 유닛을 소환하려고 만든 Index
    public GameObject[] arrCharButtonobj;//유닛들이 소환될 위치의 O오브젝트들 단순히 끄기위한 오브젝트들이다.
    public TMP_Dropdown drdmonsterIndex;//몬스터의 종류를 입력하는게 아니라 하나씩 보고 소환하게 만들기 위한 드롭다운

    [Header("ChildMonster")]
    public MonsterPropertieScript shotPome;//꼬마 젤리의 몬스터 정보 스크립트
    public MonsterPropertieScript shotChill;//꼬마 페퍼팡의 몬스터 정보 스크립트

    [Header("GameOver")]
    public GameObject objGameOverPn;//게임이 끝나면 나오는 오브젝트
    public TextMeshProUGUI tmpGameOver;//게임이 끝나면 결과를 알려주는 텍스트
    public testojt to1;

    public void DropDown()//드롭다운 할때 사용함 지금은 꺼둔상태 (게임메니저의 인덱스를 보고 소환하는게 더 편하기 때문)
    {
       // MonLevel = Dropdown.value;
    }

    void Start()
    {
        listDataCsvdic = CSVReader.Read("UnitStateCSV");
        Screen.SetResolution(1440, 2960, true);

    

        for (var i = 1; i < listDataCsvdic.Count; i++)//CSV파일의 능력치 담아주기
        {
         
            listCsvHealthPoint.Add(float.Parse(listDataCsvdic[i]["Hp"]));
            listCsvAttackPower.Add(float.Parse(listDataCsvdic[i]["Power"]));
            listCsvDefend.Add(float.Parse(listDataCsvdic[i]["Defense"]));
            listCsvAttackSpeed.Add(float.Parse(listDataCsvdic[i]["Attack_Speed"]));
            listCsvCriticalPower.Add(float.Parse(listDataCsvdic[i]["Critical_Power"]));
            listCsvAttackRange.Add(float.Parse(listDataCsvdic[i]["Attack_Range"]));
            listCsvMoveSpeed.Add(float.Parse(listDataCsvdic[i]["Move_Speed"]));
            listCsvCriticalPercent.Add(float.Parse(listDataCsvdic[i]["Critical_Percent"]));
            listCsvSkillDuration.Add(float.Parse(listDataCsvdic[i]["Duration"]));
            listCsvSkillRadius.Add(float.Parse(listDataCsvdic[i]["Skill_Radius"]));
            listCsvSkillPower1.Add(float.Parse(listDataCsvdic[i]["Skill_Power1"]));
            listCsvSkillPower2.Add(float.Parse(listDataCsvdic[i]["Skill_Power2"]));
            listCsvSkillCoolTime.Add(float.Parse(listDataCsvdic[i]["Skill_Cooltime"]));
            listCsvSkillEffect1.Add(float.Parse(listDataCsvdic[i]["Skill_Effect1"]));
            listCsvSkillEffect2.Add(float.Parse(listDataCsvdic[i]["Skill_Effect2"]));

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))//인 게임 화면에서 하얀 버튼을 누르고 몬스터Index와 레벨을 적은 뒤 Enter를 눌렀을 경우 소환
        {
            if (infiMonsterLevel.text != "")//입력받은 숫자가 있다면 입력받은 데이터로 소환하세요
            {
                 int monsterIndex = int.Parse(infiMonsterIndex.text);//입력받은 몬스터를 사용하기 위한 변수

                //int MomIndex = Dropdown.value;
                int monsterlevel = int.Parse(infiMonsterLevel.text);//입력받은 몬스터의 Level수치를 적용하기 위한 변수
                GameObject objUnitSummon;//게임메니저의 적 혹은 아군 리스트에 넣어주기 위한 오브젝트/ 입력받은 몬스터의 레벨도 넣어줌

                objUnitSummon = Instantiate(listUnitobj[monsterIndex], objCharbtn.transform.position, Quaternion.identity);
                objUnitSummon.GetComponent<MonsterPropertieScript>().myLevel = monsterlevel;

                if(objUnitSummon.transform.position.y > 0) // y값보다 위에 소환되었다면 적
                {
                    listEnemyUnitObj.Add(objUnitSummon);
                    listCashingEnemyUnitScript.Add(objUnitSummon.GetComponent<MonsterPropertieScript>());

                }
                else // y값보다 밑에 소환되었다면 아군
                {
                    listMyUnitObj.Add(objUnitSummon);
                    listCashingMyUnitScript.Add(objUnitSummon.GetComponent<MonsterPropertieScript>());

                }

                infiMonsterLevel.text = "";
                infiMonsterIndex.text = "";
                infiMonsterLevel.gameObject.SetActive(false);
                infiMonsterIndex.gameObject.SetActive(false);

                objCharbtn.gameObject.SetActive(false);
                unitNumber = 1;

            }

           
        }

    }

    private void FixedUpdate()
    {
        if (isStartedBattle == true)//게임이 시작된 상태에서 몹들이 죽었으면 게임 종료 판단
        {

            if (listMyUnitObj.Count <= 0 && listEnemyUnitObj.Count > 0)//내팀이 다 죽고 적팀이 살아있다면
            {
                if (countGameOver > 1)
                {
                    objGameOverPn.gameObject.SetActive(true);
                    tmpGameOver.text = "Enemy Win!";
                    to1.ReadCSV(0,1, myTeamUnitScroe, enemyTeamUnitScore, 0, 1);
                    isStartedBattle = false;
                    countGameOver = 0;
                    StartCoroutine(CoRestartAuto());
                }

                else
                {
                    countGameOver += Time.deltaTime;
                  
                }
            }

            else if (listEnemyUnitObj.Count <= 0 && listMyUnitObj.Count > 0)//적팀이 다 죽고 내팀이 살아 있다면
            {
                if (countGameOver > 1)
                {
                    objGameOverPn.gameObject.SetActive(true);
                    tmpGameOver.text = "MyTeam Win!";
                    to1.ReadCSV(1, 0,myTeamUnitScroe,enemyTeamUnitScore,1,0);
                    isStartedBattle = false;
                    countGameOver = 0;

                    StartCoroutine(CoRestartAuto());
                }

                else
                {
                    countGameOver += Time.deltaTime;
                   
                }
            }


            else if (listEnemyUnitObj.Count <= 0 && listMyUnitObj.Count <= 0)//아군도 죽고 적군도 다 죽었으면 비김
            {
                if (countGameOver > 1)
                {
                    objGameOverPn.gameObject.SetActive(true);
                    tmpGameOver.text = "Drow!";
                    to1.ReadCSV(0,0, myTeamUnitScroe, enemyTeamUnitScore, 0, 0);
                    isStartedBattle = false;
                    countGameOver = 0;
                    StartCoroutine(CoRestartAuto());
                }

                else
                {
                    countGameOver += Time.deltaTime;
              
                }

            }

        }

        for(int i = 0; i<listCashingMyUnitScript.Count;i++)//죽었으면 바로 우리팀 리스트 크기 줄여주기
        {
            if (listMyUnitObj[i] == null)
            {
                listMyUnitObj.RemoveAt(i);
            }

            if (listCashingMyUnitScript[i] == null)
            {
                listCashingMyUnitScript.RemoveAt(i);
            }
            
        }

        for(int z = 0; z < listCashingEnemyUnitScript.Count; z++)//죽었으면 바로 적 리스트 줄여주기
        {
            if (listEnemyUnitObj[z]==null)
            {
                listEnemyUnitObj.RemoveAt(z);
            }

            if (listCashingEnemyUnitScript[z]==null)
            {
                listCashingEnemyUnitScript.RemoveAt(z);
            }

        }

    }

   

    public void StartBattle()//게임시작
    {

        isStartedBattle = true;
        
    

        if(unitNumber == 0)//유닛을 하나도 소환하지 않은채로 게임시작 버튼을 눌렀다면 자동 랜덤으로 12개의 몬스터를 생성한다.
        {

            unitNumber = 12;

            for (int i = 0; i < unitNumber; i++)//자동으로 12개 생성을 위한 For문
            {

                int characterRandom = Random.Range(0, unitType);//우리팀 캐릭 랜덤소환을 위한 변수
                int enemyRandom = Random.Range(0, unitType);//적 캐릭 랜덤소환을 위한 변수

                listFinalUnitNumber.Add(characterRandom);
                listEnemyFinalUnitNumber.Add(enemyRandom);

                GameObject objEnemyUnit;//Instantiate를 캐싱으로 넣어주기 위한 임시 변수
                GameObject objMyUnit;//Instantiate로 소환된 몹을 캐싱하여 담아주기 위한 임시 변수

                objMyUnit = Instantiate(listUnitobj[listFinalUnitNumber[i]], listMyUnitSitobj[i].transform.position, Quaternion.identity);
                objEnemyUnit = Instantiate(listUnitobj[listEnemyFinalUnitNumber[i]], listEnemyUnitSitobj[i].transform.position, Quaternion.identity);

                listMyUnitObj.Add(objMyUnit);
                listEnemyUnitObj.Add(objEnemyUnit);

                listCashingMyUnitScript.Add(objMyUnit.GetComponent<MonsterPropertieScript>());
                listCashingEnemyUnitScript.Add(objEnemyUnit.GetComponent<MonsterPropertieScript>());

                listCashingMyUnitScript[i].nowState = State.Walk;
                listCashingEnemyUnitScript[i].nowState = State.Walk;
            }

        }


        if (listCashingEnemyUnitScript.Count == listCashingMyUnitScript.Count)//적과 내 아군의 수가 같다면 
        {

            for (int i = 0; i < unitNumber; i++)//몬스터들의 거리 Distance의 List크기 늘려주기 유닛들의 적 List를 넣어주기
            {
                for (int j = 0; j < unitNumber; j++)
                {

                    listCashingMyUnitScript[i].listMyEnemyPosDistance.Add(0);
                    listCashingEnemyUnitScript[i].listMyEnemyPosDistance.Add(0);

                    listCashingMyUnitScript[i].listMyEnemyobj.Add(listEnemyUnitObj[j]);
                    listCashingEnemyUnitScript[i].listMyEnemyobj.Add(listMyUnitObj[j]);

                }

            }
        }

        else //유저가 선택하여 임의로 몬스터를 소환했을 경우 List를 따로따로 넣어줘야함
        {
            for (int i = 0; i < listCashingMyUnitScript.Count; i++)//아군들에게 적들의 리스트 넣어주기
            {
                
                for (int j = 0; j < listCashingEnemyUnitScript.Count; j++)
                {
                    listCashingMyUnitScript[i].listMyEnemyPosDistance.Add(0);
                    listCashingMyUnitScript[i].listMyEnemyobj.Add(listEnemyUnitObj[j]);

                }

            }

            for (int i = 0; i < listCashingEnemyUnitScript.Count; i++)
            {
              
                for (int j = 0; j < listCashingMyUnitScript.Count; j++)//적들에게 아군의 리스트 넣어주기
                {
                    listCashingEnemyUnitScript[i].listMyEnemyPosDistance.Add(0);
                    listCashingEnemyUnitScript[i].listMyEnemyobj.Add(listMyUnitObj[j]);
                   
                }
            }
        }

        for(int i = 0; i< arrCharButtonobj.Length; i++)//게임이 시작되면 모든 버튼 꺼주기
        {
            arrCharButtonobj[i].gameObject.SetActive(false);
        }
        StartCoroutine(PlusUnitScore());

        infiMonsterIndex.gameObject.SetActive(false);
        infiMonsterLevel.gameObject.SetActive(false);

    }


    public void SummonUnitTwentyTwo()//유닛 바로 12개생성
    {
        if(unitNumber < 12 && unitNumber > 0) // PlusOne을 한번이라도 했을경우 12개에서 소환된 유닛만큼 제외하여 최종적으로 12개의 유닛만 소환을 위한 if문
        {
            for(int i = unitNumber; i < 12; i++)//12개 소환을 위한 For문
            {
                int characterRandom = Random.Range(0, unitType);//캐릭터를 랜덤소환 하기 위한 변수
                int enemyRandom = Random.Range(0, unitType);//적 캐릭터를 랜덤소환 하기 위한 변수

                listFinalUnitNumber.Add(characterRandom);
                listEnemyFinalUnitNumber.Add(enemyRandom);

                GameObject objEnemyUnit;//적 유닛을 캐싱하여 쓰기 위한 변수
                GameObject objMyUnit;//내 유닛을 캐싱하여 쓰기 위한 변수

                objMyUnit = Instantiate(listUnitobj[listFinalUnitNumber[i]], listMyUnitSitobj[i].transform.position, Quaternion.identity);
                objEnemyUnit = Instantiate(listUnitobj[listEnemyFinalUnitNumber[i]], listEnemyUnitSitobj[i].transform.position, Quaternion.identity);

                listMyUnitObj.Add(objMyUnit);
                listEnemyUnitObj.Add(objEnemyUnit);

                listCashingMyUnitScript.Add(objMyUnit.GetComponent<MonsterPropertieScript>());
                listCashingEnemyUnitScript.Add(objEnemyUnit.GetComponent<MonsterPropertieScript>());
            }

          
        }

        unitNumber = 12;

        if (unitNumber == 12 && listFinalUnitNumber.Count < 12)//아무 소환도 안한 상태에서 한번에 12개 몹을 소환하기 위한 if문
        {

            for (int i = 0; i < unitNumber; i++)//12개 소환을 위한 for문
            {
                listMyUnitSitobj[i].SetActive(false);
                listEnemyUnitSitobj[i].SetActive(false);

                int characterRandom = Random.Range(0, unitType);//캐릭터 랜덤 뽑기 위한 변수
                int enemyRandom = Random.Range(0, unitType);// 적 캐릭터 랜덤 뽑기 위한 변수

                listFinalUnitNumber.Add(characterRandom);
                listEnemyFinalUnitNumber.Add(enemyRandom);

                GameObject objEnemyUnit; //캐싱하기 위한 임시 변수
                GameObject objMyUnit;// 생성된 캐릭 캐싱하기 위한 임시 변수

                objMyUnit = Instantiate(listUnitobj[listFinalUnitNumber[i]], listMyUnitSitobj[i].transform.position, Quaternion.identity);
                objEnemyUnit = Instantiate(listUnitobj[listEnemyFinalUnitNumber[i]], listEnemyUnitSitobj[i].transform.position, Quaternion.identity);

                listMyUnitObj.Add(objMyUnit);
                listEnemyUnitObj.Add(objEnemyUnit);

                listCashingMyUnitScript.Add(objMyUnit.GetComponent<MonsterPropertieScript>());
                listCashingEnemyUnitScript.Add(objEnemyUnit.GetComponent<MonsterPropertieScript>());

            }

        }

        if (unitNumber == 12 && listFinalUnitNumber.Count == 12)//12개의 소환이 꽉 차있는 상태에서 나온 유닛들이 마음에 들지 않아서 12개의 유닛들을 다시 소환하고 싶을 때
        {
            for(int i = 0; i <unitNumber; i++)
            {
                Destroy(listMyUnitObj[i]);
                Destroy(listEnemyUnitObj[i]);
            }

            listEnemyUnitObj.RemoveRange(0, listEnemyUnitObj.Count);
            listMyUnitObj.RemoveRange(0, listMyUnitObj.Count);          
            listCashingMyUnitScript.RemoveRange(0, listCashingMyUnitScript.Count);
            listCashingEnemyUnitScript.RemoveRange(0, listCashingEnemyUnitScript.Count);
            listFinalUnitNumber.RemoveRange(0, listFinalUnitNumber.Count);
            listEnemyFinalUnitNumber.RemoveRange(0, listEnemyFinalUnitNumber.Count);

            for (int i = 0; i < unitNumber; i++)
            {

                int CharacterRandom = Random.Range(0, unitType);//랜덤뽑기
                int enemyRandom = Random.Range(0, unitType);// 적 랜덤뽑기

                listFinalUnitNumber.Add(CharacterRandom);
                listEnemyFinalUnitNumber.Add(enemyRandom);

                GameObject objEnemyUnit;//생성된 유닛 캐싱하기 위한 임시 변수
                GameObject objMyUnit;//생성된 유닛 캐싱하기 위한 임시 변수

                objMyUnit = Instantiate(listUnitobj[listFinalUnitNumber[i]], listMyUnitSitobj[i].transform.position, Quaternion.identity);
                objEnemyUnit = Instantiate(listUnitobj[listEnemyFinalUnitNumber[i]], listEnemyUnitSitobj[i].transform.position, Quaternion.identity);

                listMyUnitObj.Add(objMyUnit);
                listEnemyUnitObj.Add(objEnemyUnit);

                listCashingMyUnitScript.Add(objMyUnit.GetComponent<MonsterPropertieScript>());
                listCashingEnemyUnitScript.Add(objEnemyUnit.GetComponent<MonsterPropertieScript>());


            }

        }

    }

    public void AddUnitPlus()//유닛하나씩 생성
    {
        if (unitNumber < 12)//12개의 유닛보다 적다면 계속 소환
        {
            unitNumber += 1;
            listMyUnitSitobj[unitNumber - 1].SetActive(false);
            listEnemyUnitSitobj[unitNumber - 1].SetActive(false);
        }

        for(int i = 0; i < unitNumber; i++)//유닛소환
        {
            if(listFinalUnitNumber.Count < i + 1 )//12개의 이상의 유닛을 제어하기 위한 if문
            {

                int characterRandom = Random.Range(0, unitType);//팀 유닛 랜덤 생성
                int enemyRandom = Random.Range(0, unitType);//적 유닛 랜덤 생성

                listFinalUnitNumber.Add(characterRandom);
                listEnemyFinalUnitNumber.Add(enemyRandom);

                GameObject objEnemyUnit;//소환된 유닛 캐싱하여 사용하기 위한 임시 변수
                GameObject objMyUnit;//소환된 유닛 캐싱하여 사용하기 위한 임시 변수

                objMyUnit = Instantiate(listUnitobj[listFinalUnitNumber[i]], listMyUnitSitobj[i].transform.position, Quaternion.identity);
                objEnemyUnit = Instantiate(listUnitobj[listEnemyFinalUnitNumber[i]], listEnemyUnitSitobj[i].transform.position, Quaternion.identity);

                listMyUnitObj.Add(objMyUnit);
                listEnemyUnitObj.Add(objEnemyUnit);

                listCashingMyUnitScript.Add(objMyUnit.GetComponent<MonsterPropertieScript>());
                listCashingEnemyUnitScript.Add(objEnemyUnit.GetComponent<MonsterPropertieScript>());

               
            }

        }

        

    }

    public void PlayFastBtn()
    {

        Time.timeScale = 5;
    }

    IEnumerator CoRestartAuto()
    {
        yield return new WaitForSeconds(1.5f);

        //SummonUnitTwentyTwo();

        //AssetDatabase.Refresh();


        for (int i = 0; i<listMyUnitObj.Count; i ++)
        {
            Destroy(listMyUnitObj[i]);
        }

        for(int i = 0; i < listEnemyUnitObj.Count;  i++)
        {
            Destroy(listEnemyUnitObj[i]);
        }

        unitNumber = 0;
        listMyUnitObj.RemoveRange(0, listMyUnitObj.Count);
        listEnemyUnitObj.RemoveRange(0, listEnemyUnitObj.Count);
        listCashingEnemyUnitScript.RemoveRange(0, listCashingEnemyUnitScript.Count);
        listCashingMyUnitScript.RemoveRange(0, listCashingMyUnitScript.Count);
        listFinalUnitNumber.Clear();
        listEnemyFinalUnitNumber.Clear();
        objGameOverPn.gameObject.SetActive(false);
        enemyTeamUnitScore = 0;
        myTeamUnitScroe = 0;
        StartBattle();
       

    }

    IEnumerator PlusUnitScore()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < listCashingMyUnitScript.Count; i++)
        {

            myTeamUnitScroe += listCashingMyUnitScript[i].MyScore;

        }

        for (int j = 0; j < listCashingEnemyUnitScript.Count; j++)
        {
            enemyTeamUnitScore += listCashingEnemyUnitScript[j].MyScore;

        }
    }

}
