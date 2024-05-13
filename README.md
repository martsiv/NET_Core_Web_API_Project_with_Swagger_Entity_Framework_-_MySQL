# NET Core Web API Project with Swagger, Entity Framework & MySQL

Project with clean architecture. I started the project on endpoints, but now it works on controllers. Controllers were added with unit tests first to the new branch and then merged to main.
The project includes Repository Pattern, Swagger Documentation, Custom HTTP Exception Handler, Validators, Custom File Logger. 
Connection string in the User Secrets json file wich ignored by Git. Added some Unit tests. 

OAuth Authorization.
Requirements:
- Add OAuth2.0 authorization to Client.
- Authorization Grant must be "Authorization Code" with PKCE + Refresh Token.
- Access Token must be by OpenID Standart (JWT).
Additionally for implementation:
- Create own Authorization server.
- Create at least one Resource server (Ideally, make 2 or more).
- Create functionality for registration Client applications.
