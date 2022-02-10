# FluentRest

## A .Net Library to use fluent method to set rest request



The simple use of this library is like how to show in the next code block.

```c#
var rest = RestClient.New;

            var result = await rest.GetAsync<UserSample>("https://gorest.co.in/public/v1/users");

            /* Show result 
                Console.WriteLine(result.Meta.Pagination.Total);
                Console.WriteLine(result.Meta.Pagination.Page);
                Console.WriteLine(string.Join(',', result.Data.Take(6).Select(x => x.Name)));
             */
```

In this example you get a instance from global factory but there are many way to make a 
instance of the `RestClient`. This class has all classic rest method to consume Rest API 
or any http endpoint. The library uses the http factory from `Microsoft.Extensions.Http` 
to keep healthy performance in runtime. The example uses https://gorest.com to make requests.
