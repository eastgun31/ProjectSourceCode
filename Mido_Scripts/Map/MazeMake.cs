using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEngine.SceneManagement;

public class MazeMake : MonoBehaviour
{
    //맵 오브젝트
    [SerializeField]
    GameObject plane;   //바닥
    [SerializeField]
    GameObject wallPrefab;  //벽 프리펩
    [SerializeField]
    GameObject wallPrefab2; //히든방 벽 프리펩
    [SerializeField]
    GameObject doorPrefab;  //문 프리펩
    [SerializeField]
    GameObject mazepool;    //벽 오브젝트를 넣을 pool
    [SerializeField]
    GameObject insideWalls; //내벽
    [SerializeField]
    GameObject st2HiddenWall;//금간 벽
    //벽 텍스쳐 및 바닥 메테리얼
    [SerializeField]
    Texture2D[] stagetex;   
    [SerializeField]
    Texture2D[] normaltex;
    [SerializeField]
    Texture2D[] metaltex;
    [SerializeField]
    Material[] stageplane;
    [SerializeField]
    private PlayerMovement player;  //플레이어
    public GameObject rooms;
    public GameObject Hole;
    MazeRoomSetting roomset;
    BlockPooling blockPool;
    NavMeshSurface nav;
    bool[,] maze;   //plane의 사이즈만큼 배열을 생성, true 빈공간, false 벽생성
    int size;
    int wallcount = 0;
    GameManager gm;
    bool doorCreate;
    GameObject door;
    Dictionary<int, MazeBlocks> blockDic;

    private void Awake()
    {
        blockPool = mazepool.GetComponent<BlockPooling>();  // 벽 오브젝트 풀링 초기화
    }

    void Start()
    {
        //초기설정
        blockDic = new Dictionary<int, MazeBlocks>();
        gm = GameManager.instance;
        player = GameObject.FindAnyObjectByType<PlayerMovement>();
        roomset = rooms.GetComponent<MazeRoomSetting>();
        roomset._1RoomSettig();
        doorCreate = false;
        nav = plane.GetComponent<NavMeshSurface>();

        //스테이지1 미로생성
        ChangePlane(3.1f, 31f);
        CreateMazeBlocks();
        GenerateMaze();
        MakeMaze(maze);
        
        //스테이지1 텍스쳐 설정
        wallPrefab.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = stagetex[gm.stagelevel];
        wallPrefab.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_BumpMap", normaltex[gm.stagelevel]);
        wallPrefab.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MetallicGlossMap", metaltex[0]);
        gm.stagelevel = 1;

        ScoreManager.instance.SaveData();
    }

    void GenerateMaze()
    {
        size = ((int)plane.GetComponent<RectTransform>().rect.width);
        maze = new bool[size, size];
        // 격자 기반 초기화
        for (int i = 0;  i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {

                if (i % 2 == 0 || j % 2 == 0)   // 홀수 좌표에 빈공간 생성
                    maze[i, j] = false;
                else
                    maze[i, j] = true;
            }
        }
        // 통로 생성 로직
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (x % 2 == 0 || y % 2 == 0)
                    continue;

                if (x == size - 2 && y == size - 2)
                    continue;

                if (x == size - 2)
                {
                    maze[x, y + 1] = true;
                    continue;
                }

                if (y == size - 2)
                {
                    maze[x + 1, y] = true;
                    continue;
                }

                // 랜덤으로 통로 생성
                if (Random.Range(0, 2) == 0)
                    maze[x , y+1] = true;  // 오른쪽 뚫기
                else
                    maze[x+1, y] = true;  // 아래 뚫기
            }
        }

        for(int i = 2; i < 5; i++)
        {
            maze[size - i,size -2] = true;
            maze[size - i,size -3] = true;
        }

        // 스테이지별 빈공간 생성
        CreateEmpty(maze, size);

        maze[1,0] = true;

        if(gm.stagelevel == 2)
            maze[1, 0] = false;
    }

    void CreateEmpty(bool[,] maze, int size)    //스테이지마다 미로내 빈공간 생성 함수
    {
        int emptysize = size / 3;
        int emptysize2 = size / 4;
        int emptysize3 = size / 5;

        if (gm.stagelevel == 0) //스테이지1 미로 중앙 빈공간 생성
        {
            for (int i = emptysize; i < emptysize + 10; i++)
            {
                for (int j = emptysize; j < emptysize + 10; j++)
                {
                    maze[i, j] = true;
                }
            }
        }
        else if(gm.stagelevel == 1) //스테이지2 미로 5개 빈공간 생성
        {
            for (int i = emptysize3 * 2; i < emptysize3 * 2 + 7; i++)
            {
                for (int j = emptysize3 * 2; j < emptysize3 * 2 + 7; j++)
                {
                    maze[i, j] = true;
                }
            }

            for(int i = emptysize3-3; i < emptysize3 + 4; i++)
            {
                for(int j = emptysize3-3; j < emptysize3 + 4; j++)
                {
                    maze[i,j] = true;
                }

                for (int j = emptysize3*4 - 3; j < emptysize3*4 + 4; j++)
                {
                    maze[i, j] = true;
                }
            }

            for (int i = emptysize3*4 - 3; i < emptysize3*4 + 4; i++)
            {
                for (int j = emptysize3 - 3; j < emptysize3 + 4; j++)
                {
                    maze[i, j] = true;
                }

                for (int j = emptysize3 * 4 - 3; j < emptysize3 * 4 + 4; j++)
                {
                    maze[i, j] = true;
                }
            }

            maze[size - 1, 8] = true;
        }
        else if (gm.stagelevel == 2) //스테이지3 미로 9개 빈공간 생성
        {
            for (int i = 1; i < size - 1; i++)
            {
                maze[size - 3, i] = true;
                maze[i, size - 3] = true;
            }

            for (int i = emptysize3-3; i < emptysize3 + 4; i++)
            {
                for(int j = emptysize3-3; j < emptysize3 + 4; j++)
                {
                    maze[i, j] = true;
                }

                for (int j = emptysize3 * 2 + 1; j < emptysize3 * 2 + 8; j++)
                {
                    maze[i, j] = true;
                }

                for (int j = emptysize3*4-3; j < emptysize3*4 + 4; j++)
                {
                    maze[i, j] = true;
                }
            }

            for (int i = emptysize3 * 2 + 1; i < emptysize3 * 2 + 8; i++)
            {
                for (int j = emptysize3 - 3; j < emptysize3 + 4; j++)
                {
                    maze[i, j] = true;
                }

                for (int j = emptysize3 * 2 + 1; j < emptysize3 * 2 + 8; j++)
                {
                    maze[i, j] = true;
                }

                for (int j = emptysize3 * 4 - 3; j < emptysize3 * 4 + 4; j++)
                {
                    maze[i, j] = true;
                }
            }

            for (int i = emptysize3 * 4 - 3; i < emptysize3 * 4 + 4; i++)
            {
                for (int j = emptysize3 - 3; j < emptysize3 + 4; j++)
                {
                    maze[i, j] = true;
                }

                for (int j = emptysize3 * 2 + 1; j < emptysize3 * 2 + 8; j++)
                {
                    maze[i, j] = true;
                }

                for (int j = emptysize3 * 4 - 3; j < emptysize3 * 4 + 4; j++)
                {
                    maze[i, j] = true;
                }
            }

            maze[size/3, size - 1] = true;
        }
    }

    void CreateMazeBlocks() //초기 블록 생성 및 블록풀에 저장
    {
        for(int i = 0; i < 1400; i++)
        {
            blockDic.Add(i, blockPool.CreateObj());   
        }
    }

    void InitializationBlocks(int count)    //블록 초기화
    {
        for(int i = count; i < 1400; i++)
        {
            blockDic[i].blockInstalled = false;
            blockDic[i].OffBlock();
            blockDic[i].gameObject.transform.parent = mazepool.transform;
        }
    }

    void MakeMaze(bool[,] maze) // 미로 생성
    {
        float vsize = plane.GetComponent<RectTransform>().rect.width;

        int low = maze.GetLength(0);
        int col = maze.GetLength(1);
        float yPos = (wallPrefab.transform.localScale.y) / 2f;

        MazeBlocks wall = gameObject.AddComponent<MazeBlocks>();

        // 각 위치에서 벽 블록을 생성 또는 활성화
        //스테이지 크기에 따라 벽 블록들의 위치도 초기화
        for (int i = 0; i < low; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (!maze[i, j])
                {
                    if (blockDic.Count <= wallcount)    //벽 블록이 부족하면 벽 오브젝트를 생성
                    {
                        wall = blockPool.CreateObj();
                        blockDic.Add(wallcount, wall);
                        blockDic[wallcount].blockInstalled = true;
                    }
                    else    //아닐경우 pool에 있는 벽 블록 활성화 및 체크
                    {
                        blockPool.OnGet(blockDic[wallcount]);
                        wall = blockDic[wallcount];
                        blockDic[wallcount].blockInstalled = true;
                    }
                    wallcount++;
                    //블록 위치 이동
                    wall.gameObject.transform.position = new Vector3((i - (vsize / 2)) + .5f, yPos, (j - (vsize / 2)) + .5f);

                    wall.gameObject.transform.parent = mazepool.transform;

                    if(gm.stagelevel == 3)
                    {
                        if (i == 0 || j == 0 || i == low - 1 || j == col - 1)
                            wall.gameObject.transform.parent = mazepool.transform;
                        else
                            wall.gameObject.transform.parent = insideWalls.transform;
                    }

                    //벽 레이어를 나눠 부술수 있는지 없는지 구분
                    if (i == 0 || j == 0 || i == low - 1 || j == col - 1)
                        wall.gameObject.layer = 21;
                    else
                        wall.gameObject.layer = 14;
                }
                //히든 방 벽 생성
                if (gm.stagelevel == 2)
                {
                    if (i == low - 1 && j == 8)
                    {
                        st2HiddenWall = Instantiate(wallPrefab2);
                        st2HiddenWall.gameObject.transform.position = new Vector3((i - (vsize / 2)) + .5f, yPos, (j - (vsize / 2)) + .5f);
                        st2HiddenWall.gameObject.transform.parent = mazepool.transform;
                        st2HiddenWall.gameObject.layer = 14;
                    }

                }
                else if (gm.stagelevel == 3)
                {
                    if (i == 17 && j == col - 1)
                    {
                        GameObject wall2 = Instantiate(wallPrefab2);
                        wall2.gameObject.transform.position = new Vector3((i - (vsize / 2)) + .5f, yPos, (j - (vsize / 2)) + .5f);
                        wall2.gameObject.transform.parent = mazepool.transform;
                        wall2.gameObject.layer = 14;
                    }

                }
            }
        }

        if (!doorCreate)    //탈출구 생성
        {
            door = Instantiate(doorPrefab);
            doorCreate = true;
        }

        door.gameObject.transform.position = new Vector3((1 - (vsize / 2)) + 1.1f, 0, -(vsize / 2) + .5f);

        if (gm.stagelevel == 3)
        {
            door.SetActive(false);
            HiddenWallReset();
        }
            

        InitializationBlocks(wallcount);
    }

    void ChangePlane(float x, float y)  //스테이지 크기 마다 바닥 변경
    {
        plane.transform.localScale = new Vector3(x, 1, x);
        plane.GetComponent<RectTransform>().sizeDelta = new Vector2(y, y);
        plane.GetComponent<Renderer>().material = stageplane[gm.stagelevel];
    }

    public void ChangeMaze()    //미로 변경 함수
    {
        wallcount = 0;
        if (gm.stagelevel == 0) //스테이지1 미로 생성
        {
            wallPrefab.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = stagetex[gm.stagelevel];
            wallPrefab.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_BumpMap", normaltex[gm.stagelevel]);

            player.controller.enabled = false;
            player.gameObject.transform.position = gm.playerStartPos1;
            player.gameObject.transform.rotation = Quaternion.Euler(0, -140, 0);
            
            wallPrefab.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MetallicGlossMap", metaltex[0]);
            ChangePlane(3.1f, 31f);
            GenerateMaze();
            roomset._1RoomSettig();
            player.controller.enabled = true;
            gm.stagelevel = 1;
            MakeMaze(maze);
            gm.getkey = false;
        }
        else if (gm.stagelevel == 1)    //스테이지2 미로로 변경
        {
            wallPrefab.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = stagetex[gm.stagelevel];
            wallPrefab.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_BumpMap", normaltex[gm.stagelevel]);
            player.controller.enabled = false;
            player.gameObject.transform.position = gm.playerStartPos2;
            player.gameObject.transform.rotation = Quaternion.Euler(0, -140, 0);
            wallPrefab.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MetallicGlossMap", metaltex[0]);
            ChangePlane(4.1f, 41f);
            GenerateMaze();
            roomset._2RoomSettig();
            player.controller.enabled = true;

            gm.stagelevel = 2;
            MakeMaze(maze);
            gm.getkey = false;
        }
        else if (gm.stagelevel == 2)    //스테이지3 미로로 변경
        {
            wallPrefab.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = stagetex[gm.stagelevel];
            wallPrefab.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_BumpMap", normaltex[gm.stagelevel]);
            player.controller.enabled = false;
            player.gameObject.transform.position = gm.playerStartPos3;
            player.gameObject.transform.rotation = Quaternion.Euler(0, -140, 0);
            wallPrefab.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MetallicGlossMap", metaltex[1]);
            ChangePlane(5.1f, 51f);
            GenerateMaze();
            roomset._3RoomSettig();
            player.controller.enabled = true;
            gm.stagelevel = 3;
            MakeMaze(maze);
            nav.BuildNavMesh();
            gm.getkey = false;
        }
        else if (gm.stagelevel == 3)
            SceneManager.LoadScene(2);

        gm.quizeSolve = false;
    }

    public void PlayerPosReset()    //스테이지 시작시 플레이어 위치 초기화
    {
        if (gm.stagelevel == 1)
        {
            player.controller.enabled = false;
            player.gameObject.transform.position = gm.playerStartPos1;
            player.gameObject.transform.rotation = Quaternion.Euler(0, -140, 0);
            player.controller.enabled = true;
        }
        else if (gm.stagelevel == 2)
        {
            player.controller.enabled = false;
            player.gameObject.transform.position = gm.playerStartPos2;
            player.gameObject.transform.rotation = Quaternion.Euler(0, -140, 0);
            player.controller.enabled = true;
        }
        else if (gm.stagelevel == 3)
        {
            player.controller.enabled = false;
            player.gameObject.transform.position = gm.playerStartPos3;
            player.gameObject.transform.rotation = Quaternion.Euler(0, -140, 0);
            player.controller.enabled = true;
        }
        gm.quizeSolve = false;
        gm.quizOn = false;
    }

    //맵 오브젝트 초기화 함수들
    public void HoleReset()
    {
        BoxCollider holecol = Hole.GetComponent<BoxCollider>();
        holecol.enabled = true;
        Hole.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void HiddenWallReset()
    {
        st2HiddenWall.SetActive(false);
    }
}
