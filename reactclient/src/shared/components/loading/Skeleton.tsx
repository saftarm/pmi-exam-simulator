import type { CSSProperties } from 'react';

type SkeletonProps = {
  className?: string;
  rounded?: string;
  style?: CSSProperties;
};

export default function Skeleton({
  className = '',
  rounded = 'rounded-md',
  style,
}: SkeletonProps) {
  return (
    <span className={`skeleton-shimmer ${rounded} ${className}`} aria-hidden="true" style={style} />
  );
}
