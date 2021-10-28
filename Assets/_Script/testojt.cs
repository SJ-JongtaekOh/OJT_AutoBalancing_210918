using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;
public class testojt : MonoBehaviour
{
    string filename = "";

    public TextAsset textAssset;

    //public Gam eObject temp;

    //public float a = 3;

    //public float b = 1.4f;

    //public List<GameObject> aaa = new List<GameObject>();
    //// Start is called before the first frame update

    //public GameObject tt;
    //public GameObject ss;

    ////public Vector2 v3Dest = new Vector2();
    ////public Vector2 v3Dest2 = new Vector2();
    ////public Vector2 v3Dest3 = new Vector2();
    ////public Vector2 v3Dest4 = new Vector2();
    ////public Vector2 v3Dest5 = new Vector2();

    ////public Vector2[] V3Final = new Vector2[4];

    //public List<float> ffaa = new List<float>();

    //float aa = 254;
    //float bb = 689;

    public StringBuilder sb = new StringBuilder();

    public int MyteamScroe;
    public int EnemyTeamScroe;

    void Start()
    {

        filename = Application.dataPath + "/test.csv";



        //for(int i = 0; i< 1000; i++)
        //{
        //    Debug.Log(Random.Range(0, 3));

        //}
        //sb.Append("도레미파솔라시도");

        //Debug.Log(sb);

        //  float cc =(aa / bb);
        //ffaa.Sort();

        //if(ss == ss1)
        //{
        //    Debug.Log("야");
        //}

        // Debug.Log(aa / bb);
        // float a = Mathf.Atan2(ss.transform.position.y - tt.transform.position.y, ss.transform.position.x - tt.transform.position.x) * 180 /Mathf.PI;

        // Vector2 v3Source = tt.transform.position;   // 중심이 되는 오브젝트        
        // Vector2 v3Distance = Vector2.up * 0.5f;    // 거리벡터(forward는 Y축 기준으로 앞을 보고있는 벡터. 필요에따라 수정 필요)
        // Vector2 v3Distance2 = Vector2.down * 0.5f;
        // Vector2 v3Distance3 = Vector2.right * 5.5f;
        // Quaternion qRotate = Quaternion.Euler(0f, 0, a);  // 회전할 각도(Z축 기준 회전. 필요에따라 수정 필요)

        // Vector2 v3TargetPoint = qRotate * v3Distance;        // 원점을 기준으로 거리와 각도를 연산한 후, 벡터
        // Vector2 v3TargetPoint2 = qRotate * v3Distance2;
        // Vector2 v3TargetPoint3 = qRotate * v3Distance3;


        // v3Dest =  v3Source + v3TargetPoint;    // 중심이 되는 오브젝트에서 해당 거리와 각도만큼 이동한 곳의 좌표
        // v3Dest2 = v3Source + v3TargetPoint2;
        // v3Dest3 = v3Source + v3TargetPoint3;

        // Instantiate(tt, v3Dest, qRotate);
        // Instantiate(tt, v3Dest2, qRotate);
        //// Instantiate(tt, v3Dest3, qRotate);

        // Vector2 v3Source2 = v3Dest3;       
        // float b = Mathf.Atan2(tt.transform.position.y - v3Dest3.y, tt.transform.position.x - v3Dest3.x) * 180 / Mathf.PI;

        // Quaternion BRotate = Quaternion.Euler(0f, 0, b);

        // Vector2 v3TargetPoint4 = BRotate * v3Distance;
        // Vector2 v3TargetPoint5 = BRotate * v3Distance2;

        // v3Dest4 = v3Source2 + v3TargetPoint4;
        // v3Dest5 = v3Source2 + v3TargetPoint5;

        // Instantiate(tt, v3Dest4, BRotate);
        // Instantiate(tt, v3Dest5, BRotate);

        // V3Final[0] = v3Dest;
        // V3Final[1] = v3Dest5;
        // V3Final[2] = v3Dest4;
        // V3Final[3] = v3Dest2;

        // polyCheck(ss.transform.position, V3Final);

    
     
  

    }

    //public void WriteCSV(int _myteamWinCsv,int _enemyTeamCsv, int _myTeamPlusScore, ,int _enemyTeamPlusScore, int _XX, int _YY, string []data)
    public void WriteCSV(int _myteamWinCsv, int _enemyTeamCsv, int _myTeamPlusScore, int _enemyTeamPlusScore,int _a,int _b, int _XX, int _YY, string[] data)
    {
        int readMyTeamCsv = _myteamWinCsv;
        int readEnemyTeamCsv = _enemyTeamCsv;
        //string ass = aa.ToString();
        //string abb = bb.ToString();

        readMyTeamCsv += _myTeamPlusScore;
        readEnemyTeamCsv += _enemyTeamPlusScore;

        string addTeamScore = readMyTeamCsv.ToString();
        string AddEnemyScore = readEnemyTeamCsv.ToString();

        string myTeamUnitScore = _a.ToString();
        string enemyTeamUnitScore = _b.ToString();

        
        string myTeamWin = _XX.ToString();
        string enemyTeamWin = _YY.ToString();



        //readMyTeamCsv

        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine("WeWin, EnemyWin, MyUnitScore, EnemyScore, MyTeamWin, EnemyTeamWin");
        tw.Close();
        tw = new StreamWriter(filename, true);
        tw.WriteLine(addTeamScore + "," + AddEnemyScore);
     
        for (int i = 0; i < data.Length; i++ )
        {
            if (i == data.Length -1)
            {        
                tw.WriteLine("," + "," + myTeamUnitScore + "," + enemyTeamUnitScore + "," + myTeamWin + "," + enemyTeamWin);    
            }

        }
        

        tw.Close();
    }

    public void ReadCSV(int _myTeamScore, int _enemyTeamScore, int _myTeamUnitScore, int _enemyTeamUnitScore, int _XX, int _YY)
    {
        string[] data = textAssset.text.Split(new string[] { ",", "\n", "\n", "\n", "\n", "\n", "\n" }, StringSplitOptions.None);


        int MyTeamWin = int.Parse(data[6]);
        int EnemyTeamWin = int.Parse(data[7]);

        //Debug.Log(data[0]);
        //Debug.Log(data[1]);
        //Debug.Log(data[2]);
        //Debug.Log(data[3]);
        //Debug.Log(data[4]);
        //Debug.Log(data[5]);
 
        //for(int i = 0; i<)

        WriteCSV(MyTeamWin, EnemyTeamWin, _myTeamScore, _enemyTeamScore,_myTeamUnitScore, _enemyTeamUnitScore, _XX, _YY,  data);



    }

    //private void Update()
    //{

    //    if(Input.GetMouseButtonDown(0))
    //    {
    //        //   polyCheck(ss.transform.position,V3Final);

    //        AAA();
    //    }

    //}

    public void polyCheck(Vector2 v, Vector2[] p)
    {
        int crosses = 0;
        for (int i = 0; i < p.Length; i++)
        {
            int j = (i + 1) % p.Length;


          
            //점 B가 선분 (p[i], p[j])의 y좌표 사이에 있음
            if ((p[i].y > v.y) || (p[j].y > v.y))
            {
                //atX는 점 B를 지나는 수평선과 선분 (p[i], p[j])의 교점
                double atX = (p[j].x - p[i].x) * (v.y - p[i].y) / (p[j].y - p[i].y) + p[i].x;
                //atX가 오른쪽 반직선과의 교점이 맞으면 교점의 개수를 증가시킨다.
                if (v.x < atX)
                    crosses++;

            }
        }

     //   Debug.Log(crosses);
      
    }

    // Update is called once per frame

    //public void ddd(Vector2 B, Vector2 C, Vector2 D)
    //{
    //    int crosses = 0;

    //    for (int i = 0; i < 2; i++)
    //    {
    //        int j = (i + 1) % 4;
    //        //점 B가 선분 (p[i], p[j])의 y좌표 사이에 있음
    //        if ((C.y > B.y) != (D.y > B.y))
    //        {               
    //            //atX는 점 B를 지나는 수평선과 선분 (p[i], p[j])의 교점
    //            double atX = (C.x - D.x) * (B.y - D.y) / (C.y - D.y) + D.x;

    //            Debug.Log(atX);
    //            Debug.Log(B.x);
    //            //atX가 오른쪽 반직선과의 교점이 맞으면 교점의 개수를 증가시킨다.
    //            if (B.x < atX)
    //                crosses++;
    //        }
    //    }

    //   if( crosses % 2 > 0)
    //   {
    //        Debug.Log("내부에있음");
    //   }

    //   // Debug.Log(crosses);

    //}


   
}

