import Skeleton from './Skeleton';

function SingleExamCardSkeleton() {
  return (
    <div className="bg-white rounded-xl border border-outline-variant overflow-hidden flex flex-col">
      <Skeleton className="h-48 w-full rounded-none" />
      <div className="p-lg flex flex-col gap-md">
        <Skeleton className="h-7 w-3/4" />
        <Skeleton className="h-4 w-1/2" />
        <div className="grid grid-cols-2 gap-md my-sm">
          <Skeleton className="h-4 w-full" />
          <Skeleton className="h-4 w-full" />
          <Skeleton className="h-4 w-full" />
          <Skeleton className="h-4 w-full" />
        </div>
        <div className="mt-auto pt-lg border-t border-outline-variant space-y-sm">
          <Skeleton className="h-10 w-full rounded-lg" />
          <Skeleton className="h-12 w-full rounded-lg" />
        </div>
      </div>
    </div>
  );
}

export default function ExamCardSkeleton({ count = 3, className = '' }) {
  return (
    <div
      className={`grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-xl loading-stagger ${className}`}
      aria-hidden="true"
    >
      {Array.from({ length: count }).map((_, i) => (
        <SingleExamCardSkeleton key={i} />
      ))}
    </div>
  );
}
