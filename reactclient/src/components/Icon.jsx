export default function Icon({ name, className = '', filled = false, style = {} }) {
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
