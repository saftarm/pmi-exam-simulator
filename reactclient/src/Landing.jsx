import { useNavigate } from 'react-router-dom';
import { useAuth } from './context/AuthContext';
import './Landing.css';

function Landing() {
    const navigate = useNavigate();
    const { isAuthenticated, logout } = useAuth();

    const handleBrowseExams = () => {
        if (isAuthenticated) navigate('/exams');
        else navigate('/login', { state: { from: { pathname: '/exams' } } });
    };

    return (
        <div className="landing-page">
            <div className="landing-orb" />

            <nav className="landing-nav">
                <div className="nav-logo">
                    <div className="nav-logo-dot" />
                    📋 PMI Exam Simulator
                </div>
                <div className="landing-nav-actions">
                    <span className="header-badge">Free Practice</span>
                    {isAuthenticated ? (
                        <>
                            <button className="btn-nav" onClick={() => navigate('/exams')}>
                                My Exams
                            </button>
                            <button className="btn-nav" onClick={() => { logout(); }}>
                                Log Out
                            </button>
                        </>
                    ) : (
                        <button className="btn-nav" onClick={() => navigate('/login')}>
                            Log In
                        </button>
                    )}
                </div>
            </nav>

            <section className="landing-hero">
                <div className="hero-eyebrow">
                    <span>✦</span> PMI Exam Simulator
                </div>
                <h1 className="hero-title">
                    Prepare for your <span className="hero-accent">PMI exam</span><br />
                    with confidence
                </h1>
                <p className="hero-sub">
                    Practice with realistic PMI-style questions, track your progress,
                    and identify weak spots before exam day.
                </p>
                <div className="hero-actions">
                    <button className="btn-primary" onClick={handleBrowseExams}>
                        Browse Exams →
                    </button>
                    {!isAuthenticated && (
                        <button className="btn-secondary" onClick={() => navigate('/login')}>
                            Log In
                        </button>
                    )}
                    <a href="#features" className="btn-secondary">
                        See how it works
                    </a>
                </div>
                <div className="stats-row">
                    {[
                        { num: '150+', lbl: 'Questions' },
                        { num: '5', lbl: 'Domains' },
                        { num: 'PMI', lbl: 'Aligned' },
                        { num: '100%', lbl: 'Free' },
                    ].map((s) => (
                        <div className="stat-item" key={s.lbl}>
                            <span className="stat-num">{s.num}</span>
                            <span className="stat-lbl">{s.lbl}</span>
                        </div>
                    ))}
                </div>
            </section>

            <section className="landing-section" id="features">
                <span className="section-label">Why use this</span>
                <h2 className="section-title">
                    Everything you need<br />to prepare
                </h2>
                <div className="features-grid">
                    {[
                        { icon: '🎯', title: 'Realistic Questions', desc: 'Questions written to mirror PMI exam format, difficulty, and domain coverage.' },
                        { icon: '📊', title: 'Instant Score Report', desc: 'See your score, correct vs incorrect answers, and every choice you made — right after finishing.' },
                        { icon: '🔁', title: 'Unlimited Retakes', desc: 'Restart exams as many times as you want to build confidence and reinforce weak areas.' },
                        { icon: '🌙', title: 'Dark Mode First', desc: 'Designed for long study sessions — easy on the eyes whether it\'s 9am or 2am.' },
                    ].map((f) => (
                        <div className="feature-card" key={f.title}>
                            <div className="feature-icon">{f.icon}</div>
                            <div className="feature-title">{f.title}</div>
                            <div className="feature-desc">{f.desc}</div>
                        </div>
                    ))}
                </div>
            </section>

            <section className="landing-cta">
                <div className="cta-card">
                    <h2 className="cta-title">Ready to start practicing?</h2>
                    <p className="cta-sub">
                        Create a free account and access the full practice library.
                        See where you stand and what to focus on.
                    </p>
                    <button className="btn-primary" onClick={handleBrowseExams}>
                        Get Started →
                    </button>
                </div>
            </section>

            <footer className="landing-footer">
                <span className="footer-logo">📋 PMI Exam Simulator</span>
                <span className="footer-note">Practice tool — not affiliated with PMI®</span>
            </footer>
        </div>
    );
}

export default Landing;
