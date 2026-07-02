# TestAPI HTTP API Reference

TestAPI is an ASP.NET Core Web API for an exam simulator. It exposes public discovery endpoints, learner exam/session endpoints, and admin endpoints for content, users, analytics, and site settings.

## Base URL And Explorer

- Local API base URL: `http://localhost:5033`
- React dev client origin allowed by CORS: `http://localhost:5173`
- Development API explorer: `http://localhost:5033/scalar`
- Development OpenAPI document: `http://localhost:5033/openapi/v1.json`

Scalar/OpenAPI is mapped only in `Development`. Protected endpoints currently require manually adding the `Authorization` header because the OpenAPI setup does not define a JWT Bearer security scheme.

## Authentication

Protected endpoints use JWT Bearer authentication:

```http
Authorization: Bearer <accessToken>
```

Authorization levels:

| Level | Meaning |
|-------|---------|
| Public | No token required. |
| Authenticated | Any valid user token is accepted. |
| Admin | Requires the `AdminOnly` policy, which checks the user's `Admin` role. |

Token flow:

1. Register with `POST /api/auth/register`, or login with `POST /api/auth/login`.
2. Store the returned `accessToken` and `refreshToken`.
3. Send `Authorization: Bearer <accessToken>` to protected endpoints.
4. Refresh expired tokens with `POST /api/auth/refresh`.
5. Get or update the current profile with `/api/auth/me`.

## Common Conventions

Requests and responses use JSON unless the endpoint says otherwise. ASP.NET Core serializes property names in camel case by default, so DTO property `DisplayName` appears as `displayName`.

Enums are serialized as strings:

| Enum | Values |
|------|--------|
| `UserRole` | `Learner`, `Pro`, `Admin` |
| `AccountStatus` | `Active`, `Suspended`, `Pending` |
| `ExamStatus` | `Draft`, `Published`, `Archived` |
| `AttemptStatus` | `InProgress`, `Completed`, `Abandoned` |
| `QuestionType` | `SingleChoice`, `MultipleChoice`, `TrueFalse` |

`QuestionType.MultipleChoice` exists in the API contract, but the entity comment notes that multiple-choice scoring is not fully implemented yet.

### Pagination

Paged endpoints accept:

| Query parameter | Type | Default |
|-----------------|------|---------|
| `pageNumber` | `int` | `1` when missing or less than `1` |
| `pageSize` | `int` | `20` when missing or less than `1` |

Paged responses use this shape:

```json
{
  "items": [],
  "page": 1,
  "pageSize": 20,
  "totalCount": 0,
  "hasNextPage": false,
  "hasPreviousPage": false
}
```

### Errors

Most service errors are mapped by the result pattern:

| Status | Body |
|--------|------|
| `400 Bad Request` | Usually empty for some controller validation failures. |
| `401 Unauthorized` | Empty body or a plain string error. |
| `403 Forbidden` | Plain string error. |
| `404 Not Found` | Plain string error. |
| `409 Conflict` | Plain string error. |
| `422 Unprocessable Entity` | Plain string error or validation array. |
| `503 Service Unavailable` | Plain string error. |
| `500 Internal Server Error` | Plain string error or `ProblemDetails` for unhandled exceptions. |

Validation arrays have this shape:

```json
[
  {
    "propertyName": "Email",
    "errorMessage": "Please enter a valid email address."
  }
]
```

## Endpoint Reference

### Auth

| Method | Route | Auth | Request | Success response |
|--------|-------|------|---------|------------------|
| `POST` | `/api/auth/register` | Public | `RegisterUserRequest` | `201 Created`, empty body |
| `POST` | `/api/auth/login` | Public | `LoginUserRequest` | `200 OK`, `TokenResponse` |
| `POST` | `/api/auth/refresh` | Public | `RefreshTokenRequest` | `200 OK`, `RefreshTokenResponse` |
| `GET` | `/api/auth/me` | Authenticated | None | `200 OK`, `UserDto` |
| `PATCH` | `/api/auth/me` | Authenticated | `UpdateProfileRequest` | `200 OK`, `UserDto` |

Key request contracts:

```json
{
  "userName": "admin",
  "password": "Admin123!"
}
```

`RegisterUserRequest` fields: `userName`, `password`, `firstName`, `displayName`, `email`.

`TokenResponse` fields: `accessToken`, `refreshToken`.

`RefreshTokenRequest` fields: `accessToken`, `refreshToken`.

`RefreshTokenResponse` fields: `newAccessToken`.

`UserDto` fields: `id`, `firstName`, `userName`, `displayName`, `email`, `role`, `status`, `createdAt`.

### Public

| Method | Route | Auth | Request | Success response |
|--------|-------|------|---------|------------------|
| `GET` | `/api/public/stats` | Public | None | `PublicStatsDto` |
| `GET` | `/api/public/settings` | Public | None | `{ siteName, allowRegistration, maintenanceMode }` |

`PublicStatsDto` fields: `totalQuestions`, `totalUsers`, `publishedExamCount`.

The public settings endpoint intentionally returns only a subset of admin settings.

### Exams

| Method | Route | Auth | Request | Success response |
|--------|-------|------|---------|------------------|
| `GET` | `/api/exams/{examId}/details` | Public | Route `examId` | `ExamDetailsDto` |
| `GET` | `/api/exams/details` | Public | `pageNumber`, `pageSize` | Paged `ExamDetailsDto` |
| `GET` | `/api/exams` | Admin | None | `ExamSummaryDto[]` |
| `POST` | `/api/exams` | Admin | `CreateExamDto` | `201 Created`, empty body |
| `PATCH` | `/api/exams/{id}/update` | Admin | Route `id`, `UpdateExamRequest` | `204 No Content` |
| `DELETE` | `/api/exams/{id}` | Admin | Route `id` | `204 No Content` |
| `DELETE` | `/api/exams` | Admin | `BulkDeleteExamsRequest` | `204 No Content` |
| `POST` | `/api/exams/{id}/publish` | Admin | Route `id` | `204 No Content` |
| `POST` | `/api/exams/{id}/archive` | Admin | Route `id` | `204 No Content` |
| `GET` | `/api/admin/exams/stats` | Admin | None | `ExamOverviewStatsDto[]` |

Important contracts:

- `CreateExamDto`: `categoryId`, `title`, `context`, `durationInMinutes`, `numberOfQuestions`, `createDomainDtos`.
- `UpdateExamRequest`: `numberOfQuestions`, `durationInMinutes`.
- `BulkDeleteExamsRequest`: `examIds`.
- `ExamDetailsDto`: `id`, `title`, `context`, `durationInMinutes`, `numberOfQuestions`, `attemptCount`, `isMostPopular`.
- `ExamSummaryDto`: `id`, `title`, `categoryTitle`, `categoryId`, `numberOfQuestions`, `durationInMinutes`, `status`.
- `ExamOverviewStatsDto`: `examId`, `examTitle`, `attemptCount`, `uniqueUsersCount`, `averageScore`.

Example create request:

```json
{
  "categoryId": "00000000-0000-0000-0000-000000000000",
  "title": "PMI Practice Exam",
  "context": "Practice exam description",
  "durationInMinutes": 230,
  "numberOfQuestions": 180,
  "createDomainDtos": [
    {
      "examId": "00000000-0000-0000-0000-000000000000",
      "title": "People",
      "description": "People domain",
      "weight": 42
    }
  ]
}
```

When creating an exam with nested domains, validation expects domain weights to total roughly 100.

### Sessions And Attempts

| Method | Route | Auth | Request | Success response |
|--------|-------|------|---------|------------------|
| `POST` | `/api/session/start` | Authenticated | Query `examId` | `SessionDto` |
| `POST` | `/api/session/finish` | Authenticated | `FinishSessionRequest` | `SessionResultDto` |
| `POST` | `/api/session/abandon` | Authenticated | `AbandonSessionRequest` | `204 No Content` |
| `DELETE` | `/api/attempts/{id}` | Admin | Route `id` | `204 No Content` |

`POST /api/session/start?examId={examId}` returns a session snapshot:

- `SessionDto`: `sessionId`, `questions`.
- `QuestionSnapshotDto`: `id`, `title`, `questionType`, `answerOptionsDtos`.
- `AnswerOptionDto`: `id`, `text`, `isCorrect`.

Finish request:

```json
{
  "sessionId": "00000000-0000-0000-0000-000000000000",
  "sessionResponses": [
    {
      "questionId": "00000000-0000-0000-0000-000000000000",
      "selectedOptionIds": [
        "00000000-0000-0000-0000-000000000000"
      ]
    }
  ]
}
```

`SessionResultDto` fields: `scorePoints`, `percentageScore`.

Session start, finish, and abandon depend on the session snapshot store. If Redis/session storage is unavailable, endpoints may return `503 Service Unavailable`.

### Questions

All question endpoints require admin access.

| Method | Route | Request | Success response |
|--------|-------|---------|------------------|
| `GET` | `/api/questions` | `pageNumber`, `pageSize`, `domainId`, `questionType`, `search` | Paged `QuestionListItemDto` |
| `POST` | `/api/questions/import` | `multipart/form-data`, field `file` | `QuestionImportResultDto` |
| `POST` | `/api/questions` | `CreateQuestionDto` | `201 Created`, empty body |
| `GET` | `/api/questions/{id}` | Route `id` | `QuestionAdminDto` |
| `PUT` | `/api/questions/{id}` | Route `id`, `UpdateQuestionRequest` | `204 No Content` |
| `DELETE` | `/api/questions/{questionId}` | Route `questionId` | `204 No Content` |
| `DELETE` | `/api/questions` | `BulkDeleteQuestionsRequest` | `204 No Content` |

Question request contracts:

- `CreateQuestionDto`: `title`, `explanation`, `questionType`, `domainId`, `answerOptionsDtos`.
- `UpdateQuestionRequest`: `title`, `explanation`, `questionType`, `answerOptionsDtos`. The route `id` is copied into the request server-side.
- `CreateAnswerOptionDto`: `text`, `isCorrect`.
- `UpdateAnswerOptionDto`: optional `id`, `text`, `isCorrect`.
- `BulkDeleteQuestionsRequest`: `questionIds`.

Example create request:

```json
{
  "title": "What is the best next step?",
  "explanation": "Explanation shown after scoring.",
  "questionType": "SingleChoice",
  "domainId": "00000000-0000-0000-0000-000000000000",
  "answerOptionsDtos": [
    {
      "text": "Option A",
      "isCorrect": true
    },
    {
      "text": "Option B",
      "isCorrect": false
    }
  ]
}
```

Question response contracts:

- `QuestionListItemDto`: `id`, `title`, `questionType`, `domainId`, `domainTitle`, `examTitle`, `answerOptionCount`.
- `QuestionAdminDto`: `id`, `title`, `explanation`, `questionType`, `domainId`, `domainTitle`, `examTitle`, `answerOptions`.

#### Excel Import

`POST /api/questions/import` expects a multipart form upload:

```http
Content-Type: multipart/form-data

file=<Questions workbook>
```

Workbook requirements:

- Sheet name must be `Questions`.
- Data starts at row `3`.
- Column 1: title.
- Column 2: explanation.
- Column 3: domain ID as a GUID.
- Column 4: question type: `SingleChoice`, `MultipleChoice`, or `TrueFalse`.
- Columns 5-14: up to five answer option text/correct pairs.
- Correct flags are read as strings and treated as correct when the value is `TRUE`.

Import response:

```json
{
  "success": true,
  "importedCount": 10,
  "errors": []
}
```

Each error item has `row` and `reason`.

### Categories

| Method | Route | Auth | Request | Success response |
|--------|-------|------|---------|------------------|
| `GET` | `/api/categories` | Public | None | `CategoryDto[]` |
| `GET` | `/api/categories/{id}` | Public | Route `id` | `CategoryDto` |
| `POST` | `/api/categories` | Admin | `CreateCategoryDto` | `201 Created`, empty body |
| `PUT` | `/api/categories/{id}` | Admin | Route `id`, `UpdateCategoryRequest` | `204 No Content` |
| `DELETE` | `/api/categories/{id}` | Admin | Route `id` | `204 No Content` |

Contracts:

- `CreateCategoryDto`: `title`, `description`.
- `UpdateCategoryRequest`: `title`, `description`. The route `id` is copied into `categoryId` server-side.
- `CategoryDto`: `id`, `title`, `description`, `numberOfExams`, `examSummaryDtos`.

### Domains

| Method | Route | Auth | Request | Success response |
|--------|-------|------|---------|------------------|
| `GET` | `/api/domains` | Public | None | `DomainDto[]` |
| `GET` | `/api/domains/{id}` | Public | Route `id` | `DomainDto` |
| `GET` | `/api/domains/withTitles` | Public | Query `examId` | Domain title data |
| `POST` | `/api/domains` | Admin | `CreateDomainDto` | `201 Created`, empty body |
| `PUT` | `/api/domains/{id}` | Admin | Route `id`, `UpdateDomainDto` | `204 No Content` |
| `DELETE` | `/api/domains/{id}` | Admin | Route `id` | `204 No Content` |

Contracts:

- `CreateDomainDto`: `examId`, `title`, `description`, `weight`.
- `UpdateDomainDto`: `title`, `description`, `weight`.
- `DomainDto`: `id`, `title`, `description`, `weight`, `examId`.

Domain validation expects `weight` between `0` and `99`.

### Progress

| Method | Route | Auth | Request | Success response |
|--------|-------|------|---------|------------------|
| `GET` | `/api/progress/domains` | Authenticated | None | `DomainPerformanceDto[]` |

`DomainPerformanceDto` fields: `domainId`, `domainTitle`, `examId`, `examTitle`, `totalAnswered`, `totalCorrect`, `percentageScore`, `lastUpdated`.

The response is scoped to the current user from the JWT claims.

### Admin Users

All admin user endpoints require admin access.

| Method | Route | Request | Success response |
|--------|-------|---------|------------------|
| `GET` | `/api/admin/users` | `pageNumber`, `pageSize`, `search`, `role`, `status` | Paged `UserListItemDto` |
| `GET` | `/api/admin/users/count` | None | `{ count }` |
| `GET` | `/api/admin/users/stats` | None | `UserStatsDto` |
| `GET` | `/api/admin/users/{id}` | Route `id` | `UserDto` |
| `PATCH` | `/api/admin/users/{id}/status` | Route `id`, `UpdateUserStatusRequest` | `204 No Content` |

Contracts:

- `UserListItemDto`: `id`, `displayName`, `email`, `role`, `status`, `createdAt`.
- `UserStatsDto`: `totalCount`, `byRole`, `byStatus`.
- `UpdateUserStatusRequest`: `status`.

The service layer contains `CreateUserRequest` and `UpdateUserRequest`, and the React client appears to expect admin create/update routes. The current controller does not expose `POST /api/admin/users` or `PUT /api/admin/users/{id}`.

### Admin Analytics

All analytics endpoints require admin access.

| Method | Route | Request | Success response |
|--------|-------|---------|------------------|
| `GET` | `/api/admin/analytics/attempts` | Optional query `days`, default `30` | `AttemptVolumeDto[]` |
| `GET` | `/api/admin/analytics/pass-rate` | None | `PassRateAnalyticsDto` |

Contracts:

- `AttemptVolumeDto`: `date`, `count`.
- `PassRateAnalyticsDto`: `averageScore`, `totalCompletedAttempts`, `passCount`, `passRate`, `passThreshold`.

### Admin Settings

All admin settings endpoints require admin access.

| Method | Route | Request | Success response |
|--------|-------|---------|------------------|
| `GET` | `/api/admin/settings` | None | `SiteSettingsDto` |
| `PUT` | `/api/admin/settings` | `UpdateSiteSettingsDto` | `SiteSettingsDto` |

Contracts:

- `SiteSettingsDto`: `siteName`, `supportEmail`, `allowRegistration`, `maintenanceMode`.
- `UpdateSiteSettingsDto`: `siteName`, `supportEmail`, `allowRegistration`, `maintenanceMode`.

Example update request:

```json
{
  "siteName": "PMI Exam Simulator",
  "supportEmail": "support@example.com",
  "allowRegistration": true,
  "maintenanceMode": false
}
```

## Workflows

### Learner Exam Flow

1. Browse public categories, domains, and published exam details.
2. Register or login.
3. Start an exam session with `POST /api/session/start?examId={examId}`.
4. Submit answers with `POST /api/session/finish`.
5. Review the returned score.
6. Check domain performance with `GET /api/progress/domains`.

### Admin Content Flow

1. Login as an admin user.
2. Create or review categories and domains.
3. Create an exam, including domain weights, or manage existing exams.
4. Add questions manually with `POST /api/questions` or import them from Excel.
5. Publish the exam with `POST /api/exams/{id}/publish`.
6. Monitor usage through exam stats and admin analytics.

## Known Gaps And Caveats

- Scalar/OpenAPI is available only in development.
- OpenAPI does not currently define a JWT Bearer security scheme.
- Some endpoints return empty success bodies after creation, so clients may need to re-fetch data.
- Error response shapes are not fully consistent across controllers.
- `POST /api/admin/users` and `PUT /api/admin/users/{id}` are not exposed by `AdminUserController`, even though DTOs and frontend service code suggest those operations may be expected.
- Most routes use absolute action-level route attributes. `ProgressController` uses controller-level routing with `[Route("api/progress")]`.

## Optional OpenAPI Improvements

These changes are not required for the manual reference above, but would make Scalar/OpenAPI more useful:

1. Add a JWT Bearer security scheme to the OpenAPI setup so Scalar can authorize protected endpoints without manually typing headers.
2. Add response metadata such as `[ProducesResponseType]` to high-traffic controllers first: auth, exams, sessions, questions, and admin users.
3. Add XML documentation comments only if generated OpenAPI descriptions should become the main reference source.
4. Normalize validation and error response shapes before documenting generated responses in detail.
