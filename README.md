# Daemon-Client-Command-API

A .Net Core console app for interacting with the CommandAPI. 

## About
I wanted to try out a few different ideas so I created this basic .Net Core 3.x console app to interact with the Command API project. The project uses Dependency Injection, Azure Active Directories for Authentication and a custom CLI command parser (kind of...). 

## Command Parser
I thought it would be a cool if the user could enter the name of the method they want (with parameters) to the console and a dedicated CommandParser class would invoke this method automatically. This is different of course to starting the console application with arguments *( main(string[] yourArguments) etc.)*, for which there are many libraries. I was interested in parsing string input from the user while the console application was running.

### How It Works

#### Custom Attributes
This project uses dependency injection, so we're programming against interfaces instead of concrete classes. If the methods in the interface are decorated using the CLIMethodAttribute then they can be called from the command line.

```csharp
   public interface ITestInterface
    {
        [CLIMethod("Test Method", "A test method with no input parameters")]
        void TestMethod();

        [CLIMethod("Test Method", "A test method with a single int input parameter", ":InputParameterInt")]
        void TestMethod(int intInput);

        [CLIMethod("Test Method", "A test method with a single string parameter", ":InputParameterString")]
        void TestMethod(string intInput);

        [CLIMethod("Test Method", "A test method with two input parameters" ,":InputParameterInt :InputParameterInt")]
        void TestMethod(int firstInput, int secondInput);
    }
```
The custom attribute also allows us to store additional information about each method such as a description and the input parameter list. We are also able to use method overloading (shown above).

#### Configuring The Command Parser
In the program.cs once we have configured the service provider we can configure the CommandParser for each abstraction/Interface so that their methods can be called from the CLI by the user.

```csharp

  var commandParser = serviceProvider.GetService<ICommandParser>();
  
  //Configuring what methods are invokable.
  //Using the interface to get the decorated methods and the class instance to invoke them.
  
  commandParser.ConfigureForCLI<IUpdateSetting>(serviceProvider.GetService<IUpdateSetting>());
  commandParser.ConfigureForCLI<IDaemonHttpClient>(serviceProvider.GetService<IDaemonHttpClient>());
  
```
#### Example
To invoke the method decorated as:

```csharp

   [CLIMethod("Test Method", "A test method with two input parameters" ,":InputParameterInt :InputParameterInt")]
   void TestMethod(int firstInput, int secondInput);

```
The console input would be:

```console
   TestMethod :-2 :10
```

