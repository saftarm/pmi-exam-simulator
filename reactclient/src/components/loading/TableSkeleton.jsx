import Skeleton from './Skeleton';

export default function TableSkeleton({ rows = 5, columns = 4, cellClassName = 'px-lg py-md' }) {
  return (
    <>
      {Array.from({ length: rows }).map((_, rowIndex) => (
        <tr key={rowIndex} aria-hidden="true">
          {Array.from({ length: columns }).map((__, colIndex) => (
            <td key={colIndex} className={cellClassName}>
              <Skeleton className={`h-4 ${colIndex === 0 ? 'w-3/4' : 'w-full'}`} />
            </td>
          ))}
        </tr>
      ))}
    </>
  );
}

export function TableLoadingOverlay({ columns = 4, message = 'Loading…' }) {
  return (
    <tr>
      <td colSpan={columns} className="px-lg py-xl">
        <div className="flex flex-col items-center justify-center gap-sm loading-enter">
          <span className="loader-spinner loader-spinner--md" aria-hidden="true" />
          <span className="text-sm text-on-surface-variant">{message}</span>
        </div>
      </td>
    </tr>
  );
}
