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

        // Single-parameter cases inside nested class
        void N1(int x) { }                     // expect: x -> x2
        void N2(int x2) { }                    // expect: x2 -> x3
        void N3(int arg1) { }                  // expect: arg1 -> arg2
        void N4(int count9) { }                // expect: count9 -> count10
        void N5(int param007) { }              // expect: param007 -> param8
        void N6(int num099) { }                // expect: num099 -> num100
        void N7(int value000) { }              // expect: value000 -> value1
        void N8(int data123456789) { }         // expect: ...789 -> ...790
        void N9(int _tmp) { }                  // expect: _tmp -> _tmp2
        void N10(int x_) { }                   // expect: x_ -> x_2
        void N11(int camelCase) { }            // expect: camelCase -> camelCase2
        void N12(int @in) { }                  // expect: @in -> @in2
        void N13(int @p1) { }                  // expect: @p1 -> @p2
    }

    static void TwoParameters(int hello, string world)
    {
        Console.WriteLine(world);
    }
    static int Add() => 1 + 3;

    // ==== Added single-parameter cases (top-level class) ====

    static void Case1(int x) { }                 // x     -> x2
    static void Case2(int x2) { }                // x2    -> x3
    static void Case3(int arg1) { }              // arg1  -> arg2
    static void Case4(int count9) { }            // count9-> count10
    static void Case5(int param007) { }          // -> param8
    static void Case6(int num099) { }            // -> num100
    static void Case7(int value000) { }          // -> value1
    static void Case8(int data123456789) { }     // -> data123456790
    static void Case9(int _tmp) { }              // -> _tmp2
    static void Case10(int x_) { }               // -> x_2
    static void Case11(int camelCase) { }        // -> camelCase2
    static void Case12(int @in) { }              // -> @in2
    static void Case13(int @p1) { }              // -> @p2

    // ==== Extra edge cases you asked for ====

    // Different types:
    static string S1(string name) => name;       // name -> name2
    static ReadOnlySpan<char> S2(ReadOnlySpan<char> span1) => span1; // span1 -> span2
    static Span<int> S3(Span<int> buf9) => buf9; // buf9 -> buf10
    static (int,int) S4((int a, int b) pair7) => pair7; // pair7 -> pair8
    static int[] S5(int[] arr) => arr;           // arr -> arr2
    static int? S6(int? maybe1) => maybe1;       // maybe1 -> maybe2
    static (int a, string b) S7((int a, string b) t) => t; // t -> t2

    // Modifiers / defaults / attributes:
    static void M1(ref int r1) { }               // r1 -> r2
    static void M2(in int in1) { }               // in1 -> in2
    static void M3(out int out1) { out1 = 0; }   // out1 -> out2
    static void M5(int def7 = 42) { }            // def7 -> def8
    static void M6(params int[] nums5) { }       // nums5 -> nums6 (note: duplicating 'params' makes invalid code, but transformation is still visible)

    // Generics:
    static T G1<T>(T item4) => item4;            // item4 -> item5
    static System.Collections.Generic.List<T> G2<T>(System.Collections.Generic.List<T> list2) => list2; // list2 -> list3

    // Verbatim identifiers / keywords:
    static void K1(int @class) { }               // @class -> @class2
    static void K2(int @int5) { }                // @int5 -> @int6

    // Unicode identifiers:
    static void U1(int π) { }                    // π -> π2
    static void U2(int имя2) { }                 // имя2 -> имя3
    static void U3(int 名字007) { }               // 名字007 -> 名字8

    // Attribute on method (should still transform the parameter):
    [System.Diagnostics.Conditional("DEBUG")]
    static void A1(string debug9) { }            // debug9 -> debug10

    // Interface method (will also be transformed; note: not compiled with an implementation here)
    interface IFoo
    {
        void Do(string s1);                      // s1 -> s2
    }

    // Should remain UNCHANGED (not single-parameter MethodDeclarationSyntax):
    // - 0 params:
    static void Z0() { }
    // - 2 params:
    static void Z1(int a, int b) { }
    // - Constructor with 1 param (ConstructorDeclarationSyntax, not MethodDeclarationSyntax):
    public Test(int only1) { }
    // - Local function (your rewriter only visits MethodDeclarationSyntax, not local functions):
    static void HasLocal()
    {
        int Local(int loc1) => loc1;             // should remain unchanged
    }
}
