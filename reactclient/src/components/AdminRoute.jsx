import { useEffect, useState } from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import LoadingState from './loading/LoadingState';

export default function AdminRoute({ children }) {
    const { isAuthenticated, isAdmin, profileLoading, refreshUser } = useAuth();
    const location = useLocation();
    const [checked, setChecked] = useState(false);

    useEffect(() => {
        let cancelled = false;
        if (isAuthenticated && !checked) {
            refreshUser()
                .catch(() => {})
                .finally(() => {
                    if (!cancelled) setChecked(true);
                });
        } else if (!isAuthenticated) {
            setChecked(true);
        }
        return () => { cancelled = true; };
    }, [isAuthenticated, refreshUser, checked]);

    if (!checked || profileLoading) {
        return (
            <LoadingState
                fullScreen
                message="Loading admin…"
            />
        );
    }

    if (!isAuthenticated) {
        return <Navigate to="/login" state={{ from: location }} replace />;
    }

    if (!isAdmin) {
        return (
            <div className="min-h-screen flex flex-col items-center justify-center bg-[#F4F5F7] px-md text-center content-reveal">
                <h1 className="text-2xl font-bold text-primary mb-sm">Access denied</h1>
                <p className="text-on-surface-variant mb-lg">Admin role is required to view this area.</p>
                <a href="/exams" className="text-secondary-container font-bold hover:underline">
                    Go to My Exams
                </a>
            </div>
        );
    }

    return children;
}
