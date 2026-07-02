import { Routes, Route, Navigate } from 'react-router-dom';
import { HomePage, AboutPage } from '../features/public-home';
import { AuthPage, ProtectedRoute } from '../features/auth';
import { ExamsDashboardPage, ExamDetailPage } from '../features/learner-exams';
import { ExamSessionPage } from '../features/exam-session';
import { ProfilePage } from '../features/profile';
import { AdminOverviewPage } from '../features/admin-dashboard';
import {
  AdminExamsPage,
  AdminExamCreatePage,
  AdminExamDetailPage,
  AdminExamEditPage,
} from '../features/admin-exams';
import { AdminCategoriesPage } from '../features/admin-categories';
import { AdminUsersPage } from '../features/admin-users';
import { AdminAnalyticsPage } from '../features/admin-analytics';
import { AdminSettingsPage } from '../features/admin-settings';
import {
  AdminQuestionsPage,
  AdminQuestionCreatePage,
  AdminQuestionEditPage,
} from '../features/admin-questions';
import LearnerLayout from './layouts/LearnerLayout';
import AdminRouteLayout from './layouts/AdminRouteLayout';

export default function AppRoutes() {
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
