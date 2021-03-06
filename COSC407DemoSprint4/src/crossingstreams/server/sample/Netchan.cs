// automatically generated by the FlatBuffers compiler, do not modify

namespace sample
{

using System;
using FlatBuffers;

public struct Netchan : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static Netchan GetRootAsNetchan(ByteBuffer _bb) { return GetRootAsNetchan(_bb, new Netchan()); }
  public static Netchan GetRootAsNetchan(ByteBuffer _bb, Netchan obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Netchan __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int Protocol { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Sequence { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Ack { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public Player? Players(int j) { int o = __p.__offset(10); return o != 0 ? (Player?)(new Player()).__assign(__p.__vector(o) + j * 16, __p.bb) : null; }
  public int PlayersLength { get { int o = __p.__offset(10); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<Netchan> CreateNetchan(FlatBufferBuilder builder,
      int protocol = 0,
      int sequence = 0,
      int ack = 0,
      VectorOffset playersOffset = default(VectorOffset)) {
    builder.StartObject(4);
    Netchan.AddPlayers(builder, playersOffset);
    Netchan.AddAck(builder, ack);
    Netchan.AddSequence(builder, sequence);
    Netchan.AddProtocol(builder, protocol);
    return Netchan.EndNetchan(builder);
  }

  public static void StartNetchan(FlatBufferBuilder builder) { builder.StartObject(4); }
  public static void AddProtocol(FlatBufferBuilder builder, int protocol) { builder.AddInt(0, protocol, 0); }
  public static void AddSequence(FlatBufferBuilder builder, int sequence) { builder.AddInt(1, sequence, 0); }
  public static void AddAck(FlatBufferBuilder builder, int ack) { builder.AddInt(2, ack, 0); }
  public static void AddPlayers(FlatBufferBuilder builder, VectorOffset playersOffset) { builder.AddOffset(3, playersOffset.Value, 0); }
  public static void StartPlayersVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(16, numElems, 4); }
  public static Offset<Netchan> EndNetchan(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Netchan>(o);
  }
  public static void FinishNetchanBuffer(FlatBufferBuilder builder, Offset<Netchan> offset) { builder.Finish(offset.Value); }
};


}
