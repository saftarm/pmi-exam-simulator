import Skeleton from './Skeleton';

export default function DetailPageSkeleton() {
  return (
    <div className="grid grid-cols-1 lg:grid-cols-3 gap-lg loading-enter" aria-hidden="true">
      <div className="lg:col-span-2 bg-white rounded-xl border border-outline-variant shadow-sm p-xl space-y-md">
        <Skeleton className="h-9 w-2/3" />
        <Skeleton className="h-4 w-full" />
        <Skeleton className="h-4 w-4/5" />
        <div className="grid grid-cols-2 gap-md pt-md">
          <Skeleton className="h-14 w-full rounded-lg" />
          <Skeleton className="h-14 w-full rounded-lg" />
        </div>
        <Skeleton className="h-12 w-48 rounded-lg mt-md" />
      </div>
      <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg space-y-md">
        <Skeleton className="h-6 w-40" />
        {Array.from({ length: 4 }).map((_, i) => (
          <div key={i} className="py-sm border-b border-outline-variant last:border-0">
            <Skeleton className="h-4 w-full mb-xs" />
            <Skeleton className="h-3 w-3/4" />
          </div>
        ))}
      </div>
    </div>
  );
}
