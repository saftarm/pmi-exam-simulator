import Skeleton from './Skeleton';

export default function ProfileSkeleton() {
  return (
    <div
      className="bg-white rounded-xl border border-outline-variant shadow-sm p-xl loading-enter"
      aria-hidden="true"
    >
      <div className="flex items-center gap-lg mb-xl">
        <Skeleton className="h-16 w-16 rounded-full shrink-0" />
        <div className="flex-1 space-y-sm">
          <Skeleton className="h-6 w-40" />
          <Skeleton className="h-4 w-28" />
        </div>
      </div>
      <div className="space-y-md">
        {Array.from({ length: 5 }).map((_, i) => (
          <div key={i} className="flex justify-between border-b border-outline-variant pb-md">
            <Skeleton className="h-4 w-24" />
            <Skeleton className="h-4 w-32" />
          </div>
        ))}
      </div>
    </div>
  );
}
