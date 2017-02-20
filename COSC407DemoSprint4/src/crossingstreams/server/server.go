package main

import (
	netbuffer "crossingstreams/server/sample"
	"flag"
	"fmt"
	flatbuffers "github.com/google/flatbuffers/go"
	"net"
	"os"
	"sync"
)

var (
	ServerConn *net.UDPConn
	NWorkers   = flag.Int("n", 4, "The number of workers to start")
	UDPAddr    = flag.String("http", "127.0.0.1:8000", "Address to listen for HTTP requests on")
)

/*
fmt.Println(i)
i++
_, addr, err := ServerConn.ReadFromUDP(bufin)
recv = deserializeNetchan(bufin)

bufout = serializeNetchan(0, recv.sequence)
//fmt.Println("Received from ",addr)
//msg := deserializeNetchan(buf)

_, err = ServerConn.WriteToUDP(bufout,addr)
if err != nil {
	fmt.Println("Error: ",err)
}
*/
type vector3 struct{
	x float32
	y float32
	z float32
}

type WorkRequest struct {
	buf  []byte
	addr *net.UDPAddr
}

var WorkQueue = make(chan WorkRequest, 100)
var WorkerQueue chan chan WorkRequest

func StartDispatcher(nworkers int) {
	WorkerQueue = make(chan chan WorkRequest, nworkers)

	for i := 0; i < nworkers; i++ {
		fmt.Println("Starting worker", i+1)
		worker := NewWorker(i+1, WorkerQueue)
		worker.Start()
	}

	go func() {
		for {
			select {
			case work := <-WorkQueue:
				go func() {
					worker := <-WorkerQueue

					worker <- work
				}()
			}
		}
	}()
}

func Collector(work WorkRequest){
	WorkQueue <- work
	fmt.Println("Work request queued")
	return
}

func NewWorker(id int, workerQueue chan chan WorkRequest) Worker {
	worker := Worker{
		ID:          id,
		Work:        make(chan WorkRequest),
		WorkerQueue: workerQueue,
		QuitChan:    make(chan bool)}
	return worker
}

type Worker struct {
	ID          int
	Work        chan WorkRequest
	WorkerQueue chan chan WorkRequest
	QuitChan    chan bool
}

//recv = deserializeNetchan(bufin)

//bufout := serializeNetchan(0, recv.sequence)
//fmt.Println("Received from ",addr)

/*msg := deserializeNetchan(buf)

		_, err = ServerConn.WriteToUDP(bufout,addr)
        if err != nil {
            fmt.Println("Error: ",err)
        } */

func (w *Worker) Start() {
	go func() {
		for {
			w.WorkerQueue <- w.Work

			select {
			case work := <-w.Work:
				fmt.Printf("\nworker%d: Received work request\n", w.ID)
				recv := deserializeNetchan(work.buf)
				_ = recv
				//bufout := serializeNetchan(recv)
				_, err := ServerConn.WriteToUDP(work.buf, work.addr)
				CheckError(err)
			case <-w.QuitChan:
				return
			}
		}
	}()
}

func (w *Worker) Stop() {
	go func() {
		w.QuitChan <- true
	}()
}

/*func (n *netchan) Print() {
	fmt.Printf("Protocol: %d\nSequence: %d\nACK: %d\n", n.protocol, n.sequence, 
	n.ack)
	for _, p := range n.players{
	
	}
	//fmt.Printf("Position[\nx: %f\ny: %f\nz: %f\n]", n.vecx, n.vecy, n.vecz)
}*/

/* A Simple function to verify error */
func CheckError(err error) {
	if err != nil {
		fmt.Println("Error: ", err)
		os.Exit(0)
	}
}

func serializeNetchan(seq int32, ack int32, pid int32, pos vector3, rot vector3) []byte {
	builder := flatbuffers.NewBuilder(1)
	
	netbuffer.PlayerStart(builder)
	netbuffer.PlayerAddId(builder, pid)
	posvec3 := netbuffer.CreateVec3(builder, pos.x, pos.y, pos.z)
	rotvec3 := netbuffer.CreateVec3(builder, rot.x, rot.y, rot.z)
	netbuffer.PlayerAddPos(builder, posvec3)
	netbuffer.PlayerAddPos(builder, rotvec3)
	p1 := netbuffer.PlayerEnd(builder)
	
	netbuffer.NetchanStartPlayersVector(builder, 1)
	builder.PrependUOffsetT(p1)
	plays := builder.EndVector(1)
	
	netbuffer.NetchanStart(builder)
	netbuffer.NetchanAddProtocol(builder, 1234)
	netbuffer.NetchanAddSequence(builder, seq)
	netbuffer.NetchanAddAck(builder, ack)
	netbuffer.NetchanAddPlayers(builder, plays)
	netchan := netbuffer.NetchanEnd(builder)
	
	builder.Finish(netchan)
	
	buf := builder.FinishedBytes()
	
	return buf
}

func deserializeNetchan(buf []byte) *netbuffer.Netchan {
	net := netbuffer.GetRootAsNetchan(buf, 0)
	return net
}

func main() {
	wg := sync.WaitGroup{}
	defer wg.Wait()
	
	flag.Parse()

	fmt.Println("Starting the dispatcher")
	StartDispatcher(*NWorkers)

	//recv := netchan{123,0,0}
	//bufout := serializeNetchan(recv.sequence,recv.ack)

	// Lets prepare a address at any address at port 10001
	ServerAddr, err := net.ResolveUDPAddr("udp", *UDPAddr)
	CheckError(err)

	// Now listen at selected port
	ServerConn, err = net.ListenUDP("udp", ServerAddr)
	CheckError(err)

	fmt.Println("UDP Socket server listening on", *UDPAddr)

	
	defer ServerConn.Close()

	i := 0
	for {
		bufin := make([]byte, 2048)
		i++
		n, addr, err := ServerConn.ReadFromUDP(bufin)
		if err != nil {
			fmt.Printf("Error: UDP read error: %v", err)
		}
		newAddr := new(net.UDPAddr)
		*newAddr = *addr
		newAddr.IP = make(net.IP, len(addr.IP))
		copy(newAddr.IP, addr.IP)
		
		Collector(WorkRequest{bufin[:n], newAddr})
	}
}
