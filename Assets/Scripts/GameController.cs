using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colorstate;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    //��¼�˴���ʼλ�ÿ�ʼ��ƫ��
    public float X_Offset = 0;
    public float Y_Offset = 0;

    //��¼��ÿ����Ԫ���С
    public float Unit_X = 1;
    public float Unit_Y = 1;

    //��¼�˳�����С
    public static int rowNum = 10;//����
    public static int colNum = 10;//����

    //��¼����Ҫ���ɷ��������,���ɵ�λ��
    public GameObject[] FallingBlocks;
    public float FallingPositin_X = 0;
    public float FallingPositin_Y = 0;

    //�������
    public static Transform[] Grid ;

    private List<List<ColorState>> m_GameManager;
    private List<List<bool>> m_CancelManager;

    //��Ϸ���еĲ���
    public float CD = 3;//��غ����������һ��


    private bool m_isfallen = false;
    private float m_timer = 0;
    void Start()
    {
        //���ȳ�ʼ��ɾ�������������ɫ��¼����Ϸ��������
        m_CancelManager = new List<List<bool>>();
        for(int row = 0;row < rowNum; row++)
        {
            List<bool> list = new List<bool>();
            for(int col = 0;col < colNum; col++)
            {
                list.Add(false);
            }
            m_CancelManager.Add(list);
        }

        m_GameManager = new List<List<ColorState>>();
        for (int row = 0; row < rowNum; row++)
        {
            List<ColorState> list = new List<ColorState>();
            for (int col = 0; col < colNum; col++)
            {
                list.Add(ColorState.None);
            }
            m_GameManager.Add(list);
        }

        //��ʼ��һ������
        RandomGenerateBlock();
    }

    // Update is called once per frame
    void Update()
    {
        //��һ���Ѿ����
        if (m_isfallen)
        {
            m_timer += Time.deltaTime;
            if(m_timer >= CD)//����CD
            {
                RandomGenerateBlock();
                m_isfallen=false;
                m_timer=0;
            }
        }
    }

    private void RandomGenerateBlock()
    {
        int index = Random.Range(0,FallingBlocks.Length);
        Instantiate(FallingBlocks[index],new Vector3(X_Offset+FallingPositin_X,Y_Offset+FallingPositin_Y,0),Quaternion.identity);

    }
}
