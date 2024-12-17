
# KEASOCIAL

---

## Project Overview
This project is a backend application designed to combine features of a **social network** with a **lecture management platform** for an educational institution. The backend provides functionality for:
- User management
- Creating and managing social posts and comments.
- Managing lectures, classes, and student enrollment.

The application uses **three types of databases** to demonstrate their respective strengths:
1. **MySQL** (Relational Database)
2. **MongoDB** (Document Database)
3. **Neo4j** (Graph Database)

---

## Technology Stack
- **Backend Framework**: ASP.NET Core 8.0
- **Programming Language**: C#
- **Databases**:
   - MySQL (Relational)
   - MongoDB (Document-based)
   - Neo4j (Graph-based)
- **Testing Tools**: Swagger & Postman
- **Containerization**: Docker

---


## Prerequisites
Before running the project, ensure you have the following installed:
1. **.NET 6.0 SDK**: [Download Here](https://dotnet.microsoft.com/download)
2. **MongoDB**: Installed locally or running in a Docker container.
3. **Docker**: [Install Docker](https://www.docker.com/get-started)
4. **MongoDB Compass** (optional): A GUI for managing MongoDB data.

---

## How to Run the Application (MongoDB Configuration)

### 1. Switch to the 'MongoDb' branch in the repository.
```bash
git checkout MongoDb
```

### 2. Start MongoDB in a Docker Container
To run MongoDB in Docker, use the following command:

```bash
docker run --name mongo-keasocial -d -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=root -e MONGO_INITDB_ROOT_PASSWORD=admin mongo:latest
```

This will:
- Create a MongoDB container named `mongo-keasocial`.
- Map the default MongoDB port `27017` to your local machine.
- Set the root username to `root` and password to `admin`.

---

### 3. Update the Connection String
Edit the `appsettings.json` file in your project to include the MongoDB configuration:

```json
{
  "ConnectionStrings": {
    "MongoDbConnection": "mongodb://root:admin@localhost:27017"
  },
  "DatabaseSettings": {
    "MongoDatabaseName": "keasocial"
  }
}
```

Here:
- `root` and `admin` are the MongoDB credentials.
- The database name is set to `keasocial`.

---

### 4. Build and Run the Application

1. **Restore Dependencies**:
   Open a terminal in the project folder and run:
   ```bash
   dotnet restore
   ```

2. **Run the Application**:
   Start the application using:
   ```bash
   dotnet run
   ```

3. **Verify MongoDB Integration**:
   - The application will connect to MongoDB and use it for posts and comments.
   - You can test the endpoints using Swagger at:
     ```
     http://localhost:5000/swagger
     ```

---

### Seeding Mock Data in MongoDB
The application is set up to automatically insert mock data the first time the application is run. If more is needed it can modified into the MongoDbSeeder.cs:



---

### Key Endpoints (Swagger)

| **Endpoint**                  | **Method** | **Description**                          |
|-------------------------------|------------|------------------------------------------|
| `/api/posts`                  | GET        | Fetch all posts.                         |
| `/api/posts/{postId}`         | GET        | Fetch a specific post.                   |
| `/api/posts`                  | POST       | Create a new post.                       |
| `/api/{postId}/comments`      | GET        | Get comments for a specific post.        |
| `/api/{postId}/comments`      | POST       | Add a comment to a specific post.        |
| `/api/{postId}/comments/{id}` | DELETE     | Delete a specific comment.               |

---               |

---

## How to Run the Application (Neo4j Configuration)

This section provides instructions for running Neo4j in a Docker container and configuring it for integration with the **KEASOCIAL** backend application.

### 1. Start Neo4j in a Docker Container
To run Neo4j in Docker, use the following command:

```bash
docker run --name neo4j-keasocial -d -p 7687:7687 -p 7474:7474 -e NEO4J_AUTH=neo4j/password neo4j:latest
```

### 2. Switch to the 'neo4j' branch in the repository.
```bash
git checkout neo4j
```

### 3. Update the Connection String
Edit the `appsettings.json` file in your project to include the Neo4J configuration:

```json
  "Neo4j": {
    "Uri": "bolt://localhost:7687",
    "Username": "neo4j",
    "Password": "password"
  },
```

### 4. Build and Run the Application

1. **Restore Dependencies**:
   Open a terminal in the project folder and run:
   ```bash
   dotnet restore
   ```

2. **Run the Application**:
   Start the application using:
   ```bash
   dotnet run
   ```



---

### Seeding Mock Data in Neo4j
To seed the database with data, copy the Cypher script from 'keasocial/cypher/datagen.txt'
You can run this cypher script in the neo4j browser available at 'http://localhost:7474/browser' when your neo4j docker container is running.
This script will seed the database with nodes and create the needed relationships between nodes.

