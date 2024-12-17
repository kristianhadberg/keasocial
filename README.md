
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
- **Backend Framework**: ASP.NET Core 6.0
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

### 1. Start MongoDB in a Docker Container
To run MongoDB in Docker, use the following command:

```bash
docker run --name mongo-keasocial -d -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=root -e MONGO_INITDB_ROOT_PASSWORD=admin mongo:latest
```

This will:
- Create a MongoDB container named `mongo-keasocial`.
- Map the default MongoDB port `27017` to your local machine.
- Set the root username to `root` and password to `admin`.

---

### 2. Update the Connection String
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

### 3. Build and Run the Application

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
To test the application, you can insert some mock data into MongoDB:

1. Open a MongoDB shell or GUI (like Compass).
2. Insert mock documents into the `Posts` and `Comments` collections:

```javascript
db.Posts.insertMany([
  {
    "PostId": 1,
    "UserId": 1,
    "Content": "Hello, this is my first post!",
    "CreatedAt": new Date(),
    "LikeCount": 0,
    "Comments": []
  },
  {
    "PostId": 2,
    "UserId": 2,
    "Content": "Anyone going to the lecture tomorrow?",
    "CreatedAt": new Date(),
    "LikeCount": 0,
    "Comments": []
  }
]);

db.Comments.insertMany([
  {
    "CommentId": 1,
    "PostId": 1,
    "UserId": 2,
    "Content": "Welcome to the platform!",
    "CreatedAt": new Date(),
    "LikeCount": 0
  }
]);
```

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

---

### Troubleshooting

1. **MongoDB Connection Error**:
   - Ensure MongoDB is running (`docker ps` to check the container).
   - Verify the connection string in `appsettings.json`.

2. **Port Conflicts**:
   - If port `27017` is in use, change the port mapping in the Docker command:
     ```bash
     -p 27018:27017
     ```

3. **Seed Data Not Showing**:
   - Use MongoDB Compass or a shell to verify the data in the correct database.

---

### Conclusion
This application combines relational, document, and graph databases to showcase the flexibility and power of modern database systems. By running MongoDB alongside our C# ASP.NET Core application, we achieved a scalable and adaptable backend capable of handling unstructured and hierarchical data.

Let me know if you need further clarification or assistance with setup! 🚀
