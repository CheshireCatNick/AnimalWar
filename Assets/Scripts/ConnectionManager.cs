using System.IO;
using System.Net.Sockets;

public class ConnectionManager {
    private TcpClient client;
    private Stream s;
    private StreamReader sr;
    private StreamWriter sw;

    public string Receive()
    {
        return sr.ReadLine();
    }

    public void Send(string str)
    { 
        sw.WriteLine(str);
        sw.Flush();
    }

    public void Close()
    {
        s.Close();
        client.Close();
    }

	// Constructor
	public ConnectionManager() {
        client = new TcpClient("140.112.30.38", 36251);

        s = client.GetStream();
        sr = new StreamReader(s);
        sw = new StreamWriter(s);
    }


}
