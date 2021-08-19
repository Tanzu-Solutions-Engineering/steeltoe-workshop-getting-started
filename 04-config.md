---
uid: guides/get-to-know-steeltoe/exercise4
_disableContribution: true
_disableToc: false
_disableFooter: true
_homePath: "./index.html"
_disableNav: true
_hideTocVersionToggle: true
---

[home-page-link]: index.md
[exercise-1-link]: exercise1.md
[exercise-2-link]: exercise2.md
[exercise-3-link]: exercise3.md
[exercise-4-link]: exercise4.md
[summary-link]: summary.md

| [<< Previous Exercise][exercise-3-link] |     |
| :-------------------------------------- | --: |

# Using an external configuration provider

## Goal

Setup an external git repo holding configuration values and using Spring Config to retrieve the values, in a .NET Core application.

## Expected Results

With a running instance of Spring Config server, navigate to an endpoint in a .NET Core application and see the values output.

> [!NOTE]
> For this exercise a Spring Config server have already been initialized. The settings have been preloaded below.

## Get Started

To communicate with an external config server we're going to need to add a client to the previously created application.

# [Visual Studio](#tab/visual-studio)

Right click on the project name in the solution explorer and choose "Manage NuGet packages...". In the package manger window choose "Browse", then search for `Steeltoe.Extensions.Configuration.ConfigServerCore`, and install.

<img src="images/vs-add-configserver.png" alt="Add configuration server library" width="100%">

# [.NET CLI](#tab/dotnet-cli)

```powershell
dotnet add package Steeltoe.Extensions.Configuration.ConfigServerCore
```

---

## Implement Spring Config client

Open "Program.cs" and implement a Spring Config client in the host builder.

```csharp
using Steeltoe.Extensions.Configuration.ConfigServer;
```

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .AddConfigServer()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .AddSteeltoe();
```

## Modify controller to read config

Lets add a method to our controller that reads and displays a config value. Add the following method to `WeatherService` class:

```csharp
using Microsoft.Extensions.Configuration;
...
  
[HttpGet("location")]
public string GetLocation([FromServices] IConfiguration config) => config.GetValue<string>("location");
```

 

In 'appsettings.json' add the following json just below the "sqlserver" section. This should be preloaded with the correct connection values of a Spring Config server.

```json
,"Spring": {
  "Application": {
    "Name": "myapplication"
  },
  "Cloud": {
    "Config": {
      "Uri": "http://localhost:8888",
    }
  }
}
```

> [!NOTE]
> Notice the value of 'spring:application:name' in the json. This value of "myapplication" will be used to connect the correct values in the Spring Config server.

## Run Config Server

Application configuration has been externalized into a Git repo, and the actual values are served by Spring Config Server. We can launch Spring Config Server and point it to our config git repo from which it will serve values. Lets do that now

# [Java app](#tab:java)

First lets set environmental variables for location of our config repo



```powershell
PS> $env:SPRING_CLOUD_CONFIG_SERVER_GIT_URI="https://github.com/macsux/workshop-config-repo.git"
PS> java -jar config-server-2.5.3.jar 
```



## [Docker](#tab:docker)

Run the following command

```
docker run -e SPRING_CLOUD_CONFIG_SERVER_GIT_URI=https://github.com/macsux/workshop-config-repo.git -p:8888:8888 --rm -it steeltoeoss/config-server
```

------



## Run the application

With the data context in place, we are ready to see everything in action. Run the application.

# [Visual Studio](#tab/visual-studio)

Clicking the `Debug > Start Debugging` top menu item. You may be prompted to "trust the IIS Express SSL certificate" and install the certificate. It's safe, trust us. Once started your default browser should open and automatically load the weather forecast endpoint.

<img src="images/vs-run-application.png" alt="Run the project" width="100%">

# [.NET CLI](#tab/dotnet-cli)

Executing the below command will start the application. 

```powershell
dotnet run
```

---

With the application running, access `http://localhost:5000/weatherforecast/location`.

If

## See the config values output

To execute the values endpoint, replace `WeatherForecast` with `values` in the browser address bar. The values will be retrieved from the Spring Config server and output in the window.

```json
["hello from development config"]
```

## Stop the application

# [Visual Studio](#tab/visual-studio)

Either close the browser window or click the red stop button in the top menu.

# [.NET CLI](#tab/dotnet-cli)

Use the key combination "ctrl+c" on windows/linux or "cmd+c" on Mac.

---

## Summary

With an existing Spring Config server running that was configured to retrieve values from a yaml file, we added a Spring Config client to our application and output the retrieved vale. With this architecture in place you can now do things like updating the yaml file and visit the `/actuator/refresh` management endpoint in the application. This will automatically refresh values within the application without and down time (or restart). You could store a server's connection name in the yaml and have the application retrieve the value. As the application moves through different environments (dev, test, staging, prod) the connection value can change, but the original tested application stays unchanged.

We've just begun to scratch the surface of what Spring Config can really do and all it's many features. Learn more about config in the [Steeltoe docs](/api/v3/configuration/config-server-provider.html).

| [<< Previous Exercise][exercise-3-link] | [Workshop Summary >>][summary-link] |
| :-------------------------------------- | ----------------------------------: |
