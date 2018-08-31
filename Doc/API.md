# API

## Авторизованные запросы:

Header: 

`X-Login: <login>`

`X-Token: <password>` :)


## API Controllers

### /api/file

GET /api/file - все файлы

GET /api/file?type=\<type\> - файлы типа
* doc,image,audio,video,archive

````
[{
  "id": 1,
  "name": "name",
  "type": "image/jpeg",
  "size": 123454,
  "description": "description",
  "link": "http://ipfs",
  "modified": "2018-08-29T13:17:31.349Z",
  "created": "2018-08-29T13:17:31.349Z"
},
{
  "id": 1,
  "name": "name",
  "type": "image/jpeg",
  "size": 123454,
  "description": "description",
  "link": "http://ipfs",
  "modified": "2018-08-29T13:17:31.349Z",
  "created": "2018-08-29T13:17:31.349Z"
}]
````

GET /api/file/1 - инфо о файле
````
{
  "id": 1,
  "name": "name",
  "type": "image/jpeg",
  "size": 123454,
  "description": "description",
  "link": "http://ipfs",
  "modified": "2018-08-29T13:17:31.349Z",
  "created": "2018-08-29T13:17:31.349Z"
}
````

POST /api/file - добавить  файл
````
{
  "link": "C://files/document.txt",
}
````

PUT /api/file/1 - редактировать файл
````
{
  "id": "id",
  "name": "name",
  "description": "description",
}
````

DELETE /api/file/1 - удалить файл

### /api/user

POST /api/user - зарегаться
````
{
  "login": "login",
  "password": "password",
  "firstname": "name",
  "lastname": "name",
  "info": "info",    <----- не обязательное
}
````

GET /api/user - инфо о профиле (инфо о кошельке отображается только админам)
````
{
  "user": {
    "login": "login",
    "password": "",
    "firstname": "name",
    "lastname": "name",
    "info": "info",
    "isadmin": true
  },  
  "wallet": {
    "address": "0x1000000000",
    "wei": "1000000000000",
    "ether": "100"
  }
}
````

PUT /api/user - редактировать профиль
````
{
  "firstname": "name",
  "lastname": "name",
  "info": "info",
}
````

## Ok & BadRequest

200 Ok()
````
{
  "success": "File #4 was deleted",
}
````
````
{
  "error": "Wrong credentials",
}
````
