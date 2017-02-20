// automatically generated by the FlatBuffers compiler, do not modify

namespace sample
{

using System;
using FlatBuffers;

public struct Player : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static Player GetRootAsPlayer(ByteBuffer _bb) { return GetRootAsPlayer(_bb, new Player()); }
  public static Player GetRootAsPlayer(ByteBuffer _bb, Player obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Player __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int Id { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public Vec3? Pos { get { int o = __p.__offset(6); return o != 0 ? (Vec3?)(new Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Vec3? Rot { get { int o = __p.__offset(8); return o != 0 ? (Vec3?)(new Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartPlayer(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddId(FlatBufferBuilder builder, int id) { builder.AddInt(0, id, 0); }
  public static void AddPos(FlatBufferBuilder builder, Offset<Vec3> posOffset) { builder.AddStruct(1, posOffset.Value, 0); }
  public static void AddRot(FlatBufferBuilder builder, Offset<Vec3> rotOffset) { builder.AddStruct(2, rotOffset.Value, 0); }
  public static Offset<Player> EndPlayer(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Player>(o);
  }
};


}
