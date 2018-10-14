# SignalR without web and JS

### 1. About
 This repository is created just to show how to bind SignalR trough native c# applications, without any web application or JavaScript.
 
### 2. In Short
 In Server-side, we create a console application with owin and SignalR dependency. Then create a startup class, and of course a hub. In console main method, just call for static class Microsoft.Owin.Hosting.WebApp's Start method which accepts server URL as parameter.
 In Client side, we have two properties, one is HubProxy, and HubConnection. By HubConnection, we connect to server, and by HubProxy we dynamicly Invoke server methods with WebSockets.
