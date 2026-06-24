import { useEffect, useState } from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import LoadingState from './loading/LoadingState';

function ProtectedRoute({ children }) {
  const { isAuthenticated, refreshUser } = useAuth();
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

  if (!checked) {
    return <LoadingState fullScreen message="Loading…" />;
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return children;
}

export default ProtectedRoute;
