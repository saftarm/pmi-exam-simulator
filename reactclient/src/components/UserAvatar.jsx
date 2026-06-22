import { getInitials } from '../utils/userDisplay';

const SIZE_CLASS = {
    sm: 'w-8 h-8 text-xs',
    md: 'w-9 h-9 text-sm',
    lg: 'w-10 h-10 text-sm',
};

export default function UserAvatar({ name, size = 'md', className = '' }) {
    return (
        <div
            className={`rounded-full bg-primary flex items-center justify-center text-white font-bold shrink-0 ring-2 ring-secondary-container/25 ${SIZE_CLASS[size] || SIZE_CLASS.md} ${className}`}
            aria-hidden="true"
        >
            {getInitials(name)}
        </div>
    );
}
