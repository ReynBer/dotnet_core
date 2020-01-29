# tree_api


## Summary
The goal is to expose an API of generation trees.
It s not persisted for the moment...


### Local (without docker)
Prerequesites : dotnet core (>=3.1) should be installed on the machine
- `dotnet build`
- `dotnet run --project Core.Api/Core.Api.csproj` the service is running on the ports 5000 (http) and 5001 (https)
You can consult the [local swagger](https://localhost:5001/swagger/index.html)

### Local (with docker)
- `dotnet clean`
- `dotnet publish -c release -o ./.publish`
- `docker build -f Dockerfile -t tree ./`
- `docker images` -> normally the image tree is available
- `docker run -p 80:80 -it tree`  
You can consult the [local swagger](http://localhost/swagger/)
