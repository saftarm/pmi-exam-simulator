import Skeleton from './Skeleton';

function SingleStatCardSkeleton() {
    return (
        <div className="bg-white p-lg rounded-xl border border-outline-variant shadow-sm">
            <div className="flex justify-between items-start mb-md">
                <Skeleton className="h-10 w-10 rounded-lg" />
                <Skeleton className="h-3 w-16" />
            </div>
            <Skeleton className="h-4 w-24 mb-xs" />
            <Skeleton className="h-8 w-20" />
        </div>
    );
}

export default function StatCardSkeleton({ count = 4, className = '' }) {
    return (
        <div
            className={`grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-lg loading-stagger ${className}`}
            aria-hidden="true"
        >
            {Array.from({ length: count }).map((_, i) => (
                <SingleStatCardSkeleton key={i} />
            ))}
        </div>
    );
}
