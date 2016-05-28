##Introduction

A simple Asp.Net Web API 2 application that carries out lookup on https://www.themoviedb.org/ to retrieve various meta data.

##Aims

Capture various notes for myself in developing these style of apps, including:
* Composing app via IoC
* Testability of various layers
* Well defined REST endpoints & return information
* Secure storing config data such as API key
* Improve upon hardcoded absolute path to secrets.(todo)
* Auto Mapping from user data/public object to domain model
* Persistant (todo)
* Cloud hosting (todo)
* Web page that renders information stored (todo)

##Build

nuget restore
msbuild /p:Confiration=debug /t:rebuild .\XGMovies.sln

##Test
mstest /testcontainer:.\XGMoviesTest\bin\Debug\XGMoviesTest.dll

##Runtime

Note that the ASP.NET and tests require the existance of "D:\XGMoviesSecrets.config" appsetting in which the api key for themoviedb.org is placed as follows:

```xml
<appSettings>
	<add key="TheMovieDbOrgApiKey" value="xxxxxxx" />
</appSettings>

```

Reference to D:\XGMoviesSecrets.config can be found in "XGMovies\Web.config" and "XGMoviesTest\app.config".

 

