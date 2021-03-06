// automatically generated by the FlatBuffers compiler, do not modify

package sample

import (
	flatbuffers "github.com/google/flatbuffers/go"
)

type Player struct {
	_tab flatbuffers.Table
}

func GetRootAsPlayer(buf []byte, offset flatbuffers.UOffsetT) *Player {
	n := flatbuffers.GetUOffsetT(buf[offset:])
	x := &Player{}
	x.Init(buf, n+offset)
	return x
}

func (rcv *Player) Init(buf []byte, i flatbuffers.UOffsetT) {
	rcv._tab.Bytes = buf
	rcv._tab.Pos = i
}

func (rcv *Player) Table() flatbuffers.Table {
	return rcv._tab
}

func (rcv *Player) Id() int32 {
	o := flatbuffers.UOffsetT(rcv._tab.Offset(4))
	if o != 0 {
		return rcv._tab.GetInt32(o + rcv._tab.Pos)
	}
	return 0
}

func (rcv *Player) MutateId(n int32) bool {
	return rcv._tab.MutateInt32Slot(4, n)
}

func (rcv *Player) Pos(obj *Vec3) *Vec3 {
	o := flatbuffers.UOffsetT(rcv._tab.Offset(6))
	if o != 0 {
		x := o + rcv._tab.Pos
		if obj == nil {
			obj = new(Vec3)
		}
		obj.Init(rcv._tab.Bytes, x)
		return obj
	}
	return nil
}

func (rcv *Player) Rot(obj *Vec3) *Vec3 {
	o := flatbuffers.UOffsetT(rcv._tab.Offset(8))
	if o != 0 {
		x := o + rcv._tab.Pos
		if obj == nil {
			obj = new(Vec3)
		}
		obj.Init(rcv._tab.Bytes, x)
		return obj
	}
	return nil
}

func PlayerStart(builder *flatbuffers.Builder) {
	builder.StartObject(3)
}
func PlayerAddId(builder *flatbuffers.Builder, id int32) {
	builder.PrependInt32Slot(0, id, 0)
}
func PlayerAddPos(builder *flatbuffers.Builder, pos flatbuffers.UOffsetT) {
	builder.PrependStructSlot(1, flatbuffers.UOffsetT(pos), 0)
}
func PlayerAddRot(builder *flatbuffers.Builder, rot flatbuffers.UOffsetT) {
	builder.PrependStructSlot(2, flatbuffers.UOffsetT(rot), 0)
}
func PlayerEnd(builder *flatbuffers.Builder) flatbuffers.UOffsetT {
	return builder.EndObject()
}
