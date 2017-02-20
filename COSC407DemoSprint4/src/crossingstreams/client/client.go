package main
 
import (
    "fmt"
    "net"
	"os"
	"math/rand"
    "time"
	flatbuffers "github.com/google/flatbuffers/go"
	netchan_flatbuffer "crossingstreams/server/sample"
)
  
type netchan struct {
	protocol int32
	sequence int32
	ack int32
}
  
func (n *netchan) Print() {
	fmt.Printf("Sequence: %d\nACK: %d\n",n.sequence,n.ack)
}
  
/* A Simple function to verify error */
func CheckError(err error) {
    if err  != nil {
        fmt.Println("Error: " , err)
        os.Exit(0)
    }
}

func serializeNetchan(seq int32, ack int32) []byte{
	// Create a 'FlatBufferBuilder', which is used to create the
	// netChannel FlatBuffers.
	builder := flatbuffers.NewBuilder(0)
		
	// Create 'netchan'
	netchan_flatbuffer.NetchanStart(builder)
	netchan_flatbuffer.NetchanAddProtocol(builder, 123)
	netchan_flatbuffer.NetchanAddSequence(builder, seq)
	netchan_flatbuffer.NetchanAddAck(builder, ack)
	
	netchan := netchan_flatbuffer.NetchanEnd(builder)
	builder.Finish(netchan)
	buf := builder.FinishedBytes()
	return buf
}

func deserializeNetchan(buf []byte) netchan{
	net := netchan_flatbuffer.GetRootAsNetchan(buf, 0)
	protocol := net.Protocol()
	sequence := net.Sequence()
	ack := net.Ack()
	n := netchan{protocol, sequence, ack}
	return n
}

  
func main() {
	send := netchan{123,0,0}
	recv := netchan{}
	bufin := serializeNetchan(send.sequence,send.ack)
	bufout := make([]byte, 1024)

    ServerAddr,err := net.ResolveUDPAddr("udp","127.0.0.1:8000")
    CheckError(err)
 
    LocalAddr, err := net.ResolveUDPAddr("udp", "127.0.0.1:0")
    CheckError(err)
 
    Conn, err := net.DialUDP("udp", LocalAddr, ServerAddr)
    CheckError(err)
 
    defer Conn.Close()
	i := int32(0)
	//t0 := time.Now()
	rtt := float64(0.0)
	newrtt := float64(0.0)
	
	go func(){
		for {
			_, addr, err := Conn.ReadFromUDP(bufout)
			CheckError(err)
			recv = deserializeNetchan(bufout)
			fmt.Printf("\nReceived from %s",addr)
			recv.Print()
			bufin = serializeNetchan(i, recv.sequence)
			send = deserializeNetchan(bufin)
		}
	}()
	
    for {
		i++
		t0 := time.Now()
		//fmt.Println(i)
		send = netchan{123,rand.Int31(),0}
		_, err := Conn.Write(bufin)
		CheckError(err)
			
		//fmt.Println("\nSent:")
		//send.Print()
		fmt.Println(i)
		
		//t1 := time.Now()
		if rtt == 0 {
			rtt = float64(time.Since(t0).Nanoseconds() / 1000000)
		} else {
			newrtt = float64(time.Since(t0).Nanoseconds() / 1000000)
			rtt = float64(rtt + (newrtt - rtt) / 10)
		}
		fmt.Printf("RTT: %6.0fms\n",rtt)
		fmt.Printf("newRTT: %fms\n",newrtt)
		fmt.Printf("RTTDiff: %fms\n",newrtt - rtt)
    }
	
	
}