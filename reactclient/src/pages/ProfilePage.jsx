import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { fetchCurrentUser } from '../services/authService';
import AppHeader from '../components/AppHeader';
import AppFooter from '../components/AppFooter';
import StatusBadge from '../components/admin/StatusBadge';
import { formatDate, getInitials } from '../utils/userDisplay';
import { ProfileSkeleton, ContentReveal } from '../components/loading';

export default function ProfilePage() {
    const [profile, setProfile] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        fetchCurrentUser()
            .then(setProfile)
            .catch(() => setError('Failed to load profile.'))
            .finally(() => setLoading(false));
    }, []);

    return (
        <div className="min-h-screen flex flex-col bg-[#F4F5F7]">
            <AppHeader activeLink="Profile" />

            <main className="flex-1 max-w-lg mx-auto px-margin-desktop py-xl w-full">
                <h1 className="font-headline-xl text-headline-xl text-primary mb-xl">Your Profile</h1>

                {loading && <ProfileSkeleton />}
                {error && <p className="text-red-600 loading-enter">{error}</p>}

                <ContentReveal show={!!profile}>
                    <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-xl">
                        <div className="flex items-center gap-lg mb-xl">
                            <div className="w-16 h-16 rounded-full bg-primary/10 flex items-center justify-center text-xl font-bold text-primary">
                                {getInitials(profile.displayName || profile.userName)}
                            </div>
                            <div>
                                <h2 className="font-headline-md text-headline-md font-bold">{profile.displayName}</h2>
                                <p className="text-on-surface-variant">@{profile.userName}</p>
                            </div>
                        </div>

                        <dl className="space-y-md text-sm">
                            <div className="flex justify-between border-b border-outline-variant pb-md">
                                <dt className="text-on-surface-variant">Email</dt>
                                <dd className="font-medium">{profile.email}</dd>
                            </div>
                            <div className="flex justify-between border-b border-outline-variant pb-md">
                                <dt className="text-on-surface-variant">First name</dt>
                                <dd className="font-medium">{profile.firstName || '—'}</dd>
                            </div>
                            <div className="flex justify-between border-b border-outline-variant pb-md">
                                <dt className="text-on-surface-variant">Role</dt>
                                <dd className="font-medium">{profile.role}</dd>
                            </div>
                            <div className="flex justify-between border-b border-outline-variant pb-md">
                                <dt className="text-on-surface-variant">Status</dt>
                                <dd>
                                    <StatusBadge
                                        status={profile.status}
                                        type={
                                            profile.status === 'Active'
                                                ? 'success'
                                                : profile.status === 'Suspended'
                                                  ? 'error'
                                                  : 'warning'
                                        }
                                    />
                                </dd>
                            </div>
                            <div className="flex justify-between">
                                <dt className="text-on-surface-variant">Member since</dt>
                                <dd className="font-medium">{formatDate(profile.createdAt)}</dd>
                            </div>
                        </dl>

                        <p className="mt-xl text-xs text-on-surface-variant">
                            Profile is read-only. Contact an administrator to update your account.
                        </p>

                        <Link
                            to="/progress"
                            className="mt-lg inline-block text-secondary-container font-bold text-sm hover:underline"
                        >
                            View your progress →
                        </Link>
                    </div>
                </ContentReveal>
            </main>

            <AppFooter />
        </div>
    );
}
