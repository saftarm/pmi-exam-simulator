import { useState, useRef, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../useAuth';
import UserAvatar from '../../../shared/components/UserAvatar';
import Icon from '../../../shared/components/Icon';

const MENU_ITEMS = [
  { label: 'Profile', to: '/profile', icon: 'person' },
  { label: 'My Exams', to: '/exams', icon: 'quiz' },
] as const;

type MenuItem = (typeof MENU_ITEMS)[number] | { label: string; to: string; icon: string };

export default function UserMenu() {
  const [open, setOpen] = useState(false);
  const menuRef = useRef<HTMLDivElement>(null);
  const navigate = useNavigate();
  const { user, isAdmin, logout } = useAuth();

  const displayName = user?.displayName || user?.userName || 'User';

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        setOpen(false);
      }
    };
    if (open) {
      document.addEventListener('mousedown', handleClickOutside);
    }
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [open]);

  useEffect(() => {
    const handleEscape = (event: KeyboardEvent) => {
      if (event.key === 'Escape') setOpen(false);
    };
    if (open) {
      document.addEventListener('keydown', handleEscape);
    }
    return () => document.removeEventListener('keydown', handleEscape);
  }, [open]);

  const handleLogout = () => {
    setOpen(false);
    logout();
    navigate('/');
  };

  const adminItem: MenuItem[] = isAdmin
    ? [{ label: 'Admin Panel', to: '/admin', icon: 'admin_panel_settings' }]
    : [];

  return (
    <div className="relative" ref={menuRef}>
      <button
        type="button"
        onClick={() => setOpen((prev) => !prev)}
        className={`flex items-center gap-sm pl-xs pr-sm py-xs rounded-full border transition-all duration-200 ${
          open
            ? 'bg-surface-container-low border-outline-variant shadow-sm'
            : 'border-transparent hover:bg-surface-container-low hover:border-outline-variant'
        }`}
        aria-expanded={open}
        aria-haspopup="menu"
        aria-label={`Account menu for ${displayName}`}
      >
        <UserAvatar name={displayName} size="md" />
        <span className="hidden sm:block font-label-lg text-label-lg text-primary max-w-[8rem] truncate">
          {displayName}
        </span>
        <Icon
          name="expand_more"
          className={`text-on-surface-variant transition-transform duration-200 ${open ? 'rotate-180' : ''}`}
          style={{ fontSize: 20 }}
        />
      </button>

      {open && (
        <div
          role="menu"
          className="absolute right-0 mt-sm w-60 bg-white rounded-xl border border-outline-variant shadow-lg py-xs z-50 loading-enter"
        >
          <div className="px-md py-sm border-b border-outline-variant">
            <div className="flex items-center gap-sm">
              <UserAvatar name={displayName} size="lg" />
              <div className="min-w-0">
                <p className="font-bold text-sm text-primary truncate">{displayName}</p>
                <p className="text-xs text-on-surface-variant truncate">@{user?.userName}</p>
              </div>
            </div>
            {user?.role && (
              <span className="inline-block mt-sm px-sm py-xs rounded-full text-[10px] font-bold uppercase tracking-wider bg-primary/10 text-primary">
                {user.role}
              </span>
            )}
          </div>

          {[...MENU_ITEMS, ...adminItem].map((item) => (
            <Link
              key={item.to}
              to={item.to}
              role="menuitem"
              onClick={() => setOpen(false)}
              className="flex items-center gap-sm px-md py-sm text-sm text-on-surface hover:bg-surface-container-low transition-colors"
            >
              <Icon name={item.icon} style={{ fontSize: 18 }} className="text-on-surface-variant" />
              {item.label}
            </Link>
          ))}

          <div className="border-t border-outline-variant mt-xs pt-xs">
            <button
              type="button"
              role="menuitem"
              onClick={handleLogout}
              className="w-full flex items-center gap-sm px-md py-sm text-sm text-red-600 hover:bg-red-50 transition-colors"
            >
              <Icon name="logout" style={{ fontSize: 18 }} />
              Log Out
            </button>
          </div>
        </div>
      )}
    </div>
  );
}
