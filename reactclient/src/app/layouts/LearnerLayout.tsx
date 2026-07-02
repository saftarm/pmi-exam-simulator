import { Outlet, useLocation } from 'react-router-dom';
import AppHeader from '../../features/auth/components/AppHeader';
import AppFooter from '../../shared/components/AppFooter';

function resolveActiveLink(pathname: string) {
  if (pathname.startsWith('/exams')) return 'Exams';
  if (pathname === '/profile') return 'Profile';
  if (pathname === '/about') return 'About';
  return 'Home';
}

type LearnerPageShellProps = {
  children: React.ReactNode;
  activeLink?: string;
  mainClassName?: string;
};

export function LearnerPageShell({
  children,
  activeLink = 'Home',
  mainClassName = '',
}: LearnerPageShellProps) {
  return (
    <div className="min-h-screen flex flex-col bg-[#F4F5F7]">
      <AppHeader activeLink={activeLink} />
      <main className={`flex-1 w-full ${mainClassName}`}>{children}</main>
      <AppFooter />
    </div>
  );
}

export default function LearnerLayout() {
  const { pathname } = useLocation();
  const activeLink = resolveActiveLink(pathname);
  const isAbout = pathname === '/about';
  const mainClassName = isAbout
    ? ''
    : 'max-w-container-max mx-auto px-margin-desktop py-xl';

  return (
    <LearnerPageShell activeLink={activeLink} mainClassName={mainClassName}>
      <Outlet />
    </LearnerPageShell>
  );
}
