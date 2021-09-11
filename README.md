# LinenAndBird

## APIs

Because .NET is **SOOO** massive, Microsoft has separated it into several sections. One of the most commonly used sections for web developers is [MVC](https://docs.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-2.2). This is *very creatively* 🙄 named after the more general architectural pattern known as [Model-View-Controller](https://www.wikiwand.com/en/Model%E2%80%93view%E2%80%93controller) (MVC).

There is a section in MVC to build APIs. This is the section that we'll be working with for right now. You can think of the View (the V section of MVC) of a API as JSON that is returned to the client or browser.

### Controllers

Controllers, in a general sense, are classes that have the specific purpose of processing an incoming request, calling any additional logic, and returning a response to that request.

You can think of Controllers as an "orchestration layer" that will be responsible for accessing code in other parts of your application and handling the result of that code.

### Models

Models are a representation of your data in C#. Functionally, they are just classes with nothing but properties in them. You'll also hear people call these classes with only properties "POCOs" for Plain Old C# Object

For instance, if your application was tracking doctors in a hospital, then one of your models might look something like this.

```csharp
public class Doctor
{ 
	public int Id { get; set; } 
	public string Name { get; set; } 
	public string Specialty { get; set; } 
	public string Gender { get; set; } 
	public int Rating { get; set; } 
	public string Department { get; set; }
}
```

### Attributes

[Attributes](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/) are a feature that can be used anywhere in C#, but we'll be discussing them in the [context of WebAPI](https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-2.2). They are identified by square brackets, like those directly above the method signature of a controller.

```csharp
[HttpGet] //<-- attribute
public IEnumerable<User> GetUsers()
{
  // get all the users
}
```

Attributes can also be used at the class level.

```csharp
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{ 
	// get User methods
}
```

Attributes can also be used directly inside a parameter list

```csharp
[HttpPost]
public IActionResult CreateUser([FromBody] User newUser)
{ 
	// save a new User
}
```
