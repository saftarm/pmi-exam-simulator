import { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from './context/AuthContext';
import { formatApiErrors } from './services/authService';
import './Auth.css';

function Auth() {
    const navigate = useNavigate();
    const location = useLocation();
    const { login, register, isAuthenticated } = useAuth();

    const [tab, setTab] = useState('login');
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);
    const [successMsg, setSuccessMsg] = useState(null);

    const [loginForm, setLoginForm] = useState({ userName: '', password: '' });
    const [signupForm, setSignupForm] = useState({
        userName: '',
        firstName: '',
        displayName: '',
        email: '',
        password: '',
        confirm: '',
    });

    useEffect(() => {
        if (isAuthenticated) {
            const from = location.state?.from?.pathname || '/exams';
            navigate(from, { replace: true });
        }
    }, [isAuthenticated, location.state, navigate]);

    const handleLogin = async (e) => {
        e.preventDefault();
        setError(null);
        setLoading(true);
        try {
            await login(loginForm.userName, loginForm.password);
            const from = location.state?.from?.pathname || '/exams';
            navigate(from, { replace: true });
        } catch (err) {
            setError(formatApiErrors(err));
        } finally {
            setLoading(false);
        }
    };

    const handleSignup = async (e) => {
        e.preventDefault();
        setError(null);
        setSuccessMsg(null);

        if (signupForm.password !== signupForm.confirm) {
            setError('Passwords do not match.');
            return;
        }

        setLoading(true);
        try {
            await register({
                userName: signupForm.userName,
                password: signupForm.password,
                firstName: signupForm.firstName,
                displayName: signupForm.displayName,
                email: signupForm.email,
            });
            setSuccessMsg('Account created! You can log in now.');
            setTab('login');
            setLoginForm({ userName: signupForm.userName, password: '' });
        } catch (err) {
            setError(formatApiErrors(err));
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="auth-page">
            <div className="auth-orb" />

            <div className="auth-header">
                <h1 className="header-title">📋 PMI Exam Simulator</h1>
                <span className="header-badge">
                    {tab === 'login' ? 'Welcome Back' : 'Create Account'}
                </span>
            </div>

            <div className="auth-card">
                <div className="auth-tabs">
                    <button
                        className={`auth-tab ${tab === 'login' ? 'auth-tab-active' : ''}`}
                        onClick={() => { setTab('login'); setError(null); setSuccessMsg(null); }}
                    >
                        Log In
                    </button>
                    <button
                        className={`auth-tab ${tab === 'signup' ? 'auth-tab-active' : ''}`}
                        onClick={() => { setTab('signup'); setError(null); setSuccessMsg(null); }}
                    >
                        Sign Up
                    </button>
                </div>

                {error && <div className="auth-error">{error}</div>}
                {successMsg && <div className="auth-success">{successMsg}</div>}

                {tab === 'login' && (
                    <form className="auth-form" onSubmit={handleLogin}>
                        <div className="auth-field">
                            <label className="auth-label">Username</label>
                            <input
                                className="auth-input"
                                type="text"
                                placeholder="your_username"
                                value={loginForm.userName}
                                onChange={(e) => setLoginForm({ ...loginForm, userName: e.target.value })}
                                required
                            />
                        </div>
                        <div className="auth-field">
                            <div className="auth-label-row">
                                <label className="auth-label">Password</label>
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
                        <button type="submit" className="auth-btn-submit" disabled={loading}>
                            {loading ? 'Logging in...' : 'Log In →'}
                        </button>
                        <p className="auth-switch">
                            Don't have an account?{' '}
                            <button type="button" className="auth-switch-link" onClick={() => setTab('signup')}>
                                Sign up
                            </button>
                        </p>
                    </form>
                )}

                {tab === 'signup' && (
                    <form className="auth-form" onSubmit={handleSignup}>
                        <div className="auth-field">
                            <label className="auth-label">Username</label>
                            <input
                                className="auth-input"
                                type="text"
                                placeholder="your_username"
                                value={signupForm.userName}
                                onChange={(e) => setSignupForm({ ...signupForm, userName: e.target.value })}
                                required
                            />
                        </div>
                        <div className="auth-field">
                            <label className="auth-label">First Name</label>
                            <input
                                className="auth-input"
                                type="text"
                                placeholder="Jane"
                                value={signupForm.firstName}
                                onChange={(e) => setSignupForm({ ...signupForm, firstName: e.target.value })}
                                required
                            />
                        </div>
                        <div className="auth-field">
                            <label className="auth-label">Display Name</label>
                            <input
                                className="auth-input"
                                type="text"
                                placeholder="Jane Smith"
                                value={signupForm.displayName}
                                onChange={(e) => setSignupForm({ ...signupForm, displayName: e.target.value })}
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
                        <button type="submit" className="auth-btn-submit" disabled={loading}>
                            {loading ? 'Creating account...' : 'Create Account →'}
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

            <button className="auth-back-link" onClick={() => navigate('/')}>
                ← Back to home
            </button>

            <p className="auth-footer-note">
                Practice tool — not affiliated with PMI®
            </p>
        </div>
    );
}

export default Auth;
