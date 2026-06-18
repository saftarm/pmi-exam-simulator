import { Routes, Route, Navigate } from 'react-router-dom';
import Landing from './Landing';
import Auth from './Auth';
import ExamList from './ExamList';
import ProtectedRoute from './components/ProtectedRoute';

function App() {
    return (
        <Routes>
            <Route path="/" element={<Landing />} />
            <Route path="/login" element={<Auth />} />
            <Route
                path="/exams"
                element={
                    <ProtectedRoute>
                        <ExamList />
                    </ProtectedRoute>
                }
            />
            <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
    );
}

export default App;
