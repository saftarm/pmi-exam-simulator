import { Link } from 'react-router-dom';
import Icon from './Icon';

export default function BackLink({ to, label, onClick }) {
  const className =
    'mb-lg text-sm text-secondary-container font-bold hover:underline flex items-center gap-xs';

  if (onClick) {
    return (
      <button type="button" onClick={onClick} className={className}>
        <Icon name="arrow_back" style={{ fontSize: 18 }} />
        {label}
      </button>
    );
  }

  return (
    <Link to={to} className={className}>
      <Icon name="arrow_back" style={{ fontSize: 18 }} />
      {label}
    </Link>
  );
}
