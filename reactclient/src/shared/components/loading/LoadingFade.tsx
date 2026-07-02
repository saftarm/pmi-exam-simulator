import type { ReactNode } from 'react';

type FadeProps = {
  show: boolean;
  children: ReactNode;
  className?: string;
};

export function LoadingFade({ show, children, className = '' }: FadeProps) {
  if (!show) return null;
  return <div className={`loading-enter ${className}`}>{children}</div>;
}

export function ContentReveal({ show, children, className = '' }: FadeProps) {
  if (!show) return null;
  return <div className={`content-reveal ${className}`}>{children}</div>;
}
