# HackathonManager API Documentation

This document provides comprehensive documentation for all endpoints available in the HackathonManager API.

**Base URL:** `{Base-URL}/api`

---

## Table of Contents

1. [Authentication](#authentication)
2. [Authorization & Roles](#authorization--roles)
3. [Endpoints](#endpoints)
   - [Auth Endpoints](#auth-endpoints)
   - [Users Endpoints](#users-endpoints)
   - [Hackathons Endpoints](#hackathons-endpoints)
   - [Teams Endpoints](#teams-endpoints)
   - [Submissions Endpoints](#submissions-endpoints)
   - [Evaluations Endpoints](#evaluations-endpoints)

---

## Authentication

The API uses JWT (JSON Web Token) authentication. To access protected endpoints:

1. **Obtain a Token** by calling the login endpoint
2. **Include the Token** in the `Authorization` header as a Bearer token:
   ```
   Authorization: Bearer <your_jwt_token>
   ```

---

## Authorization & Roles

The API supports the following user roles:

- **Admin** - Full access to user management and hackathon administration
- **Mentor** - Can create evaluations and manage evaluations for submissions
- **Participant** - Can participate in hackathons, create teams, and make submissions

---

## Endpoints

### Auth Endpoints

#### 1. Get Token (Login)
Authenticate a user and receive a JWT token.

- **Method:** `POST`
- **Endpoint:** `/api/auth/token`
- **Query Parameters:**
  - `username` (string, required) - The username of the user
  - `password` (string, required) - The password of the user
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** JWT token (string)
- **Error Responses:**
  - **Code:** `401 Unauthorized` - Invalid credentials

**Example:**
```bash
curl -X POST "{Base-URL}/api/auth/token?username=john&password=password123"
```

---

#### 2. Register User
Register a new user in the system.

- **Method:** `POST`
- **Endpoint:** `/api/auth/register`
- **Query Parameters:**
  - `username` (string, required) - Unique username
  - `displayName` (string, required) - Display name of the user
  - `password` (string, required) - Password for the account
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** `"User Registered Successfully"`
- **Error Responses:**
  - **Code:** `400 Bad Request` - Invalid input or user already exists

**Example:**
```bash
curl -X POST "{Base-URL}/api/auth/register?username=john&displayName=John%20Doe&password=password123"
```

---

#### 3. Change Password
Change the password of the authenticated user.

- **Method:** `PUT`
- **Endpoint:** `/api/auth/password`
- **Authentication:** Required (Bearer token)
- **Authorization:** Any authenticated user
- **Request Body:**
  ```json
  {
	"currentPassword": "oldPassword123",
	"newPassword": "newPassword123"
  }
  ```
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** `"Password Changed Successfully"`
- **Error Responses:**
  - **Code:** `400 Bad Request` - Invalid current password or weak new password

---

### Users Endpoints

**Note:** All Users endpoints require **Admin** role authorization.

#### 1. Get All Users
Retrieve a list of all users with optional filtering.

- **Method:** `GET`
- **Endpoint:** `/api/users`
- **Authentication:** Required (Bearer token)
- **Authorization:** Admin role required
- **Query Parameters (Filter):**
  - `Name` (string, optional) - Filter by username or display name
  - `Role` (string, optional) - Filter by user role
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** Array of user objects
  ```json
  [
	{
	  "id": 1,
	  "userName": "john",
	  "displayName": "John Doe",
	  "role": "Participant"
	}
  ]
  ```

---

#### 2. Get User by ID
Retrieve a specific user by their ID.

- **Method:** `GET`
- **Endpoint:** `/api/users/{id}`
- **Authentication:** Required (Bearer token)
- **Authorization:** Admin role required
- **Path Parameters:**
  - `id` (integer, required) - The user ID
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** User object
- **Error Responses:**
  - **Code:** `404 Not Found` - User not found

---

#### 3. Create User
Create a new user account.

- **Method:** `POST`
- **Endpoint:** `/api/users`
- **Authentication:** Required (Bearer token)
- **Authorization:** Admin role required
- **Request Body:**
  ```json
  {
	"username": "jane",
	"displayName": "Jane Smith",
	"password": "password123",
	"role": "Participant"
  }
  ```
- **Success Response:**
  - **Code:** `201 Created`
  - **Content:** Created user object
- **Error Responses:**
  - **Code:** `400 Bad Request` - Invalid input or user already exists

---

#### 4. Update User
Update user information.

- **Method:** `PUT`
- **Endpoint:** `/api/users/{id}`
- **Authentication:** Required (Bearer token)
- **Authorization:** Admin role required
- **Path Parameters:**
  - `id` (integer, required) - The user ID
- **Request Body:**
  ```json
  {
	"displayName": "Updated Name"
  }
  ```
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** Updated user object
- **Error Responses:**
  - **Code:** `400 Bad Request` - Invalid input
  - **Code:** `404 Not Found` - User not found

---

#### 5. Delete User
Delete a user account.

- **Method:** `DELETE`
- **Endpoint:** `/api/users/{id}`
- **Authentication:** Required (Bearer token)
- **Authorization:** Admin role required
- **Path Parameters:**
  - `id` (integer, required) - The user ID
- **Success Response:**
  - **Code:** `204 No Content`
- **Error Responses:**
  - **Code:** `404 Not Found` - User not found

---

#### 6. Change User Password (Admin)
Change password for a specific user (admin operation).

- **Method:** `PUT`
- **Endpoint:** `/api/users/password`
- **Authentication:** Required (Bearer token)
- **Authorization:** Admin role required
- **Query Parameters:**
  - `userId` (integer, required) - The ID of the user whose password to change
  - `password` (string, required) - The new password
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** `"Password Changed Successfully"`
- **Error Responses:**
  - **Code:** `400 Bad Request` - Invalid input

---

### Hackathons Endpoints

#### 1. Get All Hackathons
Retrieve a list of all hackathons with optional filtering.

- **Method:** `GET`
- **Endpoint:** `/api/hackathons`
- **Authentication:** Not required
- **Query Parameters (Filter):**
  - `Theme` (string, optional) - Filter by hackathon theme
  - `Status` (integer, optional) - Filter by hackathon status (0, 1, or 2)
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** Array of hackathon objects
  ```json
  [
	{
	  "id": 1,
	  "theme": "AI & Machine Learning",
	  "rules": "Use any programming language",
	  "startDate": "2025-01-15T09:00:00Z",
	  "endDate": "2025-01-17T17:00:00Z",
	  "evaluationCriteria": "Innovation, Code Quality, Presentation"
	}
  ]
  ```

---

#### 2. Get Hackathon by ID
Retrieve a specific hackathon by its ID.

- **Method:** `GET`
- **Endpoint:** `/api/hackathons/{id}`
- **Authentication:** Not required
- **Path Parameters:**
  - `id` (integer, required) - The hackathon ID
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** Hackathon object
- **Error Responses:**
  - **Code:** `404 Not Found` - Hackathon not found

---

#### 3. Create Hackathon
Create a new hackathon event.

- **Method:** `POST`
- **Endpoint:** `/api/hackathons`
- **Authentication:** Required (Bearer token)
- **Authorization:** Admin role required
- **Request Body:**
  ```json
  {
	"theme": "Web Development Challenge",
	"rules": "Frontend and backend using modern frameworks",
	"startDate": "2025-02-01T10:00:00Z",
	"endDate": "2025-02-03T18:00:00Z",
	"evaluationCriteria": "User Experience, Performance, Code Quality"
  }
  ```
- **Success Response:**
  - **Code:** `201 Created`
  - **Location:** `/api/hackathons/{id}`
  - **Content:** Created hackathon object
- **Error Responses:**
  - **Code:** `400 Bad Request` - Invalid input

---

#### 4. Update Hackathon
Update hackathon information.

- **Method:** `PUT`
- **Endpoint:** `/api/hackathons/{id}`
- **Authentication:** Required (Bearer token)
- **Authorization:** Admin role required
- **Path Parameters:**
  - `id` (integer, required) - The hackathon ID
- **Request Body:**
  ```json
  {
	"theme": "Updated Theme",
	"rules": "Updated rules",
	"startDate": "2025-02-01T10:00:00Z",
	"endDate": "2025-02-03T18:00:00Z",
	"evaluationCriteria": "Updated criteria"
  }
  ```
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** Updated hackathon object
- **Error Responses:**
  - **Code:** `400 Bad Request` - Invalid input
  - **Code:** `404 Not Found` - Hackathon not found

---

#### 5. Delete Hackathon
Delete a hackathon event.

- **Method:** `DELETE`
- **Endpoint:** `/api/hackathons/{id}`
- **Authentication:** Required (Bearer token)
- **Authorization:** Admin role required
- **Path Parameters:**
  - `id` (integer, required) - The hackathon ID
- **Success Response:**
  - **Code:** `204 No Content`
- **Error Responses:**
  - **Code:** `404 Not Found` - Hackathon not found

---

### Teams Endpoints

#### 1. Get All Teams
Retrieve a list of all teams with optional filtering.

- **Method:** `GET`
- **Endpoint:** `/api/teams`
- **Authentication:** Not required
- **Query Parameters (Filter):**
  - `Query` (string, optional) - Search by team name or description
  - `LeaderId` (integer, optional) - Filter by team leader ID
  - `MemberId` (integer, optional) - Filter by team member ID
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** Array of team objects
  ```json
  [
	{
	  "id": 1,
	  "name": "Team Alpha",
	  "description": "AI specialists team",
	  "leaderId": 5,
	  "leaderDisplayName": "John Doe",
	  "members": [
		{
		  "id": 5,
		  "displayName": "John Doe",
		  "role": "TeamMember"
		}
	  ]
	}
  ]
  ```

---

#### 2. Get Team by ID
Retrieve a specific team by its ID.

- **Method:** `GET`
- **Endpoint:** `/api/teams/{id}`
- **Authentication:** Not required
- **Path Parameters:**
  - `id` (integer, required) - The team ID
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** Team object with members
- **Error Responses:**
  - **Code:** `404 Not Found` - Team not found

---

#### 3. Create Team
Create a new team.

- **Method:** `POST`
- **Endpoint:** `/api/teams`
- **Authentication:** Required (Bearer token)
- **Authorization:** Participant role required
- **Request Body:**
  ```json
  {
	"name": "Team Beta",
	"description": "Full-stack development specialists"
  }
  ```
  - `name` (string, required) - Team name
  - `description` (string, required) - Team description
- **Success Response:**
  - **Code:** `201 Created`
  - **Location:** `/api/teams/{id}`
  - **Content:** Created team object
- **Error Responses:**
  - **Code:** `400 Bad Request` - Invalid input
  - **Code:** `404 Not Found` - Resource not found

---

#### 4. Update Team
Update team information.

- **Method:** `PUT`
- **Endpoint:** `/api/teams/{id}`
- **Authentication:** Required (Bearer token)
- **Authorization:** Participant role required (team leader)
- **Path Parameters:**
  - `id` (integer, required) - The team ID
- **Request Body:**
  ```json
  {
	"name": "Updated Team Name",
	"description": "Updated description",
	"leaderId": 5
  }
  ```
  - `name` (string, required) - Updated team name
  - `description` (string, required) - Updated team description
  - `leaderId` (integer, required) - Team leader ID
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** Updated team object
- **Error Responses:**
  - **Code:** `400 Bad Request` - Invalid input or not team leader
  - **Code:** `404 Not Found` - Team not found

---

#### 5. Delete Team
Delete a team.

- **Method:** `DELETE`
- **Endpoint:** `/api/teams/{id}`
- **Authentication:** Required (Bearer token)
- **Authorization:** Participant role required (team leader)
- **Path Parameters:**
  - `id` (integer, required) - The team ID
- **Success Response:**
  - **Code:** `204 No Content`
- **Error Responses:**
  - **Code:** `404 Not Found` - Team not found

---

#### 6. Join Team
Join an existing team as a participant.

- **Method:** `POST`
- **Endpoint:** `/api/teams/{id}/members`
- **Authentication:** Required (Bearer token)
- **Authorization:** Participant role required
- **Path Parameters:**
  - `id` (integer, required) - The team ID to join
- **Success Response:**
  - **Code:** `204 No Content`
- **Error Responses:**
  - **Code:** `400 Bad Request` - Already a member or team is full
  - **Code:** `404 Not Found` - Team not found

---

#### 7. Leave Team
Leave a team as a participant.

- **Method:** `DELETE`
- **Endpoint:** `/api/teams/{id}/members`
- **Authentication:** Required (Bearer token)
- **Authorization:** Participant role required
- **Path Parameters:**
  - `id` (integer, required) - The team ID to leave
- **Success Response:**
  - **Code:** `204 No Content`
- **Error Responses:**
  - **Code:** `400 Bad Request` - Not a team member or team leader cannot leave
  - **Code:** `404 Not Found` - Team not found

---

### Submissions Endpoints

#### 1. Get All Submissions
Retrieve a list of all submissions with optional filtering.

- **Method:** `GET`
- **Endpoint:** `/api/submissions`
- **Authentication:** Required (Bearer token)
- **Query Parameters (Filter):**
  - `Query` (string, optional) - Search by submission title or description
  - `TeamId` (integer, optional) - Filter by team ID
  - `HackathonId` (integer, optional) - Filter by hackathon ID
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** Array of submission objects
  ```json
  [
	{
	  "id": 1,
	  "title": "AI Chat Application",
	  "description": "A conversational AI platform",
	  "url": "https://github.com/example/repo",
	  "teamId": 1,
	  "hackathonId": 1
	}
  ]
  ```

---

#### 2. Get Submission by ID
Retrieve a specific submission by its ID.

- **Method:** `GET`
- **Endpoint:** `/api/submissions/{id}`
- **Authentication:** Required (Bearer token)
- **Path Parameters:**
  - `id` (integer, required) - The submission ID
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** Submission object
- **Error Responses:**
  - **Code:** `404 Not Found` - Submission not found

---

#### 3. Create Submission
Submit a project for a hackathon.

- **Method:** `POST`
- **Endpoint:** `/api/submissions`
- **Authentication:** Required (Bearer token)
- **Authorization:** Participant role required
- **Request Body:**
  ```json
  {
	"title": "ML Prediction Engine",
	"description": "Advanced machine learning model for prediction",
	"url": "https://github.com/example/ml-engine",
	"teamId": 1,
	"hackathonId": 1
  }
  ```
  - `title` (string, required) - Submission title
  - `description` (string, required) - Submission description
  - `url` (string, required) - URL to the submission repository or project
  - `teamId` (integer, required) - Team ID for the submission
  - `hackathonId` (integer, required) - Hackathon ID for the submission
- **Success Response:**
  - **Code:** `201 Created`
  - **Location:** `/api/submissions/{id}`
  - **Content:** Created submission object
- **Error Responses:**
  - **Code:** `400 Bad Request` - Invalid input

---

#### 4. Delete Submission
Delete a submission (only by the submitter).

- **Method:** `DELETE`
- **Endpoint:** `/api/submissions/{id}`
- **Authentication:** Required (Bearer token)
- **Authorization:** Participant role required (submission owner)
- **Path Parameters:**
  - `id` (integer, required) - The submission ID
- **Success Response:**
  - **Code:** `204 No Content`
- **Error Responses:**
  - **Code:** `404 Not Found` - Submission not found

---

### Evaluations Endpoints

#### 1. Get All Evaluations
Retrieve a list of all evaluations with optional filtering.

- **Method:** `GET`
- **Endpoint:** `/api/evaluations`
- **Authentication:** Required (Bearer token)
- **Query Parameters (Filter):**
  - `HackathonId` (integer, optional) - Filter by hackathon ID
  - `TeamId` (integer, optional) - Filter by team ID
  - `SubmissionId` (integer, optional) - Filter by submission ID
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** Array of evaluation objects
  ```json
  [
	{
	  "id": 1,
	  "innovationScore": 8.5,
	  "technicalQualityScore": 9.0,
	  "presentationQualityScore": 8.0,
	  "solutionPertinenceScore": 8.5,
	  "submissionId": 1,
	  "teamId": 1,
	  "teamName": "Team Alpha",
	  "hackathonId": 1,
	  "hackathonTheme": "AI & Machine Learning",
	  "mentorId": 3,
	  "mentorName": "Jane Smith"
	}
  ]
  ```

---

#### 2. Get Evaluation by ID
Retrieve a specific evaluation by its ID.

- **Method:** `GET`
- **Endpoint:** `/api/evaluations/{id}`
- **Authentication:** Required (Bearer token)
- **Path Parameters:**
  - `id` (integer, required) - The evaluation ID
- **Success Response:**
  - **Code:** `200 OK`
  - **Content:** Evaluation object
- **Error Responses:**
  - **Code:** `404 Not Found` - Evaluation not found

---

#### 3. Create Evaluation
Create an evaluation for a submission.

- **Method:** `POST`
- **Endpoint:** `/api/evaluations`
- **Authentication:** Required (Bearer token)
- **Authorization:** Mentor role required
- **Request Body:**
  ```json
  {
	"innovationScore": 8.5,
	"technicalQualityScore": 9.0,
	"presentationQualityScore": 8.0,
	"solutionPertinenceScore": 8.5,
	"submissionId": 1
  }
  ```
  - `innovationScore` (float, optional) - Score 0-10 for innovation
  - `technicalQualityScore` (float, optional) - Score 0-10 for technical quality
  - `presentationQualityScore` (float, optional) - Score 0-10 for presentation quality
  - `solutionPertinenceScore` (float, optional) - Score 0-10 for solution pertinence
  - `submissionId` (integer, required) - The submission ID to evaluate
- **Success Response:**
  - **Code:** `201 Created`
  - **Location:** `/api/evaluations/{id}`
  - **Content:** Created evaluation object
- **Error Responses:**
  - **Code:** `400 Bad Request` - Invalid score or submission not found

---

#### 4. Delete Evaluation
Delete an evaluation (only by the mentor who created it).

- **Method:** `DELETE`
- **Endpoint:** `/api/evaluations/{id}`
- **Authentication:** Required (Bearer token)
- **Authorization:** Mentor role required (evaluation creator)
- **Path Parameters:**
  - `id` (integer, required) - The evaluation ID
- **Success Response:**
  - **Code:** `204 No Content`
- **Error Responses:**
  - **Code:** `400 Bad Request` - Not the evaluation creator
  - **Code:** `404 Not Found` - Evaluation not found

---

## Common Response Codes

| Status Code | Meaning |
|-------------|---------|
| `200` | OK - Request succeeded |
| `201` | Created - Resource successfully created |
| `204` | No Content - Successful deletion or update |
| `400` | Bad Request - Invalid input or validation error |
| `401` | Unauthorized - Missing or invalid authentication |
| `403` | Forbidden - Insufficient permissions for the role |
| `404` | Not Found - Resource not found |
| `500` | Internal Server Error - Server error |

---

## Error Response Format

All error responses follow this format:

```json
{
  "error": "Error message describing what went wrong"
}
```

---

## Interactive API Testing

You can test all endpoints interactively using the Swagger UI:

**URL:** `{Base-URL}/swagger/`

The Swagger interface allows you to:
- View all endpoints and their documentation
- Test endpoints directly from the browser
- See request/response examples
- Authenticate and test protected endpoints

---

## Rate Limiting

Currently, there are no rate limits on the API. However, it is recommended to implement reasonable request intervals to avoid server overload.

---

## Data Models

### GetHackathonDto
```json
{
  "id": "integer",
  "theme": "string",
  "rules": "string",
  "startDate": "datetime",
  "endDate": "datetime",
  "evaluationCriteria": "string"
}
```

### GetTeamDto
```json
{
  "id": "integer",
  "name": "string",
  "description": "string",
  "leaderId": "integer",
  "leaderDisplayName": "string",
  "members": [
	{
	  "id": "integer",
	  "displayName": "string",
	  "role": "string"
	}
  ]
}
```

### GetUserDto
```json
{
  "id": "integer",
  "userName": "string",
  "displayName": "string",
  "role": "string"
}
```

### GetSubmissionDto
```json
{
  "id": "integer",
  "title": "string",
  "description": "string",
  "url": "string (URI format)",
  "teamId": "integer",
  "hackathonId": "integer"
}
```

### GetEvaluationDto
```json
{
  "id": "integer",
  "innovationScore": "float (0-10)",
  "technicalQualityScore": "float (0-10)",
  "presentationQualityScore": "float (0-10)",
  "solutionPertinenceScore": "float (0-10)",
  "submissionId": "integer",
  "teamId": "integer",
  "teamName": "string",
  "hackathonId": "integer",
  "hackathonTheme": "string",
  "mentorId": "integer",
  "mentorName": "string"
}
```

### CreateEvaluationDto
```json
{
  "innovationScore": "float (0-10, optional)",
  "technicalQualityScore": "float (0-10, optional)",
  "presentationQualityScore": "float (0-10, optional)",
  "solutionPertinenceScore": "float (0-10, optional)",
  "submissionId": "integer (required)"
}
```

### AddUserDto
```json
{
  "username": "string (required)",
  "displayName": "string (required)",
  "password": "string (required)",
  "role": "string (required)"
}
```

### UpdateUserDto
```json
{
  "displayName": "string (required)",
  "role": "string (required)"
}
```

### CreateHackathonDto
```json
{
  "theme": "string (required)",
  "rules": "string (required)",
  "startDate": "datetime (required)",
  "endDate": "datetime (required)",
  "evaluationCriteria": "string (required)"
}
```

### UpdateHackathonDto
```json
{
  "theme": "string (required)",
  "rules": "string (required)",
  "startDate": "datetime (required)",
  "endDate": "datetime (required)",
  "evaluationCriteria": "string (required)"
}
```

### CreateTeamDto
```json
{
  "name": "string (required)",
  "description": "string (required)"
}
```

### UpdateTeamDto
```json
{
  "name": "string (required)",
  "description": "string (required)",
  "leaderId": "integer (required)"
}
```

### CreateSubmissionDto
```json
{
  "title": "string (required)",
  "description": "string (required)",
  "url": "string URI format (required)",
  "teamId": "integer (required)",
  "hackathonId": "integer (required)"
}
```

### ChangePasswordDto
```json
{
  "currentPassword": "string (required)",
  "newPassword": "string (required)"
}
```

---

## Support & Troubleshooting

For issues or questions:

1. Check the Swagger UI documentation at `{Base-URL}/swagger/`
2. Review this documentation for endpoint details
3. Verify your authentication token is valid
4. Check that you have the required role for protected endpoints
5. Ensure all required parameters are provided
6. Review the request/response format in the examples

---

**Last Updated:** January 2025
