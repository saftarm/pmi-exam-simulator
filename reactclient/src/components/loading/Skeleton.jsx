export default function Skeleton({ className = '', rounded = 'rounded-md', style }) {
    return (
        <span
            className={`skeleton-shimmer ${rounded} ${className}`}
            aria-hidden="true"
            style={style}
        />
    );
}
