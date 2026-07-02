import type { ReactNode } from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import LoadingState from '../../../shared/components/loading/LoadingState';
import { useAuth } from '../useAuth';

interface AdminRouteProps {
  children: ReactNode;
}

export default function AdminRoute({ children }: AdminRouteProps) {
  const { isAuthenticated, isAdmin, profileLoading, authReady } = useAuth();
  const location = useLocation();

  if (!authReady || profileLoading) {
    return <LoadingState fullScreen message="Loading admin…" minHeight={undefined} />;
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  if (!isAdmin) {
    return <Navigate to="/exams" replace />;
  }

  return children;
}
