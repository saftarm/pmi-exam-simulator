import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import AppHeader from '../components/AppHeader';
import AppFooter from '../components/AppFooter';
import Icon from '../components/Icon';

const FEATURES = [
    {
        icon: 'verified',
        color: 'text-primary',
        bg: 'bg-primary-container/10',
        title: 'Expert Content',
        text: 'Questions curated by certified project managers with decades of experience. Each question includes deep explanations and PMBOK® Guide cross-references.',
    },
    {
        icon: 'timer',
        color: 'text-secondary',
        bg: 'bg-secondary-container/10',
        title: 'Timed Mode',
        text: 'Build your mental stamina with full-length simulations that mirror the actual exam interface, constraints, and pressure environments.',
    },
    {
        icon: 'query_stats',
        color: 'text-tertiary-fixed',
        bg: 'bg-tertiary-container/10',
        title: 'Performance Analytics',
        text: 'Detailed breakdown of your strengths and weaknesses across the People, Process, and Business Environment domains.',
    },
];

export default function LandingPage() {
    const navigate = useNavigate();
    const { isAuthenticated } = useAuth();

    const handleGetStarted = () => {
        if (isAuthenticated) navigate('/exams');
        else navigate('/login', { state: { from: { pathname: '/exams' } } });
    };

    const handleExamsNav = () => {
        if (isAuthenticated) navigate('/exams');
        else navigate('/login', { state: { from: { pathname: '/exams' } } });
    };

    return (
        <div className="bg-background text-on-surface font-body-md selection:bg-secondary-fixed-dim selection:text-on-secondary-container min-h-screen flex flex-col">
            <AppHeader activeLink="Home" onExamsClick={handleExamsNav} />

            <header className="hero-gradient relative overflow-hidden py-32 md:py-48">
                <div className="relative z-10 max-w-container-max mx-auto px-margin-desktop">
                    <div className="max-w-3xl">
                        <div className="inline-flex items-center px-sm py-xs bg-primary-container/50 border border-primary-fixed/20 rounded-lg mb-lg">
                            <span className="text-primary-fixed font-label-sm uppercase tracking-widest px-xs">New 2024 Exam Standards</span>
                        </div>
                        <h1 className="font-headline-xl text-headline-xl md:text-[64px] md:leading-[1.1] text-white mb-lg">
                            Master Your <span className="text-secondary-fixed-dim">PMI Exams</span> with Confidence
                        </h1>
                        <p className="font-body-lg text-body-lg text-primary-fixed-dim/80 mb-xl max-w-2xl">
                            The industry&apos;s most precise PMP, CAPM, and ACP simulation platform. Developed by expert PMPs to mirror the actual exam environment and cognitive rigor.
                        </p>
                        <div className="flex flex-col sm:flex-row gap-md">
                            <button
                                type="button"
                                onClick={handleGetStarted}
                                className="bg-[#FF5F00] text-white font-label-lg px-xl py-4 rounded-lg flex items-center justify-center gap-sm hover:brightness-110 transition-all shadow-xl active:scale-95"
                            >
                                Start Free Practice Exam
                                <Icon name="arrow_forward" />
                            </button>
                            <button
                                type="button"
                                className="border border-primary-fixed/30 text-white font-label-lg px-xl py-4 rounded-lg hover:bg-white/5 transition-all flex items-center justify-center gap-sm"
                            >
                                View Study Plans
                            </button>
                        </div>
                    </div>
                </div>
                <div className="absolute right-[-10%] top-1/2 -translate-y-1/2 w-[500px] h-[500px] border border-primary-fixed/10 rounded-full hidden lg:block pointer-events-none" />
                <div className="absolute right-[-5%] top-1/2 -translate-y-1/2 w-[300px] h-[300px] border border-primary-fixed/5 rounded-full hidden lg:block pointer-events-none" />
            </header>

            <section className="bg-surface-container-lowest -mt-12 relative z-20 max-w-5xl mx-auto rounded-xl shadow-xl border border-outline-variant flex flex-col md:flex-row divide-y md:divide-y-0 md:divide-x divide-outline-variant overflow-hidden mx-margin-mobile md:mx-auto">
                {[
                    { num: '1500+', lbl: 'Realistic Questions' },
                    { num: '100%', lbl: 'Free To Start' },
                    { num: '50k+', lbl: 'Active Users' },
                ].map((s) => (
                    <div key={s.lbl} className="flex-1 px-xl py-lg text-center bento-card-hover">
                        <h3 className={`font-headline-xl text-headline-xl ${s.lbl === 'Realistic Questions' ? 'text-secondary' : 'text-primary'}`}>{s.num}</h3>
                        <p className="font-label-lg text-on-surface-variant uppercase tracking-wider">{s.lbl}</p>
                    </div>
                ))}
            </section>

            <section className="py-32 bg-[#F4F5F7]" id="features">
                <div className="max-w-container-max mx-auto px-margin-desktop">
                    <div className="text-center mb-24">
                        <h2 className="font-headline-lg text-headline-lg md:text-[40px] text-primary mb-md">Engineered for Success</h2>
                        <p className="text-on-surface-variant max-w-2xl mx-auto">
                            Our simulation technology focuses on the three pillars of exam performance: content accuracy, endurance, and analytical feedback.
                        </p>
                    </div>
                    <div className="grid grid-cols-1 md:grid-cols-3 gap-lg">
                        {FEATURES.map((f) => (
                            <div key={f.title} className="bg-white p-xl rounded-xl border border-outline-variant bento-card-hover shadow-sm">
                                <div className={`w-12 h-12 ${f.bg} rounded flex items-center justify-center mb-xl`}>
                                    <Icon name={f.icon} className={f.color} style={{ fontSize: 32 }} />
                                </div>
                                <h3 className="font-headline-md text-headline-md text-primary mb-md">{f.title}</h3>
                                <p className="text-on-surface-variant font-body-md leading-relaxed">{f.text}</p>
                            </div>
                        ))}
                    </div>
                </div>
            </section>

            <section className="py-32 bg-white">
                <div className="max-w-container-max mx-auto px-margin-desktop">
                    <div className="grid grid-cols-1 lg:grid-cols-12 gap-lg h-auto lg:h-[600px]">
                        <div className="lg:col-span-8 bg-primary overflow-hidden rounded-xl relative group">
                            <img
                                className="w-full h-full object-cover opacity-70 group-hover:scale-105 transition-transform duration-700 min-h-[300px]"
                                alt="Professional workspace with exam simulation software"
                                src="https://lh3.googleusercontent.com/aida-public/AB6AXuBoO4RNobq8W9FSbTcnpIabQ-I-K8jaDuUW3_MnlEHW10y9rQ6Pqv34W5ioP3FXI4AZ8kjmRx7pNjDgvqvlktATOrOdZrCBdlCaJwsasKi4D8jofbUymgullj2LPsG1rPzJ1UIYz0t22F5o5utNpV9MXaycDIw8Jsb_05wq-dLlFERkqk20wZg2nLtXKPzqvU-OMTzZLZuFZtyv7LnbstXWQMp6nW_5qRzOKeb4fZNL1sj7ClUzVvJMiomnQ3a-y3b0aweMeE53Blg"
                            />
                            <div className="absolute inset-0 bg-gradient-to-t from-primary/90 to-transparent p-xl flex flex-col justify-end">
                                <h4 className="text-white font-headline-lg text-headline-lg mb-sm">The Real Exam Experience</h4>
                                <p className="text-primary-fixed-dim/80 max-w-lg font-body-md">
                                    Practice in an environment that looks, feels, and acts exactly like the Pearson VUE testing platform.
                                </p>
                            </div>
                        </div>
                        <div className="lg:col-span-4 grid grid-rows-2 gap-lg">
                            <div className="bg-surface-container p-lg rounded-xl flex flex-col justify-between border border-outline-variant">
                                <div>
                                    <Icon name="workspace_premium" className="text-secondary mb-sm" filled />
                                    <h4 className="font-headline-md text-headline-md text-primary mb-xs">Certified Feedback</h4>
                                    <p className="text-on-surface-variant text-body-md">Personalized insights based on your simulation results to target your study time.</p>
                                </div>
                                <span className="text-secondary font-label-lg flex items-center gap-xs">
                                    Learn More <Icon name="chevron_right" />
                                </span>
                            </div>
                            <div className="bg-primary-container text-white p-lg rounded-xl flex flex-col justify-between overflow-hidden relative">
                                <div className="relative z-10">
                                    <h4 className="font-headline-md text-headline-md mb-xs">Flashcards API</h4>
                                    <p className="text-primary-fixed-dim text-body-md">Access 2,000+ terms and definitions on the go with our adaptive mobile app.</p>
                                </div>
                                <button type="button" className="relative z-10 w-fit text-white font-label-lg border-b border-white hover:text-secondary-fixed-dim transition-colors">
                                    Download App
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <section className="py-24 bg-[#F4F5F7] border-y border-outline-variant" id="about">
                <div className="max-w-container-max mx-auto px-margin-desktop text-center">
                    <div className="flex justify-center mb-lg gap-xs">
                        {Array.from({ length: 5 }).map((_, i) => (
                            <Icon key={i} name="star" className="text-secondary" filled />
                        ))}
                    </div>
                    <blockquote className="font-headline-lg text-headline-lg text-primary italic max-w-4xl mx-auto mb-xl">
                        &quot;The simulation accuracy was incredible. I recognized multiple logic patterns in the actual PMP exam that I had mastered through this simulator. It&apos;s the ultimate prep tool.&quot;
                    </blockquote>
                    <div className="flex items-center justify-center gap-md">
                        <div className="w-12 h-12 rounded-full overflow-hidden border-2 border-white shadow-sm">
                            <img
                                className="w-full h-full object-cover"
                                alt="Sarah Jenkins"
                                src="https://lh3.googleusercontent.com/aida-public/AB6AXuACe-1xkMdBw9bw5g-yCIKmwGi91u_maEvG15hCwktRyIVckl5vYlDoMSmOuZHqKdm2yX-cQLkKC2PktRtYqjNgy97DDBQCqzbVEALum7DmxJN6ygLrKJHZ8tgrILfq0Ih1_t1IyjH-6N5AG-g8zl11ZOuMmqljIOsFkce6iqGKyCDKMGSqGP8aHO4I2IMJjtGKiKCHdefhtPGBgoyExpEPTueGoZGKylJ92IgGDMNzDHQpCpTHQoBJb_qa4v_1Si8FiWBARf5RNyU"
                            />
                        </div>
                        <div className="text-left">
                            <p className="font-label-lg text-primary">Sarah Jenkins, PMP</p>
                            <p className="text-label-sm text-on-surface-variant">Senior Project Lead at TechCorp</p>
                        </div>
                    </div>
                </div>
            </section>

            <AppFooter />
        </div>
    );
}
