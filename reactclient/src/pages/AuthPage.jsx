import { useState, useEffect } from 'react';
import { useNavigate, useLocation, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { formatApiErrors } from '../services/authService';
import { AuthFooter } from '../components/AppFooter';
import Icon from '../components/Icon';

export default function AuthPage() {
    const navigate = useNavigate();
    const location = useLocation();
    const { login, register, isAuthenticated } = useAuth();

    const [tab, setTab] = useState('signin');
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);
    const [successMsg, setSuccessMsg] = useState(null);
    const [showPassword, setShowPassword] = useState(false);

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
            setTab('signin');
            setLoginForm({ userName: signupForm.userName, password: '' });
        } catch (err) {
            setError(formatApiErrors(err));
        } finally {
            setLoading(false);
        }
    };

    const isSignIn = tab === 'signin';

    return (
        <div className="min-h-screen flex flex-col font-body-md text-on-surface selection:bg-secondary-fixed bg-[#F4F5F7]">
            <header className="bg-surface dark:bg-surface-container-low border-b border-outline-variant dark:border-outline w-full h-16 fixed top-0 z-50">
                <nav className="flex justify-between items-center px-margin-desktop max-w-container-max mx-auto h-full">
                    <Link to="/" className="font-headline-md text-headline-md font-bold text-primary dark:text-primary-fixed tracking-tight">
                        PMI Exam Simulator
                    </Link>
                    <div className="hidden md:flex items-center gap-lg">
                        <Link to="/" className="font-body-md text-body-md text-on-surface-variant hover:text-primary transition-all duration-200">Home</Link>
                        <Link to="/exams" className="font-body-md text-body-md text-on-surface-variant hover:text-primary transition-all duration-200">Exams</Link>
                    </div>
                    <div className="flex items-center gap-md">
                        <button type="button" onClick={() => setTab('signin')} className="font-label-lg text-label-lg px-md py-sm rounded-lg text-primary hover:bg-surface-container-low transition-colors">
                            Log In
                        </button>
                        <button type="button" onClick={() => setTab('signup')} className="bg-secondary text-on-secondary font-label-lg text-label-lg px-lg py-sm rounded-lg transition-all duration-200 active:scale-95 shadow-sm hover:brightness-110">
                            Get Started
                        </button>
                    </div>
                </nav>
            </header>

            <main className="flex-grow flex items-center justify-center pt-16 px-margin-mobile">
                <div className="w-full max-w-[440px] bg-white rounded-xl border border-outline-variant shadow-sm overflow-hidden p-xl flex flex-col gap-xl">
                    <div className="text-center">
                        <h1 className="font-headline-lg text-headline-lg text-primary mb-xs">Welcome back</h1>
                        <p className="font-body-md text-body-md text-on-surface-variant">Your path to PMP certification starts here.</p>
                    </div>

                    <div className="relative bg-surface-container p-xs rounded-full flex items-center w-full">
                        <div
                            className="absolute h-[calc(100%-8px)] w-[calc(50%-4px)] bg-white rounded-full shadow-sm transition-transform duration-300"
                            style={{ transform: isSignIn ? 'translateX(0)' : 'translateX(calc(100% + 4px))' }}
                        />
                        <button
                            type="button"
                            onClick={() => { setTab('signin'); setError(null); setSuccessMsg(null); }}
                            className={`relative z-10 flex-1 py-sm font-label-lg text-label-lg text-center transition-colors ${isSignIn ? 'text-primary' : 'text-on-surface-variant'}`}
                        >
                            Sign In
                        </button>
                        <button
                            type="button"
                            onClick={() => { setTab('signup'); setError(null); setSuccessMsg(null); }}
                            className={`relative z-10 flex-1 py-sm font-label-lg text-label-lg text-center transition-colors ${!isSignIn ? 'text-primary' : 'text-on-surface-variant'}`}
                        >
                            Create Account
                        </button>
                    </div>

                    {error && (
                        <div className="bg-error-container text-on-error-container px-md py-sm rounded-lg font-label-sm">{error}</div>
                    )}
                    {successMsg && (
                        <div className="bg-primary-fixed text-primary px-md py-sm rounded-lg font-label-sm">{successMsg}</div>
                    )}

                    {isSignIn ? (
                        <form className="flex flex-col gap-lg" onSubmit={handleLogin}>
                            <div className="flex flex-col gap-xs">
                                <label className="font-label-lg text-label-lg text-primary" htmlFor="username">Username</label>
                                <div className="relative group">
                                    <Icon name="person" className="absolute left-md top-1/2 -translate-y-1/2 text-outline text-[20px] group-focus-within:text-primary transition-colors" />
                                    <input
                                        id="username"
                                        className="w-full pl-[48px] pr-md py-md bg-white border border-outline-variant rounded-lg font-body-md text-body-md focus:ring-0 focus:border-primary transition-all outline-none"
                                        placeholder="your_username"
                                        value={loginForm.userName}
                                        onChange={(e) => setLoginForm({ ...loginForm, userName: e.target.value })}
                                        required
                                    />
                                </div>
                            </div>
                            <div className="flex flex-col gap-xs">
                                <div className="flex justify-between items-center">
                                    <label className="font-label-lg text-label-lg text-primary" htmlFor="password">Password</label>
                                </div>
                                <div className="relative group">
                                    <Icon name="lock" className="absolute left-md top-1/2 -translate-y-1/2 text-outline text-[20px] group-focus-within:text-primary transition-colors" />
                                    <input
                                        id="password"
                                        type={showPassword ? 'text' : 'password'}
                                        className="w-full pl-[48px] pr-md py-md bg-white border border-outline-variant rounded-lg font-body-md text-body-md focus:ring-0 focus:border-primary transition-all outline-none"
                                        placeholder="••••••••"
                                        value={loginForm.password}
                                        onChange={(e) => setLoginForm({ ...loginForm, password: e.target.value })}
                                        required
                                    />
                                    <button type="button" onClick={() => setShowPassword(!showPassword)} className="absolute right-md top-1/2 -translate-y-1/2 text-outline hover:text-on-surface">
                                        <Icon name={showPassword ? 'visibility_off' : 'visibility'} style={{ fontSize: 20 }} />
                                    </button>
                                </div>
                            </div>
                            <button type="submit" disabled={loading} className="w-full bg-secondary-container hover:bg-secondary text-on-secondary font-label-lg text-label-lg py-md rounded-lg shadow-sm transition-all duration-200 active:scale-[0.98] mt-md disabled:opacity-60">
                                {loading ? 'Signing in...' : 'Sign In to Simulator'}
                            </button>
                        </form>
                    ) : (
                        <form className="flex flex-col gap-lg" onSubmit={handleSignup}>
                            {[
                                { id: 'su-username', label: 'Username', key: 'userName', icon: 'person', type: 'text' },
                                { id: 'su-first', label: 'First Name', key: 'firstName', icon: 'badge', type: 'text' },
                                { id: 'su-display', label: 'Display Name', key: 'displayName', icon: 'account_circle', type: 'text' },
                                { id: 'su-email', label: 'Email Address', key: 'email', icon: 'mail', type: 'email' },
                            ].map((field) => (
                                <div key={field.key} className="flex flex-col gap-xs">
                                    <label className="font-label-lg text-label-lg text-primary" htmlFor={field.id}>{field.label}</label>
                                    <div className="relative group">
                                        <Icon name={field.icon} className="absolute left-md top-1/2 -translate-y-1/2 text-outline text-[20px] group-focus-within:text-primary transition-colors" />
                                        <input
                                            id={field.id}
                                            type={field.type}
                                            className="w-full pl-[48px] pr-md py-md bg-white border border-outline-variant rounded-lg font-body-md text-body-md focus:ring-0 focus:border-primary transition-all outline-none"
                                            value={signupForm[field.key]}
                                            onChange={(e) => setSignupForm({ ...signupForm, [field.key]: e.target.value })}
                                            required
                                        />
                                    </div>
                                </div>
                            ))}
                            <div className="flex flex-col gap-xs">
                                <label className="font-label-lg text-label-lg text-primary" htmlFor="su-password">Password</label>
                                <div className="relative group">
                                    <Icon name="lock" className="absolute left-md top-1/2 -translate-y-1/2 text-outline text-[20px]" />
                                    <input id="su-password" type="password" className="w-full pl-[48px] pr-md py-md bg-white border border-outline-variant rounded-lg font-body-md focus:border-primary outline-none" value={signupForm.password} onChange={(e) => setSignupForm({ ...signupForm, password: e.target.value })} required />
                                </div>
                            </div>
                            <div className="flex flex-col gap-xs">
                                <label className="font-label-lg text-label-lg text-primary" htmlFor="su-confirm">Confirm Password</label>
                                <div className="relative group">
                                    <Icon name="verified_user" className="absolute left-md top-1/2 -translate-y-1/2 text-outline text-[20px]" />
                                    <input id="su-confirm" type="password" className="w-full pl-[48px] pr-md py-md bg-white border border-outline-variant rounded-lg font-body-md focus:border-primary outline-none" value={signupForm.confirm} onChange={(e) => setSignupForm({ ...signupForm, confirm: e.target.value })} required />
                                </div>
                            </div>
                            <button type="submit" disabled={loading} className="w-full bg-secondary-container hover:bg-secondary text-on-secondary font-label-lg text-label-lg py-md rounded-lg shadow-sm transition-all duration-200 active:scale-[0.98] mt-md disabled:opacity-60">
                                {loading ? 'Creating account...' : 'Create Free Account'}
                            </button>
                        </form>
                    )}
                </div>
            </main>

            <AuthFooter />
        </div>
    );
}
