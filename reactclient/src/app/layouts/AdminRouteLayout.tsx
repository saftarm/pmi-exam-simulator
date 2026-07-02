import { Outlet } from 'react-router-dom';
import { AdminRoute } from '../../features/auth';

export default function AdminRouteLayout() {
  return (
    <AdminRoute>
      <Outlet />
    </AdminRoute>
  );
}
