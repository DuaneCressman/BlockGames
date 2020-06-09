using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    //All nodes on the screen
    public List<Node> Nodes;
    public int nodeCount;

    //Constants
    private const int OVER_CROWDING = 3;
    private const int UNDER_POPULATION = 2;
    private const int CAN_SPAWN = 3;
    public float UpdateTimmer_Max = 2;
    private int GradiantSpeed = 25;

    //game components
    public GameObject PrefabNode;
    public GameObject PrefabNode_Marker;
    private Transform trans;
    private MarkerNode Marker;

    private float Scale;
    private float UpdateTimmer;
   
    //presets
    private KeyCode PressedKey;
    private List<MarkerNode> PresetPreviewMarkers;
    private bool[,] PreveiwPreset;
    private bool[,] Butterfly;
    private bool[,] LongLine;
    private bool[,] HorLine;
    private bool[,] Grid;
    private bool[,] Tumblr;
    private bool[,] Glider;
    private bool[,] BigGlider;
    private bool[,] SelectedPreset;


    //copy/paste tools
    private bool SelectionStarted;
    private Vector2Int FirstSelection;
    private int StepsTaken;
    private Vector2Int LastMarkerPosition;



    // Start is called before the first frame update
    void Start()
    {
        //set up the components
        trans = gameObject.GetComponent<Transform>();
        GOL_GameHandler.SetGameBoard(this);
        UpdateTimmer = UpdateTimmer_Max;
        Nodes = new List<Node>();

        SetupPresets();
        SelectedPreset = null;
        PresetPreviewMarkers = new List<MarkerNode>();

        //set up the marker
        Marker = CreateMarkerNode(new Vector2Int(0, 0));
        PreveiwPreset = null;
        StepsTaken = 0;
        Scale = 1;
    }




    // Update is called once per frame
    void Update()
    {
        //if the game is running 
        if (GOL_GameHandler.IsGameRunning())
        {
            //update the timmer
            UpdateTimmer += Time.deltaTime;

            if (UpdateTimmer > GOL_GameHandler.GetGameSpeed())
            {
                UpdateTimmer = 0;

                //do one step
                Step();
            }
        }

        //update the marker position
        UpdateMarker();
        CheckPresetPress();
        HandleSelection();
    }

    /* Method Name: GetInstructions
        * Parameters: void
        * Return: List<Instruction> - THe list of all the updates to the nodes needed for this step.
        * 
        * Purpose: This method will generate all of the instructions for the updates to the nodes.
        */
    private List<Instruction> GetInstructions()
    {
        //the output list
        List<Instruction> instructions = new List<Instruction>();

        //the nodes to check if they should come alive
        List<Vector2Int> deadToCheck = new List<Vector2Int>();

        foreach (Node node in Nodes)
        {
            //get the neighboring nodes
            List<Vector2Int> DeadNeighbors = GetDeadNeighbors(node.GetGamePos());

            //check if it should stay alive
            int aliveNabours = 8 - DeadNeighbors.Count;

            if (aliveNabours < UNDER_POPULATION || aliveNabours > OVER_CROWDING)
            {
                instructions.Add(new Instruction(false, node.GetGamePos()));
            }

            //add the dead naobors to a list to check if they should be alive
            foreach (Vector2Int dn in DeadNeighbors)
            {
                if (!deadToCheck.Contains(dn))
                {
                    deadToCheck.Add(dn);
                }
            }

        }

        foreach (Vector2Int n in deadToCheck)
        {
            if (8 - GetDeadNeighbors(n).Count == CAN_SPAWN)
            {
                instructions.Add(new Instruction(true, n));
            }
        }

        return instructions;
    }

    private void HandleSelection()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartSelection();
            GOL_GameHandler.PauseGame();
        }
        else if (SelectionStarted)
        {
            if (Input.GetMouseButtonUp(1))
            {
                StopSelection();
                GOL_GameHandler.ResumeGame();
            }
        }
    }

    private void SetupPresets()
    {
        Butterfly = new bool[5, 3];
        Butterfly[0, 0] = true;
        Butterfly[0, 1] = true;
        Butterfly[1, 0] = true;
        Butterfly[1, 1] = true;
        Butterfly[2, 2] = true;
        Butterfly[3, 0] = true;
        Butterfly[3, 1] = true;
        Butterfly[4, 0] = true;
        Butterfly[4, 1] = true;


        LongLine = new bool[1, 11];
        HorLine = new bool[11, 1];

        for (int i = 0; i < 10; i++)
        {
            LongLine[0, i] = true;
            HorLine[i, 0] = true;
        }

        Grid = new bool[10, 10];

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (i % 2 == 0)
                {
                    if (j % 2 == 0)
                    {
                        Grid[i, j] = true;
                    }
                }
                else
                {
                    if (j % 2 != 0)
                    {
                        Grid[i, j] = true;
                    }
                }
            }
        }

        Tumblr = new bool[7, 6];

        Tumblr[1, 0] = true;
        Tumblr[5, 0] = true;
        Tumblr[1, 1] = true;
        Tumblr[2, 1] = true;
        Tumblr[4, 1] = true;
        Tumblr[5, 1] = true;
        Tumblr[2, 2] = true;
        Tumblr[4, 2] = true;
        Tumblr[0, 3] = true;
        Tumblr[2, 3] = true;
        Tumblr[4, 3] = true;
        Tumblr[6, 3] = true;
        Tumblr[0, 4] = true;
        Tumblr[1, 4] = true;
        Tumblr[5, 4] = true;
        Tumblr[6, 4] = true;
        Tumblr[1, 5] = true;
        Tumblr[5, 5] = true;

        Glider = new bool[36, 7];
        Glider[0, 2] = true;
        Glider[0, 3] = true;
        Glider[1, 2] = true;
        Glider[1, 3] = true;
        Glider[6, 3] = true;
        Glider[7, 3] = true;
        Glider[8, 2] = true;
        Glider[8, 4] = true;
        Glider[9, 2] = true;
        Glider[9, 4] = true;
        Glider[9, 1] = true;
        Glider[9, 5] = true;
        Glider[10, 0] = true;
        Glider[10, 6] = true;
        Glider[11, 3] = true;
        Glider[12, 0] = true;
        Glider[12, 1] = true;
        Glider[12, 5] = true;
        Glider[12, 6] = true;
        Glider[22, 3] = true;
        Glider[22, 4] = true;
        Glider[23, 3] = true;
        Glider[23, 5] = true;
        Glider[24, 4] = true;
        Glider[24, 5] = true;
        Glider[26, 5] = true;
        Glider[27, 5] = true;
        Glider[27, 6] = true;
        Glider[28, 4] = true;
        Glider[28, 6] = true;
        Glider[34, 4] = true;
        Glider[34, 5] = true;
        Glider[35, 4] = true;
        Glider[35, 5] = true;

        BigGlider = new bool[11, 21];
        BigGlider[4, 0] = true;
        BigGlider[1, 1] = true;
        BigGlider[2, 1] = true;
        BigGlider[3, 1] = true;
        BigGlider[4, 1] = true;
        BigGlider[5, 1] = true;
        BigGlider[0, 2] = true;
        BigGlider[6, 2] = true;
        BigGlider[4, 3] = true;
        BigGlider[5, 3] = true;
        BigGlider[1, 4] = true;
        BigGlider[4, 4] = true;
        BigGlider[8, 5] = true;
        BigGlider[9, 5] = true;
        BigGlider[6, 6] = true;
        BigGlider[7, 6] = true;
        BigGlider[9, 6] = true;
        BigGlider[10, 6] = true;
        BigGlider[6, 7] = true;
        BigGlider[7, 7] = true;
        BigGlider[9, 7] = true;
        BigGlider[1, 8] = true;
        BigGlider[4, 8] = true;
        BigGlider[8, 8] = true;
        BigGlider[1, 9] = true;
        BigGlider[2, 9] = true;
        BigGlider[3, 9] = true;
        BigGlider[4, 9] = true;
        BigGlider[5, 9] = true;
        BigGlider[6, 9] = true;
        BigGlider[7, 9] = true;

        for (int i = 9; i >= 0; i--)
        {
            for (int j = 0; j < 11; j++)
            {
                BigGlider[j, 20 - i] = BigGlider[j, i];
            }
        }

    }

    private void StartSelection()
    {
        FirstSelection = GetMarkerPosition();
        SelectionStarted = true;
    }

    private void StopSelection()
    {
        Vector2Int SecondSelection = GetMarkerPosition();

        List<Vector2Int> nodes = new List<Vector2Int>();

        int MaxX = 0;
        int MinX = 0;
        int MaxY = 0;
        int MinY = 0;

        if (SecondSelection.x <= FirstSelection.x)
        {
            MaxX = FirstSelection.x;
            MinX = SecondSelection.x;
        }
        else
        {
            MinX = FirstSelection.x;
            MaxX = SecondSelection.x;
        }

        if (SecondSelection.y <= FirstSelection.y)
        {
            MaxY = FirstSelection.y;
            MinY = SecondSelection.y;
        }
        else
        {
            MinY = FirstSelection.y;
            MaxY = SecondSelection.y;
        }

        SelectedPreset = new bool[MaxX - MinX + 1, MaxY - MinY + 1];

        for (int i = 0; i < MaxX - MinX; i++)
        {
            for (int j = 0; j < MaxY - MinY; j++)
            {
                if (CheckNode(new Vector2Int(i + MinX, j + MinY)))
                {
                    SelectedPreset[i, j] = true;
                }
            }
        }
    }

    private MarkerNode CreateMarkerNode(Vector2Int inPos)
    {
        MarkerNode node = Instantiate(PrefabNode_Marker).GetComponent<MarkerNode>();
        node.transform.parent = this.gameObject.transform;
        node.gameObject.transform.position = new Vector3(inPos.x * Scale, inPos.y * Scale, 0) + this.gameObject.transform.position;
        node.SetGamePos(inPos);

        return node;
    }


    private List<MarkerNode> CreateMarkerNode(bool[,] inArray, Vector2Int bottomLeft)
    {
        List<MarkerNode> output = new List<MarkerNode>();

        for (int i = 0; i < inArray.GetLength(0); i++)
        {
            for (int j = 0; j < inArray.GetLength(1); j++)
            {
                if (inArray[i, j])
                {
                    output.Add(CreateMarkerNode(new Vector2Int(i + bottomLeft.x, j + bottomLeft.y)));
                }
            }
        }

        return output;
    }

    public void CheckPresetPress()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            PressedKey = KeyCode.B;
            PreveiwPreset = Butterfly;
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            PressedKey = KeyCode.L;
            PreveiwPreset = LongLine;
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            PressedKey = KeyCode.H;
            PreveiwPreset = HorLine;
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            PressedKey = KeyCode.G;
            PreveiwPreset = Grid;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            PressedKey = KeyCode.T;
            PreveiwPreset = Tumblr;
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            PressedKey = KeyCode.I;
            PreveiwPreset = Glider;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            PressedKey = KeyCode.O;
            PreveiwPreset = BigGlider;
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            if (SelectedPreset != null)
            {
                PressedKey = KeyCode.V;
                PreveiwPreset = SelectedPreset;
            }
        }

        if (Input.GetKeyUp(PressedKey))
        {
            if (PreveiwPreset != SelectedPreset || (PreveiwPreset == SelectedPreset && SelectedPreset != null))
            {
                foreach (MarkerNode node in PresetPreviewMarkers)
                {
                    CreateNode(node.GetGamePos());
                    Destroy(node.gameObject);
                }

                PresetPreviewMarkers.Clear();
                PreveiwPreset = null;
            }

            PressedKey = KeyCode.None;
        }
    }

    public Vector2Int GetMarkerPosition()
    {
        return Marker.GetGamePos();
    }

    public void MoveGameBoard(Vector3 movement)
    {
        trans.position += movement;
    }

    public void Step()
    {
        StepsTaken++;
        ExecuteInstructions(GetInstructions());
    }

    private void UpdateMarker()
    {
        Vector3 vec = (GOL_GameHandler.GetMousePosition() - trans.position) / Scale;

        float tempX = (float)(vec.x - Math.Truncate(vec.x));

        if (tempX > 0.5f)
        {
            tempX = (int)vec.x + 1;
        }
        else
        {
            tempX = (int)vec.x;
        }

        float tempY = (float)(vec.y - Math.Truncate(vec.y));

        if (tempY > 0.5f)
        {
            tempY = (int)vec.y + 1;
        }
        else
        {
            tempY = (int)vec.y;
        }


        Vector2Int NewGamePosition = new Vector2Int((int)tempX, (int)tempY);


        if (LastMarkerPosition != NewGamePosition)
        {
            Marker.transform.localPosition = new Vector3(tempX, tempY);
            Marker.SetGamePos(NewGamePosition);
            LastMarkerPosition = NewGamePosition;

            if (PressedKey != KeyCode.None)
            {
                foreach (MarkerNode node in PresetPreviewMarkers)
                {
                    Destroy(node.gameObject);
                }

                PresetPreviewMarkers.Clear();

                PresetPreviewMarkers = CreateMarkerNode(PreveiwPreset, Marker.GetGamePos());

            }
            else if (Input.GetMouseButton(0))
            {
                CreateMarker();
            }
        }
    }

    public void UpdateScale(float input)
    {
        Scale += input;

        gameObject.transform.localScale = new Vector3(Scale, Scale, Scale);
    }

    public void ResetBoard()
    {
        foreach (Node node in Nodes)
        {
            node.KillNode();
        }

        Nodes.Clear();
    }

    public void CreateMarker()
    {
        if (CheckNode(Marker.GetGamePos()))
        {
            DestroyNode(Marker.GetGamePos());
        }
        else
        {
            CreateNode(Marker.GetGamePos());
        }
    }

    private void DestroyNode(Vector2Int input)
    {
        Node toDestroy = null;

        foreach (Node node in Nodes)
        {
            if (node.GetGamePos() == input)
            {
                toDestroy = node;
                break;
            }
        }

        toDestroy.KillNode();
        Nodes.Remove(toDestroy);
    }

   

    private List<Vector2Int> GetDeadNeighbors(Vector2Int inPos)
    {
        List<Vector2Int> DeadNabours = new List<Vector2Int>();

        Vector2Int posToCheck;

        posToCheck = inPos + new Vector2Int(0, 1);
        if (!CheckNode(posToCheck))
        {
            DeadNabours.Add(posToCheck);
        }

        posToCheck = inPos + new Vector2Int(0, -1);
        if (!CheckNode(posToCheck))
        {
            DeadNabours.Add(posToCheck);
        }

        posToCheck = inPos + new Vector2Int(1, 0);
        if (!CheckNode(posToCheck))
        {
            DeadNabours.Add(posToCheck);
        }

        posToCheck = inPos + new Vector2Int(-1, 0);
        if (!CheckNode(posToCheck))
        {
            DeadNabours.Add(posToCheck);
        }

        posToCheck = inPos + new Vector2Int(-1, 1);
        if (!CheckNode(posToCheck))
        {
            DeadNabours.Add(posToCheck);
        }

        posToCheck = inPos + new Vector2Int(1, 1);
        if (!CheckNode(posToCheck))
        {
            DeadNabours.Add(posToCheck);
        }

        posToCheck = inPos + new Vector2Int(-1, -1);
        if (!CheckNode(posToCheck))
        {
            DeadNabours.Add(posToCheck);
        }

        posToCheck = inPos + new Vector2Int(1, -1);
        if (!CheckNode(posToCheck))
        {
            DeadNabours.Add(posToCheck);
        }

        return DeadNabours;
    }

    private bool CheckNode(Vector2Int input)
    {
        foreach (Node node in Nodes)
        {
            if (node.GetGamePos() == input)
            {
                return true;
            }
        }

        return false;
    }

    private void ExecuteInstructions(List<Instruction> instructions)
    {
        foreach (Instruction instruction in instructions)
        {
            if (instruction.Mode)
            {
                CreateNode(instruction.Position);
            }
            else
            {
                DestroyNode(instruction.Position);
            }
        }
    }

    public void CreateNode(Vector2Int inPos)
    {
        if (!CheckNode(inPos))
        {
            Node node = Instantiate(PrefabNode).GetComponent<Node>();
            node.transform.parent = this.gameObject.transform;
            node.GetComponentInChildren<MeshRenderer>().material.color = Assets.GOL_Scripts.Gradient.GetGradient(StepsTaken % GradiantSpeed, GradiantSpeed);
            node.gameObject.transform.position = new Vector3(inPos.x * Scale, inPos.y * Scale, 0) + this.gameObject.transform.position;
            node.SetGamePos(inPos);

            Nodes.Add(node);
        }
    }

    public void CreateNode(bool[,] inArray, Vector2Int bottomLeft)
    {
        for (int i = 0; i < inArray.GetLength(0); i++)
        {
            for (int j = 0; j < inArray.GetLength(1); j++)
            {
                if (inArray[i, j])
                {
                    CreateNode(new Vector2Int(i, j) + bottomLeft);
                }
            }
        }
    }

    public void CreateNode(List<Vector2Int> nodePos, Vector2Int bottomLeft)
    {
        foreach (Vector2Int v in nodePos)
        {
            CreateNode(v + bottomLeft);
        }
    }
}

public class Instruction
{

    public bool Mode; //true = create, false = kill
    public Vector2Int Position;

    public Instruction(bool mode, Vector2Int position)
    {
        Mode = mode;
        Position = position;
    }
}
