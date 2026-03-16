using System;
using System.Net.Sockets;

class PortScanner
{
    static bool ScanPort(string host, int port)
    {
        try
        {
            TcpClient client = new TcpClient();
            IAsyncResult result = client.BeginConnect(host, port, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(200, true);

            if (success)
            {
                client.EndConnect(result);
                Console.WriteLine("[+] Port " + port + " OPEN");
                client.Close();
                return true;
            }

            client.Close();
        }
        catch
        {
        }

        return false;
    }

    static void ScanAllPorts(string host)
    {
        Console.WriteLine("Scanning ALL ports on " + host);

        for (int port = 1; port <= 65535; port++)
        {
            ScanPort(host, port);
        }
    }

    static void ScanCustomPorts(string host, string portList)
    {
        string[] ports = portList.Split(new char[] { ',' });

        foreach (string p in ports)
        {
            int port = Convert.ToInt32(p);
            ScanPort(host, port);
        }
    }

    static void DefaultScan(string host)
    {
        int[] ports = {21,22,23,25,53,80,110,111,135,139,143,443,445,873,993,995,1433,1723,3306,3389,5432,5985,5900,8080};

        foreach (int port in ports)
        {
            ScanPort(host, port);
        }
    }

    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("portscanner.exe hosts=IP ports=PORTS");
            Console.WriteLine("portscanner.exe hosts=IP allports");
            Console.WriteLine("portscanner.exe IP");
            return;
        }

        if (args[0].StartsWith("hosts="))
        {
            string host = args[0].Replace("hosts=", "");

            if (args.Length > 1)
            {
                if (args[1] == "allports")
                {
                    ScanAllPorts(host);
                }
                else if (args[1].StartsWith("ports="))
                {
                    string ports = args[1].Replace("ports=", "");
                    ScanCustomPorts(host, ports);
                }
            }
        }
        else
        {
            DefaultScan(args[0]);
        }
    }
}
