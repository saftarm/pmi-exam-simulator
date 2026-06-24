import Skeleton from './Skeleton';

export default function ProgressSkeleton() {
  return (
    <div className="space-y-lg loading-enter" aria-hidden="true">
      <div className="bg-white rounded-xl border border-outline-variant shadow-sm overflow-hidden">
        <div className="p-lg border-b border-outline-variant">
          <Skeleton className="h-6 w-48" />
        </div>
        <div className="p-lg space-y-md">
          {Array.from({ length: 4 }).map((_, i) => (
            <div key={i} className="flex justify-between items-center">
              <Skeleton className="h-4 w-40" />
              <Skeleton className="h-4 w-16" />
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
