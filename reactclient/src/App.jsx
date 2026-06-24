import { Routes, Route, Navigate } from 'react-router-dom';
import HomePage from './pages/HomePage';
import AboutPage from './pages/AboutPage';
import AuthPage from './pages/AuthPage';
import ExamsDashboardPage from './pages/ExamsDashboardPage';
import ExamSessionPage from './pages/ExamSessionPage';
import ExamDetailPage from './pages/ExamDetailPage';
import ProfilePage from './pages/ProfilePage';
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
import AdminQuestionCreatePage from './pages/admin/AdminQuestionCreatePage';
import AdminQuestionsPage from './pages/admin/AdminQuestionsPage';
import ProtectedRoute from './components/ProtectedRoute';
import AdminRoute from './components/AdminRoute';

function AdminRoutes({ children }) {
  return <AdminRoute>{children}</AdminRoute>;
}

function App() {
  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/about" element={<AboutPage />} />
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
        path="/exams/:examId"
        element={
          <ProtectedRoute>
            <ExamDetailPage />
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
        path="/progress"
        element={
          <ProtectedRoute>
            <Navigate to="/" replace />
          </ProtectedRoute>
        }
      />
      <Route
        path="/profile"
        element={
          <ProtectedRoute>
            <ProfilePage />
          </ProtectedRoute>
        }
      />
      <Route
        path="/admin"
        element={
          <AdminRoutes>
            <AdminOverviewPage />
          </AdminRoutes>
        }
      />
      <Route
        path="/admin/exams"
        element={
          <AdminRoutes>
            <AdminExamsPage />
          </AdminRoutes>
        }
      />
      <Route
        path="/admin/exams/new"
        element={
          <AdminRoutes>
            <AdminExamCreatePage />
          </AdminRoutes>
        }
      />
      <Route
        path="/admin/questions/new"
        element={
          <AdminRoutes>
            <AdminQuestionCreatePage />
          </AdminRoutes>
        }
      />
      <Route
        path="/admin/questions/:questionId"
        element={
          <AdminRoutes>
            <AdminQuestionEditPage />
          </AdminRoutes>
        }
      />
      <Route
        path="/admin/questions"
        element={
          <AdminRoutes>
            <AdminQuestionsPage />
          </AdminRoutes>
        }
      />
      <Route
        path="/admin/exams/:examId/edit"
        element={
          <AdminRoutes>
            <AdminExamEditPage />
          </AdminRoutes>
        }
      />
      <Route
        path="/admin/exams/:examId"
        element={
          <AdminRoutes>
            <AdminExamDetailPage />
          </AdminRoutes>
        }
      />
      <Route
        path="/admin/categories"
        element={
          <AdminRoutes>
            <AdminCategoriesPage />
          </AdminRoutes>
        }
      />
      <Route
        path="/admin/users"
        element={
          <AdminRoutes>
            <AdminUsersPage />
          </AdminRoutes>
        }
      />
      <Route
        path="/admin/analytics"
        element={
          <AdminRoutes>
            <AdminAnalyticsPage />
          </AdminRoutes>
        }
      />
      <Route
        path="/admin/settings"
        element={
          <AdminRoutes>
            <AdminSettingsPage />
          </AdminRoutes>
        }
      />
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}

export default App;
