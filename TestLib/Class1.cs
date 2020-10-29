using System;

namespace TestLib
{
   

    public class Class1
    {
        Class2 class2;
        int a;
        Boolean b;

        char k { get; set; }

        public class Class2
        {
            private string word;
            private int count;

            public Class2()
            {
                word = "Hello";
            }

            private string SayHello()
            {
                return word;
            }
        }

        public Class1(int a)
        {
            this.a = a;
        }

        public Class1()
        {
        }

        protected int Create()
        {
            return ++a;
        }
    }
    public static class ExtensionMethod
    {
        public static int CharCount(this Class1.Class2 str, char c)
        {
            int counter = 0;
            
            return counter;
        }

        public static int CharCounter(this string str, char c)
        {
            int counter = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == c)
                    counter++;
            }
            return counter;
        }
    }
}
