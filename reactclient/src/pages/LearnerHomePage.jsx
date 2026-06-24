import { Link } from 'react-router-dom';
import AppHeader from '../components/AppHeader';
import AppFooter from '../components/AppFooter';
import LearnerProgressDashboard from '../components/LearnerProgressDashboard';
import Icon from '../components/Icon';

export default function LearnerHomePage() {
  return (
    <div className="min-h-screen flex flex-col bg-[#F4F5F7]">
      <AppHeader activeLink="Home" />

      <main className="flex-1 max-w-container-max mx-auto px-margin-desktop py-xl w-full">
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
      </main>

      <AppFooter />
    </div>
  );
}
