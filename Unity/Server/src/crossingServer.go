package main

import (
	"github.com/gorilla/mux"
	"fmt"
	"os"
	"net"
	"net/http"
	"time"
	"sync"
)

var (
	mutex = &sync.Mutex{}
	seed = ""
	host_addr = ""
	port = ":25565"
)

const (
	VERSION = "Client version (0.0.1)"
	BANNER = `
 ______                          __                   _______ __                                   
|      |.----.-----.-----.-----.|__|.-----.-----.    |     __|  |_.----.-----.---.-.--------.-----.
|   ---||   _|  _  |__ --|__ --||  ||     |  _  |    |__     |   _|   _|  -__|  _  |        |__ --|
|______||__| |_____|_____|_____||__||__|__|___  |    |_______|____|__| |_____|___._|__|__|__|_____|
                                          |_____|     ` + VERSION + ` `

)

//this is also the handler for joining to the chat
func get(w http.ResponseWriter, r *http.Request) {
	if r.Method != "GET" {
		fmt.Fprintf(w, "-3")
		return
	}
	if seed != "" {
		fmt.Fprintf(w, seed)
	} else {
		fmt.Fprintf(w, "-1")
	}
}

//##############SERVING THE SEED
func send(w http.ResponseWriter, r *http.Request) {
	if r.Method != "POST" {
		fmt.Fprintf(w, "-3")
		return
	}
	r.ParseForm()
	if r.Form["seed"][0] != "" {
		mutex.Lock()
		if seed == "" {
			seed = r.Form["seed"][0]
			fmt.Fprintf(w, "1")
		} else {
			fmt.Fprintf(w, "-2")
		}
		mutex.Unlock()
	} else {
		fmt.Fprintf(w, "-1")
	}
}

//#############MAIN FUNCTION and INITIALIZATIONS

func main() {
	
	if len(os.Args) > 1 {
		if os.Args[1] != "" {
			port = ":" + os.Args[1]
		}
	}
	
	// grab server's interface addresses
	addrs, err := net.InterfaceAddrs()
	if err != nil {
		os.Stderr.WriteString("Oops: " + err.Error() + "\n")
		os.Exit(1)
	}
	// parse through server's interface addresses
	// and find network IP address 
	for _, a := range addrs {
		if ipnet, ok := a.(*net.IPNet); ok && !ipnet.IP.IsLoopback() {
			if ipnet.IP.To4() != nil {
				host_addr = ipnet.IP.String()
			}
		}
	}

	// Display banner message
	bannerMessage := fmt.Sprintf("%s: Running at %s%s", time.Now().Format(time.UnixDate),host_addr, port)
	fmt.Println(BANNER + "\n" + bannerMessage)	
	
	r := mux.NewRouter()
	r.HandleFunc("/get", get)
	r.HandleFunc("/send", send)
	http.Handle("/", r)
	http.ListenAndServe(port, nil)
	
}

// handles errors from function calls
func checkErr(err error) {
	if err != nil {
		panic(err)
	}
}
