import { useNavigate, useParams, useLocation } from 'react-router-dom';
import { useAuth } from '../../auth';
import { useExamSession } from '../hooks/useExamSession';
import SessionResultsView from '../components/session/SessionResultsView';
import SessionQuestionView from '../components/session/SessionQuestionView';
import { ExamSessionSkeleton } from '../../../shared/components/loading';

export default function ExamSessionPage() {
  const { sessionId = '', examId = '' } = useParams();
  const navigate = useNavigate();
  const location = useLocation();
  const { user } = useAuth();
  const examMeta = location.state?.exam;

  const session = useExamSession({
    sessionId,
    examId,
    examMeta,
    userId: user?.userId,
  });

  if (session.loading) {
    return <ExamSessionSkeleton />;
  }

  if (session.error) {
    return (
      <div className="h-screen flex flex-col items-center justify-center gap-md bg-background px-md text-center">
        <p className="text-error font-body-lg">{session.error}</p>
        <div className="flex flex-wrap gap-md justify-center">
          {examId && (
            <button
              type="button"
              onClick={() => navigate(`/exams/${examId}`)}
              className="text-secondary font-label-lg hover:underline"
            >
              Back to exam
            </button>
          )}
          <button
            type="button"
            onClick={() => navigate('/exams')}
            className="text-secondary font-label-lg hover:underline"
          >
            Back to Exams
          </button>
        </div>
      </div>
    );
  }

  if (session.finished && session.result) {
    return (
      <SessionResultsView
        result={session.result}
        domainResults={session.domainResults}
        answers={session.answers}
        total={session.questions.length}
      />
    );
  }

  if (!session.questions[session.currentIndex]) {
    return (
      <div className="h-screen flex items-center justify-center bg-background">
        <p className="text-on-surface-variant">No questions in this session.</p>
      </div>
    );
  }

  return (
    <SessionQuestionView
      examTitle={session.examTitle}
      durationMinutes={session.durationMinutes}
      questions={session.questions}
      currentIndex={session.currentIndex}
      answers={session.answers}
      flagged={session.flagged}
      submitting={session.submitting}
      submitError={session.submitError}
      onSelectIndex={session.setCurrentIndex}
      onSelectOption={session.handleSelectOption}
      onToggleFlag={session.toggleFlag}
      onSubmit={session.handleSubmit}
      onExpire={session.handleExpire}
    />
  );
}
