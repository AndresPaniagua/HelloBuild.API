# HelloBuild.API

## Project Description
HelloBuild.API is a .NET 6.0 web API designed for user management and performing queries on the GitHub platform. The API consists of three main controllers:

### TokenController
The TokenController provides authentication through the `/api/Token/Authentication` endpoint. It receives a JSON object containing the user's email and password. If the user exists in the database, an authentication token is generated (StatusCode 200). If the user is not found in the database, an error message is returned (StatusCode 400).

### GithubController
To use the methods in this controller, a valid authentication token is required. The controller has two endpoints:

- `/api/Github/GetRepositories`: Receives a JSON object that includes the GitHub username and a personal access token. It returns all repositories for that user on GitHub (StatusCode 200) or an error message in case of issues (StatusCode 400).

- `/api/Github/GetFavRepositories`: Also receives a JSON object with the GitHub username and a personal access token. In this case, it returns only the starred repositories on GitHub (StatusCode 200) or an error message if any errors occur (StatusCode 400).

### UserController
This controller has three methods, but only one of them requires authentication. The endpoints are:

- `/api/User/SaveUser`: Allows saving a user in the database without requiring authentication.

- `/api/User/UserRegistered`: Verifies if a user exists in the database without requiring authentication.

- `/api/User/GetUserInfo`: Requires authentication and searches for the information of a specific user using their email address.

It's essential to note that this API uses an in-memory SQL database, meaning the database exists only while the API is running. This provides flexibility and performance for user management and GitHub queries.

In summary, our .NET 6.0 API offers secure authentication, user management, and access to relevant GitHub information, all with an in-memory database for efficient operation. This API is built for the "HelloBuild.APP" ReactJS project.

## Installation
To run this API, build and run the project using .NET 6.0.

## Usage
You can use this API in conjunction with the "HelloBuild.APP" ReactJS project for user management and GitHub queries. Refer to the specific endpoints mentioned above for more details on how to interact with the API.

## Features
- Secure token-based authentication.
- User management and verification.
- GitHub queries and data retrieval.
- In-memory SQL database for efficient operation.
- Unit Tests
- Integration Tests

## Technologies Used
- .NET 6.0
- XUnit and Mock for unit testing
- JWT
- Entity Framework (Model First)
- LinQ

## Architecture
- Hexagonal
- DDD

## Credits
This project was created as part of an entrance test for the HelloBuild company.

## Project Status
This project is considered complete and ready for use.

## Contact
For any questions or inquiries, please contact:

- Andres Paniagua
- GitHub: [My profile](https://github.com/AndresPaniagua)

## Release Notes
- .NET 6.0 used in the project.
