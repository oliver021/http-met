using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

namespace HttpMet.Testing
{
    public delegate string FixtureDelegate(string data);
    public delegate Task<string> FixtureDelegateAsync(string data);

    class Research
    {
        [Test]
        public void TestResearchAboutDelegate()
        {

            Execute(x => $"hello {x}");
            ///ExecuteDelegate(x => $"hello {x}");
        }

        [Test]
        public  void TestResearchRequestDelegate()
        {
            var request = RestFactory.RequestFromDelegate<FixtureDelegateAsync, string, string>((x, msg) =>
            {
                // nothing do stuff
                // only create delegate
            });

            Assert.IsAssignableFrom<FixtureDelegateAsync>(request);
        }

        private void Execute(Func<string, string> func)
        {
            FixtureDelegate fixture = (FixtureDelegate)func.Method.CreateDelegate(typeof(FixtureDelegate), null);
            Console.WriteLine(fixture.Invoke("world"));
        }

        private static void ExecuteDelegate(FixtureDelegate func)
        {
            Console.WriteLine(func.Invoke("world"));
        }
    }
}
