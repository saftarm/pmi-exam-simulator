import Skeleton from './Skeleton';

export default function FormSkeleton({ fields = 6 }) {
  return (
    <div
      className="bg-white rounded-xl border border-outline-variant shadow-sm p-xl space-y-lg loading-enter max-w-3xl"
      aria-hidden="true"
    >
      {Array.from({ length: fields }).map((_, i) => (
        <div key={i} className="space-y-xs">
          <Skeleton className="h-3 w-24" />
          <Skeleton className="h-10 w-full rounded-lg" />
        </div>
      ))}
      <div className="flex gap-md pt-md">
        <Skeleton className="h-10 w-28 rounded-lg" />
        <Skeleton className="h-10 w-24 rounded-lg" />
      </div>
    </div>
  );
}
