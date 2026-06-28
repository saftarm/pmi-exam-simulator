import { Outlet } from 'react-router-dom';
import AdminRoute from '../components/AdminRoute';

export default function AdminRouteLayout() {
  return (
    <AdminRoute>
      <Outlet />
    </AdminRoute>
  );
}
