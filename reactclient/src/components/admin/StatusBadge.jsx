import Icon from '../Icon';
import { Skeleton } from '../loading';

const STATUS_STYLES = {
    success: 'bg-green-100 text-green-700',
    error: 'bg-red-100 text-red-700',
    info: 'bg-blue-100 text-blue-700',
    neutral: 'bg-gray-100 text-gray-700',
    warning: 'bg-amber-100 text-amber-700',
};

export default function StatusBadge({ status, type = 'neutral' }) {
    return (
        <span className={`px-sm py-xs rounded-full text-xs font-bold ${STATUS_STYLES[type] || STATUS_STYLES.neutral}`}>
            {status}
        </span>
    );
}

export function StatCard({
    label,
    value,
    change,
    changeType = 'positive',
    icon,
    iconBg = 'bg-blue-50',
    iconColor = 'text-primary',
    loading = false,
}) {
    const changeClass = changeType === 'positive' ? 'text-green-600' : 'text-on-surface-variant';

    return (
        <div className="bg-white p-lg rounded-xl border border-outline-variant shadow-sm">
            <div className="flex justify-between items-start mb-md">
                {loading ? (
                    <Skeleton className="h-10 w-10 rounded-lg" />
                ) : (
                    <div className={`p-sm ${iconBg} ${iconColor} rounded-lg`}>
                        <Icon name={icon} />
                    </div>
                )}
                {change && !loading && (
                    <span className={`text-xs font-bold ${changeClass}`}>{change}</span>
                )}
                {loading && <Skeleton className="h-3 w-16" />}
            </div>
            <p className="text-on-surface-variant font-label-lg text-label-lg">{label}</p>
            {loading ? (
                <Skeleton className="h-8 w-20 mt-xs" />
            ) : (
                <h3 className="font-headline-md text-headline-md font-bold mt-xs">{value}</h3>
            )}
        </div>
    );
}
