# Backend Admin API — Manual Verification Checklist



## Prerequisites

- API running (`dotnet run`)

- Database migrated (`dotnet ef database update`)

- Dev admin seeded: `admin` / `Admin123!` (from `appsettings.Development.json`)



## Auth

- [ ] `POST /api/auth/login` with admin credentials returns JWT

- [ ] Decode JWT — contains `role` claim = `Admin`

- [ ] `GET /api/auth/me` with Bearer token returns user profile (no password)

- [ ] `POST /api/auth/login` with suspended user returns 403



## Authorization (403 for non-admin)

- [ ] Login as Learner (register new user)

- [ ] `GET /api/admin/users` with Learner token → 403

- [ ] `GET /api/questions` with Learner token → 403



## Questions (Admin token)

- [ ] `GET /api/questions?pageNumber=1&pageSize=20` returns paginated global pool

- [ ] `GET /api/questions?domainId={guid}` filters by domain

- [ ] List items include `domainTitle` and `examTitle` (read-only context from domain's exam)

- [ ] `POST /api/questions` creates question with `domainId` only (`ExamId` not set)

- [ ] `GET /api/questions/{id}` returns title, explanation, type, domain metadata, options with `isCorrect`

- [ ] `PUT /api/questions/{id}` updates text and syncs answer options

- [ ] `POST /api/questions/import` imports Excel with **DomainId** column (column 3); no `examId` query param

- [ ] Import rejects invalid or missing DomainId GUIDs per row

- [ ] `DELETE /api/questions/{id}` deletes single question

- [ ] `DELETE /api/questions` with `{ "questionIds": [...] }` bulk deletes

- [ ] `GET /api/exams/{examId}/questions` → 404 (removed)



## Excel import format

Header row (sheet `Questions`, data from row 3): `Title | Explanation | DomainId | QuestionType | option text/correct pairs…`



## Session compile (learner)

- [ ] `POST /api/session/start` still compiles random questions per exam domain weights (domain-based, not exam-owned questions)



## Users (Admin token)

- [ ] `GET /api/admin/users` returns paginated list

- [ ] `GET /api/admin/users/count` returns `{ count: N }`

- [ ] `POST /api/admin/users` creates user with role/status

- [ ] `PUT /api/admin/users/{id}` updates profile, role, status

- [ ] `PATCH /api/admin/users/{id}/status` with `{ "status": "Suspended" }` suspends user

- [ ] Suspended user cannot login (403)

- [ ] `PATCH` own status → 403 (cannot suspend self)

- [ ] Demoting last admin → 403



## Refresh token

- [ ] `POST /api/auth/refresh` with valid access + refresh tokens returns new access token

