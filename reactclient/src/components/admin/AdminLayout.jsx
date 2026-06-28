import { useState } from 'react';
import { Link, NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import Icon from '../Icon';
import { ADMIN_NAV_ITEMS } from './adminNav';
import { ADMIN_APP_NAME } from '../../constants/branding';
import { getInitials } from '../../utils/userDisplay';
import UserAvatar from '../UserAvatar';

function AdminAccountFooter({ onNavigate }) {
  const navigate = useNavigate();
  const { logout } = useAuth();

  const handleLogout = () => {
    onNavigate?.();
    logout();
    navigate('/');
  };

  const linkClass =
    'flex items-center gap-sm px-md py-sm rounded-lg text-sm opacity-80 hover:opacity-100 hover:bg-white/5 transition-colors w-full text-left';

  return (
    <div className="space-y-xs">
      <Link to="/profile" onClick={onNavigate} className={linkClass}>
        <Icon name="person" style={{ fontSize: 18 }} />
        Profile
      </Link>
      <Link to="/exams" onClick={onNavigate} className={linkClass}>
        <Icon name="quiz" style={{ fontSize: 18 }} />
        My Exams
      </Link>
      <button type="button" onClick={handleLogout} className={linkClass}>
        <Icon name="logout" style={{ fontSize: 18 }} />
        Log out
      </button>
    </div>
  );
}

function AdminSidebar({ onNavigate }) {
  const { user } = useAuth();
  const displayName = user?.displayName || user?.userName || 'Admin';

  return (
    <>
      <div className="h-16 flex items-center px-lg border-b border-white/10 gap-md min-w-0">
        <Link
          to="/admin"
          onClick={onNavigate}
          className="font-headline-md text-headline-md font-extrabold tracking-tight hover:text-secondary-fixed-dim transition-colors shrink-0"
        >
          {ADMIN_APP_NAME}
        </Link>
        <Link
          to="/profile"
          onClick={onNavigate}
          className="hidden lg:flex items-center gap-sm min-w-0 border-l border-white/10 pl-md hover:opacity-90 transition-opacity"
        >
          <UserAvatar name={displayName} size="sm" className="ring-white/20" />
          <div className="min-w-0">
            <p className="font-label-sm text-label-sm truncate">{displayName}</p>
            <p className="text-[10px] opacity-60 truncate">{user?.role || 'Administrator'}</p>
          </div>
        </Link>
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
      <div className="p-lg border-t border-white/10 hidden lg:block">
        <AdminAccountFooter onNavigate={onNavigate} />
      </div>
      <div className="p-lg border-t border-white/10 lg:hidden">
        <Link to="/profile" onClick={onNavigate} className="flex items-center gap-md mb-md">
          <div className="w-10 h-10 rounded-full bg-secondary-container flex items-center justify-center text-white font-bold">
            {getInitials(displayName)}
          </div>
          <div className="min-w-0">
            <p className="font-label-lg text-label-lg truncate">{displayName}</p>
            <p className="text-xs opacity-60">{user?.role || 'Administrator'}</p>
          </div>
        </Link>
        <AdminAccountFooter onNavigate={onNavigate} />
      </div>
    </>
  );
}

export function AdminTopBar({ title, children, showNewExam, onNewExam }) {
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
        {showNewExam && (
          <div className="flex items-center gap-lg">
            <button
              type="button"
              onClick={() => (onNewExam ? onNewExam() : navigate('/admin/exams/new'))}
              className="bg-secondary-container hover:bg-secondary text-white px-md py-sm rounded-lg font-label-lg text-label-lg transition-all flex items-center gap-sm"
            >
              <Icon name="add" style={{ fontSize: 20 }} />
              <span className="hidden sm:inline">New Exam</span>
            </button>
          </div>
        )}
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

export default function AdminLayout({ title, children, topBarExtra, showNewExam = false, onNewExam }) {
  return (
    <div className="flex min-h-screen overflow-hidden bg-[#F4F5F7] text-on-surface">
      <aside className="pmi-sidebar w-64 flex-shrink-0 hidden md:flex flex-col text-white bg-[#001430] min-h-screen">
        <AdminSidebar />
      </aside>
      <main className="flex-1 flex flex-col min-w-0 overflow-y-auto">
        <AdminTopBar title={title} showNewExam={showNewExam} onNewExam={onNewExam}>
          {topBarExtra}
        </AdminTopBar>
        <div className="p-margin-desktop max-w-container-max mx-auto w-full flex-1">{children}</div>
      </main>
    </div>
  );
}
