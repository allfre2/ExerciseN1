# ExerciseN1

Simple Authentication and Authorization using jwts through Microsofts "Identity".

## database

build the docker image

```
docker build -t sqlserver -f DatabaseDockerfile .
```

run the database docker image

```
docker run -d -p 1433:1433 sqlserver
```

apply migrations to database. From root directory in the repo:

```
dotnet ef database update -- --environment Local
```

## main program

create api keys using a database management program (I used Dbeaver on linux)

configure environment api key in postman

create roles using the roles endpoint

register a user using te register endpoint

test the login and verify jwt endpoints

## Endpoints

```
POST /api/auth/login
```
Obtener un jwt token y un refresh token mediante un clientId y un clientSecret.

```
GET /api/auth/verify
```
Verificar la validez de un token. Si el mismo ha vencido la respuesta contiene un header que lo indica (Token-Expired: True).

```
POST /api/auth/refresh
```
Obtener un nuevo token una vez el mismo haya vencido mediante un refreshToken.

```
POST /api/auth/register
```
Obtener nuevos clientId y clientSecret mediante un api key.

```
GET /api/role
```
Obtener una lista de roles en el sistema. Se necesita un api key.

```
GET /api/role/{user}
```
Obtener una lista de los roles de un "usuario" en el sistema. Se necesita un api key.

```
POST /api/role
```
Crear nuevo rol. Se necesita un api key.
