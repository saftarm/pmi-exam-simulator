import { Link } from 'react-router-dom';
import LearnerProgressDashboard from '../../learner-progress/components/LearnerProgressDashboard';
import Icon from '../../../shared/components/Icon';

export default function LearnerHomePage() {
  return (
    <>
      <LearnerProgressDashboard showWelcome />

      <section className="mt-xl">
        <div className="bg-white rounded-xl border border-outline-variant p-lg flex flex-col sm:flex-row justify-between items-center gap-md">
          <div>
            <h2 className="font-headline-sm text-headline-sm font-bold text-primary mb-xs">
              Ready for your next session?
            </h2>
            <p className="text-sm text-on-surface-variant">
              Launch a timed simulation to practice under real exam conditions.
            </p>
          </div>
          <Link
            to="/exams"
            className="bg-secondary-container text-white px-lg py-md rounded-lg font-bold flex items-center gap-sm hover:brightness-110 shrink-0"
          >
            Browse exams
            <Icon name="arrow_forward" style={{ fontSize: 18 }} />
          </Link>
        </div>
      </section>
    </>
  );
}
