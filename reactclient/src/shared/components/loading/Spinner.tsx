const SIZE_CLASS = {
  sm: 'loader-spinner--sm',
  md: 'loader-spinner--md',
  lg: 'loader-spinner--lg',
  xl: 'loader-spinner--xl',
} as const;

type SpinnerSize = keyof typeof SIZE_CLASS;

type SpinnerProps = {
  size?: SpinnerSize;
  onDark?: boolean;
  className?: string;
  label?: string;
};

export default function Spinner({
  size = 'md',
  onDark = false,
  className = '',
  label = 'Loading',
}: SpinnerProps) {
  return (
    <span
      role="status"
      aria-live="polite"
      className={`inline-flex items-center justify-center ${className}`}
    >
      <span
        className={`loader-spinner ${SIZE_CLASS[size] || SIZE_CLASS.md} ${onDark ? 'loader-spinner--on-dark' : ''}`}
        aria-hidden="true"
      />
      <span className="sr-only">{label}</span>
    </span>
  );
}
