export default function Pagination({
  page,
  hasNextPage,
  hasPreviousPage,
  loading = false,
  onPageChange,
  totalLabel,
}) {
  const canGoPrev = hasPreviousPage ?? page > 1;

  return (
    <div className="flex justify-between items-center mt-lg text-sm text-on-surface-variant">
      {totalLabel && <span>{totalLabel}</span>}
      <div className="flex gap-md ml-auto">
        <button
          type="button"
          disabled={!canGoPrev || loading}
          onClick={() => onPageChange(page - 1)}
          className="px-md py-sm border border-outline-variant rounded-lg disabled:opacity-50"
        >
          Previous
        </button>
        <span className="py-sm">Page {page}</span>
        <button
          type="button"
          disabled={!hasNextPage || loading}
          onClick={() => onPageChange(page + 1)}
          className="px-md py-sm border border-outline-variant rounded-lg disabled:opacity-50"
        >
          Next
        </button>
      </div>
    </div>
  );
}
