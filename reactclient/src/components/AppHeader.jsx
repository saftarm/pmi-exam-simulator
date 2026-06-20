import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const NAV_LINKS = [
    { label: 'Home', to: '/' },
    { label: 'Exams', to: '/exams' },
    { label: 'About', to: '#about' },
];

export default function AppHeader({
    activeLink = 'Home',
    variant = 'default',
    onExamsClick,
}) {
    const navigate = useNavigate();
    const { isAuthenticated, logout } = useAuth();

    const isExamHeader = variant === 'exam';

    if (isExamHeader) {
        return null;
    }

    return (
        <header className="bg-surface dark:bg-surface-container-low border-b border-outline-variant dark:border-outline w-full h-16 sticky top-0 z-50">
            <nav className="flex justify-between items-center px-margin-desktop max-w-container-max mx-auto h-full">
                <Link
                    to="/"
                    className="font-headline-md text-headline-md font-bold text-primary dark:text-primary-fixed tracking-tight"
                >
                    PMI Exam Simulator
                </Link>
                <div className="hidden md:flex items-center gap-xl h-full">
                    {NAV_LINKS.map((link) => {
                        const isActive = link.label === activeLink;
                        return (
                            <Link
                                key={link.label}
                                to={link.to}
                                onClick={link.label === 'Exams' && onExamsClick ? (e) => {
                                    if (!isAuthenticated) {
                                        e.preventDefault();
                                        onExamsClick();
                                    }
                                } : undefined}
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
                        <>
                            <button
                                type="button"
                                onClick={() => navigate('/admin')}
                                className="hidden md:block font-body-md text-body-md text-on-surface-variant hover:bg-surface-container-low transition-colors px-md py-sm rounded"
                            >
                                Admin
                            </button>
                            <button
                                type="button"
                                onClick={() => navigate('/exams')}
                                className="hidden md:block font-body-md text-body-md text-on-surface-variant hover:bg-surface-container-low transition-colors px-md py-sm rounded"
                            >
                                My Exams
                            </button>
                            <button
                                type="button"
                                onClick={() => { logout(); navigate('/'); }}
                                className="font-label-lg text-label-lg px-md py-sm rounded-lg text-primary dark:text-primary-fixed hover:bg-surface-container-low dark:hover:bg-surface-container-highest transition-colors"
                            >
                                Log Out
                            </button>
                        </>
                    ) : (
                        <>
                            <button
                                type="button"
                                onClick={() => navigate('/login')}
                                className="hidden sm:block text-on-surface-variant font-label-lg px-lg py-sm hover:bg-surface-container-low transition-colors duration-200"
                            >
                                Log In
                            </button>
                            <button
                                type="button"
                                onClick={() => navigate('/login')}
                                className="bg-secondary-container text-on-secondary-container font-label-lg px-lg py-sm rounded transition-all duration-200 active:scale-95 shadow-md hover:brightness-110"
                            >
                                Get Started
                            </button>
                        </>
                    )}
                </div>
            </nav>
        </header>
    );
}
