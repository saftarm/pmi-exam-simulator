import Spinner from './Spinner';

export default function LoadingButton({
    loading = false,
    loadingText,
    children,
    className = '',
    spinnerSize = 'sm',
    type = 'button',
    ...props
}) {
    const isDisabled = loading || props.disabled;

    return (
        <button
            type={type}
            {...props}
            disabled={isDisabled}
            aria-busy={loading || undefined}
            className={`inline-flex items-center justify-center gap-sm transition-opacity duration-200 ${loading ? 'opacity-80' : ''} ${className}`}
        >
            {loading && <Spinner size={spinnerSize} label={loadingText || 'Loading'} />}
            <span>{loading ? (loadingText ?? children) : children}</span>
        </button>
    );
}
