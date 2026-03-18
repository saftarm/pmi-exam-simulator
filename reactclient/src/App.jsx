import { Routes, Route, Navigate } from 'react-router-dom';
import Landing from './Landing';
import Auth from './Auth';
import Exam from './Exam';
import ExamList from './ExamList';
import ExamDetails from './ExamDetails';

function App() {
  return (
    <Routes>
      <Route path="/"      element={<Landing />} />
      <Route path="/login" element={<Auth />} />
      <Route path="/exam"  element={<Exam />} />
          <Route path="/exams" element={<ExamList />} />
          <Route path="/examDetails/:id" element={<ExamDetails />} />


    </Routes>
  );
}

export default App;