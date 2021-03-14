# MutifyApi

#### Port:
- https://localhost:5001 or http://localhost:5000

#### Migration cmds:
- Create migration: 
```
dotnet ef migrations add InitialCreate
```

- Add migration: 
```
dotnet ef migrations add <DescribeWhatYouChange>
```

- Update dabase: 
```
dotnet ef database update
```
#### Postgres connection:
- Docker cmd: 
``` 
docker run -d --name mutify-postgres -e POSTGRES_PASSWORD=admin -e POSTGRES_USER=admin -e POSTGRES_DB=mutify -p 5432:5432 -h 127.0.0.1 postgres
```

- Connection string: 
```
"MutifyContextPostgresql": "Host=127.0.0.1;Database=mutify;Username=admin;Password=admin"
```
#### Docker compose:
- Architecture:
  - nginx : reverse proxy port 80
  - api : dotnet 5 local port 8000
  - postgres: database port 5432.
    
- Note: there is DevopsConfig folder, used to store conf files for containers
  
- Build:
```
docker-compose build
```
- Run:
```
docker-compose up
```
- Test:
```
http://localhost/test
```
(Test database + server work)
```
http://localhost/api/tracks
```