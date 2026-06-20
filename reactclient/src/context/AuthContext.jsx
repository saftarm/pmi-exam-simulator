import { createContext, useContext, useState, useCallback, useEffect } from 'react';
import * as authService from '../services/authService';

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
    const [user, setUser] = useState(() => authService.getStoredUser());
    const [profileLoading, setProfileLoading] = useState(false);

    const refreshUser = useCallback(async () => {
        if (!authService.isAuthenticated()) {
            setUser(null);
            return null;
        }
        setProfileLoading(true);
        try {
            const profile = await authService.fetchCurrentUser();
            setUser(profile);
            return profile;
        } catch {
            const stored = authService.getStoredUser();
            setUser(stored);
            return stored;
        } finally {
            setProfileLoading(false);
        }
    }, []);

    useEffect(() => {
        if (authService.isAuthenticated() && !user?.role) {
            refreshUser();
        }
    }, [refreshUser, user?.role]);

    const login = useCallback(async (userName, password) => {
        const loggedInUser = await authService.login(userName, password);
        setUser(loggedInUser);
        return loggedInUser;
    }, []);

    const register = useCallback(async (payload) => {
        await authService.register(payload);
    }, []);

    const logout = useCallback(() => {
        authService.logout();
        setUser(null);
    }, []);

    const isAdmin = user?.role === 'Admin';

    return (
        <AuthContext.Provider
            value={{
                user,
                isAuthenticated: !!user && authService.isAuthenticated(),
                isAdmin,
                profileLoading,
                refreshUser,
                login,
                register,
                logout,
            }}
        >
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    const ctx = useContext(AuthContext);
    if (!ctx) throw new Error('useAuth must be used within AuthProvider');
    return ctx;
}
