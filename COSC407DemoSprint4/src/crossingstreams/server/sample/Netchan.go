// automatically generated by the FlatBuffers compiler, do not modify

package sample

import (
	flatbuffers "github.com/google/flatbuffers/go"
)

type Netchan struct {
	_tab flatbuffers.Table
}

func GetRootAsNetchan(buf []byte, offset flatbuffers.UOffsetT) *Netchan {
	n := flatbuffers.GetUOffsetT(buf[offset:])
	x := &Netchan{}
	x.Init(buf, n+offset)
	return x
}

func (rcv *Netchan) Init(buf []byte, i flatbuffers.UOffsetT) {
	rcv._tab.Bytes = buf
	rcv._tab.Pos = i
}

func (rcv *Netchan) Table() flatbuffers.Table {
	return rcv._tab
}

func (rcv *Netchan) Protocol() int32 {
	o := flatbuffers.UOffsetT(rcv._tab.Offset(4))
	if o != 0 {
		return rcv._tab.GetInt32(o + rcv._tab.Pos)
	}
	return 0
}

func (rcv *Netchan) MutateProtocol(n int32) bool {
	return rcv._tab.MutateInt32Slot(4, n)
}

func (rcv *Netchan) Sequence() int32 {
	o := flatbuffers.UOffsetT(rcv._tab.Offset(6))
	if o != 0 {
		return rcv._tab.GetInt32(o + rcv._tab.Pos)
	}
	return 0
}

func (rcv *Netchan) MutateSequence(n int32) bool {
	return rcv._tab.MutateInt32Slot(6, n)
}

func (rcv *Netchan) Ack() int32 {
	o := flatbuffers.UOffsetT(rcv._tab.Offset(8))
	if o != 0 {
		return rcv._tab.GetInt32(o + rcv._tab.Pos)
	}
	return 0
}

func (rcv *Netchan) MutateAck(n int32) bool {
	return rcv._tab.MutateInt32Slot(8, n)
}

func (rcv *Netchan) Players(obj *Player, j int) bool {
	o := flatbuffers.UOffsetT(rcv._tab.Offset(10))
	if o != 0 {
		x := rcv._tab.Vector(o)
		x += flatbuffers.UOffsetT(j) * 4
		x = rcv._tab.Indirect(x)
		obj.Init(rcv._tab.Bytes, x)
		return true
	}
	return false
}

func (rcv *Netchan) PlayersLength() int {
	o := flatbuffers.UOffsetT(rcv._tab.Offset(10))
	if o != 0 {
		return rcv._tab.VectorLen(o)
	}
	return 0
}

func NetchanStart(builder *flatbuffers.Builder) {
	builder.StartObject(4)
}
func NetchanAddProtocol(builder *flatbuffers.Builder, protocol int32) {
	builder.PrependInt32Slot(0, protocol, 0)
}
func NetchanAddSequence(builder *flatbuffers.Builder, sequence int32) {
	builder.PrependInt32Slot(1, sequence, 0)
}
func NetchanAddAck(builder *flatbuffers.Builder, ack int32) {
	builder.PrependInt32Slot(2, ack, 0)
}
func NetchanAddPlayers(builder *flatbuffers.Builder, players flatbuffers.UOffsetT) {
	builder.PrependUOffsetTSlot(3, flatbuffers.UOffsetT(players), 0)
}
func NetchanStartPlayersVector(builder *flatbuffers.Builder, numElems int) flatbuffers.UOffsetT {
	return builder.StartVector(4, numElems, 4)
}
func NetchanEnd(builder *flatbuffers.Builder) flatbuffers.UOffsetT {
	return builder.EndObject()
}