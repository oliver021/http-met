# HttpMet

## A .Net Library to use fluent and easy methods to consume rest api OR any other http endpoint

This library dedicated to facilitating the consumption of web api and any 
type of endpoint using  the .Net platform, this is an effort to contribute to the 
increasingly large catalog of options that exist for this, but here are some 
exclusive features offered by this library.

- **Template Request**.  This is a way to build http request from delegate as template. It basically consists
of establishing a request model with some parameters that are established in a delegate that do not change
and other parameters that vary in each call that is later made to said delegate.
- **Object as Request.** It consists of relating a class with an http 
request by inserting attributes to the class that defines concepts, 
the path, query params, body data, etc. This feature provide 
a easy way to build a SDK for web api.
- **Internal Service Provider**. Out the box this library has a global instance of service provider 
that allow use quickly in projects without ASP.NET Core or that not has a service infrastructure. 

Other features are also...

- **Complete API for Consuming Rest**.  The `RestClient` class provides a complete set of methods
for querying the most common methods of a web service. Supports GET, POST, PUT, PATCH, DELETE 
with ability and custom methods.
- **Support for all**. Install from NuGet on any .Net platform. This library uses .Net Standard.
- **Care tracking the http client lifecycle**. This library has complete support for http factory.
- **Custom serializer body request**. Support for your custom data serializer process is covered by 
two cases, in global instance and individual instances.
- **Extension method to help with requests**. There is a group of extensions method to make easy 
http request from simple calls.
- **Support for files upload**. There is an additional support for file uploading.
- **Async friendly**. All operations use async, await keywords for all methods.


## Quick Sample

The simple usage of this library is as shown in the following code block.

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

To use this library within ASP.NET Core there is the usual way of adding it via `IServiceCollection` 
through a provided extension method: `AddRestClient`. This practice helps maintain the use of the 
`http-factory` which is responsible for managing the lifecycles of the http client instances.

```c#

public void ConfigureServices(IServiceCollection services){
    services.AddRestClient();
}

```

This way you can already expect the service provider to resolve instances of `RestClient` 
in the constructors of controllers and other services in your application.