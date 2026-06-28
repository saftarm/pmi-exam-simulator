import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import LandingPage from './LandingPage';
import LearnerHomePage from './LearnerHomePage';
import { LearnerPageShell } from '../layouts/LearnerLayout';
import LoadingState from '../components/loading/LoadingState';

export default function HomePage() {
  const { isAuthenticated, isAdmin, authReady, profileLoading } = useAuth();

  if (isAuthenticated && (!authReady || profileLoading)) {
    return <LoadingState fullScreen message="Loading…" />;
  }

  if (!isAuthenticated) {
    return <LandingPage />;
  }

  if (isAdmin) {
    return <Navigate to="/admin" />;
  }

  return (
    <LearnerPageShell activeLink="Home">
      <LearnerHomePage />
    </LearnerPageShell>
  );
}
