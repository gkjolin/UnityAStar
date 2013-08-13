using UnityEngine;
using System.Collections;

public class Heuristics
{
	public static float ManhattanDistance(int aX, int aY, int aZ, int bX, int bY, int bZ)
	{
		return Mathf.Abs(aX - bX) + Mathf.Abs(aY - bY) + Mathf.Abs(aZ - bZ);
	}
	
	public static float ManhattanDistance(float aX, float aY, float aZ, float bX, float bY, float bZ)
	{
		return Mathf.Abs(aX - bX) + Mathf.Abs(aY - bY) + Mathf.Abs(aZ - bZ);
	}
	
	public static float ManhattanDistance(Vec2 a, Vec2 b)
	{
		return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
	}
	
	public static float ManhattanDistance(Vector2 a, Vector2 b)
	{
		return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
	}
	
	public static float ManhattanDistance(Vector3 a, Vector3 b)
	{
		return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
	}
	
	public static float EuclideanDistance(int aX, int aY, int aZ, int bX, int bY, int bZ)
	{
		var x = aX - bX;
		var y = aY - bY;
		var z = aZ - bZ;
		return Mathf.Sqrt((x * x) + (y * y) + (z * z));
	}
	
	public static float EuclideanDistance(float aX, float aY, float aZ, float bX, float bY, float bZ)
	{
		var x = aX - bX;
		var y = aY - bY;
		var z = aZ - bZ;
		return Mathf.Sqrt((x * x) + (y * y) + (z * z));
	}
	
	public static float EuclideanDistance(Vec2 a, Vec2 b)
	{
		var x = a.x - b.x;
		var y = a.y - b.y;
		return Mathf.Sqrt((x * x) + (y * y));
	}
	
	public static float EuclideanDistance(Vector3 a, Vector3 b)
	{
		var x = a.x - b.x;
		var y = a.y - b.y;
		var z = a.z - b.z;
		return Mathf.Sqrt((x * x) + (y * y) + (z * z));
	}
	
	public static float EuclideanDistance(Vector2 a, Vector2 b)
	{
		var x = a.x - b.x;
		var y = a.y - b.y;
		return Mathf.Sqrt((x * x) + (y * y));
	}
	
	public static float SquareEuclideanDistance(int aX, int aY, int aZ, int bX, int bY, int bZ)
	{
		var x = aX - bX;
		var y = aY - bY;
		var z = aZ - bZ;
		return (x * x) + (y * y) + (z * z);
	}
	
	public static float SquareEuclideanDistance(float aX, float aY, float aZ, float bX, float bY, float bZ)
	{
		var x = aX - bX;
		var y = aY - bY;
		var z = aZ - bZ;
		return (x * x) + (y * y) + (z * z);
	}
	
	public static float SquareEuclideanDistance(Vec2 a, Vec2 b)
	{
		var x = a.x - b.x;
		var y = a.y - b.y;
		return (x * x) + (y * y);
	}
	
	public static float SquareEuclideanDistance(Vector3 a, Vector3 b)
	{
		var x = a.x - b.x;
		var y = a.y - b.y;
		var z = a.z - b.z;
		return (x * x) + (y * y) + (z * z);
	}
	
	public static float SquareEuclideanDistance(Vector2 a, Vector2 b)
	{
		var x = a.x - b.x;
		var y = a.y - b.y;
		return (x * x) + (y * y);
	}
}
