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
import LearnerLayout from './layouts/LearnerLayout';
import AdminRouteLayout from './layouts/AdminRouteLayout';

function App() {
  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/login" element={<AuthPage />} />

      <Route element={<LearnerLayout />}>
        <Route path="/about" element={<AboutPage />} />
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
          path="/profile"
          element={
            <ProtectedRoute>
              <ProfilePage />
            </ProtectedRoute>
          }
        />
      </Route>

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

      <Route element={<AdminRouteLayout />}>
        <Route path="/admin" element={<AdminOverviewPage />} />
        <Route path="/admin/exams" element={<AdminExamsPage />} />
        <Route path="/admin/exams/new" element={<AdminExamCreatePage />} />
        <Route path="/admin/questions/new" element={<AdminQuestionCreatePage />} />
        <Route path="/admin/questions/:questionId" element={<AdminQuestionEditPage />} />
        <Route path="/admin/questions" element={<AdminQuestionsPage />} />
        <Route path="/admin/exams/:examId/edit" element={<AdminExamEditPage />} />
        <Route path="/admin/exams/:examId" element={<AdminExamDetailPage />} />
        <Route path="/admin/categories" element={<AdminCategoriesPage />} />
        <Route path="/admin/users" element={<AdminUsersPage />} />
        <Route path="/admin/analytics" element={<AdminAnalyticsPage />} />
        <Route path="/admin/settings" element={<AdminSettingsPage />} />
      </Route>

      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}

export default App;
