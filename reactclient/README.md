# PMI Exam Simulator — React Client

TypeScript React frontend for the TestAPI backend. Uses Vite, React Router, and a feature-sliced folder layout.

## Scripts

| Script | Description |
|--------|-------------|
| `npm run dev` | Start Vite dev server |
| `npm run build` | Typecheck and production build |
| `npm run typecheck` | Run TypeScript without emitting |
| `npm run lint` | ESLint across the project |
| `npm run test` | Node test runner (`postLoginPath` unit tests) |
| `npm run preview` | Preview production build |
| `npm run api` | Start the ASP.NET API (`dotnet run` in repo root) |
| `npm run start` | Run API and dev server together |

## Folder structure

```
src/
  app/                 Entry point, routes, app-level layouts
    main.tsx
    App.tsx
    routes.tsx
    layouts/
  features/            Feature slices (pages, components, api, types)
    auth/
    public-home/
    learner-exams/
    exam-session/
    learner-progress/
    profile/
    admin-dashboard/
    admin-exams/
    admin-questions/
    admin-categories/
    admin-users/
    admin-analytics/
    admin-settings/
  shared/              Cross-feature reuse only
    api/               HTTP client, primitives, error helpers
    components/        UI primitives (Icon, loading, modals, etc.)
    constants/
    utils/
```

**Import direction:** `app` → `features` → `shared`. Feature types stay in each feature's `types.ts`; shared code must not import features.

## Local development

1. From `reactclient/`, run `npm install` if needed.
2. Start both stacks: `npm run start` (or run `npm run api` and `npm run dev` in separate terminals).
3. Open the Vite URL (typically `http://localhost:5173`).

Ensure the API is configured (connection string, JWT, etc.) per the root `TestAPI` project.

## Validation

```bash
npm run typecheck
npm run lint
npm run test
npm run build
```
