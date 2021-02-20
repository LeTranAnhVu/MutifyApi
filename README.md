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
