import { Link, useNavigate } from 'react-router-dom';
import AppFooter from '../AppFooter';
import Icon from '../Icon';
import { APP_NAME } from '../../constants/branding';
import { countAnswered } from '../../utils/sessionAnswers';

export default function SessionResultsView({
  result,
  domainResults,
  answers,
  total,
  onBackToExams,
}) {
  const navigate = useNavigate();

  return (
    <div className="min-h-screen bg-[#F4F5F7] flex flex-col">
      <header className="bg-primary-container text-white h-16 flex items-center px-margin-desktop">
        <span className="font-headline-md text-headline-md font-bold">{APP_NAME}</span>
      </header>
      <main className="flex-1 flex items-center justify-center p-xl">
        <div className="max-w-lg w-full bg-white border border-outline-variant rounded-xl p-xl shadow-sm text-center">
          <Icon name="celebration" className="text-secondary mb-md" style={{ fontSize: 48 }} />
          <h1 className="font-headline-lg text-headline-lg text-primary mb-sm">Exam Complete</h1>
          <p className="font-body-lg text-on-surface-variant mb-xl">
            You answered {countAnswered(answers)} of {total} questions.
          </p>
          <div className="grid grid-cols-2 gap-md mb-xl">
            <div className="bg-surface-container-low p-md rounded-lg">
              <span className="font-headline-xl text-headline-xl text-primary">
                {result.scorePoints?.toFixed?.(1) ?? result.scorePoints}
              </span>
              <p className="font-label-sm text-on-surface-variant">Points</p>
            </div>
            <div className="bg-surface-container-low p-md rounded-lg">
              <span className="font-headline-xl text-headline-xl text-secondary">
                {result.percentageScore?.toFixed?.(1) ?? result.percentageScore}%
              </span>
              <p className="font-label-sm text-on-surface-variant">Score</p>
            </div>
          </div>

          {domainResults.length > 0 && (
            <div className="mb-xl text-left">
              <h2 className="font-headline-sm text-headline-sm font-bold mb-md text-center">
                Performance by domain
              </h2>
              <p className="text-sm text-on-surface-variant text-center mb-md">
                Domain scores count a question as correct only when all choices are right.
              </p>
              <ul className="space-y-sm text-sm">
                {domainResults.map((row) => (
                  <li
                    key={row.domainId}
                    className="flex justify-between items-center border border-outline-variant rounded-lg px-md py-sm"
                  >
                    <span className="font-medium">{row.domainTitle}</span>
                    <span className="font-bold text-primary">
                      {Number(row.percentageScore).toFixed(1)}%
                    </span>
                  </li>
                ))}
              </ul>
            </div>
          )}

          <div className="space-y-sm">
            <button
              type="button"
              onClick={onBackToExams || (() => navigate('/exams'))}
              className="w-full bg-secondary-container text-on-secondary font-label-lg py-md rounded-lg hover:brightness-110 transition-all"
            >
              Back to Exams
            </button>
            <Link
              to="/"
              className="block w-full border border-outline-variant text-primary font-label-lg py-md rounded-lg hover:bg-surface-container-low transition-all"
            >
              View full progress
            </Link>
          </div>
        </div>
      </main>
      <AppFooter compact />
    </div>
  );
}
