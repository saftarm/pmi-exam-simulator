import type { CSSProperties } from 'react';

type IconProps = {
  name: string;
  className?: string;
  filled?: boolean;
  style?: CSSProperties;
};

export default function Icon({ name, className = '', filled = false, style = {} }: IconProps) {
  return (
    <span
      className={`material-symbols-outlined ${className}`}
      style={{
        fontVariationSettings: filled ? "'FILL' 1, 'wght' 400, 'GRAD' 0, 'opsz' 24" : undefined,
        ...style,
      }}
    >
      {name}
    </span>
  );
}
