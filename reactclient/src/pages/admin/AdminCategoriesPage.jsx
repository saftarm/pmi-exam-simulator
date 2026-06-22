import { Navigate, useSearchParams } from 'react-router-dom';

export default function AdminCategoriesPage() {
    const [searchParams] = useSearchParams();
    const tab = searchParams.get('tab') || 'categories';
    return <Navigate to={`/admin/exams?tab=${tab}`} replace />;
}
