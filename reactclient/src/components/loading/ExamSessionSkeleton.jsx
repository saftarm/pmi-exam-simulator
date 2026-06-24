import Skeleton from './Skeleton';
import Spinner from './Spinner';

export default function ExamSessionSkeleton() {
  return (
    <div
      className="h-screen flex flex-col bg-background loading-enter"
      role="status"
      aria-live="polite"
    >
      <div className="border-b border-outline-variant bg-white px-lg py-md flex items-center justify-between">
        <Skeleton className="h-6 w-32" />
        <div className="flex items-center gap-md">
          <Spinner size="sm" label="Loading exam session" />
          <Skeleton className="h-8 w-24 rounded-lg" />
        </div>
      </div>
      <div className="flex flex-1 overflow-hidden">
        <aside className="hidden lg:block w-64 border-r border-outline-variant p-md space-y-sm">
          {Array.from({ length: 12 }).map((_, i) => (
            <Skeleton key={i} className="h-8 w-full rounded-md" />
          ))}
        </aside>
        <main className="flex-1 p-xl space-y-lg">
          <Skeleton className="h-4 w-24" />
          <Skeleton className="h-8 w-full max-w-2xl" />
          <Skeleton className="h-4 w-3/4 max-w-xl" />
          <div className="space-y-md mt-xl max-w-2xl">
            {Array.from({ length: 4 }).map((_, i) => (
              <Skeleton key={i} className="h-14 w-full rounded-lg" />
            ))}
          </div>
        </main>
      </div>
      <p className="sr-only">Loading exam session…</p>
    </div>
  );
}
