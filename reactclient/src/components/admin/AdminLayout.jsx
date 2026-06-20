import { NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import Icon from '../Icon';

const NAV_ITEMS = [
    { to: '/admin', label: 'Overview', icon: 'dashboard', end: true },
    { to: '/admin/exams', label: 'Manage Exams', icon: 'quiz' },
    { to: '/admin/users', label: 'User Management', icon: 'group' },
    { to: '/admin/analytics', label: 'Analytics', icon: 'analytics' },
    { to: '/admin/categories', label: 'Categories', icon: 'category' },
    { to: '/admin/settings', label: 'Settings', icon: 'settings' },
];

function AdminSidebar() {
    const { user } = useAuth();
    const initials = (user?.userName || 'AD').slice(0, 2).toUpperCase();

    return (
        <aside className="pmi-sidebar w-64 flex-shrink-0 hidden md:flex flex-col text-white bg-[#001430] min-h-screen">
            <div className="h-16 flex items-center px-lg border-b border-white/10">
                <span className="font-headline-md text-headline-md font-extrabold tracking-tight">PMI Admin</span>
            </div>
            <nav className="flex-1 mt-md px-sm space-y-sm">
                {NAV_ITEMS.map((item) => (
                    <NavLink
                        key={item.to}
                        to={item.to}
                        end={item.end}
                        className={({ isActive }) =>
                            `flex items-center gap-md px-md py-sm rounded-lg transition-colors ${
                                isActive
                                    ? 'bg-white/10 border-l-4 border-secondary-container'
                                    : 'hover:bg-white/5 border-l-4 border-transparent'
                            }`
                        }
                    >
                        <Icon name={item.icon} />
                        <span className="font-label-lg text-label-lg">{item.label}</span>
                    </NavLink>
                ))}
            </nav>
            <div className="p-lg border-t border-white/10">
                <div className="flex items-center gap-md">
                    <div className="w-10 h-10 rounded-full bg-secondary-container flex items-center justify-center text-white font-bold">
                        {initials}
                    </div>
                    <div>
                        <p className="font-label-lg text-label-lg">{user?.userName || 'Admin User'}</p>
                        <p className="text-xs opacity-60">Super Administrator</p>
                    </div>
                </div>
            </div>
        </aside>
    );
}

export function AdminTopBar({ title, children, onNewExam }) {
    const navigate = useNavigate();

    return (
        <header className="h-16 bg-white border-b border-outline-variant px-margin-desktop flex items-center justify-between sticky top-0 z-10">
            <div className="flex items-center gap-xl">
                <h1 className="font-headline-md text-headline-md text-primary font-bold">{title}</h1>
                {children}
            </div>
            <div className="flex items-center gap-lg">
                <button type="button" className="text-on-surface-variant hover:text-primary transition-colors">
                    <Icon name="notifications" />
                </button>
                <button type="button" className="text-on-surface-variant hover:text-primary transition-colors">
                    <Icon name="help_outline" />
                </button>
                <div className="h-8 w-px bg-outline-variant" />
                <button
                    type="button"
                    onClick={() => (onNewExam ? onNewExam() : navigate('/admin/exams/new'))}
                    className="bg-secondary-container hover:bg-secondary text-white px-md py-sm rounded-lg font-label-lg text-label-lg transition-all flex items-center gap-sm"
                >
                    <Icon name="add" style={{ fontSize: 20 }} />
                    New Exam
                </button>
            </div>
        </header>
    );
}

export default function AdminLayout({ title, children, topBarExtra, onNewExam }) {
    return (
        <div className="flex min-h-screen overflow-hidden bg-[#F4F5F7] text-on-surface">
            <AdminSidebar />
            <main className="flex-1 flex flex-col min-w-0 overflow-y-auto">
                <AdminTopBar title={title} onNewExam={onNewExam}>
                    {topBarExtra}
                </AdminTopBar>
                <div className="p-margin-desktop max-w-container-max mx-auto w-full flex-1">
                    {children}
                </div>
            </main>
        </div>
    );
}
