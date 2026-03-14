import { useState } from 'react';
import './Auth.css';

function Auth({ onAuth }) {
  const [tab, setTab] = useState('login'); // 'login' | 'signup'

  const [loginForm, setLoginForm] = useState({ email: '', password: '' });
  const [signupForm, setSignupForm] = useState({ name: '', email: '', password: '', confirm: '' });

  const handleLogin = (e) => {
    e.preventDefault();
    // wire up your real auth logic here
    onAuth();
  };

  const handleSignup = (e) => {
    e.preventDefault();
    // wire up your real auth logic here
    onAuth();
  };

  return (
    <div className="auth-page">
      <div className="auth-orb" />

      {/* Header */}
      <div className="auth-header">
        <h1 className="header-title">📋 CAPM Certification</h1>
        <span className="header-badge">
          {tab === 'login' ? 'Welcome Back' : 'Create Account'}
        </span>
      </div>

      {/* Card */}
      <div className="auth-card">

        {/* Tabs */}
        <div className="auth-tabs">
          <button
            className={`auth-tab ${tab === 'login' ? 'auth-tab-active' : ''}`}
            onClick={() => setTab('login')}
          >
            Log In
          </button>
          <button
            className={`auth-tab ${tab === 'signup' ? 'auth-tab-active' : ''}`}
            onClick={() => setTab('signup')}
          >
            Sign Up
          </button>
        </div>

        {/* ── Login Form ── */}
        {tab === 'login' && (
          <form className="auth-form" onSubmit={handleLogin}>
            <div className="auth-field">
              <label className="auth-label">Email</label>
              <input
                className="auth-input"
                type="email"
                placeholder="you@example.com"
                value={loginForm.email}
                onChange={(e) => setLoginForm({ ...loginForm, email: e.target.value })}
                required
              />
            </div>
            <div className="auth-field">
              <div className="auth-label-row">
                <label className="auth-label">Password</label>
                <a href="#" className="auth-forgot">Forgot password?</a>
              </div>
              <input
                className="auth-input"
                type="password"
                placeholder="••••••••"
                value={loginForm.password}
                onChange={(e) => setLoginForm({ ...loginForm, password: e.target.value })}
                required
              />
            </div>
            <button type="submit" className="auth-btn-submit">
              Log In →
            </button>
            <p className="auth-switch">
              Don't have an account?{' '}
              <button type="button" className="auth-switch-link" onClick={() => setTab('signup')}>
                Sign up
              </button>
            </p>
          </form>
        )}

        {/* ── Signup Form ── */}
        {tab === 'signup' && (
          <form className="auth-form" onSubmit={handleSignup}>
            <div className="auth-field">
              <label className="auth-label">Full Name</label>
              <input
                className="auth-input"
                type="text"
                placeholder="Jane Smith"
                value={signupForm.name}
                onChange={(e) => setSignupForm({ ...signupForm, name: e.target.value })}
                required
              />
            </div>
            <div className="auth-field">
              <label className="auth-label">Email</label>
              <input
                className="auth-input"
                type="email"
                placeholder="you@example.com"
                value={signupForm.email}
                onChange={(e) => setSignupForm({ ...signupForm, email: e.target.value })}
                required
              />
            </div>
            <div className="auth-field">
              <label className="auth-label">Password</label>
              <input
                className="auth-input"
                type="password"
                placeholder="••••••••"
                value={signupForm.password}
                onChange={(e) => setSignupForm({ ...signupForm, password: e.target.value })}
                required
              />
            </div>
            <div className="auth-field">
              <label className="auth-label">Confirm Password</label>
              <input
                className="auth-input"
                type="password"
                placeholder="••••••••"
                value={signupForm.confirm}
                onChange={(e) => setSignupForm({ ...signupForm, confirm: e.target.value })}
                required
              />
            </div>
            <button type="submit" className="auth-btn-submit">
              Create Account →
            </button>
            <p className="auth-switch">
              Already have an account?{' '}
              <button type="button" className="auth-switch-link" onClick={() => setTab('login')}>
                Log in
              </button>
            </p>
          </form>
        )}

      </div>

      {/* Footer note */}
      <p className="auth-footer-note">
        Practice tool — not affiliated with PMI®
      </p>
    </div>
  );
}

export default Auth;