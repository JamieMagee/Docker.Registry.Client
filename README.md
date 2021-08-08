# Docker.Registry.Client

![Nuget](https://img.shields.io/nuget/v/Docker.Registry.Client?style=for-the-badge)
![GitHub Workflow Status (branch)](https://img.shields.io/github/workflow/status/JamieMagee/Docker.Registry.Client/build/main?style=for-the-badge)

A .NET client library for interacting with a Docker Registry API (v2 only).

## Setup

Add via the dotnet CLI:

```
dotnet add package Docker.Registry.Client
```

## Usage

```csharp
var configuration = new RegistryClientConfiguration("localhost:5000");

using (var client = configuration.CreateClient())
{
    await client.System.PingAsync();
}
```
