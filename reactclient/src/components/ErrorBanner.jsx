export default function ErrorBanner({ message, onDismiss, variant = 'error' }) {
  if (!message) return null;

  const styles =
    variant === 'warning'
      ? 'bg-amber-50 text-amber-900 border-amber-200'
      : 'bg-red-50 text-red-700 border-red-200';

  return (
    <div
      className={`mb-lg p-md rounded-lg border flex justify-between gap-md loading-enter ${styles}`}
    >
      <span>{message}</span>
      {onDismiss && (
        <button type="button" onClick={onDismiss} className="shrink-0 font-bold">
          Dismiss
        </button>
      )}
    </div>
  );
}
