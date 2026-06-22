import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import LandingPage from './LandingPage';
import LearnerHomePage from './LearnerHomePage';

export default function HomePage() {
    const { isAuthenticated, isAdmin } = useAuth();

    if (!isAuthenticated) {
        return <LandingPage />;
    }

    if (isAdmin) {
        return <Navigate to="/admin" replace />;
    }

    return <LearnerHomePage />;
}
