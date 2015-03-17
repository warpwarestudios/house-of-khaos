using UnityEngine;
using System.Collections;

public static class MapDirections{

	public const int Count = 4;

	private static IntVector2[] vectors = {
		new IntVector2(0, 1),
		new IntVector2(1, 0),
		new IntVector2(0, -1),
		new IntVector2(-1, 0)
	};
	
	public static IntVector2 ToIntVector2 (this MapDirection direction) {
		return vectors[(int)direction];
	}

	public static MapDirection RandomValue 
	{
		get
		{
			return (MapDirection)Random.Range(0,Count);
		}
	}

	private static MapDirection[] opposites = {
		MapDirection.South,
		MapDirection.West,
		MapDirection.North,
		MapDirection.East
	};
	
	public static MapDirection GetOpposite (this MapDirection direction) {
		return opposites[(int)direction];
	}

	private static Quaternion[] rotations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 90f, 0f),
		Quaternion.Euler(0f, 180f, 0f),
		Quaternion.Euler(0f, 270f, 0f)
	};
	
	public static Quaternion ToRotation (this MapDirection direction) {
		return rotations[(int)direction];
	}
}
