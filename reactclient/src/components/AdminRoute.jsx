import { useEffect, useState } from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import LoadingState from './loading/LoadingState';

export default function AdminRoute({ children }) {
  const { isAuthenticated, isAdmin, profileLoading, authReady, refreshUser } = useAuth();
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
    return () => {
      cancelled = true;
    };
  }, [isAuthenticated, refreshUser, checked]);

  if (!authReady || !checked || profileLoading) {
    return <LoadingState fullScreen message="Loading admin…" />;
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  if (!isAdmin) {
    return <Navigate to="/exams" replace />;
  }

  return children;
}
