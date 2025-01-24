using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// 블록 풀링을 관리하는 클래스
public class BlockPooling : MonoBehaviour {
    // MazeBlocks 타입의 오브젝트 풀
    public IObjectPool pool;

    // 생성할 블록의 프리팹
    [SerializeField] MazeBlocks blockPrefab;

    // 초기화 메서드
    private void Awake()
    {
        // 오브젝트 풀 초기화: 생성, 가져오기, 반환, 파괴 메서드와 최대 크기 설정
        pool = new ObjectPool(CreateObj, OnGet, OnRelease, DestroyBlocks, maxSize: 1400);
    }

    // 새로운 MazeBlocks 오브젝트 생성 메서드
    public MazeBlocks CreateObj()
    {
        // 프리팹을 인스턴스화하고 MazeBlocks 컴포넌트를 가져옴
        MazeBlocks _obj = Instantiate(blockPrefab).GetComponent();
        // 생성된 오브젝트에 풀을 설정
        _obj.SetPool(pool);
        return _obj; // 생성된 오브젝트 반환
    }

    // 오브젝트를 풀에서 가져올 때 호출되는 메서드
    public void OnGet(MazeBlocks _obj)
    {
        // 오브젝트를 활성화
        _obj.gameObject.SetActive(true);
    }

    // 오브젝트를 풀에 반환할 때 호출되는 메서드
    public void OnRelease(MazeBlocks _obj)
    {
        // 오브젝트를 비활성화
        _obj.gameObject.SetActive(false);
    }

    // 오브젝트를 파괴할 때 호출되는 메서드
    public void DestroyBlocks(MazeBlocks _obj)
    {
        // 오브젝트를 파괴
        Destroy(_obj.gameObject);
    }
}