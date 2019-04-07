# Setting up your development environment

Launch external services.

```
docker-compose up
```

```
postgres: localhost:5432
seq: localhost:5341
```

Connect to postgres server and create the `triple_triad` database.

Run the database migrations

```
dotnet ef database update \
    -p src/TripleTriad.Data/TripleTriad.Data.csproj \
    -s src/TripleTriad.Web/TripleTriad.Web.csproj
```

Run the front end

```
(cd src/TripleTriad.Client && ng serve)
```

Run the back end

```
(cd src/TripleTriad.Web && dotnet watch run)
```
