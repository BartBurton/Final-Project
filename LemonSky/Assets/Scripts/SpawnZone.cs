using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    float planeMultiplier = 5f;
    [SerializeField]VectorSquare zone;
    void Awake(){
        zone = new VectorSquare(transform.position, transform.localScale.x * planeMultiplier, transform.localScale.z * planeMultiplier);
    }
    public Vector3 NextRandomPosition(){
        return zone.GetRandomPoint(transform.position.y);
    }
}

public struct VectorSquare{
    public Vector2 Start;
    public Vector2 End;
    public VectorSquare(Vector2 position, float Xlen, float Ylen){
        Start = new Vector2(position.x - Xlen, position.y - Ylen);
        End = new Vector2(position.x + Xlen, position.y + Ylen);
    }
    public VectorSquare(Vector3 position, float Xlen, float Ylen){
        Start = new Vector2(position.x - Xlen, position.z - Ylen);
        End = new Vector2(position.x + Xlen, position.z + Ylen);
    }
    public bool Contain(Vector3 point){
        return Contain(new Vector2(point.x, point.z));
    }
    public bool Contain(Vector2 point){
        if(point.x < Start.x || point.x > End.x) return false;
        if(point.y < Start.y || point.y > End.y) return false;
        return true;
    }
    public Vector3 GetRandomPoint(float Y){
        var vector = GetRandomPoint();
        return new Vector3(vector.x, Y , vector.y);
    }
    public Vector2 GetRandomPoint(){
        var x = Start.x + new System.Random().NextDouble() * (End.x - Start.x);
        var y = Start.y + new System.Random().NextDouble() * (End.y - Start.y);
        return new Vector2((float)x, (float)y);
    }
}
