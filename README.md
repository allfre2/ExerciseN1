# ExerciseN1

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
