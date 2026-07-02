const ADMIN_PREFIX = '/admin';
const LEARNER_DEFAULT = '/exams';
const ADMIN_DEFAULT = '/admin';

/** Shared routes admins may return to after sign-in (not learner-only paths like /exams). */
const ADMIN_SHARED_ROUTES = ['/profile', '/about'];

type UserLike = { role?: string | null } | null | undefined;

export function isAdminRole(role?: string | null) {
  return role === 'Admin';
}

export function isPathAllowedForRole(pathname: string | undefined, role?: string | null) {
  if (!pathname || pathname === '/login') return false;
  if (pathname.startsWith(ADMIN_PREFIX)) return isAdminRole(role);
  return true;
}

function resolveDefaultHomePath(user: UserLike) {
  return isAdminRole(user?.role) ? ADMIN_DEFAULT : LEARNER_DEFAULT;
}

/** Default landing route after sign-in. */
export function resolvePostLoginPath(user: UserLike, fromPathname?: string) {
  const role = user?.role;

  if (isAdminRole(role)) {
    if (fromPathname?.startsWith(ADMIN_PREFIX)) {
      return fromPathname;
    }
    if (fromPathname && ADMIN_SHARED_ROUTES.includes(fromPathname)) {
      return fromPathname;
    }
    return ADMIN_DEFAULT;
  }

  if (isPathAllowedForRole(fromPathname, role) && fromPathname) {
    return fromPathname;
  }
  return LEARNER_DEFAULT;
}

/** Default home for an already-authenticated user (CTAs, marketing pages). */
export function resolveAuthenticatedHomePath(user: UserLike) {
  return resolveDefaultHomePath(user);
}
