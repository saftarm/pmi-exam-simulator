import { useState } from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import Icon from '../Icon';
import { ADMIN_NAV_ITEMS } from './adminNav';
import { getInitials } from '../../utils/userDisplay';

function AdminSidebar({ onNavigate }) {
    const { user } = useAuth();
    const initials = getInitials(user?.displayName || user?.userName);

    return (
        <>
            <div className="h-16 flex items-center px-lg border-b border-white/10">
                <span className="font-headline-md text-headline-md font-extrabold tracking-tight">PMI Admin</span>
            </div>
            <nav className="flex-1 mt-md px-sm space-y-sm overflow-y-auto">
                {ADMIN_NAV_ITEMS.map((item) => (
                    <NavLink
                        key={item.to}
                        to={item.to}
                        end={item.end}
                        onClick={onNavigate}
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
                        <p className="font-label-lg text-label-lg">{user?.displayName || user?.userName || 'Admin'}</p>
                        <p className="text-xs opacity-60">{user?.role || 'Administrator'}</p>
                    </div>
                </div>
            </div>
        </>
    );
}

export function AdminTopBar({ title, children, onNewExam }) {
    const navigate = useNavigate();
    const [mobileOpen, setMobileOpen] = useState(false);

    return (
        <>
            <header className="h-16 bg-white border-b border-outline-variant px-margin-desktop flex items-center justify-between sticky top-0 z-20">
                <div className="flex items-center gap-md">
                    <button
                        type="button"
                        className="md:hidden text-on-surface-variant"
                        onClick={() => setMobileOpen(true)}
                        aria-label="Open admin menu"
                    >
                        <Icon name="menu" />
                    </button>
                    <div className="flex items-center gap-xl">
                        <h1 className="font-headline-md text-headline-md text-primary font-bold">{title}</h1>
                        {children}
                    </div>
                </div>
                <div className="flex items-center gap-lg">
                    <button
                        type="button"
                        onClick={() => navigate('/exams')}
                        className="hidden sm:block text-sm text-on-surface-variant hover:text-primary font-medium"
                    >
                        Learner view
                    </button>
                    <button
                        type="button"
                        onClick={() => (onNewExam ? onNewExam() : navigate('/admin/exams/new'))}
                        className="bg-secondary-container hover:bg-secondary text-white px-md py-sm rounded-lg font-label-lg text-label-lg transition-all flex items-center gap-sm"
                    >
                        <Icon name="add" style={{ fontSize: 20 }} />
                        <span className="hidden sm:inline">New Exam</span>
                    </button>
                </div>
            </header>

            {mobileOpen && (
                <div className="fixed inset-0 z-30 md:hidden">
                    <button
                        type="button"
                        className="absolute inset-0 bg-black/40"
                        onClick={() => setMobileOpen(false)}
                        aria-label="Close admin menu"
                    />
                    <aside className="relative w-72 max-w-[85vw] h-full flex flex-col text-white bg-[#001430] shadow-xl">
                        <button
                            type="button"
                            className="absolute top-4 right-4 text-white/80"
                            onClick={() => setMobileOpen(false)}
                        >
                            <Icon name="close" />
                        </button>
                        <AdminSidebar onNavigate={() => setMobileOpen(false)} />
                    </aside>
                </div>
            )}
        </>
    );
}

export default function AdminLayout({ title, children, topBarExtra, onNewExam }) {
    return (
        <div className="flex min-h-screen overflow-hidden bg-[#F4F5F7] text-on-surface">
            <aside className="pmi-sidebar w-64 flex-shrink-0 hidden md:flex flex-col text-white bg-[#001430] min-h-screen">
                <AdminSidebar />
            </aside>
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
