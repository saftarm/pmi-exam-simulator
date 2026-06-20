import { Link } from 'react-router-dom';

const FOOTER_LINKS = ['Privacy Policy', 'Terms of Service', 'Cookie Policy', 'Support'];

export default function AppFooter({ compact = false }) {
    if (compact) {
        return (
            <footer className="bg-primary dark:bg-primary-container h-12 flex items-center shrink-0 z-50">
                <div className="flex w-full justify-between items-center px-margin-desktop max-w-container-max mx-auto">
                    <span className="text-primary-fixed-dim opacity-80 font-label-lg text-label-lg">
                        © 2024 PMI Exam Simulator. Professional PMP Certification Prep.
                    </span>
                    <div className="flex gap-lg">
                        <a className="text-primary-fixed-dim opacity-80 hover:opacity-100 font-label-lg text-label-lg hover:underline transition-opacity" href="#support">Support</a>
                        <a className="text-primary-fixed-dim opacity-80 hover:opacity-100 font-label-lg text-label-lg hover:underline transition-opacity" href="#terms">Terms</a>
                    </div>
                </div>
            </footer>
        );
    }

    return (
        <footer className="bg-primary dark:bg-primary-container w-full py-xl mt-auto">
            <div className="flex flex-col md:flex-row justify-between items-center px-margin-desktop max-w-container-max mx-auto gap-lg">
                <div className="flex flex-col items-center md:items-start gap-sm">
                    <span className="font-headline-md text-headline-md text-on-primary">PMI Exam Simulator</span>
                    <p className="font-label-lg text-label-lg text-primary-fixed-dim opacity-80 text-center md:text-left">
                        © 2024 PMI Exam Simulator. All rights reserved. Professional PMP Certification Prep.
                    </p>
                </div>
                <div className="flex flex-wrap justify-center gap-lg">
                    {FOOTER_LINKS.map((label) => (
                        <a
                            key={label}
                            href={`#${label.toLowerCase().replace(/\s+/g, '-')}`}
                            className="font-label-lg text-label-lg text-primary-fixed-dim opacity-80 hover:opacity-100 hover:underline transition-opacity"
                        >
                            {label}
                        </a>
                    ))}
                </div>
            </div>
        </footer>
    );
}

export function AuthFooter() {
    return (
        <footer className="bg-primary dark:bg-primary-container w-full py-xl mt-auto">
            <div className="flex flex-col md:flex-row justify-between items-center px-margin-desktop max-w-container-max mx-auto gap-lg">
                <div className="font-headline-md text-headline-md text-on-primary">
                    PMI Exam Simulator
                </div>
                <div className="flex flex-wrap justify-center gap-md">
                    {FOOTER_LINKS.map((label) => (
                        <a
                            key={label}
                            href={`#${label.toLowerCase().replace(/\s+/g, '-')}`}
                            className="font-label-lg text-label-lg text-primary-fixed-dim opacity-80 hover:opacity-100 hover:underline transition-opacity"
                        >
                            {label}
                        </a>
                    ))}
                </div>
                <p className="font-label-lg text-label-lg text-on-primary opacity-60 text-center md:text-right">
                    © 2024 PMI Exam Simulator. All rights reserved. Professional PMP Certification Prep.
                </p>
            </div>
        </footer>
    );
}
