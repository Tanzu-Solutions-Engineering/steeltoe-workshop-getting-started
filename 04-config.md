
# Using an external configuration provider

## Goal

Setup an external git repo holding configuration values and using Spring Config to retrieve the values, in a .NET Core application.

## Expected Results

With a running instance of Spring Config server, navigate to an endpoint in a .NET Core application and see the values output.

> For this exercise a Spring Config server have already been initialized. The settings have been preloaded below.

## Get Started

*For this lab all the instructions will be against `WeatherService` project.*

To communicate with an external config server we're going to need to add a client to the previously created application. We're also going to add placeholder provider that allows us to define config values by referencing OTHER config sections as variables, reducing duplication.  

### Terminal

```powershell
dotnet add WeatherService\WeatherService.csproj package Steeltoe.Extensions.Configuration.ConfigServerCore
dotnet add WeatherService\WeatherService.csproj package Steeltoe.Extensions.Configuration.PlaceholderCore
```

---



## Modify controller to read config

Lets add a method to our controller that reads and displays a config value. Add the following method to `WeatherService` class:

```csharp
using Microsoft.Extensions.Configuration;
...
  
[HttpGet("location")]
public string GetLocation([FromServices] IConfiguration config) => config.GetValue<string>("location");
```

 

In 'appsettings.json' **add** the following json just below to the `Spring` section. Config server reads which environment it should config for by reading `Spring:Cloud:Config:Env` configuration key. We're going to map its value with Steeltoe placeholder provider to `ASPNETCORE_ENVIRONMENT`, which is the default way for configuring environments in ASP.NET Core.

```json
{
...
  "Spring": {
    "Application": {
      "Name": "WeatherService"
    },
    "Boot": {
      "Admin": {
        "Client": {
          "Url": "http://localhost:8080",
          "Metadata": {
            "user.name": "actuatorUser",
            "user.password": "actuatorPassword"
          }
        }
      }
    },
    "Cloud": {
      "Config": {
        "Env": "${ASPNETCORE_ENVIRONMENT}"
      }
    }
  },
...
}
```

> Notice the value of `spring:application:name` in the json. This value of "WeatherService" will be used to connect the correct values in the Spring Config server, as the config server can be servicing multiple apps.

## Run Config Server

Application configuration has been externalized into a Git repo, and the actual values are served by Spring Config Server. We can launch Spring Config Server and point it to our config git repo from which it will serve values. Lets do that now

From the `c:\workshop\services` folder, run `_run-config-server.bat` to start config server.

> Edit the bat file. Notice how the backend is configured by setting environmental variable. 

 

------



## Run the application

Lets launch WeatherService 

Clicking the `Debug > Start Debugging` top menu item. With the application running, access `http://localhost:5000/weatherforecast/location`.

You should see the value of `Toronto` displayed. Now examine the config repo we used for config server:https://github.com/Tanzu-Solutions-Engineering/steeltoe-workshop-config-repo, specifically `WeatherService.yml` and `WeatherService-Development.yml`. By default, when launching locally, the app starts in "Development" environment. Now lets change it to run under production environment.

Edit `Properties\launchSettings.json` and change value for `ASPNETCORE_ENVIRONMENT` to `Production` under `WeatherService` profile.

Relaunch the app and check `http://localhost:5000/weatherforecast/location` again. You should see `New York`. 



---

## Summary

With an existing Spring Config server running that was configured to retrieve values from a yaml file, we added a Spring Config client to our application and output the retrieved vale. With this architecture in place you can now do things like updating the yaml file and visit the `/actuator/refresh` management endpoint in the application. This will automatically refresh values within the application without and down time (or restart). You could store a server's connection name in the yaml and have the application retrieve the value. As the application moves through different environments (dev, test, staging, prod) the connection value can change, but the original tested application stays unchanged.

We've just begun to scratch the surface of what Spring Config can really do and all it's many features. Learn more about config in the [Steeltoe docs](https://steeltoe.io/api/v3/configuration/config-server-provider.html).
