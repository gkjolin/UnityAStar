using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode : MonoBehaviour 
{
	private List<PathNode> connections;
	private PathType[] connectionWeights;
	private Transform _transform;
	private PathTypeState _pathType;
	
	[HideInInspector][SerializeField] private PathType lastSetType = PathType.road;
	[HideInInspector][SerializeField] private int _x;
	[HideInInspector][SerializeField] private int _y;
	
	public int X{ get{ return _x;} private set{ _x = value;} }
	public int Y{ get{ return _y;} private set{ _y = value;} }
	public Vec2 GridPosition{ get{ return new Vec2(X, Y); } }
	public int ID{ get; private set; }
	public PathType Type { get{ return NodeType.Type; } }
	
	private PathTypeState NodeType
	{
		get
		{
			if(_pathType == null)
			{
				ChangePathType(lastSetType);
			}
			return _pathType;
		}
		set{ _pathType = value; }
	}
	
	public Transform Trans
	{
		get
		{
			if(_transform == null)
			{
				_transform = transform;
			}
			return _transform;
		}
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	public void Initialize(int id, int x, int y, PathType type)
	{
		ID = id;
		_x = x;
		_y = y;
		ChangePathType(type);
	}
	
	public void Initialize(int id, int x, int y, PathType type, Vector3 position)
	{
		ID = id;
		_x = x;
		_y = y;
		ChangePathType(type);
		Trans.position = position;
	}
	
	public void Initialize(int id, int x, int y, PathType type, Vector3 position, PathNode[] connections)
	{
		ID = id;
		_x = x;
		_y = y;
		ChangePathType(type);
		Trans.position = position;
		SetConnections(connections);
	}
	
	public void SetConnections(PathNode[] nodes)
	{
		var length = nodes.Length;
		connections = new List<PathNode>();
		for(int i = 0; i < length; i++)
		{
			connections.Add(nodes[i]);
		}
	}
	
	public void AddConnection(PathNode node)
	{
		connections.Add(node);
	}
	
	public void RemoveConnection(PathNode node)
	{
		connections.Remove(node);
	}
	
	public PathNode[] GetConnections()
	{
		if(connections == null){ connections = new List<PathNode>(PathManager.MainManager.GetConnections(this)); }
		return connections.ToArray();
	}
	
	public PathType[] GetConnectionWeights()
	{
		if(connectionWeights == null){ CalculateConnectionWeights(); }
		return connectionWeights;
	}
	
	private void CalculateConnectionWeights()
	{
		if(connections == null) { connections = new List<PathNode>(PathManager.MainManager.GetConnections(this)); }
		var length = connections.Count;
		connectionWeights = new PathType[length];
		for(int i = 0; i < length; i++)
		{
			if(connections[i] == null){ continue; }
			connectionWeights[i] = connections[i].Type;
		}
		
	}
	
	public void OnMouseDown()
	{
		if(Input.GetKey(KeyCode.A))
		{
			PathManager.a = this;
			SetColor(Color.green);
		}
		else if(Input.GetKey(KeyCode.B))
		{
			PathManager.b = this;
			SetColor(Color.red);
		}
		else
		{
			SetColor(Color.grey);
		}
//		var connections = PathManager.MainManager.GetConnections(this);
//		for(int i = 0; i < 4; i++)
//		{
//			if(connections[i] != null)
//			{
//				connections[i].SetColor(Color.green);
//			}
//		}
	}
	
	public void SetColor(Color c)
	{
		renderer.material.color = c;
	}
	
	public void SetSharedMaterial(int index)
	{
		renderer.sharedMaterial = PathManager.MainManager.sharedNodeMaterials[index];
	}
	
	public bool Walkable(PathFollowerTypes follower)
	{
		return NodeType.Walkable(follower);
	}
	
	public void ChangePathType(PathTypeState newType)
	{
		lastSetType = newType.Type;
		if(_pathType != null){ _pathType.OnExitState(); }
		_pathType = newType;
		_pathType.OnEnterState(this);
		
	}
	
	public void ChangePathType(PathType newType)
	{
		PathTypeState state = null;
		lastSetType = newType;
		switch(newType)
		{
		case PathType.city:
			state = new CityType();
			break;
		case PathType.desert:
			state = new DesertType();
			break;
		case PathType.field:
			state = new FieldType();
			break;
		case PathType.forest:
			state = new ForestType();
			break;
		case PathType.mountain:
			state = new MountainType();
			break;
		case PathType.road:
			state = new RoadType();
			break;
		case PathType.rocks:
			state = new RocksType();
			break;
		case PathType.swamp:
			state = new SwampType();
			break;
		case PathType.water:
			state = new WaterType();
			break;
		}
		
		if(_pathType != null){ _pathType.OnExitState(); }
		_pathType = state;
		_pathType.OnEnterState(this);
	}
}
