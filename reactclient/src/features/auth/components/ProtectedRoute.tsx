import type { ReactNode } from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import LoadingState from '../../../shared/components/loading/LoadingState';
import { useAuth } from '../useAuth';

interface ProtectedRouteProps {
  children: ReactNode;
}

export default function ProtectedRoute({ children }: ProtectedRouteProps) {
  const { isAuthenticated, authReady, profileLoading } = useAuth();
  const location = useLocation();

  if (!authReady || profileLoading) {
    return <LoadingState fullScreen message="Loading…" minHeight={undefined} />;
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return children;
}
