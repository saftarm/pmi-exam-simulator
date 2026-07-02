import Spinner from './Spinner';

type SpinnerSize = 'sm' | 'md' | 'lg' | 'xl';

type LoadingStateProps = {
  message?: string;
  fullScreen?: boolean;
  size?: SpinnerSize;
  className?: string;
  minHeight?: number | string;
};

export default function LoadingState({
  message,
  fullScreen = false,
  size = 'lg',
  className = '',
  minHeight,
}: LoadingStateProps) {
  const wrapperClass = fullScreen
    ? 'min-h-screen flex flex-col items-center justify-center bg-[#F4F5F7]'
    : 'flex flex-col items-center justify-center py-xl';

  return (
    <div
      className={`loading-enter ${wrapperClass} ${className}`}
      style={minHeight ? { minHeight } : undefined}
      role="status"
      aria-live="polite"
    >
      <Spinner size={size} label={message || 'Loading'} />
      {message && (
        <p className="mt-md text-sm text-on-surface-variant font-medium tracking-wide">{message}</p>
      )}
    </div>
  );
}
