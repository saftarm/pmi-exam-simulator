/**
 * Default landing route after sign-in.
 * Admins go to the admin panel unless they were heading to a specific admin URL.
 */
export function resolvePostLoginPath(user, fromPathname) {
    const role = user?.role;

    if (role === 'Admin') {
        if (fromPathname?.startsWith('/admin')) {
            return fromPathname;
        }
        return '/admin';
    }

    if (fromPathname && fromPathname !== '/login') {
        return fromPathname;
    }

    return '/exams';
}

/**
 * Default home for an already-authenticated user (CTAs, marketing pages).
 */
export function resolveAuthenticatedHomePath(user) {
    return user?.role === 'Admin' ? '/admin' : '/exams';
}
