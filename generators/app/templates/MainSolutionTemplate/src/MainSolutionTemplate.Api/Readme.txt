﻿Urls
-------------------------

http://www.asp.net/signalr/overview/signalr-20/getting-started-with-signalr-20/tutorial-signalr-20-self-host


Install packages
----------------------

-- Install-Package Microsoft.AspNet.WebApi.Owin
-- Install-Package Microsoft.AspNet.SignalR.Owin

Install-Package log4net
Install-Package AutoMapper
Install-Package Owin.Extensions
Install-Package Microsoft.Owin.Host.HttpListener
install-package Microsoft.AspNet.WebApi.OData
install-package Microsoft.Owin.Security
install-package Microsoft.Owin.Cors
install-package Microsoft.AspNet.WebApi.Owin
install-package Microsoft.Owin.Host.SystemWeb


Install-Package Microsoft.Owin.Host.SystemWeb
Install-Package Owin.Extensions
Install-Package Microsoft.Owin.Host.HttpListener
Install-Package Nancy.Owin
Install-Package Microsoft.AspNet.SignalR.Core
Install-Package Microsoft.AspNet.SignalR.JS

Setup Owin
--------------------
cinst owinhost
[assembly: OwinStartup(typeof(WebApplication3.Startup))]