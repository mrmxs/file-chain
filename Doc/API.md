# API

## Авторизованные запросы:

Header: 

`X-Login: <login>`

`X-Token: <password>` :)


## API Controllers

### /api/file

GET /api/file - все файлы

GET /api/file?type=\<type\> - файлы типа
* text,image,audio,video,other

````
[{
  "id": 1,
  "name": "name",
  "type": "image/jpeg",
  "size": 123454,
  "description": "description",
  "modified": "2018-08-29T13:17:31.349Z",
  "created": "2018-08-29T13:17:31.349Z"
},
{
  "id": 1,
  "name": "name",
  "type": "image/jpeg",
  "size": 123454,
  "description": "description",
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
  "link": "http://ipfs"
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
}
````

GET /api/user - инфо о профиле
````
{
  "login": "login",
  "password": "",
  "firstname": "name",
  "lastname": "name",
  "info": "info",
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
