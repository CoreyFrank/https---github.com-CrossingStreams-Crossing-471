/* SimpleEchoServer
 */
package main

import (
	//"bufio"
	"fmt"
	"log"
	"net"
	"os"
	"time"
)

type Message struct {
	clientId int
	msg      []byte
	t time.Time
}

func NewMessage(id int, m []byte) Message {
	message := Message{}
	message.clientId = id
	message.msg = m
	message.t = time.Now()

	return message
}

func main() {
	// Number of people connected
	clientCount := 0

	// All people who are connected; a map wherein
	// the keys are net.Conn objects and the values
	// are client "ids", an integer.
	allClients := make(map[net.Conn]int)

	// Channel into which the TCP server will push
	// new connections.
	//
	newConnections := make(chan net.Conn)

	// Channel into which we'll push dead connections
	// for removal from allClients.
	//
	deadConnections := make(chan net.Conn)

	// Channel into which we'll push messages from
	// connected clients so that we can broadcast them
	// to every connection in allClients.
	//
	messages := make(chan Message)

	port := ":25565"
	tcpAddr, err := net.ResolveTCPAddr("tcp4", port)
	checkError(err)

	listener, err := net.ListenTCP("tcp", tcpAddr)
	checkError(err)

	go func() {
		for {
			conn, err := listener.Accept()
			if err != nil {
				checkError(err)
			}
			newConnections <- conn
		}
	}()

	for {
		// Handle 1) new connections; 2) dead connections;
		// and, 3) broadcast messages.
		select {
		// Accept new clients
		//
		case conn := <-newConnections:

			// Add this connection to the `allClients` map
			//
			if len(allClients) > 4 {
				log.Printf("max Clients reached")
				conn.Close()
			}
			allClients[conn] = clientCount
			clientCount += 1

			log.Printf("Accepted new client, #%d", clientCount)

			// Constantly read incoming messages from this
			// client in a goroutine and push those onto
			// the messages channel for broadcast to others.
			//
			go func(conn net.Conn, clientId int) {
				//log.Println("Listening for messages")
				var buf [512]byte
				//log.Println("reader setup")
				for {
					n, err := conn.Read(buf[0:])
					if err != nil {
						if opErr, ok := err.(*net.OpError); ok && opErr.Timeout() {
							continue
						}
						log.Println(err)
						break
					}
					id := make([]byte, 1)
					id[0] = byte(clientId)
					send := NewMessage(clientId, buf[0:n])
					messages <- send
				}

				// When we encouter `err` reading, send this
				// connection to `deadConnections` for removal.
				//
				deadConnections <- conn

			}(conn, allClients[conn])

		// Accept messages from connected clients
		//
		case message := <-messages:
			// Loop over all connected clients
			//
			for conn, id := range allClients {
				
				// Send them a message in a go-routine
				// so that the network operation doesn't block
				//
				if (id != message.clientId || time.Now().Sub(message.t) >= 50 * time.Millisecond ) {
					go func(conn net.Conn, message []byte) {
						_, err := conn.Write(message[0:])

						// If there was an error communicating
						// with them, the connection is dead.
						if err != nil {
							deadConnections <- conn
						}
					}(conn, message.msg)
					log.Printf("Sending message from %d, to %d", message.clientId, id)
					}
			}
			
			log.Printf("New message: %s, length: %d", message.msg, len(message.msg))
			log.Printf("Broadcast to %d clients", len(allClients))

		// Remove dead clients
		//
		case conn := <-deadConnections:
			log.Printf("Client %d disconnected", allClients[conn])
			delete(allClients, conn)
		}
	}
}

func checkError(err error) {
	if err != nil {
		fmt.Fprintf(os.Stderr, "Fatal error: %s", err.Error())
		os.Exit(1)
	}
}
