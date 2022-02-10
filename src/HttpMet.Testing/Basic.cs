using NUnit.Framework;
using System;
using HttpMet;
using System.Threading.Tasks;
using System.Linq;

namespace HttpMet.Testing
{
    public class Tests
    {
        /// <summary>
        /// put your api key here for <see cref="https://gorest.co.in"/> mock api rest online for tests
        /// </summary>
        public const string GORestToken = "";
        private const string TestStatusActive = "active";
        private const string TestStatusInactive = "inactive";
        private const string TestQueryName = "Jessica";

        /// <summary>
        /// Basic intro test
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestIntro()
        {
            var rest = RestClient.New;

            var result = await rest.GetAsync<UserSample>("https://gorest.co.in/public/v1/users");

            /* Test by show string
             
                Console.WriteLine(result.Meta.Pagination.Total);
                Console.WriteLine(result.Meta.Pagination.Page);

                Console.WriteLine(string.Join(',', result.Data.Take(6).Select(x => x.Name)));
             */

            Assert.IsTrue(result is not null);
            Assert.IsTrue(result.Data is not null);
            Assert.IsTrue(result.Meta.Pagination is not null);
        }

        /// <summary>
        /// This is a test for post, get, put and delete operation
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestRequestTemplate()
        {

            /*
             * These test methods help you build delegates that execute http
             * requests as templates based on arguments and parameters you set to them.
             * 
             */

            // create template for pagination
            var paginate = RestFactory.Request<int, UserSample>((page, request) =>
            {
                request.Url($"https://gorest.co.in/public/v1/users?page={page}");
            });

            // create template for search
            var search = RestFactory.Request<string, string, UserSample>((term, status, request) => {
                request.Url($"https://gorest.co.in/public/v1/users?name={term}&status={status}");
            });

            Assert.IsNotNull(await paginate(3));

            var users = await search(TestQueryName, TestStatusActive);

            // test result
            Assert.AreEqual(users.Data[0].Status, Status.Active);
            Assert.IsTrue(users.Data.All(x => x.Name.StartsWith(TestQueryName)));

            // test again execution with different parameters
            var users2 = await search(TestQueryName, TestStatusInactive);
            Assert.IsTrue(users2.Data.All(x => x.Status == Status.Inactive));
        }

        /// <summary>
        /// This is a test for post, get, put and delete operation
        /// </summary>
        /// <returns></returns>
        [Test]
        public Task TestEntityCycle()
        {
            return null;
        }

        [Test]
        public Task TestEntityCycleWithoutGenericMethods()
        {
            return null;
        }
    }
}