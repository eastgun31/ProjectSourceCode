using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MazeBlocks : MonoBehaviour 
{
    public bool blockInstalled = false; // 블록이 설치되었는지 여부를 나타내는 변수
    private IObjectPool<MazeBlocks> blockpool; // 객체 풀을 관리하는 인터페이스

    // 객체 풀 설정 메서드
    public void SetPool(IObjectPool<MazeBlocks> pool)
    {
        blockpool = pool; // 해당 블록이 속한 객체 풀을 설정
    }

    // 블록을 비활성화하는 메서드
    public void OffBlock()
    {
        gameObject.SetActive(false); // 블록의 GameObject를 비활성화
    }
}
