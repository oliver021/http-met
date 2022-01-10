using NUnit.Framework;
using System;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace FluentRest.Test
{
    interface IObject2
    {
        public int Value(int d);
    }

    class Test2
    {
        public T TestDynamic<T>()
        {
            Func<int,int> d = x => x + 11;

            return null;
            
        }
    }
    class Prev
    {
        [Test] 
        public void Test()
        {
            var d = new Test2();

            Console.WriteLine(d.TestDynamic<IObject2>().Value(1));
        }
    }
}
