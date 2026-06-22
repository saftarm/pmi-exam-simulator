# Backend Changes Report — Data Integrity Refactor

This document lists all backend changes made to support frontend data integrity (no fabricated facts; API-driven metrics and settings).

**Migration required:** run `dotnet ef database update` to apply `20260620171103_AddSiteSettings`.

---

## 1. New public endpoints (no auth)

| Method | Route | Response | Purpose |
|--------|-------|----------|---------|
| GET | `/api/public/stats` | `{ totalQuestions, totalUsers, publishedExamCount }` | Landing page live counters |
| GET | `/api/public/settings` | `{ siteName, allowRegistration, maintenanceMode }` | Public platform flags (for future register/maintenance gating) |

**Files:**
- `Controllers/PublicController.cs` (new)
- `DTO/Public/PublicStatsDto.cs` (new)
- `Services/Interfaces/IPublicStatsService.cs` (new)
- `Services/Implementation/PublicStatsService.cs` (new)

---

## 2. Exam details — popularity fields

**Extended DTO:** `DTO/Exam/ExamDetailsDto.cs`

| Field | Type | Description |
|-------|------|-------------|
| `attemptCount` | `int` | Completed attempts for this exam |
| `isMostPopular` | `bool` | `true` when `attemptCount` equals the max among published exams (ties allowed) |

**Logic:** `Services/Implementation/ExamService.cs`
- `GetPublishedExamsDetailsAsync` — joins attempt counts, sets `IsMostPopular`
- `GetDetailsByIdAsync` — includes `Id`, `AttemptCount` (no popularity flag on single-exam detail)

**Repository:** `Persistence/Implementation/ExamAttemptRepository.cs`
- New: `GetCompletedAttemptCountsByExamAsync()`

---

## 3. Domain creation linked to exam

**DTO:** `DTO/Domain/CreateDomainDto.cs` — added `ExamId` (required)

**Service:** `Services/Implementation/DomainService.cs`
- `CreateDomain` validates exam exists and sets `ExamId` on new `Domain`

**Entity:** `Entities/Domain.cs` — constructor accepts optional `examId`

**Controller:** `Controllers/DomainController.cs` — route normalized to `POST /api/domains`

---

## 4. Site settings (replaces frontend localStorage)

**Entity:** `Entities/SiteSettings.cs` (new)  
**Migration:** `Migrations/20260620171103_AddSiteSettings.cs`

| Field | Default |
|-------|---------|
| SiteName | PMI Exam Simulator |
| SupportEmail | support@pmi-simulator.com |
| AllowRegistration | true |
| MaintenanceMode | false |
| DefaultExamDuration | 230 |
| PassThreshold | 80 |
| NotifyOnNewUser | true |
| NotifyOnExamComplete | false |

Singleton row created on first `GET` via `SiteSettingsRepository.GetOrCreateAsync`.

| Method | Route | Auth |
|--------|-------|------|
| GET | `/api/admin/settings` | Admin |
| PUT | `/api/admin/settings` | Admin |

**Files:**
- `Controllers/AdminSettingsController.cs` (new)
- `DTO/Settings/SiteSettingsDto.cs`, `UpdateSiteSettingsDto.cs` (new)
- `Validation/Settings/UpdateSiteSettingsDtoValidator.cs` (new)
- `Persistence/Interfaces/ISiteSettingsRepository.cs`, `Implementation/SiteSettingsRepository.cs` (new)
- `Services/Interfaces/ISiteSettingsService.cs`, `Implementation/SiteSettingsService.cs` (new)
- `Data/ApplicationDbContext.cs` — `DbSet<SiteSettings>`

---

## 5. Admin analytics endpoints

| Method | Route | Response | Auth |
|--------|-------|----------|------|
| GET | `/api/admin/analytics/attempts?days=30` | `[{ date, count }]` — daily completed attempts | Admin |
| GET | `/api/admin/analytics/pass-rate` | `{ averageScore, totalCompletedAttempts, passCount, passRate, passThreshold }` | Admin |

Pass threshold read from `SiteSettings.PassThreshold`.

**Files:**
- `Controllers/AdminAnalyticsController.cs` (new)
- `DTO/Analytics/AttemptVolumeDto.cs`, `PassRateAnalyticsDto.cs` (new)
- `Services/Interfaces/IAnalyticsService.cs`, `Implementation/AnalyticsService.cs` (new)

**Not implemented (no billing domain):** revenue analytics, audit/activity log.

---

## 6. User list & profile self-service

**DTO:** `DTO/User/UserListItemDto.cs` — added `CreatedAt`

**User service:** `UserService.MapToListItem` maps `CreatedAt`

| Method | Route | Body | Auth |
|--------|-------|------|------|
| PATCH | `/api/auth/me` | `{ displayName, firstName, email }` | Any authenticated user |

**Files:**
- `DTO/User/UpdateProfileRequest.cs` (new)
- `Validation/User/UpdateProfileRequestValidator.cs` (new)
- `Entities/User.cs` — `UpdateProfile()` method
- `Services/Interfaces/IUserService.cs` — `UpdateProfileAsync`
- `Controllers/AuthController.cs` — `PATCH /api/auth/me`

---

## 7. Route normalization (bug fixes)

Absolute `/api/...` routes applied for consistency (fixes relative-route 404s from nested SPA paths):

| Controller | Routes fixed |
|------------|--------------|
| `ExamController` | `PATCH/DELETE/POST` publish, update, delete |
| `DomainController` | `POST`, `PUT` |
| `AuthController` | `POST /api/auth/refresh` |

---

## 8. DI registration

`Extensions/ServiceExtensions.cs`:
- `IPublicStatsService` → `PublicStatsService`
- `ISiteSettingsService` → `SiteSettingsService`
- `IAnalyticsService` → `AnalyticsService`

`Extensions/RepositoryExtensions.cs`:
- `ISiteSettingsRepository` → `SiteSettingsRepository`

---

## 9. Existing endpoints reused (unchanged)

| Endpoint | Frontend use |
|----------|--------------|
| `GET /api/progress/domains` | Learner performance on Exams dashboard |
| `GET /api/admin/exams/stats` | Admin overview + analytics popularity list |
| `GET /api/exams/details` | Exam cards with `attemptCount` / `isMostPopular` |
| `GET /api/admin/users` | User tables with `createdAt` |

---

## 10. Deployment checklist

1. `dotnet ef database update`
2. Restart API
3. First visit to Admin → Settings seeds default `SiteSettings` row
4. Publish at least one exam so landing `publishedExamCount` > 0
5. Complete exam sessions so popularity badge and analytics populate

---

## 11. Optional follow-ups (not in scope)

| Feature | Suggested backend work |
|---------|------------------------|
| Registration gate | Check `allowRegistration` in `AuthController.Register` |
| Maintenance mode | Middleware returning 503 when `MaintenanceMode` is true |
| Email notifications | Wire `NotifyOnNewUser` / `NotifyOnExamComplete` to mail provider |
| Audit log | `AuditLog` entity + `GET /api/admin/audit-log` |
| Revenue analytics | Requires payment/billing module |
| CMS for About/Landing prose | Content API (marketing copy intentionally static per scope) |
