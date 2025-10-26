using System;

class Demo
{
    static void Ping(string host)
    {
        Console.WriteLine(host);
    }

    static void TwoParameters(int hello, string world)
    {
        Console.WriteLine(world);
    }
    
    static int Add(int a, int b) => a + b;
}