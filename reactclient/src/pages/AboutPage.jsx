import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Icon from '../components/Icon';
import { resolveAuthenticatedHomePath } from '../utils/postLoginPath';

const STEPS = [
  {
    icon: 'quiz',
    title: 'Browse exams',
    text: 'Choose from published PMI certification simulations aligned with current exam domains.',
  },
  {
    icon: 'timer',
    title: 'Launch the simulator',
    text: 'Practice under timed conditions with an interface designed to mirror the real exam experience.',
  },
  {
    icon: 'query_stats',
    title: 'Review your progress',
    text: 'See domain-level scores on your home dashboard after each completed session.',
  },
];

const AUDIENCE = [
  'PMP candidates preparing for the Project Management Professional exam',
  'CAPM candidates building foundational project management knowledge',
  'PMI-ACP and agile practitioners seeking realistic practice scenarios',
];

export default function AboutPage() {
  const navigate = useNavigate();
  const { isAuthenticated, user, authReady, profileLoading } = useAuth();

  const handleCta = () => {
    if (isAuthenticated) {
      if (!authReady || profileLoading) return;
      navigate(resolveAuthenticatedHomePath(user));
    } else {
      navigate('/login', { state: { from: { pathname: '/exams' } } });
    }
  };

  return (
    <>
      <section className="hero-gradient py-20 md:py-28">
          <div className="max-w-container-max mx-auto px-margin-desktop">
            <h1 className="font-headline-xl text-headline-xl md:text-[48px] text-white mb-md max-w-3xl">
              About PMI Exam Simulator
            </h1>
            <p className="text-primary-fixed-dim/90 font-body-lg max-w-2xl">
              A dedicated platform for PMI certification exam preparation — built to help candidates
              practice with confidence before test day.
            </p>
          </div>
        </section>

        <section className="max-w-container-max mx-auto px-margin-desktop py-xl">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-xl">
            <div className="bg-white rounded-xl border border-outline-variant p-xl shadow-sm">
              <h2 className="font-headline-lg text-headline-lg text-primary mb-md">Our purpose</h2>
              <p className="text-on-surface-variant mb-md leading-relaxed">
                PMI Exam Simulator exists to close the gap between studying theory and performing
                under exam conditions. We provide realistic question pools, timed sessions, and
                actionable feedback so you know exactly where to focus next.
              </p>
            </div>

            <div className="bg-white rounded-xl border border-outline-variant p-xl shadow-sm">
              <h2 className="font-headline-lg text-headline-lg text-primary mb-md">
                What you can do
              </h2>
              <ul className="space-y-md">
                {[
                  'Take full-length timed exam simulations',
                  'Review questions organized by performance domain',
                  'Track scores and improvement over time on your home dashboard',
                  'Access explanations to deepen your understanding',
                ].map((item) => (
                  <li key={item} className="flex items-start gap-sm text-on-surface-variant">
                    <Icon
                      name="check_circle"
                      className="text-secondary-container shrink-0 mt-xs"
                      style={{ fontSize: 20 }}
                    />
                    {item}
                  </li>
                ))}
              </ul>
            </div>
          </div>
        </section>

        <section className="bg-white border-y border-outline-variant py-xl">
          <div className="max-w-container-max mx-auto px-margin-desktop">
            <h2 className="font-headline-lg text-headline-lg text-primary mb-lg text-center">
              Who it&apos;s for
            </h2>
            <ul className="grid grid-cols-1 md:grid-cols-3 gap-md max-w-4xl mx-auto">
              {AUDIENCE.map((item) => (
                <li
                  key={item}
                  className="bg-surface-container-low rounded-xl p-lg text-sm text-on-surface-variant border border-outline-variant"
                >
                  {item}
                </li>
              ))}
            </ul>
          </div>
        </section>

        <section className="max-w-container-max mx-auto px-margin-desktop py-xl">
          <h2 className="font-headline-lg text-headline-lg text-primary mb-xl text-center">
            How it works
          </h2>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-lg">
            {STEPS.map((step, index) => (
              <div
                key={step.title}
                className="bg-white rounded-xl border border-outline-variant p-xl text-center shadow-sm"
              >
                <div className="w-12 h-12 rounded-full bg-primary/10 flex items-center justify-center mx-auto mb-md">
                  <Icon name={step.icon} className="text-primary" style={{ fontSize: 28 }} />
                </div>
                <span className="text-xs font-bold uppercase tracking-wider text-secondary-container">
                  Step {index + 1}
                </span>
                <h3 className="font-headline-md text-headline-md text-primary mt-sm mb-sm">
                  {step.title}
                </h3>
                <p className="text-sm text-on-surface-variant">{step.text}</p>
              </div>
            ))}
          </div>
        </section>

        <section className="max-w-container-max mx-auto px-margin-desktop pb-xl text-center">
          <button
            type="button"
            onClick={handleCta}
            className="bg-secondary-container text-white font-label-lg px-xl py-md rounded-lg hover:brightness-110 transition-all shadow-md active:scale-95 inline-flex items-center gap-sm"
          >
            {isAuthenticated ? 'Browse exams' : 'Get started'}
            <Icon name="arrow_forward" style={{ fontSize: 20 }} />
          </button>
          {!isAuthenticated && (
            <p className="mt-md text-sm text-on-surface-variant">
              Already have an account?{' '}
              <Link to="/login" className="text-secondary-container font-bold hover:underline">
                Log in
              </Link>
            </p>
          )}
        </section>
    </>
  );
}
