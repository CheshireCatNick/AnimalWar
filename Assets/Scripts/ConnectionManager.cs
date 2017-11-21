using System.IO;
using System.Net.Sockets;

public class ConnectionManager {
    private TcpClient client;
    private Stream s;
    private StreamReader sr;
    private StreamWriter sw;

    // blocking
    public string ReceiveID()
    {
        return sr.ReadLine();
    }

    // non-blocking
    public string ReceiveActionStr()
    {
        if (client.Available > 0)
            return sr.ReadLine();
        return "";
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
