using System;

class Demo
{
    static void Ping(string host)
    {
        Console.WriteLine(host);
    }
    
    class C {
        bool Test(C test) { return true; }
        bool EnterNewName(string name) { return true; }
    }

    static void TwoParameters(int hello, string world)
    {
        Console.WriteLine(world);
    }
    
    static int Add(int a, int b) => a + b;
}