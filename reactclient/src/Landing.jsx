import { useNavigate } from 'react-router-dom';
import './Landing.css';

function Landing({ onStart, onLogin }) {

  const navigate = useNavigate();
  return (
    <div className="landing-page">

      {/* Glow orb */}
      <div className="landing-orb" />

      {/* NAV */}
      <nav className="landing-nav">
        <div className="nav-logo">
          <div className="nav-logo-dot" />
          📋 Capm Simulator
        </div>
        <span className="header-badge">Free Practice</span>
      </nav>
      {/* HERO */}

      <section className="landing-hero">
        <div className="hero-eyebrow">
          <span>✦</span> Exam Simulator
        </div>
        <h1 className="hero-title">
          Pass your <span className="hero-accent">CAPM</span><br />
          on the first try
        </h1>
        <p className="hero-sub">
          Practice with real-style questions, track your progress, and identify
          weak spots before exam day.
        </p>
        <div className="hero-actions">
         

          <button className="btn-primary" onClick={() => navigate('/exams')}>
            View Certifications →
          </button>

          <button className="btn-secondary" onClick={() => navigate('/login')}>
          Log In
                  </button>

               
          <a href="#features" className="btn-secondary">
            See how it works
          </a>
        </div>
        <div className="stats-row">
          {[
            { num: '150+', lbl: 'Questions' },
            { num: '5',    lbl: 'Domains'   },
            { num: 'PMI',  lbl: 'Aligned'   },
            { num: '100%', lbl: 'Free'       },
          ].map((s) => (
            <div className="stat-item" key={s.lbl}>
              <span className="stat-num">{s.num}</span>
              <span className="stat-lbl">{s.lbl}</span>
            </div>
          ))}
        </div>
      </section>

      {/* FEATURES */}
      <section className="landing-section" id="features">
        <span className="section-label">Why use this</span>
        <h2 className="section-title">
          Everything you need<br />to prepare
        </h2>
        <div className="features-grid">
          {[
            { icon: '🎯', title: 'Realistic Questions',  desc: 'Questions written to mirror the actual CAPM exam format, difficulty, and domain coverage.' },
            { icon: '📊', title: 'Instant Score Report', desc: 'See your score, correct vs incorrect answers, and every choice you made — right after finishing.' },
            { icon: '⚡', title: 'No Account Needed',    desc: 'Jump straight into practice. No sign-up, no paywalls, no distractions.' },
            { icon: '🔁', title: 'Unlimited Retakes',    desc: 'Restart the exam as many times as you want to build confidence and reinforce weak areas.' },
            { icon: '🌙', title: 'Dark Mode First',      desc: 'Designed for long study sessions — easy on the eyes whether it\'s 9am or 2am.' },
            { icon: '📱', title: 'Works Everywhere',     desc: 'Fully responsive — study on your laptop, tablet, or phone without losing functionality.' },
          ].map((f) => (
            <div className="feature-card" key={f.title}>
              <div className="feature-icon">{f.icon}</div>
              <div className="feature-title">{f.title}</div>
              <div className="feature-desc">{f.desc}</div>
            </div>
          ))}
        </div>
      </section>

      {/* EXAM PREVIEW */}
      <section className="landing-section">
        <span className="section-label">Preview</span>
        <h2 className="section-title" style={{ marginBottom: '32px' }}>
          What the exam looks like
        </h2>
        <div className="preview-wrapper">
          <div className="preview-topbar">
            <div className="dot dot-red" />
            <div className="dot dot-yellow" />
            <div className="dot dot-green" />
            <span className="preview-topbar-label">CAPM Exam Simulator</span>
          </div>
          <div className="preview-content">
            <div className="preview-left">
              <span className="panel-label">Question 5</span>
              <p className="preview-question">
                A project manager is creating a work breakdown structure. What is
                the primary purpose of this tool in project planning?
              </p>
              <div className="progress-bar-labels">
                <span className="progress-bar-label">Progress</span>
                <span className="progress-bar-percent">35%</span>
              </div>
              <div className="progress-bar-track">
                <div className="progress-bar-fill" style={{ width: '35%' }} />
              </div>
            </div>
            <div className="preview-right">
              <span className="panel-sublabel">Select your answer</span>
              <ul className="options-list">
                {[
                  'To assign responsibilities to team members',
                  'To decompose deliverables into manageable components',
                  'To estimate project costs accurately',
                  'To define the project schedule baseline',
                ].map((text, i) => (
                  <li key={i}>
                    <div className={`option-button ${i === 1 ? 'selected' : 'default'}`}>
                      <span className={`option-letter ${i === 1 ? 'selected' : 'default'}`}>
                        {String.fromCharCode(65 + i)}
                      </span>
                      {text}
                    </div>
                  </li>
                ))}
              </ul>
              <button className="next-button active" style={{ marginTop: '24px' }}>
                Next Question →
              </button>
            </div>
          </div>
        </div>
      </section>

      {/* CTA */}
      <section className="landing-cta">
        <div className="cta-card">
          <h2 className="cta-title">Ready to start practicing?</h2>
          <p className="cta-sub">
            Take a full practice exam right now — no sign-up required. See where
            you stand and what to focus on.
          </p>
          <button className="btn-primary" onClick={onStart}>
            Launch Exam Simulator →
          </button>
        </div>
      </section>

      {/* FOOTER */}
      <footer className="landing-footer">
        <span className="footer-logo">📋 CAPM Simulator</span>
        <span className="footer-note">Practice tool — not affiliated with PMI®</span>
      </footer>

    </div>
  );
}

export default Landing;