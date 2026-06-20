import { Routes, Route, Navigate } from 'react-router-dom';
import LandingPage from './pages/LandingPage';
import AuthPage from './pages/AuthPage';
import ExamsDashboardPage from './pages/ExamsDashboardPage';
import ExamSessionPage from './pages/ExamSessionPage';
import AdminOverviewPage from './pages/admin/AdminOverviewPage';
import AdminExamsPage from './pages/admin/AdminExamsPage';
import AdminExamCreatePage from './pages/admin/AdminExamCreatePage';
import AdminExamDetailPage from './pages/admin/AdminExamDetailPage';
import AdminExamEditPage from './pages/admin/AdminExamEditPage';
import AdminCategoriesPage from './pages/admin/AdminCategoriesPage';
import AdminUsersPage from './pages/admin/AdminUsersPage';
import AdminAnalyticsPage from './pages/admin/AdminAnalyticsPage';
import AdminSettingsPage from './pages/admin/AdminSettingsPage';
import AdminQuestionEditPage from './pages/admin/AdminQuestionEditPage';
import ProtectedRoute from './components/ProtectedRoute';

function App() {
    return (
        <Routes>
            <Route path="/" element={<LandingPage />} />
            <Route path="/login" element={<AuthPage />} />
            <Route
                path="/exams"
                element={
                    <ProtectedRoute>
                        <ExamsDashboardPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/exams/:examId/session/:sessionId"
                element={
                    <ProtectedRoute>
                        <ExamSessionPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/admin"
                element={
                    <ProtectedRoute>
                        <AdminOverviewPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/admin/exams"
                element={
                    <ProtectedRoute>
                        <AdminExamsPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/admin/exams/new"
                element={
                    <ProtectedRoute>
                        <AdminExamCreatePage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/admin/exams/:examId"
                element={
                    <ProtectedRoute>
                        <AdminExamDetailPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/admin/exams/:examId/edit"
                element={
                    <ProtectedRoute>
                        <AdminExamEditPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/admin/categories"
                element={
                    <ProtectedRoute>
                        <AdminCategoriesPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/admin/users"
                element={
                    <ProtectedRoute>
                        <AdminUsersPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/admin/analytics"
                element={
                    <ProtectedRoute>
                        <AdminAnalyticsPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/admin/settings"
                element={
                    <ProtectedRoute>
                        <AdminSettingsPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/admin/questions/:questionId"
                element={
                    <ProtectedRoute>
                        <AdminQuestionEditPage />
                    </ProtectedRoute>
                }
            />
            <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
    );
}

export default App;
