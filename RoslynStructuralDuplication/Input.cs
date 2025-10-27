class Test
{
    static void Ping(string host) => Console.WriteLine(host);
    class C
    {
        C(int par1, int par2, string par3)
        {
            Console.WriteLine("Nothing to do");
        }
        bool TestCase(string testCase) { return true; }

        public C MaybeThis(C myClass) => this;
    }

    static void TwoParameters(int hello, string world)
    {
        Console.WriteLine(world);
    }
    static int Add() => 1 + 3;
}