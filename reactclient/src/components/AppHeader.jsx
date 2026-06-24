import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import UserMenu from './UserMenu';
import Icon from './Icon';

const NAV_LINKS = [
  { label: 'Home', to: '/' },
  { label: 'Exams', to: '/exams' },
  { label: 'About', to: '/about' },
];

export default function AppHeader({ activeLink = 'Home', variant = 'default', onExamsClick }) {
  const navigate = useNavigate();
  const { isAuthenticated, isAdmin } = useAuth();

  const isExamHeader = variant === 'exam';

  if (isExamHeader) {
    return null;
  }

  return (
    <header
      className={`w-full h-16 sticky top-0 z-50 border-b transition-colors duration-200 ${
        isAuthenticated
          ? 'bg-white border-outline-variant shadow-sm'
          : 'bg-surface dark:bg-surface-container-low border-outline-variant dark:border-outline'
      }`}
    >
      <nav className="flex justify-between items-center px-margin-desktop max-w-container-max mx-auto h-full">
        <div className="flex items-center gap-md">
          <Link
            to="/"
            className="font-headline-md text-headline-md font-bold text-primary dark:text-primary-fixed tracking-tight"
          >
            PMI Exam Simulator
          </Link>
        </div>

        <div className="hidden md:flex items-center gap-xl h-full">
          {NAV_LINKS.filter((link) => !link.authOnly || isAuthenticated).map((link) => {
            const isActive = link.label === activeLink;
            const linkTo = link.label === 'Home' && isAdmin ? '/admin' : link.to;
            return (
              <Link
                key={link.label}
                to={linkTo}
                onClick={
                  link.label === 'Exams' && onExamsClick
                    ? (e) => {
                        if (!isAuthenticated) {
                          e.preventDefault();
                          onExamsClick();
                        }
                      }
                    : undefined
                }
                className={`h-full flex items-center px-xs transition-all duration-200 active:scale-95 font-body-md text-body-md ${
                  isActive
                    ? 'text-secondary dark:text-secondary-fixed-dim font-bold border-b-2 border-secondary'
                    : 'text-on-surface-variant dark:text-surface-variant hover:text-primary'
                }`}
              >
                {link.label}
              </Link>
            );
          })}
        </div>

        <div className="flex items-center gap-md">
          {isAuthenticated ? (
            <UserMenu />
          ) : (
            <>
              <button
                type="button"
                onClick={() => navigate('/login')}
                className="hidden sm:flex items-center gap-xs text-on-surface-variant font-label-lg px-lg py-sm hover:bg-surface-container-low rounded-lg transition-colors duration-200"
              >
                <Icon name="login" style={{ fontSize: 18 }} />
                Log In
              </button>
              <button
                type="button"
                onClick={() => navigate('/login')}
                className="bg-secondary-container text-on-secondary-container font-label-lg px-lg py-sm rounded-lg transition-all duration-200 active:scale-95 shadow-md hover:brightness-110 flex items-center gap-xs"
              >
                <Icon name="rocket_launch" style={{ fontSize: 18 }} />
                Get Started
              </button>
            </>
          )}
        </div>
      </nav>
    </header>
  );
}
