# AuthService

Este repositorio contiene una implementaci�n sencilla de un servicio de autenticaci�n y autorizaci�n basado solamente en ASP.NET Core Identity. La necesidad surge del proyecto de Remesas 2.0 y de no tener que reimplementar l�gica de oauth2 en cada nuevo servicio.

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

```
GET /api/application
```
Devuelve una lista de aplicaciones registradas en el servicio. Se necesita un api key.

```
GET /api/application/{name}
```
Devuelve un modelo con los datos de configuraci�n de la aplicaci�n registrada con nombre "name". Se necesita un api key.

```
POST /api/application
```
Sirve para registrar una aplicaci�n y su configuraci�n de duraci�n de token y refreshToken


