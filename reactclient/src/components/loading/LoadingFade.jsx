export function LoadingFade({ show, children, className = '' }) {
    if (!show) return null;
    return <div className={`loading-enter ${className}`}>{children}</div>;
}

export function ContentReveal({ show, children, className = '' }) {
    if (!show) return null;
    return <div className={`content-reveal ${className}`}>{children}</div>;
}
