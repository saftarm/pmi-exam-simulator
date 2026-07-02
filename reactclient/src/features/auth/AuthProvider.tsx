import { useCallback, useEffect, useState, type ReactNode } from 'react';
import * as authApi from './api';
import { AuthContext } from './AuthContext';
import type { AuthContextValue, AuthUser, RegisterUserRequest } from './types';

interface AuthProviderProps {
  children: ReactNode;
}

export function AuthProvider({ children }: AuthProviderProps) {
  const [user, setUser] = useState<AuthUser | null>(() => authApi.getStoredUser());
  const [profileLoading, setProfileLoading] = useState(false);
  const [authReady, setAuthReady] = useState(() => !authApi.isAuthenticated());

  const refreshUser = useCallback(async () => {
    if (!authApi.isAuthenticated()) {
      setUser(null);
      return null;
    }
    setProfileLoading(true);
    try {
      const profile = await authApi.fetchCurrentUser();
      setUser(profile);
      return profile;
    } catch {
      const stored = authApi.getStoredUser();
      setUser(stored);
      return stored;
    } finally {
      setProfileLoading(false);
    }
  }, []);

  useEffect(() => {
    let cancelled = false;

    if (!authApi.isAuthenticated()) {
      setUser(null);
      setAuthReady(true);
      return undefined;
    }

    refreshUser().finally(() => {
      if (!cancelled) setAuthReady(true);
    });

    return () => {
      cancelled = true;
    };
  }, [refreshUser]);

  const login = useCallback(async (userName: string, password: string) => {
    const loggedInUser = await authApi.login(userName, password);
    setUser(loggedInUser);
    return loggedInUser;
  }, []);

  const register = useCallback(async (payload: RegisterUserRequest) => {
    await authApi.register(payload);
  }, []);

  const logout = useCallback(() => {
    authApi.logout();
    setUser(null);
  }, []);

  const isAdmin = user?.role === 'Admin';

  const value: AuthContextValue = {
    user,
    isAuthenticated: !!user && authApi.isAuthenticated(),
    isAdmin,
    profileLoading,
    authReady,
    refreshUser,
    login,
    register,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
