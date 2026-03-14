import './Landing2.css';

function Landing2({ onStart }) {
  return (
    <div className="lp-root">

      {/* ── Top utility bar ── */}
      <div className="lp-topbar">
        <span className="lp-topbar-logo">📋 CAPM Simulator</span>
        <div className="lp-topbar-links">
          <a href="#features">Features</a>
          <a href="#preview">Preview</a>
          <button className="lp-btn-register" onClick={onStart}>Start Free</button>
        </div>
      </div>

      {/* ── Nav bar ── */}
      <nav className="lp-nav">
        <ul>
          <li><a href="#about">About CAPM</a></li>
          <li><a href="#features">What's Covered</a></li>
          <li><a href="#preview">Exam Preview</a></li>
          <li><a href="#faq">FAQ</a></li>
        </ul>
      </nav>

      {/* ── Hero ── */}
      <section className="lp-hero">
        {/* Left — badge graphic */}
        <div className="lp-hero-left">
          <div className="lp-badge-circle">
            <div className="lp-badge-inner">
              <span className="lp-badge-text">CAPM®</span>
              <span className="lp-badge-sub">Exam Simulator</span>
            </div>
          </div>
        </div>

        {/* Right — copy */}
        <div className="lp-hero-right">
          <span className="lp-hero-eyebrow">Certification Practice</span>
          <h1 className="lp-hero-title">
            Certified Associate in<br />Project Management<br />(CAPM)®
          </h1>
          <p className="lp-hero-tag">No experience required</p>
          <p className="lp-hero-desc">
            Practice with exam-style questions aligned to the PMI CAPM blueprint.
            Build confidence across all five domains before your real exam day.
          </p>
          <hr className="lp-hero-divider" />
          <div className="lp-hero-pricing">
            <div className="lp-price-block">
              <span className="lp-price-label">Questions</span>
              <span className="lp-price-value">150+</span>
            </div>
            <div className="lp-price-block">
              <span className="lp-price-label">Domains</span>
              <span className="lp-price-value">5</span>
            </div>
            <button className="lp-btn-cta" onClick={onStart}>Start Now</button>
          </div>
          <p className="lp-hero-footnote">
            Free to use — no account or payment required.
          </p>
          <a href="#features" className="lp-hero-learn">
            Learn More About the Exam ›
          </a>
        </div>
      </section>

      {/* ── Features ── */}
      <section className="lp-features" id="features">
        <div className="lp-features-inner">
          <div className="lp-features-header">
            <span className="lp-section-eyebrow">What's covered</span>
            <h2 className="lp-section-title">Everything you need to prepare</h2>
          </div>
          <div className="lp-features-grid">
            {[
              { icon: '🎯', title: 'Realistic Questions',  desc: 'Mirrors actual CAPM exam format, difficulty, and domain distribution.' },
              { icon: '📊', title: 'Instant Results',      desc: 'Full score breakdown with every answer reviewed after you finish.' },
              { icon: '⚡', title: 'No Sign-Up',           desc: 'Jump straight in. No account, no payment, no friction.' },
              { icon: '🔁', title: 'Unlimited Retakes',    desc: 'Restart as many times as you need to nail your weak areas.' },
              { icon: '📱', title: 'Any Device',           desc: 'Works on desktop, tablet, and mobile without losing anything.' },
              { icon: '🌙', title: 'Easy on the Eyes',     desc: 'Dark interface built for long study sessions day or night.' },
            ].map((f) => (
              <div className="lp-feature-card" key={f.title}>
                <div className="lp-feature-icon">{f.icon}</div>
                <h3 className="lp-feature-title">{f.title}</h3>
                <p className="lp-feature-desc">{f.desc}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* ── Exam Preview ── */}
      <section className="lp-preview" id="preview">
        <div className="lp-preview-inner">
          <span className="lp-section-eyebrow">See it in action</span>
          <h2 className="lp-section-title">What the exam looks like</h2>
          <div className="lp-preview-card">
            <div className="lp-preview-topbar">
              <div className="lp-dot lp-dot-red" />
              <div className="lp-dot lp-dot-yellow" />
              <div className="lp-dot lp-dot-green" />
              <span className="lp-preview-topbar-label">CAPM Exam Simulator</span>
            </div>
            <div className="lp-preview-body">
              <div className="lp-preview-left">
                <span className="lp-qlabel">Question 5</span>
                <p className="lp-qtext">
                  A project manager is creating a work breakdown structure.
                  What is the primary purpose of this tool in project planning?
                </p>
                <div className="lp-progress-labels">
                  <span>Progress</span>
                  <span className="lp-progress-pct">35%</span>
                </div>
                <div className="lp-progress-track">
                  <div className="lp-progress-fill" style={{ width: '35%' }} />
                </div>
              </div>
              <div className="lp-preview-right">
                <span className="lp-sublabel">Select your answer</span>
                <ul className="lp-options">
                  {[
                    'To assign responsibilities to team members',
                    'To decompose deliverables into manageable components',
                    'To estimate project costs accurately',
                    'To define the project schedule baseline',
                  ].map((text, i) => (
                    <li key={i} className={`lp-option ${i === 1 ? 'lp-option-selected' : ''}`}>
                      <span className={`lp-option-letter ${i === 1 ? 'lp-option-letter-selected' : ''}`}>
                        {String.fromCharCode(65 + i)}
                      </span>
                      {text}
                    </li>
                  ))}
                </ul>
                <button className="lp-btn-next">Next Question →</button>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* ── CTA Banner ── */}
      <section className="lp-cta-banner">
        <div className="lp-cta-inner">
          <h2 className="lp-cta-title">Ready to start practicing?</h2>
          <p className="lp-cta-desc">
            Free, instant access. No account needed.
          </p>
          <button className="lp-btn-cta" onClick={onStart}>
            Launch Exam Simulator
          </button>
        </div>
      </section>

      {/* ── Footer ── */}
      <footer className="lp-footer">
        <span className="lp-footer-logo">📋 CAPM Simulator</span>
        <span className="lp-footer-note">Practice tool — not affiliated with PMI®</span>
      </footer>

    </div>
  );
}

export default Landing2;