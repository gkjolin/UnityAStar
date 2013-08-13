using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathManager : MonoBehaviour 
{
	private static PathManager _mainManager;
	public static PathManager MainManager
	{ 
		get
		{
			if(_mainManager == null)
			{
				_mainManager = GetMainManager();
			}
			return _mainManager;
		} 
	}
	
	public Material[] sharedNodeMaterials;
	public int fieldWidth, fieldHeight;
	public float nodeSize;
	public Vector3 creationStartPoint = Vector3.zero;
	
	private Dictionary<int, PathNode> nodeCollection;
	[HideInInspector][SerializeField]private float gridNodeSize = 0;
	[HideInInspector][SerializeField]private int gridWidth = 0;
	[HideInInspector][SerializeField]private int gridHeight = 0;
	[HideInInspector][SerializeField]private PathNode[] nodeGrid;
	private int currentNodeIndex = 0;
	
	private void Awake()
	{
		PathWeights.WakeUp();
	}
	
	static private PathManager GetMainManager()
	{
		if(_mainManager != null){ return _mainManager; }
		_mainManager = (PathManager)FindObjectOfType(typeof(PathManager));
		if(_mainManager == null)
		{
			GameObject go = new GameObject("PathManager");
			_mainManager = go.AddComponent<PathManager>();
		}
		return _mainManager;
	}
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(a != null && b != null)
			{
				//Debug.Log(Heuristics.ManhattanDistance(a.X, a.Y, 0, b.X, b.Y, 0));
				GetPath(a, b, PathFollowerTypes.foot);
			}
		}
	}
	
	private int GetNewNodeID()
	{
		if(currentNodeIndex == 0)
		{
			FindOldField();
		}
		currentNodeIndex++;
		return currentNodeIndex - 1;
	}
	
	public void CreateField(int width, int height, float nodeSize, Vector3 startPosition)
	{
		RemoveOldField();
		PathNode node = null;
		GameObject nodeGO = new GameObject("Nodes");
		Transform parent = nodeGO.transform;
		GameObject defaultNode = GameObject.CreatePrimitive(PrimitiveType.Quad);
		defaultNode.name = "Temporary node";
		defaultNode.AddComponent<PathNode>();
		Vector3 position = startPosition;
		Quaternion rotation = Quaternion.Euler(90, 0, 0);
		gridWidth = width;
		gridHeight = height;
		gridNodeSize = nodeSize;
		nodeGrid = new PathNode[width * height];
		nodeCollection = new Dictionary<int, PathNode>();
		
		for(int x = 0; x < width; x++)
		{
			position.x += nodeSize;
			for(int y = 0; y < height; y++)
			{
				var id = GetNewNodeID();
				position.z += nodeSize;
				nodeGO = (GameObject)Instantiate(defaultNode, position, rotation);
				nodeGO.transform.parent = parent;
				nodeGO.name = "Node "+x+"-"+y;
				node = nodeGO.GetComponent<PathNode>();
				node.Initialize(id, x, y, PathType.road);
				nodeGrid[y * gridWidth + x] = node;
				nodeCollection.Add(node.ID, node);
			}
			position.z = startPosition.z;
		}
		
		if(Application.isPlaying){ Destroy(defaultNode); }
		else{ DestroyImmediate(defaultNode); }
		
	}
	
	private void FindOldField()
	{
		var oldField = GameObject.FindObjectsOfType(typeof(PathNode));
		if(oldField == null){ return; }
		if(oldField.Length == 1 && oldField[0].name == "Temporary node"){ return; }
		currentNodeIndex = 0;
		nodeCollection = new Dictionary<int, PathNode>();
		PathNode node = null;
		int maxX = 0, maxY = 0;
		for(int i = 0; i < oldField.Length; i++)
		{
			node = (PathNode)oldField[i];
			if(nodeCollection.ContainsKey(node.ID))
			{
				Debug.LogWarning(	"Double node found! Recommend to remove the old field and create a new field. " +
									"Double node will be destroyed.",node.transform.parent);
				if(Application.isPlaying){ Destroy(node.gameObject); }
				else{ DestroyImmediate(node.gameObject); }
				continue;
			}
			maxX = Mathf.Max(maxX, node.X + 1);
			maxY = Mathf.Max(maxY, node.Y + 1);
			nodeCollection.Add(node.ID, node);
			if(node.ID >= currentNodeIndex){ currentNodeIndex = node.ID + 1; }
		}
		nodeGrid = DictionaryToGrid(nodeCollection, maxX, maxY);
	}
	
	private PathNode[] DictionaryToGrid(Dictionary<int, PathNode> dictionary, int width, int height)
	{
		if(dictionary == null || width == 0 || height == 0){ return new PathNode[0]; }
		PathNode[] grid = new PathNode[width * height];
		PathNode node;
		foreach(KeyValuePair<int, PathNode> kvp in dictionary)
		{
			node = kvp.Value;
			grid[node.Y * gridWidth + node.X] = node;
		}
		return grid;
	}
	
	private Dictionary<int, PathNode> GridToDictionary(PathNode[] grid)
	{
		if(grid == null || grid.Length == 0)
		{
			return new Dictionary<int, PathNode>();
		}
		Dictionary<int, PathNode> dict = new Dictionary<int, PathNode>();
		var length = grid.Length;
		for(int i = 0; i < length; i++)
		{
			if(dict.ContainsKey(grid[i].ID))
			{
				Debug.LogWarning(	"Double node found! Recommend to remove the old field and create a new field. " +
									"Double node will be destroyed.", grid[i].transform.parent);
				if(Application.isPlaying){ Destroy(grid[i].gameObject); }
				else{ DestroyImmediate(grid[i].gameObject); }
				continue;
			}
			dict.Add(grid[i].ID, grid[i]);
		}
		return dict;
	}
	
	public void RemoveOldField()
	{
		currentNodeIndex = 0;
		var oldField = GameObject.FindObjectsOfType(typeof(PathNode));
		if(oldField == null){ return; }
		for(int i = 0; i < oldField.Length; i++)
		{
			if(oldField[i] == null){ continue; }
			GameObject go = ((PathNode)oldField[i]).gameObject;
			if(Application.isPlaying){ Destroy(go.transform.parent.gameObject); }
			else{ DestroyImmediate(go.transform.parent.gameObject); }
			
			if(oldField[i] == null || go == null){ continue; }
			if(Application.isPlaying){ Destroy(go); }
			else{ DestroyImmediate(go); }
		}
		nodeGrid = new PathNode[0];
		nodeCollection = new Dictionary<int, PathNode>();
		gridWidth = 0;
		gridHeight = 0;
		//if(Application.isPlaying){ Destroy(oldField); }
		//else{ DestroyImmediate(oldField); }
	}
	
	public PathNode GetNodeWithID(int id)
	{
		if(nodeCollection.ContainsKey(id))
		{
			return nodeCollection[id];
		}
		return null;
	}
	
	public PathNode GetNode(int x, int y)
	{
		if(x < 0 || x >= gridWidth || y < 0 || y >= gridHeight){ return null; }
		int i = y * gridWidth + x;
		if(i < nodeGrid.Length)
		{
			return nodeGrid[i];
		}
		return null;
	}
	
	public PathNode[] GetConnections(PathNode node)
	{
		int x = node.X;
		int y = node.Y;
		var connections = new PathNode[4];
		if(x > 0)
		{
			connections[0] = nodeGrid[y * gridWidth + (x - 1)];
		}
		if(x < gridWidth - 1)
		{
			connections[1] = nodeGrid[y * gridWidth + (x + 1)];
		}
		if(y > 0)
		{
			connections[2] = nodeGrid[(y - 1) * gridWidth + x];
		}
		if(y < gridHeight - 1)
		{
			connections[3] = nodeGrid[(y + 1) * gridWidth + x];
		}
		return connections;
	}
	
	static public PathNode a, b;
	public void GetPath(PathNode start, PathNode end, PathFollowerTypes type, Action<PathData> callback)
	{
		Debug.Log("Start = "+start.name +" End = "+end.name);	
		var endPos = end.GridPosition;
		var closedSet = new List<PathNode>();
		var openSet = new List<PathNode>();
		openSet.Add(start);
		var navigatedSet = new List<PathNode>();
		var pathWeight = new List<int>();
		List<float> score = new List<float>();
		
		float estimatedScore = 0;
		var openConnections = new PathNode[0];
		var iWeights = PathWeights.GetWeights(type);
		float[] weights = new float[iWeights.Count];
		for(int i = 0; i < weights.Length; i++)
		{
			weights[i] = iWeights[(PathType)i] * gridNodeSize;
		}
		bool endIsFound = false;
		for(int i = 0; i < openSet.Count; i++)
		{
			if(openSet[i] == end)
			{
				endIsFound = true;
				break;
			}
			Debug.Log("Checking connections of: "+ openSet[i].name, openSet[i].gameObject);
			openConnections = GetConnections(openSet[i]);
			int index = 0;
			float distance = 100000;
			bool foundNewNode = false;
			bool[] newConnections = new bool[4];
			bool hasConnectionsAvailable = false;
			for(int c = 0; c < 4; c++)
			{
				if(navigatedSet.Contains(openConnections[c])){continue;}
				newConnections[c] = true;
				hasConnectionsAvailable = true;
			}
			if(!hasConnectionsAvailable){ continue; }
			for(int c = 0; c < 4; c++)
			{
				if(openConnections[c] == null || !newConnections[c]){ continue; }
				if(openConnections[c] == end)
				{
					distance = 0;
					index = c;
					foundNewNode = true;
					break;
				}
				
				//Debug.Log((openConnections[c] == null) +" "+c, openSet[i].gameObject);
				estimatedScore = Heuristics.ManhattanDistance(openConnections[c].GridPosition, endPos) + weights[(int)openConnections[c].Type];
				if(estimatedScore < distance)
				{
					distance = estimatedScore;
					index = c;
					foundNewNode = true;
				}
			}
			if(!foundNewNode || !openConnections[index].Walkable(type))
			{
				//openSet.RemoveAt(i);
				i = -1;
				openSet.Clear();
				openSet.Add(start);
				if(openSet.Count == 0)
				{ 
					Debug.Log("No path found");
					return;
				}
			}
			else
			{
				Debug.Log("Next = " + openConnections[index].name);
				openSet.Add(openConnections[index]);
				navigatedSet.Add(openConnections[index]);
			}
		}
		if(!endIsFound)
		{
			Debug.Log("Impossible to create a path!");
		}
		foreach(PathNode p in openSet)
		{
			p.SetColor(Color.cyan);
		}
	}
	
	public void GetPath(PathNode start, PathNode end, PathFollowerTypes type)
	{
		var openSet = new List<PathNode>();
		openSet.Add(start);
		var closedSet = new List<PathNode>();
		var cameFrom = new Dictionary<PathNode, PathNode>();
		
		var globalScore = new Dictionary<PathNode, float>();
		globalScore.Add(start, 0);
		var fScore = new Dictionary<PathNode, float>();
		fScore.Add(start, globalScore[start] + Heuristics.ManhattanDistance(start.GridPosition, end.GridPosition));
		
		var iWeights = PathWeights.GetWeights(type);
		float[] weights = new float[iWeights.Count];
		for(int w = 0; w < weights.Length; w++)
		{
			weights[w] = (float)iWeights[(PathType)w] * gridNodeSize;
		}
		
		while(openSet.Count > 0)
		{
			float lowestF = -1;
			PathNode current = null;
			foreach(PathNode node in openSet)
			{
				var score = fScore[node];
				if(score < lowestF || lowestF == -1)
				{
					lowestF = score;
					current = node;
				}
			}
			if(current == end)
			{
				Debug.Log("Path found! With a cost of "+ fScore[current]);
				RecreatePath(current, cameFrom);
				start.SetColor(Color.green);
				end.SetColor(Color.red);
				return;
			}
			
			openSet.Remove(current);
			closedSet.Add(current);
			var connections = current.GetConnections();
			float tentativeScore = 0;
			foreach(PathNode node in connections)
			{
				if(node == null || !node.Walkable(type)){ continue; }
				tentativeScore = globalScore[current] + weights[(int)node.Type]; //Heuristics.ManhattanDistance(current.GridPosition, node.GridPosition);
				if(closedSet.Contains(node) && tentativeScore >= globalScore[node])
				{
					continue;
				}
				else
				{
					if(cameFrom.ContainsKey(node))
					{
						cameFrom[node] = current;
					}
					else{ cameFrom.Add(node, current); }
					globalScore[node] = tentativeScore;
					fScore[node] = tentativeScore + Heuristics.ManhattanDistance(node.GridPosition, end.GridPosition);
					if(!openSet.Contains(node))
					{
						openSet.Add(node);
					}
					
				}
			}
		}
		Debug.Log("Failure");
	}
	
	private void RecreatePath(PathNode current, Dictionary<PathNode, PathNode> cameFrom)
	{
		current.SetColor(Color.cyan);
		if(cameFrom.ContainsKey(current))
		{
			RecreatePath(cameFrom[current], cameFrom);
		}
	}
}
/*
 	closedset := the empty set    // The set of nodes already evaluated.
	openset := {start}    // The set of tentative nodes to be evaluated, initially containing the start node
	came_from := the empty map    // The map of navigated nodes.
	
	g_score[start] := 0    // Cost from start along best known path.
	// Estimated total cost from start to goal through y.
	f_score[start] := g_score[start] + heuristic_cost_estimate(start, goal)
	
	while openset is not empty
		current := the node in openset having the lowest f_score[] value
		if current = goal
			return reconstruct_path(came_from, goal)
	
		remove current from openset
		add current to closedset
		for each neighbor in neighbor_nodes(current)
			tentative_g_score := g_score[current] + dist_between(current,neighbor)
			if neighbor in closedset and tentative_g_score >= g_score[neighbor]
				continue
	
			if neighbor not in openset or tentative_g_score < g_score[neighbor] 
				came_from[neighbor] := current
				g_score[neighbor] := tentative_g_score
				f_score[neighbor] := g_score[neighbor] + heuristic_cost_estimate(neighbor, goal)
				if neighbor not in openset
					add neighbor to openset
	
	return failure
	
	function reconstruct_path(came_from, current_node)
		if current_node in came_from
			p := reconstruct_path(came_from, came_from[current_node])
			return (p + current_node)
		else
			return current_node
 */